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

public  class  MatrixLayout : System.Windows.DependencyObject
{

//========================================================================
//
//    Properties.
//


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

}   //  End op class  MatrixLayout

}   //  End of namespace  WpfControl.Editor
