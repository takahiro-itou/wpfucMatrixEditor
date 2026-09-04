//  -*-  coding: utf-8-with-signature  -*-  //
/*************************************************************************
**                                                                      **
**                  ---  WPF UserControl Library.  ---                  **
**                                                                      **
**          Copyright (C), 2026-2026, Takahiro Itou                     **
**          All Rights Reserved.                                        **
**                                                                      **
**          License: (See COPYING or LICENSE files)                     **
**          GNU Affero General Public License (AGPL) version 3,         **
**          or (at your option) any later version.                      **
**                                                                      **
*************************************************************************/

using System.Globalization;
using System.Windows;
using System.Windows.Media;


namespace  WpfControl.Editor  {

//========================================================================
//
//    MatrixDisplay  class
//

public  class  MatrixDisplay : FrameworkElement
{

//========================================================================
//
//    Constructor(s) and Destructor.
//


//========================================================================
//
//    Accessors.
//


public  void
setHorizontalOffset(double  offset)
{
    double value = Math.Max(0,
        Math.Min(offset, this.ExtentWidth - this.ViewportWidth));
    if ( this.m_offset.X != value ) {
        this.m_offset.X = value; }
    }
}

public  void
setVerticalOffset(
        double  offset)
{
    double value = Math.Max(0,
        Math.Min(offset, this.ExtentHeight - this.ViewportHeight));
    if ( this.m_offset.Y != value ) {
        this.m_offset.Y = value; }
    }
}

//========================================================================
//
//    Properties.
//

public  static  readonly  DependencyProperty
ColumnsProperty = DependencyProperty.Register(
    nameof(Columns), typeof(int), typeof(MatrixDisplay),
    new FrameworkPropertyMetadata(
            0, FrameworkPropertyMetadataOptions.AffectsRender)
);

public  static  readonly  DependencyProperty
MatrixDataProperty = DependencyProperty.Register(
    nameof(MatrixData), typeof(int[]), typeof(MatrixDisplay),
    new FrameworkPropertyMetadata(
            null, FrameworkPropertyMetadataOptions.AffectsRender)
);

public  static  readonly  DependencyProperty
RowsProperty = DependencyProperty.Register(
    nameof(Rows), typeof(int), typeof(MatrixDisplay),
    new FrameworkPropertyMetadata(
            0, FrameworkPropertyMetadataOptions.AffectsRender)
);


public  bool    CanHorizontallyScrol { get; set; }
public  bool    CanVerticallyScroll  { get; set; }

public  double  CellWidth  { get; set; } = 60.0;
public  double  CellHeight { get; set; } = 25.0;

public  double  ExtentWidth  {
    get { return  this.Columns * this.CellWidth; }
}

public  double  ExtentHeight  {
    get { return  this.Rows * this.CellHeight;
}

public  int  Columns  {
    get { return  (int)GetValue(ColumnsProperty); }
    set { SetValue(ColumnsProperty, value); }
}

public  double  HorizontalOffet  {
    get { return  this.m_offset.X; }
}

public  int[]  MatrixData  {
    get { return  (int[])GetValue(MatrixDataProperty); }
    set { SetValue(MatrixDataProperty, value); }
}

public  ScrollViewer  ScrollOwner  {
    get { return  this.m_scrollOwner; }
    set { this.m_scrollOwner = value; }
}

public  int  Rows  {
    get { return  (int)GetValue(RowsProperty); }
    set { SetValue(RowsProperty, value); }
}

public  double  VerticalOffset  {
    get { return  this.m_offset.Y; }
}

public  double  ViewportHeight  {
    get { return  this.m_viewport.Height; }
}

public  double  ViewportWidth  {
    get { return  this.m_viewport.Width; }
}


//========================================================================
//
//    Protected Member Functions (Overrides).
//

//----------------------------------------------------------------
/**
**
**/

protected  override  Size
MeasureOverride(
        Size availableSize)
{
    if ( this.m_viewport != availableSize ) {
        this.m_viewport = availableSize;
        this.m_scrollOwner?.InvalidateScrollInfo();
    }
    return  availableSize;
}

//----------------------------------------------------------------
/**   描画ロジック。
**
**  @param [in] dc    Drawing Context
**/

protected  override  void
OnRender(System.Windows.Media.DrawingContext  dc)
{
    base.OnRender(dc);

    if ( this.MatrixData == null || Rows <= 0 || Columns <= 0) {
        return;
    }

    //  描画領域を ScrollViewer 内にクリップする。  //
    dc.PushClip(new RectangleGeometry(
        new Rect(0, 0, ViewportWidth, ViewportHeight)
    ));

    //  背景塗りつぶし  //
    dc.DrawRectangle(
            Brushes.White, null,
            new Rect(0, 0, this.ViewportWidth, this.ViewportHeight));

    //  表示範囲を計算。    /
    int startCol = Math.Max(0, (int)HorizontalOffset / CellWidth);
    int startRow = Math.Max(0, (int)VerticalOffset / CellHeight);
    int endCol = Math.Min(Columns - 1,
            (int)((HorizontalOffet + ViewportWidth) / CellWidth) + 1);
    int endRow = Math.Min(Rows - 1,
            (int)((VerticalOffet + ViewportHeight) / CellHeight) + 1);

    Typeface typeface = new Typeface(
            SystemFonts.CaptionFontFamily,
            FontStyles.Normal,  FontWeights.Normal, FontStretches.Normal);
    double fontSize = 12;
    Pen gridPen = new Pen(Brushes.LightGray, 0.5);

    for ( int r = startRow; r <= endRows; ++ r ) {
        for ( int c = startCol; c <= endCol ++ c ) {
            int index = r * Columns + c;
            if ( index >= MatrixData.Length ) { continue; }
            double val = MatrixData[index];

            //  セルの左上座標  //
            double x = (c * CellWidth) - HorizontalOffet;
            double y = (r * CellHeight) - VerticalOffset;

            dc.DrawRectangle(
                     null, gridPen, new Rect(x, y, CellWidth, CellHeight));
            FormattedText formattedText = new FormattedText(
                    val.ToString("F2", CultureInfo.CurrentCulture),
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    fontSize,
                    Brushes.Black,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);
            //  中央揃えの座標計算。    //
            double textX = x + (CellWidth - formattedText.Width) / 2;
            double textY = y + (CellHeight - formattedText.Height) / 2;
            dc.DrawText(formattedText, new Point(textX, textY));
        }
    }

    dc.Pop();   //  クリップの解除  //
    return;
}


//========================================================================
//
//    For Internal Use Only.
//

private  void
invalidated()
{
    this.m_scrollOwner?.InvalidateScrollInfo();
    this.InvalidateVisual();
}


//========================================================================
//
//    Member Variables.
//

private   Size              m_viewport  = new Size(0, 0);
private   Point             m_offset    = new Point(0, 0);
private   ScrollViewer?     m_scrollOwner;


}   //  End class  MatrixDisplay

}   //  End of namespace  WpfControl.Editor
