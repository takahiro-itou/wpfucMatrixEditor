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


public  int  Columns  {
    get { return  (int)GetValue(ColumnsProperty); }
    set { SetValue(ColumnsProperty, value); }
}

public  int[]  MatrixData  {
    get { return  (int[])GetValue(MatrixDataProperty); }
    set { SetValue(MatrixDataProperty, value); }
}

public  int  Rows  {
    get { return  (int)GetValue(RowsProperty); }
    set { SetValue(RowsProperty, value); }
}


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
    if ( this.MatrixData.Length <= Rows * Columns ) {
        return;
    }

    //  描画サイズを計算。  //
    double renderWidth  = this.RenderSize.Width;
    double renderHeight = this.RenderSize.Height;
    double cellWidth    = renderWidth / Columns;
    double cellHeight   = renderHeight / Rows;

    Pen gridPen = new Pen(Brushes.LightGray, 0.5);
    Typeface typeface = new Typeface(
            SystemFonts.CaptionFontFamily,
            FontStyles.Normal,  FontWeights.Normal, FontStretches.Normal);
    double fontSize = Math.Min(cellHeight * 0.5, 12);

    dc.DrawRectangle(
            Brushes.Green, null, new Rect(0, 0, renderWidth, renderHeight));
    return;
}


//========================================================================
//
//    For Internal Use Only.
//

//========================================================================
//
//    Member Variables.
//


}   //  End class  MatrixDisplay

}   //  End of namespace  WpfControl.Editor
