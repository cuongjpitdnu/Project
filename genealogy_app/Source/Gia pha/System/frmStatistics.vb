'   ******************************************************************
'      TITLE      : MAIN FORM
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/09/14　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Form Main class
'      MEMO       : 
'      CREATE     : 2011/09/14  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmStatistics

    Private Const mcstrClsName As String = "frmStatistics"             'class name

    Dim mstStats As stStatistic                                         'structure of info

    Dim mfrmWaiting As frmProgress                                      'waiting screen
    Dim mfrmAnni As frmPersonalAnniversary                              'anniversary screen
    Dim mobjLoadingThread As System.Threading.Thread                    'waiting process

    'structure of information
    Private Structure stStatistic

        Dim intTotalMem As Integer
        Dim intMale As Integer
        Dim intFemale As Integer
        Dim intUnknow As Integer
        Dim intDecease As Integer
        Dim intAlive As Integer
        Dim int0to5 As Integer
        Dim int6to17 As Integer
        Dim int18to35 As Integer
        Dim int36to55 As Integer
        Dim int56to70 As Integer
        Dim int71toHigher As Integer

    End Structure


#Region "Form Events"

    '   ******************************************************************
    '　　　FUNCTION   : frmStatistics_Load, show form
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmStatistics_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            xFormLoad()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmStatistics_Load", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, cancel button clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : LinkLabel1_LinkClicked, link clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblBirth.LinkClicked
        Try
            mfrmWaiting = New frmProgress()
            mobjLoadingThread = New System.Threading.Thread(AddressOf xShowAnniBirth)

            mobjLoadingThread.Start()
            mfrmWaiting.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lblAnniBirth_LinkClicked", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : LinkLabel2_LinkClicked, link clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles lblDecease.LinkClicked
        Try
            mfrmWaiting = New frmProgress()
            mobjLoadingThread = New System.Threading.Thread(AddressOf xShowAnniDecease)

            mobjLoadingThread.Start()
            mfrmWaiting.ShowDialog()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "lblAnniDecease_LinkClicked", ex)
        End Try
    End Sub

#End Region


#Region "Form Functions"

    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show form
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm() As Boolean

        fncShowForm = False

        Try

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFormLoad, form load event
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFormLoad() As Boolean

        xFormLoad = False

        Dim tblData As DataTable = Nothing

        Try
            'get data
            tblData = gobjDB.fncGetMemberMain()

            If tblData Is Nothing Then Exit Function

            If Not xCount(tblData) Then Exit Function

            xFillGrid()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFormLoad", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xCount, calculate infor
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : tblData DataTable, data
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCount(ByRef tblData As DataTable) As Boolean

        xCount = False

        Try
            Dim intDecease As Integer
            Dim intGender As Integer
            Dim intAge As Integer
            Dim intByea As Integer
            Dim intDyea As Integer
            'Dim dtBirth As Date
            'Dim dtDecease As Date

            With mstStats

                'total member
                .intTotalMem = tblData.Rows.Count

                For i As Integer = 0 To .intTotalMem - 1

                    Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("DECEASED")), intDecease)
                    Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("GENDER")), intGender)
                    'Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("BIRTH_DAY")), dtBirth)
                    'Date.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("DECEASED_DATE")), dtDecease)

                    Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("BIR_YEA")), intByea)
                    Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(i)("DEA_YEA")), intDyea)

                    'gender
                    Select Case intGender
                        Case clsEnum.emGender.MALE
                            .intMale += 1

                        Case clsEnum.emGender.FEMALE
                            .intFemale += 1

                        Case Else
                            .intUnknow += 1
                    End Select

                    intAge = basConst.gcintNONE_VALUE

                    'decease member
                    If intDecease = gcintDIED Then
                        .intDecease += 1
                        'If dtDecease > Date.MinValue And dtDecease < Date.MaxValue And dtBirth > Date.MinValue And dtBirth < Date.MaxValue Then _
                        '    intAge = dtDecease.Year - dtBirth.Year
                        If intDyea > 0 And intByea > 0 Then intAge = intDyea - intByea
                    Else
                        'If dtBirth > Date.MinValue And dtBirth < Date.MaxValue Then _
                        '    intAge = Date.Today.Year - dtBirth.Year
                        If intByea > 0 Then intAge = Date.Today.Year - intByea
                    End If

                    If 0 <= intAge And intAge <= 5 Then
                        .int0to5 += 1
                    ElseIf 6 <= intAge And intAge <= 17 Then
                        .int6to17 += 1
                    ElseIf 18 <= intAge And intAge <= 35 Then
                        .int18to35 += 1
                    ElseIf 36 <= intAge And intAge <= 55 Then
                        .int36to55 += 1
                    ElseIf 56 <= intAge And intAge <= 70 Then
                        .int56to70 += 1
                    ElseIf 71 <= intAge Then
                        .int71toHigher += 1
                    End If

                Next

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCount", ex)
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillGrid, fill infor to gird
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillGrid() As Boolean

        xFillGrid = False

        Try
            Dim strFormat As String = "{0} người ({1:0.0}%)"

            dgvStats.Rows.Clear()

            With mstStats
                'total member
                xAdd2Grid("Tổng số thành viên", String.Format("{0} người", .intTotalMem.ToString()))

                'decease
                xAdd2Grid(basConst.gcstrDeadDateUNKNOWText, String.Format(strFormat, .intDecease, .intDecease * 100 / .intTotalMem))

                'alive
                .intAlive = .intTotalMem - .intDecease
                xAdd2Grid("Còn sống", String.Format(strFormat, .intAlive, .intAlive * 100 / .intTotalMem))

                'male
                xAdd2Grid("Thành viên nam", String.Format(strFormat, .intMale, .intMale * 100 / .intTotalMem))

                'female
                xAdd2Grid("Thành viên nữ", String.Format(strFormat, .intFemale, .intFemale * 100 / .intTotalMem))

                'unknown
                xAdd2Grid("Chưa rõ giới tính", String.Format(strFormat, .intUnknow, .intUnknow * 100 / .intTotalMem))

                '0 - 5
                xAdd2Grid("Độ tuổi từ 0 đến 5", String.Format(strFormat, .int0to5, .int0to5 * 100 / .intTotalMem))

                '6 - 17
                xAdd2Grid("Độ tuổi từ 6 đến 17", String.Format(strFormat, .int6to17, .int6to17 * 100 / .intTotalMem))

                '18 - 35
                xAdd2Grid("Độ tuổi từ 17 đến 35", String.Format(strFormat, .int18to35, .int18to35 * 100 / .intTotalMem))

                '36 - 55
                xAdd2Grid("Độ tuổi từ 36 đến 55", String.Format(strFormat, .int36to55, .int36to55 * 100 / .intTotalMem))

                '56 - 70
                xAdd2Grid("Độ tuổi từ 56 đến 70", String.Format(strFormat, .int56to70, .int56to70 * 100 / .intTotalMem))

                '71 and higher
                xAdd2Grid("Trên 71 tuổi", String.Format(strFormat, .int71toHigher, .int71toHigher * 100 / .intTotalMem))

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGrid", ex)
        Finally

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAdd2Grid, add a row to grid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : strField1   String,
    '      PARAMS     : strField2   String,
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAdd2Grid(ByVal strField1 As String, ByVal strField2 As String) As Boolean

        xAdd2Grid = False

        Dim objContent(1) As String

        Try
            'add content to row
            objContent(0) = strField1
            objContent(1) = strField2

            'add to grid
            dgvStats.Rows.Add(objContent)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAdd2Grid", ex)
        Finally
            Erase objContent
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xShowAnniBirth, Show form of Anni Birth
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xShowAnniBirth()

        Try
            'new form
            mfrmAnni = New frmPersonalAnniversary()

            'set event handler to close waiting dialog
            AddHandler mfrmAnni.evnShown, AddressOf xProgressDone

            'show in birth list mode
            mfrmAnni.fncShowForm(frmPersonalAnniversary.emFormMode.BIRTH_LIST)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowAnniBirth", ex)
        Finally
            If mfrmAnni IsNot Nothing Then mfrmAnni.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xShowAnniDecease, Show form of Anni Birth
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xShowAnniDecease()

        Try
            'new form
            mfrmAnni = New frmPersonalAnniversary()

            'set event handler to close waiting dialog
            AddHandler mfrmAnni.evnShown, AddressOf xProgressDone

            'show in birth list mode
            mfrmAnni.fncShowForm(frmPersonalAnniversary.emFormMode.DECEASE_LIST)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xShowAnniDecease", ex)
        Finally
            If mfrmAnni IsNot Nothing Then mfrmAnni.Dispose()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xProgressDone, close waiting dialog
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xProgressDone()

        Try
            Dim objCloseWaitForm As MethodInvoker

            'close thread
            mobjLoadingThread = Nothing

            'close waiting form
            objCloseWaitForm = New MethodInvoker(AddressOf xCloseWaitForm)
            Me.Invoke(objCloseWaitForm)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xProgressDone", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCloseWaitForm, close waiting form
    '      MEMO       : 
    '      CREATE     : 2012/01/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xCloseWaitForm()

        Try
            mfrmWaiting.Close()
            mfrmWaiting.Dispose()
            mfrmWaiting = Nothing

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCloseWaitForm", ex)
        End Try

    End Sub


#End Region


End Class