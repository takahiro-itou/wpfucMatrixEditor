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

using   System.Windows.Media;


namespace  WpfControl.Editor  {

//========================================================================
//
//    MatrixCellData  struct
//
/**
**    行列のセルを管理する構造体。
**
**    メモリ効率を最大化するため値型（構造体）として定義。
**/

public  struct  MatrixCellData
{

public  System.String   Value   { get; set; }
public  Color           BgColor { get; set; }
public  Color           FgColor { get; set; }

public  HorizontalAlignment     HorizontalTextAlign { get; set; }
public  VerticalAlignment       VerticalTextAlign   { get; set; }


}   //  End of struct  MatrixCellData

}   //  End of namespace  WpfControl.Editor
