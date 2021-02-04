'   ******************************************************************
'      TITLE      : ANNIVERSARY LIST
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/12/29　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

Imports System.Data
'Imports OfficeOpenXml
Imports System.IO
'Imports System.Threading

'   ******************************************************************
'　　　FUNCTION   : Form Anniversary List
'      MEMO       : 
'      CREATE     : 2011/12/29  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmPersonalAnniversary

#Region "Variables and Constants"

    Private Const mcstrClsName As String = "frmPersonalAnniversary"                     'class name
    Private Const mcstrBornTitle As String = "DANH SÁCH SINH NHẬT GẦN NHẤT"                 'form title
    Private Const mcstrDeadTitle As String = "DANH SÁCH GIỖ GẦN NHẤT"                      'form title
    Private Const mcstrDateType As String = "System.DateTime"                           'datetime type
    Private Const mcstrANNI_BIRTH As String = "ANNI_BIRTH"                              'column in datatable
    Private Const mcstrANNI_DECEASE As String = "ANNI_DECEASE"                          'column in datatable
    Private Const mcstrLUNAR_DECEASE_DATE As String = "LUNAR_DECEASE_DATE"              'column in datatable
    Private Const mcstrDateString As String = "{0:0000}{1:00}{2:00}"                    'format for solar date in cell content
    Private Const mcstrDateStringLunar As String = "{0:dd/MM/yyyy} ({1})"               'format for lunar date in cell content
    Private Const mcstrTitleDecease As String = "Danh sách giỗ gần nhất"                      'form title
    Private Const mcstrTitleBirth As String = "Danh sách sinh nhật gần nhất"                    'form title
    Private Const mcstrDeadDaySun As String = "Ngày mất (Dương Lịch)"                          'header text
    Private Const mcstrDeadDayMoon As String = "Ngày mất (Âm Lịch)"                          'header text
    Private Const mcstrHeaderText2 As String = "Ngày sinh"                              'header text
    Private Const mcintShortListItem As Integer = 3                                     'item in short list

    Private mintCount As Integer                                                        'NO. counter
    Private memMode As emFormMode                                                       'form mode
    Private mtblData As DataTable                                                       'datatable
    Private mstDetail As stMemberDetail                                                 'struc to store detail
    Private mobjVnCal As clsLunarCalendar                                               'lunar calendar
    Private mlstAnniBirth As List(Of String)                                            'list of anni birth
    Private mlstAnniDecease As List(Of String)                                          'list of anni decease
    Private mblnShown As Boolean                                                        '

#End Region


    '   ******************************************************************
    '　　　FUNCTION   : evnShown, raise when form loaded successfully
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Event evnShown()


    'Form mode
    Public Enum emFormMode

        BIRTH_LIST
        DECEASE_LIST

    End Enum


    'Structure to store information
    Private Structure stMemberDetail

        Dim intID As Integer                    'member id
        Dim strFName As String                  'first name
        Dim strMName As String                  'middle name
        Dim strLName As String                  'last name
        Dim strAlias As String                  'alias

        Dim intGender As Integer                'gender
        Dim intDecease As Integer               'decease flag

        'Dim dtBirth As Date                     'date of birth
        Dim stBirthDaySun As stCalendar

        'Dim dtDecease As Date                   'date of decease
        Dim stDeadDayMoon As stCalendar
        Dim stDeadDaySun As stCalendar


        'Dim dtAnniBirth As Date                 'date of anni birth
        Dim stAnniDate As stCalendar

        'Dim intLunarDecease As Integer          'date of anni decease in lunar calendar

    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : AnniBirth Property, return first 3 member in list
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property AnniBirth() As List(Of String)

        Get
            memMode = emFormMode.BIRTH_LIST
            mlstAnniBirth.Clear()

            If xGetData() Then
                'fill grid
                xFillGrid(True)
            End If

            Return mlstAnniBirth

        End Get

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : AnniDecease Property, return first 3 member in list
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property AnniDecease() As List(Of String)

        Get
            memMode = emFormMode.DECEASE_LIST
            mlstAnniDecease.Clear()

            If xGetData() Then
                'fill grid
                xFillGrid(True)
            End If

            Return mlstAnniDecease

        End Get

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : FormShown Property, form shown or not
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property FormShown() As Boolean
        Get
            Return mblnShown
        End Get
    End Property


    '   ******************************************************************
    '　　　FUNCTION   : CONSTRUCTOR
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        mobjVnCal = New clsLunarCalendar()
        mlstAnniBirth = New List(Of String)
        mlstAnniDecease = New List(Of String)
        mblnShown = False

    End Sub


#Region "Form Events"

    '   ******************************************************************
    '　　　FUNCTION   : frmPersonalAnniversary_Load, form load event
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmPersonalAnniversary_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            mblnShown = True

            xInit()

            'get data
            If xGetData() Then

                'fill grid
                xFillGrid(False)

            End If

            RaiseEvent evnShown()

            Me.BringToFront()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "frmPersonalAnniversary_Load", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmPersonalAnniversary_FormClosed, form close event
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmPersonalAnniversary_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Try
            mblnShown = False

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "frmPersonalAnniversary_FormClosed", ex)

        End Try
    End Sub


    'Private Sub frmPersonalAnniversary_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
    '    Try
    '        mblnShown = False

    '    Catch ex As Exception

    '        basCommon.fncSaveErr(mcstrClsName, "frmPersonalAnniversary_FormClosed", ex)

    '    End Try
    'End Sub

    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, form close event
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.Close()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "frmPersonalAnniversary_Load", ex)

        End Try

    End Sub

#End Region


#Region "Form Functions"

    '   ******************************************************************
    '　　　FUNCTION   : xInit, init values
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function xInit() As Boolean

        xInit = False

        Dim dblTemp As Double

        Try
            If memMode = emFormMode.DECEASE_LIST Then
                Me.Text = mcstrTitleDecease
                lblTitle.Text = mcstrDeadTitle

                clmBirthDate.HeaderText = mcstrDeadDaySun
                clmBirthDate.Width = 100 '165

                clmAge.Visible = True
                clmAge.HeaderText = mcstrDeadDayMoon
                clmAge.Width = 150 '165

                clmEventDate.Width = 380
                'dgvMemberList.Width = 680 '(30 = 707 - 677)
                Me.Width = 920 '707

            Else
                Me.Text = mcstrTitleBirth
                lblTitle.Text = mcstrBornTitle

                clmBirthDate.HeaderText = mcstrHeaderText2
                clmAge.Visible = True
                clmAge.HeaderText = "Tuổi"
                clmAge.Width = 45
                clmBirthDate.Width = 135
                clmEventDate.Width = 165
                'dgvMemberList.Width = 650
                Me.Width = 677

            End If

            btnCancel.Location = New Point(CInt((Me.Width - btnCancel.Width) / 2), btnCancel.Location.Y)
            btnExcelExport.Location = New Point(btnCancel.Location.X - btnExcelExport.Width - 100, btnCancel.Location.Y)

            dblTemp = Me.Width / 2 - (btnCancel.Right - btnExcelExport.Left) / 2
            dblTemp = dblTemp / 2

            btnCancel.Location = New Point(btnCancel.Location.X + CInt(dblTemp), btnCancel.Location.Y)
            btnExcelExport.Location = New Point(btnExcelExport.Location.X + CInt(dblTemp), btnExcelExport.Location.Y)

            lblTitle.Location = New Point(CInt((Me.Width - lblTitle.Width) / 2), lblTitle.Location.Y)

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show form
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm(ByVal enMode As emFormMode) As Boolean

        fncShowForm = False

        Try
            'set form mode
            Me.memMode = enMode

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex, Nothing, False)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetData, get data from database
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetData() As Boolean

        xGetData = False

        Try
            'Dim dtTempBirth As Date
            'Dim dtTempDecease As Date
            Dim stBirth As stCalendar
            Dim stDeathMoon As stCalendar
            Dim stDeathSun As stCalendar
            Dim intTempYear As Integer

            mtblData = gobjDB.fncGetMemberMain()

            If mtblData Is Nothing Then Exit Function

            'add 2 new columns for sorting
            'mtblData.Columns.Add(mcstrANNI_BIRTH, System.Type.GetType(mcstrDateType))
            'mtblData.Columns.Add(mcstrANNI_BIRTH)
            'mtblData.Columns.Add(mcstrANNI_DECEASE)
            mtblData.Columns.Add("ANI_DAY")
            mtblData.Columns.Add("ANI_MON")
            mtblData.Columns.Add("ANI_YEA")

            'mtblData.Columns.Add(mcstrLUNAR_DECEASE_DATE)

            'loop from end of datatable
            For i As Integer = mtblData.Rows.Count - 1 To 0 Step -1

                stBirth = Nothing
                stDeathMoon = Nothing
                stDeathSun = Nothing
                With stBirth
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("BIR_DAY")), .intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("BIR_MON")), .intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("BIR_YEA")), .intYear)
                End With

                With stDeathMoon
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("DEA_DAY")), .intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("DEA_MON")), .intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("DEA_YEA")), .intYear)
                End With

                With stDeathSun
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("DEA_DAY_SUN")), .intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("DEA_MON_SUN")), .intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(mtblData.Rows(i)("DEA_YEA_SUN")), .intYear)
                End With


                If stBirth.intDay <= 0 And stBirth.intMonth <= 0 And stBirth.intYear <= 0 And stDeathMoon.intDay <= 0 And stDeathMoon.intMonth <= 0 And stDeathMoon.intYear <= 0 Then

                    mtblData.Rows.RemoveAt(i)
                    Continue For

                End If

                If memMode = emFormMode.BIRTH_LIST Then
                    'there is a value
                    'set ANI BIRTH value
                    If stBirth.intDay > 0 Or stBirth.intMonth > 0 Or stBirth.intYear > 0 Then

                        'birthday already passed
                        If basCommon.fncCompareDate(Date.Today.Year, stBirth.intMonth, stBirth.intDay, Date.Today) < 0 Then
                            intTempYear = Date.Today.Year + 1
                        Else
                            intTempYear = Date.Today.Year
                        End If

                        mtblData.Rows(i)("ANI_DAY") = String.Format("{0:00}", stBirth.intDay)
                        mtblData.Rows(i)("ANI_MON") = String.Format("{0:00}", stBirth.intMonth)
                        mtblData.Rows(i)("ANI_YEA") = intTempYear

                        If Not Date.IsLeapYear(intTempYear) And stBirth.intMonth = 2 And stBirth.intDay = 29 Then
                            mtblData.Rows(i)("ANI_DAY") = stBirth.intDay - 1
                        End If

                    End If

                Else
                    'set TEMP DECEASE
                    If stDeathMoon.intDay <= 0 And stDeathMoon.intMonth <= 0 And stDeathMoon.intYear <= 0 Then Continue For
                    If stDeathMoon.intDay > 0 Or stDeathMoon.intMonth > 0 Or stDeathMoon.intYear > 0 Then

                        mtblData.Rows(i)("ANI_DAY") = String.Format("{0:00}", stDeathMoon.intDay)

                        Dim intTmpMonth As Integer = 0
                        Dim intCurLunYea As Integer

                        intCurLunYea = mobjVnCal.GetYear(Date.Today)

                        'Check Leap Year
                        'If mobjVnCal.IsLeapYear(Date.Today.Year) Then
                        If mobjVnCal.IsLeapYear(intCurLunYea) Then
                            'Check Leap Month of year

                            intTmpMonth = mobjVnCal.GetLeapMonth(intCurLunYea)

                            ' ▽ 2013/01/24   AKB Quyet （変更内容）*********************************
                            'For j As Integer = 1 To 12
                            '    'If mobjVnCal.IsLeapMonth(Date.Today.Year, j) Then
                            '    If mobjVnCal.IsLeapMonth(intCurLunYea, j) Then
                            '        intTmpMonth = j
                            '        Exit For
                            '    End If
                            'Next
                            ' △ 2013/01/24   AKB Quyet *********************************************

                            If stDeathMoon.intMonth > intTmpMonth Then
                                stDeathMoon.intMonth += 1
                            End If

                        End If
                        mtblData.Rows(i)("ANI_MON") = String.Format("{0:00}", stDeathMoon.intMonth)


                        ' ▽ 2013/01/24   AKB Quyet （変更内容）*********************************
                        ''birthday already passed
                        'If basCommon.fncCompareDate(Date.Today.Year, stDeath.intMon, stDeath.intDay, Date.Today) < 0 Then
                        '    mtblData.Rows(i)("ANI_YEA") = Date.Today.Year + 1
                        'Else
                        '    mtblData.Rows(i)("ANI_YEA") = Date.Today.Year
                        'End If


                        'use curent year
                        'Dim intCurLunYea As Integer
                        Dim intCurLunMon As Integer
                        Dim dtSolarDecease As Date

                        'intCurLunYea = mobjVnCal.GetYear(Date.Today)
                        intCurLunMon = mobjVnCal.GetMonth(Date.Today)

                        If intCurLunMon > stDeathMoon.intMonth And stDeathMoon.intMonth > 0 Then
                            intCurLunYea = intCurLunYea + 1
                        End If

                        'If stDeathMoon.intDay <= 0 And stDeathMoon.intMonth <= 0 Then
                        If stDeathMoon.intMonth <= 0 Then

                            mtblData.Rows(i)("ANI_YEA") = intCurLunYea

                            'ElseIf stDeathMoon.intMonth > 0 And stDeathMoon.intDay <= 0 Then
                        ElseIf stDeathMoon.intDay <= 0 Then
                            'get solar deacease mon and day from lunar date
                            dtSolarDecease = mobjVnCal.fncGetSolarDate(1, stDeathMoon.intMonth, intCurLunYea)

                            'decease day already passed
                            If basCommon.fncCompareDate(dtSolarDecease.Year, dtSolarDecease.Month, dtSolarDecease.Day, Date.Today) < 0 Then
                                mtblData.Rows(i)("ANI_YEA") = intCurLunYea + 1 'must use current lunar year + 1
                            Else
                                mtblData.Rows(i)("ANI_YEA") = intCurLunYea 'must use current lunar year
                            End If


                        Else

                            'get solar deacease mon and day from lunar date
                            dtSolarDecease = mobjVnCal.fncGetSolarDate(stDeathMoon.intDay, stDeathMoon.intMonth, intCurLunYea)

                            'decease day already passed
                            If basCommon.fncCompareDate(dtSolarDecease.Year, dtSolarDecease.Month, dtSolarDecease.Day, Date.Today) < 0 Then
                                mtblData.Rows(i)("ANI_YEA") = intCurLunYea + 1 'must use current lunar year + 1
                            Else
                                mtblData.Rows(i)("ANI_YEA") = intCurLunYea 'must use current lunar year
                            End If

                        End If

                        ' △ 2013/01/24   AKB Quyet *********************************************

                    End If
                End If

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetData", ex, Nothing, False)
        End Try


    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillGrid, fill data to datagridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : blnShortList    Boolean, used for property
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillGrid(ByVal blnShortList As Boolean) As Boolean

        xFillGrid = False

        Dim vwData As DataView = Nothing

        Try
            Dim strSort As String

            'new dataview
            vwData = New DataView(mtblData)

            'reset counter
            mintCount = 0

            dgvMemberList.Rows.Clear()
            strSort = "ANI_YEA, ANI_MON, ANI_DAY"

            If memMode = emFormMode.BIRTH_LIST Then

                xAddGrid(vwData, strSort, False, blnShortList)

            Else

                xAddGrid(vwData, strSort, True, blnShortList)

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillGrid", ex)
        Finally
            If vwData IsNot Nothing Then vwData.Dispose()
            If mtblData IsNot Nothing Then mtblData.Dispose()
        End Try

    End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : xAddGrid, add each row to grid
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : vwData  DataView, 
    ''      PARAMS     : strSort  String, sort string for dataview
    ''      PARAMS     : blnDeceaseMode  Boolean, form mode
    ''      PARAMS     : blnShortList  Boolean,
    ''      MEMO       : 
    ''      CREATE     : 2011/12/29  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    Private Function xAddGrid(ByVal vwData As DataView, ByVal strSort As String, ByVal blnDeceaseMode As Boolean, ByVal blnShortList As Boolean) As Boolean

        xAddGrid = False

        Dim objContent(6) As Object
        Dim intAge As Integer
        Dim strAniDea As String
        Dim intTmpMonth As Integer = 0
        Dim strDeadDate As String

        Try
            Dim intCurLunYea As Integer

            intCurLunYea = mobjVnCal.GetYear(Date.Now)

            vwData.Sort = strSort

            For i As Integer = 0 To vwData.Count - 1

                'read for first 3 record
                If blnShortList And mintCount > mcintShortListItem - 1 Then Exit For

                With mstDetail

                    'get data at row(i)
                    xGetDataStruc(i, vwData)
                    'in birth date list, does not list member who dead
                    If blnDeceaseMode Then
                        If .intDecease = basConst.gcintALIVE Then Continue For
                    Else
                        If .intDecease = basConst.gcintDIED Then Continue For
                    End If
                    'If Not blnDeceaseMode Then _
                    'If .intDecease = basConst.gcintDIED Then Continue For

                    'clear array before use it
                    Array.Clear(objContent, 0, objContent.Length)

                    'member id
                    objContent(0) = .intID

                    'image field
                    objContent(1) = GiaPha.My.Resources.Gender_unknown16
                    If .intGender = clsEnum.emGender.MALE Then objContent(1) = GiaPha.My.Resources.Gender_man16
                    If .intGender = clsEnum.emGender.FEMALE Then objContent(1) = GiaPha.My.Resources.Gender_woman16

                    'full name
                    objContent(3) = basCommon.fncGetFullName(.strFName, .strMName, .strLName, .strAlias)

                    'birth date
                    objContent(4) = basCommon.fncGetDateName("", .stBirthDaySun, True)
                    If blnDeceaseMode Then

                        'Dead Date in Sun Calendar
                        strDeadDate = basCommon.fncGetDateName("", .stDeadDaySun, True, False)
                        objContent(4) = strDeadDate

                        'Dead Date in Moon Calendar
                        strDeadDate = basCommon.fncGetDateName("", .stDeadDayMoon, True, True) & Environment.NewLine
                        objContent(5) = strDeadDate

                    End If


                    If blnDeceaseMode Then

                        If (Not .stDeadDayMoon.IsYearMonthDayisInputed()) Or .stDeadDayMoon.intYear > Now.Year Then Continue For

                        'add name to list
                        If blnShortList Then mlstAnniDecease.Add(objContent(3).ToString())

                        'If mobjVnCal.IsLeapYear(Date.Today.Year) Then
                        'must use anniversal lunar year to check leap year
                        If mobjVnCal.IsLeapYear(.stAnniDate.intYear) Then
                            'Check Leap Month of year
                            'For j As Integer = 1 To 12
                            '    If mobjVnCal.IsLeapMonth(intCurLunYea, j) Then
                            '        intTmpMonth = j
                            '        Exit For
                            '    End If
                            'Next
                            intTmpMonth = mobjVnCal.GetLeapMonth(.stAnniDate.intYear)

                            If .stAnniDate.intMonth > intTmpMonth Then
                                strAniDea = basCommon.fncGetDateName("", .stAnniDate.intDay, .stAnniDate.intMonth - 1, .stAnniDate.intYear, True, True)
                            Else
                                strAniDea = basCommon.fncGetDateName("", .stAnniDate, True, True)
                            End If
                        Else
                            If .stAnniDate.intMonth > 12 Then .stAnniDate.intMonth = 12
                            strAniDea = basCommon.fncGetDateName("", .stAnniDate, True, True)
                        End If

                        strAniDea = "Âm lịch: " & strAniDea
                        Try

                            If .stAnniDate.intDay > 0 Then

                                'strAniDea &= String.Format(" ( tức " & basConst.gcstrDateFormat2 & " )", mobjVnCal.fncGetSolarDate(.stAnniDate))
                                strAniDea &= String.Format("          Dương lịch: " & basConst.gcstrDateFormat2, mobjVnCal.fncGetSolarDate(.stAnniDate))

                            Else

                                'strAniDea &= String.Format(" ( tức " & "{0:MM/yyyy}" & " )", mobjVnCal.fncGetSolarDate(1, .stAnniDate.intMonth, .stAnniDate.intYear))
                                strAniDea &= String.Format("          Dương lịch: " & "{0:MM/yyyy}", mobjVnCal.fncGetSolarDate(1, .stAnniDate.intMonth, .stAnniDate.intYear))

                            End If

                        Catch ex As Exception
                        End Try

                        objContent(6) = strAniDea

                    Else

                        If Not .stBirthDaySun.IsYearMonthDayisInputed() Or .stBirthDaySun.intYear > Now.Year Then Continue For

                        'add name to list
                        If blnShortList Then mlstAnniBirth.Add(objContent(3).ToString())

                        'age
                        If .stBirthDaySun.intYear > 0 Then
                            'remove nagative age
                            intAge = Date.Now.Year - .stBirthDaySun.intYear
                            If intAge < 0 Then intAge = 0

                            'age for next year if the anniversary of this year passed
                            'If .dtAnniBirth < Date.Today Then intAge += 1
                            If .stAnniDate.intMonth < Date.Today.Month Then
                                intAge += 1
                            ElseIf .stAnniDate.intMonth = Date.Today.Year Then
                                If .stAnniDate.intDay < Date.Today.Day Then
                                    intAge += 1
                                End If
                            End If

                            objContent(5) = intAge

                        End If

                        'anniversary
                        objContent(6) = basCommon.fncGetDateName("", .stAnniDate, True)

                    End If

                    'NO field
                    mintCount += 1
                    objContent(2) = mintCount

                End With

                'add new row to gird view
                If Not blnShortList Then dgvMemberList.Rows.Add(objContent)

            Next

            Return True
        Catch ex As Exception
            Dim st As New StackTrace(True)
            st = New StackTrace(ex, True)

            basCommon.fncSaveErr(mcstrClsName, "xAddGrid", ex)
        Finally
            Erase objContent
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetDataStruc, read each row to structure
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRow    Integer, row number
    '      PARAMS     : vwData    DataView, 
    '      MEMO       : 
    '      CREATE     : 2011/12/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetDataStruc(ByVal intRow As Integer, ByVal vwData As DataView) As Boolean

        xGetDataStruc = False

        Try
            'get data at row
            With vwData(intRow)

                'id and gender
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("MEMBER_ID")), mstDetail.intID)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("GENDER")), mstDetail.intGender)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED")), mstDetail.intDecease)

                'name
                mstDetail.strFName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                mstDetail.strMName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                mstDetail.strLName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                mstDetail.strAlias = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))

                'birth and decease date
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), mstDetail.dtBirth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), mstDetail.stBirthDaySun.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), mstDetail.stBirthDaySun.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), mstDetail.stBirthDaySun.intYear)

                'Date.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED_DATE")), mstDetail.dtDecease)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY")), mstDetail.stDeadDayMoon.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON")), mstDetail.stDeadDayMoon.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA")), mstDetail.stDeadDayMoon.intYear)

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY_SUN")), mstDetail.stDeadDaySun.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON_SUN")), mstDetail.stDeadDaySun.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA_SUN")), mstDetail.stDeadDaySun.intYear)


                'Date.TryParse(basCommon.fncCnvNullToString(.Item(mcstrANNI_BIRTH)), mstDetail.dtAnniBirth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("ANI_DAY")), mstDetail.stAnniDate.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("ANI_MON")), mstDetail.stAnniDate.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("ANI_YEA")), mstDetail.stAnniDate.intYear)


                'Integer.TryParse(basCommon.fncCnvNullToString(.Item(mcstrANNI_DECEASE)), mstDetail.intLunarDecease)

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetDataStruc", ex)
        End Try

    End Function



#End Region


#Region "Not used"


    ''   ******************************************************************
    ''　　　FUNCTION   : xAdd2Grid, add each row to grid
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : vwData  DataView, 
    ''      PARAMS     : blnDeceaseMode  Boolean, form mode
    ''      PARAMS     : strFilter  String, filter string for dataview
    ''      PARAMS     : strSort  String, sort string for dataview
    ''      PARAMS     : blnShortList  Boolean,
    ''      MEMO       : 
    ''      CREATE     : 2011/12/29  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xAdd2Grid(ByVal vwData As DataView, _
    '                           ByVal blnDeceaseMode As Boolean, _
    '                           ByVal strFilter As String, _
    '                           ByVal strSort As String, _
    '                           ByVal blnShortList As Boolean) As Boolean

    '    xAdd2Grid = False

    '    Dim objContent(6) As Object

    '    Try
    '        Dim intAge As Integer
    '        Dim intToday As Integer
    '        Dim dtNextYear As Date

    '        'vwData.RowFilter = strFilter
    '        vwData.Sort = strSort

    '        'get today lunar date in formarted string : yyyyMMdd
    '        'If Not Integer.TryParse(xGetLunarDate(Date.Today), intToday) Then Exit Function

    '        dtNextYear = New Date(Date.Today.Year + 1, Date.Today.Month, Date.Today.Day)

    '        For i As Integer = 0 To vwData.Count - 1

    '            'read for first 3 record
    '            If blnShortList And mintCount > mcintShortListItem - 1 Then Exit For

    '            With mstDetail
    '                'get data at row(i)
    '                xGetDataStruc(i, vwData)

    '                'in birth date list, does not list member who dead
    '                If Not blnDeceaseMode Then _
    '                    If .intDecease = basConst.gcintDIED Then Continue For

    '                'clear array before use it
    '                Array.Clear(objContent, 0, objContent.Length)

    '                'member id
    '                objContent(0) = .intID

    '                'image field
    '                objContent(1) = GiaPha.My.Resources.Gender_unknown16
    '                If .intGender = clsEnum.emGender.MALE Then objContent(1) = GiaPha.My.Resources.Gender_man16
    '                If .intGender = clsEnum.emGender.FEMALE Then objContent(1) = GiaPha.My.Resources.Gender_woman16

    '                'NO field
    '                mintCount += 1
    '                objContent(2) = mintCount

    '                'full name
    '                objContent(3) = basCommon.fncGetFullName(.strFName, .strMName, .strLName, .strAlias)

    '                'birth date
    '                'objContent(4) = String.Format(basConst.gcstrDateFormat2, .dtBirth)
    '                'If blnDeceaseMode Then objContent(4) = String.Format(basConst.gcstrDateFormat2, .dtDecease)

    '                objContent(4) = basCommon.fncGetDateName("", .intBday, .intBmon, .intByea, True)
    '                If blnDeceaseMode Then objContent(4) = basCommon.fncGetDateName("", .intDday, .intDmon, .intDyea, True)


    '                If blnDeceaseMode Then

    '                    'exit if lunar date is invalid
    '                    If .intLunarDecease <= 0 Then Continue For

    '                    'add name to list
    '                    If blnShortList Then mlstAnniDecease.Add(objContent(3).ToString())

    '                    'anniversary
    '                    If .intLunarDecease >= intToday Then
    '                        objContent(6) = String.Format(mcstrDateStringLunar, xGetSolarDate(.intLunarDecease.ToString()), mobjVnCal.fncGetLunarDateString(xGetSolarDate(.intLunarDecease.ToString()), 2))
    '                    Else
    '                        'get next year lunar date in formarted string : yyyyMMdd
    '                        Integer.TryParse(xGetAnniLunarDate(.dtDecease, dtNextYear), .intLunarDecease)
    '                        objContent(6) = String.Format(mcstrDateStringLunar, xGetSolarDate(.intLunarDecease.ToString()), mobjVnCal.fncGetLunarDateString(xGetSolarDate(.intLunarDecease.ToString()), 2))

    '                    End If

    '                Else
    '                    'add name to list
    '                    If blnShortList Then mlstAnniBirth.Add(objContent(3).ToString())

    '                    'age
    '                    If .intByea > 0 Then
    '                        'remove nagative age
    '                        intAge = Date.Now.Year - .intByea
    '                        If intAge < 0 Then intAge = 0

    '                        'age for next year if the anniversary of this year passed
    '                        'If .dtAnniBirth < Date.Today Then intAge += 1
    '                        If .intAmon < Date.Today.Month Then
    '                            intAge += 1
    '                        ElseIf .intAmon = Date.Today.Year Then
    '                            If .intAday < Date.Today.Day Then
    '                                intAge += 1
    '                            End If
    '                        End If

    '                        objContent(5) = intAge

    '                    End If

    '                    'anniversary
    '                    'If .dtAnniBirth >= Date.Today Then
    '                    If .intAyea >= Date.Today.Year And .intAmon >= Date.Today.Month And .intAday >= Date.Today.Day Then
    '                        'objContent(6) = String.Format(basConst.gcstrDateFormat2, .dtAnniBirth)
    '                        objContent(6) = basCommon.fncGetDateName("", .intAday, .intAmon, .intAyea, True)
    '                    Else
    '                        ''if birthday is Feb 29 and next year is not leap year then temp birth is Feb 28
    '                        'If .dtAnniBirth.Month = 2 And .dtAnniBirth.Day = 29 And Not Date.IsLeapYear(.dtAnniBirth.Year + 1) Then
    '                        '    .dtAnniBirth = New Date(.dtAnniBirth.Year + 1, 2, 28)
    '                        'Else
    '                        '    'convert to next year
    '                        '    .dtAnniBirth = New Date(.dtAnniBirth.Year + 1, .dtAnniBirth.Month, .dtAnniBirth.Day)
    '                        'End If

    '                        'objContent(6) = String.Format(basConst.gcstrDateFormat2, .dtAnniBirth)

    '                        'if birthday is Feb 29 and next year is not leap year then temp birth is Feb 28
    '                        If .intAmon = 2 And .intAday = 29 And Not Date.IsLeapYear(.intAyea + 1) Then
    '                            '.dtAnniBirth = New Date(.dtAnniBirth.Year + 1, 2, 28)
    '                            .intAday = 28
    '                            .intAyea += 1
    '                        Else
    '                            'convert to next year
    '                            '.dtAnniBirth = New Date(.dtAnniBirth.Year + 1, .dtAnniBirth.Month, .dtAnniBirth.Day)
    '                            .intAyea += 1
    '                        End If

    '                        objContent(6) = basCommon.fncGetDateName("", .intAday, .intAmon, .intAyea, True)


    '                    End If


    '                End If


    '            End With

    '            'add new row to gird view
    '            If Not blnShortList Then dgvMemberList.Rows.Add(objContent)

    '        Next

    '        Return True

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xAdd2Grid", ex)
    '    Finally
    '        Erase objContent
    '    End Try

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : xGetLunarDate, get lunar date from solar date
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : 
    ''      MEMO       : 
    ''      CREATE     : 2011/12/29  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xGetLunarDate(ByVal dtDecease As Date) As String

    '    Dim strResult As String = ""

    '    Try
    '        Dim intYear As Integer                  'decease year
    '        Dim intMonth As Integer                 'decease month
    '        Dim intDay As Integer                   'decease day

    '        'check min value
    '        If Not basCommon.fncIsValidSolarDate(dtDecease) Then Return strResult

    '        intYear = mobjVnCal.GetYear(dtDecease)
    '        intMonth = mobjVnCal.GetMonth(dtDecease)
    '        intDay = mobjVnCal.GetDayOfMonth(dtDecease)

    '        strResult = String.Format(mcstrDateString, intYear, intMonth, intDay)

    '        Return strResult

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xGetLunarDate", ex)
    '    End Try

    '    Return strResult

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : xGetSolarDate, get solar date from lunar date string
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : strDate String, lunar date string in format: yyyyMMdd
    ''      MEMO       : 
    ''      CREATE     : 2011/12/29  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xGetSolarDate(ByVal strDate As String) As Date

    '    Dim dtResult As Date = Nothing

    '    Try
    '        Dim intYear As Integer
    '        Dim intMonth As Integer
    '        Dim intDay As Integer

    '        'get date time from string in format yyyyMMdd
    '        xGetDateFromString(strDate, intYear, intMonth, intDay)

    '        'check null
    '        If intYear < basConst.gcintMinYear Then Exit Function

    '        dtResult = mobjVnCal.fncGetSolarDate(intDay, intMonth, intYear)

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xGetSolarDate", ex, Nothing, False)
    '    End Try

    '    Return dtResult

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : xGetAnniLunarDate, get anni date in lunar calendar
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : dtDecease Date, decease date
    ''      PARAMS     : dtAnniDate Date, date to compare
    ''      MEMO       : 
    ''      CREATE     : 2011/12/29  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xGetAnniLunarDate(ByVal dtDecease As Date, ByVal dtAnniDate As Date) As String
    '    'Private Function xGetAnniLunarDate(ByVal stDate As stCalendar, ByVal dtAnniDate As Date) As String

    '    Dim strResult As String = ""

    '    Try
    '        Dim intYear As Integer                  'decease year
    '        Dim intMonth As Integer                 'decease month
    '        Dim intDay As Integer                   'decease day
    '        Dim intCurYear As Integer               'current year
    '        Dim intLeapMo As Integer                'leap month

    '        'check min value
    '        'If Not basCommon.fncIsValidSolarDate(dtDecease) Then Return strResult

    '        intYear = mobjVnCal.GetYear(dtDecease)
    '        intMonth = mobjVnCal.GetMonth(dtDecease)
    '        intDay = mobjVnCal.GetDayOfMonth(dtDecease)
    '        intCurYear = mobjVnCal.GetYear(dtAnniDate)

    '        strResult = String.Format(mcstrDateString, intCurYear, intMonth, intDay)

    '        'if current year and decease year are leap year, do nothing
    '        If mobjVnCal.IsLeapYear(intYear) And mobjVnCal.IsLeapYear(intCurYear) Then Return strResult

    '        'if current year and decease year are not leap year, do nothing
    '        If Not mobjVnCal.IsLeapYear(intYear) And Not mobjVnCal.IsLeapYear(intCurYear) Then Return strResult

    '        'if decease year is leap year and current year is not
    '        If mobjVnCal.IsLeapYear(intYear) And Not mobjVnCal.IsLeapYear(intCurYear) Then

    '            'get leap month
    '            intLeapMo = mobjVnCal.GetLeapMonth(intYear)

    '            'do nothing and exit
    '            If intLeapMo > intMonth Then Return strResult

    '            'decrease by 1 if leap month smaller than decease month
    '            intMonth -= 1

    '            strResult = String.Format(mcstrDateString, intCurYear, intMonth, intDay)

    '            Return strResult

    '        End If

    '        'if decease year is not leap year and current year is leap year
    '        If Not mobjVnCal.IsLeapYear(intYear) And mobjVnCal.IsLeapYear(intCurYear) Then

    '            'get leap month
    '            intLeapMo = mobjVnCal.GetLeapMonth(intCurYear)

    '            'do nothing and exit
    '            If intLeapMo > intMonth Then Return strResult

    '            'decrease by 1 if leap month smaller than decease month
    '            intMonth += 1

    '            strResult = String.Format(mcstrDateString, intCurYear, intMonth, intDay)

    '            Return strResult

    '        End If


    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xGetAnniLunarDate", ex)
    '    End Try

    '    Return strResult

    'End Function


    ''   ******************************************************************
    ''　　　FUNCTION   : xGetDateFromString, return date time from lunar string
    ''      VALUE      : Boolean, true - success, false - failure
    ''      PARAMS     : strDate String, date in format yyyyMMdd
    ''      PARAMS     : intYear Integer, year
    ''      PARAMS     : intMonth Integer, month
    ''      PARAMS     : intDay Integer, day
    ''      MEMO       : 
    ''      CREATE     : 2011/12/29  AKB Quyet
    ''      UPDATE     : 
    ''   ******************************************************************
    'Private Function xGetDateFromString(ByVal strDate As String, ByRef intYear As Integer, ByRef intMonth As Integer, ByRef intDay As Integer) As Boolean

    '    xGetDateFromString = False

    '    Try
    '        If strDate.Length < 8 Then Exit Function

    '        Integer.TryParse(strDate.Substring(0, 4), intYear)
    '        Integer.TryParse(strDate.Substring(4, 2), intMonth)
    '        Integer.TryParse(strDate.Substring(6, 2), intDay)

    '        Return True

    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "xGetDateFromString", ex)
    '    End Try

    'End Function

#End Region



    Private Sub btnExcelExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExcelExport.Click

        If memMode = emFormMode.DECEASE_LIST Then
            DataGridToExcel(dgvMemberList, Nothing, mcstrTitleDecease)
        Else
            DataGridToExcel(dgvMemberList, Nothing, mcstrTitleBirth)
        End If
    End Sub

    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================


End Class