//  -*-  coding: utf-8-with-signature-unix     -*-  //
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

using   System.Globalization;
using   System.Windows;
using   System.Windows.Controls;
using   System.Windows.Media;

using   WpfHelper.Controls;


namespace  WpfControl.Editor  {

//========================================================================
//
//    MatrixDisplay  class
//

public  class  MatrixDisplay : ScrollFrameworkElementBase
{

//========================================================================
//
//    Constructor(s) and Destructor.
//

//----------------------------------------------------------------
/**   コンストラクタ。
**
**/

public  MatrixDisplay()
{
    //  デバイスの物理ピクセルに配置を強制する  //
    this.SnapsToDevicePixels  = true;
    this.UseLayoutRounding    = true;

    this.m_flagColCacheEnabled  = false;
    this.m_flagRowCacheEnabled  = false;
}


//========================================================================
//
//    Public Consts.
//

public  static  readonly  Brush     DEFAULT_BACKGROUND  =
new  SolidColorBrush(Color.FromRgb(240, 240, 240));

public  static  readonly  Brush     DEFAULT_BORDER_LINE =
new  SolidColorBrush(Color.FromRgb(104, 140, 175));


//========================================================================
//
//    Accessors.
//

//----------------------------------------------------------------
/**
**
**/
public  double
GetColWidth(int c)
{
    IList<double>   colSize = this.Layouts.ColumnWidths;
    return ( (colSize != null && c < colSize.Count)
            ? colSize[c] : this.Layouts.DefaultCellWidth );
}

//----------------------------------------------------------------
/**
**
**/

public  double
GetRowHeight(int r)
{
    IList<double>   rowSize = this.Layouts.RowHeights;
    return ( (rowSize != null && r < rowSize.Count)
            ? rowSize[r] : this.Layouts.DefaultCellHeight );
}


//========================================================================
//
//    Properties (Overrides).
//

public  override  double  ExtentWidth
{
    get { return  this.m_totalWidth; }
}

public  override  double  ExtentHeight
{
    get { return  this.m_totalHeight; }
}

public  override  double  SmallChangeX
{
    get { return  this.Layouts.DefaultCellWidth; }
}

public  override  double  SmallChangeY
{
    get { return  this.Layouts.DefaultCellHeight; }
}


//========================================================================
//
//    Properties.
//


public  int  Columns  {
    get { return  (int)GetValue(ColumnsProperty); }
    set { SetValue(ColumnsProperty, value); }
}

//----------------------------------------------------------------
/**   レイアウトに関する設定をまとめたプロパティ。
**
**/
public  MatrixLayout   Layouts
{
    get { return  (MatrixLayout)GetValue(LayoutsProperty); }
    set { SetValue(LayoutsProperty, value); }
}

public  MatrixCellData[]?  MatrixData  {
    get { return  (MatrixCellData[]?)GetValue(MatrixDataProperty); }
    set { SetValue(MatrixDataProperty, value); }
}

//----------------------------------------------------------------
/**   表示等に関する設定をまとめたプロパティ。
**
**/
public  MatrixOption  Options
{
    get { return  (MatrixOption)GetValue(OptionsProperty); }
    set { SetValue(OptionsProperty, value); }
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
META_INHERITS =
FrameworkPropertyMetadataOptions.Inherits;

private  const  FrameworkPropertyMetadataOptions
AFFECTS_RENDER =
        FrameworkPropertyMetadataOptions.AffectsRender;

private  const  FrameworkPropertyMetadataOptions
AFFECTS_LAYOUT =
        FrameworkPropertyMetadataOptions.AffectsMeasure |
        AFFECTS_RENDER;


public  static  readonly  DependencyProperty  ColumnsProperty =
DependencyProperty.RegisterAttached(
        nameof(Columns), typeof(int), typeof(MatrixDisplay),
        new FrameworkPropertyMetadata(0, META_INHERITS | AFFECTS_LAYOUT)
);


public  static  readonly  DependencyProperty  LayoutsProperty =
DependencyProperty.Register(
    nameof(Layouts), typeof(MatrixLayout), typeof(MatrixDisplay),
    new FrameworkPropertyMetadata(
            null, AFFECTS_RENDER, OnLayoutsChanged, CoerceLayouts)
);


public  static  readonly  DependencyProperty  MatrixDataProperty =
DependencyProperty.Register(
    nameof(MatrixData), typeof(MatrixCellData[]), typeof(MatrixDisplay),
    new FrameworkPropertyMetadata(null, AFFECTS_LAYOUT)
);

public  static  readonly  DependencyProperty  OptionsProperty =
DependencyProperty.Register(
    nameof(Options), typeof(MatrixOption), typeof(MatrixDisplay),
    new FrameworkPropertyMetadata(
            null, AFFECTS_RENDER, OnOptionsChanged, CoerceOptions)
);

public  static  readonly  DependencyProperty  RowsProperty =
DependencyProperty.RegisterAttached(
        nameof(Rows), typeof(int), typeof(MatrixDisplay),
        new FrameworkPropertyMetadata(0, META_INHERITS | AFFECTS_LAYOUT)
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
    UpdateColPositions();
    UpdateRowPositions();
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

    //  描画領域を ScrollViewer 内にクリップする。  //
    dc.PushClip(new RectangleGeometry(
        new Rect(0, 0, ViewportWidth, ViewportHeight)
    ));

    //  背景塗りつぶし  //
    Pen penBorder = new Pen(this.Options.BorderLine, 1.0);

    dc.DrawRectangle(
            this.Options.Background,
            penBorder,
            new Rect(
                0.5,  0.5,
                this.ViewportWidth  - 1.0,
                this.ViewportHeight - 1.0)
    );

    if ( this.MatrixData == null || Rows <= 0 || Columns <= 0) {
        dc.Pop();
        return;
    }

    //  表示範囲を計算。    /
    int startCol = GetColIndexAtX(HorizontalOffset);
    int startRow = GetRowIndexAtY(VerticalOffset);
    int endCol  = GetColIndexAtX(HorizontalOffset + ViewportWidth);
    int endRow  = GetRowIndexAtY(VerticalOffset + ViewportHeight);

    Typeface typeface = new Typeface(
            SystemFonts.CaptionFontFamily,
            FontStyles.Normal,  FontWeights.Normal, FontStretches.Normal);
    double fontSize = 12;
    Pen gridPen = new Pen(this.Options.GridLine, 0.5);

    for ( int r = startRow; r <= endRow; ++ r ) {
        double  absoluteY = this.m_rowPos[r];
        double  y = absoluteY - VerticalOffset + 1.0;
        double  rH  = GetRowHeight(r);

        for ( int c = startCol; c <= endCol; ++ c ) {
            int index = r * Columns + c;
            if ( index >= MatrixData.Length ) { continue; }

            MatrixCellData  dat = MatrixData[index];
            System.String   val = dat.Value ?? "";

            //  セルの左上座標  //
            double  absoluteX = this.m_colPos[c];
            double  x = absoluteX - HorizontalOffset + 1.0;
            double  cW  = GetColWidth(c);

            Brush   bgBrush = dat.BgBrush ?? Brushes.White;
            Brush   fgBrush = dat.FgBrush ?? Brushes.Black;

            Thickness   celPad  = dat.TextPadding ?? this.Options.TextPadding;
            HorizontalAlignment  hAlign =
                dat.HorizontalTextAlign ?? this.Options.HorizontalTextAlign;
            VerticalAlignment    vAlign =
                dat.VerticalTextAlign ?? this.Options.VerticalTextAlign;

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

            //  パディングと配置を考慮して描画位置を計算。  //
            double  inX  = x + celPad.Left;
            double  inY  = y + celPad.Top;
            double  inW  = cW - (celPad.Left + celPad.Right);
            double  inH  = rH - (celPad.Top + celPad.Bottom);

            if ( inW < 0 ) { inW = 0; }
            if ( inH < 0 ) { inH = 0; }

            double  textX = inX;
            double  textY = inY;

            switch ( hAlign ) {
            case  HorizontalAlignment.Center:
                textX = inX + (inW - formattedText.Width) / 2;
                break;
            case  HorizontalAlignment.Right:
                textX = inX + (inW - formattedText.Width);
                break;
            case  HorizontalAlignment.Stretch:
            case  HorizontalAlignment.Left:
            default:
                textX = inX;
                break;
            }

            switch ( vAlign ) {
            case  VerticalAlignment.Center:
                textY = inY + (inH - formattedText.Height) / 2;
                break;
            case  VerticalAlignment.Bottom:
                textY = inY + (inH - formattedText.Height);
                break;
            case  VerticalAlignment.Stretch:
            case  VerticalAlignment.Top:
            default:
                textY = inY;
                break;
            }

            dc.PushClip(new RectangleGeometry(
                new Rect(x + 0.5, y + 0.5, cW - 1.0, rH - 1.0)
            ));
            dc.DrawText(formattedText, new Point(textX, textY));
            dc.Pop();
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
/**   Layouts 内のプロパティが変化した時の処理。
**
**    Layouts プロパティのインスタンス（参照）は変化しないが、
**  そのインスタンスの中身が変更された場合の処理。
**/
private  void
OnLayoutsPropertyChanged(object? sender, EventArgs e)
{
    this.UpdateRowPositions(true);
    this.UpdateColPositions(true);

    this.InvalidateMeasure();
    this.InvalidateVisual();
}

//----------------------------------------------------------------
/**   Options 内のプロパティが変化した時の処理。
**
**    Options プロパティのインスタンス（参照）は変化しないが、
**  そのインスタンスの中身が変更された場合の処理。
**/
private  void
OnOptionsPropertyChanged(object? sender, EventArgs e)
{
    this.InvalidateVisual();
}

//----------------------------------------------------------------
/**
**
**/
private  static  int
GetIndexFromCache(
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

//----------------------------------------------------------------
/**
**
**/
private  int
GetColIndexAtX(double  x)
{
    return  GetIndexFromCache(this.m_colPos, x, this.Columns);
}

//----------------------------------------------------------------
/**
**
**/
private  int
GetRowIndexAtY(double  y)
{
    return  GetIndexFromCache(this.m_rowPos, y, this.Rows);
}

//----------------------------------------------------------------
/**
**
**/
private  void
UpdateColPositions(
        System.Boolean  bForce  = false)
{
    //  列数や幅データが変わっていたら位置キャッシュを更新  //
    if ( this.m_colPos.Count != this.Columns ) {
        this.m_flagColCacheEnabled  = false;
    }
    if ( this.m_flagColCacheEnabled && ! bForce ) { return; }

    this.m_colPos.Clear();
    int     numCols = this.Columns;
    double  current = 0;

    for ( int c = 0; c < numCols; ++ c ) {
        this.m_colPos.Add(current);
        double  w = GetColWidth(c);
        current += w;
    }
    this.m_totalWidth   = current;

    this.m_flagColCacheEnabled  = true;
}

//----------------------------------------------------------------
/**
**
**/
private  void
UpdateRowPositions(
        System.Boolean  bForce  = false)
{
    if ( this.m_rowPos.Count != this.Rows ) {
        this.m_flagRowCacheEnabled  = false;
    }
    if ( this.m_flagRowCacheEnabled && ! bForce ) { return; }

    this.m_rowPos.Clear();
    int     numRows = this.Rows;
    double  current = 0;

    for ( int r = 0; r < numRows; ++ r ) {
        this.m_rowPos.Add(current);
        double  h = GetRowHeight(r);
        current += h;
    }
    this.m_totalHeight  = current;

    this.m_flagRowCacheEnabled  = true;
}

//========================================================================
//
//    For Internal Use Only (Static Members).
//

//----------------------------------------------------------------
/**
**
**/
private  static  object
CoerceLayouts(DependencyObject d, object baseValue)
{
    if ( baseValue == null ) {
        return  new MatrixLayout();
    }
    return ( baseValue );
}

//----------------------------------------------------------------
/**
**
**/
private  static  object
CoerceOptions(DependencyObject d, object baseValue)
{
    if ( baseValue == null ) {
        return  new MatrixOption();
    }
    return ( baseValue );
}


//----------------------------------------------------------------
/**
**
**/
private  static  void
OnColumnWidthsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    ((MatrixDisplay)d).UpdateColPositions(true);
}

//----------------------------------------------------------------
/**   Layouts プロパティ自体が丸々交換されたときの処理
**
**    Layouts プロパティにセットされていたインスタンスが、
**  別のインスタンスを参照するよう変更された場合の処理。
**/
private  static  void
OnLayoutsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    if ( d is MatrixDisplay display ) {
        if ( e.OldValue is MatrixLayout oldLayouts ) {
            oldLayouts.PropertyChanged -= display.OnLayoutsPropertyChanged;
        }
        if ( e.NewValue is MatrixLayout newLayouts ) {
            newLayouts.PropertyChanged += display.OnLayoutsPropertyChanged;
       }
    }
}

//----------------------------------------------------------------
/**   Options プロパティ自体が丸々交換されたときの処理
**
**    Options プロパティにセットされていたインスタンスが、
**  別のインスタンスを参照するよう変更された場合の処理。
**/
private  static  void
OnOptionsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    if ( d is MatrixDisplay display ) {
        if ( e.OldValue is MatrixOption oldOptions ) {
            oldOptions.PropertyChanged -= display.OnOptionsPropertyChanged;
        }
        if ( e.NewValue is MatrixOption newOptions ) {
            newOptions.PropertyChanged += display.OnOptionsPropertyChanged;
       }
    }
}

//----------------------------------------------------------------
/**
**
**/
private  static  void
OnRowHeightsChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    ((MatrixDisplay)d).UpdateRowPositions(true);
}


//========================================================================
//
//    Member Variables.
//

private   List<double>      m_colPos = new List<double>();
private   List<double>      m_rowPos = new List<double>();

private   double            m_totalWidth;
private   double            m_totalHeight;

private   System.Boolean    m_flagColCacheEnabled;
private   System.Boolean    m_flagRowCacheEnabled;


}   //  End op class  MatrixDisplay

}   //  End of namespace  WpfControl.Editor
