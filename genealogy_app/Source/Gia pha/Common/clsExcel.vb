'   ******************************************************************
'      TITLE      : EXCEL FUNCTIONS
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/12/12　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict Off

'   ******************************************************************
'　　　FUNCTION   : Excel class
'      MEMO       : 
'      CREATE     : 2011/12/12　AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class clsExcel

    Private Const mcstrFileNotFound As String = "Không tìm thấy tập tin mẫu."       'file not found message

    Private mobjApp As Object                                               'excel application
    Private mobjBook As Object                                              'workbook
    Private mobjSheet As Object                                             'sheet
    Private mobjCardLeft As Object                                          'temporary card
    Private mobjCardRight As Object                                         'temporary card
    'Private mblnIsSmallCard As Boolean                                      'draw small card
    Private mintMEM_CARD_SPACE_LEFT As Integer                              'margin left
    Private mintMEM_CARD_SPACE_DOWN As Integer                              'margin bottom
    Private mintMEM_CARD_W As Integer                                       'card width
    Private mintMEM_CARD_H As Integer                                       'card height
    'Vinhnn Added 2012/10/03
    Private mobjPrgBar As frmProgressBar
    Private mobjHashTbl As Hashtable
    Private mobjMemberInfo As Hashtable
    Private mintRootID As Integer

    Private mobjTreDraw As Object

    Private mblnExportSuccess As Boolean = False
    Private mblnExportComplete As Boolean = False

    Private Const mcstrclsName As String = "clsAKBExcel"
    Private mlstNormalLine As List(Of usrLine)
    Private mlstSpecialLine As List(Of usrLine)
    'Vinhnn End Added
    Private mstrSaveName As String
    Private mstrXLSAdvGroupName As String

    Private Const mintConectorLineWeight As Integer = 4
    Private mcintXLHeightDelta As Integer = 10

    Private mintXLPtoI As Integer = 72
    Private mintXLMemberShapeWidth As Integer


    Private mdblXLMultiPlierX As Double = 1.0
    Private mdblXLMultiPlierY As Double = 1.0
    Private Const mcstrPicName As String = "PicTemp"
    Private Const mcstrTxtData As String = "txtData"

    'EXCELファイル保存名称プロパティ
    Public Property SaveName() As String
        Get
            Return mstrSaveName
        End Get
        Set(ByVal Value As String)
            mstrSaveName = Value
        End Set
    End Property


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/12/12　AKB Quyet
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New()



    End Sub
    'Vinhnn Added 2012/10/03
    Public Property Sheet() As Object
        Get
            Return mobjSheet
        End Get
        Set(ByVal Value As Object)
            fncReleaseObject(mobjSheet)
            mobjSheet = Value
        End Set
    End Property

    Public Property Sheets() As Object
        Get
            Return mobjSheet
        End Get
        Set(ByVal Value As Object)
            fncReleaseObject(mobjSheet)
            mobjSheet = Value
        End Set
    End Property

    'EXCELブック
    Public Property Book() As Object
        Get
            Return mobjBook
        End Get
        Set(ByVal Value As Object)
            fncReleaseObject(mobjBook)
            mobjBook = Value
        End Set
    End Property

    'EXCELアプリ
    Public Property App() As Object
        Get
            Return mobjApp
        End Get
        Set(ByVal Value As Object)
            fncReleaseObject(mobjApp)
            mobjApp = Value
        End Set
    End Property


    '   ******************************************************************
    '　　　FUNCTION   : fncOpenTemplate, open template file
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : strTemplateFile String
    '      PARAMS2    : intSheetNo      Integer
    '      MEMO       : 
    '      CREATE     : 2011/12/12　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateXlsApp() As Boolean

        fncCreateXlsApp = False

        Try
            'create excell application
            Try
                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
                mobjApp = CreateObject("Excel.Application")

                mintXLPtoI = mobjApp.InchesToPoints(1)

            Catch e As Exception
                Return False
            End Try

            mobjApp.Visible = False

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncOpenTemplate, open template file
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : strTemplateFile String
    '      PARAMS2    : intSheetNo      Integer
    '      MEMO       : 
    '      CREATE     : 2011/12/12　AKB VINH
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncOpenTemplate(ByVal strTemplateFile As String, ByVal intSheetNo As Integer, _
                                    Optional ByVal strFilePassword As String = "") As Boolean

        fncOpenTemplate = False

        Dim objFI As System.IO.FileInfo = Nothing

        Try
            objFI = New System.IO.FileInfo(strTemplateFile)

            If Not objFI.Exists Then

                If Not basCommon.fncRenameTemplate(strTemplateFile) Then

                    basCommon.fncMessageWarning(mcstrFileNotFound)
                    Return False

                End If

            End If
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            'open excel file
            mobjBook = mobjApp.Workbooks.Open(strTemplateFile, , False, , strFilePassword, strFilePassword, True)
            'mobjBook = mobjApp.Workbooks.Open(strTemplateFile, , False, , , , True)
            mobjSheet = mobjBook.WorkSheets.Item(intSheetNo)
            mobjSheet.Activate()

            gdblFaChildConnWeight = CDbl(fncGetCellData(2, 2))
            gdblParentConnWeight = CDbl(fncGetCellData(1, 2))
            mstrXLSAdvGroupName = CStr(fncGetCellData(1, 4))

            fncSetCellData(2, 1, "")
            fncSetCellData(2, 2, "")
            fncSetCellData(1, 1, "")
            fncSetCellData(1, 2, "")
            fncSetCellData(3, 1, "")
            fncSetCellData(4, 1, "")
            'default culture is en-US

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncOpenTemplate, open template file
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : strTemplateFile String
    '      PARAMS2    : intSheetNo      Integer
    '      MEMO       : Anh Vinh bao Quyet lam the nay :D
    '      CREATE     : 2011/12/12　AKB QUYET
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncOpenTemplateSearch(ByVal strTemplateFile As String, ByVal intSheetNo As Integer, _
                                        Optional ByVal strFilePassword As String = "") As Boolean

        fncOpenTemplateSearch = False

        Dim objFI As System.IO.FileInfo = Nothing

        Try
            objFI = New System.IO.FileInfo(strTemplateFile)

            If Not objFI.Exists Then

                If Not basCommon.fncRenameTemplate(strTemplateFile) Then

                    basCommon.fncMessageWarning(mcstrFileNotFound)
                    Return False

                End If

            End If
            System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
            'open excel file
            mobjBook = mobjApp.Workbooks.Open(strTemplateFile, , False, , strFilePassword, strFilePassword, True)
            'mobjBook = mobjApp.Workbooks.Open(strTemplateFile, , False, , , , True)
            mobjSheet = mobjBook.WorkSheets.Item(intSheetNo)
            mobjSheet.Activate()

            'default culture is en-US

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Private Sub xInit(ByVal vobjStExport As Object)

        vobjStExport = CType(vobjStExport, stExportInfo)
        mobjTreDraw = vobjStExport.objTreeType
        gintPercent = 0
        mobjHashTbl = New Hashtable
        mlstNormalLine = New List(Of usrLine)
        mlstSpecialLine = New List(Of usrLine)
        mlstNormalLine = vobjStExport.lstNormalLine
        mlstSpecialLine = vobjStExport.lstSpecialLine
        mobjHashTbl = vobjStExport.tblControl
        mintRootID = vobjStExport.intRootID
        mobjMemberInfo = vobjStExport.tblMemberInfo

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncExportTree, export F-tree to excel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : tblDrawControl      Hashtable
    '      PARAMS2    : tblNotDrawControl   Hashtable
    '      MEMO       : 
    '      CREATE     : 2011/12/13　AKB Quyet
    '      UPDATE     : 2012/10/04　AKB Vinh
    '   ******************************************************************
    'Public Function fncExportTree(ByVal tblDrawControl As Hashtable, _
    '                              ByVal lstNormalLine As List(Of usrLine), _
    '                              ByVal lstSpecialLine As List(Of usrLine), _
    '                              ByVal objTreeDraw As Object, _
    '                              Optional ByVal intCardStyle As clsEnum.emCardStyle = clsEnum.emCardStyle.CARD1) As Boolean

    Public Function fncExportTree(ByVal vobjStExport As Object) As Boolean
        fncExportTree = False

        Dim objProgressThread As System.Threading.Thread = Nothing
        Dim objThreadExcel As System.Threading.Thread = Nothing

        Try
            xInit(vobjStExport)
            vobjStExport = CType(vobjStExport, stExportInfo)
            mobjPrgBar = New frmProgressBar
            objProgressThread = New System.Threading.Thread(AddressOf xGetProgress)

            If vobjStExport.intCardStyle = clsEnum.emCardStyle.CARD1 Then

                If TypeOf mobjTreDraw Is clsDrawTreeS1 Then

                    objThreadExcel = New System.Threading.Thread(AddressOf xExportTreeNomalS1)

                ElseIf TypeOf mobjTreDraw Is clsDrawTreeA1 Then

                    objThreadExcel = New System.Threading.Thread(AddressOf xExportTreeNomalA1)

                End If


            Else

                objThreadExcel = New System.Threading.Thread(AddressOf xExportTreeNomalS2)

            End If

            objThreadExcel.Start()
            objProgressThread.Start()
            mobjPrgBar.ShowDialog()

            Return True
        Catch ex As Exception
            Throw ex
        Finally

            Do Until mblnExportComplete = True
                Application.DoEvents()
            Loop

            objProgressThread.Abort()
            objProgressThread = Nothing
            objThreadExcel.Abort()
            objThreadExcel = Nothing
            xEndTreeRenderAdvanced()

            'objThread.Abort()
            'objThread = Nothing

            'If mblnExportSuccess Then

            '    objThreadExcel.Abort()
            '    objThreadExcel = Nothing
            'fncClose()

            'Else
            '    objThreadExcel.Abort()
            '    objThreadExcel = Nothing
            'fncClose(True)
            'End If



        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xConvertFormXPositionToExcel
    '      VALUE      : 
    '      MEMO       : 
    '      CREATE     : 2012/10/18　AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xConvertFormXPositionToExcel(ByVal intValue As Integer, _
                                                  Optional ByVal blnMultiPlier As Boolean = True) As Integer

        Dim dblValue As Double = mdblXLMultiPlierX

        If Not blnMultiPlier Then
            dblValue = 1.0
        End If
        Return CInt(intValue * mintXLPtoI / gintTreePanelDPIX * dblValue)

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xConvertFormXPositionToExcel
    '      VALUE      : 
    '      MEMO       : 
    '      CREATE     : 2012/10/18　AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xConvertFormYPositionToExcel(ByVal intValue As Integer, _
                                                  Optional ByVal blnMultiPlier As Boolean = True) As Integer

        Dim dblValue As Double = mdblXLMultiPlierY

        If Not blnMultiPlier Then
            dblValue = 1.0
        End If
        Return CInt(intValue * mintXLPtoI / gintTreePanelDPIY * dblValue)

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xExportTreeNomal
    '      VALUE      : 
    '      MEMO       : 
    '      CREATE     : 2012/10/04　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xExportTreeNomalS2()
        '        Dim objSelection As Object = Nothing
        Dim objPic As Object = Nothing

        mblnExportComplete = False
        Try

            Dim objCard As usrMemCardBase
            Dim intId As Integer
            Dim ptPosition As clsCoordinate
            Dim strPath As String
            Dim strFolder As String
            Dim intCount As Integer = 0

            mblnExportSuccess = False
            gintPercent = 0
            Dim objDtView As New DataView
            objDtView = mobjTreDraw.DrawList()
            ''determine card size
            'Me.mblnIsSmallCard = blnIsSmallCard
            'xSetCardSize()

            strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder

            ' create temp folder
            If Not basCommon.fncCreateFolder(strFolder, True) Then Exit Sub

            With Me.App
                For i As Integer = 0 To objDtView.Count - 1
                    gintPercent = CInt((i + 1) * 99 / objDtView.Count)
                    intId = CInt(objDtView.Item(i)(1))
                    objCard = mobjHashTbl.Item(intId)
                    ptPosition = objCard.CardCoor
                    strPath = objCard.fncGetImage(strFolder, False)
                    'Add Picture to Excel
                    objPic = mobjSheet.Shapes.AddPicture(strPath, basConst.gcintXlsTriStateFalse, _
                                                        basConst.gcintXlsTriStateTrue, _
                                                        xConvertFormXPositionToExcel(ptPosition.X), _
                                                        xConvertFormYPositionToExcel(ptPosition.Y), _
                                                        objCard.Width, objCard.Height)
                    objPic.Select()

                    objPic.ScaleHeight(1, gcintXlsTriStateTrue) 'msoTrue = -1
                    objPic.ScaleWidth(1, gcintXlsTriStateTrue)  'msoTrue = -1

                    'Set name to Picture
                    'objSelection = .Selection

                    objPic.Name = "Picture" + intId.ToString
                    'Draw Connector
                    'Connect(Father And Childs)
                    If objCard.ParentID <> basConst.gcintNONE_VALUE Then
                        If objCard.ParentID <> basConst.gcintNONE_VALUE Then
                            If basCommon.fncIsFhead(intId) And basCommon.fncIsFhead(objCard.ParentID) Then
                                fncAddConnector("Picture" + objCard.ParentID.ToString, "Picture" + intId.ToString, True, , gdblFaChildConnWeight, 2, False)
                            Else
                                fncAddConnector("Picture" + objCard.ParentID.ToString, "Picture" + intId.ToString, True, , gdblFaChildConnWeight)
                            End If

                        End If
                    End If
                    'Connect parents
                    If objCard.SpouseID <> basConst.gcintNONE_VALUE Then
                        fncAddConnector("Picture" + objCard.SpouseID.ToString, "Picture" + intId.ToString, False, , gdblParentConnWeight)
                    End If

                    fncReleaseObject(objPic)

                Next
            End With
            fncDeleteSheet(3)
            fncDeleteSheet(2)

            mblnExportSuccess = True

        Catch ex As Exception
            Throw ex
        Finally
            fncReleaseObject(objPic)
            gintPercent = 100

            'mobjPrgBar.CloseTheForm()
        End Try

        mblnExportComplete = True
    End Sub

#Region "Export to Excel Standard type with New Method"
    'Add connector between 2 Spouse
    Private Sub xAddConnectorToSpouse(ByVal intSpouseID1 As Integer, _
                                      ByVal intSpouseID2 As Integer, _
                                      ByVal strPrefix As String)
        fncAddConnector(strPrefix + intSpouseID1.ToString, strPrefix + intSpouseID2.ToString, False, , gdblParentConnWeight)
    End Sub

    'Add connector from parent to child
    Private Sub xAddConnectorParent2ChildOfCouple(ByVal intLeftParentID As Integer, _
                                                  ByVal intRightParentID As Integer, _
                                                  ByVal intChildID As Integer, _
                                                  ByVal strPrefix As String)
        Dim objLeftParent As Object = Nothing
        Dim objRightParent As Object = Nothing

        Try
            With Me.App
                objLeftParent = Sheet.Shapes(strPrefix + intLeftParentID.ToString())
                objRightParent = Sheet.Shapes(strPrefix + intRightParentID.ToString())
                Dim intXOffset As Integer = CInt((objRightParent.Left - objLeftParent.Left - objLeftParent.Width) / 2)
                If basCommon.fncIsFhead(intLeftParentID) And basCommon.fncIsFhead(intChildID) Then
                    fncAddConnectorParent2Child(strPrefix + intLeftParentID.ToString, strPrefix + intChildID.ToString, intXOffset, 0, , gdblFaChildConnWeight, 2, False)
                Else
                    fncAddConnectorParent2Child(strPrefix + intLeftParentID.ToString, strPrefix + intChildID.ToString, intXOffset, 0, , gdblFaChildConnWeight, , True)
                End If
            End With

        Catch ex As Exception

        Finally

            fncReleaseObject(objRightParent)
            fncReleaseObject(objLeftParent)

        End Try

    End Sub
    'Add connector from parent to child
    Private Sub xAddConnectorParent2Child(ByVal intParentID As Integer, ByVal intChildID As Integer, _
                                          ByVal strPrefix As String, _
                                          Optional ByVal blnStepChild As Boolean = False, _
                                          Optional ByVal intPos As Integer = 1)

        If blnStepChild Then
            If basCommon.fncIsFhead(intParentID) And basCommon.fncIsFhead(intChildID) Then
                fncAddConnector(strPrefix + intParentID.ToString, strPrefix + intChildID.ToString, True, , gdblFaChildConnWeight, 2, False)
            Else
                fncAddConnector(strPrefix + intParentID.ToString, strPrefix + intChildID.ToString, True, , gdblFaChildConnWeight)
            End If
            Return
        End If

        Dim intBuffer As Integer = clsDefine.MEM_CARD_HORIZON_BUFFER_L

        If My.Settings.intCardSize = CInt(clsEnum.emCardSize.SMALL) Then
            intBuffer = clsDefine.MEM_CARD_HORIZON_BUFFER_S
        End If

        Dim intXOffset As Integer = intPos * xConvertFormXPositionToExcel(intBuffer / 2)


        'If TypeOf mobjTreDraw Is clsDrawTreeA1 Then
        '    If gblnDrawTreeAdvance Then
        '        intXOffset = intPos * gintTreePanelDPIX * intBuffer / 2 / mintXLPtoI
        '    End If

        'End If

        If basCommon.fncIsFhead(intParentID) And basCommon.fncIsFhead(intChildID) Then
            fncAddConnectorParent2Child(strPrefix + intParentID.ToString, strPrefix + intChildID.ToString, intXOffset, 0, , gdblFaChildConnWeight, 2, False)
        Else
            fncAddConnectorParent2Child(strPrefix + intParentID.ToString, strPrefix + intChildID.ToString, intXOffset, 0, , gdblFaChildConnWeight, , True)
        End If

    End Sub

    Private Sub xAddCard1Shape(ByVal objCard As usrMemberCard1, ByVal strFolder As String)

        Dim strPath As String
        Dim objPic As Object = Nothing

        strPath = objCard.fncGetImage(strFolder, False)

        objPic = mobjSheet.Shapes.AddPicture(strPath, basConst.gcintXlsTriStateFalse, _
                                           basConst.gcintXlsTriStateTrue, _
                                           xConvertFormXPositionToExcel(objCard.CardCoor.X), _
                                           xConvertFormYPositionToExcel(objCard.CardCoor.Y), _
                                           objCard.Width, objCard.Height)

        objPic.Select()
        objPic.ScaleHeight(1, gcintXlsTriStateTrue) 'msoTrue = -1
        objPic.ScaleWidth(1, gcintXlsTriStateTrue)  'msoTrue = -1
        'Set name to Picture
        'objSelection = .Selection
        objPic.Name = "Picture" + objCard.CardID.ToString

        fncReleaseObject(objPic)
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xExportTreeNomal
    '      VALUE      : 
    '      MEMO       : 
    '      CREATE     : 2012/10/04　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xExportTreeNomalS1()
        '        Dim objSelection As Object = Nothing


        mblnExportComplete = False
        Dim tblDrawed As New Hashtable
        Try

            Dim objCard As usrMemberCard1
            Dim objCardTemp As usrMemberCard1
            Dim stCard As stCardInfo

            Dim intId As Integer
            Dim strFolder As String
            Dim intCount As Integer = 0

            mblnExportSuccess = False
            gintPercent = 0
            Dim tblCardInfo As Hashtable

            tblCardInfo = mobjTreDraw.DrawList()

            strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder

            ' create temp folder
            If Not basCommon.fncCreateFolder(strFolder, True) Then Exit Sub

            With Me.App
                Dim i As Integer = -1
                Dim j As Integer

                For Each element As DictionaryEntry In tblCardInfo

                    i = i + 1
                    gintPercent = CInt((i + 1) * 99 / mobjHashTbl.Count)
                    objCard = CType(mobjHashTbl(element.Key), usrMemberCard1)
                    intId = CInt(objCard.CardID)

                    'if the member was not drawed, draw it
                    If Not tblDrawed.ContainsKey(element.Key) Then

                        xAddCard1Shape(objCard, strFolder)
                        tblDrawed.Add(objCard.CardID, objCard.CardID)

                    End If

                    'Draw Spouse and draw connector
                    stCard = CType(tblCardInfo(element.Key), stCardInfo)
                    If Not stCard.lstSpouse Is Nothing Then
                        For j = 0 To stCard.lstSpouse.Count - 1
                            objCardTemp = CType(mobjHashTbl(stCard.lstSpouse(j)), usrMemberCard1)

                            'if the spouse was not drawed, draw it
                            If Not tblDrawed.ContainsKey(objCardTemp.CardID) Then
                                xAddCard1Shape(objCardTemp, strFolder)
                                tblDrawed.Add(objCardTemp.CardID, objCardTemp.CardID)
                            End If

                            xAddConnectorToSpouse(intId, stCard.lstSpouse(j), "Picture")
                            intId = stCard.lstSpouse(j)

                        Next

                    End If

                    'Draw Children and draw connector
                    intId = CInt(objCard.CardID)
                    If Not stCard.lstChild Is Nothing Then
                        For j = 0 To stCard.lstChild.Count - 1

                            objCardTemp = CType(mobjHashTbl(stCard.lstChild(j)), usrMemberCard1)

                            'if the child was not drawed, draw it
                            If Not tblDrawed.ContainsKey(objCardTemp.CardID) Then
                                xAddCard1Shape(objCardTemp, strFolder)
                                tblDrawed.Add(objCardTemp.CardID, objCardTemp.CardID)
                            End If

                            xAddConnectorParent2Child(intId, stCard.lstChild(j), "Picture", True)

                        Next
                    End If
                Next
            End With
            fncSetCellSelect(1, 1)
            fncDeleteSheet(3)
            fncDeleteSheet(2)

            mblnExportSuccess = True

        Catch ex As Exception
            Throw ex
        Finally
            'fncReleaseObject(objPic)
            gintPercent = 100
            tblDrawed.Clear()
            tblDrawed = Nothing

            'mobjPrgBar.CloseTheForm()
        End Try

        mblnExportComplete = True
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xExportTreeNomal
    '      VALUE      : 
    '      MEMO       : 
    '      CREATE     : 2012/10/04　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xExportTreeNomalA1()
        '        Dim objSelection As Object = Nothing

        mblnExportComplete = False
        Dim tblDrawed As New Hashtable
        Try

            Dim objCard As usrMemberCard1
            Dim strFolder As String
            Dim intCount As Integer = 0

            mblnExportSuccess = False
            gintPercent = 0
            Dim tblCardInfo As Hashtable

            tblCardInfo = mobjTreDraw.DrawList()

            strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder

            ' create temp folder
            If Not basCommon.fncCreateFolder(strFolder, True) Then Exit Sub

            With Me.App
                Dim i As Integer = -1

                For Each element As DictionaryEntry In tblCardInfo

                    i = i + 1
                    gintPercent = CInt((i + 1) * 99 / mobjHashTbl.Count)
                    objCard = CType(mobjHashTbl(element.Key), usrMemberCard1)

                    'if the member was not drawed, draw it
                    If Not tblDrawed.ContainsKey(element.Key) Then

                        xAddCard1Shape(objCard, strFolder)
                        tblDrawed.Add(objCard.CardID, objCard.CardID)

                    End If


                Next

                xDrawLine(mintRootID, "Picture")

            End With
            fncSetCellSelect(1, 1)
            fncDeleteSheet(3)
            fncDeleteSheet(2)

            mblnExportSuccess = True

        Catch ex As Exception
            Throw ex
        Finally
            'fncReleaseObject(objPic)
            gintPercent = 100
            tblDrawed.Clear()
            tblDrawed = Nothing

            'mobjPrgBar.CloseTheForm()
        End Try

        mblnExportComplete = True
    End Sub

    Private Function xGetMemberCard(ByVal intID As Integer) As stCardInfo

        Return CType(mobjMemberInfo.Item(intID), stCardInfo)

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2Child, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2Child(ByVal intParentID As Integer, _
                                     ByVal intChildID As Integer, _
                                     ByVal strPrefix As String, _
                                     Optional ByVal blnStepChild As Boolean = False, _
                                     Optional ByVal intPos As Integer = 1)

        If blnStepChild Then

            xAddConnectorParent2Child(intParentID, intChildID, strPrefix, True, intPos)

        Else

            xAddConnectorParent2Child(intParentID, intChildID, strPrefix, False, intPos)

        End If

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2ListChild, Draw To 2 List of Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnector2ListChild(ByVal intParentID As Integer, _
                                         ByVal blnStepChild As Boolean, _
                                         ByVal strPrefix As String, _
                                         Optional ByVal intPos As Integer = 1)

        Dim stParent As stCardInfo
        Dim lstChild As List(Of Integer)
        Dim i As Integer
        stParent = xGetMemberCard(intParentID)

        If blnStepChild Then

            lstChild = stParent.lstStepChild

        Else

            lstChild = stParent.lstChild

        End If

        If lstChild Is Nothing Then Return

        For i = 0 To lstChild.Count - 1
            xDrawConnector2Child(intParentID, lstChild(i), strPrefix, blnStepChild, intPos)
        Next

    End Sub

    Private Sub xDrawLineChild(ByVal intParentID As Integer, _
                               ByVal blnStepChild As Boolean, _
                               ByVal strPrefix As String)

        Dim stParent As stCardInfo
        stParent = xGetMemberCard(CInt(intParentID))
        Dim lstChildID As List(Of Integer)
        Dim i As Integer

        If blnStepChild Then

            lstChildID = stParent.lstStepChild

        Else

            lstChildID = stParent.lstChild

        End If

        If lstChildID Is Nothing Then Return
        For i = 0 To lstChildID.Count - 1
            xDrawLine(lstChildID(i), strPrefix)
        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnector2Child, Draw To 2 Child
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************    
    Private Sub xDrawConnector2ListSpouse(ByVal intRootID As Integer, ByVal strPrefix As String)


        Dim stRootCard As stCardInfo
        stRootCard = xGetMemberCard(CInt(intRootID))
        Dim lstSpouse As List(Of Integer)
        Dim i As Integer
        Dim intLeftSpouse As Integer

        lstSpouse = stRootCard.lstSpouse

        If lstSpouse Is Nothing Then Return

        If lstSpouse.Count = 1 Then
            xAddConnectorToSpouse(intRootID, lstSpouse(0), strPrefix)
            Return
        End If

        xAddConnectorToSpouse(lstSpouse(0), intRootID, strPrefix)

        intLeftSpouse = intRootID
        For i = 1 To lstSpouse.Count - 1
            xAddConnectorToSpouse(intLeftSpouse, lstSpouse(i), strPrefix)
            intLeftSpouse = lstSpouse(i)
        Next

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xDrawConnectorListChildOfCouple, Draw To 2 List Child of a couple
    '      VALUE      : 
    '      PARAMS     : intPos means that this parent is on the left or right of Other Spouse
    '                   intPos can be 1 or -1
    '      MEMO       : 
    '      CREATE     : 2012/12/11  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawConnectorListChildOfCouple(ByVal intParentLeftID As Integer, _
                                                ByVal intParentRightID As Integer, _
                                                ByVal strPrefix As String)

        Dim stParentLeft As stCardInfo
        Dim stParentRight As stCardInfo

        Dim lstChild As List(Of Integer)
        Dim intDeltaX As Integer
        Dim i As Integer
        stParentLeft = xGetMemberCard(intParentLeftID)
        stParentRight = xGetMemberCard(intParentRightID)

        intDeltaX = CInt((stParentRight.intX - stParentLeft.intX - mintMEM_CARD_W) / 2)

        lstChild = stParentLeft.lstChild


        If lstChild Is Nothing Then Return

        For i = 0 To lstChild.Count - 1

            xAddConnectorParent2ChildOfCouple(intParentLeftID, intParentRightID, lstChild(i), strPrefix)

        Next

    End Sub

    Private Sub xDrawLine(ByVal intRootID As Integer, _
                          ByVal strPrefix As String, _
                          Optional ByVal blnDrawSpouse As Boolean = True, _
                          Optional ByVal intPos As Integer = 1)

        Dim stRootCard As stCardInfo
        Dim i As Integer

        stRootCard = xGetMemberCard(CInt(intRootID))

        If Not stRootCard.lstSpouse Is Nothing And blnDrawSpouse Then

            If stRootCard.lstSpouse.Count = 1 Then
                'Connect to StepChild
                xDrawConnector2ListChild(intRootID, True, strPrefix, 1)

                'Connect to Child

                xDrawConnectorListChildOfCouple(intRootID, stRootCard.lstSpouse(0), strPrefix)
                'xDrawConnector2ListChild(intRootID, False, strPrefix, 1)

                'Connect to Spouse
                xAddConnectorToSpouse(intRootID, stRootCard.lstSpouse(0), strPrefix)

                'Call DrawLine for Child
                'Step child
                xDrawLineChild(intRootID, True, strPrefix)
                'Child
                xDrawLineChild(intRootID, False, strPrefix)

                xDrawLineChild(stRootCard.lstSpouse(0), True, strPrefix)
                xDrawConnector2ListChild(stRootCard.lstSpouse(0), True, strPrefix)

            Else

                xDrawLine(stRootCard.lstSpouse(0), strPrefix, False)
                xDrawConnector2ListChild(intRootID, True, strPrefix, 1)
                For i = 1 To stRootCard.lstSpouse.Count - 1
                    xDrawLine(stRootCard.lstSpouse(i), strPrefix, False, -1)
                Next

                xDrawLineChild(intRootID, True, strPrefix)
                xDrawConnector2ListSpouse(intRootID, strPrefix)

            End If

        Else

            If blnDrawSpouse Then
                xDrawConnector2ListChild(intRootID, True, strPrefix, 1)
                xDrawLineChild(intRootID, True, strPrefix)

            Else
                If intPos > 0 Then
                    xDrawConnector2ListChild(intRootID, True, strPrefix)
                    xDrawConnector2ListChild(intRootID, False, strPrefix, 1)

                    xDrawLineChild(intRootID, True, strPrefix)
                    xDrawLineChild(intRootID, False, strPrefix)
                Else
                    xDrawConnector2ListChild(intRootID, False, strPrefix, -1)
                    xDrawConnector2ListChild(intRootID, True, strPrefix)

                    xDrawLineChild(intRootID, False, strPrefix)
                    xDrawLineChild(intRootID, True, strPrefix)

                End If

            End If

        End If



    End Sub

#End Region


    '   ******************************************************************
    '　　　FUNCTION   : fncOpenPrintPreview, open file in print preview
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : blnInPrintPreviewMode   Boolean
    '      MEMO       : 
    '      CREATE     : 2011/12/12　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDisplay(Optional ByVal blnInPrintPreviewMode As Boolean = False) As Boolean

        fncDisplay = False

        Try
            'open in print preview mode
            mobjBook.Password = ""
            mobjBook.SaveAs(Me.SaveName)
            mobjApp.Visible = True
            If blnInPrintPreviewMode Then mobjSheet.PrintPreview()

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncSetCellData, set cell's data
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intRow          Integer
    '      PARAMS2    : intCol          Integer
    '      PARAMS3    : strValue        String
    '      PARAMS4    : intBorderWeight Integer
    '      PARAMS5    : intBgColor      Integer
    '      MEMO       : 
    '      CREATE     : 2011/12/12　AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncSetCellData(ByVal intRow As Integer, ByVal intCol As Integer, ByVal strValue As String, ByVal intBorderWeight As Integer, Optional ByVal intBgColor As Integer = -1) As Boolean

        fncSetCellData = False

        Try
            'set cell value
            mobjSheet.Cells(intRow, intCol) = strValue

            'cell border
            If intBorderWeight > 0 Then

                mobjSheet.Cells(intRow, intCol).Borders(gcintEdgeBorderBotton).LineStyle = gcintLineStyleContinuous
                mobjSheet.Cells(intRow, intCol).Borders(gcintEdgeBorderBotton).Weight = intBorderWeight

            End If

            'cell color if available
            If intBgColor <> -1 Then

                mobjSheet.Cells(intRow, intCol).Interior.ColorIndex = intBgColor
                mobjSheet.Cells(intRow, intCol).Font.ColorIndex = basConst.gcintXlsFontWhite

            End If

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncClose, close and dispose
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : blnCloseOnError  Boolean
    '      MEMO       : 
    '      CREATE     : 2011/12/12　AKB Quyet
    '      UPDATE     : 2012/10/08
    '   ******************************************************************
    Public Function fncClose(Optional ByVal blnCloseOnError As Boolean = False) As Boolean

        fncClose = False

        Try
            ' Dim strFolder As String

            ' strFolder = My.Application.Info.DirectoryPath & basConst.gcstrTempFolder

            'delete temp folder after using
            'basCommon.fncDeleteFolder(strFolder)

            'close and dispose object
            If blnCloseOnError Then
                If mobjBook IsNot Nothing Then mobjBook.Close(False)
                If mobjApp IsNot Nothing Then mobjApp.Quit()
            End If

            fncReleaseObject(mobjApp)
            fncReleaseObject(mobjBook)
            fncReleaseObject(mobjSheet)

            Return True

        Catch ex As Exception
            Throw ex
        End Try

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
            Dim ptStart As Point
            Dim ptEnd As Point
            Dim objLine As Object

            'draw normal line
            For i As Integer = 0 To lstNormalLine.Count - 1
                gintPercent = CInt(i * 60 / lstNormalLine.Count)
                ptStart = lstNormalLine(i).Location

                ptEnd = ptStart

                If lstNormalLine(i).LineDirection = clsEnum.emLineDirection.HORIZONTAL Then
                    ptEnd.X += lstNormalLine(i).Width
                Else
                    ptEnd.Y += lstNormalLine(i).Height
                End If

                'mobjSheet.Shapes.AddLine(ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y).Line.Weight = 2

                objLine = mobjSheet.Shapes.AddLine(ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y)
                objLine.Line.Weight = 2

            Next

            'draw special line
            For i As Integer = 0 To lstSpecialLine.Count - 1

                ptStart = lstSpecialLine(i).Location

                ptEnd = ptStart

                If lstSpecialLine(i).LineDirection = clsEnum.emLineDirection.HORIZONTAL Then
                    ptEnd.X += lstSpecialLine(i).Width
                Else
                    ptEnd.Y += lstSpecialLine(i).Height
                End If

                'mobjSheet.Shapes.AddLine(ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y).Line.Weight = 3

                objLine = mobjSheet.Shapes.AddLine(ptStart.X, ptStart.Y, ptEnd.X, ptEnd.Y)
                objLine.Line.Weight = 3
                objLine.Line.ForeColor.RGB = RGB(255, 0, 0)

            Next

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    ''   ****************************************************************** 
    ''      FUNCTION   : xSetCardSize, init value 
    ''      MEMO       :  
    ''      CREATE     : 2012/01/11  AKB Quyet 
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
    ''      CREATE     : 2011/12/13  AKB Quyet
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
    ''      CREATE     : 2011/09/14  AKB Quyet
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
    ''      CREATE     : 2011/12/13  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xDrawSameLv() As Boolean

    '    xDrawSameLv = False

    '    Try
    '        Dim pt1 As Point
    '        Dim pt2 As Point

    '        pt1 = mobjCardLeft.CardMidRight
    '        pt2 = mobjCardRight.CardMidLeft
    '        mobjSheet.Shapes.AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y).Line.Weight = 3

    '        pt1.Y -= 5
    '        pt2.Y -= 5
    '        mobjSheet.Shapes.AddLine(pt1.X, pt1.Y, pt2.X, pt2.Y).Line.Weight = 3

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
    ''      CREATE     : 2011/12/13  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xDrawDiffLv() As Boolean

    '    xDrawDiffLv = False

    '    Dim ptDraw(0 To 4, 0 To 1) As Single

    '    Try
    '        'calculate the collection of point to draw
    '        ptDraw(0, 0) = mobjCardLeft.CardMidRight.X
    '        ptDraw(0, 1) = mobjCardLeft.CardMidRight.Y

    '        ptDraw(1, 0) = mobjCardLeft.CardMidRight.X
    '        ptDraw(1, 0) += (mintMEM_CARD_SPACE_LEFT - mintMEM_CARD_W) \ 2
    '        ptDraw(1, 1) = mobjCardLeft.CardMidRight.Y

    '        ptDraw(2, 0) = ptDraw(1, 0)
    '        ptDraw(2, 1) = ptDraw(1, 1)
    '        ptDraw(2, 1) += mobjCardRight.CardMidTop.Y - mobjCardLeft.CardMidRight.Y - ((mintMEM_CARD_SPACE_DOWN - mintMEM_CARD_H) \ 2)

    '        ptDraw(3, 0) = mobjCardRight.CardMidTop.X
    '        ptDraw(3, 1) = mobjCardRight.CardMidTop.Y
    '        ptDraw(3, 1) -= ((mintMEM_CARD_SPACE_DOWN - mintMEM_CARD_H) \ 2)

    '        ptDraw(4, 0) = mobjCardRight.CardMidTop.X
    '        ptDraw(4, 1) = mobjCardRight.CardMidTop.Y

    '        mobjSheet.Shapes.AddPolyline(ptDraw)

    '        Return True

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        Erase ptDraw
    '    End Try

    'End Function

    'Vinhnn added 2012/10/03
    '   ******************************************************************
    '　　　FUNCTION   : fncExportTree, export F-tree to excel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : tblDrawControl      Hashtable
    '      PARAMS2    : tblNotDrawControl   Hashtable
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    'Public Function fncExportTreeAdvance(ByVal tblDrawControl As Hashtable, ByVal objTreeDraw As Object) As Boolean
    Public Function fncExportTreeAdvance(ByVal vobjStExport As Object) As Boolean
        fncExportTreeAdvance = False
        Dim objProgressThread As System.Threading.Thread = Nothing
        Dim objThreadExcel As System.Threading.Thread = Nothing

        Try
            xInit(vobjStExport)

            mobjPrgBar = New frmProgressBar
            objProgressThread = New System.Threading.Thread(AddressOf xGetProgress)

            If My.Settings.intCardSize = CInt(clsEnum.emCardSize.LARGE) Then

                If Not fncOpenTemplate(Application.StartupPath & "\docs\PhaHe.xls", 2, gcstrTemplatePass) Then Exit Function

            ElseIf My.Settings.intCardSize = CInt(clsEnum.emCardSize.SMALL) Then

                If Not fncOpenTemplate(Application.StartupPath & "\docs\PhaHe.xls", 3, gcstrTemplatePass) Then Exit Function

            End If



            If TypeOf mobjTreDraw Is clsDrawTreeS1 Then

                objThreadExcel = New System.Threading.Thread(AddressOf xExportTree1AdvancedS1)

            ElseIf TypeOf mobjTreDraw Is clsDrawTreeA1 Then

                objThreadExcel = New System.Threading.Thread(AddressOf xExportTree1AdvancedA1)

            End If



            objThreadExcel.Start()
            objProgressThread.Start()
            mobjPrgBar.ShowDialog()

            Return True
        Catch ex As Exception
            Throw ex
        Finally

            Do Until mblnExportComplete = True
                Application.DoEvents()
            Loop

            objProgressThread.Abort()

            objProgressThread = Nothing
            objThreadExcel.Abort()
            objThreadExcel = Nothing
            xEndTreeRenderAdvanced()

        End Try

    End Function

    Private Sub xEndTreeRenderAdvanced()
        If mblnExportSuccess Then
            basCommon.fncMessageInfo("Quá trình tạo tệp Excel đã thành công.")
            If Not xSaveTree() Then fncClose(True)
        Else
            basCommon.fncMessageError("Quá trình tạo tệp Excel không thành công !")
            fncClose(True)
        End If
    End Sub

    'Open file dialog to save tree file
    Private Function xSaveTree() As Boolean

        xSaveTree = False

        Try

            Dim dlgSaveFile As SaveFileDialog = New SaveFileDialog()
            dlgSaveFile.CheckPathExists = True
            dlgSaveFile.InitialDirectory = Application.StartupPath + "\List"
            dlgSaveFile.Title = "Giapha.xls"
            dlgSaveFile.Filter = "Excel files(*.xls)|*.xls|All files(*.*)|*.*"

            If dlgSaveFile.ShowDialog() = Windows.Forms.DialogResult.OK Then
                SaveName = dlgSaveFile.FileName
                mobjApp.DisplayAlerts = False
                fncDisplay()
            Else
                Return False
            End If
            fncClose()

            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function



    '   ******************************************************************
    '　　　FUNCTION   : xDrawTree1Reduce
    '      VALUE      : 
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xDrawTreeCard1NoImage()
        Try




        Catch ex As Exception

        End Try


    End Sub


    Private Sub xDrawCard(ByVal objCard As usrMemberCard1, _
                          ByVal blnHasImage As Boolean)



        Dim objShapeRange As Object = Nothing
        Dim objSelection As Object = Nothing

        Try
            Dim ptPosition As clsCoordinate
            Dim intShapeWidth As Integer
            Dim dblX As Double = 0
            Dim dblY As Double = 0

            ptPosition = objCard.CardCoor
            'If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then
            '    ptPosition.Y = xConvertFormYPositionToExcel(ptPosition.Y)
            '    ptPosition.X = xConvertFormXPositionToExcel(ptPosition.X)
            'End If
            ptPosition.Y = xConvertFormYPositionToExcel(ptPosition.Y)
            ptPosition.X = xConvertFormXPositionToExcel(ptPosition.X)
            Sheet.Paste()
            With Me.App
                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange
                objShapeRange.Name = "Grp" + objCard.CardID.ToString

                'Set Top Possison
                objShapeRange.Top = ptPosition.Y
                ' Set Left Possison
                objShapeRange.Left = ptPosition.X

                intShapeWidth = objShapeRange.Width

                objShapeRange.GroupItems(1).Select()
                .Selection.Name = "Pic" + objCard.CardID.ToString

                objShapeRange.GroupItems(2).Select()
                .Selection.Name = "txtData" + objCard.CardID.ToString

                'objShapeRange.GroupItems(3).Select()
                '.Selection.Name = "recImg" + intId.ToString
                'Ungroup 
                objShapeRange.Ungroup()
                'Get Info values
                Dim strInfo As String = xGetMemberInfo(objCard)

                If Not blnHasImage Then
                    'Set Info value to excel textbox
                    fncSetShapeText("", "", "txtData" + objCard.CardID.ToString, strInfo)
                Else
                    dblX = ptPosition.X + (intShapeWidth - xXLMemberImageWidth()) / 2
                    dblY = ptPosition.Y + 10
                    'Set Info value to excel textbox
                    fncSetShapeText("", "", "txtData" + objCard.CardID.ToString, strInfo, dblY + xXLMemberImageHeight())
                    'If objCard.CardGender <> Nothing Then
                    xInsertMemberImage(dblX, dblY, objCard.CardID, xGetMemberImagePath(objCard))

                    'End If
                End If
            End With

        Catch ex As Exception

        Finally
            fncReleaseObject(objShapeRange)
            fncReleaseObject(objSelection)
        End Try
        

    End Sub

    Private Function xGetMemberImagePath(ByVal objCard As usrMemberCard1)

        xGetMemberImagePath = My.Application.Info.DirectoryPath & "\docs\no_avatar_m.jpg"

        If objCard.CardImageLocation() <> "" Then Return objCard.CardImageLocation()

        If objCard.CardGender = clsEnum.emGender.FEMALE Then

            Return My.Application.Info.DirectoryPath & "\docs\no_avatar_f.jpg"

        ElseIf objCard.CardGender = clsEnum.emGender.UNKNOW Then

            Return My.Application.Info.DirectoryPath & "\docs\UnknownMember.jpg"

        End If

    End Function

    Private Sub xInsertMemberImage(ByVal dblX As Double, ByVal dblY As Double, _
                                   ByVal intID As Integer, ByVal strImagePath As String)

        fncInsertPicture(dblX, dblY, strImagePath, "Picture" + intID.ToString)
        fncSetShapeDimension("Picture" + intID.ToString, xXLMemberImageWidth(), xXLMemberImageHeight())

    End Sub

    Private Sub xInitExportTreeAdvanced()

        Dim objSelection As Object = Nothing
        Dim objShapeRange As Object = Nothing
        Dim objTxtShape As Object = Nothing
        Dim objFrameShape As Object = Nothing
        Dim objShape As Object = Nothing
        Dim strArrShape As Object()
        Try
            With Me.App
                'Change Frame
                strArrShape = New Object() {mcstrPicName, mcstrTxtData}

                If My.Settings.strCard1Bg <> "" Then
                    If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then
                        fncChangeFrame(mstrXLSAdvGroupName, mcstrPicName, My.Settings.strCard1Bg, strArrShape)
                    ElseIf My.Settings.intCardSize = clsEnum.emCardSize.SMALL Then
                        fncChangeFrame(mstrXLSAdvGroupName, mcstrPicName, My.Settings.strCard1Bg, strArrShape)
                    End If

                End If
                'End
                objShape = Sheet.Shapes(mstrXLSAdvGroupName)
                objShape.Select()
                objSelection = .Selection
                objSelection.Height = objSelection.Height + mcintXLHeightDelta

                objShapeRange = objSelection.ShapeRange
                objTxtShape = objShapeRange.GroupItems(2)
                If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then
                    objTxtShape.Top = objSelection.Top + mcintXLHeightDelta + xConvertFormYPositionToExcel(clsDefine.THUMBNAIL_H)
                ElseIf My.Settings.intCardSize = clsEnum.emCardSize.SMALL Then
                    objTxtShape.Top = objSelection.Top + mcintXLHeightDelta
                End If

                objTxtShape.Width = objShape.Width
                objTxtShape.Left = objShape.Left
                If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then
                    objTxtShape.Height = objSelection.Height - xConvertFormYPositionToExcel(clsDefine.THUMBNAIL_H) - mcintXLHeightDelta
                ElseIf My.Settings.intCardSize = clsEnum.emCardSize.SMALL Then
                    objTxtShape.Height = objSelection.Height - mcintXLHeightDelta
                End If
                

                objTxtShape.Select()
                objSelection.VerticalAlignment = -4160 'xlTop

                objShape.Select()
                mintXLMemberShapeWidth = objSelection.Width() * gintTreePanelDPIX / mintXLPtoI
                mdblXLMultiPlierX = mintXLMemberShapeWidth / clsDefine.MEM_CARD_W_L
                If My.Settings.intCardSize = clsEnum.emCardSize.LARGE Then
                    mdblXLMultiPlierY = objSelection.Height() * gintTreePanelDPIX / mintXLPtoI / clsDefine.MEM_CARD_H_L
                ElseIf My.Settings.intCardSize = clsEnum.emCardSize.SMALL Then
                    mdblXLMultiPlierY = objSelection.Height() * gintTreePanelDPIX / mintXLPtoI / clsDefine.MEM_CARD_H_S
                End If


                objSelection.Copy()
            End With
        Catch ex As Exception

        Finally
            fncReleaseObject(objShape)
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)
            fncReleaseObject(objTxtShape)
            fncReleaseObject(objFrameShape)
        End Try
    End Sub

    Private Function xXLMemberImageWidth() As Integer
        Return xConvertFormXPositionToExcel(clsDefine.THUMBNAIL_W * 1.3, False)
    End Function

    Private Function xXLMemberImageHeight() As Integer
        Return xConvertFormYPositionToExcel(clsDefine.THUMBNAIL_H * 1.3, False)
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncExportTree1AdvanceWithImage, export F-tree to excel with image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : tblDrawControl      Hashtable
    '      PARAMS2    : tblNotDrawControl   Hashtable
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xExportTree1AdvancedS1()

        mblnExportComplete = False
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Dim objSelection As Object = Nothing
        Dim blnHasImage As Boolean = My.Settings.intCardSize = CInt(clsEnum.emCardSize.LARGE)

        Dim objSheet As Object = Nothing

        mblnExportSuccess = False
        Try
            Dim objCard As usrMemberCard1
            Dim objCardTemp As usrMemberCard1
            Dim stCard As stCardInfo
            Dim intId As Integer

            Dim intPercent As Integer = 0
            'Dim stCard As stCardInfo

            Dim intCount As Integer = 0
            gintPercent = 0
            Dim tblCardInfo As Hashtable
            Dim tblDrawed As New Hashtable

            tblCardInfo = mobjTreDraw.DrawList()

            With Me.App
                xInitExportTreeAdvanced()
                objSelection = .Selection

                Sheet.Activate()
                Dim i As Integer = -1
                Dim j As Integer = 0

                For Each element As DictionaryEntry In tblCardInfo
                    i = i + 1
                    gintPercent = CInt((i + 1) * 80 / mobjHashTbl.Count)
                    objCard = CType(mobjHashTbl(element.Key), usrMemberCard1)
                    intId = CInt(objCard.CardID)

                    'if the member was not drawed, draw it
                    If Not tblDrawed.ContainsKey(element.Key) Then

                        xDrawCard(objCard, blnHasImage)
                        tblDrawed.Add(objCard.CardID, objCard.CardID)

                    End If

                    'Draw Spouse and draw connector
                    stCard = CType(tblCardInfo(element.Key), stCardInfo)
                    If Not stCard.lstSpouse Is Nothing Then
                        For j = 0 To stCard.lstSpouse.Count - 1
                            objCardTemp = CType(mobjHashTbl(stCard.lstSpouse(j)), usrMemberCard1)

                            'if the spouse was not drawed, draw it
                            If Not tblDrawed.ContainsKey(objCardTemp.CardID) Then
                                xDrawCard(objCardTemp, blnHasImage)
                                tblDrawed.Add(objCardTemp.CardID, objCardTemp.CardID)
                            End If

                            xAddConnectorToSpouse(intId, stCard.lstSpouse(j), "Pic")
                            intId = stCard.lstSpouse(j)

                        Next

                    End If

                    'Draw Children and draw connector
                    intId = CInt(objCard.CardID)
                    If Not stCard.lstChild Is Nothing Then
                        For j = 0 To stCard.lstChild.Count - 1

                            objCardTemp = CType(mobjHashTbl(stCard.lstChild(j)), usrMemberCard1)

                            'if the child was not drawed, draw it
                            If Not tblDrawed.ContainsKey(objCardTemp.CardID) Then
                                xDrawCard(objCardTemp, blnHasImage)
                                tblDrawed.Add(objCardTemp.CardID, objCardTemp.CardID)
                            End If

                            xAddConnectorParent2Child(intId, stCard.lstChild(j), "Pic", True)

                        Next
                    End If

                Next

            End With

            'Group Shapes
            xMakeGroupMemberInfoControl(mobjHashTbl, intPercent, blnHasImage)
            'Delete(Template)
            fncDeleteShape(mstrXLSAdvGroupName)
            'Select Cell A1
            fncSetCellSelect(1, 1)
            fncDeleteSheet(1)
            If blnHasImage Then
                '    'Delete Sheet unuse
                fncDeleteSheet(2)

            Else

                fncDeleteSheet(3)

            End If

            mblnExportSuccess = True
            Return

        Catch ex As Exception

            fncSaveErr(mcstrclsName, "xExportTree1Advanced", ex)
            Throw ex

        Finally

            fncReleaseObject(objSelection)
            fncReleaseObject(objSheet)
            gintPercent = 100

            'mobjPrgBar.CloseTheForm()
            mblnExportComplete = True
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : fncExportTree1AdvanceWithImage, export F-tree to excel with image
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : tblDrawControl      Hashtable
    '      PARAMS2    : tblNotDrawControl   Hashtable
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xExportTree1AdvancedA1()

        mblnExportComplete = False
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Dim objSelection As Object = Nothing
        'Dim objShape As Object = Nothing
        Dim blnHasImage As Boolean = My.Settings.intCardSize = CInt(clsEnum.emCardSize.LARGE)

        Dim objSheet As Object = Nothing

        mblnExportSuccess = False
        Try
            Dim objCard As usrMemberCard1
            Dim intId As Integer

            Dim intPercent As Integer = 0
            'Dim stCard As stCardInfo

            Dim intCount As Integer = 0
            gintPercent = 0
            Dim tblCardInfo As Hashtable
            Dim tblDrawed As New Hashtable

            tblCardInfo = mobjTreDraw.DrawList()


            With Me.App
                xInitExportTreeAdvanced()
                'objShape = Sheet.Shapes(mstrXLSAdvGroupName)
                'objShape.Select()
                objSelection = .Selection
                'objSelection.Copy()

                'Sheet.Activate()
                Dim i As Integer = -1
                Dim j As Integer = 0

                For Each element As DictionaryEntry In tblCardInfo
                    i = i + 1
                    gintPercent = CInt((i + 1) * 80 / mobjHashTbl.Count)
                    objCard = CType(mobjHashTbl(element.Key), usrMemberCard1)
                    intId = CInt(objCard.CardID)

                    'if the member was not drawed, draw it
                    If Not tblDrawed.ContainsKey(element.Key) Then
                        xDrawCard(objCard, blnHasImage)
                        tblDrawed.Add(objCard.CardID, objCard.CardID)
                    End If
                Next

                'DrawLine
                xDrawLine(mintRootID, "Pic")
            End With

            'Group Shapes
            'xMakeGroupMemberInfoControl(mobjHashTbl, intPercent, blnHasImage)

            'Delete(Template)
            fncDeleteShape(mstrXLSAdvGroupName)
            fncSetCellSelect(1, 1)
            fncDeleteSheet(1)
            If blnHasImage Then
                '    'Delete Sheet unuse
                fncDeleteSheet(2)

            Else

                fncDeleteSheet(3)

            End If

            mblnExportSuccess = True
            Return

        Catch ex As Exception

            fncSaveErr(mcstrclsName, "xExportTree1Advanced", ex)
            Throw ex

        Finally

            'fncReleaseObject(objShape)
            fncReleaseObject(objSelection)
            fncReleaseObject(objSheet)
            gintPercent = 100

            'mobjPrgBar.CloseTheForm()
            mblnExportComplete = True
        End Try

    End Sub



    Private Function xMakeGroupMemberInfoControl(ByVal tblDrawControl As Hashtable, _
                                                 ByRef intPercent As Integer, _
                                                 Optional ByVal blnHasImage As Boolean = True)
        xMakeGroupMemberInfoControl = False
        Try

            'Group Shapes
            For Each element As DictionaryEntry In tblDrawControl

                Dim ArrStrShape As Object() = Nothing
                Dim intID As Integer

                gintPercent += CInt((intPercent * 19) / tblDrawControl.Count)

                intID = CInt(element.Key)

                If blnHasImage Then
                    ArrStrShape = New Object() {"Pic" + intID.ToString, "txtData" + intID.ToString, "Picture" + intID.ToString}
                Else
                    ArrStrShape = New Object() {"Pic" + intID.ToString, "txtData" + intID.ToString}
                End If


                fncMakeGroup(ArrStrShape, "Group" & intID.ToString())
                intPercent += 1

            Next

            xMakeGroupMemberInfoControl = True

        Catch ex As Exception

        End Try

    End Function

    Private Function xDrawConnectorFatherAndChilds(ByVal objCard As usrMemberCard1, _
                                                   ByVal intPicIndex As Integer) As Boolean
        xDrawConnectorFatherAndChilds = False
        Try


            'Draw connecter
            'Connect(Father And Childs)
            If objCard.ParentID <> basConst.gcintNONE_VALUE Then
                If objCard.ParentID <> basConst.gcintNONE_VALUE Then
                    If basCommon.fncIsFhead(intPicIndex) And basCommon.fncIsFhead(objCard.ParentID) Then
                        fncAddConnector("Pic" + objCard.ParentID.ToString, "Pic" + intPicIndex.ToString, True, , gdblFaChildConnWeight, 2, False)
                    Else
                        fncAddConnector("Pic" + objCard.ParentID.ToString, "Pic" + intPicIndex.ToString, True, , gdblFaChildConnWeight)
                    End If

                End If
            End If


            'Connect parents
            If objCard.SpouseID <> basConst.gcintNONE_VALUE Then
                fncAddConnector("Pic" + objCard.SpouseID.ToString, "Pic" + intPicIndex.ToString, False, , gdblParentConnWeight)
            End If

            Return True
        Catch ex As Exception

        End Try

    End Function

    Private Function xGetMemberInfo(ByVal objCard As usrMemberCard1) As String

        xGetMemberInfo = ""

        'Get Info values
        Dim strInfo As String = ""

        strInfo = objCard.CardName + vbCrLf
        If objCard.CardName.IndexOf(vbCrLf) < 0 Then
            strInfo = strInfo + vbCrLf
        End If

        'strInfo += vbCrLf + objCard.CardBirth
        'strInfo += vbCrLf + objCard.CardDeath

        'If objCard.CardGender <> Nothing Then
        '    If objCard.CardGender = 1 Then
        '        strInfo += vbCrLf + "Nam"
        '    Else
        '        strInfo += vbCrLf + "Nữ"
        '    End If

        'End If

        Return strInfo

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetProgress
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Private Sub xGetProgress()
        Try


            Do Until mblnExportComplete 'gintPercent = 100

                System.Threading.Thread.Sleep(500)
                mobjPrgBar.UpdatePro(gintPercent)

            Loop

            mobjPrgBar.CloseTheForm()

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "xGetProgress", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : fncSetShapeText
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Private Function fncSetShapeText(ByVal strTxtInfoName As String, _
                                     ByVal strInfo As String, _
                                     ByVal strTxtName As String, _
                                     ByVal strName As String, _
                                     Optional ByVal intTop As Integer = -1, _
                                     Optional ByVal intLeft As Integer = -1) As Boolean

        fncSetShapeText = False
        Try
            If strTxtInfoName <> "" Then
                fncSetShapeText(strTxtInfoName, strInfo, intTop, intLeft)
            End If

            fncSetShapeText(strTxtName, strName, intTop, intLeft)

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncSetShapeText", ex)
        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncDeleteShape
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDeleteShape(ByVal strShapeName As String) As Boolean
        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        fncDeleteShape = False
        Try
            objShape = Sheet.Shapes(strShapeName)
            objShape.Select()
            With Me.App
                objSelection = .Selection
                objSelection.Delete()
            End With

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncDeleteShape", ex)
        Finally
            fncReleaseObject(objSelection)
            fncReleaseObject(objShape)
            fncReleaseObject(objShapeRange)
        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncDeleteSheet
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncDeleteSheet(ByVal intSheetIndex As Integer) As Boolean
        fncDeleteSheet = False
        Try

            With Me.App
                .DisplayAlerts = False
                .ActiveWorkbook.Sheets(intSheetIndex).Delete()
                .DisplayAlerts = True
            End With

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncDeleteSheet", ex)
        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncSetShapeAutoSize
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncSetShapeAutoSize(ByVal strShapeName As String) As Boolean

        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        Dim objCharacters As Object = Nothing

        Try

            With Me.App
                objShape = Sheet.Shapes(strShapeName)
                objShape.TextFrame.MultiLine = True
                objShape.TextFrame.WordWrap = True
                objShape.TextFrame.AutoSize = True

            End With

        Catch ex As Exception

            fncSaveErr(mcstrclsName, "SetShapeText", ex)

        Finally

            fncReleaseObject(objCharacters)
            fncReleaseObject(objShape)
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)

        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : SetShapeText
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncSetShapeText(ByVal strShapeName As String, _
                                    ByVal strValue As String, _
                                    Optional ByVal intTop As Integer = -1, _
                                    Optional ByVal intLeft As Integer = -1) As Boolean

        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        Dim objCharacters As Object = Nothing

        Try

            With Me.App
                objShape = Sheet.Shapes(strShapeName)
                objShape.Select()
                objSelection = .Selection
                objCharacters = objSelection.Characters
                objCharacters.Text = strValue
                If intTop >= 0 Then
                    objShape.Top = intTop
                End If

                If intLeft >= 0 Then
                    objShape.Left = intLeft
                End If

            End With

        Catch ex As Exception

            fncSaveErr(mcstrclsName, "SetShapeText", ex)

        Finally

            fncReleaseObject(objCharacters)
            fncReleaseObject(objShape)
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)

        End Try
    End Function
    '   ******************************************************************
    '　　　FUNCTION   : fncGetShapePos
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncGetShapePos(ByVal strShapeName As String, _
                                   ByRef dblXCoord As Double, _
                                   ByRef dblYCoord As Double) As Boolean

        fncGetShapePos = False
        Dim objShape As Object = Nothing

        Try

            With Me.App
                objShape = Sheet.Shapes(strShapeName)
                dblXCoord = CDbl(objShape.Left)
                dblYCoord = CDbl(objShape.Top)

            End With

        Catch ex As Exception

            fncSaveErr(mcstrclsName, "fncGetShapePos", ex)

        Finally

            fncReleaseObject(objShape)

        End Try
    End Function
    '   ******************************************************************
    '　　　FUNCTION   : fncGetShapeDimension
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncGetShapeDimension(ByVal strShapeName As String, _
                                   ByRef dblWidth As Double, _
                                   ByRef dblHeight As Double) As Boolean

        fncGetShapeDimension = False
        Dim objShape As Object = Nothing

        Try

            With Me.App
                objShape = Sheet.Shapes(strShapeName)
                dblWidth = objShape.Width
                dblHeight = objShape.Height

            End With

        Catch ex As Exception

            fncSaveErr(mcstrclsName, "fncGetShapeDimension", ex)

        Finally

            fncReleaseObject(objShape)

        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncSetShapeDimension
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncSetShapeDimension(ByVal strShapeName As String, _
                                    Optional ByVal dblWidth As Double = 0, _
                                    Optional ByVal dblHeight As Double = 0, _
                                    Optional ByVal dblXCoord As Double = 0, _
                                    Optional ByVal dblYCoord As Double = 0) As Boolean

        fncSetShapeDimension = False
        Dim objShape As Object = Nothing

        Try

            With Me.App
                objShape = Sheet.Shapes(strShapeName)

                objShape.LockAspectRatio = 0
                objShape.Placement = 1
                If dblWidth <> 0 Then
                    objShape.Width() = dblWidth
                End If
                If dblHeight <> 0 Then
                    objShape.Height = dblHeight
                End If

                If dblYCoord <> 0 Then

                    objShape.Top = dblYCoord

                End If
                If dblXCoord <> 0 Then

                    objShape.Left = dblXCoord

                End If
            End With

        Catch ex As Exception

            fncSaveErr(mcstrclsName, "fncGetShapeDimension", ex)

        Finally

            fncReleaseObject(objShape)

        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncAddConnector
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncAddConnector(ByVal strShapesFa As String, _
                                       ByVal strShapesChild As String, _
                                       ByVal blnIsParent As Boolean, _
                                       Optional ByVal DashStyle As Integer = 1, _
                                       Optional ByVal LineWeight As Double = 1.5, _
                                       Optional ByVal ColorIndex As Integer = 64, _
                                       Optional ByVal blnSendtoBack As Boolean = True) As Boolean

        Dim objShape As Object = Nothing
        Dim objShape1 As Object = Nothing
        Dim objShape2 As Object = Nothing
        Dim objSelection As Object = Nothing
        Dim objShapeRange As Object = Nothing
        Dim objShapeRangeLine As Object = Nothing
        Dim objShapeRangeLineFC As Object = Nothing
        Dim x1 As Double = 0
        Dim x2 As Double = 0
        Dim y1 As Double = 0
        Dim y2 As Double = 0

        Try
            With Me.App
                objShape1 = Sheet.Shapes(strShapesFa)
                objShape2 = Sheet.Shapes(strShapesChild)
                With objShape1
                    x1 = .Left + .Width / 2
                    y1 = .Top + .Height
                End With

                With objShape2
                    x2 = .Left + .Width / 2
                    y2 = .Top
                End With

                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")
                If CInt(.Version) < 12 Then
                    x2 = x2 - x1
                    y2 = y2 - y1
                End If

                If blnIsParent = False Then
                    objShape = Sheet.Shapes.AddConnector(1, x1, y1, x2, y2)
                Else
                    objShape = Sheet.Shapes.AddConnector(2, x1, y1, x2, y2)
                End If

                objShape.Select()
                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange
                Sheet.Activate()
                If blnIsParent = True Then
                    objShapeRange.ConnectorFormat.BeginConnect(objShape1, 3)
                    objShapeRange.ConnectorFormat.EndConnect(objShape2, 1)
                Else
                    objShapeRange.ConnectorFormat.BeginConnect(Sheet.Shapes(strShapesFa), 4)
                    objShapeRange.ConnectorFormat.EndConnect(Sheet.Shapes(strShapesChild), 2)
                End If

                If blnSendtoBack = True Then
                    objShapeRange.ZOrder(1)
                End If
                objShapeRangeLine = objShapeRange.Line
            End With


            With objShapeRangeLine
                .Weight = LineWeight
                .DashStyle = DashStyle

                objShapeRangeLineFC = .ForeColor
            End With

            With objShapeRangeLineFC
                .SchemeColor = ColorIndex
            End With



            Return True

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "AddLine", ex)

        Finally
            fncReleaseObject(objShape)
            fncReleaseObject(objShape1)
            fncReleaseObject(objShape2)
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)
            fncReleaseObject(objShapeRangeLine)
            fncReleaseObject(objShapeRangeLineFC)

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncAddConnector
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncAddConnectorParent2Child(ByVal strShapesFa As String, _
                                                ByVal strShapesChild As String, _
                                                Optional ByVal intXOffset As Integer = 30, _
                                                Optional ByVal intYOffset As Integer = 0, _
                                                Optional ByVal DashStyle As Integer = 1, _
                                                Optional ByVal LineWeight As Double = 1.5, _
                                                Optional ByVal ColorIndex As Integer = 64, _
                                                Optional ByVal blnSendtoBack As Boolean = True, _
                                                Optional ByVal blnMiddle As Boolean = False) As Boolean

        Dim objConnector As Object = Nothing

        Dim objFather As Object = Nothing
        Dim objChild As Object = Nothing
        Dim objTempShape As Object = Nothing

        Dim objSelection As Object = Nothing
        Dim objShapeRange As Object = Nothing
        Dim objShapeRangeLine As Object = Nothing
        Dim objShapeRangeLineFC As Object = Nothing
        Dim x1 As Double = 0
        Dim x2 As Double = 100
        Dim y1 As Double = 0
        Dim y2 As Double = 100

        Try



            With Me.App
                objFather = Sheet.Shapes(strShapesFa)
                objChild = Sheet.Shapes(strShapesChild)

                System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")


                If intXOffset > 0 Then

                    objTempShape = Sheet.Shapes.AddShape(1, objFather.Left + objFather.Width + intXOffset, objFather.Top + CInt(objFather.Height / 2), 1, 1)

                Else

                    objTempShape = Sheet.Shapes.AddShape(1, objFather.Left + intXOffset, objFather.Top + CInt(objFather.Height / 2), 1, 1)

                End If


                objConnector = Sheet.Shapes.AddConnector(2, x1, y1, x2, y2)
                objConnector.Select()

                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange
                Sheet.Activate()


                objShapeRange.ConnectorFormat.BeginConnect(objTempShape, 3)
                objShapeRange.ConnectorFormat.EndConnect(objChild, 1)
                Dim dblBuffer As Double
                Dim dblTotalHeight As Single
                dblBuffer = objChild.Top - objFather.Top - objFather.Height

                dblTotalHeight = objChild.Top - objFather.Top - objFather.Height / 2

                Try
                    objShapeRange.Adjustments.Item(1) = (objFather.Height + dblBuffer - xConvertFormYPositionToExcel(LineWeight)) / 2 / dblTotalHeight
                Catch ex As Exception
                End Try

                If blnSendtoBack = True Then
                    objShapeRange.ZOrder(1)
                End If
                objShapeRangeLine = objShapeRange.Line
            End With

            With objShapeRangeLine
                .Weight = LineWeight
                .DashStyle = DashStyle

                objShapeRangeLineFC = .ForeColor
            End With

            With objShapeRangeLineFC
                .SchemeColor = ColorIndex
            End With

            Return True

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "AddLine", ex)

        Finally
            fncReleaseObject(objConnector)
            fncReleaseObject(objTempShape)
            fncReleaseObject(objFather)
            fncReleaseObject(objChild)
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)
            fncReleaseObject(objShapeRangeLine)
            fncReleaseObject(objShapeRangeLineFC)

        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncInsertPicture
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncInsertPicture(ByVal sngXconn As Single, _
                                 ByVal sngYconn As Single, _
                                 ByVal strFile As String, _
                                 ByVal strPicName As String) As Boolean


        Dim objRange As Object = Nothing
        Dim objPictures As Object = Nothing
        Dim objSelection As Object = Nothing
        Dim objShapeRange As Object = Nothing

        Try
            objPictures = Sheet.Pictures

            With objPictures.Insert(strFile)
                .Top = sngYconn
                .Left = sngXconn
                .Select()
            End With

            With Me.App
                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange
                objSelection.Name = strPicName
            End With
            Return True
        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncInsertPicture", ex)

        Finally
            fncReleaseObject(objRange)
            fncReleaseObject(objPictures)
        End Try

    End Function
    '   ******************************************************************
    '　　　FUNCTION   : fncUnGroup
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncUnGroup(ByVal strShapeName As String) As Boolean
        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        fncUnGroup = False
        Try
            With Me.App
                objShape = Sheet.Shapes(strShapeName)
                objShape.Select()
                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange
                objShapeRange.Ungroup()
            End With

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncUnGroup", ex)
        Finally
            fncReleaseObject(objSelection)
            fncReleaseObject(objShape)
            fncReleaseObject(objShapeRange)
        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncSetCellSelect
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20013/06/24　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncSetCellSelect(ByVal intRow As Integer, ByVal intCol As Integer) As Boolean
        Dim objRange As Object = Nothing
        Try

            objRange = mobjSheet.Range(CalC2A(intCol) & intRow, CalC2A(intCol) & intRow)
            objRange.select()
        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncSetCellSelect", ex)
        Finally
            fncReleaseObject(objRange)
        End Try
    End Function
    '   ******************************************************************
    '　　　FUNCTION   : fncReGroup
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncReGroup(ByVal strShapeName As String) As Boolean
        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        fncReGroup = False
        Try
            With Me.App
                objShape = Sheet.Shapes(strShapeName)
                objShape.Select()
                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange
                objShapeRange.Regroup()
            End With

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncReGroup", ex)
        Finally
            fncReleaseObject(objSelection)
            fncReleaseObject(objShape)
            fncReleaseObject(objShapeRange)
        End Try
    End Function
    '   ******************************************************************
    '　　　FUNCTION   : fncMakeGroup
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncMakeGroup(ByVal ArrStrShapeName As Object(), _
                                 Optional ByVal strGroupName As String = "") As Boolean
        Dim objSelection As Object = Nothing
        Dim objShapeRange As Object = Nothing
        fncMakeGroup = False
        Try
            Sheet.Shapes.Range(ArrStrShapeName).Group().Select()
            With Me.App
                'objSelection = .Selection
                'objShapeRange = objSelection.ShapeRange
                'objShapeRange.Group.Select()
                ''Set name to Group
                If strGroupName <> "" Then
                    objSelection = .Selection
                    objSelection.Name = strGroupName
                End If

            End With

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncMakeGroup", ex)
        Finally
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)
        End Try
    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncCopyShape template 1
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncCopyShapeTemp1(ByVal strShapeName As String, _
                                 ByVal strNewPic As String, _
                                 ByVal strNewtxtName As String, _
                                 Optional ByVal strNewRecName As String = "", _
                                     Optional ByVal strNewName As String = "", _
                                     Optional ByVal strSheetName As String = "", _
                                     Optional ByVal blnGrpExplode As Boolean = False, _
                                     Optional ByVal sngTop As Double = -999, _
                                     Optional ByVal sngLeft As Double = -999) As Boolean

        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        Dim objSheet As Object = Nothing

        Try
            With Me.App

                If strSheetName = "" Then
                    objShape = Sheet.Shapes(strShapeName)
                Else
                    objSheet = mobjSheet.Item(strSheetName)
                    objSheet.Activate()
                    objShape = objSheet.Shapes(strShapeName)
                End If

                objShape.Select()
                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange

                objSelection.Copy()
                Sheet.Activate()
                Sheet.Paste()

                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange

                If strNewName <> "" Then
                    objShapeRange.Name = strNewName
                End If
                'Set Top Possison
                If sngTop <> -999 Then
                    objShapeRange.Top = sngTop
                End If
                ' Set Left Possison
                If sngLeft <> -999 Then
                    objShapeRange.Left = sngLeft
                End If


                If blnGrpExplode = True Then
                    If strNewRecName <> "" Then
                        objShapeRange.GroupItems(1).Select()
                        .Selection.Name = strNewPic

                        objShapeRange.GroupItems(2).Select()
                        .Selection.Name = strNewtxtName

                        objShapeRange.GroupItems(3).Select()
                        .Selection.Name = strNewRecName
                        'Ungroup 
                        objShapeRange.Ungroup()
                    Else
                        objShapeRange.GroupItems(1).Select()
                        .Selection.Name = strNewPic

                        objShapeRange.GroupItems(2).Select()
                        .Selection.Name = strNewtxtName
                        'Ungroup 
                        objShapeRange.Ungroup()
                    End If

                End If

            End With

            Return True

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "CopyShapePos", ex)

        Finally

            fncReleaseObject(objShape)
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)
            fncReleaseObject(objSheet)
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncCopyShape template 2
    '　　　VALUE      : 
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 20012/10/10　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************

    Public Function fncCopyShapeTemp2(ByVal strShapeName As String, _
                                 ByVal strNewRec As String, _
                                 ByVal strNewtxtInfo As String, _
                                 ByVal strNewtxtNote As String, _
                                     Optional ByVal strNewRecImg As String = "", _
                                     Optional ByVal strNewName As String = "", _
                                     Optional ByVal strSheetName As String = "", _
                                     Optional ByVal blnGrpExplode As Boolean = False, _
                                     Optional ByVal sngTop As Single = -999, _
                                     Optional ByVal sngLeft As Single = -999) As Boolean

        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        Dim objSheet As Object = Nothing

        Try
            With Me.App

                If strSheetName = "" Then
                    objShape = Sheet.Shapes(strShapeName)
                Else
                    objSheet = mobjSheet.Item(strSheetName)
                    objSheet.Activate()
                    objShape = objSheet.Shapes(strShapeName)
                End If

                objShape.Select()
                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange

                objSelection.Copy()
                Sheet.Activate()
                Sheet.Paste()

                objSelection = .Selection
                objShapeRange = objSelection.ShapeRange

                If strNewName <> "" Then
                    objShapeRange.Name = strNewName
                End If

                If sngTop <> -999 Then
                    objShapeRange.Top = sngTop
                End If

                If sngLeft <> -999 Then
                    objShapeRange.Left = sngLeft
                End If

                If blnGrpExplode = True Then
                    If strNewRecImg <> "" Then
                        objShapeRange.GroupItems(1).Select()
                        .Selection.Name = strNewRec
                        objShapeRange.GroupItems(2).Select()
                        .Selection.Name = strNewRecImg

                        objShapeRange.GroupItems(3).Select()
                        .Selection.Name = strNewtxtInfo

                        objShapeRange.GroupItems(4).Select()
                        .Selection.Name = strNewtxtNote
                    Else
                        objShapeRange.GroupItems(1).Select()
                        .Selection.Name = strNewRec

                        objShapeRange.GroupItems(2).Select()
                        .Selection.Name = strNewtxtInfo

                        objShapeRange.GroupItems(3).Select()
                        .Selection.Name = strNewtxtNote
                    End If

                    objShapeRange.Ungroup()

                End If

            End With

            Return True

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "fncCopyShapeTemp2", ex)

        Finally

            fncReleaseObject(objShape)
            fncReleaseObject(objSelection)
            fncReleaseObject(objShapeRange)
            fncReleaseObject(objSheet)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : OpenExcel
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : tblDrawControl      Hashtable
    '      PARAMS2    : tblNotDrawControl   Hashtable
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function OpenExcel(ByVal strFile As String, _
                              Optional ByVal intSheetActive As Integer = 1, _
                              Optional ByVal blnReadOnly As Boolean = True) As Boolean

        Try

            mobjApp = CreateObject("Excel.Application")
            mobjBook = mobjApp.Workbooks

            If blnReadOnly = True Then

                mobjBook = mobjBook.Add(template:=strFile)
                mobjSheet = mobjBook.Worksheets
                mobjSheet = mobjSheet.Item(intSheetActive)

            Else
                mobjBook = mobjBook.Open(strFile)
                mobjSheet = mobjBook.Worksheets
                mobjSheet = mobjSheet.Item(intSheetActive)
            End If

            mobjSheet.Activate()

            Return True

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "OpenExcel", ex)

        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : ActiveSheet
    '      VALUE      : boolean, true - success, false - failure
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************    
    Public Function ActiveSheet(ByVal intSheetNo As Integer) As Boolean

        Try

            Sheet = Sheets.Item(intSheetNo)
            Sheet.Activate()

            Return True

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "ActiveSheet", ex)

        Finally
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : ActiveSheet
    '      VALUE      : boolean, true - success, false - failure
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************     
    Public Function ActiveSheet(ByVal strSheetName As String) As Boolean

        Try

            Sheet = mobjSheet.Item(strSheetName)
            Sheet.Activate()

            Return True

        Catch ex As Exception
            'fncSaveErr(mcstrclsName, "ActiveSheet", ex)
            Return False
        Finally
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : GetCellData
    '      VALUE      : boolean, true - success, false - failure
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetCellData(ByVal intSRow As Integer, _
                                    ByVal intSCol As Integer) As Object

        Dim objRange As Object = Nothing
        Dim objSheet As Object = Nothing
        Dim objCellData As Object = Nothing

        Try
            With Me.App

                objRange = mobjSheet.Range(CalC2A(intSCol) & intSRow, CalC2A(intSCol) & intSRow)

                objCellData = objRange.Value

            End With

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "GetCellData", ex)

        Finally
            fncReleaseObject(objRange)
            fncReleaseObject(objSheet)

        End Try

        Return objCellData

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : CalC2A
    '      VALUE      : boolean, true - success, false - failure
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function CalC2A(ByVal lngCol As Long) As String

        Dim strWork As String = Nothing
        Dim lngWork As Long

        Try

            CalC2A = ""

            'A1へ変換
            strWork = ""

            '上の桁


            lngWork = (lngCol - 1) \ 26
            If lngWork >= 1 Then
                strWork = Chr(64 + lngWork)
            End If

            '下の桁


            lngWork = (lngCol - 1) Mod 26
            lngWork = lngWork + 65
            strWork = strWork + Chr(lngWork)

        Catch ex As Exception
            fncSaveErr(mcstrclsName, "CalC2A", ex)
        Finally

        End Try

        Return strWork

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : SetCellData
    '      VALUE      : boolean, true - success, false - failure
    '      MEMO       : 
    '      CREATE     : 2012/09/24　AKB Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncSetCellData(ByVal intCol As Integer, _
                                   ByVal intRow As Integer, _
                                   ByVal objValue As Object) As Boolean

        Dim objRange As Object = Nothing    'セル範囲
        Dim strCell As String               'セル範囲指定用の文字列

        Try

            'セル範囲の文字列を作成
            strCell = CalC2A(intCol) + CStr(intRow)

            'セル範囲を取得


            objRange = Me.App.Range(CalC2A(intCol) + CStr(intRow))

            '貼り付け実行


            objRange.Value = objValue

            Return True
        Catch ex As Exception
            fncSaveErr(mcstrclsName, "SetCellData", ex)
        Finally
            fncReleaseObject(objRange)
        End Try

    End Function
    '   ******************************************************************
    '　　　FUNCTION   : Thay doi khung cua template
    '　　　VALUE      : Boolean
    '      PARAMS     : strShapeName - Ten cua Template
    '                   strCurTemp - Ten cua khung bao ben ngoai
    '                   strNewTempPath - Duong dan den anh cua khung moi
    '                   strArrShape danh sach cac shape can group lai
    '      MEMO       : 
    '      CREATE     : 20012/12/26　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncChangeFrame(ByVal strShapeName As String, _
                                ByVal strCurTemp As String, _
                                ByVal strNewTempPath As String, _
                                ByVal strArrShape As Object()) As Boolean
        Try

            Dim dblX As Double
            Dim dblY As Double
            Dim dblShapeWidth As Double
            Dim dblShapeHeight As Double


            'ungroup template
            fncUnGroup(strShapeName)
            'Lay toa do cua khung hien tai
            fncGetShapePos(strCurTemp, dblX, dblY)
            'Lay Kich thuoc cua khung hien tai
            fncGetShapeDimension(strCurTemp, dblShapeWidth, dblShapeHeight)
            'Xoa Khung hien tai
            fncDeleteShape(strCurTemp)
            'Them Khung moi
            fncInsertPicture(dblX, dblY, strNewTempPath, strCurTemp)
            'Set Kich Thuoc Shape moi
            fncSetShapeDimension(strCurTemp, dblShapeWidth, dblShapeHeight)
            'Set Shape send to back
            xSetShapeSendToBack(strCurTemp)
            'Group template moi
            fncMakeGroup(strArrShape, strShapeName)


        Catch ex As Exception

        End Try
    End Function
    '   ******************************************************************
    '　　　FUNCTION   : xSetShapeSendToBack
    '　　　VALUE      : Boolean
    '      PARAMS     : strShapeName - Ten cua Shape can send to back
    '      MEMO       : 
    '      CREATE     : 20012/12/26　AKB　Vinh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSetShapeSendToBack(ByVal strShapeName As String) As Boolean

        Dim objSelection As Object = Nothing
        Dim objShape As Object = Nothing
        Dim objShapeRange As Object = Nothing
        With Me.App
            objShape = Sheet.Shapes(strShapeName)
            objShape.Select()
            objSelection = .Selection
            objShapeRange = objSelection.ShapeRange
            objShapeRange.ZOrder(1)
        End With

    End Function
    'Vinhnn end added 2012/10/03


End Class