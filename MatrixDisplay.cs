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
using System.Windows.Controls;
using System.Windows.Media;


namespace  WpfControl.Editor  {

//========================================================================
//
//    MatrixDisplay  class
//

public  class  MatrixDisplay : WpfControl.Common.ScrollFrameworkElementBase
{

//========================================================================
//
//    Constructor(s) and Destructor.
//


//========================================================================
//
//    Accessors.
//

//========================================================================
//
//    Properties.
//

public  double  CellWidth  { get; set; } = 60.0;
public  double  CellHeight { get; set; } = 25.0;

public  override  double  ExtentWidth  {
    get { return  this.Columns * this.CellWidth; }
}

public  override  double  ExtentHeight  {
    get { return  this.Rows * this.CellHeight; }
}

public  int  Columns  {
    get { return  (int)GetValue(ColumnsProperty); }
    set { SetValue(ColumnsProperty, value); }
}

public  IList<double>  ColumnWidths  {
    get { return  (IList<double>)GetValue(ColumnWidthsProperty); }
    set { SetValue(ColumnWidthsProperty, value); }
}

public  MatrixCellData[]?  MatrixData  {
    get { return  (MatrixCellData[]?)GetValue(MatrixDataProperty); }
    set { SetValue(MatrixDataProperty, value); }
}


public  override  double  SmallChangeX => CellWidth;

public  override  double  SmallChangeY => CellHeight;


public  IList<double>  RowHeights  {
    get { return  (IList<double>)GetValue(RowHeightsProperty); }
    set { SetValue(RowHeightsProperty, value); }
}

public  int  Rows  {
    get { return  (int)GetValue(RowsProperty); }
    set { SetValue(RowsProperty, value); }
}

//========================================================================
//
//    Dependency Properties.
//

private  const  FrameworkPropertyMetadataOptions
AFFECTS_LAYOUT =
        FrameworkPropertyMetadataOptions.AffectsMeasure |
        FrameworkPropertyMetadataOptions.AffectsRender;


public  static  readonly  DependencyProperty  ColumnsProperty =
DependencyProperty.Register(
        nameof(Columns), typeof(int), typeof(MatrixDisplay),
        new FrameworkPropertyMetadata(0, AFFECTS_LAYOUT)
);

public  static  readonly  DependencyProperty  ColumnWidthsProperty =
DependencyProperty.Register(
        nameof(ColumnWidths), typeof(IList<double>), typeof(MatrixDisplay),
        new FrameworkPropertyMetadata(
                null, AFFECTS_LAYOUT, OnColumnWidthsChanged)
);

public  static  readonly  DependencyProperty  MatrixDataProperty =
DependencyProperty.Register(
        nameof(MatrixData), typeof(MatrixCellData[]), typeof(MatrixDisplay),
        new FrameworkPropertyMetadata(null, AFFECTS_LAYOUT)
);

public  static  readonly  DependencyProperty  RowHeightsProperty =
DependencyProperty.Register(
        nameof(RowHeights), typeof(IList<double>), typeof(MatrixDisplay),
        new FrameworkPropertyMetadata(
                null, AFFECTS_LAYOUT, OnRowHeightsChanged)
);

public  static  readonly  DependencyProperty  RowsProperty =
DependencyProperty.Register(
        nameof(Rows), typeof(int), typeof(MatrixDisplay),
        new FrameworkPropertyMetadata(0, AFFECTS_LAYOUT)
);


//========================================================================
//
//    Protected Member Functions (Overrides).
//


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
    int startCol = Math.Max(0, (int)(HorizontalOffset / CellWidth));
    int startRow = Math.Max(0, (int)(VerticalOffset / CellHeight));
    int endCol = Math.Min(Columns - 1,
            (int)((HorizontalOffset + ViewportWidth) / CellWidth) + 1);
    int endRow = Math.Min(Rows - 1,
            (int)((VerticalOffset + ViewportHeight) / CellHeight) + 1);

    Typeface typeface = new Typeface(
            SystemFonts.CaptionFontFamily,
            FontStyles.Normal,  FontWeights.Normal, FontStretches.Normal);
    double fontSize = 12;
    Pen gridPen = new Pen(Brushes.LightGray, 0.5);

    for ( int r = startRow; r <= endRow; ++ r ) {
        for ( int c = startCol; c <= endCol; ++ c ) {
            int index = r * Columns + c;
            if ( index >= MatrixData.Length ) { continue; }
            MatrixCellData  dat = MatrixData[index];
            System.String   val = dat.Value;

            //  セルの左上座標  //
            double x = (c * CellWidth) - HorizontalOffset;
            double y = (r * CellHeight) - VerticalOffset;

            Brush   bgBrush = dat.Background ?? Brushes.White;
            Brush   fgBrush = dat.Foreground ?? Brushes.Black;

            dc.DrawRectangle(
                     bgBrush, gridPen, new Rect(x, y, CellWidth, CellHeight));
            FormattedText formattedText = new FormattedText(
                    val,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    fontSize,
                    fgBrush,
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

private  static  void
OnColumnWidthsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
}

private  static  void
OnRowHeightsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
}


//========================================================================
//
//    Member Variables.
//


}   //  End class  MatrixDisplay

}   //  End of namespace  WpfControl.Editor
