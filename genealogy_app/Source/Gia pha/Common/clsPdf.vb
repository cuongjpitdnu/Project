'   ******************************************************************
'      TITLE      : Pdf FUNCTIONS
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/12/12　AKB
'      UPDATE     : 
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict Off

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports PdfSharp.Pdf.Advanced
Imports System.Runtime.InteropServices
Imports System.Drawing.Imaging


' ▽ 2017/07/27 AKB Nguyen Thanh Tung --------------------------------
'Public Class clsPdf
Public Class clsPdf
    Implements IDisposable
    ' △ 2017/07/27 AKB Nguyen Thanh Tung --------------------------------

    Private mobjDocument As PdfDocument = New PdfDocument
    Private mobjPage As PdfPage
    Private mobjGfx As XGraphics
    Private mobjPen As XPen

    Private mobjCardLeft As Object                                          'temporary card
    Private mobjCardRight As Object                                         'temporary card
    Private mintStartY As Integer = 100
    Private mintStartX As Integer = 30
    Private mintA4 As Integer = 2500
    Private mintA4Width As Integer = 846
    Private mintA4Height As Integer = 600

    Private mintFontZoom As Integer = 1
    Private mintTreeWidth As Integer = 1
    Private Const mcstrFontName As String = "Arial"

    Private mintMargin As Integer = 30
    Private mintStringLineSpace As Integer = 5
    Private mintNomalFontSize As Integer = 16
    Private mintTitleFontSize As Integer = 30
    Private mfontOptions As XPdfFontOptions = New XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always)

    Private mobjForm As PdfSharp.Forms.PagePreview

    Private mblnIsSmallCard As Boolean                                      'draw small card
    Private mintMEM_CARD_SPACE_LEFT As Integer                              'margin left
    Private mintMEM_CARD_SPACE_DOWN As Integer                              'margin bottom
    Private mintMEM_CARD_W As Integer                                       'card width
    Private mintMEM_CARD_H As Integer                                       'card height

    Private mstrFamilyInfo As String
    Private mstrFamilyAnniInfo As String
    Private mstrRootInfo As String
    Private mstrCreateDate As String
    Private mstrCreateMember As String
    Private mblnBorder As Boolean

    Private mintImageMode As Long
    Private mdblCARD_W As Double
    Private mdblCARD_H As Double
    Private mdblPIC_W As Double
    Private mdblPIC_H As Double
    Private mCardBg As XImage

    ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
    Private mbushText As New SolidBrush(My.Settings.objColorText)
    Private mobjFont As Font
    ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------

    '   ******************************************************************
    '　　　	FUNCTION   : emLocation
    '      	MEMO       : Location In Page PDF
    '      	CREATE     : 2017/08/01 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Enum emLocation
        TopLeft = 0
        TopRight = 1
        BottomLeft = 2
        BottomRight = 3
    End Enum

    Public Function fncSetImageMode(ByVal intImageMode As Integer) As Boolean
        Try
            If intImageMode = 1 Then

                mdblCARD_W = clsDefine.CARD_MIDD_W
                mdblCARD_H = clsDefine.CARD_MIDD_H
                mdblPIC_W = clsDefine.PIC_MIDD_W
                mdblPIC_H = clsDefine.PIC_MIDD_H

            End If

        Catch ex As Exception

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Save pdf file
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Sub New(Optional ByVal dblWidth As Double = 0, Optional ByVal dblHeight As Double = 0, Optional ByVal intTreeWidth As Integer = 0)

        mintTreeWidth = intTreeWidth
        xInit(dblWidth, dblHeight)

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : Save pdf file
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Sub New(ByVal blnAddPage As Boolean)
        Try

        Catch ex As Exception

        End Try

    End Sub

    '   ****************************************************************** 
    '      FUNCTION   : CreatDate
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property CreateDate() As String
        Get
            Return mstrCreateDate
        End Get

        Set(ByVal strValue As String)
            mstrCreateDate = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : MaxHeight Property, max height of panel
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property FamilyInfo() As String
        Get
            Return mstrFamilyInfo
        End Get

        Set(ByVal strValue As String)
            mstrFamilyInfo = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : MaxHeight Property, max height of panel
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property FamilyAnniInfo() As String
        Get
            Return mstrFamilyAnniInfo
        End Get

        Set(ByVal strValue As String)
            mstrFamilyAnniInfo = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : MaxHeight Property, max height of panel
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property RootInfo() As String
        Get
            Return mstrRootInfo
        End Get

        Set(ByVal strValue As String)
            mstrRootInfo = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : CreatDate
    '      MEMO       :  
    '      CREATE     : 2012/11/22  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property CreateMember() As String
        Get
            Return mstrCreateMember
        End Get

        Set(ByVal strValue As String)
            mstrCreateMember = strValue
        End Set

    End Property

    '   ****************************************************************** 
    '      FUNCTION   : ShowBorder
    '      MEMO       :  
    '      CREATE     : 2013/01/30  AKB Manh
    '      UPDATE     :  
    '   ******************************************************************
    Public Property ShowBorder() As Boolean
        Get
            Return mblnBorder
        End Get

        Set(ByVal blnValue As Boolean)
            mblnBorder = blnValue
        End Set

    End Property

    '   ******************************************************************
    '　　　	PROPERTY   : FontUser
    '      	MEMO       : 
    '      	CREATE     : 2018/04/27 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public WriteOnly Property FontUser As Font
        Set(value As Font)
            If IsNothing(value) Then value = My.Settings.objFontDefaut
            mobjFont = value
        End Set
    End Property

    '   ******************************************************************
    '　　　FUNCTION   : Save pdf file
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Sub New(ByVal objFrom As PdfSharp.Forms.PagePreview, _
            Optional ByVal dblWidth As Double = 0, _
            Optional ByVal dblHeight As Double = 0, _
            Optional ByVal intTreeWidth As Integer = 0)

        mobjForm = mobjForm
        'Dim objRender As PdfSharp.Forms.PagePreview.RenderEvent = New PdfSharp.Forms.PagePreview.RenderEvent(AddressOf Render)
        'mobjForm.SetRenderEvent(objRender)
        mintTreeWidth = intTreeWidth
        xInit(dblWidth, dblHeight)

    End Sub

    Private Sub xInit(Optional ByVal dblWidth As Double = 0, Optional ByVal dblHeight As Double = 0)
        mobjDocument.Info.Title = "Phan mem gia pha"
        mobjDocument.Info.Creator = "AKBSoftware(akb.Com.vn)"

        mobjPage = mobjDocument.AddPage
        mobjPen = New XPen(XColor.FromArgb(0, 0, 0))

        If dblWidth > mintA4Width Then

            mobjPage.Width = dblWidth

        Else

            mobjPage.Width = mintA4Width

        End If

        mintStartX = CInt((mobjPage.Width.Point - dblWidth) / 2)

        If dblHeight > mintA4Height Then

            mobjPage.Height = dblHeight '+ 130

        Else

            mobjPage.Height = mintA4Height '+ 40

        End If

        If CInt(mobjPage.Width.Point / mintA4) > 0 Then

            mintFontZoom = CInt(mobjPage.Width.Point / mintA4)

        End If

        If mintFontZoom > 2 Then mintFontZoom = 2

        mintMargin = mintFontZoom * mintMargin
        mintTitleFontSize = mintFontZoom * mintTitleFontSize
        mintStringLineSpace = mintStringLineSpace * mintFontZoom

        mobjPage.Height = CInt(mobjPage.Height) + 3 * mintMargin
        mobjPage.Width = CInt(mobjPage.Width) + 3 * mintMargin
        mintStartX = mintMargin
        mintStartY = CInt(1.5 * mintMargin)
        mobjGfx = XGraphics.FromPdfPage(mobjPage)

    End Sub

    Public Sub fncAddpage(Optional ByVal dblWidth As Double = 0, Optional ByVal dblHeight As Double = 0)
        'mobjDocument.Info.Title = "Phan mem gia pha"
        'mobjDocument.Info.Creator = "AKBSoftware(akb.Com.vn)"
        mintMargin = 30
        mintStringLineSpace = 5
        mintTitleFontSize = 30
        mintNomalFontSize = 16
        mobjPage = mobjDocument.AddPage
        mobjPen = New XPen(XColor.FromArgb(0, 0, 0))
        mintTreeWidth = dblWidth
        If dblWidth > mintA4Width Then

            mobjPage.Width = dblWidth

        Else

            mobjPage.Width = mintA4Width

        End If

        mintStartX = CInt((mobjPage.Width.Point - dblWidth) / 2)

        If dblHeight > mintA4Height Then

            mobjPage.Height = dblHeight '+ 130

        Else

            mobjPage.Height = mintA4Height '+ 40

        End If

        If CInt(mobjPage.Width.Point / mintA4) > 0 Then

            mintFontZoom = CInt(mobjPage.Width.Point / mintA4)

        End If

        If mintFontZoom > 2 Then mintFontZoom = 2

        mintMargin = mintFontZoom * mintMargin
        mintTitleFontSize = mintFontZoom * mintTitleFontSize
        mintStringLineSpace = mintStringLineSpace * mintFontZoom

        mobjPage.Height = CInt(mobjPage.Height) + 3 * mintMargin
        mobjPage.Width = CInt(mobjPage.Width) + 3 * mintMargin
        mintStartX = mintMargin
        mintStartY = CInt(1.5 * mintMargin)
        mobjGfx = XGraphics.FromPdfPage(mobjPage)

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : Save pdf file
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Sub Save(ByVal strFile As String, Optional ByVal blnShow As Boolean = True)
        Try

            mobjDocument.Close()

            mobjDocument.Save(strFile)


            If blnShow = True Then

                Try
                    Process.Start(strFile)
                Catch ex As Exception
                    fncMessageWarning("Bạn cần cài đặt chường trình đọc file Pdf để mở file này.")
                End Try

            End If

            mobjPage = Nothing
            mobjDocument = Nothing

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncOpen, open pdf file
    '      PARAMS     : strFile String, file path
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncOpen(ByVal strFile As String) As Boolean

        fncOpen = False

        Try
            Process.Start(strFile)

            mobjPen = Nothing
            mobjGfx = Nothing
            mobjPage = Nothing
            mobjDocument = Nothing

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : Save pdf file
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddInfo() As Boolean

        Try

            Dim intFontZoom As Integer = 1
            Dim intExtent As Integer = mintMargin

            Dim xfontTitle As XFont = New XFont(mcstrFontName, mintTitleFontSize, XFontStyle.Regular, mfontOptions)
            Dim xfontNomal As XFont = New XFont(mcstrFontName, mintNomalFontSize, XFontStyle.Regular, mfontOptions)
            Dim intHeight As Integer
            Dim strMax As String = ""

            'Reduce fontsize if String too long
            If strMax.Length < mstrFamilyAnniInfo.Length Then strMax = mstrFamilyAnniInfo
            If strMax.Length < mstrFamilyInfo.Length Then strMax = mstrFamilyInfo
            If strMax.Length < mstrRootInfo.Length Then strMax = mstrRootInfo

            Dim xsizeString As XSize = mobjGfx.MeasureString(strMax, xfontTitle)
            Do Until xsizeString.Width < mobjPage.Width.Point - 3 * mintMargin
                xfontTitle = New XFont(mcstrFontName, xfontTitle.Size - 1, XFontStyle.Regular, mfontOptions)
                xsizeString = mobjGfx.MeasureString(strMax, xfontTitle)
            Loop

            If mstrCreateMember <> "" Then mstrCreateDate = mstrCreateDate & "                      NGƯỜI LẬP: " & mstrCreateMember.ToUpper()

            'Calculate  the position of Text
            Dim strPosDate As XPoint = xGetPos(mstrCreateDate, xfontTitle, mintMargin, intHeight)
            Dim strPos1 As XPoint = xGetPos(mstrFamilyInfo, xfontTitle, strPosDate.Y, intHeight)
            Dim strPos2 As XPoint = xGetPos(mstrFamilyAnniInfo, xfontTitle, strPos1.Y + mintStringLineSpace, intHeight)
            Dim strPos3 As XPoint = xGetPos(mstrRootInfo, xfontTitle, strPos2.Y + mintStringLineSpace, intHeight)

            mintStartY = strPos3.Y + mintStringLineSpace + 2 * intHeight
            mobjPage.Height = CInt(mobjPage.Height) + mintStartY - mintMargin

            mintStartX = (mobjPage.Width.Point - mintTreeWidth) / 2

            'mobjGfx = XGraphics.FromPdfPage(mobjPage)
            mobjGfx.DrawString(mstrCreateDate, xfontNomal, XBrushes.Black, New XPoint(CInt(mintMargin + mintStringLineSpace), strPosDate.Y))
            mobjGfx.DrawString(mstrFamilyInfo, xfontTitle, XBrushes.Black, strPos1)
            mobjGfx.DrawString(mstrFamilyAnniInfo, xfontTitle, XBrushes.Black, strPos2)
            mobjGfx.DrawString(mstrRootInfo, xfontTitle, XBrushes.Black, strPos3)

            'If ShowBorder Then

            '    mobjGfx.DrawRectangle(mobjPen, mintMargin, mintMargin, mobjPage.Width.Point - 2 * mintMargin, mobjPage.Height.Point - 2 * mintMargin)

            'End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    '   ******************************************************************
    '　　　FUNCTION   : Save pdf file
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddMemInfo() As Boolean

        Try

            Try

                Dim intFontZoom As Integer = 1
                Dim intExtent As Integer = mintMargin

                Dim xfontTitle As XFont = New XFont(mcstrFontName, mintTitleFontSize, XFontStyle.Regular, mfontOptions)
                Dim xfontNomal As XFont = New XFont(mcstrFontName, mintNomalFontSize, XFontStyle.Regular, mfontOptions)
                Dim intHeight As Integer = 0
                Dim strMax As String = ""
                Dim strCreate As String = ""

                'Reduce fontsize if String too long
                If strMax.Length < mstrFamilyAnniInfo.Length Then strMax = mstrFamilyAnniInfo
                If strMax.Length < mstrFamilyInfo.Length Then strMax = mstrFamilyInfo
                If strMax.Length < mstrRootInfo.Length Then strMax = mstrRootInfo

                Dim xsizeString As XSize = mobjGfx.MeasureString(strMax, xfontTitle)
                Do Until xsizeString.Width < mobjPage.Width.Point - 3 * mintMargin
                    xfontTitle = New XFont(mcstrFontName, xfontTitle.Size - 1, XFontStyle.Regular, mfontOptions)
                    xsizeString = mobjGfx.MeasureString(strMax, xfontTitle)
                Loop

                If mstrCreateMember <> "" Then strCreate = mstrCreateDate & "                      NGƯỜI LẬP: " & mstrCreateMember.ToUpper()

                'Calculate  the position of Text
                Dim strPosDate As XPoint = xGetPos(mstrCreateDate, xfontTitle, mintMargin, intHeight)
                Dim strPos1 As XPoint = xGetPos(mstrFamilyInfo, xfontTitle, strPosDate.Y, intHeight)
                Dim strPos2 As XPoint = xGetPos(mstrFamilyAnniInfo, xfontTitle, strPos1.Y + mintStringLineSpace, intHeight)
                Dim strPos3 As XPoint = xGetPos(mstrRootInfo, xfontTitle, strPos2.Y + mintStringLineSpace, intHeight)

                mintStartY = strPos3.Y + mintStringLineSpace + 2 * intHeight
                mobjPage.Height = CInt(mobjPage.Height) + mintStartY - mintMargin

                mintStartX = (mobjPage.Width.Point - mintTreeWidth) / 2

                'mobjGfx = XGraphics.FromPdfPage(mobjPage)
                mobjGfx.DrawString(strCreate, xfontNomal, XBrushes.Black, New XPoint(CInt(mintMargin + mintStringLineSpace), strPosDate.Y))
                mobjGfx.DrawString(mstrFamilyInfo, xfontTitle, XBrushes.Black, strPos1)
                mobjGfx.DrawString(mstrFamilyAnniInfo, xfontTitle, XBrushes.Black, strPos2)
                mobjGfx.DrawString(mstrRootInfo, xfontTitle, XBrushes.Black, strPos3)

                If ShowBorder Then
                    mobjGfx.DrawRectangle(mobjPen, mintMargin, mintMargin, mobjPage.Width.Point - 2 * mintMargin, mobjPage.Height.Point - 2 * mintMargin)
                End If

            Catch ex As Exception
                Throw ex
            End Try

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Private Function xGetPos(ByVal strInfo As String, _
                             ByRef charFont As XFont, _
                             ByVal intStartY As Integer, _
                             ByRef intHeight As Integer) As XPoint

        If strInfo = "" Then strInfo = "A"
        Dim xsizeString As XSize = mobjGfx.MeasureString(strInfo, charFont)
        intHeight = xsizeString.Height
        Dim xpPos As XPoint = New XPoint((CInt(mobjPage.Width) - CInt(xsizeString.Width)) / 2, intStartY + xsizeString.Height + mintStringLineSpace)

        Return xpPos

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncExportTree, export F-tree to excel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : tblDrawControl      Hashtable
    '      PARAMS2    : tblNotDrawControl   Hashtable
    '      MEMO       : 
    '      CREATE     : 2011/12/13　AKB 
    '      UPDATE     : 
    '   ******************************************************************
    'Public Function fncExportTree(ByVal objImage() As XImage, _
    '                              ByVal objCard() As usrMemCardBase, _
    '                              ByVal lstNormalLine As List(Of usrLine), _
    '                              ByVal lstSpecialLine As List(Of usrLine), _
    '                              Optional ByVal blnShowInfo As Boolean = True) As Boolean

    '    fncExportTree = False
    '    Dim intLineWeight As Integer = 1
    '    Dim intArgbColor As Integer = 1
    '    'intLineWeight = My.Settings.intLineWeight
    '    'intArgbColor = My.Settings.LineColor.ToArgb
    '    Try

    '        Dim intId As Integer
    '        If blnShowInfo Then
    '            xAddInfo()
    '        Else
    '            xAddMemInfo()

    '        End If


    '        'xDrawBackground(strRootInfo)

    '        'draw connector first so that it will be set to back
    '        'If Not xDrawConnector(mobjDraw.DrawingCard, mobjDraw.NotDrawingCard) Then Return
    '        'If Not xDrawConnector(lstNormalLine, lstSpecialLine) Then Return False
    '        fncDrawPdfConnector(mobjGfx, lstNormalLine, New XPen(XColor.FromArgb(intArgbColor), intLineWeight), mintStartX, mintStartY)
    '        fncDrawPdfConnector(mobjGfx, lstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), intLineWeight + 1), mintStartX, mintStartY)

    '        If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD1 Then

    '            Dim strDefault As String
    '            If My.Settings.strCard1Bg = "" Then
    '                strDefault = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & gcstrDefaultFrame
    '            Else
    '                strDefault = My.Settings.strCard1Bg
    '            End If

    '            mCardBg = fncMakeImage(strDefault)

    '            For intId = 0 To objCard.Length - 1
    '                'draw card to pdf file
    '                fncDrawCard(objCard(intId), objImage(intId))

    '            Next intId

    '        Else

    '            For intId = 0 To objCard.Length - 1
    '                objImage(intId).Interpolate = False
    '                mobjGfx.DrawImage(objImage(intId), PdfMetric(objCard(intId).Location.X + mintStartX), PdfMetric(objCard(intId).Location.Y + mintStartY))
    '            Next

    '        End If


    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    Public Function fncExportTree(ByVal objImage() As XImage, _
                                  ByVal objCard() As usrMemCardBase, _
                                  ByVal lstNormalLine As List(Of usrLine), _
                                  ByVal lstSpecialLine As List(Of usrLine), _
                                  Optional ByVal blnShowInfo As Boolean = True) As Boolean

        fncExportTree = False
        Dim intLineWeight As Integer
        Dim intArgbColor As Integer
        'intLineWeight = My.Settings.intLineWeight
        'intArgbColor = My.Settings.LineColor.ToArgb
        Try

            Dim intId As Integer
            If blnShowInfo Then
                xAddInfo()
            Else
                xAddMemInfo()

            End If


            'xDrawBackground(strRootInfo)

            'draw connector first so that it will be set to back
            'If Not xDrawConnector(mobjDraw.DrawingCard, mobjDraw.NotDrawingCard) Then Return
            'If Not xDrawConnector(lstNormalLine, lstSpecialLine) Then Return False
            fncDrawPdfConnector(mobjGfx, lstNormalLine, New XPen(XColor.FromArgb(intArgbColor), intLineWeight), mintStartX, mintStartY)
            fncDrawPdfConnector(mobjGfx, lstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), intLineWeight + 1), mintStartX, mintStartY)
            If ShowBorder Then
                mobjGfx.DrawRectangle(mobjPen, mintMargin, mintMargin, mobjPage.Width.Point - 2 * mintMargin, mobjPage.Height.Point - 2 * mintMargin)
            End If
            If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD1 Then

                mCardBg = fncMakeImage(My.Settings.strCard1Bg)

                For intId = 0 To objCard.Length - 1
                    'draw card to pdf file
                    fncDrawCard(objCard(intId), objImage(intId))

                Next intId

            Else

                For intId = 0 To objCard.Length - 1
                    objImage(intId).Interpolate = False
                    mobjGfx.DrawImage(objImage(intId), PdfMetric(objCard(intId).CardCoor.X + mintStartX), PdfMetric(objCard(intId).CardCoor.Y + mintStartY))
                Next

            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

#Region "Add By: 2017/06/29 AKB Nguyen Thanh Tung"

    '   ******************************************************************
    '		FUNCTION   : fncExportTree2
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - XImage() - Array Image of Card
    '                    ARG2(IN) - usrMemCardBase() - Array Card Info
    '                    ARG3(IN) - List(Of usrLine) - Normal Line
    '                    ARG4(IN) - List(Of usrLine) - Special Line
    '                    ARG5(IN) - String - Title Page
    '		MEMO       : Add Title to Page
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncExportTree2(ByVal objImage() As XImage, _
                                   ByVal objCard() As usrMemCardBase, _
                                   ByVal lstNormalLine As List(Of usrLine), _
                                   ByVal lstSpecialLine As List(Of usrLine), _
                                   ByVal strTitle As String) As Boolean

        fncExportTree2 = False
        Dim intLineWeight As Integer
        Dim intArgbColor As Integer
        Dim intId As Integer
        'intLineWeight = My.Settings.intLineWeight
        'intArgbColor = My.Settings.LineColor.ToArgb
        Try

            'Add Title
            xAddTitle(strTitle)

            'xDrawBackground(strRootInfo)

            'draw connector first so that it will be set to back
            'If Not xDrawConnector(mobjDraw.DrawingCard, mobjDraw.NotDrawingCard) Then Return
            'If Not xDrawConnector(lstNormalLine, lstSpecialLine) Then Return False
            fncDrawPdfConnector(mobjGfx, lstNormalLine, New XPen(XColor.FromArgb(intArgbColor), intLineWeight), mintStartX, mintStartY)
            fncDrawPdfConnector(mobjGfx, lstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), intLineWeight + 1), mintStartX, mintStartY)
            If ShowBorder Then
                mobjGfx.DrawRectangle(mobjPen, mintMargin, mintMargin, mobjPage.Width.Point - 2 * mintMargin, mobjPage.Height.Point - 2 * mintMargin)
            End If
            If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD1 Then

                mCardBg = fncMakeImage(My.Settings.strCard1Bg)

                For intId = 0 To objCard.Length - 1
                    'draw card to pdf file
                    fncDrawCard(objCard(intId), objImage(intId))
                Next intId

            Else

                For intId = 0 To objCard.Length - 1
                    objImage(intId).Interpolate = False
                    mobjGfx.DrawImage(objImage(intId), PdfMetric(objCard(intId).CardCoor.X + mintStartX), PdfMetric(objCard(intId).CardCoor.Y + mintStartY))
                    objImage(intId).Dispose()
                Next

            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    '   ******************************************************************
    '		FUNCTION   : xAddTitle
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - String - Content Title
    '		MEMO       : Add Title to Page
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Sub xAddTitle(ByVal strTitle As String)

        Dim xfontTitle As XFont
        Dim xsizeString As XSize
        Dim strPos As XPoint
        Dim intHeight As Integer

        Try

            xfontTitle = New XFont(mcstrFontName, mintTitleFontSize, XFontStyle.Regular, mfontOptions)
            xsizeString = mobjGfx.MeasureString(strTitle, xfontTitle)

            Do Until xsizeString.Width < mobjPage.Width.Point - 3 * mintMargin
                xfontTitle = New XFont(mcstrFontName, xfontTitle.Size - 1, XFontStyle.Regular, mfontOptions)
                xsizeString = mobjGfx.MeasureString(strTitle, xfontTitle)
            Loop

            strPos = xGetPos(strTitle, xfontTitle, mintMargin, intHeight)

            mintStartY = strPos.Y + mintStringLineSpace + 2 * intHeight
            mobjPage.Height = CInt(mobjPage.Height) + mintStartY - mintMargin
            mintStartX = (mobjPage.Width.Point - mintTreeWidth) / 2

            mobjGfx.DrawString(strTitle, xfontTitle, XBrushes.Black, strPos)
        Catch
            Throw
        End Try
    End Sub

    '   ******************************************************************
    '		FUNCTION   : fncSetPageSize
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(Optional) - Double - Width Page (Default: 0)
    '                    ARG2(Optional) - Double - Height Page (Default: 0)
    '		MEMO       : Set Page Size
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function fncSetPageSize(Optional dblWidth As Double = 0, Optional dblHeight As Double = 0) As Boolean

        fncSetPageSize = False

        Try

            If dblWidth > mintA4Width Then

                mobjPage.Width = dblWidth

            Else

                mobjPage.Width = mintA4Width

            End If

            mintStartX = CInt((mobjPage.Width.Point - dblWidth) / 2)

            If dblHeight > mintA4Height Then

                mobjPage.Height = dblHeight '+ 130

            Else

                mobjPage.Height = mintA4Height '+ 40

            End If

            If CInt(mobjPage.Width.Point / mintA4) > 0 Then

                mintFontZoom = CInt(mobjPage.Width.Point / mintA4)

            End If

            If mintFontZoom > 2 Then mintFontZoom = 2

            mintMargin = mintFontZoom * mintMargin
            mintTitleFontSize = mintFontZoom * mintTitleFontSize
            mintStringLineSpace = mintStringLineSpace * mintFontZoom

            mobjPage.Height = CInt(mobjPage.Height) + 3 * mintMargin
            mobjPage.Width = CInt(mobjPage.Width) + 3 * mintMargin
            mintStartX = mintMargin
            mintStartY = CInt(1.5 * mintMargin)
            'mobjGfx = XGraphics.FromPdfPage(mobjPage)

            fncSetPageSize = True
        Catch
            Throw
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : fncCreatePageInfo
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(Optional) - Double - Width Page (Default: 0)
    '                    ARG2(Optional) - Double - Height Page (Default: 0)
    '		MEMO       : Create Page Info
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncCreatePageInfo(Optional dblWidth As Double = 0, Optional dblHeight As Double = 0) As Boolean

        fncCreatePageInfo = False

        Dim arrInfo() As String = Nothing    'Content Page Info
        Dim arrPos() As XPoint      'Position of Text
        Dim xfontTitle As XFont = New XFont(mcstrFontName, mintTitleFontSize, XFontStyle.Regular, mfontOptions)
        Dim intHeight As Integer
        Dim intStartY As Integer
        Dim strMax As String = ""
        Dim strValue As String = String.Empty

        Try

            'Create Array Info and Reduce fontsize if String too long
            If mstrFamilyInfo.Length > 0 Then
                strValue = xAddValueToArrayInfo(arrInfo, Trim(mstrFamilyInfo).ToUpper)
                If strMax.Length < strValue.Length Then strMax = strValue
            End If

            If mstrFamilyAnniInfo.Length > 0 Then
                strValue = xAddValueToArrayInfo(arrInfo, Trim(mstrFamilyAnniInfo).ToUpper)
                If strMax.Length < strValue.Length Then strMax = strValue
            End If

            If mstrRootInfo.Length > 0 Then
                strValue = xAddValueToArrayInfo(arrInfo, Trim(mstrRootInfo).ToUpper)
                If strMax.Length < strValue.Length Then strMax = strValue
            End If

            If mstrCreateDate.Length > 0 Then
                strValue = "Ngày Lập: " & Trim(mstrCreateDate)
                strValue = xAddValueToArrayInfo(arrInfo, strValue.ToUpper)
                If strMax.Length < strValue.Length Then strMax = strValue
            End If

            If mstrCreateMember.Length > 0 Then
                strValue = "Người Lập: " & Trim(mstrCreateMember)
                strValue = xAddValueToArrayInfo(arrInfo, strValue.ToUpper)
                If strMax.Length < strValue.Length Then strMax = strValue
            End If

            If Not IsNothing(arrInfo) AndAlso arrInfo.Length > 0 Then

                fncSetPageSize(dblWidth, dblHeight)

                Dim xsizeString As XSize = mobjGfx.MeasureString(strMax, xfontTitle)
                Do Until xsizeString.Width < mobjPage.Width.Point - 3 * mintMargin
                    xfontTitle = New XFont(mcstrFontName, xfontTitle.Size - 1, XFontStyle.Regular, mfontOptions)
                    xsizeString = mobjGfx.MeasureString(strMax, xfontTitle)
                Loop

                intStartY = CInt((CInt(mobjPage.Height.Point) - (xsizeString.Height * arrInfo.Length + mintStringLineSpace * (arrInfo.Length - 1)) - xsizeString.Height) / 2)
                ReDim arrPos(UBound(arrInfo))
                arrPos(0) = xGetPos(arrInfo(0), xfontTitle, intStartY, intHeight)
                mobjGfx.DrawString(arrInfo(0).ToUpper, xfontTitle, XBrushes.Black, arrPos(0))

                For i As Integer = 1 To UBound(arrInfo)
                    arrPos(i) = xGetPos(arrInfo(i), xfontTitle, arrPos(i - 1).Y + mintStringLineSpace, intHeight)
                    mobjGfx.DrawString(arrInfo(i).ToUpper, xfontTitle, XBrushes.Black, arrPos(i))
                Next

                If ShowBorder Then
                    mobjGfx.DrawRectangle(mobjPen, mintMargin, mintMargin, mobjPage.Width.Point - 2 * mintMargin, mobjPage.Height.Point - 2 * mintMargin)
                End If
            End If

            fncCreatePageInfo = True
        Catch
            Throw
        Finally
            arrInfo = Nothing
            arrPos = Nothing
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : fncAddNumberPage
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - Integer
    '		MEMO       : Add number page
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncAddNumberPage(ByVal intNumber As Integer) As Boolean

        fncAddNumberPage = False

        Dim xfont As XFont = New XFont(mcstrFontName, mintNomalFontSize, XFontStyle.Regular, mfontOptions)
        Dim xsizeString As XSize = mobjGfx.MeasureString(intNumber.ToString, xfont)
        Dim objPos As XPoint

        Try

            objPos.X = ((mobjPage.Width.Point - xsizeString.Width) / 2)

            Do Until xsizeString.Width < objPos.X
                xfont = New XFont(mcstrFontName, xfont.Size - 1, XFontStyle.Regular, mfontOptions)
                xsizeString = mobjGfx.MeasureString(intNumber.ToString, xfont)
                objPos.X = ((mobjPage.Width.Point - xsizeString.Width) / 2)
            Loop

            objPos.Y = CInt(mobjPage.Height.Point - Math.Ceiling((mintMargin - xsizeString.Height) / 2))

            mobjGfx.DrawString(intNumber.ToString, xfont, XBrushes.Black, objPos)

            fncAddNumberPage = True
        Catch
            Throw
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : fncAddTextLocation
    '		VALUE      : True - Suscess/False - Errors
    '		PARAMS     : ARG1(IN) - String - Text
    '                    ARG2(IN) - emLocation - Location
    '		MEMO       : Add Text to Location Input
    '		CREATE     : 2017/08/01 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Public Function fncAddTextLocation(ByVal strText As String,
                                       ByVal intLocation As emLocation) As Boolean

        fncAddTextLocation = False

        Dim intMargin As Integer
        Dim intWidthText As Integer
        Dim xfont As XFont = New XFont(mcstrFontName, mintNomalFontSize, XFontStyle.Regular, mfontOptions)
        Dim xsizeString As XSize = mobjGfx.MeasureString(strText, xfont)
        Dim objPos As XPoint

        Try

            If String.IsNullOrWhiteSpace(strText) Then Exit Function

            'Add text out border
            intMargin = Math.Ceiling((mintMargin - xsizeString.Height) / 2) + 1
            intWidthText = xsizeString.Width + intMargin

            Select Case intLocation
                Case emLocation.TopLeft
                    objPos.X = intMargin
                    objPos.Y = mintMargin - intMargin
                Case emLocation.TopRight
                    objPos.X = mobjPage.Width.Point - intWidthText
                    objPos.Y = mintMargin - intMargin
                Case emLocation.BottomLeft
                    objPos.X = intMargin
                    objPos.Y = mobjPage.Height.Point - intMargin
                Case emLocation.BottomRight
                    objPos.X = mobjPage.Width.Point - intWidthText
                    objPos.Y = mobjPage.Height.Point - intMargin
                Case Else
                    Exit Function
            End Select

            'Add text in border
            'intMargin = mintMargin + mintStringLineSpace
            'intWidthText = xsizeString.Width + intMargin

            'Select Case intLocation
            '    Case emLocation.TopLeft
            '        objPos.X = intMargin
            '        objPos.Y = intMargin + xsizeString.Height
            '    Case emLocation.TopRight
            '        objPos.X = mobjPage.Width.Point - intWidthText
            '        objPos.Y = intMargin + xsizeString.Height
            '    Case emLocation.BottomLeft
            '        objPos.X = intMargin
            '        objPos.Y = mobjPage.Height.Point - intMargin
            '    Case emLocation.BottomRight
            '        objPos.X = mobjPage.Width.Point - intWidthText
            '        objPos.Y = mobjPage.Height.Point - intMargin
            '    Case Else
            '        Exit Function
            'End Select

            mobjGfx.DrawString(strText, xfont, XBrushes.Black, objPos)

            fncAddTextLocation = True
        Catch
            Throw
        End Try
    End Function

    '   ******************************************************************
    '		FUNCTION   : xAddValueToArrayInfo
    '		VALUE      : STring - Value add Array
    '		PARAMS     : ARG1(OUT) - String() - Array Info
    '                    ARG2(IN) - String - Value add Array
    '		MEMO       : Add a value into Array Info
    '		CREATE     : 2017/06/29 AKB Nguyen Thanh Tung
    '		UPDATE     : 
    '   ******************************************************************
    Private Function xAddValueToArrayInfo(ByRef arrInfo() As String, ByVal strValue As String) As String

        xAddValueToArrayInfo = String.Empty

        Try

            If String.IsNullOrWhiteSpace(strValue) Then Exit Function

            If IsNothing(arrInfo) Then
                ReDim arrInfo(0)
                arrInfo(0) = strValue
            Else
                ReDim Preserve arrInfo(UBound(arrInfo) + 1)
                arrInfo(UBound(arrInfo)) = strValue
            End If

            xAddValueToArrayInfo = strValue
        Catch
            Throw
        End Try
    End Function
#End Region

    '       ******************************************************************
    '    　 FUNCTION   : fncDrawCard
    '       MEMO:
    '       CREATE     : 2012/01/07  AKB Nghia
    '       UPDATE:
    '       ******************************************************************
    Private Function fncDrawCard(ByVal objCard As usrMemCardBase, ByVal objImage As XImage) As Boolean

        Try
            Const intFontSize As Double = 8.25
            Const intMerginTop As Double = 15
            Dim fontOptions As XPdfFontOptions = New XPdfFontOptions(PdfFontEncoding.Unicode, PdfFontEmbedding.Always)

            Dim objCard1 As usrMemberCard1
            Dim dblStartX As Double
            Dim dblStartY As Double
            Dim dblStartTextY As Integer

            Dim strAvarta As String = ""
            'Dim strBirthDate As String = ""
            'Dim strDDate As String = ""
            'Dim strName As String = ""
            'Dim strAlias As String = ""
            Dim strInfo1 As String = ""
            Dim strInfo2 As String = ""
            Dim strInfo3 As String = ""
            Dim strInfo4 As String = ""

            Dim intAddH As Integer = 0
            Dim intImgW As Integer = 0
            Dim intImgH As Integer = 0

            objCard1 = CType(objCard, usrMemberCard1)

            ' ▽ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            Dim fontName As String = clsDefine.gcstrFontName
            Dim dbFontSize As Double = intFontSize

            If Not IsNothing(mobjFont) Then
                fontName = mobjFont.FontFamily.Name
                dbFontSize = CDbl(mobjFont.Size)
            End If

            Dim xfontTitle As XFont = New XFont(fontName, fncGetZoomValue(dbFontSize), XFontStyle.Bold, fontOptions)
            Dim xfontNomal As XFont = New XFont(fontName, fncGetZoomValue(dbFontSize), XFontStyle.Regular, fontOptions)
            'Dim xfontTitle As XFont = New XFont(clsDefine.gcstrFontName, fncGetZoomValue(intFontSize), XFontStyle.Bold, fontOptions)
            'Dim xfontNomal As XFont = New XFont(clsDefine.gcstrFontName, fncGetZoomValue(intFontSize), XFontStyle.Regular, fontOptions)
            ' △ 2018/04/27 AKB Nguyen Thanh Tung --------------------------------

            dblStartX = objCard1.CardCoor.X + mintStartX
            dblStartY = objCard1.CardCoor.Y + mintStartY

            ' ▽2018/04/24 AKB Nguyen Thanh Tung --------------------------------
            Dim bush As SolidBrush

            If My.Settings.blnShowBackgroupDie AndAlso Not objCard1.AliveStatus Then

                bush = New SolidBrush(My.Settings.objColorBackgroupCardDie)

            Else

                bush = New SolidBrush(My.Settings.objColorBackgroupCard)

            End If

            mobjGfx.DrawRectangle(bush, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))

            Dim arrName As String()

            If My.Settings.blnTypeCardShort Then

                arrName = (objCard1.CardName & "").Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
                mobjGfx.DrawRectangle(mobjPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))

                If IsNothing(arrName) OrElse arrName.Length = 0 Then Exit Function

                Dim dbTop As Double

                Dim objPos As New XPoint()

                Using objPic As New Bitmap(1, 1)

                    Using objGrap As Graphics = Graphics.FromImage(objPic)

                        Dim sizeText1 As Size = objGrap.MeasureString(objCard1.CardName.Trim(), mobjFont).ToSize()
                        Dim sizePading1 As Size = objGrap.MeasureString("K", mobjFont).ToSize()

                        If My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Horizontal _
                            AndAlso My.Settings.intTypeDrawText <> CInt(clsEnum.emTypeDrawText.Normal) Then

                            If My.Settings.intTypeDrawText = CInt(clsEnum.emTypeDrawText.RotateLeft) Then

                                objPos.X = fncPdfMetric(CInt(dblStartX) + CInt(mobjFont.Height * 0.3))
                                objPos.Y = fncPdfMetric(CInt(dblStartY) + CInt(sizeText1.Width * 0.03))

                                mobjGfx.RotateAtTransform(90, objPos)
                                mobjGfx.DrawString(objCard1.CardName, xfontNomal, mbushText, objPos)
                                mobjGfx.RotateAtTransform(-90, objPos)
                            ElseIf My.Settings.intTypeDrawText = CInt(clsEnum.emTypeDrawText.RotateRight) Then

                                objPos.X = fncPdfMetric(CInt(dblStartX) + objCard1.Width - CInt(mobjFont.Height * 0.35))
                                objPos.Y = fncPdfMetric(CInt(dblStartY) + objCard1.Height - CInt(sizeText1.Width * 0.03))

                                mobjGfx.RotateAtTransform(-90, objPos)
                                mobjGfx.DrawString(objCard1.CardName, xfontNomal, mbushText, objPos)
                                mobjGfx.RotateAtTransform(90, objPos)
                            End If

                            Return True
                        End If

                        dbTop = dblStartY + sizePading1.Width + (objCard1.Height - sizeText1.Height - UBound(arrName)) / 2

                        If My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Vertical Then dbTop -= sizePading1.Width * 0.25

                        For i As Integer = 0 To UBound(arrName)

                            If String.IsNullOrEmpty(arrName(i)) Then Continue For

                            sizeText1 = objGrap.MeasureString(arrName(i).Trim(), mobjFont).ToSize()
                            'objPos.X = fncPdfMetric(CInt(dblStartX + sizePading.Width + (objCard1.Width - sizeText.Width) / 2))
                            objPos.X = dblStartX + sizePading1.Width * 0.25 + (objCard1.Width - sizeText1.Width) / 2
                            objPos.Y = dbTop

                            objPos.X = objPos.X * 0.75
                            objPos.Y = objPos.Y * 0.75

                            mobjGfx.DrawString(arrName(i), xfontNomal, mbushText, objPos)

                            dbTop += sizePading1.Height + 1
                        Next
                    End Using
                End Using

                Return True
            End If
            ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------

            If My.Settings.intCardSize = CInt(clsEnum.emCardSize.LARGE) Then

                If My.Settings.strCard1Bg <> "" Then

                    If System.IO.File.Exists(My.Settings.strCard1Bg) Then

                        fncDrawCardAvatar(mobjGfx, fncPdfMetric(dblStartX), fncPdfMetric(dblStartY), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height), mCardBg)

                    End If

                Else

                    If Not IsNothing(mCardBg) Then
                        fncDrawCardAvatar(mobjGfx, fncPdfMetric(dblStartX), fncPdfMetric(dblStartY), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height), mCardBg)
                    Else
                        mobjGfx.DrawRectangle(mobjPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))
                    End If

                    'mobjGfx.DrawRectangle(mobjPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))

                End If

                strAvarta = GetMemberImagePath(objCard1)

                If objCard1.CardImageLocation <> "" Then
                    intImgW = CInt(fncGetZoomValue(objCard1.CardImage.Width))
                    intImgH = CInt(fncGetZoomValue(objCard1.CardImage.Height))
                Else
                    intImgW = CInt(fncGetZoomValue(clsDefine.THUMBNAIL_W))
                    intImgH = CInt(fncGetZoomValue(clsDefine.THUMBNAIL_H))
                End If

                'draw image
                If strAvarta <> "" Then

                    fncDrawCardAvatar(mobjGfx, fncPdfMetric(CInt(dblStartX + (objCard1.Width - intImgW) / 2)), fncPdfMetric(CInt(dblStartY + fncGetZoomValue(intMerginTop))), fncPdfMetric(intImgW), fncPdfMetric(intImgH), objImage)

                End If

            Else


                If My.Settings.strCard1Bg <> "" Then

                    If System.IO.File.Exists(My.Settings.strCard1Bg) Then

                        fncDrawCardAvatar(mobjGfx, fncPdfMetric(dblStartX), fncPdfMetric(dblStartY), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height), mCardBg)

                    End If

                Else

                    If Not IsNothing(mCardBg) Then
                        fncDrawCardAvatar(mobjGfx, fncPdfMetric(dblStartX), fncPdfMetric(dblStartY), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height), mCardBg)
                    Else
                        mobjGfx.DrawRectangle(mobjPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))
                    End If

                    'mobjGfx.DrawRectangle(mobjPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))

                End If


            End If


            'strBirthDate = objCard1.CardBirth
            'strBirthDate = ""
            'strDDate = objCard1.CardDeath
            'strDDate = ""
            ' ▽2018/04/24 AKB Nguyen Thanh Tung --------------------------------
            arrName = objCard1.CardName.Split(CChar(vbCrLf))
            'Dim arrName As String() = objCard1.CardName.Split(CChar(vbCrLf))
            ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------

            If arrName.Length > 0 Then
                'strName = arrName(0).Trim(CChar(vbCrLf))
                strInfo1 = arrName(0).Trim(CChar(vbCrLf))
            End If

            If arrName.Length > 1 Then
                strInfo2 = arrName(1).Trim(CChar(vbCrLf))
                strInfo2 = strInfo2.Trim(CChar(vbCr))
                strInfo2 = strInfo2.Trim(CChar(vbLf))
            End If

            If arrName.Length > 2 Then
                strInfo3 = arrName(2).Trim(CChar(vbCrLf))
                strInfo3 = strInfo3.Trim(CChar(vbCr))
                strInfo3 = strInfo3.Trim(CChar(vbLf))
            End If

            If arrName.Length > 3 Then
                strInfo4 = arrName(3).Trim(CChar(vbCrLf))
                strInfo4 = strInfo4.Trim(CChar(vbCr))
                strInfo4 = strInfo4.Trim(CChar(vbLf))
            End If

            dblStartTextY = CInt(dblStartY + intImgH + fncGetZoomValue(6))

            'draw name
            functionWriteText(strInfo1, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontTitle, intAddH)
            dblStartTextY = dblStartTextY + intAddH

            'draw alias
            If strInfo2 <> "" Then
                functionWriteText(strInfo2, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontTitle, intAddH)
                dblStartTextY = dblStartTextY + intAddH
            End If


            'draw birthDay
            If strInfo3 <> "" Then
                functionWriteText(strInfo3, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontNomal, intAddH)
                dblStartTextY = dblStartTextY + intAddH
            End If


            'draw DeathDay
            If strInfo4 <> "" Then
                functionWriteText(strInfo4, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontNomal, intAddH)
                dblStartTextY = dblStartTextY + intAddH
            End If

        Catch ex As Exception

        End Try

    End Function



    '   ******************************************************************
    '　　　FUNCTION   : functionWriteText
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function functionWriteText(ByVal strText As String, _
                                       ByVal intStartX As Integer, _
                                       ByVal intStartY As Integer, _
                                       ByVal intWidth As Integer, _
                                       ByVal objfontTitle As XFont, _
                                       ByRef intHeight As Integer) As Boolean
        Try

            Dim objPos As XPoint
            Dim objTempFont As XFont
            objTempFont = objfontTitle
            objPos = xGetPos(mobjGfx, fncPdfMetric(intStartX), fncPdfMetric(intStartY), fncPdfMetric(intWidth), strText, objTempFont, intHeight)

            mobjGfx.DrawString(strText, objTempFont, mbushText, objPos)

        Catch ex As Exception

        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xGetPos
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetPos(ByVal objGfx As XGraphics, _
                             ByVal intStartX As Integer, _
                             ByVal intStartY As Integer, _
                             ByVal intWidth As Integer, _
                             ByVal strInfo As String, _
                             ByRef charFont As XFont, _
                             ByRef intHeight As Integer) As XPoint

        Const cintAddHeight As Integer = 5
        If strInfo = "" Then strInfo = "A"


        Dim xsizeString As XSize = objGfx.MeasureString(strInfo, charFont)


        Do Until xsizeString.Width < intWidth - fncGetZoomValue(9)
            charFont = New XFont(charFont.Name, charFont.Size - 1, charFont.Style, charFont.PdfOptions)
            xsizeString = objGfx.MeasureString(strInfo, charFont)
        Loop

        intHeight = CInt(xsizeString.Height + fncGetZoomValue(cintAddHeight))
        Dim xpPos As XPoint = New XPoint(intStartX + (CInt(intWidth) - CInt(xsizeString.Width)) / 2, intStartY + intHeight)

        Return xpPos

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncDrawCardAvatar
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDrawCardAvatar(ByVal objGfx As XGraphics, _
                                      ByVal dblStartX As Double, ByVal dblStartY As Double, _
                                      ByVal dblPic_W As Double, ByVal dblPic_H As Double, _
                                      ByVal imgAvarta As XImage) As Boolean

        Try


            Dim dblImgWidth As Double
            Dim dblImgHeight As Double
            If Not IsNothing(imgAvarta) Then

                dblImgWidth = dblPic_W
                dblImgHeight = dblPic_H
                'If objImg.Width * dblPic_H > objImg.Height * dblPic_W Then
                '    dblImgWidth = dblPic_W
                '    dblImgHeight = objImg.Height * dblPic_W / objImg.Width

                'Else
                '    dblImgWidth = objImg.Width * dblPic_H / objImg.Height
                '    dblImgHeight = dblPic_H
                'End If


                objGfx.DrawImage(imgAvarta, dblStartX, dblStartY, dblImgWidth, dblImgHeight)


            End If


        Catch ex As Exception

        End Try

    End Function

    Private Function PdfMetric(ByVal intValue As Integer) As Integer

        Dim intDPI As Integer = CInt(intValue * 0.75)
        Return intDPI

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector, draw lines
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : lstNormalLine   List
    '      PARAMS     : lstSpecialLine  List
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawConnector(ByVal lstNormalLine As List(Of usrLine), ByVal lstSpecialLine As List(Of usrLine)) As Boolean

        xDrawConnector = False

        Try
            'Dim penBlack As XPen
            'Dim penRed As XPen
            'Dim ptStart As Point
            'Dim ptEnd As Point

            'penBlack = New XPen(XColor.FromArgb(0, 0, 0), 2)
            'penRed = New XPen(XColor.FromArgb(255, 0, 0), 3)

            'fncDrawPdfConnector(mobjGfx, lstNormalLine, New XPen(XColor.FromArgb(0, 0, 0), 2), mintStartX, mintStartY)
            'fncDrawPdfConnector(mobjGfx, lstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), 3), mintStartX, mintStartY)

            'draw normal line
            'For i As Integer = 0 To lstNormalLine.Count - 1

            '    ptStart = lstNormalLine(i).Location
            '    ptStart.X += mintStartX
            '    ptStart.Y += mintStartY

            '    ptEnd = ptStart

            '    If lstNormalLine(i).LineDirection = clsEnum.emLineDirection.HORIZONTAL Then
            '        ptEnd.X += lstNormalLine(i).Width
            '    Else
            '        ptEnd.Y += lstNormalLine(i).Height
            '    End If

            '    mobjGfx.DrawLine(penBlack, PdfMetric(ptStart.X), PdfMetric(ptStart.Y), PdfMetric(ptEnd.X), PdfMetric(ptEnd.Y))

            'Next

            ''draw special line
            'For i As Integer = 0 To lstSpecialLine.Count - 1

            '    ptStart = lstSpecialLine(i).Location
            '    ptStart.X += mintStartX
            '    ptStart.Y += mintStartY

            '    ptEnd = ptStart

            '    If lstSpecialLine(i).LineDirection = clsEnum.emLineDirection.HORIZONTAL Then
            '        ptEnd.X += lstSpecialLine(i).Width
            '    Else
            '        ptEnd.Y += lstSpecialLine(i).Height
            '    End If

            '    mobjGfx.DrawLine(penRed, ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y)

            'Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Sub fncExportToImage()
        Dim imageCount As Integer

        'Iterate pages
        For Each page As PdfPage In mobjDocument.Pages

            ' Get resources dictionary
            Dim resources As PdfDictionary = page.Elements.GetDictionary("/Resources")

            If Not resources Is Nothing Then
                ' Get external objects dictionary
                Dim xObjects As PdfDictionary = resources.Elements.GetDictionary("/XObject")
                If Not xObjects Is Nothing Then
                    Dim items As ICollection(Of PdfItem) = xObjects.Elements.Values

                    'Iterate references to external objects
                    For Each item As PdfItem In items

                        Dim reference As PdfReference = CType(item, PdfReference)
                        If (Not reference Is Nothing) Then
                            Dim xObject As PdfDictionary = CType(reference.Value, PdfDictionary)

                            ' Is external object an image?
                            If Not xObject Is Nothing And xObject.Elements.GetString("/Subtype") = "/Image" Then
                                xExportImage(xObject, imageCount)
                            End If
                        End If
                    Next
                End If
            End If
        Next
    End Sub

    Private Sub xExportImage(ByVal image As PdfDictionary, ByRef count As Integer)
        Dim filter As String = image.Elements.GetName("/Filter")
        Select Case filter
            Case "/DCTDecode"
                xExportJpegImage(image, count)
                Exit Select

            Case "/FlateDecode"
                xExportAsPngImage(image, count)
                Exit Select
        End Select
    End Sub

    Private Sub xExportJpegImage(ByVal image As PdfDictionary, ByRef count As Integer)

        'Fortunately JPEG has native support in PDF and exporting an image is just writing the stream to a file.
        Dim stream As Byte() = image.Stream.Value

        Dim fs As System.IO.FileStream = New System.IO.FileStream("C:\" & String.Format("Image{0}.jpeg", count + 1), System.IO.FileMode.Create, System.IO.FileAccess.Write)
        Dim bw As System.IO.BinaryWriter = New System.IO.BinaryWriter(fs)
        bw.Write(stream)
        bw.Close()

    End Sub


    Private Sub xExportAsPngImage(ByVal image As PdfDictionary, ByRef count As Integer)
        'Dim width As Integer = image.Elements.GetInteger(PdfImage.Keys.Width)
        'Dim height As Integer = image.Elements.GetInteger(PdfImage.Keys.Height)
        'Dim bitsPerComponent As Integer = image.Elements.GetInteger(PdfImage.Keys.BitsPerComponent)

        'Dim flate As PdfSharp.Pdf.Filters.FlateDecode = New PdfSharp.Pdf.Filters.FlateDecode()
        'Dim decodedBytes As Byte() = flate.Decode()

        'Dim pixelFormat As System.Drawing.Imaging.PixelFormat

        'Select Case bitsPerComponent
        '    Case 1
        '        pixelFormat = pixelFormat.Format1bppIndexed
        '        Exit Select
        '    Case 8
        '        pixelFormat = pixelFormat.Format8bppIndexed
        '        Exit Select
        '    Case 24
        '        pixelFormat = pixelFormat.Format24bppRgb
        '        Exit Select
        '    Case Else
        '        Throw New Exception("Unknown pixel format " + bitsPerComponent)
        'End Select


        'Dim bmp As Bitmap = New Bitmap(width, height, pixelFormat)
        'Dim bmpData = bmp.LockBits(New Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, pixelFormat)

        'Dim length As Integer = CInt(Math.Ceiling(width * bitsPerComponent / 8.0))
        'For i As Integer = 0 To height - 1
        '    Dim offset As Integer = i * length
        '    Dim scanOffset As Integer = i * bmpData.Stride
        '    Marshal.Copy(decodedBytes, offset, New IntPtr(bmpData.Scan0.ToInt32() + scanOffset), length)
        'Next

        'bmp.UnlockBits(bmpData);
        'using (FileStream fs = new FileStream(@"C:\Export\PdfSharp\" + String.Format("Image{0}.png", count), FileMode.Create, FileAccess.Write))
        '{
        '    bmp.Save(fs, System.Drawing.Imaging.ImageFormat.Png);
        '}

    End Sub




    ' TODO: You can put the code here that converts vom PDF internal image format to a Windows bitmap
    ' and use GDI+ to save it in PNG format.
    ' It is the work of a day or two for the most important formats. Take a look at the file
    ' PdfSharp.Pdf.Advanced/PdfImage.cs to see how we create the PDF image formats.
    ' We don't need that feature at the moment and therefore will not implement it.
    ' If you write the code for exporting images I would be pleased to publish it in a future release
    ' of PDFsharp.




#Region " IDisposable Support - Add by:2017/07/27 AKB Nguyen Thanh Tung"
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)

        If disposing Then
            ' TODO: free unmanaged resources when explicitly called
        End If

    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class
