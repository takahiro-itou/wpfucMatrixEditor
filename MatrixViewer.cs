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

using   System.Collections;
using   System.Windows;
using   System.Windows.Controls;


namespace  WpfControl.Editor  {

//========================================================================
//
//    MatrixViewer  class
//

public  class  MatrixViewer : Control
{

//========================================================================
//
//    Constructor(s) and Destructor.
//

//----------------------------------------------------------------
/**   コンストラクタ。
**
**/

static  MatrixViewer()
{
    DefaultStyleKeyProperty.OverrideMetadata(
        typeof(MatrixViewer),
        new FrameworkPropertyMetadata(typeof(MatrixViewer)));
}


//========================================================================
//
//    Public Member Functions.
//

public  override  void
OnApplyTemplate()
{
    base.OnApplyTemplate();

    this.m_mdPart = GetTemplateChild("PART_MatrixDisplay") as MatrixDisplay;
    updateInternalData();
}


//========================================================================
//
//    Properties.
//

public  int  Columns  {
    get { return  (int)GetValue(ColumnsProperty); }
    set { SetValue(ColumnsProperty, value); }
}

public  IList<double>  ColumnWidths  {
    get { return  (IList<double>)GetValue(ColumnWidthsProperty); }
    set { SetValue(ColumnWidthsProperty, value); }
}

public  ScrollBarVisibility  HorizontalScrollBarVisibility  {
    get => (ScrollBarVisibility)GetValue(HorizontalScrollBarVisibilityProperty);
    set { SetValue(HorizontalScrollBarVisibilityProperty, value); }
}

public  IList  MatrixData  {
    get { return  (IList)GetValue(MatrixDataProperty); }
    set { SetValue(MatrixDataProperty, value); }
}

public  IList<double>  RowHeights  {
    get { return  (IList<double>)GetValue(RowHeightsProperty); }
    set { SetValue(RowHeightsProperty, value); }
}

public  int  Rows  {
    get { return  (int)GetValue(RowsProperty); }
    set { SetValue(RowsProperty, value); }
}

public  ScrollBarVisibility  VerticalScrollBarVisibility  {
    get => (ScrollBarVisibility)GetValue(VerticalScrollBarVisibilityProperty);
    set { SetValue(VerticalScrollBarVisibilityProperty, value); }
}


//========================================================================
//
//    Dependency Properties.
//

public  static  readonly  DependencyProperty  ColumnsProperty =
DependencyProperty.Register(
        nameof(Columns), typeof(int), typeof(MatrixViewer),
        new FrameworkPropertyMetadata(0)
);

public  static  readonly  DependencyProperty  ColumnWidthsProperty =
DependencyProperty.Register(
        nameof(ColumnWidths), typeof(IList<double>), typeof(MatrixViewer),
        new FrameworkPropertyMetadata(null)
);

public  static  readonly  DependencyProperty
HorizontalScrollBarVisibilityProperty =
ScrollViewer.HorizontalScrollBarVisibilityProperty.AddOwner(
        typeof(MatrixViewer)
);

public  static  readonly  DependencyProperty  MatrixDataProperty =
DependencyProperty.Register(
        nameof(MatrixData), typeof(IList), typeof(MatrixViewer),
        new FrameworkPropertyMetadata(null, OnMatrixDataChanged)
);

public  static  readonly  DependencyProperty  RowHeightsProperty =
DependencyProperty.Register(
        nameof(RowHeights), typeof(IList<double>), typeof(MatrixViewer),
        new FrameworkPropertyMetadata(null)
);

public  static  readonly  DependencyProperty  RowsProperty =
DependencyProperty.Register(
        nameof(Rows), typeof(int), typeof(MatrixViewer),
        new FrameworkPropertyMetadata(0)
);

public  static  readonly  DependencyProperty
VerticalScrollBarVisibilityProperty =
ScrollViewer.VerticalScrollBarVisibilityProperty.AddOwner(
        typeof(MatrixViewer)
);


//========================================================================
//
//    For Internal Use Only.
//

private  static  void
OnMatrixDataChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    if ( d is MatrixViewer owner ) {
        owner.updateInternalData();
    }
}

private  void
updateInternalData()
{
    if ( this.m_mdPart == null ) { return; }

    if ( MatrixData is MatrixCellData[] rawArray ) {
        this.m_mdPart.MatrixData = rawArray;
    } else {
        this.m_mdPart.MatrixData = null;
    }
}


//========================================================================
//
//    Member Variables.
//

private   MatrixDisplay?    m_mdPart;


}   //  End class  MatrixViewer

}   //  End of namespace  WpfControl.Editor
