'   ******************************************************************
'      TITLE      : MAIN FORM
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/09/14　AKB 
'      UPDATE     : 
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

Imports PdfSharp
Imports PdfSharp.Drawing
Imports PdfSharp.Pdf
Imports PdfSharp.Pdf.IO
Imports System.Drawing.Printing

Public Class PrintPreview

    Private mcstrClsName As String = "PrintPreview"
    'Private mobjGfx As XGraphics
    'Private mobjPen As XPen
    ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
    'Private mobjFont As XFont = New PdfSharp.Drawing.XFont("Arial", 20, XFontStyle.Bold)
    ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------
    Private mobjCardLeft As usrMemberCard1                                          'temporary card
    Private mobjCardRight As usrMemberCard1                                         'temporary card
    Private mintStartY As Integer = 10
    Private mintStartX As Integer = 10

    Private mintEndY As Integer = 10

    Private mintA4 As Integer = 2500
    Private mintA4Width As Integer = 846
    Private mintA4Height As Integer = 600

    Private mblnIsSmallCard As Boolean                                              'draw small card
    'Private mintMEM_CARD_SPACE_LEFT As Integer                                      'margin left
    'Private mintMEM_CARD_SPACE_DOWN As Integer                                      'margin bottom
    'Private mintMEM_CARD_W As Integer                                               'card width
    'Private mintMEM_CARD_H As Integer                                               'card height
    Private mintMaxH As Integer
    Private mintMaxW As Integer

    Private mintMaxHMM As Integer
    Private mintMaxWMM As Integer

    'Private mobjDraw1 As clsDrawTree1
    Private mobjDraw1 As clsDrawTreeS1
    Private mobjDraw2 As clsDrawTreeS2
    Private mobjDraw3 As clsDrawTreeS3
    Private mobjDrawSS As clsDrawTreeSS
    Private mobjDrawA1 As clsDrawTreeA1
    Private mstExportInfo As basConst.stExportInfo

    Private mtblControl As Hashtable
    Public mlstNormalLine As List(Of usrLine)
    Public mlstSpecialLine As List(Of usrLine)
    Private mobjTreeDraw As Object
    Private mblnMenual As Boolean = False
    Private mstrAutosize As String = ""
    Private mstrRootInfo As String = ""

    Private mcintCboPageSize() As PageSize = {0, PageSize.A0, PageSize.A1, PageSize.A2, PageSize.A3, PageSize.A4, PageSize.A5}
    Private mintMouseCanvasClick As Point

    Public mobjImage() As XImage
    Public mobjCard() As usrMemCardBase

    Private mctlCanvas As Control
    Public mintMaxX As Integer = -1
    Public mintMinX As Integer = Integer.MaxValue

    Private mNoAvatar_Img As XImage
    Private mNoAvatar_F_Img As XImage
    Private mUnKnowAvatar_F_Img As XImage
    Private mCardBg As XImage

    ' ▽ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------
    Public ReadOnly Property TreeDraw As Object
        Get
            TreeDraw = mobjTreeDraw
        End Get
    End Property
    ' △ 2017/06/29 AKB Nguyen Thanh Tung --------------------------------

    ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
    Private mbushText As New SolidBrush(My.Settings.objColorText)
    Private mobjFont As Font
    Private mfrmMain As frmMain
    Public mblnUpdatePrint As Boolean = False
    Public mintPrintPageSizeSelected As Integer
    Public mintPrintPageZoomSelected As Integer
    Public mblnPrintPageLandScape As Boolean

    Public Property FontUser As Font
        Get
            Return mobjFont
        End Get
        Set(value As Font)
            If IsNothing(value) Then value = My.Settings.objFontDefaut
            mobjFont = value
        End Set
    End Property

    Public WriteOnly Property MainFrom As frmMain
        Set(value As frmMain)
            mfrmMain = value
        End Set
    End Property
    ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    'Public Sub Run(ByVal objTreeDraw As Object)
    '    'Dim objCard As usrMemCardBase
    '    Dim intId As Integer

    '    Dim strPath As String
    '    Dim strFolder As String = ""
    '    Dim strDefault As String
    '    Try

    '        mNoAvatar_Img = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\no_avatar_m.jpg")
    '        mNoAvatar_F_Img = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\no_avatar_f.jpg")
    '        mUnKnowAvatar_F_Img = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\UnknownMember.jpg")

    '        If My.Settings.strCard1Bg = "" Then
    '            strDefault = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & gcstrDefaultFrame
    '        Else
    '            strDefault = My.Settings.strCard1Bg
    '        End If
    '        mCardBg = fncMakeImage(strDefault)

    '        mobjTreeDraw = objTreeDraw


    '        mstExportInfo = xSetDataDraw(objTreeDraw)

    '        mtblControl = mstExportInfo.tblControl
    '        mlstNormalLine = mstExportInfo.lstNormalLine
    '        mlstSpecialLine = mstExportInfo.lstSpecialLine

    '        strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder
    '        'create temp folder
    '        If Not basCommon.fncCreateFolder(strFolder, True) Then Exit Sub

    '        Dim i As Integer
    '        Dim objCard As usrMemCardBase

    '        ReDim mobjImage(mtblControl.Count - 1)
    '        ReDim mobjCard(mtblControl.Count - 1)

    '        i = -1

    '        For Each element As DictionaryEntry In mtblControl
    '            objCard = CType(element.Value, usrMemCardBase)
    '            If objCard.Visible = True Then
    '                i = i + 1
    '                intId = CInt(element.Key)
    '                mobjCard(i) = objCard
    '                If mintMinX > objCard.Location.X Then
    '                    mintMinX = objCard.Location.X
    '                End If

    '                If mintMaxX < objCard.Location.X + objCard.Width Then

    '                    mintMaxX = objCard.Location.X + objCard.Width

    '                End If

    '                strPath = mobjCard(i).fncGetImage(strFolder)

    '                If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD2 Then
    '                    mobjImage(i) = XImage.FromFile(strPath)
    '                Else
    '                    mobjImage(i) = xGetMemberAvatarImage(CType(mobjCard(i), usrMemberCard1))
    '                End If

    '            End If
    '        Next

    '        'mstrAutosize = "Tự động (" + CInt((mintMaxW + mintStartX) / 4).ToString + " x " + CInt((mintMaxH + mintStartY + mintEndY) / 4).ToString + "mm)"
    '        mstrAutosize = "Tự động (" + (fncPdfMetric(mintMaxWMM)).ToString + " x " + (fncPdfMetric(mintMaxHMM)).ToString + "mm)"
    '        cboPagesize.Items(0) = mstrAutosize

    '        mctlCanvas = pagePreview.Controls(0)

    '        mintMouseCanvasClick = New Point(-1000, -1000)

    '        xAddHandlers(mctlCanvas)

    '        Dim objRender As PdfSharp.Forms.PagePreview.RenderEvent = New PdfSharp.Forms.PagePreview.RenderEvent(AddressOf Render)
    '        pagePreview.SetRenderEvent(objRender)

    '        Me.ShowDialog()

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "Run", ex)
    '    End Try
    'End Sub
    Public Sub Run(ByVal objTreeDraw As Object)
        'Dim objCard As usrMemCardBase
        Dim intId As Integer

        Dim strPath As String
        Dim strFolder As String = ""
        Try

            mNoAvatar_Img = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\no_avatar_m.jpg")
            mNoAvatar_F_Img = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\no_avatar_f.jpg")
            mUnKnowAvatar_F_Img = fncMakeImage(My.Application.Info.DirectoryPath & "\docs\UnknownMember.jpg")
            mCardBg = fncMakeImage(My.Settings.strCard1Bg)

            mobjTreeDraw = objTreeDraw


            mstExportInfo = xSetDataDraw(objTreeDraw)

            mtblControl = mstExportInfo.tblControl
            mlstNormalLine = mstExportInfo.lstNormalLine
            mlstSpecialLine = mstExportInfo.lstSpecialLine

            strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder
            'create temp folder
            If Not basCommon.fncCreateFolder(strFolder, True) Then Exit Sub

            Dim i As Integer
            Dim objCard As usrMemCardBase

            ReDim mobjImage(mtblControl.Count - 1)
            ReDim mobjCard(mtblControl.Count - 1)

            i = -1

            For Each element As DictionaryEntry In mtblControl
                objCard = CType(element.Value, usrMemCardBase)
                If objCard.Visible = True Then
                    i = i + 1
                    intId = CInt(element.Key)
                    mobjCard(i) = objCard
                    If mintMinX > objCard.CardCoor.X Then
                        mintMinX = objCard.CardCoor.X
                    End If

                    If mintMaxX < objCard.CardCoor.X + objCard.Width Then

                        mintMaxX = objCard.CardCoor.X + objCard.Width

                    End If

                    strPath = mobjCard(i).fncGetImage(strFolder)

                    If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD2 Then
                        mobjImage(i) = XImage.FromFile(strPath)
                    Else
                        mobjImage(i) = xGetMemberAvatarImage(CType(mobjCard(i), usrMemberCard1))
                    End If

                End If
            Next

            'mstrAutosize = "Tự động (" + CInt((mintMaxW + mintStartX) / 4).ToString + " x " + CInt((mintMaxH + mintStartY + mintEndY) / 4).ToString + "mm)"
            mstrAutosize = "Tự động (" + (fncPdfMetric(mintMaxWMM)).ToString + " x " + (fncPdfMetric(mintMaxHMM)).ToString + "mm)"
            cboPagesize.Items(0) = mstrAutosize

            mctlCanvas = pagePreview.Controls(0)

            mintMouseCanvasClick = New Point(-1000, -1000)

            xAddHandlers(mctlCanvas)

            Dim objRender As PdfSharp.Forms.PagePreview.RenderEvent = New PdfSharp.Forms.PagePreview.RenderEvent(AddressOf Render)
            pagePreview.SetRenderEvent(objRender)

            Me.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "Run", ex)
        End Try
    End Sub



    Sub xAddHandlers(ByVal inputObject As Control)
        AddHandler inputObject.MouseDown, AddressOf xCanvasMouseDown
        AddHandler inputObject.MouseUp, AddressOf xCanvasMouseUp
        AddHandler inputObject.MouseHover, AddressOf xCanvasMouseHover
        AddHandler inputObject.MouseMove, AddressOf xCanvasMouseMove
    End Sub

    Public Sub xCanvasMouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)
        Try

            mintMouseCanvasClick = New Point(e.X, e.Y)

        Catch ex As Exception

        End Try
    End Sub

    Private Sub xCanvasMouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        mintMouseCanvasClick = New Point(-1000, -1000)

    End Sub

    Private Sub xCanvasMouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        If mintMouseCanvasClick.X >= 0 And mintMouseCanvasClick.Y >= 0 Then

            Dim intValue As Integer
            Dim objVScroll As VScrollBar = CType(pagePreview.Controls(2), VScrollBar)
            Dim objHScroll As HScrollBar = CType(pagePreview.Controls(1), HScrollBar)

            intValue = objVScroll.Value - e.Y + mintMouseCanvasClick.Y
            If intValue >= objVScroll.Minimum And intValue <= objVScroll.Maximum Then

                objVScroll.Value = intValue

            End If

            intValue = objHScroll.Value - e.X + mintMouseCanvasClick.X

            If intValue >= objHScroll.Minimum And intValue <= objHScroll.Maximum Then

                objHScroll.Value = intValue

            End If

            mintMouseCanvasClick = New Point(e.X, e.Y)

        End If

    End Sub

    Private Sub xCanvasMouseHover(ByVal sender As System.Object, ByVal e As System.EventArgs)

        mctlCanvas.Cursor = Cursors.Hand

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    'Private Sub xPdfDraw(ByVal gfx As XGraphics)

    '    Dim intId As Integer
    '    Try
    '        'mobjGfx = gfx

    '        'draw connector first so that it will be set to back
    '        'If Not xDrawConnector(mobjDraw.DrawingCard, mobjDraw.NotDrawingCard) Then Return
    '        'If Not xDrawConnector(gfx, mlstNormalLine, mlstSpecialLine) Then Return

    '        fncDrawPdfConnector(gfx, mlstNormalLine, New XPen(XColor.FromArgb(0, 0, 0), 1), mintStartX, mintStartY)
    '        fncDrawPdfConnector(gfx, mlstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), 2), mintStartX, mintStartY)

    '        For intId = 0 To mobjCard.Length - 1
    '            mobjImage(intId).Interpolate = False
    '            gfx.DrawImage(mobjImage(intId), fncPdfMetric(mobjCard(intId).Location.X + mintStartX), fncPdfMetric(mobjCard(intId).Location.Y + mintStartY))

    '        Next

    '        Return
    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "Render", ex)
    '    Finally
    '        'delete temp folder after using
    '        'basCommon.fncDeleteFolder(strFolder)
    '    End Try
    'End Sub

    Private Sub xPdfDraw(ByVal gfx As XGraphics)

        Dim intId As Integer
        Try
            'mobjGfx = gfx

            'draw connector first so that it will be set to back
            'If Not xDrawConnector(mobjDraw.DrawingCard, mobjDraw.NotDrawingCard) Then Return
            'If Not xDrawConnector(gfx, mlstNormalLine, mlstSpecialLine) Then Return

            fncDrawPdfConnector(gfx, mlstNormalLine, New XPen(XColor.FromArgb(0, 0, 0), 1), mintStartX, mintStartY)
            fncDrawPdfConnector(gfx, mlstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), 2), mintStartX, mintStartY)

            For intId = 0 To mobjCard.Length - 1
                mobjImage(intId).Interpolate = False
                gfx.DrawImage(mobjImage(intId), fncPdfMetric(mobjCard(intId).CardCoor.X + mintStartX), fncPdfMetric(mobjCard(intId).CardCoor.Y + mintStartY))
            Next

            Return
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "Render", ex)
        Finally
            'delete temp folder after using
            'basCommon.fncDeleteFolder(strFolder)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xPdfDrawVector
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xPdfDrawVector(ByVal gfx As XGraphics)

        Dim intId As Integer
        Try

            fncDrawPdfConnector(gfx, mlstNormalLine, New XPen(XColor.FromArgb(0, 0, 0), 1), mintStartX, mintStartY)
            fncDrawPdfConnector(gfx, mlstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), 2), mintStartX, mintStartY)

            For intId = 0 To mobjCard.Length - 1

                Dim stCard As stCardInfo

                Dim objPen As XPen

                objPen = New XPen(XColor.FromArgb(0, 0, 0), 1)

                stCard = CType(mstExportInfo.tblMemberInfo(mobjCard(intId).CardID), stCardInfo)

                'draw card to pdf file
                fncDrawCard(gfx, objPen, stCard, mobjCard(intId), intId)


            Next

            Return
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "Render", ex)
        Finally
            'delete temp folder after using
            'basCommon.fncDeleteFolder(strFolder)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xGetMemberAvatarImage
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Public Function xGetMemberAvatarImage(ByVal objCard As usrMemberCard1) As XImage



        xGetMemberAvatarImage = mNoAvatar_Img
        Try

            If objCard.CardImageLocation() <> "" Then Return XImage.FromFile(objCard.CardImageLocation)

            If objCard.CardGender = clsEnum.emGender.FEMALE Then

                Return mNoAvatar_F_Img

            ElseIf objCard.CardGender = clsEnum.emGender.UNKNOW Then

                Return mUnKnowAvatar_F_Img

            End If
        Catch ex As Exception

        End Try


    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xGetCardImageLocation
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetCardImageLocation(ByVal objCard As usrMemCardBase) As String
        xGetCardImageLocation = ""
        Try

            Return DirectCast(objCard, usrMemberCard1).CardImageLocation

        Catch ex As Exception

        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncDrawCard
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function fncDrawCard(ByVal objGfx As XGraphics, ByVal objPen As XPen, _
                                ByRef stCard As stCardInfo, ByVal objCard As usrMemCardBase, ByVal intIndex As Integer) As Boolean

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

            Dim strInfo1 As String = ""
            Dim strInfo2 As String = ""
            Dim strInfo3 As String = ""
            Dim strInfo4 As String = ""

            'Dim strAlias As String = ""

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

            objGfx.DrawRectangle(bush, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))

            Dim arrName As String()

            If My.Settings.blnTypeCardShort Then

                arrName = (objCard1.CardName & "").Split({vbCrLf}, StringSplitOptions.RemoveEmptyEntries)
                objGfx.DrawRectangle(objPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))

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

                                objPos.X = fncPdfMetric(CInt(dblStartX) + CInt(mobjFont.Height * 0.35))
                                objPos.Y = fncPdfMetric(CInt(dblStartY) + CInt(sizeText1.Width * 0.03))

                                objGfx.RotateAtTransform(90, objPos)
                                objGfx.DrawString(objCard1.CardName, xfontNomal, mbushText, objPos)
                                objGfx.RotateAtTransform(-90, objPos)
                            ElseIf My.Settings.intTypeDrawText = CInt(clsEnum.emTypeDrawText.RotateRight) Then

                                objPos.X = fncPdfMetric(CInt(dblStartX) + objCard1.Width - CInt(mobjFont.Height * 0.35))
                                objPos.Y = fncPdfMetric(CInt(dblStartY) + objCard1.Height - CInt(sizeText1.Width * 0.03))

                                objGfx.RotateAtTransform(-90, objPos)
                                objGfx.DrawString(objCard1.CardName, xfontNomal, mbushText, objPos)
                                objGfx.RotateAtTransform(90, objPos)
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

                            objGfx.DrawString(arrName(i), xfontNomal, mbushText, objPos)

                            dbTop += sizePading1.Height + 1
                        Next
                    End Using
                End Using

                Return True
            End If
            ' △2018/04/24 AKB Nguyen Thanh Tung --------------------------------

            If My.Settings.intCardSize = CInt(clsEnum.emCardSize.LARGE) Then
                'draw border 
                If My.Settings.strCard1Bg <> "" Then
                    If System.IO.File.Exists(My.Settings.strCard1Bg) Then

                        fncDrawCardAvatar(objGfx, fncPdfMetric(objCard1.CardCoor.X + mintStartX), _
                                          fncPdfMetric(objCard1.CardCoor.Y + mintStartY), fncPdfMetric(objCard1.Width), _
                                          fncPdfMetric(objCard1.Height), mCardBg)

                    End If

                Else
                    If Not IsNothing(mCardBg) Then
                        fncDrawCardAvatar(objGfx, fncPdfMetric(objCard1.CardCoor.X + mintStartX), _
                                          fncPdfMetric(objCard1.CardCoor.Y + mintStartY), fncPdfMetric(objCard1.Width), _
                                          fncPdfMetric(objCard1.Height), mCardBg)
                    Else
                        objGfx.DrawRectangle(objPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))
                    End If


                End If

                strAvarta = basCommon.GetMemberImagePath(objCard1)

                If objCard1.CardImageLocation <> "" Then
                    intImgW = CInt(fncGetZoomValue(objCard1.CardImage.Width))
                    intImgH = CInt(fncGetZoomValue(objCard1.CardImage.Height))
                Else
                    intImgW = CInt(fncGetZoomValue(clsDefine.THUMBNAIL_W))
                    intImgH = CInt(fncGetZoomValue(clsDefine.THUMBNAIL_H))
                End If

                'draw image
                If strAvarta <> "" Then

                    fncDrawCardAvatar(objGfx, fncPdfMetric(CInt(dblStartX + (objCard1.Width - intImgW) / 2)), fncPdfMetric(CInt(dblStartY + fncGetZoomValue(intMerginTop))), fncPdfMetric(intImgW), fncPdfMetric(intImgH), mobjImage(intIndex))

                End If

            Else

                If My.Settings.strCard1Bg <> "" Then
                    If System.IO.File.Exists(My.Settings.strCard1Bg) Then

                        fncDrawCardAvatar(objGfx, fncPdfMetric(objCard1.CardCoor.X + mintStartX), _
                                          fncPdfMetric(objCard1.CardCoor.Y + mintStartY), fncPdfMetric(objCard1.Width), _
                                          fncPdfMetric(objCard1.Height), mCardBg)

                    End If

                Else
                    If Not IsNothing(mCardBg) Then
                        fncDrawCardAvatar(objGfx, fncPdfMetric(objCard1.CardCoor.X + mintStartX), _
                                          fncPdfMetric(objCard1.CardCoor.Y + mintStartY), fncPdfMetric(objCard1.Width), _
                                          fncPdfMetric(objCard1.Height), mCardBg)
                    Else
                        objGfx.DrawRectangle(objPen, fncPdfMetric(CInt(dblStartX)), fncPdfMetric(CInt(dblStartY)), fncPdfMetric(objCard1.Width), fncPdfMetric(objCard1.Height))
                    End If


                End If


            End If


            'strBirthDate = objCard1.CardBirth
            'strBirthDate = ""
            'strDDate = objCard1.CardDeath

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

            dblStartTextY = CInt(CInt(dblStartY) + intImgH + fncGetZoomValue(6))

            'draw name
            fncWriteText(objGfx, strInfo1, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontTitle, intAddH)
            dblStartTextY = dblStartTextY + intAddH

            'draw alias
            If strInfo2 <> "" Then
                fncWriteText(objGfx, strInfo2, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontTitle, intAddH)
                dblStartTextY = dblStartTextY + intAddH
            End If


            'draw birthDay
            If strInfo3 <> "" Then
                fncWriteText(objGfx, strInfo3, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontNomal, intAddH)
                dblStartTextY = dblStartTextY + intAddH
            End If


            'draw DeathDay
            If strInfo4 <> "" Then
                fncWriteText(objGfx, strInfo4, CInt(dblStartX), dblStartTextY, objCard1.Width, xfontNomal, intAddH)
                dblStartTextY = dblStartTextY + intAddH
            End If


        Catch ex As Exception

        End Try

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

    '   ******************************************************************
    '　　　FUNCTION   : functionWriteText
    '      MEMO       : 
    '      CREATE     : 2012/01/07  AKB Nghia
    '      UPDATE     : 
    '   ******************************************************************
    Private Function fncWriteText(ByVal objGfx As XGraphics, _
                                       ByVal strText As String, _
                                       ByVal intStartX As Integer, _
                                       ByVal intStartY As Integer, _
                                       ByVal intWidth As Integer, _
                                       ByVal objfontTitle As XFont, _
                                       ByRef intHeight As Integer) As Boolean
        Try

            Dim objPos As XPoint
            Dim objTempFont As XFont
            objTempFont = objfontTitle
            objPos = xGetPos(objGfx, fncPdfMetric(intStartX), fncPdfMetric(intStartY), fncPdfMetric(intWidth), strText, objTempFont, intHeight)

            objGfx.DrawString(strText, objTempFont, mbushText, objPos)

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

        'XFont(clsDefine.gcstrFontName, fncGetZoomValue(intFontSize), XFontStyle.Bold, fontOptions)

        intHeight = CInt(xsizeString.Height + fncGetZoomValue(cintAddHeight))
        Dim xpPos As XPoint = New XPoint(intStartX + (CInt(intWidth) - CInt(xsizeString.Width)) / 2, intStartY + intHeight)

        Return xpPos

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : xSetCardSize, init value 
    '      MEMO       :  
    '      CREATE     : 2012/01/11  AKB Manh 
    '      UPDATE     :  
    '   ******************************************************************
    Private Sub xSetCardSize(ByRef mintCardW As Integer, ByRef intCardH As Integer)

        Try

            If mblnIsSmallCard Then

                mintCardW = CInt(fncGetZoomValue(clsDefine.MEM_CARD_W_S))
                intCardH = CInt(fncGetZoomValue(clsDefine.MEM_CARD_H_S))
            Else

                mintCardW = CInt(fncGetZoomValue(clsDefine.MEM_CARD_W_L))
                intCardH = CInt(fncGetZoomValue(clsDefine.MEM_CARD_H_L))
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetCardSize", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : Render
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub Render(ByVal gfx As XGraphics)

        Try

            If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD2 Then
                xPdfDraw(gfx)
            Else

                xPdfDrawVector(gfx)
            End If


            If mblnMenual Then
                cboZoom.SelectedItem = "Tự động"
                pagePreview.Zoom = Forms.Zoom.FullPage
                mblnMenual = False

            End If

            Return
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "Render", ex)
        Finally
            'delete temp folder after using
            'basCommon.fncDeleteFolder(strFolder)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector, draw lines
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : lstNormalLine   List
    '      PARAMS     : lstSpecialLine  List
    '      MEMO       : 
    '      CREATE     : 2011/09/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDrawConnector(ByVal gfx As XGraphics, ByVal lstNormalLine As List(Of usrLine), ByVal lstSpecialLine As List(Of usrLine)) As Boolean

        xDrawConnector = False

        Try
            'Dim penBlack As XPen
            'Dim penRed As XPen
            'Dim ptStart As Point
            'Dim ptEnd As Point

            'penBlack = New XPen(XColor.FromArgb(0, 0, 0), 1)
            'penRed = New XPen(XColor.FromArgb(255, 0, 0), 2)

            fncDrawPdfConnector(gfx, lstNormalLine, New XPen(XColor.FromArgb(0, 0, 0), 1), mintStartX, mintStartY)
            fncDrawPdfConnector(gfx, lstSpecialLine, New XPen(XColor.FromArgb(255, 0, 0), 2), mintStartX, mintStartY)

            ''draw normal line
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

            '    gfx.DrawLine(penBlack, PdfMetric(ptStart.X), PdfMetric(ptStart.Y), PdfMetric(ptEnd.X), PdfMetric(ptEnd.Y))

            'Next

            'draw special line
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

            '    gfx.DrawLine(penRed, ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y)

            'Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDrawConnector", ex)
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub cboPagesize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPagesize.SelectedIndexChanged

        Try

            rbtLand.Enabled = True
            rbtPortrait.Enabled = True

            If cboPagesize.SelectedIndex <= 0 Then
                rbtLand.Enabled = False
                rbtPortrait.Enabled = False
            End If

            xSetPageSize()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "cboPagesize_SelectedIndexChanged", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSetPageSize()
        Try
            'Label3.Visible = False
            'Label4.Visible = False
            'txtWidth.Visible = False
            'txtHeight.Visible = False
            'btnOk.Visible = False
            'cboZoom.SelectedItem = "Tự động"
            'pagePreview.Zoom = Forms.Zoom.BestFit


            Select Case fncCnvNullToString(cboPagesize.SelectedItem)
                Case mstrAutosize
                    'pagePreview.PageSize = New System.Drawing.Size(mintMaxW, mintMaxH)
                    pagePreview.PageSize = New System.Drawing.Size(fncPdfMetric(mintMaxW + mintStartX), fncPdfMetric(mintMaxH + mintStartY + mintEndY))
                    'cboZoom.SelectedItem = "50"

                Case Else

                    pagePreview.PageSize = xCalPagePreviewPageSize(mcintCboPageSize(cboPagesize.SelectedIndex), rbtLand.Checked)

                    'Case "A0 (1189 x 841 mm)"

                    'pagePreview.PageSize = xCalPagePreviewPageSize(PageSize.A0, rbtLand.Checked)
                    'If rbtLand.Checked = True Then

                    '    'pagePreview.PageSize = New System.Drawing.Size(3368, 2380)
                    '    pagePreview.PageSize = New System.Drawing.Size(CInt(1189 * intDPIX / 25.4), CInt(841 * intDPIX / 25.4))

                    'Else
                    '    pagePreview.PageSize = PageSizeConverter.ToSize(PageSize.A0)
                    'End If
                    ''pagePreview.PageSize = New System.Drawing.Size(595, 842)

                    'Case "A1 (841 x 594 mm)"
                    '    pagePreview.PageSize = xCalPagePreviewPageSize(PageSize.A1, rbtLand.Checked)
                    '    'If rbtLand.Checked = True Then
                    '    '    'pagePreview.PageSize = New System.Drawing.Size(2380, 1684)
                    '    '    pagePreview.PageSize = New System.Drawing.Size(CInt(841 * intDPIX / 25.4), CInt(594 * intDPIX / 25.4))

                    '    'Else
                    '    '    pagePreview.PageSize = PageSizeConverter.ToSize(PageSize.A1)
                    '    'End If

                    'Case "A2 (594 x 420 mm)"
                    '    pagePreview.PageSize = xCalPagePreviewPageSize(PageSize.A2, rbtLand.Checked)
                    '    'If rbtLand.Checked = True Then
                    '    '    pagePreview.PageSize = New System.Drawing.Size(1684, 1190)
                    '    'Else
                    '    '    pagePreview.PageSize = PageSizeConverter.ToSize(PageSize.A2)
                    '    'End If

                    'Case "A3 (420 x 297 mm)"
                    '    pagePreview.PageSize = xCalPagePreviewPageSize(PageSize.A3, rbtLand.Checked)
                    '    'If rbtLand.Checked = True Then
                    '    '    pagePreview.PageSize = New System.Drawing.Size(1190, 842)
                    '    'Else
                    '    '    pagePreview.PageSize = PageSizeConverter.ToSize(PageSize.A3)
                    '    'End If

                    'Case "A4 (297 x 210 mm)"
                    '    pagePreview.PageSize = xCalPagePreviewPageSize(PageSize.A4, rbtLand.Checked)

                    '    'If rbtLand.Checked = True Then
                    '    '    pagePreview.PageSize = New System.Drawing.Size(842, 595)
                    '    'Else
                    '    '    pagePreview.PageSize = PageSizeConverter.ToSize(PageSize.A4)
                    '    'End If

                    'Case "A5 (210 x 148 mm)"

                    '    pagePreview.PageSize = xCalPagePreviewPageSize(PageSize.A5, rbtLand.Checked)

                    '    'If rbtLand.Checked = True Then
                    '    '    pagePreview.PageSize = New System.Drawing.Size(595, 421)
                    '    'Else
                    '    '    pagePreview.PageSize = PageSizeConverter.ToSize(PageSize.A5)
                    '    'End If

            End Select
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetPageSize", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xCalPagePreviewPageSize, Caculate Pagesize of Page Preview
    '      RETURN     : XSize, depend on Paper Orientation
    '      MEMO       : 
    '      CREATE     : 2012/10/17  AKB MANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCalPagePreviewPageSize(ByVal intPageSize As PageSize, ByVal blnLandScape As Boolean) As XSize
        Try

            Dim retPSize As XSize   'Size of Page
            Dim retSPSize As Size   'Size of Standard Papers
            Dim intDPIX As Integer
            Dim intDPIY As Integer

            intDPIX = CInt(pagePreview.CreateGraphics.DpiX)
            intDPIY = CInt(pagePreview.CreateGraphics.DpiY)
            retSPSize = xPageSize(intPageSize)

            If blnLandScape Then

                retPSize = New System.Drawing.Size(CInt(retSPSize.Width * intDPIX / 25.4), CInt(retSPSize.Height * intDPIY / 25.4))

            Else

                retPSize = PageSizeConverter.ToSize(intPageSize)

            End If

            Return retPSize

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xCalPagePreviewPageSize", ex)

        End Try


    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xPageSize, Get Pagesize
    '      RETURN     : Size (width,height) in mm
    '      MEMO       : 
    '      CREATE     : 2012/10/17  AKB MANH
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xPageSize(ByVal intPageSize As PageSize) As Size

        Try

            'ISO paper sizes (plus rounded inch values) Format 	A series 	B series 	C series
            'Size 	mm × mm 	in × in 	mm × mm 	in × in 	mm × mm 	in × in
            '0 	841 × 1189 	33.11 × 46.81 	1000 × 1414 	39.37 × 55.67 	917 × 1297 	36.10 × 51.06
            '1 	594 x 841 	23.39 × 33.11 	707 × 1000 	27.83 × 39.37 	648 × 917 	25.51 × 36.10
            '2 	420 × 594 	16.54 × 23.39 	500 × 707 	19.69 × 27.83 	458 × 648 	18.03 × 25.51
            '3 	297 × 420 	11.69 × 16.54 	353 × 500 	13.90 × 19.69 	324 × 458 	12.76 × 18.03
            '4 	210 × 297 	8.27 × 11.69 	250 × 353 	9.84 × 13.90 	229 × 324 	9.02 × 12.76
            '5 	148 × 210 	5.83 × 8.27 	176 × 250 	6.93 × 9.84 	162 × 229 	6.38 × 9.02
            '6 	105 × 148 	4.13 × 5.83 	125 × 176 	4.92 × 6.93 	114 × 162 	4.49 × 6.38
            '7 	74 × 105 	2.91 × 4.13 	88 × 125 	3.46 × 4.92 	81 × 114 	3.19 × 4.49
            '8 	52 × 74 	2.05 × 2.91 	62 × 88 	2.44 × 3.46 	57 × 81 	2.24 × 3.19
            '9 	37 × 52 	1.46 × 2.05 	44 × 62 	1.73 × 2.44 	40 × 57 	1.57 × 2.24
            '10 	26 × 37 	1.02 × 1.46 	31 × 44 	1.22 × 1.73 	28 × 40 	1.10 × 1.57

            Dim retSize As Size

            Select Case intPageSize

                Case PageSize.A0
                    retSize = New Size(1189, 841)

                Case PageSize.A1
                    retSize = New Size(841, 594)

                Case PageSize.A2
                    retSize = New Size(594, 420)

                Case PageSize.A3
                    retSize = New Size(420, 297)

                Case PageSize.A4
                    retSize = New Size(297, 210)

                Case PageSize.A5
                    retSize = New Size(210, 148)

            End Select

            Return retSize

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xPageSize", ex)
        End Try

    End Function



    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub rbtPortrait_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtPortrait.CheckedChanged
        Try
            If rbtPortrait.Checked = True Then
                xSetPageSize()
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "rbtPortrait_CheckedChanged", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub rbtLand_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbtLand.CheckedChanged
        Try
            If rbtLand.Checked = True Then
                xSetPageSize()
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "rbtLand_CheckedChanged", ex)
        End Try
    End Sub

    Public Function PdfPagePreview() As PdfSharp.Forms.PagePreview
        Return pagePreview
    End Function


    Private Sub xExportToPDf()
        Try

            Dim frmOpt As New frmPdfOption
            frmOpt.fncShowForm(mstrRootInfo, Me)

            Return
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xExportToPDf", ex)
        End Try

    End Sub

    Private Sub xExportToImage()
        Try
            Dim objPdfPrint As clsPdf = New clsPdf(mintMaxW, mintMaxH)
            Me.Cursor = Cursors.WaitCursor
            'try to export F-tree to PDF
            'If objPdfPrint.fncExportTree(mtblControl, mlstNormalLine, mlstSpecialLine) Then
            If objPdfPrint.fncExportTree(mobjImage, mobjCard, mlstNormalLine, mlstSpecialLine) Then
                objPdfPrint.fncExportToImage()
            End If

            Return
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xExportToPDf", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnPDF_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPDF.Click

        Try

            xExportToPDf()

            Return
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnPDF_Click", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnExcelNormal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcelNormal.Click
        Dim prdPrint As System.Drawing.Printing.PrintDocument = Nothing
        Dim dlgPrint As PrintDialog = Nothing
        Dim objXlsPrint As clsExcel = Nothing

        Try
            gblnDrawTreeAdvance = False
            objXlsPrint = New clsExcel()

            'try to create excel instance
            If Not objXlsPrint.fncCreateXlsApp() Then

                basCommon.fncMessageWarning(basConst.gcstrNoExcel)
                Exit Sub

            End If
            'try to open template file
            If Not objXlsPrint.fncOpenTemplate(My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrXltPath2, gcintSheetNo, gcstrTemplatePass) Then Exit Sub
            'try to export F-tree to Excel
            If objXlsPrint.fncExportTree(mstExportInfo) Then Return

        Catch ex As Exception

            If objXlsPrint IsNot Nothing Then objXlsPrint.fncClose(True)
            objXlsPrint = Nothing
            basCommon.fncSaveErr("", "btnExcel_Click", ex)

        Finally

            If prdPrint IsNot Nothing Then prdPrint.Dispose()
            If dlgPrint IsNot Nothing Then dlgPrint.Dispose()
            If objXlsPrint IsNot Nothing Then objXlsPrint.fncClose()
            objXlsPrint = Nothing

        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click
        Try
            Me.Cursor = Cursors.WaitCursor

            Dim strPrinter As String

            strPrinter = frmPrint.ShowForm
            If strPrinter = "" Then

                Return
            End If

            Dim pd As New System.Drawing.Printing.PrintDocument()
            pd.PrinterSettings.PrinterName = strPrinter
            pd.DefaultPageSettings.Landscape = rbtLand.Checked
            AddHandler pd.PrintPage, AddressOf PrintPage
            pd.Print()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnPrint_Click", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub PrintPage(ByVal sender As Object, ByVal ev As PrintPageEventArgs)
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim graphics As Graphics = ev.Graphics
            'graphics.PageUnit = GraphicsUnit.Point
            graphics.PageUnit = GraphicsUnit.Display
            Dim gfx As XGraphics = XGraphics.FromGraphics(graphics, pagePreview.PageSize)
            Render(gfx)

            ev.HasMorePages = False
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "PrintPage", ex)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtName_KeyPress, handle keypress
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnEnd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEnd.Click
        Try
            Me.Dispose()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnEnd_Click", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : PrintPreview_FormClosed
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************

    Private Sub PrintPreview_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Try

            Dim i As Integer = 0

            Do While i < mobjImage.Length
                mobjImage(i).Dispose()
                mobjImage(i) = Nothing
                mobjCard(i) = Nothing
                i = i + 1
            Loop

            mobjImage = Nothing
            mobjCard = Nothing

            Dim strFolder As String
            strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder
            basCommon.fncDeleteFolder(strFolder)

        Catch ex As Exception

        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : PrintPreview_Load, Form load after ShowDialog
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub PrintPreview_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            ' ▽2018/04/27 AKB Nguyen Thanh Tung --------------------------------
            If IsNothing(mobjFont) Then mobjFont = My.Settings.objFontDefaut
            If Not mblnUpdatePrint Then
                cboPagesize.SelectedItem = mstrAutosize
                cboZoom.SelectedItem = "Tự động"
                xSetZoom()
            Else
                cboPagesize.SelectedIndex = mintPrintPageSizeSelected
                cboZoom.SelectedIndex = mintPrintPageZoomSelected
                If mblnPrintPageLandScape Then
                    rbtLand.Checked = True
                Else
                    rbtPortrait.Checked = True
                End If
            End If
            'cboPagesize.SelectedItem = mstrAutosize
            'cboZoom.SelectedItem = "Tự động"
            'xSetZoom()
            ' △2018/04/27 AKB Nguyen Thanh Tung --------------------------------

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "PrintPreview_Load", ex)

        End Try
    End Sub


#Region "NOT USED"

    ''   ****************************************************************** 
    ''      FUNCTION   : xSetCardSize, init value 
    ''      MEMO       :  
    ''      CREATE     : 2012/01/11  AKB  
    ''      UPDATE     :  
    ''   ******************************************************************
    'Private Sub xSetCardSize()

    '    Try
    '        If mblnIsSmallCard Then
    '            mintMEM_CARD_SPACE_LEFT = clsDefine.MEM_CARD_SPACE_LEFT_SMALL
    '            mintMEM_CARD_SPACE_DOWN = clsDefine.MEM_CARD_SPACE_DOWN_SMALL
    '            mintMEM_CARD_W = clsDefine.MEM_CARD_W_S
    '            mintMEM_CARD_H = clsDefine.MEM_CARD_H_S
    '        Else
    '            mintMEM_CARD_SPACE_LEFT = clsDefine.MEM_CARD_SPACE_LEFT_LARGE
    '            mintMEM_CARD_SPACE_DOWN = clsDefine.MEM_CARD_SPACE_DOWN_LARGE
    '            mintMEM_CARD_W = clsDefine.MEM_CARD_W_L
    '            mintMEM_CARD_H = clsDefine.MEM_CARD_H_L
    '        End If

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Sub

    ''   ******************************************************************
    ''　　　FUNCTION   : xDrawConnector, draw connector
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : tblDrawControl      Hashtable
    ''      PARAMS     : tblNotDrawControl   Hashtable
    ''      MEMO       : 
    ''      CREATE     : 2011/12/13  AKB
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xDrawConnector(ByVal tblDrawControl As Hashtable, ByVal tblNotDrawControl As Hashtable) As Boolean

    '    xDrawConnector = False

    '    Dim tblRel As DataTable = Nothing

    '    Dim objCard1 As usrMemberCard1 = Nothing
    '    Dim objCard2 As usrMemberCard1 = Nothing

    '    Try
    '        Dim intID1 As Integer
    '        Dim intID2 As Integer
    '        Dim intRel As Integer

    '        tblRel = gobjDB.fncGetRel()
    '        If tblRel Is Nothing Then Return True
    '        'loop for all member to draw connector
    '        For i As Integer = 0 To tblRel.Rows.Count - 1

    '            'reset value
    '            intID1 = 0
    '            intID2 = 0
    '            intRel = 0

    '            'get id from database
    '            Integer.TryParse(basCommon.fncCnvNullToString(tblRel.Rows(i).Item("MEMBER_ID")), intID1)
    '            Integer.TryParse(basCommon.fncCnvNullToString(tblRel.Rows(i).Item("REL_FMEMBER_ID")), intID2)
    '            Integer.TryParse(basCommon.fncCnvNullToString(tblRel.Rows(i).Item("RELID")), intRel)

    '            'exit if member doesn't exist in hastable
    '            If Not tblDrawControl.ContainsKey(intID1) Then Continue For
    '            If Not tblDrawControl.ContainsKey(intID2) Then Continue For

    '            'exit if this member should not be drawn
    '            If intRel = CInt(clsEnum.emRelation.NATURAL) And tblNotDrawControl.ContainsKey(intID2) Then Continue For

    '            objCard1 = CType(tblDrawControl.Item(intID1), usrMemberCard1)
    '            objCard2 = CType(tblDrawControl.Item(intID2), usrMemberCard1)

    '            xDrawLine(objCard1, objCard2)

    '        Next

    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        If tblRel IsNot Nothing Then tblRel.Dispose()
    '    End Try

    'End Function

    ''   ******************************************************************
    ''　　　FUNCTION   : xDrawLine, draw family tree
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS1    : objCard1    usrMemberCard
    ''      PARAMS2    : objCard2    usrMemberCard
    ''      MEMO       : 
    ''      CREATE     : 2011/09/14  AKB
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xDrawLine(ByVal objCard1 As usrMemberCard1, ByVal objCard2 As usrMemberCard1) As Boolean

    '    xDrawLine = False

    '    Try

    '        mobjCardLeft = objCard1
    '        mobjCardRight = objCard2

    '        If objCard1.Location.Y = objCard2.Location.Y Then
    '            'in case 2 cards have same Y - spouse relationship

    '            If objCard1.Location.X > objCard2.Location.X Then

    '                mobjCardLeft = objCard2
    '                mobjCardRight = objCard1

    '            End If

    '        Else
    '            '2 cards have different Y - parent-son relationship
    '            'the higher will be the cardleft

    '            If objCard1.Location.Y > objCard2.Location.Y Then

    '                mobjCardLeft = objCard2
    '                mobjCardRight = objCard1

    '            End If

    '        End If

    '        If mobjCardLeft.Location.Y = mobjCardRight.Location.Y Then
    '            'draw same level
    '            xDrawSameLv()

    '        Else
    '            'draw different level
    '            xDrawDiffLv()

    '        End If

    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    ''   ******************************************************************
    ''　　　FUNCTION   : xDrawSameLv, draw same level connector
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : 
    ''      MEMO       : 
    ''      CREATE     : 2011/12/13  AKB 
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xDrawSameLv() As Boolean

    '    xDrawSameLv = False

    '    Try
    '        Dim pt1 As Point
    '        Dim pt2 As Point

    '        pt1 = mobjCardLeft.CardMidRight
    '        pt2 = mobjCardRight.CardMidLeft
    '        mobjGfx.DrawLine(mobjPen, pt1.X + mintStartX, pt1.Y + mintStartY, pt2.X + mintStartX, pt2.Y + mintStartY)

    '        pt1.Y -= 5
    '        pt2.Y -= 5
    '        mobjGfx.DrawLine(mobjPen, pt1.X + mintStartX, pt1.Y + mintStartY, pt2.X + mintStartX, pt2.Y + mintStartY)

    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

    ''   ******************************************************************
    ''　　　FUNCTION   : xDrawDiffLv, draw different connector
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : 
    ''      MEMO       : 
    ''      CREATE     : 2011/12/13  AKB 
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xDrawDiffLv() As Boolean

    '    xDrawDiffLv = False

    '    Dim ptDraw(4) As Point

    '    Try
    '        'calculate the collection of point to draw
    '        ptDraw(0) = New Point(mobjCardLeft.CardMidRight.X + mintStartX, mobjCardLeft.CardMidRight.Y + mintStartY)

    '        ptDraw(1) = New Point(mobjCardLeft.CardMidRight.X + mintStartX + (mintMEM_CARD_SPACE_LEFT - mintMEM_CARD_W) \ 2, mobjCardLeft.CardMidRight.Y + mintStartY)

    '        ptDraw(2) = New Point(ptDraw(1).X, ptDraw(1).Y + mintStartY + mobjCardRight.CardMidTop.Y - mobjCardLeft.CardMidRight.Y - ((mintMEM_CARD_SPACE_DOWN - mintMEM_CARD_H) \ 2) - mintStartY)

    '        ptDraw(3) = New Point(mobjCardRight.CardMidTop.X + mintStartX, mobjCardRight.CardMidTop.Y - ((mintMEM_CARD_SPACE_DOWN - mintMEM_CARD_H) \ 2) + mintStartY)

    '        ptDraw(4) = New Point(mobjCardRight.CardMidTop.X + mintStartX, mobjCardRight.CardMidTop.Y + mintStartY)

    '        mobjGfx.DrawLines(mobjPen, ptDraw)

    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        Erase ptDraw
    '    End Try

    'End Function
#End Region

    '   ******************************************************************
    '　　　FUNCTION   : btnExcelAdvance_Click
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnExcelAdvance_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcelAdvance.Click
        Dim prdPrint As System.Drawing.Printing.PrintDocument = Nothing
        Dim dlgPrint As PrintDialog = Nothing
        Dim objXlsPrint As clsExcel = Nothing

        Try

            gblnDrawTreeAdvance = True
            gblnIsConfirmDraw = False
            objXlsPrint = New clsExcel()

            'try to create excel instance
            If Not objXlsPrint.fncCreateXlsApp() Then

                basCommon.fncMessageWarning(basConst.gcstrNoExcel)
                Exit Sub

            End If

            objXlsPrint.fncExportTreeAdvance(mstExportInfo)

        Catch ex As Exception

            If objXlsPrint IsNot Nothing Then objXlsPrint.fncClose(True)
            objXlsPrint = Nothing
            basCommon.fncSaveErr("", "btnExcel_Click", ex)

        Finally

            If prdPrint IsNot Nothing Then prdPrint.Dispose()
            If dlgPrint IsNot Nothing Then dlgPrint.Dispose()
            If objXlsPrint IsNot Nothing Then objXlsPrint.fncClose()
            objXlsPrint = Nothing

        End Try


    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : cboZoom_SelectedIndexChanged
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub cboZoom_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboZoom.SelectedIndexChanged
        Try

            xSetZoom()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "cboZoom_SelectedIndexChanged", ex)
        End Try
    End Sub

    Private Sub xSetZoom()

        Try

            Select Case fncCnvNullToString(cboZoom.SelectedItem)
                Case "800"
                    pagePreview.Zoom = Forms.Zoom.Percent800
                Case "600"
                    pagePreview.Zoom = Forms.Zoom.Percent600
                Case "400"
                    pagePreview.Zoom = Forms.Zoom.Percent400
                Case "200"
                    pagePreview.Zoom = Forms.Zoom.Percent200
                Case "150"
                    pagePreview.Zoom = Forms.Zoom.Percent150
                Case "100"
                    pagePreview.Zoom = Forms.Zoom.Percent100
                Case "75"
                    pagePreview.Zoom = Forms.Zoom.Percent75
                Case "50"
                    pagePreview.Zoom = Forms.Zoom.Percent50
                Case "25"
                    pagePreview.Zoom = Forms.Zoom.Percent25

                Case "Tự động"
                    pagePreview.Zoom = Forms.Zoom.FullPage

            End Select
        Catch ex As Exception

        End Try


    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xSetDataDraw
    '      MEMO       : 
    '      CREATE     : 2012/14/20  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetDataDraw(ByVal vobjDraw As Object) As basConst.stExportInfo
        xSetDataDraw = Nothing
        Dim stExportInf As basConst.stExportInfo
        Dim objTemp As New Object
        Try
            stExportInf = Nothing
            If TypeOf vobjDraw Is clsDrawTreeS1 Then
                mobjDraw1 = CType(vobjDraw, clsDrawTreeS1)
                stExportInf.objTreeType = mobjDraw1
                stExportInf.tblControl = mobjDraw1.DrawingCard
                stExportInf.lstNormalLine = mobjDraw1.NormalLine
                stExportInf.lstSpecialLine = mobjDraw1.SpecialLine
                stExportInf.tblMemberInfo = mobjDraw1.DrawList
                stExportInf.intRootID = mobjDraw1.RootID

                mintMaxH = mobjDraw1.MaxHeight
                mintMaxW = mobjDraw1.MaxWidth
                mstrRootInfo = mobjDraw1.RootMemberInfo
                mintMaxHMM = mobjDraw1.MaxHeightInMM
                mintMaxWMM = mobjDraw1.MaxWidthInMM
                btnExcelAdvance.Enabled = True
            ElseIf TypeOf vobjDraw Is clsDrawTreeS2 Then
                mobjDraw2 = CType(vobjDraw, clsDrawTreeS2)
                stExportInf.objTreeType = mobjDraw2
                stExportInf.tblControl = mobjDraw2.DrawingCard
                stExportInf.lstNormalLine = mobjDraw2.NormalLine
                stExportInf.lstSpecialLine = mobjDraw2.SpecialLine
                mintMaxH = mobjDraw2.MaxHeight
                mintMaxW = mobjDraw2.MaxWidth
                mintMaxHMM = mobjDraw2.MaxHeightInMM()
                mintMaxWMM = mobjDraw2.MaxWidthInMM()
                mstrRootInfo = mobjDraw2.RootMemberInfo
                btnExcelAdvance.Enabled = False
                'ElseIf TypeOf vobjDraw Is clsDrawTreeS3 Then
            ElseIf TypeOf vobjDraw Is clsDrawTreeS3 Then
                mobjDraw3 = CType(vobjDraw, clsDrawTreeS3)
                stExportInf.objTreeType = mobjDraw3
                stExportInf.tblControl = mobjDraw3.DrawingCard
                stExportInf.lstNormalLine = mobjDraw3.NormalLine
                stExportInf.lstSpecialLine = mobjDraw3.SpecialLine
                mintMaxH = mobjDraw3.MaxHeight
                mintMaxW = mobjDraw3.MaxWidth
                mintMaxHMM = mobjDraw3.MaxHeightInMM()
                mintMaxWMM = mobjDraw3.MaxWidthInMM()
                mstrRootInfo = mobjDraw3.RootMemberInfo
                btnExcelAdvance.Enabled = False
            ElseIf TypeOf vobjDraw Is clsDrawTreeSS Then
                mobjDrawSS = CType(vobjDraw, clsDrawTreeSS)
                stExportInf.objTreeType = mobjDrawSS
                stExportInf.tblControl = mobjDrawSS.DrawingCard
                stExportInf.lstNormalLine = mobjDrawSS.NormalLine
                stExportInf.lstSpecialLine = mobjDrawSS.SpecialLine
                mintMaxH = mobjDrawSS.MaxHeight
                mintMaxW = mobjDrawSS.MaxWidth
                mintMaxHMM = mobjDrawSS.MaxHeightInMM()
                mintMaxWMM = mobjDrawSS.MaxWidthInMM()
                mstrRootInfo = mobjDrawSS.RootMemberInfo
                btnExcelAdvance.Enabled = False
            End If
            If TypeOf vobjDraw Is clsDrawTreeA1 Then
                mobjDrawA1 = CType(vobjDraw, clsDrawTreeA1)
                stExportInf.objTreeType = mobjDrawA1
                stExportInf.tblControl = mobjDrawA1.DrawingCard
                stExportInf.lstNormalLine = mobjDrawA1.NormalLine
                stExportInf.lstSpecialLine = mobjDrawA1.SpecialLine
                stExportInf.tblMemberInfo = mobjDrawA1.DrawList
                stExportInf.intRootID = mobjDrawA1.RootID

                mintMaxH = mobjDrawA1.MaxHeight
                mintMaxW = mobjDrawA1.MaxWidth
                mstrRootInfo = mobjDrawA1.RootMemberInfo
                mintMaxHMM = mobjDrawA1.MaxHeightInMM
                mintMaxWMM = mobjDrawA1.MaxWidthInMM
                btnExcelAdvance.Enabled = True
            End If
            stExportInf.intCardStyle = My.Settings.intCardStyle
            Return stExportInf
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetDataDraw", ex)
        End Try
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        xExportToImage()
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Private Sub btSetting_Click(sender As Object, e As EventArgs) Handles btSetting.Click

        If IsNothing(mfrmMain) Then Exit Sub

        Try

            mfrmMain.tsbSetting.PerformClick()
            mfrmMain.mblnUpdatePrint = True
            mfrmMain.mintPrintPageSizeSelected = cboPagesize.SelectedIndex
            mfrmMain.mintPrintPageZoomSelected = cboZoom.SelectedIndex
            mfrmMain.mblnPrintPageLandScape = rbtLand.Checked

            Me.Hide()
            mfrmMain.tsbPrintTree.PerformClick()
            mfrmMain.mblnUpdatePrint = False
            Me.Close()
        Catch ex As Exception

        End Try
    End Sub
End Class