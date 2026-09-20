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
using   System.Windows.Media;


namespace  WpfControl.Editor  {

//========================================================================
//
//    MatrixOption  class
//

public  class  MatrixOption : System.Windows.Freezable
{

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
//    Properties.
//

public  event   EventHandler?   PropertyChanged;


public  Brush  Background {
    get { return  (Brush)GetValue(BackgroundProperty); }
    set { SetValue(BackgroundProperty, value); }
}

public  Brush  BorderLine  {
    get { return  (Brush)GetValue(BorderLineProperty); }
    set { SetValue(BorderLineProperty, value); }
}


public  Brush  GridBackground  {
    get { return  (Brush)GetValue(GridBackgroundProperty); }
    set { SetValue(GridBackgroundProperty, value); }
}

public  Brush  GridLine  {
    get { return  (Brush)GetValue(GridLineProperty); }
    set { SetValue(GridLineProperty, value); }
}


//========================================================================
//
//    Dependency Properties.
//

private  const  FrameworkPropertyMetadataOptions
AFFECTS_RENDER =
        FrameworkPropertyMetadataOptions.AffectsRender;


public  static  readonly  DependencyProperty  BackgroundProperty =
DependencyProperty.Register(
        nameof(Background), typeof(Brush), typeof(MatrixOption),
        new FrameworkPropertyMetadata(
                DEFAULT_BACKGROUND, OnInternalPropertyChanged)
);

public  static  readonly  DependencyProperty  BorderLineProperty =
DependencyProperty.Register(
        nameof(BorderLine), typeof(Brush), typeof(MatrixOption),
        new FrameworkPropertyMetadata(
                DEFAULT_BORDER_LINE, OnInternalPropertyChanged)
);

public  static  readonly  DependencyProperty  GridBackgroundProperty =
DependencyProperty.Register(
        nameof(GridBackground), typeof(Brush), typeof(MatrixOption),
        new FrameworkPropertyMetadata(
                Brushes.White, OnInternalPropertyChanged)
);


public  static  readonly  DependencyProperty  GridLineProperty =
DependencyProperty.Register(
            nameof(GridLine), typeof(Brush), typeof(MatrixOption),
        new FrameworkPropertyMetadata(
                Brushes.Black, OnInternalPropertyChanged)
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
    return  new MatrixOption();
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
    if ( d is MatrixOption options ) {
        options.PropertyChanged?.Invoke(options, EventArgs.Empty);
    }
}


}   //  End op class  MatrixOption

}   //  End of namespace  WpfControl.Editor
