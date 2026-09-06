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

//----------------------------------------------------------------
/**
**
**/

public  double
getColWidth(int c)
{
    return ( (this.ColumnWidths != null && c < this.ColumnWidths.Count)
            ? this.ColumnWidths[c] : DefaultCellWidth );
}

//----------------------------------------------------------------
/**
**
**/

public  double
getRowHeight(int r)
{
    return ( (this.RowHeights != null && r < this.RowHeights.Count)
            ? this.RowHeights[r] : this.DefaultCellHeight );
}


//========================================================================
//
//    Properties.
//

public  double  DefaultCellWidth  { get; set; } = 60.0;
public  double  DefaultCellHeight { get; set; } = 25.0;

public  override  double  ExtentWidth  {
    get { return  this.m_totalWidth; }
}

public  override  double  ExtentHeight  {
    get { return  this.m_totalHeight; }
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


public  override  double  SmallChangeX => DefaultCellWidth;

public  override  double  SmallChangeY => DefaultCellHeight;


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
/**
**
**/

protected  override  System.Windows.Size
MeasureOverride(
        System.Windows.Size     availableSize)
{
    //  列数や幅データが変わっていたら位置キャッシュを更新  //
    if ( this.m_colPos.Count != Columns ) {
        updateColumnPositions();
    }
    if ( this.m_rowPos.Count != Rows ) {
        updateRowPositions();
    }

    return  base.MeasureOverride(availableSize);
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
    int startCol = getColumnIndexAtX(HorizontalOffset);
    int startRow = getRowIndexAtY(VerticalOffset);
    int endCol  = getColumnIndexAtX(HorizontalOffset + ViewportWidth);
    int endRow  = getRowIndexAtY(VerticalOffset + ViewportHeight);

    Typeface typeface = new Typeface(
            SystemFonts.CaptionFontFamily,
            FontStyles.Normal,  FontWeights.Normal, FontStretches.Normal);
    double fontSize = 12;
    Pen gridPen = new Pen(Brushes.LightGray, 0.5);

    for ( int r = startRow; r <= endRow; ++ r ) {
        double  absoluteY = this.m_rowPos[r];
        double  y = absoluteY - VerticalOffset;
        double  rH  = getRowHeight(r);

        for ( int c = startCol; c <= endCol; ++ c ) {
            int index = r * Columns + c;
            if ( index >= MatrixData.Length ) { continue; }

            MatrixCellData  dat = MatrixData[index];
            System.String   val = dat.Value;

            //  セルの左上座標  //
            double  absoluteX = this.m_colPos[c];
            double  x = absoluteX - HorizontalOffset;
            double  cW  = getColWidth(c);

            Brush   bgBrush = dat.Background ?? Brushes.White;
            Brush   fgBrush = dat.Foreground ?? Brushes.Black;

            dc.DrawRectangle(
                     bgBrush, gridPen, new Rect(x, y, cW, rH));
            FormattedText formattedText = new FormattedText(
                    val,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    fontSize,
                    fgBrush,
                    VisualTreeHelper.GetDpi(this).PixelsPerDip);
            //  中央揃えの座標計算。    //
            double textX = x + (cW - formattedText.Width ) / 2;
            double textY = y + (rH - formattedText.Height) / 2;
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

//----------------------------------------------------------------
/**
**
**/

private  static  int
getIndexFromCache(
        List<double>    posCache,
        double          val,
        int             num)
{
    if ( posCache.Count == 0 ) { return 0; }
    int index = posCache.BinarySearch(val);
    if ( index < 0 ) {
        //  ぴったり一致しない場合は手前の要素を取る。  //
        index = ~index - 1;
    }
    return  Math.Max(0, Math.Min(index, num - 1));
}

private  int
getColumnIndexAtX(double  x)
{
    return  getIndexFromCache(this.m_colPos, x, this.Columns);
}

private  int
getRowIndexAtY(double  y)
{
    return  getIndexFromCache(this.m_rowPos, y, this.Rows);
}


private  static  void
OnColumnWidthsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    ((MatrixDisplay)d).updateColumnPositions();
}

private  static  void
OnRowHeightsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    ((MatrixDisplay)d).updateRowPositions();
}


//----------------------------------------------------------------
/**
**
**/

private  void
updateColumnPositions()
{
    this.m_colPos.Clear();
    int     numCols = this.Columns;
    double  current = 0;

    for ( int c = 0; c < numCols; ++ c ) {
        this.m_colPos.Add(current);
        double  w = getColWidth(c);
        current += w;
    }
    this.m_totalWidth   = current;
}

private  void
updateRowPositions()
{
    this.m_rowPos.Clear();
    int     numRows = this.Rows;
    double  current = 0;

    for ( int r = 0; r < numRows; ++ r ) {
        this.m_rowPos.Add(current);
        double  h = getRowHeight(r);
        current += h;
    }
    this.m_totalHeight  = current;
}


//========================================================================
//
//    Member Variables.
//

private   List<double>   m_colPos = new List<double>();
private   List<double>   m_rowPos = new List<double>();

private   double         m_totalWidth;
private   double         m_totalHeight;


}   //  End class  MatrixDisplay

}   //  End of namespace  WpfControl.Editor
