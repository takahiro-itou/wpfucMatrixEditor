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
//    Properties.
//

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
//    Dependency Properties.
//

public  static  readonly  DependencyProperty  ColumnsProperty =
DependencyProperty.Register(
    nameof(Columns), typeof(int), typeof(MatrixViewer),
    new FrameworkPropertyMetadata(0)
);

public  static  readonly  DependencyProperty  MatrixDataProperty =
DependencyProperty.Register(
    nameof(MatrixData), typeof(int[]), typeof(MatrixViewer),
    new FrameworkPropertyMetadata(null)
);

public  static  readonly  DependencyProperty  RowsProperty =
DependencyProperty.Register(
    nameof(Rows), typeof(int), typeof(MatrixViewer),
    new FrameworkPropertyMetadata(0)
);



}   //  End class  MatrixViewer

}   //  End of namespace  WpfControl.Editor
