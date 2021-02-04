'   ******************************************************************
'      TITLE      : Vietnamese Calendar
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/07/27　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

Imports System.Globalization

'   ******************************************************************
'　　　FUNCTION   : Vietnamese Calendar Class
'      MEMO       : 
'      CREATE     : 2011/07/27  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmCalendarVN

#Region "Variable"

    Private Const mcstrClsName As String = "frmCalendarVN"                                      'class name

    Private Const mcstrMessageYrRange As String = "Năm nhập vào phải nằm trong khoảng từ 1800 đến 2199."   'Message error when year is out of range
    Private Const mcstrMessageInvalid As String = "Ngày được chọn  không tồn tại."                      'Message when date is invalid
    Private Const mcstrErrorCreateMD As String = "Lỗi khởi tạo ngày tháng."                            'Message error when create day or month



    Private mintVnDay As Integer                                                                'Vietnamese day
    Private mintVnMonth As Integer                                                              'Vietnamese month
    Private mintVnYear As Integer                                                               'Vietnamese year
    Private mintLeapMonth As Integer                                                            'Leap month in a leap year

    Private mintPreYear As Integer                                                              'Previous year
    Private mintPreMonth As Integer                                                             'Previous month

    Private mdtSolar As Date                                                                    'Solar date

    Private mclsLunarCal As clsLunarCalendar                                                    'Lunar calendar instance
    Private mobjDtPicker As DateTimePicker

#End Region

#Region "Property"


    '   ******************************************************************
    '　　　FUNCTION   : SolarDate Property
    '      MEMO       : 
    '      CREATE     : 2011/08/05  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property SolarDate() As Date

        Get
            Return mdtSolar
        End Get

        Set(ByVal value As Date)
            Me.mdtSolar = value
        End Set

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : EnCalPicker Property
    '      MEMO       : 
    '      CREATE     : 2011/08/05  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public WriteOnly Property EnCalPicker() As DateTimePicker

        Set(ByVal value As DateTimePicker)
            Me.mobjDtPicker = value
        End Set

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : LunarString Property
    '      MEMO       : 
    '      CREATE     : 2011/08/05  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property LunarString() As String

        Get
            Return xGetLunarString()
        End Get

    End Property


#End Region

#Region "Constructor"

    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/08/05  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        xInit()

    End Sub

#End Region

#Region "Form's events"


    '   ******************************************************************
    '　　　FUNCTION   : frmCalendarVN_Load, Form loaded
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmCalendarVN_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try

            Dim intYear As Integer          'year
            Dim intMonth As Integer         'month
            Dim intDay As Integer           'day

            mdtSolar = mobjDtPicker.Value

            'exit if the date is out of range
            If Not basCommon.fncIsValidSolarDate(mdtSolar) Then Exit Sub

            'show initial date if
            If mdtSolar > Date.MinValue Then

                intYear = mclsLunarCal.GetYear(mdtSolar)
                intMonth = mclsLunarCal.GetMonth(mdtSolar)
                intDay = mclsLunarCal.GetDayOfMonth(mdtSolar)

                'reset value
                mintVnDay = 0
                mintVnMonth = 0
                mintVnYear = 0

                mintLeapMonth = 0
                mintPreMonth = 0
                mintPreYear = 0

                'set year textbox
                txtYear.Text = intYear.ToString()

                'fill month
                xCreateMonth()

                'set month to start fill day
                cbMonth.SelectedIndex = intMonth - 1

                'set day
                cbDay.SelectedIndex = intDay - 1

            End If

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "frmCalendarVN_Load", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnChoose_Click, Choose button clicked
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnChoose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnChoose.Click

        Try
            'check valid and get year if it is valid
            If Not xIsValidYear() Then Exit Sub

            'is valid month, day?
            If cbMonth.SelectedIndex = -1 Or cbDay.SelectedIndex = -1 Then

                basCommon.fncMessageWarning(mcstrMessageInvalid)
                Exit Sub

            End If

            mdtSolar = mclsLunarCal.fncGetSolarDate(mintVnDay, mintVnMonth, mintVnYear, lblVnCal, lblEnCal)

            Me.Close()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "btnChoose_Click", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnBack_Click, Back button clicked
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click

        Try
            Me.Close()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnBack_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtYear_LostFocus, Year box lost focus
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtYear_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtYear.LostFocus

        Try
            'fill month
            xCreateMonth()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtYear_LostFocus", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : cbMonth_SelectedIndexChanged, month selected
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub cbMonth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbMonth.SelectedIndexChanged

        Try
            'check valid and get year if it is valid
            If Not xIsValidYear() Then Exit Sub

            'fill day
            xCreateDay()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "cbMonth_SelectedIndexChanged", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : cbDay_SelectedIndexChanged, day selected
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub cbDay_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbDay.SelectedIndexChanged

        Try
            'check valid and get year if it is valid
            If Not xIsValidYear() Then Exit Sub

            'get day
            mintVnDay = cbDay.SelectedIndex + 1

            'get date then show it
            mclsLunarCal.fncGetSolarDate(mintVnDay, mintVnMonth, mintVnYear, lblVnCal, lblEnCal)

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "cbDay_SelectedIndexChanged", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtYear_KeyPress, handle ENTER key in Year box
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtYear_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtYear.KeyPress

        Try

            'handle ENTER or TAB key
            If e.KeyChar = Convert.ToChar(Keys.Enter) Or e.KeyChar = Convert.ToChar(Keys.Tab) Then

                cbMonth.Enabled = True
                cbMonth.Focus()

            End If

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "txtYear_KeyPress", ex)

        End Try

    End Sub


#End Region

#Region "Form's function"


    '   ******************************************************************
    '　　　FUNCTION   : xInit, init values
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xInit()

        Try
            If mobjDtPicker Is Nothing Then mobjDtPicker = New DateTimePicker()

            mclsLunarCal = New clsLunarCalendar()

            mdtSolar = New Date()

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)

        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm,
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
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
    '　　　FUNCTION   : xGetLunarString,
    '      VALUE      : String
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetLunarString() As String

        xGetLunarString = ""

        Try
            Dim dtTime As Date = mobjDtPicker.Value
            Dim dtMin As Date

            dtMin = New Date(gcintMinYear, gcintMinMonth, gcintMinDay)

            'check null
            If dtTime > Date.MinValue Then

                'return string if this year is >minyear and <maxyear
                If dtTime >= dtMin And dtTime.Year <= gcintMaxYear Then Return mclsLunarCal.fncGetLunarDateString(dtTime)

            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetLunarString", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xIsValidYear, check validation of Year 
    '　　　VALUE      : Boolean
    '      PARAMS1    : intDay Integer, day of week
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsValidYear() As Boolean

        xIsValidYear = False

        Try

            'check empty year
            If String.IsNullOrEmpty(txtYear.Text) Then

                xClearAll()
                Exit Function

            End If

            'try to get year
            If Not Integer.TryParse(txtYear.Text, mintVnYear) Then

                xClearAll()
                'txtYear.Focus()
                basCommon.fncMessageWarning(mcstrMessageYrRange)
                Exit Function

            End If

            'check the range of year from 1800 to 2199
            If mintVnYear < gcintMinYear Or mintVnYear > gcintMaxYear Then

                'txtYear.Focus()
                basCommon.fncMessageWarning(mcstrMessageYrRange)
                Exit Function

            End If

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xIsValidYear", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClearAll, clear all controls' value
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xClearAll()

        Try

            cbMonth.Items.Clear()
            cbDay.Items.Clear()
            cbMonth.Enabled = False
            cbDay.Enabled = False

            lblEnCal.Text = ""
            lblVnCal.Text = ""
            txtYear.Text = ""

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClearAll", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xCreateMonth, fill month
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCreateMonth() As Boolean

        xCreateMonth = False

        Try
            'check valid and get year if it is valid
            If Not xIsValidYear() Then Exit Function

            'if year doesn't change, do nothing
            If mintPreYear = mintVnYear Then Exit Function

            'fill months
            If Not mclsLunarCal.fncCreateMonth(mintVnYear, cbMonth) Then Exit Function

            'enable month box
            cbMonth.Enabled = True

            'clear text and day list
            lblEnCal.Text = ""
            lblVnCal.Text = ""
            cbDay.Enabled = False

            'store year for changing
            mintPreYear = mintVnYear

            'reset pre-month
            mintPreMonth = -1

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreateMonth", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xCreateDay, fill day
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/05  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCreateDay() As Boolean

        xCreateDay = False

        Try

            'get month
            mintVnMonth = cbMonth.SelectedIndex + 1

            'if month and year don't change, do nothing
            If mintPreMonth = mintVnMonth Then Exit Function

            'enable day combobox and clear it
            cbDay.Enabled = True

            'fill days
            If Not mclsLunarCal.fncCreateDay(mintVnMonth, mintVnYear, cbDay) Then Exit Function

            'store month for changing
            mintPreMonth = mintVnMonth

            'clear text
            lblEnCal.Text = ""
            lblVnCal.Text = ""

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xCreateDay", ex)

        End Try

    End Function


#End Region



End Class