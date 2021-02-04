Option Explicit On

Imports System.Drawing
Imports System.Drawing.Drawing2D


Public NotInheritable Class clsDefine

    Public Const MAIN_PANEL1_MIN As Integer = 335 '345 '338
    Public Const NONE_VALUE As Integer = -1
    Public Const BEGIN_VALUE As Integer = 1

    Public Const SPEC_CONTROL_HOR As Integer = 10
    Public Const SPEC_CONTROL_VER As Integer = 20

    Public Const CARD_LARG_W As Integer = 375
    Public Const CARD_LARG_H As Integer = 195
    Public Const PIC_LARG_W As Integer = 105 '85
    Public Const PIC_LARG_H As Integer = 145 '90
    Public Const MARGIN_LARG_LEFT As Integer = 130

    Public Const PIC_CROP_RATIO As Double = 0.75R '0.76R '85

    Public Const CARD_MIDD_W As Integer = 255
    Public Const CARD_MIDD_H As Integer = 115
    Public Const PIC_MIDD_W As Integer = 65 '65
    Public Const PIC_MIDD_H As Integer = 80 '70
    Public Const MARGIN_MID_LEFT As Integer = 80
    Public Const MARGIN_MID_LEFT2 As Integer = 150

    Public Const CARD_SMAL_W As Integer = 200
    Public Const CARD_SMAL_H As Integer = 75
    Public Const PIC_SMAL_W As Integer = 36 '35
    Public Const PIC_SMAL_H As Integer = 48 '45
    Public Const MARGIN_SMAL_LEFT As Integer = 50

    Public Const CARD_MINI_W As Integer = 100 '150
    Public Const CARD_MINI_H As Integer = 45

    Public Const CARD_ADD_W As Integer = 375 '150
    Public Const CARD_ADD_H As Integer = 60

    'Start Manh 2012/12/03: Convert from constant to Variable,
    'we just use these properties to get information from control which is desiged by Programmer
    Public Shared MEM_CARD_W_L As Integer = 110
    Public Shared MEM_CARD_H_L As Integer = 150
    Public Shared MEM_CARD_HORIZON_BUFFER_L As Integer = 20
    Public Shared MEM_CARD_VERTICAL_BUFFER_L As Integer = 40

    Public Shared MEM_CARD_W_S As Integer = 110
    Public Shared MEM_CARD_H_S As Integer = 100 '90
    Public Shared MEM_CARD_HORIZON_BUFFER_S As Integer = 20
    Public Shared MEM_CARD_VERTICAL_BUFFER_S As Integer = 35
    'End Manh 2012/12/03:


    Public Const MEM_CARD_SPACE_LEFT_LARGE As Integer = 150 '175
    Public Const MEM_CARD_SPACE_DOWN_LARGE As Integer = 200 '174 '175
    Public Const MEM_CARD_SPACE_LEFT_SMALL As Integer = 140 '175
    Public Const MEM_CARD_SPACE_DOWN_SMALL As Integer = 135

    Public Const MEM_CARD_SPACE_LEFT_LARGE2 As Integer = 250 '175
    Public Const MEM_CARD_SPACE_DOWN_LARGE2 As Integer = 174 '175

    Public Const SPEC_CARD_MIN_LEFT As Integer = 5
    Public Const SPEC_CARD_MIN_TOP As Integer = 5

    'Start Manh 2012/12/03: These properties is used for set height and width of image
    'The pic control at class just using for load image
    Public Const THUMBNAIL_W As Integer = 57 '60
    Public Const THUMBNAIL_H As Integer = THUMBNAIL_W / PIC_CROP_RATIO '75 '80
    'End Manh 2012/12/03

    'Start Manh 2012/12/03: Do not need to use these properties anymore
    'Public Const MEMCARD_DETAIL_W As Integer = 168
    'Public Const MEMCARD_DETAIL_H As Integer = 100
    'End Manh 2012/12/03

    Public Const MEMCARD_2_W As Integer = 500 '188
    Public Const MEMCARD_2_MARGIN_RIGHT As Integer = 5
    Public Shared MEMCARD_2_VERTICAL_BUFFER As Integer = 20


    '########
    Public Const PENSTYLE As DashStyle = DashStyle.Solid
    Public Const PENSIZE As Integer = 1
    Public Const LINE_LENGHT As Integer = 30


    Public Const TREE_S1_STARTX As Integer = 20
    Public Const TREE_S1_STARTY As Integer = 20

    Public Const gcstrFontName As String = "Arial"
End Class
