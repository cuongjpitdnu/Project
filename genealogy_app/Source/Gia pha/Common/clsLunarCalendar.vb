'   ******************************************************************
'      TITLE      : Lunar Calendar
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
'　　　FUNCTION   : Lunar Calendar Class
'      MEMO       : 
'      CREATE     : 2011/07/18  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class clsLunarCalendar
    Inherits VietnameseCalendar

    Private Const mcstrEnDateFormat As String = "Ngày {0:00} Tháng {1:00} Năm {2:0000}"     'string to format date

    Private Const mcstrVnDateFormat1 As String = "Ngày {0:00} Tháng {1:00} Năm "            'string to format date
    Private Const mcstrVnDateFormat2 As String = "Ngày {0:00} Tháng {1:00} (N) Năm "        'string to format date

    Private Const mcstrLunarDateFormat1 As String = "Âm lịch-{0:00}/{1:00}/"                  'string to format Lunar date
    Private Const mcstrLunarDateFormat2 As String = "Âm lịch-{0:00}/{1:00}(N)/"               'string to format lunar date
    Private Const mcstrLunarDateFormat3 As String = "{0:00}/{1:00}/"                        'string to format Lunar date
    Private Const mcstrLunarDateFormat4 As String = "{0:00}/{1:00}(N)/"                     'string to format lunar date

    Private Const mcstrDayMonthFormat As String = "{0}"                                     'string to format day and month
    Private Const mcstrLeapMonthFormat As String = "{0} (N)"                                'string to format leap month

    Private mintVnDay As Integer                                                            'Vietnamese day
    Private mintVnMonth As Integer                                                          'Vietnamese month
    Private mintVnYear As Integer                                                           'Vietnamese year
    Private mintLeapMonth As Integer                                                        'Leap month in a leap year


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/07/27  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New()

        'init values
        xInit()

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xInit, initialize values
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xInit()

        mintVnDay = 1
        mintVnMonth = 1
        mintVnYear = 1800

        mintLeapMonth = 0

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : fncGetSolarDate, get solar date from lunar date
    '　　　VALUE      : Date
    '      PARAMS1    : stCalendar of Moon Date    
    '      PARAMS2    : objVnCalLabel label, label to show lunar date
    '      PARAMS3    : objEnCalLabel label, label to show solar date
    '      MEMO       : 
    '      CREATE     : 2017/02/25  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetSolarDate(ByVal stMoonDate As stCalendar,
                                    Optional ByVal objVnCalLabel As Label = Nothing, _
                                    Optional ByVal objEnCalLabel As Label = Nothing) As Date

        Return fncGetSolarDate(stMoonDate.intDay, stMoonDate.intMonth, stMoonDate.intYear, objVnCalLabel, objEnCalLabel)

    End Function
    '   ******************************************************************
    '　　　FUNCTION   : fncGetSolarDate, get solar date from lunar date
    '　　　VALUE      : Date
    '      PARAMS1    : intVnDay integer, lunar day
    '      PARAMS2    : intVnMonth integer, lunar month
    '      PARAMS3    : intVnYear integer, lunar year
    '      PARAMS4    : objVnCalLabel label, label to show lunar date
    '      PARAMS5    : objEnCalLabel label, label to show solar date
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetSolarDate(ByVal intVnDay As Integer, _
                                    ByVal intVnMonth As Integer, _
                                    ByVal intVnYear As Integer, _
                                    Optional ByVal objVnCalLabel As Label = Nothing, _
                                    Optional ByVal objEnCalLabel As Label = Nothing) As Date

        fncGetSolarDate = Nothing

        Try
            Dim dtEn As Date = Nothing
            Dim strVnCal As String = ""
            Dim strEnCal As String = ""

            'get Solar date from Lunar date
            'if this day does not exist, try to get the day before
            Try
                dtEn = New Date(intVnYear, intVnMonth, intVnDay, Me)
            Catch ex As Exception
                Try
                    dtEn = New Date(intVnYear, intVnMonth, intVnDay - 1, Me)
                Catch exx As Exception
                    Throw exx
                End Try
            End Try

            fncGetSolarDate = dtEn

            If objVnCalLabel IsNot Nothing Then

                'output string lunar date
                strVnCal &= xGetDayName(dtEn.DayOfWeek + 1)
                strVnCal &= ", "
                strVnCal &= xVnCalString(intVnDay, intVnMonth, intVnYear)
                objVnCalLabel.Text = strVnCal

            End If

            If objEnCalLabel IsNot Nothing Then

                'output string solar date
                strEnCal &= xGetDayName(dtEn.DayOfWeek + 1)
                strEnCal &= ", "
                strEnCal &= xEnCalString(dtEn.Day, dtEn.Month, dtEn.Year)
                objEnCalLabel.Text = strEnCal

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Function

    ''' <summary>
    ''' Same fncGetSolarDate but catch for leap year
    ''' </summary>
    ''' <param name="intVnDay"></param>
    ''' <param name="intVnMonth"></param>
    ''' <param name="intVnYear"></param>
    ''' <param name="objVnCalLabel"></param>
    ''' <param name="objEnCalLabel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncGetSolarDate2(ByVal intVnDay As Integer, _
                                ByVal intVnMonth As Integer, _
                                ByVal intVnYear As Integer, _
                                Optional ByVal objVnCalLabel As Label = Nothing, _
                                Optional ByVal objEnCalLabel As Label = Nothing) As Date

        fncGetSolarDate2 = Nothing

        Try
            Dim dtEn As Date = Nothing
            Dim strVnCal As String = ""
            Dim strEnCal As String = ""

            'catch for leap year
            If IsLeapYear(intVnYear) Then
                If intVnMonth > GetLeapMonth(intVnYear) Then intVnMonth += 1
            End If

            'get Solar date from Lunar date
            'if this day does not exist, try to get the day before
            Try
                dtEn = New Date(intVnYear, intVnMonth, intVnDay, Me)
            Catch ex As Exception
                Try
                    dtEn = New Date(intVnYear, intVnMonth, intVnDay - 1, Me)
                Catch exx As Exception
                    Throw exx
                End Try
            End Try

            fncGetSolarDate2 = dtEn

            If objVnCalLabel IsNot Nothing Then

                'output string lunar date
                strVnCal &= xGetDayName(dtEn.DayOfWeek + 1)
                strVnCal &= ", "
                strVnCal &= xVnCalString(intVnDay, intVnMonth, intVnYear)
                objVnCalLabel.Text = strVnCal

            End If

            If objEnCalLabel IsNot Nothing Then

                'output string solar date
                strEnCal &= xGetDayName(dtEn.DayOfWeek + 1)
                strEnCal &= ", "
                strEnCal &= xEnCalString(dtEn.Day, dtEn.Month, dtEn.Year)
                objEnCalLabel.Text = strEnCal

            End If

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetLunarDateString, get lunar date string from solar date
    '　　　VALUE      : String
    '      PARAMS1    : dtSolar Date, solar date
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetLunarDateString(ByVal dtSolar As Date, Optional ByVal intFormat As Integer = 1) As String

        fncGetLunarDateString = ""

        Try
            Dim intDay As Integer = 1
            Dim intMonth As Integer = 1
            Dim intYear As Integer = 1800
            Dim intLeapMonth As Integer = 0
            Dim dtMin As Date                       'min date
            Dim strFormat1 As String
            Dim strFormat2 As String

            If intFormat = 1 Then
                strFormat1 = mcstrLunarDateFormat1
                strFormat2 = mcstrLunarDateFormat2
            Else
                strFormat1 = mcstrLunarDateFormat3
                strFormat2 = mcstrLunarDateFormat4
            End If

            dtMin = New Date(gcintMinYear, gcintMinMonth, gcintMinDay)

            'check null
            If dtSolar <= Date.MinValue Then Exit Function

            'return empty string if this year is >minyear and <maxyear
            If dtSolar <= dtMin Or dtSolar.Year >= gcintMaxYear Then Exit Function

            'get lunar day month year and leap month
            intDay = Me.GetDayOfMonth(dtSolar)
            intMonth = Me.GetMonth(dtSolar)
            intYear = Me.GetYear(dtSolar)

            If Me.IsLeapYear(intYear) Then intLeapMonth = Me.GetLeapMonth(intYear)

            'by default, this year is not a leap year and this month is smaller than leap month
            fncGetLunarDateString = String.Format(strFormat1, intDay, intMonth)

            'if this year is leap year
            If intLeapMonth > 0 Then

                'selected month is bigger than leap month
                If intMonth > intLeapMonth Then fncGetLunarDateString = String.Format(strFormat1, intDay, intMonth - 1)

                'selected month equals to leap month
                If intMonth = intLeapMonth Then fncGetLunarDateString = String.Format(strFormat2, intDay, intMonth - 1)

            End If


            'get year name
            fncGetLunarDateString &= intYear & " - " & Me.GetYearName(intYear)

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateMonth, create the list of month
    '　　　VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : intYear integer, Year
    '      PARAMS2    : objMonth ComboBox, control to fill
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateMonth(ByVal intYear As Integer, ByVal objMonth As ComboBox) As Boolean

        fncCreateMonth = False

        Try

            'clear combobox
            objMonth.Items.Clear()

            'reset leap month to 0
            Me.mintLeapMonth = 0

            'check leap year then set leap month
            If Me.IsLeapYear(intYear) Then Me.mintLeapMonth = Me.GetLeapMonth(intYear)

            'fill 12 months
            For i As Integer = 1 To 12

                objMonth.Items.Add(String.Format(mcstrDayMonthFormat, i))

            Next

            'if this year is a leap year, insert leap month
            If mintLeapMonth <> 0 Then

                objMonth.Items.Insert(Me.mintLeapMonth - 1, String.Format(mcstrLeapMonthFormat, Me.mintLeapMonth - 1))

            End If

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncCreateDay, create the list of day
    '　　　VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : intMonth integer, Month
    '      PARAMS2    : intYear integer, Year
    '      PARAMS3    : objMonth ComboBox, control to fill
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncCreateDay(ByVal intMonth As Integer, ByVal intYear As Integer, ByVal objDay As ComboBox) As Boolean

        fncCreateDay = False

        Try

            Dim intDayInMonth As Integer

            'get number of day in a month
            intDayInMonth = Me.GetDaysInMonth(intYear, intMonth)

            'clear combobox
            objDay.Items.Clear()

            'fill days
            For i As Integer = 1 To intDayInMonth

                objDay.Items.Add(String.Format(mcstrDayMonthFormat, i))

            Next

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xVnCalString, return string of Vietnamese date
    '　　　VALUE      : String
    '      PARAMS1    : intVnDay Integer, day
    '      PARAMS2    : intVnMonth Integer, month
    '      PARAMS3    : intVnYear Integer, year
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xVnCalString(ByVal intVnDay As Integer, _
                              ByVal intVnMonth As Integer, _
                              ByVal intVnYear As Integer) As String
        xVnCalString = ""

        Try
            'by default, this year is not a leap year and this month is smaller than leap month
            xVnCalString = String.Format(mcstrVnDateFormat1, intVnDay, intVnMonth)

            'if this year is leap year
            If mintLeapMonth > 0 Then

                'selected month is bigger than leap month
                If intVnMonth > mintLeapMonth Then xVnCalString = String.Format(mcstrVnDateFormat1, intVnDay, intVnMonth - 1)

                'selected month equals to leap month
                If intVnMonth = mintLeapMonth Then xVnCalString = String.Format(mcstrVnDateFormat2, intVnDay, intVnMonth - 1)

            End If


            xVnCalString &= Me.GetYearName(intVnYear)


        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xEnCalString, return string of Solar date
    '　　　VALUE      : String
    '      PARAMS1    : intVnDay Integer, day
    '      PARAMS2    : intVnMonth Integer, month
    '      PARAMS3    : intVnYear Integer, year
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xEnCalString(ByVal intDay As Integer,
                              ByVal intMonth As Integer,
                              ByVal intYear As Integer) As String
        xEnCalString = ""

        Try

            xEnCalString = String.Format(mcstrEnDateFormat, intDay, intMonth, intYear)

        Catch ex As Exception

            Throw ex

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : fncGetLunarYearName, Return Lunar year name 
    '　　　VALUE      : String
    '      PARAMS1    : intYear Integer, year number
    '      MEMO       : 
    '      CREATE     : 2016/12/20  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncIsValidSupportLunarYear(ByVal intYear As Integer) As Boolean

        Return intYear >= MinSupportedDateTime.Year And intYear <= MaxSupportedDateTime.Year

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : fncGetLunarYearName, Return Lunar year name 
    '　　　VALUE      : String
    '      PARAMS1    : intYear Integer, year number
    '      MEMO       : 
    '      CREATE     : 2016/12/20  AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncGetLunarYearName(ByVal intYear As Integer) As String

        fncGetLunarYearName = ""
        If fncIsValidSupportLunarYear(intYear) Then

            Return GetYearName(intYear)

        End If
    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetDayName, return day of week name
    '　　　VALUE      : String
    '      PARAMS1    : intDay Integer, day of week
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetDayName(ByVal intDay As Integer) As String

        xGetDayName = ""

        Try

            Select Case intDay
                Case 1
                    xGetDayName = "Chủ nhật"
                    Exit Function
                Case 2
                    xGetDayName = "Thứ hai"
                    Exit Function
                Case 3
                    xGetDayName = "Thứ ba"
                    Exit Function
                Case 4
                    xGetDayName = "Thứ tư"
                    Exit Function
                Case 5
                    xGetDayName = "Thứ năm"
                    Exit Function
                Case 6
                    xGetDayName = "Thứ sáu"
                    Exit Function
                Case 7
                    xGetDayName = "Thứ bảy"
                    Exit Function
            End Select

        Catch ex As Exception

            Throw ex

        End Try

    End Function

End Class
