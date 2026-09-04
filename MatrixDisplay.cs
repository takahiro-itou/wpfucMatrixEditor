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
}

//========================================================================
//
//    Properties.
//

public  static  readonly  DependencyProperty
RowsProperty = DependencyProperty.Register(
    name(Rows), typeof(int), typeof(MatrixDisplay),
    new FrameworkPropertyMetadata(
            0, FrameworkPropertyMetadataOptions.AffectsRender)
);


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
