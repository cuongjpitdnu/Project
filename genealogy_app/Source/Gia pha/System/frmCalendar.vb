'   ******************************************************************
'      TITLE      : CALENDAR FORM
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/04/12　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On


''' <summary>
''' CALENDAR CLASS
''' </summary>
''' <remarks></remarks>
Public Class frmCalendar

    Private Const mcstrClsName As String = "frmCalendar"                                                'class name
    Private Const mcstrInvalidDate As String = "Ngày tháng nhập vào không chính xác hoặc không tồn tại. Xin hãy nhập lại."  'message when date is wrong

    'Private mintSunDay As Integer           'day in sun calendar
    'Private mintSunMon As Integer           'month -------------
    'Private mintSunYea As Integer           'year --------------
    'Private mintLunDay As Integer           'day in lunar calendar
    'Private mintLunMon As Integer           'month ---------------
    'Private mintLunYea As Integer           'year ----------------
    Private mblnSelected As Boolean         'flag to determine date is selected
    Private memFormMode As emCalendar       'form mode
    Private mstDate As stCalendar           'returned calendar

    Private mobjLunarCal As clsLunarCalendar

    ''' <summary>
    ''' Form mode
    ''' </summary>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Public Enum emCalendar

        SUN
        LUNAR

    End Enum

    ''' <summary>
    ''' Date is chosen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DateChosen() As Boolean
        Get
            Return mblnSelected
        End Get
    End Property


    ''' <summary>
    ''' Date selected
    ''' </summary>
    ''' <value></value>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property SelectedDate() As stCalendar
        Get
            Return mstDate
        End Get
    End Property


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        mblnSelected = False
        ' Add any initialization after the InitializeComponent() call.
        mobjLunarCal = New clsLunarCalendar()

    End Sub


    ''' <summary>
    ''' Form loaded
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Sub frmCalendar_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim dtInit As Date

            Select Case memFormMode
                Case emCalendar.SUN
                    grbLunCal.Visible = False
                    Me.Height -= grbLunCal.Height
                    xInit(cbSunDay, cbSunMon, txtSunYear, mstDate)

                    'set init value for calendar control
                    If mstDate.intDay > 0 And mstDate.intMonth > 0 And mstDate.intYear > 0 Then

                        dtInit = New Date(mstDate.intYear, mstDate.intMonth, mstDate.intDay)

                        If dtInit >= calSun.MinDate And dtInit <= calSun.MaxDate Then

                            calSun.SelectionStart = dtInit
                            calSun.SelectionEnd = dtInit

                        End If

                    End If

                Case emCalendar.LUNAR
                    grbSunCal.Visible = False
                    grbLunCal.Location = grbSunCal.Location
                    Me.Height -= grbSunCal.Height
                    xInit(cbLunDay, cbLunMon, txtLunYear, mstDate)

                    'set init text year for lunar calendar
                    If mstDate.intYear >= mobjLunarCal.MinSupportedDateTime.Year And mstDate.intYear <= mobjLunarCal.MaxSupportedDateTime.Year Then _
                        lblLunYear.Text = "- " & mobjLunarCal.GetYearName(mstDate.intYear)

            End Select

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "frmCalendar_Load", ex)

        End Try
    End Sub


    ''' <summary>
    ''' Text change event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub txtLunYear_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLunYear.TextChanged
        Try
            Dim intYear As Integer

            If Not Integer.TryParse(txtLunYear.Text.Trim(), intYear) Then Exit Sub

            'show year name if it's valid
            lblLunYear.Text = ""
            If intYear >= mobjLunarCal.MinSupportedDateTime.Year And intYear <= mobjLunarCal.MaxSupportedDateTime.Year Then _
                If Not basCommon.fncIsBlank(txtLunYear.Text.Trim()) Then lblLunYear.Text = "- " & mobjLunarCal.GetYearName(intYear)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCheckData", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Show form
    ''' </summary>
    ''' <param name="emMode"></param>
    ''' <returns></returns>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Public Function fncShowForm(ByVal emMode As emCalendar, ByVal stInitDate As stCalendar) As Boolean

        fncShowForm = False

        Try
            Me.memFormMode = emMode
            Me.mstDate = stInitDate

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try

    End Function


    ''' <summary>
    ''' Init value
    ''' </summary>
    ''' <param name="cbDay"></param>
    ''' <param name="cbMon"></param>
    ''' <param name="txtYea"></param>
    ''' <param name="stInitValue">init date</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xInit(ByVal cbDay As ComboBox, ByVal cbMon As ComboBox, ByVal txtYea As TextBox, ByVal stInitValue As stCalendar) As Boolean

        xInit = False

        Try
            With stInitValue

                cbDay.SelectedIndex = .intDay
                cbMon.SelectedIndex = .intMonth

                txtYea.Text = .intYear.ToString
                If String.Compare(txtYea.Text, "0") = 0 Then txtYea.Clear()

            End With

            mblnSelected = False

            Return True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try

    End Function


    ''' <summary>
    ''' Read data from control
    ''' </summary>
    ''' <returns></returns>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Function xGetData() As Boolean

        xGetData = False

        Try
            mstDate = New stCalendar()

            Select Case memFormMode
                Case emCalendar.SUN
                    'If Not xIsValid(cbSunDay, cbSunMon, txtSunYear) Then Exit Select
                    'mstDate = xGetDate(cbSunDay, cbSunMon, txtSunYear)
                    Return xGetDate(cbSunDay, cbSunMon, txtSunYear, mstDate)

                Case emCalendar.LUNAR
                    'If Not xIsValid(cbLunDay, cbLunMon, txtLunYear) Then Exit Select
                    'mstDate = xGetDate(cbLunDay, cbLunMon, txtLunYear)
                    Return xGetDate(cbLunDay, cbLunMon, txtLunYear, mstDate)

            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetData", ex)
        End Try
    End Function


    ''' <summary>
    ''' OK button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Try

            Select Case memFormMode
                Case emCalendar.SUN
                    If Not xIsValid(cbSunDay, cbSunMon, txtSunYear) Then Exit Sub

                Case emCalendar.LUNAR
                    If Not xIsValid(cbLunDay, cbLunMon, txtLunYear) Then Exit Sub

            End Select

            If xCheckData() Then

                mblnSelected = True

                Me.Close()

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnOK_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Cancel button clicked
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Try
            Me.Close()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Clear button clear
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Sub btnSunClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSunClear.Click, Button1.Click
        Try

            lblLunYear.Text = ""

            cbLunDay.SelectedIndex = 0
            cbLunMon.SelectedIndex = 0
            cbSunDay.SelectedIndex = 0
            cbSunMon.SelectedIndex = 0

            txtLunYear.Clear()
            txtSunYear.Clear()

            xGetData()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSunClear_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Get values
    ''' </summary>
    ''' <param name="cbDay"></param>
    ''' <param name="cbMon"></param>
    ''' <param name="txtYea"></param>
    ''' <returns></returns>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Function xGetDate(ByVal cbDay As ComboBox, ByVal cbMon As ComboBox, ByVal txtYea As TextBox, ByRef stResult As stCalendar) As Boolean

        'Dim stResult As stCalendar
        xGetDate = False

        Try
            With stResult

                Integer.TryParse(basCommon.fncCnvNullToString(cbDay.SelectedIndex), .intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(cbMon.SelectedIndex), .intMonth)
                If Not basCommon.fncIsBlank(txtYea.Text.Trim()) And Not Integer.TryParse(txtYea.Text.Trim(), .intYear) Then
                    txtYea.Clear()
                    .intYear = 0
                    Return False
                End If

                If .intYear <= 0 Then
                    txtYea.Clear()
                    .intYear = 0
                End If

                '00/00/0000 is OK
                If .intDay <= 0 And .intYear <= 0 And .intMonth <= 0 Then Return True

                'check for 00/05/0000 -> 00/00/0000
                If .intDay <= 0 And .intYear <= 0 Then .intMonth = 0 : Return False

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetDate", ex)
        End Try

        'Return stResult

    End Function


    ''' <summary>
    ''' Check if data is valid
    ''' </summary>
    ''' <param name="cbDay"></param>
    ''' <param name="cbMon"></param>
    ''' <param name="txtYea"></param>
    ''' <returns></returns>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Function xIsValid(ByVal cbDay As ComboBox, ByVal cbMon As ComboBox, ByVal txtYea As TextBox) As Boolean

        xIsValid = False

        Try
            If cbDay.SelectedIndex <= 0 And cbMon.SelectedIndex <= 0 And basCommon.fncIsBlank(txtYea.Text) Then Return True

            'day selected but not month
            If cbDay.SelectedIndex > 0 And cbMon.SelectedIndex <= 0 Then
                basCommon.fncMessageWarning("Chưa nhập giá trị tháng.", cbMon)
                Exit Function
            End If

            'month selected but not day and year
            If cbDay.SelectedIndex <= 0 And cbMon.SelectedIndex > 0 And basCommon.fncIsBlank(txtYea.Text.Trim()) Then
                basCommon.fncMessageWarning("Chưa nhập giá trị năm.", txtYea)
                Exit Function
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xIsValid", ex)
        End Try

    End Function


    ''' <summary>
    ''' Check if sun-date is valid
    ''' </summary>
    ''' <returns></returns>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Function xIsValidSunDate() As Boolean

        xIsValidSunDate = False

        Try
            Dim dtResult As Date

            Try
                With mstDate

                    dtResult = New Date(.intYear, .intMonth, .intDay)

                End With

            Catch ex As Exception
                Return False
            End Try

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xIsValidSunDate", ex)
        End Try
    End Function


    ''' <summary>
    ''' Check if dd/MM is valid
    ''' </summary>
    ''' <returns></returns>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Function xIsValidDayMon() As Boolean

        xIsValidDayMon = False

        Try
            Dim dtResult As Date

            Try
                With mstDate

                    If .intDay > 0 And .intMonth > 0 And .intYear <= 0 Then

                        'check for Feb 29
                        If .intDay = 29 And .intMonth = 2 Then Return True
                        dtResult = New Date(Now.Year, .intMonth, .intDay)

                    End If

                End With

                Return True

            Catch ex As Exception
                Return False
            End Try

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xIsValidDayMon", ex)
        End Try
    End Function


    ''' <summary>
    ''' Check if lunar-date is valid
    ''' </summary>
    ''' <returns></returns>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Function xIsValidLunarDate() As Boolean

        xIsValidLunarDate = False

        Try

            With mstDate
                If .intYear < mobjLunarCal.MinSupportedDateTime.Year Or .intYear > mobjLunarCal.MaxSupportedDateTime.Year Then Return True

                If .intDay = 0 Or .intMonth = 0 Or .intYear = 0 Then Exit Function

                If mobjLunarCal.IsLeapYear(.intYear) Then

                    If .intMonth > mobjLunarCal.GetLeapMonth(.intYear) Then
                        If .intDay > mobjLunarCal.GetDaysInMonth(.intYear, .intMonth + 1) Then Exit Function
                    Else
                        If .intDay > mobjLunarCal.GetDaysInMonth(.intYear, .intMonth) Then Exit Function
                    End If

                Else

                End If

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xIsValidLunarDate", ex)
        End Try
    End Function


    ''' <summary>
    ''' Check validation of all data
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xCheckData() As Boolean

        xCheckData = False

        Try
            Dim dtTemp As Date

            'get data
            If Not xGetData() Then Return False

            With mstDate

                Select Case memFormMode
                    Case emCalendar.SUN
                        'check blank
                        If cbSunDay.SelectedIndex > 0 And cbSunMon.SelectedIndex > 0 And Not basCommon.fncIsBlank(txtSunYear.Text.Trim) Then

                            'check validation of date
                            If Not xIsValidSunDate() Then
                                basCommon.fncMessageWarning(mcstrInvalidDate, cbSunDay)
                                Return False
                            Else
                                dtTemp = New Date(.intYear, .intMonth, .intDay)

                                'set calendar
                                If dtTemp >= calSun.MinDate And dtTemp <= calSun.MaxDate Then
                                    calSun.SelectionStart = dtTemp
                                    calSun.SelectionEnd = dtTemp
                                End If

                            End If

                        ElseIf cbSunDay.SelectedIndex > 0 And cbSunMon.SelectedIndex > 0 Then

                            If Not xIsValidDayMon() Then basCommon.fncMessageWarning(mcstrInvalidDate, cbSunDay) : Return False

                        End If

                    Case emCalendar.LUNAR
                        'show year name if it's valid
                        lblLunYear.Text = ""
                        If .intYear >= mobjLunarCal.MinSupportedDateTime.Year And .intYear <= mobjLunarCal.MaxSupportedDateTime.Year Then _
                            If Not basCommon.fncIsBlank(txtLunYear.Text.Trim()) Then lblLunYear.Text = "- " & mobjLunarCal.GetYearName(.intYear)

                        'check blank
                        If cbLunDay.SelectedIndex > 0 And cbLunMon.SelectedIndex > 0 And Not basCommon.fncIsBlank(txtLunYear.Text.Trim) Then

                            'check validaiton of date
                            If Not xIsValidLunarDate() Then
                                basCommon.fncMessageWarning(mcstrInvalidDate, cbLunDay)
                                Return False
                            End If

                        End If

                End Select

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCheckData", ex)
        End Try

    End Function


    ''' <summary>
    ''' Control leaved event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Sub xControl_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) 'Handles txtSunYear.Leave, cbSunMon.Leave, cbSunDay.Leave, txtLunYear.Leave, cbLunMon.Leave, cbLunDay.Leave

        Try
            Dim dtTemp As Date

            'get data
            xGetData()

            With mstDate

                Select Case memFormMode
                    Case emCalendar.SUN
                        'check blank
                        If cbSunDay.SelectedIndex > 0 And cbSunMon.SelectedIndex > 0 And Not basCommon.fncIsBlank(txtSunYear.Text.Trim) Then

                            'check validation of date
                            If Not xIsValidSunDate() Then
                                basCommon.fncMessageWarning(mcstrInvalidDate, cbSunDay)
                            Else
                                dtTemp = New Date(.intYear, .intMonth, .intDay)

                                'set calendar
                                If dtTemp >= calSun.MinDate And dtTemp <= calSun.MaxDate Then
                                    calSun.SelectionStart = dtTemp
                                    calSun.SelectionEnd = dtTemp
                                End If

                            End If

                        ElseIf cbSunDay.SelectedIndex > 0 And cbSunMon.SelectedIndex > 0 Then

                            If Not xIsValidDayMon() Then basCommon.fncMessageWarning(mcstrInvalidDate, cbSunDay)

                        End If

                    Case emCalendar.LUNAR
                        'show year name if it's valid
                        lblLunYear.Text = ""
                        If .intYear >= mobjLunarCal.MinSupportedDateTime.Year And .intYear <= mobjLunarCal.MaxSupportedDateTime.Year Then _
                            If Not basCommon.fncIsBlank(txtLunYear.Text.Trim()) Then lblLunYear.Text = "- " & mobjLunarCal.GetYearName(.intYear)

                        'check blank
                        If cbLunDay.SelectedIndex > 0 And cbLunMon.SelectedIndex > 0 And Not basCommon.fncIsBlank(txtLunYear.Text.Trim) Then

                            'check validaiton of date
                            If Not xIsValidLunarDate() Then
                                basCommon.fncMessageWarning(mcstrInvalidDate, cbLunDay)
                            End If

                        End If

                End Select

            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xControl_Leave", ex)
        End Try

    End Sub


    ''' <summary>
    ''' Calendar value selected event
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <create>2012/04/12　AKB Quyet</create>
    ''' <remarks></remarks>
    Private Sub calSun_DateSelected(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DateRangeEventArgs) Handles calSun.DateSelected
        Try
            Dim dtSelected As Date

            dtSelected = calSun.SelectionStart

            cbSunDay.SelectedIndex = dtSelected.Day
            cbSunMon.SelectedIndex = dtSelected.Month
            txtSunYear.Text = dtSelected.Year.ToString()

            xGetData()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "calSun_DateSelected", ex)
        End Try
    End Sub


End Class