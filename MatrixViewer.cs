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

public  MatrixViewer()
{
    DefaultStyleKeyProperty.OverrideMetadata(
        typeof(MatrixViewer),
        new FrameworkPropertyMetadata(typeof(MatrixViewer)));
}


}   //  End class  MatrixViewer

}   //  End of namespace  WpfControl.Editor
