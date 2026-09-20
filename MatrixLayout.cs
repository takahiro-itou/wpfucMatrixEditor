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

using   System.Windows;


namespace  WpfControl.Editor  {

//========================================================================
//
//    MatrixLayout  class
//

public  class  MatrixLayout : System.Windows.Freezable
{

//========================================================================
//
//    Properties.
//

public  event   EventHandler?   PropertyChanged;


public  IList<double>  ColumnWidths  {
    get { return  (IList<double>)GetValue(ColumnWidthsProperty); }
    set { SetValue(ColumnWidthsProperty, value); }
}

public  double  DefaultCellHeight {
    get { return  (double)GetValue(DefaultCellHeightProperty); }
    set { SetValue(DefaultCellHeightProperty, value); }
}

public  double  DefaultCellWidth  {
    get { return  (double)GetValue(DefaultCellWidthProperty); }
    set { SetValue(DefaultCellWidthProperty, value); }
}

public  IList<double>  RowHeights  {
    get { return  (IList<double>)GetValue(RowHeightsProperty); }
    set { SetValue(RowHeightsProperty, value); }
}


//========================================================================
//
//    Dependency Properties.
//

private  const  FrameworkPropertyMetadataOptions
AFFECTS_RENDER =
        FrameworkPropertyMetadataOptions.AffectsRender;

private  const  FrameworkPropertyMetadataOptions
AFFECTS_LAYOUT =
        FrameworkPropertyMetadataOptions.AffectsMeasure |
        AFFECTS_RENDER;


public  static  readonly  DependencyProperty  ColumnWidthsProperty =
DependencyProperty.Register(
    nameof(ColumnWidths), typeof(IList<double>), typeof(MatrixLayout),
    new FrameworkPropertyMetadata(null, OnInternalPropertyChanged)
);

public  static  readonly  DependencyProperty  DefaultCellHeightProperty =
DependencyProperty.Register(
    nameof(DefaultCellHeight), typeof(double), typeof(MatrixLayout),
    new FrameworkPropertyMetadata(25.0, OnInternalPropertyChanged)
);

public  static  readonly  DependencyProperty  DefaultCellWidthProperty =
DependencyProperty.Register(
    nameof(DefaultCellWidth), typeof(double), typeof(MatrixLayout),
    new FrameworkPropertyMetadata(60.0, OnInternalPropertyChanged)
);

public  static  readonly  DependencyProperty  RowHeightsProperty =
DependencyProperty.Register(
    nameof(RowHeights), typeof(IList<double>), typeof(MatrixLayout),
    new FrameworkPropertyMetadata(null, OnInternalPropertyChanged)
);


//========================================================================
//
//    Protected Member Functions (Overrides).
//

//----------------------------------------------------------------
/**
**
**/
protected  override  Freezable
CreateInstanceCore()
{
    return  new MatrixLayout();
}


//========================================================================
//
//    Event Handlers.
//

private  static  void
OnInternalPropertyChanged(
        DependencyObject                    d,
        DependencyPropertyChangedEventArgs  e)
{
    if ( d is MatrixLayout layouts ) {
        layouts.PropertyChanged?.Invoke(layouts, EventArgs.Empty);
    }
}



}   //  End op class  MatrixLayout

}   //  End of namespace  WpfControl.Editor
