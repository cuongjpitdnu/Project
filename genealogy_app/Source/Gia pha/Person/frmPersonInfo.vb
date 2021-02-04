'   ******************************************************************
'      TITLE      : Personal Information
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/07/29　AKB Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Personal information Class
'      MEMO       : 
'      CREATE     : 2011/07/29  AKB Quyet
'      UPDATE     : 
'   ******************************************************************
Public Class frmPersonInfo

    Sub New()
        ' TODO: Complete member initialization 
    End Sub

    Public Event evnRefresh(ByVal intCurID As Integer, ByVal blnRedraw As Boolean)
    Public Event evnActivated()


#Region "Class constants"

    Private Const mcstrClsName As String = "frmPersonInfo"                                          'class name

    Private Const mcstrTitle_AddMode As String = "Thêm thông tin thành viên mới"                              'ADD mode form's title
    Private Const mcstrTitle_EditMode As String = "Cập nhật thông tin thành viên"                              'EDIT mode form's title

    Private Const mcstrMissingFirstName As String = "Tên chưa được nhập."                                 'message missing first name
    Private Const mcstrMissingLastName As String = "Họ chưa được nhập."                                   'message missing last name
    Private Const mcstrMissingOfficeName As String = "Tên cơ quan chưa được nhập."                          'message missing office name
    Private Const mcstrMissingSchoolName As String = "Tên trường chưa được nhập."                           'message missing school name
    Private Const mcstrMissingFactName As String = "Tên sự kiện chưa được nhập."                             'message missing fact name

    Private Const mcstrConfirmSave As String = "Dữ liệu sẽ được ghi, bạn đã chắc chắn?"                         'message save confirmation
    Private Const mcstrConfirmSaveWord As String = "Dữ liệu sẽ được ghi trước khi xuất ra file Word, bạn đã chắc chắn chưa?"                         'message save confirmation

    Private Const mcstrConfirmClose As String = "Dữ liệu sẽ không được ghi, bạn có chắc chắn muốn đóng cửa sổ?"       'message cancel confirmation
    Private Const mcstrConfirmDelete As String = "Dữ liệu sẽ bị xóa, bạn có chắc chắn?"                         'message delete confirmation

    Private Const mcstrErrorWriteData As String = "Không thể ghi dữ liệu."                                   'message error writing data
    Private Const mcstrErrorReadData As String = "Không thể đọc dữ liệu."                                    'message error reading data
    Private Const mcstrErrorInitComponet As String = "Lỗi khởi tạo ban đầu."                                 'message error init
    Private Const mcstrErrorDelRow As String = "Không có dữ liệu nào được chọn."                              'message error delete row
    Private Const mcstrErrorDate As String = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc."                          'message error datetime
    Private Const mcstrErrorDateBirthDecease As String = "Ngày sinh phải nhỏ hơn ngày mất."                   'message error datetime

    Private Const mcstrErrorMainInfo As String = "Lỗi không thể cập nhật thông tin chính."                        'message error updating main information
    Private Const mcstrErrorContact As String = "Lỗi không thể cập nhật thông tin liên lạc."                        'message error updating contact
    Private Const mcstrErrorCareer As String = "Lỗi không thể cập nhật thông tin nghề nghiệp."                     'message error updating career
    Private Const mcstrErrorEdu As String = "Lỗi không thể cập nhật thông tin giáo dục."                           'message error updating education
    Private Const mcstrErrorFact As String = "Lỗi không thể cập nhật thông tin sự kiện."                           'message error updating fact

    Private Const mcstrNatName As String = "NAT_NAME"                                                 'display member
    Private Const mcstrNatID As String = "NAT_ID"                                                     'display value
    Private Const mcstrRelName As String = "REL_NAME"                                                 'display member
    Private Const mcstrRelID As String = "REL_ID"                                                     'display value

Private Const mcstrDateTimeFormat As String = "00"

#End Region


#Region "Class variable"

    Private mintID As Integer                                                       'id of member
    Private mintFormMode As Integer                                                 'mode of this form ADD / EDIT
    Private mintCareerMode As Integer                                               'mode for career process ADD / EDIT
    Private mintEduMode As Integer                                                  'mode for edu process ADD / EDIT
    Private mintFactMode As Integer                                                 'mode for fact process ADD / EDIT

    Private mintFather As Integer                                                   'father's ID, default is -1
    Private mintMother As Integer                                                   'mother's ID, default is -1
    Private mintGender As Integer                                                   'set gender for new member

    Private mblnModify As Boolean                                                   'flag to check if the form is modified
    Private mblnIsRollBack As Boolean                                               'flag to determine rolling back
    Private mblnTextChange As Boolean                                               'handle change on form
    Private mblnRaiseRefreshEvent As Boolean                                        'flag to raise event

    Private mstMainInfo As clsDbAccess.stMemberInfoMain                             'structure that stores main information
    Private mstContact As clsDbAccess.stMemberInfoContact                           'structure that stores contact information
    Private mstCareer As clsDbAccess.stCareer                                       'structure that stores career information
    Private mstEdu As clsDbAccess.stCareer                                          'structure that stores edu information
    Private mstFact As clsDbAccess.stFact                                           'structure that stores edu information
    Private mstRel As stRelationship                                                'structure that stores relationship information
    Private mstBirDate As stCalendar                                    'structure that stores date infor
    Private mstDeaDate As stCalendar                                    'structure that stores date infor
    Private mstBirDateSun As stCalendar                                 'structure that stores date infor
    Private mstBirDateMoon As stCalendar                               'structure that stores date infor
    Private mstDeaDateSun As stCalendar                                 'structure that stores date infor
    Private mstDeaDateMoon As stCalendar

    Private mstSdateCareer As stCalendar
    Private mstEdateCareer As stCalendar

    Private mstSdateEdu As stCalendar
    Private mstEdateEdu As stCalendar

    Private mstSdateFact As stCalendar
    Private mstEdateFact As stCalendar

    Private mstrAvatar As String                                                    'avatar path

    Private mfrmLunarCal As frmCalendarVN                                           'lunar calendar form
    Private mblnDelImg As Boolean
    Private mintIndexListIn As Integer
#End Region


#Region "Structure"

    '   ******************************************************************
    '　　　FUNCTION   : Relationship Structure
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Structure stRelationship

        Dim intMemId As Integer                 'member id

        Dim strLastName As String               'last name
        Dim strMidName As String                'middle name
        Dim strFirstName As String              'first name

        Dim intGender As Integer                'gender
        'Dim dtBirth As Date                     'birth
        Dim stBirthDaySun As stCalendar

        Dim strRemark As String                 'remark
        Dim intRelID As Integer                 'relation id
        Dim intFamilyOrder As Integer           'relation id

    End Structure

#End Region


#Region "Class properties"


    '   ******************************************************************
    '　　　FUNCTION   : MemberID Property, Set Member ID
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property MemberID() As Integer

        Get
            Return Me.mintID
        End Get

        Set(ByVal value As Integer)
            Me.mintID = value
        End Set

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : Mode Property, Mode of this form ADDD / EDIT
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property FormMode() As Integer

        Get
            Return Me.mintFormMode
        End Get

        Set(ByVal value As Integer)
            Me.mintFormMode = value
        End Set

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : FormModified Property, flag to check if the form is modified
    '      MEMO       : 
    '      CREATE     : 2011/08/29  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public ReadOnly Property FormModified() As Boolean

        Get
            Return Me.mblnModify
        End Get

    End Property


    '   ******************************************************************
    '　　　FUNCTION   : MemberGender Property, set gender for first load
    '      MEMO       : 
    '      CREATE     : 2011/08/29  AKB  Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Property MemberGender() As clsEnum.emGender

        Set(ByVal value As clsEnum.emGender)

            mintGender = value

            'base on gender to enable radio button
            Select Case mintGender

                Case clsEnum.emGender.MALE           'enable only male
                    rdMale.Checked = True

                Case clsEnum.emGender.FEMALE          'enable only female
                    rdFemale.Checked = True

                Case clsEnum.emGender.UNKNOW         'if it is unknown, re-enable all
                    rdUnknow.Checked = True

            End Select

        End Set

        Get
            If rdFemale.Checked Then
                Return clsEnum.emGender.FEMALE
            ElseIf rdMale.Checked Then
                Return clsEnum.emGender.MALE
            Else
                Return clsEnum.emGender.UNKNOW
            End If
        End Get

    End Property


#End Region


#Region "Class constructor"


    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/07/29  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New(ByVal intFormMode As Integer, Optional ByVal intId2Edit As Integer = -1)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        'set form mode
        Me.mintFormMode = intFormMode

        'set id to edit
        If intId2Edit > -1 Then Me.mintID = intId2Edit


    End Sub


#End Region


    '===================================================================================


#Region "Class event"


    '   ******************************************************************
    '　　　FUNCTION   : frmPersonInfo_Load, Form load
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmPersonInfo_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Try
            'clear all
            xClear()

            'init value
            xInit()
            mblnDelImg = False
            lblDelImg.Visible = False

            btnWordExport.Enabled = True

            'if the form in ADD mode
            If Me.mintFormMode = clsEnum.emMode.ADD Then

                'set form's title and icon
                Me.Text = mcstrTitle_AddMode
                Me.Icon = Global.GiaPha.My.Resources.Resources.add_user

                'load components
                If Not xModeAddLoad() Then
                    basCommon.fncMessageError(mcstrErrorInitComponet)
                    Me.Close()
                End If

            End If


            'if the form in EDIT mode
            If Me.mintFormMode = clsEnum.emMode.EDIT Then

                'set form's title and icon
                Me.Text = mcstrTitle_EditMode
                Me.Icon = Global.GiaPha.My.Resources.Resources.add_user

                'load components
                If Not xModeEditLoad() Then
                    basCommon.fncMessageError(mcstrErrorInitComponet)
                    Me.Close()
                End If

            End If

            AddHandler txtBDSun.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtBMSun.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtBYSun.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtBDLunar.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtBMLunar.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtBYLunar.LostFocus, AddressOf txtDateTextLostFocus

            AddHandler txtDDSun.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtDMSun.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtDYSun.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtDDLunar.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtDMLunar.LostFocus, AddressOf txtDateTextLostFocus
            AddHandler txtDYLunar.LostFocus, AddressOf txtDateTextLostFocus

            AddHandler txtBDSun.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtBMSun.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtBYSun.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtBDLunar.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtBMLunar.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtBYLunar.KeyPress, AddressOf txtNumberKeyPress

            AddHandler txtDDSun.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtDMSun.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtDYSun.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtDDLunar.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtDMLunar.KeyPress, AddressOf txtNumberKeyPress
            AddHandler txtDYLunar.KeyPress, AddressOf txtNumberKeyPress

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmPersonInfo_Load", ex)
            Me.Close()
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtNumberKeyPress, Manh add to prevent user input character not Number
    '      MEMO       : 
    '      CREATE     : 2016/12/27   AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtNumberKeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If Not Char.IsDigit(e.KeyChar) And Not Char.IsControl(e.KeyChar) Then
            e.Handled = True
        End If
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : txtNumberKeyPress, 
    '                   add to help user input directly in date textbox and auto conver to lunar/sun calendar
    '      MEMO       : 
    '      CREATE     : 2016/12/27   AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtDateTextLostFocus(sender As Object, e As EventArgs)
        Dim txt As TextBox = DirectCast(sender, TextBox)
        Dim stDateSun As stCalendar
        Dim stDateMoon As stCalendar


        Select Case txt.Name
            Case "txtBDSun", "txtBMSun", "txtBYSun"

                mstBirDateSun = xGetDate(txtBYSun.Text, txtBMSun.Text, txtBDSun.Text)
                stDateMoon = xDateProcess(mstBirDateSun, True)

                If (stDateMoon.IsValidDate) Then
                    xSetDateToTextBox(stDateMoon, txtBYLunar, txtBMLunar, txtBDLunar)
                    lblBYLunar.Text = fncGetSolarYearName(stDateMoon.intYear)
                    mstBirDateMoon = stDateMoon
                End If

            Case "txtBDLunar", "txtBMLunar", "txtBYLunar"
                mstBirDateMoon = xGetDate(txtBYLunar.Text, txtBMLunar.Text, txtBDLunar.Text)
                stDateSun = xDateProcess(mstBirDateMoon, False)

                If (stDateSun.IsValidDate) Then
                    xSetDateToTextBox(stDateSun, txtBYSun, txtBMSun, txtBDSun)
                    mstBirDateSun = stDateSun
                End If

                lblBYLunar.Text = fncGetSolarYearName(mstBirDateMoon.intYear)

            Case "txtDDSun", "txtDMSun", "txtDYSun"

                mstDeaDateSun = xGetDate(txtDYSun.Text, txtDMSun.Text, txtDDSun.Text)
                stDateMoon = xDateProcess(mstDeaDateSun, True)

                If (stDateMoon.IsValidDate) Then
                    xSetDateToTextBox(stDateMoon, txtDYLunar, txtDMLunar, txtDDLunar)
                    lblDYLunar.Text = fncGetSolarYearName(stDateMoon.intYear)

                    mstDeaDateMoon = stDateMoon
                End If

            Case "txtDDLunar", "txtDMLunar", "txtDYLunar"
                mstDeaDateMoon = xGetDate(txtDYLunar.Text, txtDMLunar.Text, txtDDLunar.Text)

                stDateSun = xDateProcess(mstDeaDateMoon, False)

                If (stDateSun.IsValidDate) Then

                    xSetDateToTextBox(stDateSun, txtDYSun, txtDMSun, txtDDSun)
                    mstDeaDateSun = stDateSun

                End If

                lblDYLunar.Text = fncGetSolarYearName(mstDeaDateMoon.intYear)
        End Select
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xSetDateToTextBox, 
    '                  Set date to Textbox
    '      MEMO       : 
    '      CREATE     : 2016/12/27   AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xSetDateToTextBox(stDate As stCalendar, txtYear As TextBox, txtMonth As TextBox, txtDay As TextBox, Optional blnClearBeforSet As Boolean = False)

        If (blnClearBeforSet) Then
            txtYear.Text = ""
            txtMonth.Text = ""
            txtDay.Text = ""
        End If

        With stDate
            If .intDay <> 0 Then txtDay.Text = .intDay.ToString()
            If .intMonth <> 0 Then txtMonth.Text = .intMonth.ToString()
            If .intYear <> 0 Then txtYear.Text = .intYear.ToString()
        End With

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : xGetDate, 
    '                  Get date from string value
    '      MEMO       : 
    '      CREATE     : 2016/12/27   AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetDate(ByVal strYear As String, strMonth As String, strDay As String) As stCalendar

        Dim stRet As stCalendar
        stRet.intYear = basCommon.fncCnvToInt(strYear)
        stRet.intMonth = basCommon.fncCnvToInt(strMonth)
        stRet.intDay = basCommon.fncCnvToInt(strDay)

        Return stRet

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xDateProcess, 
    '                  Process of converting date from/to Sun to/from Lunar
    '      MEMO       : 
    '      CREATE     : 2016/12/27   AKB Manh
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDateProcess(ByVal stDate As stCalendar, Optional blnSun As Boolean = True) As stCalendar

        Dim stRet As stCalendar

        If blnSun Then
            basCommon.fncGetLunarDate(stDate, stRet)
        Else
            basCommon.fncGetSolarDate(stDate, stRet)
        End If

        Return stRet

    End Function



    '   ******************************************************************
    '　　　FUNCTION   : frmPersonInfo_Shown, Form loaded
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmPersonInfo_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown

        Try
            'focus to first tab and LastName when form loaded
            tbcPersonInfo.SelectedIndex = 0
            txtLastName.SelectAll()
            txtLastName.Focus()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmPersonInfo_Shown", ex)
        End Try
    End Sub


    '2017/02/27 Save Data
    Private Function xSaveData(Optional ByVal blnSaveToExportToWord As Boolean = False) As Boolean

        Dim blnBeginTrans As Boolean = False
        Dim blnSuccess As Boolean = True
        xSaveData = False

        Try

            'check valid (first name and last name are required)
            If Not xIsValid() Then Exit Function

            'confirm message

            If (blnSaveToExportToWord) Then

                If Not basCommon.fncMessageConfirm(mcstrConfirmSaveWord, txtLastName) Then Exit Function

            Else

                If Not basCommon.fncMessageConfirm(mcstrConfirmSave, txtLastName) Then Exit Function

            End If


            'start stransaction
            If mblnIsRollBack Then blnBeginTrans = gobjDB.BeginTransaction()

            'save information
            blnSuccess = xSave() And blnSuccess

            'if rollback is off, return
            If Not mblnIsRollBack Then

                If blnSuccess Then
                    mblnModify = True           'set flag Modified to true

                    'raise refresh event to update quick search grid in main form
                    If mblnRaiseRefreshEvent Then RaiseEvent evnRefresh(mintID, True)


                    xSaveData = True

                    Me.Close()


                Else : mblnModify = False       'set flag Modified to false
                End If

                Exit Function

            End If

            'commit and close form
            If blnBeginTrans And blnSuccess Then

                gobjDB.Commit()
                mblnModify = True               'set flag Modified to true

                'raise refresh event to update quick search grid in main form
                If mblnRaiseRefreshEvent Then RaiseEvent evnRefresh(mintID, True)

                If (Not blnSaveToExportToWord) Then

                    xSaveData = True
                    Me.Close()

                End If


            Else
                'fail - rollback
                gobjDB.RollBack()
                xSaveData = False

            End If

            Me.mintFormMode = clsEnum.emMode.EDIT
            xSaveData = True

        Catch ex As Exception

            xSaveData = False
            If blnBeginTrans Then gobjDB.RollBack()
            basCommon.fncSaveErr(mcstrClsName, "xSaveData", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : btnOK_Click, OK button clicked
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        xSaveData()

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnCancel_Click, Cancel button clicked
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            If mblnTextChange Then
                'confirm to close window
                If basCommon.fncMessageConfirm(mcstrConfirmClose, txtFirstName) Then Me.Close()

            Else
                Me.Close()
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : frmPersonInfo_FormClosing, raise before closing
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmPersonInfo_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing

        Try
            mstMainInfo = Nothing
            mstContact = Nothing
            mstCareer = Nothing
            mstEdu = Nothing
            mstFact = Nothing
            mstRel = Nothing

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmPersonInfo_FormClosing", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : tbcPersonInfo_SelectedIndexChanged, Tab changed
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub tbcPersonInfo_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tbcPersonInfo.SelectedIndexChanged

        Try

            'focus to control when tab changed
            Select Case tbcPersonInfo.SelectedIndex

                Case 0
                    txtLastName.Focus()
                    Exit Sub
                Case 1
                    txtHometown.Focus()
                    Exit Sub
                Case 2
                    If rdCareerDetail.Checked Then
                        txtOffName.Focus()
                    Else
                        txtCareerGeneral.Focus()
                    End If

                    Exit Sub
                Case 3
                    If rdEduDetail.Checked Then
                        txtSchoolName.Focus()
                    Else
                        txtEduGeneral.Focus()
                    End If

                    Exit Sub
                Case 4
                    If rdFactDetail.Checked Then
                        txtFactName.Focus()
                    Else
                        txtFactGeneral.Focus()
                    End If

                    Exit Sub
                Case 5
                    txtRemark.Focus()
                    Exit Sub

            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tbcPersonInfo_SelectedIndexChanged", ex)
        End Try

    End Sub


    ''' <summary>
    ''' Radio button selection changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rdCareerDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdCareerDetail.CheckedChanged
        Try
            If rdCareerDetail.Checked Then
                pnCareerDetail.Visible = True
                pnCareerDetail.BringToFront()
                pnCareerGeneral.Visible = False
                pnCareerGeneral.SendToBack()
                txtOffName.Focus()
            Else
                pnCareerDetail.Visible = False
                pnCareerDetail.SendToBack()
                pnCareerGeneral.Visible = True
                pnCareerGeneral.BringToFront()
                txtCareerGeneral.Focus()
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "rdCareerDetail_CheckedChanged", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Radio button selection changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rdEduDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdEduDetail.CheckedChanged
        Try
            If rdEduDetail.Checked Then
                pnEduDetail.Visible = True
                pnEduDetail.BringToFront()
                pnEduGeneral.Visible = False
                pnEduGeneral.SendToBack()
                txtSchoolName.Focus()
            Else
                pnEduDetail.Visible = False
                pnEduDetail.SendToBack()
                pnEduGeneral.Visible = True
                pnEduGeneral.BringToFront()
                txtEduGeneral.Focus()
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "rdEduDetail_CheckedChanged", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Radio button selection changed
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub rdFactDetail_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdFactDetail.CheckedChanged
        Try
            If rdFactDetail.Checked Then
                pnFactDetail.Visible = True
                pnFactDetail.BringToFront()
                pnFactGeneral.Visible = False
                pnFactGeneral.SendToBack()
                txtFactName.Focus()
            Else
                pnFactDetail.Visible = False
                pnFactDetail.SendToBack()
                pnFactGeneral.Visible = True
                pnFactGeneral.BringToFront()
                txtFactGeneral.Focus()
            End If
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "rdFactDetail_CheckedChanged", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Move DGV row up
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnUp1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUp1.Click, btnUp2.Click, btnUp3.Click
        Try
            Dim btnMove As Button = CType(sender, Button)
            Dim strIndex As String = btnMove.Name.Substring(btnMove.Name.Length - 1)
            Select Case strIndex
                Case "1"
                    basCommon.fncMoveGridRow(dgvCareer, +1)
                Case "2"
                    basCommon.fncMoveGridRow(dgvEdu, +1)
                Case "3"
                    basCommon.fncMoveGridRow(dgvFact, +1)
            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnUp1_Click", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Move DGV row down
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnDown1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDown1.Click, btnDown3.Click, btnDown2.Click
        Try
            Dim btnMove As Button = CType(sender, Button)
            Dim strIndex As String = btnMove.Name.Substring(btnMove.Name.Length - 1)
            Select Case strIndex
                Case "1"
                    basCommon.fncMoveGridRow(dgvCareer, -1)
                Case "2"
                    basCommon.fncMoveGridRow(dgvEdu, -1)
                Case "3"
                    basCommon.fncMoveGridRow(dgvFact, -1)
            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDown1_Click", ex)
        End Try
    End Sub


#Region "Main information Tab"


    '   ******************************************************************
    '　　　FUNCTION   : chkDie_CheckedChanged, checkbox checked
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub chkDie_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkDie.CheckedChanged

        Try
            'set visible for Die panel
            pnDieInfo.Visible = chkDie.Checked

            'if checked, show lunar date
            ' If chkDie.Checked Then basCommon.fncShowLunarDate(mfrmLunarCal, dtpDieDay, lblDieDay, False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "chkDie_CheckedChanged", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dtpBirthDay_ValueChanged, calendar value changed
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dtpBirthDay_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            If mfrmLunarCal Is Nothing Then Exit Sub

            'show lunar date string
            ' basCommon.fncShowLunarDate(mfrmLunarCal, dtpBirthDay, lblBirthDay, False)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dtpBirthDay_ValueChanged", ex)
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : dtpBirthDay_Enter, control activated
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dtpBirthDay_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            mblnTextChange = True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dtpBirthDay_Enter", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : picImage_Click, choose an image
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub picImage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles picImage.Click

        Try
            'show dialog
            If Not dlgOpenImage.ShowDialog = Windows.Forms.DialogResult.OK Then Exit Sub

            'check validation of image and get path
            If Not basCommon.fncIsValidImage(dlgOpenImage.FileName) Then Exit Sub

            'crop image
            Using frmCropt As New frmCropImage(dlgOpenImage.FileName)

                frmCropt.ShowDialog()
                If Not frmCropt.ReturnOK Then Exit Sub

                mstrAvatar = dlgOpenImage.FileName

                'show image
                'picImage.ImageLocation = mstrAvatar
                picImage.Image = frmCropt.PatientPicture
                lblDelImg.Visible = True
                mblnDelImg = False
                'mstrAvatar = String.Empty

            End Using

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "picImage_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtLastName_Leave, text box leave
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtLastName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtLastName.Leave

        Try
            xProperCase(txtLastName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "txtLastName_Leave", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtMidName_Leave, text box leave
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtMidName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtMidName.Leave

        Try
            xProperCase(txtMidName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "txtMidName_Leave", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtFirstName_Leave, text box leave
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtFirstName_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtFirstName.Leave

        Try
            xProperCase(txtFirstName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "txtFirstName_Leave", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : txtAlias_Leave, text box leave
    '      MEMO       : 
    '      CREATE     : 2012/01/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub txtAlias_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAlias.Leave

        Try
            xProperCase(txtAlias)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "txtAlias_Leave", ex)
        End Try

    End Sub



    '   ******************************************************************
    '　　　FUNCTION   : btnBSunCal_Click, button select calendar clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnBSunCal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBSunCal.Click
        Try
            Dim strYearNameLunar As String

            'basCommon.fncSelectCal(mstBirDate, frmCalendar.emCalendar.SUN, mstMainInfo.stBirth.intDay, mstMainInfo.stBirth.intMon, mstMainInfo.stBirth.intYea, False, lblBirth)
            If Not basCommon.fncSelectCal(mstBirDateSun, frmCalendar.emCalendar.SUN, mstMainInfo.stBirthSun.intDay, mstMainInfo.stBirthSun.intMonth, mstMainInfo.stBirthSun.intYear, False, Nothing) Then Exit Sub

            xSetDateToTextBox(mstMainInfo.stBirthSun, txtBYSun, txtBMSun, txtBDSun, True)

            strYearNameLunar = basCommon.fncGetLunarDate(mstMainInfo.stBirthSun, mstMainInfo.stBirthLunar)
            If Not basCommon.fncIsBlank(strYearNameLunar) Then lblBYLunar.Text = strYearNameLunar

            mstBirDateMoon = mstMainInfo.stBirthLunar

            xSetDateToTextBox(mstMainInfo.stBirthLunar, txtBYLunar, txtBMLunar, txtBDLunar, False)

        Catch ex As Exception
            lblBYLunar.Text = ""
            basCommon.fncSaveErr(mcstrClsName, "btnBSunCal_Click", ex)
        End Try
    End Sub


    Private Sub btnBLunarCal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBLunarCal.Click
        Try

            Dim strYearNameLunar As String
            If Not basCommon.fncSelectCal(mstBirDateMoon, frmCalendar.emCalendar.LUNAR, mstMainInfo.stBirthLunar.intDay, mstMainInfo.stBirthLunar.intMonth, mstMainInfo.stBirthLunar.intYear, False, Nothing) Then Exit Sub

            xSetDateToTextBox(mstMainInfo.stBirthLunar, txtBYLunar, txtBMLunar, txtBDLunar, True)
            strYearNameLunar = basCommon.fncGetSolarDate(mstMainInfo.stBirthLunar, mstMainInfo.stBirthSun)

            If Not basCommon.fncIsBlank(strYearNameLunar) Then lblBYLunar.Text = strYearNameLunar
            mstBirDateSun = mstMainInfo.stBirthSun

            xSetDateToTextBox(mstMainInfo.stBirthSun, txtBYSun, txtBMSun, txtBDSun, False)

        Catch ex As Exception
            txtBDSun.Text = ""
            txtBMSun.Text = ""
            txtBYSun.Text = ""
            basCommon.fncSaveErr(mcstrClsName, "btnBLunarCal_Click", ex)
        End Try
    End Sub



    Private Sub btnDSunCal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDSunCal.Click
        Try
            Dim strYearNameLunar As String

            If Not basCommon.fncSelectCal(mstDeaDateSun, frmCalendar.emCalendar.SUN, mstMainInfo.stDeathSun.intDay, mstMainInfo.stDeathSun.intMonth, mstMainInfo.stDeathSun.intYear, False, Nothing) Then Exit Sub

            xSetDateToTextBox(mstMainInfo.stDeathSun, txtDYSun, txtDMSun, txtDDSun, True)

            strYearNameLunar = basCommon.fncGetLunarDate(mstMainInfo.stDeathSun, mstMainInfo.stDeathLunar)
            If Not basCommon.fncIsBlank(strYearNameLunar) Then lblDYLunar.Text = strYearNameLunar

            mstDeaDateMoon = mstMainInfo.stDeathLunar

            xSetDateToTextBox(mstMainInfo.stDeathLunar, txtDYLunar, txtDMLunar, txtDDLunar, False)

        Catch ex As Exception
            lblDYLunar.Text = ""
            txtDDLunar.Text = ""
            txtDMLunar.Text = ""
            txtDYLunar.Text = ""
            basCommon.fncSaveErr(mcstrClsName, "btnDSunCal_Click", ex)
        End Try
    End Sub

    Private Sub btnDLunarCal_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDLunarCal.Click
        Try

            If Not basCommon.fncSelectCal(mstDeaDateMoon, frmCalendar.emCalendar.LUNAR, mstMainInfo.stDeathLunar.intDay, mstMainInfo.stDeathLunar.intMonth, mstMainInfo.stDeathLunar.intYear, False, Nothing) Then Exit Sub

            xSetDateToTextBox(mstMainInfo.stDeathLunar, txtDYLunar, txtDMLunar, txtDDLunar, True)

            lblDYLunar.Text = basCommon.fncGetSolarDate(mstMainInfo.stDeathLunar, mstMainInfo.stDeathSun)

            mstDeaDateSun = mstMainInfo.stDeathSun

            xSetDateToTextBox(mstMainInfo.stDeathSun, txtDYSun, txtDMSun, txtDDSun, False)

        Catch ex As Exception
            txtDDSun.Text = ""
            txtDMSun.Text = ""
            txtDYSun.Text = ""
            basCommon.fncSaveErr(mcstrClsName, "btnBLunarCal_Click", ex)
        End Try
    End Sub


#End Region


#Region "Career Tab"


    '   ******************************************************************
    '　　　FUNCTION   : btnCreateCareer_Click, create new career
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCreateCareer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateCareer.Click

        Try

            xCreate()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCreateCareer_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnSaveCareer_Click, save a career
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnSaveCareer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveCareer.Click

        Try

            xAdd2Grid(txtOffName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSaveCareer_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnDelCareer_Click, delete career
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnDelCareer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelCareer.Click

        Try

            xDelete(dgvCareer, txtOffName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDelCareer_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvCareer_CellClick,
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvCareer_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvCareer.CellClick

        Try

            xFillFromCell(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvCareer_CellClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnStartCareer_Click, button select calendar clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnStartCareer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartCareer.Click
        Try

            basCommon.fncSelectCal(mstSdateCareer, frmCalendar.emCalendar.SUN, mstCareer.intSday, mstCareer.intSmon, mstCareer.intSyea, True, lblStartCareer)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnStartCareer_Click", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnEndCareer_Click, button select calendar clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnEndCareer_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEndCareer.Click
        Try

            basCommon.fncSelectCal(mstEdateCareer, frmCalendar.emCalendar.SUN, mstCareer.intEday, mstCareer.intEmon, mstCareer.intEyea, True, lblEndCareer)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnEndCareer_Click", ex)
        End Try
    End Sub


#End Region


#Region "Education Tab"


    '   ******************************************************************
    '　　　FUNCTION   : btnCreateEdu_Click, create new edu
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCreateEdu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateEdu.Click

        Try

            xCreate()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCreateEdu_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnSaveEdu_Click, save edu
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnSaveEdu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveEdu.Click

        Try

            xAdd2Grid(txtSchoolName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSaveEdu_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnDelEdu_Click, del edu
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnDelEdu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelEdu.Click

        Try

            xDelete(dgvEdu, txtSchoolName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDelEdu_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : dgvEdu_CellClick, load edu
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvEdu_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvEdu.CellClick

        Try

            xFillFromCell(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvEdu_CellClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnStartEdu_Click, button select calendar clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnStartEdu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartEdu.Click
        Try

            basCommon.fncSelectCal(mstSdateEdu, frmCalendar.emCalendar.SUN, mstEdu.intSday, mstEdu.intSmon, mstEdu.intSyea, True, lblStartEdu)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnStartEdu_Click", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnEndEdu_Click, button select calendar clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnEndEdu_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEndEdu.Click
        Try

            basCommon.fncSelectCal(mstEdateEdu, frmCalendar.emCalendar.SUN, mstEdu.intEday, mstEdu.intEmon, mstEdu.intEyea, True, lblEndEdu)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnEndEdu_Click", ex)
        End Try
    End Sub


#End Region


#Region "Fact Tab"


    '   ******************************************************************
    '　　　FUNCTION   : btnCreateFact_Click, create new fact
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnCreateFact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCreateFact.Click

        Try

            xCreate()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCreateFact_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnSaveFact_Click, save fact
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnSaveFact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSaveFact.Click

        Try

            xAdd2Grid(txtFactName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnSaveFact_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnDelFact_Click, delete fact
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnDelFact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelFact.Click

        Try

            xDelete(dgvFact, txtFactName)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnDelFact_Click", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnDelFact_Click, delete fact
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub dgvFact_CellClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles dgvFact.CellClick

        Try

            xFillFromCell(e)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "dgvFact_CellClick", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnStartFact_Click, button select calendar clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnStartFact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnStartFact.Click
        Try

            basCommon.fncSelectCal(mstSdateFact, frmCalendar.emCalendar.SUN, mstFact.intSday, mstFact.intSmon, mstFact.intSyea, True, lblStartFact)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnStartFact_Click", ex)
        End Try
    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : btnEndFact_Click, button select calendar clicked
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnEndFact_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEndFact.Click
        Try

            basCommon.fncSelectCal(mstEdateFact, frmCalendar.emCalendar.SUN, mstFact.intEday, mstFact.intEmon, mstFact.intEyea, True, lblEndFact)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnEndFact_Click", ex)
        End Try
    End Sub

#End Region



#End Region


    '===================================================================================


#Region "Class functions"


    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm, show this form
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : blnIsRollBack   Boolean, flag rollback
    '      MEMO       : 
    '      CREATE     : 2011/07/27  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncShowForm(Optional ByVal blnIsRollBack As Boolean = True,
                                Optional ByVal blnRaiseEvent As Boolean = True,
                                Optional ByVal intFather As Integer = -1,
                                Optional ByVal intMother As Integer = -1) As Boolean

        fncShowForm = False

        Try
            mblnIsRollBack = blnIsRollBack
            mblnRaiseRefreshEvent = blnRaiseEvent

            mintFather = intFather
            mintMother = intMother

            Me.ShowDialog()

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "fncShowForm", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xInit, init values
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xInit() As Boolean

        xInit = False

        Try

            If mfrmLunarCal Is Nothing Then Me.mfrmLunarCal = New frmCalendarVN()
            Me.mstrAvatar = String.Empty

            'set mode for career and edu process
            Me.mintCareerMode = clsEnum.emMode.ADD
            Me.mintEduMode = clsEnum.emMode.ADD

            Me.mblnTextChange = False
            Me.txtFamilyOrder.Value = 1


            If (mintFather > 0 Or mintMother > 0) Then


                Dim dtFather As DataTable = gobjDB.fncGetMemberMain(mintFather)
                Dim intKidOrder As Integer = 1

                If Not dtFather Is Nothing Then

                    txtLastName.Text = basCommon.fncCnvNullToString(dtFather.Rows(0).Item("LAST_NAME"))
                    txtHometown.Text = basCommon.fncCnvNullToString(dtFather.Rows(0).Item("HOMETOWN"))

                Else

                    Dim dtMother As DataTable = gobjDB.fncGetMemberMain(mintMother)
                    txtLastName.Text = basCommon.fncCnvNullToString(dtMother.Rows(0).Item("LAST_NAME"))
                    txtHometown.Text = basCommon.fncCnvNullToString(dtMother.Rows(0).Item("HOMETOWN"))

                End If


                If (mintFather > 0 And mintMother > 0) Then

                    intKidOrder = basCommon.fncGetKidMaxOrder(mintFather, mintMother)

                ElseIf (mintFather > 0 And mintMother <= 0) Then

                    intKidOrder = basCommon.fncGetKidMaxOrder(mintFather)

                ElseIf (mintFather <= 0 And mintMother > 0) Then

                    intKidOrder = basCommon.fncGetKidMaxOrder(mintMother)

                End If

                txtFamilyOrder.Value = intKidOrder
                '

            Else

                'set default value for father and mother id
                Me.mintFather = -1
                Me.mintMother = -1

            End If



            xAddHandler(tbcPersonInfo)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInit", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSave, save / insert information
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSave() As Boolean

        xSave = False

        Try

            'form in ADD mode
            If Me.mintFormMode = clsEnum.emMode.ADD Then

                'main information tab
                If Not xInsertMemberMain() Then
                    'show message and focus to tab
                    'basCommon.fncMessageError(mcstrErrorMainInfo)
                    tbcPersonInfo.SelectedIndex = 0
                    Exit Function

                End If

                'contact tab
                If Not xInsertContact() Then
                    'show message and focus to tab
                    'basCommon.fncMessageError(mcstrErrorContact)
                    tbcPersonInfo.SelectedIndex = 1
                    Exit Function

                End If

            End If


            'form in EDIT mode
            If Me.mintFormMode = clsEnum.emMode.EDIT Then

                'main information tab
                If Not xUpdateMemberMain() Then
                    'show message and focus to tab
                    'basCommon.fncMessageError(mcstrErrorMainInfo)
                    tbcPersonInfo.SelectedIndex = 0
                    Exit Function

                End If

                'contact tab
                If Not xUpdateContact() Then
                    'show message and focus to tab
                    'basCommon.fncMessageError(mcstrErrorContact)
                    tbcPersonInfo.SelectedIndex = 1
                    Exit Function

                End If

            End If



            'career tab
            If Not xSaveCareer() Then
                'show message and focus to tab
                'basCommon.fncMessageError(mcstrErrorCareer)
                tbcPersonInfo.SelectedIndex = 2
                Exit Function

            End If

            'career tab
            If Not xSaveEdu() Then
                'show message and focus to tab
                'basCommon.fncMessageError(mcstrErrorEdu)
                tbcPersonInfo.SelectedIndex = 3
                Exit Function

            End If

            'fact tab
            If Not xSaveFact() Then
                'show message and focus to tab
                'basCommon.fncMessageError(mcstrErrorFact)
                tbcPersonInfo.SelectedIndex = 4
                Exit Function

            End If


            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSave", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xModeAddLoad, load function in ADD mode
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xModeAddLoad() As Boolean

        xModeAddLoad = False

        Try
            'check trial version
            If Not basCommon.fncTrialCheck(True) Then
                Me.Close()
                Return True
            Else
                RaiseEvent evnActivated()
            End If

            'generate id for new member
            Me.mintID = gobjDB.fncGetMaxID(clsEnum.emTable.T_FMEMBER_MAIN)

            'if generation is fail
            If Not Me.mintID > -1 Then Exit Function

            Me.mintID = Me.mintID + 1

            'show lunar date string
            'basCommon.fncShowLunarDate(mfrmLunarCal, dtpBirthDay, lblBirthDay, False)

            'set visible of death information
            pnDieInfo.Visible = chkDie.Checked

            'fill combo box
            xFillCombo(cbNation, clsEnum.emTable.M_NATIONALITY)
            xFillCombo(cbReligion, clsEnum.emTable.M_RELIGION)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xModeAddLoad", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xModeEditLoad, load function in EDIT mode
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xModeEditLoad() As Boolean

        xModeEditLoad = False               'default return

        Try

            'fill data to all control in MAIN INFORMATION tab
            If Not xFillCtrlDataMain() Then

                'basCommon.fncMessageError(mcstrErrorReadData)
                Exit Function

            End If

            'fill data to all control in CONTACT tab
            If Not xFillCtrlDataContact() Then

                'basCommon.fncMessageError(mcstrErrorReadData)
                Exit Function

            End If


            'fill data to gridview in CAREER tab
            If Not xFillCtrlDataCareer() Then

                'basCommon.fncMessageError(mcstrErrorReadData)
                Exit Function

            End If

            'fill data to gridview in EDUCATION tab
            If Not xFillCtrlDataEdu() Then

                'basCommon.fncMessageError(mcstrErrorReadData)
                Exit Function

            End If

            'fill data to gridview in FACT tab
            If Not xFillCtrlDataFact() Then

                'basCommon.fncMessageError(mcstrErrorReadData)
                Exit Function

            End If

            'fill data to gridview in RELATION tab
            If Not xFillRelation() Then

                'basCommon.fncMessageError(mcstrErrorReadData)
                Exit Function

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xModeEditLoad", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetDateFromString, get start/end date from string
    '      VALUE      : String, true - success, false - failure
    '      PARAMS     : strInput    String, input string
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetDateFromString(ByVal strInput As String,
                                        ByRef stSdate As stCalendar,
                                        ByRef stEdate As stCalendar) As Boolean

        xGetDateFromString = False

        Dim strArray(1) As String
        Dim strTimeElement(2) As String

        Try
            'split into 2 string
            strArray = strInput.Split(New Char() {"-"c})

            'exit if there is no date
            If strArray.Length < 2 Then
                stSdate = Nothing
                stEdate = Nothing
                Return True
            End If

            'we use format dd/MM/yyyy, so when splits a string:
            '(0) -> day
            '(1) -> month
            '(2) -> year

            'get start date
            If basCommon.fncIsBlank(strArray(0).Trim()) Then
                stSdate = Nothing
            Else
                strTimeElement = strArray(0).Split(New Char() {"/"c})
                Integer.TryParse(strTimeElement(0), stSdate.intDay)
                Integer.TryParse(strTimeElement(1), stSdate.intMonth)
                Integer.TryParse(strTimeElement(2), stSdate.intYear)
            End If

            'get end date
            If basCommon.fncIsBlank(strArray(1).Trim()) Then
                stEdate = Nothing
            Else
                strTimeElement = strArray(1).Split(New Char() {"/"c})
                Integer.TryParse(strTimeElement(0), stEdate.intDay)
                Integer.TryParse(strTimeElement(1), stEdate.intMonth)
                Integer.TryParse(strTimeElement(2), stEdate.intYear)
            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetDateFromString", ex)
        Finally
            Erase strArray
            Erase strTimeElement
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xIsValid, check validation
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsValid() As Boolean

        xIsValid = False

        Try

            'check last name
            If basCommon.fncIsBlank(txtLastName.Text.Trim(), mcstrMissingLastName, txtLastName) Then

                'focus to MAIN INFORMATION tab
                tbcPersonInfo.SelectedIndex = 0
                Exit Function

            End If

            'check first name
            If basCommon.fncIsBlank(txtFirstName.Text.Trim(), mcstrMissingFirstName, txtFirstName) Then

                'focus to MAIN INFORMATION tab and FirstName control
                tbcPersonInfo.SelectedIndex = 0
                Exit Function

            End If

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xIsValid", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xIsValidInfo, check validation
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsValidInfo() As Boolean

        xIsValidInfo = False

        Try

            Select Case tbcPersonInfo.SelectedIndex

                'Tab CAREER
                Case 2
                    'check blank office name
                    If basCommon.fncIsBlank(txtOffName.Text.Trim(), mcstrMissingOfficeName, txtOffName) Then Exit Function

                    'Tab EDUCATION
                Case 3

                    'check blank school name
                    If basCommon.fncIsBlank(txtSchoolName.Text.Trim(), mcstrMissingSchoolName, txtSchoolName) Then Exit Function

                    'Tab FACT
                Case 4
                    'check blank fact name
                    If basCommon.fncIsBlank(txtFactName.Text.Trim(), mcstrMissingFactName, txtFactName) Then Exit Function

            End Select

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xIsValidInfo", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xCreate, create new information event
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xCreate() As Boolean

        xCreate = False

        Try

            Select Case tbcPersonInfo.SelectedIndex


                Case 2      'Tab CAREER

                    'clear all textbox and uncheck datetime picker
                    txtOffName.Text = ""
                    txtOffAddr.Text = ""
                    txtPosition.Text = ""
                    txtOccupt.Text = ""
                    lblStartCareer.Text = basConst.gcstrDateUnknown
                    lblEndCareer.Text = basConst.gcstrDateUnknown
                    mstSdateCareer = Nothing
                    mstEdateCareer = Nothing
                    'dtpStartCareer.Checked = False
                    'dtpEndCareer.Checked = False

                    'focus to office name
                    txtOffName.Focus()

                    'do not select grid
                    dgvCareer.ClearSelection()
                    dgvCareer.CurrentCell = Nothing

                    'set mode is ADD mode
                    Me.mintCareerMode = clsEnum.emMode.ADD

                    Return True


                Case 3      'Tab EDUCATION

                    'clear all textbox and uncheck datetime picker
                    txtSchoolName.Text = ""
                    txtRemarkEdu.Text = ""
                    lblStartEdu.Text = basConst.gcstrDateUnknown
                    lblEndEdu.Text = basConst.gcstrDateUnknown
                    mstSdateEdu = Nothing
                    mstEdateEdu = Nothing
                    'dtpStartEdu.Checked = False
                    'dtpEndEdu.Checked = False

                    'focus to school name
                    txtSchoolName.Focus()

                    'do not select grid
                    dgvEdu.ClearSelection()
                    dgvEdu.CurrentCell = Nothing

                    'set mode is ADD mode
                    Me.mintEduMode = clsEnum.emMode.ADD

                    Return True


                Case 4      'Tab FACT

                    'clear all textbox and uncheck datetime picker
                    txtFactName.Text = ""
                    txtFactPlace.Text = ""
                    txtFactDesc.Text = ""
                    lblStartFact.Text = basConst.gcstrDateUnknown
                    lblEndFact.Text = basConst.gcstrDateUnknown
                    mstSdateFact = Nothing
                    mstEdateFact = Nothing
                    'dtpStartFact.Checked = False
                    'dtpEndFact.Checked = False

                    'focus to school name
                    txtFactName.Focus()

                    'do not select grid
                    dgvFact.ClearSelection()
                    dgvFact.CurrentCell = Nothing

                    'set mode is ADD mode
                    Me.mintFactMode = clsEnum.emMode.ADD

                    Return True

            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xCreate", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xDelete, delete row from datagridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : dgvInfo DataGridView, dgv to delete from
    '      PARAMS2    : objCtrl2Focus Control, control to focus 
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xDelete(ByVal dgvInfo As DataGridView, ByVal objCtrl2Focus As Control) As Boolean

        xDelete = False

        Try

            'if there is no row selected
            If dgvInfo.Rows.Count < 1 Or dgvInfo.CurrentRow Is Nothing Then

                basCommon.fncMessageWarning(mcstrErrorDelRow, objCtrl2Focus)
                Exit Function

            End If

            'confirm
            If Not basCommon.fncMessageConfirm(mcstrConfirmDelete, objCtrl2Focus) Then Exit Function

            'or delete row if there is a selected row
            dgvInfo.Rows.Remove(dgvInfo.CurrentRow)

            'call create new to clear data
            xCreate()


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xDelete", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xClear, clear all
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     :  
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xClear() As Boolean

        xClear = False

        Try
            mblnModify = False

            'MAIN tab
            txtFirstName.Clear()
            txtMidName.Clear()
            txtLastName.Clear()
            txtAlias.Clear()
            txtBDLunar.Clear()
            txtBMLunar.Clear()
            txtBYLunar.Clear()
            txtBDSun.Clear()
            txtBMSun.Clear()
            txtBYSun.Clear()
            txtDDLunar.Clear()
            txtDMLunar.Clear()
            txtDYLunar.Clear()
            txtDMSun.Clear()
            txtDYSun.Clear()
            txtDDSun.Clear()
            lblBYLunar.Text = ""
            lblDYLunar.Text = ""
            txtFamilyOrder.Value = 1
            'lblBirthDay.Text = ""
            'lblDieDay.Text = ""
            'mstBirDate = Nothing
            'mstDeaDate = Nothing
            mstBirDateSun = Nothing
            mstBirDateMoon = Nothing
            mstDeaDateSun = Nothing
            mstDeaDateMoon = Nothing
            'lblBirth.Text = "Chưa rõ"
            'lblDeath.Text = "Chưa rõ"
            'dtpBirthDay.Value = Date.Today
            'dtpBirthDay.Checked = False
            txtBirthPlace.Clear()
            picImage.Image.Dispose()
            picImage.Image = My.Resources.useradd64
            cbNation.SelectedIndex = clsDefine.NONE_VALUE
            cbReligion.SelectedIndex = clsDefine.NONE_VALUE
            chkDie.Checked = False
            txtBuryPlace.Clear()
            'dtpDieDay.Checked = False

            'CONTACT tab
            txtHometown.Clear()
            txtAddress.Clear()
            txtPhone1.Clear()
            txtPhone2.Clear()
            txtMail1.Clear()
            txtMail2.Clear()
            txtFax.Clear()
            txtURL.Clear()
            txtIM.Clear()
            txtRemarkContact.Clear()

            'CAREER tab
            txtOffName.Clear()
            txtOffName.Clear()
            txtPosition.Clear()
            txtOccupt.Clear()
            'dtpStartCareer.Checked = False
            'dtpEndCareer.Checked = False
            dgvCareer.Rows.Clear()
            mstSdateCareer = Nothing
            mstEdateCareer = Nothing
            lblStartCareer.Text = basConst.gcstrDateUnknown
            lblEndCareer.Text = basConst.gcstrDateUnknown
            'dgvCareer.ClearSelection()
            'dgvCareer.CurrentCell = Nothing
            rdCareerDetail.Checked = True
            txtCareerGeneral.Clear()

            'EDU tab
            txtSchoolName.Clear()
            txtRemarkEdu.Clear()
            'dtpStartEdu.Checked = False
            'dtpEndEdu.Checked = False
            dgvEdu.Rows.Clear()
            mstSdateEdu = Nothing
            mstEdateEdu = Nothing
            lblStartEdu.Text = basConst.gcstrDateUnknown
            lblEndEdu.Text = basConst.gcstrDateUnknown
            'dgvEdu.ClearSelection()
            'dgvEdu.CurrentCell = Nothing
            rdEduDetail.Checked = True
            txtEduGeneral.Clear()

            'FACT tab
            txtFactName.Clear()
            txtFactPlace.Clear()
            txtFactDesc.Clear()
            'dtpStartFact.Checked = False
            'dtpEndFact.Checked = False
            dgvFact.Rows.Clear()
            mstSdateFact = Nothing
            mstEdateFact = Nothing
            lblStartFact.Text = basConst.gcstrDateUnknown
            lblEndFact.Text = basConst.gcstrDateUnknown
            'dgvFact.ClearSelection()
            'dgvFact.CurrentCell = Nothing
            rdFactDetail.Checked = True
            txtFactGeneral.Clear()

            'REMARK
            'txtRemark.Clear()
            txtRemark.Text = ""

            'RELATION
            dgvRel.Rows.Clear()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xClear", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAdd2Grid, add information 2 gridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : objControl2Focus Control, control to focus 
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAdd2Grid(Optional ByVal objCtrl2Focus As Control = Nothing) As Boolean

        xAdd2Grid = False

        Try

            'check valid of data
            If Not xIsValidInfo() Then Exit Function

            Select Case tbcPersonInfo.SelectedIndex

                Case 2      'CAREER tab

                    'in ADD mode
                    If Me.mintCareerMode = clsEnum.emMode.ADD Then _
                        If Not xAddCareer2Grid() Then Exit Function
                    'If Not xAddCareer2Grid() Then basCommon.fncMessageError(mcstrErrorWriteData, objCtrl2Focus)


                    'in EDIT mode
                    If Me.mintCareerMode = clsEnum.emMode.EDIT Then _
                        If Not xSaveCareer2Grid(dgvCareer.SelectedRows(0).Index) Then Exit Function
                    'If Not xSaveCareer2Grid(dgvCareer.CurrentRow.Index) Then basCommon.fncMessageError(mcstrErrorWriteData, objCtrl2Focus)


                Case 3      'EDU tab

                    'in ADD mode
                    If Me.mintEduMode = clsEnum.emMode.ADD Then _
                        If Not xAddEdu2Grid() Then Exit Function
                    'If Not xAddEdu2Grid() Then basCommon.fncMessageError(mcstrErrorWriteData, objCtrl2Focus)


                    'in EDIT mode
                    If Me.mintEduMode = clsEnum.emMode.EDIT Then _
                        If Not xSaveEdu2Grid(dgvEdu.SelectedRows(0).Index) Then Exit Function
                    'If Not xSaveEdu2Grid(dgvEdu.CurrentRow.Index) Then basCommon.fncMessageError(mcstrErrorWriteData, objCtrl2Focus)


                Case 4      'FACT tab

                    'in ADD mode
                    If Me.mintFactMode = clsEnum.emMode.ADD Then _
                        If Not xAddFact2Grid() Then Exit Function
                    'If Not xAddFact2Grid() Then basCommon.fncMessageError(mcstrErrorWriteData, objCtrl2Focus)


                    'in EDIT mode
                    If Me.mintFactMode = clsEnum.emMode.EDIT Then _
                        If Not xSaveFact2Grid(dgvFact.SelectedRows(0).Index) Then Exit Function
                    'If Not xSaveFact2Grid(dgvFact.CurrentRow.Index) Then basCommon.fncMessageError(mcstrErrorWriteData, objCtrl2Focus)

            End Select

            'call create new to clear data
            xCreate()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAdd2Grid", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillFromCell, fill data form cell to controls
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : e DataGridViewCellEventArgs, event agurment
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillFromCell(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean

        xFillFromCell = False

        Try

            Select Case tbcPersonInfo.SelectedIndex

                Case 2      'CAREER tab
                    xFillCareerFromCell(e)
                    Return True

                Case 3      'EDU tab
                    xFillEduFromCell(e)
                    Return True

                Case 4      'FACT tab
                    xFillFactFromCell(e)
                    Return True

            End Select

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillFromCell", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xProperCase, upper case first letter
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : objTextBox TextBox, 
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xProperCase(ByVal objTextBox As TextBox) As Boolean

        xProperCase = False

        Try
            'objTextBox.Text = StrConv(objTextBox.Text, VbStrConv.ProperCase, 1066)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xProperCase", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddHandler, add handler for all children of control
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : vobjCtrl Control, control to search
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddHandler(ByVal vobjCtrl As Control) As Boolean

        xAddHandler = False

        Try

            For Each ctrChild As Control In vobjCtrl.Controls

                AddHandler ctrChild.KeyPress, AddressOf xKeyPress
                AddHandler ctrChild.MouseClick, AddressOf xMousePress

                xAddHandler(ctrChild)

            Next

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xAddHandler", ex)
            Return Nothing

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xKeyPress, handler keypress event
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xKeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        Try
            mblnTextChange = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xKeyPress", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xMousePress, handler mouse event
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub xMousePress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs)

        Try
            mblnTextChange = True
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xMousePress", ex)
        End Try

    End Sub


    '   ******************************************************************
    '　　　FUNCTION   : xIsValidDateRange, compare 2 date
    '      VALUE      : Boolean, true - from<to, false - from>to
    '      PARAMS1    : dtFrom Date, date to compare
    '      PARAMS2    : dtTo Date, date to compare
    '      MEMO       : 
    '      CREATE     : 2012/01/09  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xIsValidDateRange(ByVal stStart As stCalendar, ByVal stEnd As stCalendar) As Boolean

        Try
            'if both of 2 year is invalid
            If stStart.intYear <= 0 And stEnd.intYear <= 0 Then

                Return True

            ElseIf stStart.intYear <= 0 Or stEnd.intYear <= 0 Then 'if there is an invalid year

                Return True

            Else    '2 year is ok

                'check year
                If stStart.intYear < stEnd.intYear Then Return True
                If stStart.intYear > stEnd.intYear Then Return False

                '2 year are equal
                If stStart.intMonth = stEnd.intMonth Then

                    If stStart.intDay <= stEnd.intDay Then Return True Else Return False

                ElseIf stStart.intMonth > stEnd.intMonth Then
                    Return False
                Else
                    Return True
                End If

            End If

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xIsValidDateRange", ex)
            Return Nothing

        End Try

    End Function


#Region "Main information"


    '   ******************************************************************
    '　　　FUNCTION   : xInsertMemberMain, Insert a new member
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xInsertMemberMain() As Boolean

        xInsertMemberMain = False                       'default return

        Try

            'get data from controls in MAIN INFORMATION tab
            If Not xGetCtrlDataMain() Then Exit Function

            'update to database
            If Not gobjDB.fncInsertMemberMain(Me.mstMainInfo, False) Then Exit Function

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xInsertMemberMain", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xUpdateMemberMain, Update member's information
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xUpdateMemberMain() As Boolean

        xUpdateMemberMain = False                       'default return

        Try

            'get data from controls in MAIN INFORMATION tab
            If Not xGetCtrlDataMain() Then Exit Function

            'update to database
            If Not gobjDB.fncUpdateMemberMain(Me.mstMainInfo, False) Then Exit Function

            Return True


        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xUpdateMemberMain", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCtrlDataMain, fill data to controls
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCtrlDataMain() As Boolean

        xFillCtrlDataMain = False

        Try
            'fill Nation and Religion Combobox
            xFillCombo(cbNation, clsEnum.emTable.M_NATIONALITY)
            xFillCombo(cbReligion, clsEnum.emTable.M_RELIGION)

            'init state of controls
            'dtpBirthDay.Checked = False
            'dtpDieDay.Checked = False
            chkDie.Checked = False
            pnDieInfo.Visible = False

            'get data to structure
            If Not xGetStrucMemberMain(Me.mintID) Then Exit Function

            'start filling
            With Me.mstMainInfo

                'name
                txtFirstName.Text = .strFirstName
                txtMidName.Text = .strMidName
                txtLastName.Text = .strLastName
                txtAlias.Text = .strAlias

                'order
                txtFamilyOrder.Value = .intFamilyOrder

                'birthday - show birth date if valid
                'If .dtBirth <> Date.MinValue Then

                '    dtpBirthDay.Checked = True
                '    dtpBirthDay.Value = .dtBirth
                '    'show lunar date string
                '    basCommon.fncShowLunarDate(mfrmLunarCal, dtpBirthDay, lblBirthDay, False)

                'End If
                'lblBirth.Text = basCommon.fncGetDateName(basConst.gcstrDateUnknown, .stBirth.intDay, .stBirth.intMon, .stBirth.intYea, False)
                With mstBirDateSun
                    If mstBirDateSun.intDay <> 0 Then txtBDSun.Text = mstBirDateSun.intDay.ToString()
                    If mstBirDateSun.intMonth <> 0 Then txtBMSun.Text = mstBirDateSun.intMonth.ToString()
                    If mstBirDateSun.intYear <> 0 Then txtBYSun.Text = mstBirDateSun.intYear.ToString()
                End With

                With mstMainInfo.stBirthLunar
                    If .intDay <> 0 Then txtBDLunar.Text = .intDay.ToString()
                    If .intMonth <> 0 Then txtBMLunar.Text = .intMonth.ToString()
                    If .intYear <> 0 Then
                        txtBYLunar.Text = .intYear.ToString()
                        lblBYLunar.Text = basCommon.fncGetSolarYearName(.intYear)
                    End If
                End With

                'gender
                If .intGender = clsEnum.emGender.MALE Then rdMale.Checked = True
                If .intGender = clsEnum.emGender.FEMALE Then rdFemale.Checked = True
                If .intGender = clsEnum.emGender.UNKNOW Then rdUnknow.Checked = True

                're-enable tabstop after disabling/enabling
                'rdUnknow.TabStop = True
                'rdFemale.TabStop = True
                'rdMale.TabStop = True

                'birth place
                txtBirthPlace.Text = .strBirthPlace

                'nationality - set if avaiable or to be default if unavaiable
                If Not String.IsNullOrEmpty(.strNationality) Then
                    'set value
                    xSetSelected(cbNation, .strNationality)
                Else
                    'or default
                    xSetSelected(cbNation, basConst.gcstrDefaultNation)
                End If


                'religion - set if avaiable or to be default if unavaiable
                If Not String.IsNullOrEmpty(.strReligion) Then
                    'set value
                    xSetSelected(cbReligion, .strReligion)
                Else
                    'or default
                    xSetSelected(cbReligion, basConst.gcstrDefaultRelition)
                End If


                'deceased - if dead then show data
                If .intDeceased = basConst.gcintDIED Then

                    'enable control
                    chkDie.Checked = True
                    pnDieInfo.Visible = True

                    'get date of death
                    'If .dtDeceased <> Date.MinValue Then

                    '    dtpDieDay.Checked = True
                    '    dtpDieDay.Value = .dtDeceased
                    '    'show lunar date string
                    '    basCommon.fncShowLunarDate(mfrmLunarCal, dtpDieDay, lblDieDay, False)

                    'End If
                    'lblDeath.Text = basCommon.fncGetDateName(basConst.gcstrDateUnknown, .stDeath.intDay, .stDeath.intMon, .stDeath.intYea, False, True)
                    With mstMainInfo.stDeathSun
                        If .intDay <> 0 Then txtDDSun.Text = .intDay.ToString()
                        If .intMonth <> 0 Then txtDMSun.Text = .intMonth.ToString()
                        If .intYear <> 0 Then txtDYSun.Text = .intYear.ToString()
                    End With

                    With mstMainInfo.stDeathLunar
                        If .intDay <> 0 Then txtDDLunar.Text = .intDay.ToString()
                        If .intMonth <> 0 Then txtDMLunar.Text = .intMonth.ToString()
                        If .intYear <> 0 Then
                            lblDYLunar.Text = basCommon.fncGetSolarYearName(.intYear)
                            txtDYLunar.Text = .intYear.ToString()
                        End If
                    End With
                    'get bury place
                    txtBuryPlace.Text = .strBuryPlace

                End If

                'avatar path
                If Not String.IsNullOrEmpty(.strAvatar) Then

                    Me.mstrAvatar = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & .strAvatar

                    If System.IO.File.Exists(Me.mstrAvatar) Then
                        picImage.ImageLocation = Me.mstrAvatar
                        lblDelImg.Visible = True
                    Else
                        picImage.Image = My.Resources.useradd64
                        Me.mstrAvatar = String.Empty
                    End If

                End If

                'general information of career, education and fact
                txtCareerGeneral.Text = .strCareerGeneral
                txtEduGeneral.Text = .strEduGeneral
                txtFactGeneral.Text = .strFactGeneral

                If .intCareerType = clsEnum.emInputType.DETAIL Then rdCareerDetail.Checked = True Else rdCareerGeneral.Checked = True
                If .intEduType = clsEnum.emInputType.DETAIL Then rdEduDetail.Checked = True Else rdEduGeneral.Checked = True
                If .intFactType = clsEnum.emInputType.DETAIL Then rdFactDetail.Checked = True Else rdFactGeneral.Checked = True

                'remark - in REMARK tab
                'txtRemark.Text = .strRemark
                'txtRemark.Rtf = .strRemark
                fncSetRemarkField(txtRemark, .strRemark)

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCtrlDataMain", ex)
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xGetStrucMemberMain, fill data to structure 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : intID Integer, id to get infor 
    '      PARAMS2    : stMemInfo stMemberInfoMain, return structure 
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xGetStrucMemberMain(ByVal intID As Integer) As Boolean

        xGetStrucMemberMain = False               'default return is false

        Dim dtTable As DataTable                    'datatable that store member's infor

        dtTable = Nothing

        Try

            'get member data
            dtTable = gobjDB.fncGetMemberMain(intID)

            'check for empty data
            If dtTable Is Nothing Then Exit Function

            'init value
            Me.mstMainInfo = Nothing

            'fill data to MainInfo structure
            With dtTable.Rows(0)

                'member id
                Me.mstMainInfo.intID = intID

                'name
                Me.mstMainInfo.strFirstName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                Me.mstMainInfo.strMidName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                Me.mstMainInfo.strLastName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                Me.mstMainInfo.strAlias = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))

                'birthday
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), Me.mstMainInfo.dtBirth)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), Me.mstMainInfo.stBirth.intDay)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), Me.mstMainInfo.stBirth.intMon)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), Me.mstMainInfo.stBirth.intYea)
                'mstBirDate.intDay = Me.mstMainInfo.stBirth.intDay
                'mstBirDate.intMon = Me.mstMainInfo.stBirth.intMon
                'mstBirDate.intYea = Me.mstMainInfo.stBirth.intYea
                'birthday
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), Me.mstMainInfo.stBirthSun.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), Me.mstMainInfo.stBirthSun.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), Me.mstMainInfo.stBirthSun.intYear)

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY_LUNAR")), Me.mstMainInfo.stBirthLunar.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON_LUNAR")), Me.mstMainInfo.stBirthLunar.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA_LUNAR")), Me.mstMainInfo.stBirthLunar.intYear)

                mstBirDateSun.intDay = Me.mstMainInfo.stBirthSun.intDay
                mstBirDateSun.intMonth = Me.mstMainInfo.stBirthSun.intMonth
                mstBirDateSun.intYear = Me.mstMainInfo.stBirthSun.intYear

                mstBirDateMoon.intDay = Me.mstMainInfo.stBirthLunar.intDay
                mstBirDateMoon.intMonth = Me.mstMainInfo.stBirthLunar.intMonth
                mstBirDateMoon.intYear = Me.mstMainInfo.stBirthLunar.intYear

                'gender
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("GENDER")), Me.mstMainInfo.intGender)

                'gender
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("FAMILY_ORDER")), Me.mstMainInfo.intFamilyOrder)

                'birth place
                Me.mstMainInfo.strBirthPlace = basCommon.fncCnvNullToString(.Item("BIRTH_PLACE"))

                'nationality
                Me.mstMainInfo.strNationality = basCommon.fncCnvNullToString(.Item("NATIONALITY"))

                'religion
                Me.mstMainInfo.strReligion = basCommon.fncCnvNullToString(.Item("RELIGION"))

                'deceased
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED")), Me.mstMainInfo.intDeceased)

                'deceased date
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("DECEASED_DATE")), Me.mstMainInfo.dtDeceased)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY")), Me.mstMainInfo.stDeath.intDay)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON")), Me.mstMainInfo.stDeath.intMon)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA")), Me.mstMainInfo.stDeath.intYea)
                'mstDeaDate.intDay = Me.mstMainInfo.stDeath.intDay
                'mstDeaDate.intMon = Me.mstMainInfo.stDeath.intMon
                'mstDeaDate.intYea = Me.mstMainInfo.stDeath.intYea
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY_SUN")), Me.mstMainInfo.stDeathSun.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON_SUN")), Me.mstMainInfo.stDeathSun.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA_SUN")), Me.mstMainInfo.stDeathSun.intYear)

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_DAY")), Me.mstMainInfo.stDeathLunar.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_MON")), Me.mstMainInfo.stDeathLunar.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("DEA_YEA")), Me.mstMainInfo.stDeathLunar.intYear)

                mstDeaDateSun.intDay = Me.mstMainInfo.stDeathSun.intDay
                mstDeaDateSun.intMonth = Me.mstMainInfo.stDeathSun.intMonth
                mstDeaDateSun.intYear = Me.mstMainInfo.stDeathSun.intYear

                mstDeaDateMoon.intDay = Me.mstMainInfo.stDeathLunar.intDay
                mstDeaDateMoon.intMonth = Me.mstMainInfo.stDeathLunar.intMonth
                mstDeaDateMoon.intYear = Me.mstMainInfo.stDeathLunar.intYear

                'bury place
                Me.mstMainInfo.strBuryPlace = basCommon.fncCnvNullToString(.Item("BURY_PLACE"))

                'avatar path
                Me.mstMainInfo.strAvatar = basCommon.fncCnvNullToString(.Item("AVATAR_PATH"))

                'remark
                Me.mstMainInfo.strRemark = basCommon.fncCnvNullToString(.Item("T_FMEMBER_MAIN.REMARK"))

                'general information of career, education and fact 
                Me.mstMainInfo.strCareerGeneral = basCommon.fncCnvNullToString(.Item("CAREER"))
                Me.mstMainInfo.strEduGeneral = basCommon.fncCnvNullToString(.Item("EDUCATION"))
                Me.mstMainInfo.strFactGeneral = basCommon.fncCnvNullToString(.Item("FACT"))

                Me.mstMainInfo.intCareerType = CType(basCommon.fncCnvToInt(.Item("CAREER_TYPE")), clsEnum.emInputType)
                Me.mstMainInfo.intEduType = CType(basCommon.fncCnvToInt(.Item("EDUCATION_TYPE")), clsEnum.emInputType)
                Me.mstMainInfo.intFactType = CType(basCommon.fncCnvToInt(.Item("FACT_TYPE")), clsEnum.emInputType)

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetStrucMemberMain", ex)
        Finally
            If dtTable IsNot Nothing Then dtTable.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetCtrlDataMain, Get information from Controls 
    '                   in MAIN INFORMATION tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/07/29  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetCtrlDataMain() As Boolean

        xGetCtrlDataMain = False

        Try
            Dim strFileName As String

            mstMainInfo = Nothing

            With Me.mstMainInfo

                'get id
                .intID = Me.mintID

                'get gender
                If rdMale.Checked Then .intGender = clsEnum.emGender.MALE
                If rdFemale.Checked Then .intGender = clsEnum.emGender.FEMALE
                If rdUnknow.Checked Then .intGender = clsEnum.emGender.UNKNOW

                'get name and alias
                .strLastName = txtLastName.Text.Trim()
                .strMidName = txtMidName.Text.Trim()
                .strFirstName = txtFirstName.Text.Trim()
                .strAlias = txtAlias.Text.Trim()

                'try to get order in family
                If Not Integer.TryParse(txtFamilyOrder.Value.ToString(), .intFamilyOrder) Then
                    txtFamilyOrder.Focus()
                    Exit Function
                End If

                'birthday and birth place
                'If dtpBirthDay.Checked Then .dtBirth = dtpBirthDay.Value
                .strBirthPlace = txtBirthPlace.Text
                '.stBirth.intDay = mstBirDate.intDay
                '.stBirth.intMon = mstBirDate.intMon
                '.stBirth.intYea = mstBirDate.intYea

                .stBirthSun.intDay = mstBirDateSun.intDay
                .stBirthSun.intMonth = mstBirDateSun.intMonth
                .stBirthSun.intYear = mstBirDateSun.intYear

                .stBirthLunar.intDay = mstBirDateMoon.intDay
                .stBirthLunar.intMonth = mstBirDateMoon.intMonth
                .stBirthLunar.intYear = mstBirDateMoon.intYear

                'nationality (default is vietnamese - id 1) and religion (default is null)
                .strReligion = Nothing
                If cbNation.SelectedIndex <> -1 Then .strNationality = cbNation.SelectedValue.ToString()
                If cbReligion.SelectedIndex <> -1 Then .strReligion = cbReligion.SelectedValue.ToString()

                'avatar default is null
                .strAvatar = Nothing

                'set image file name
                strFileName = String.Format(basConst.gcstrImgFormat, .intID)

                If Not String.IsNullOrEmpty(mstrAvatar) Then

                    'try copying image file to "images" folder then set the path
                    Try
                        'basCommon.fncCopyFile(mstrAvatar, basConst.gcstrImageFolder & basConst.gcstrAvatarPath, strFileName, .strAvatar)
                        'basCommon.fncCreateThumbnailAndSave(mstrAvatar, basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath, strFileName, clsDefine.THUMBNAIL_W, clsDefine.THUMBNAIL_H)

                        basCommon.fncCreateThumbnailAndSave(picImage.Image, basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath, strFileName, clsDefine.THUMBNAIL_W, clsDefine.THUMBNAIL_H)
                        basCommon.fncSaveImage(picImage.Image, basConst.gcstrImageFolder & basConst.gcstrAvatarPath, strFileName, mstrAvatar)

                        .strAvatar = strFileName
                    Catch ex As Exception
                        basCommon.fncSaveErr(mcstrClsName, "xGetCtrlDataMain", ex, Nothing, False)
                    End Try

                Else
                    strFileName &= gcstrFileJPG
                    .strAvatar = strFileName
                End If
                Dim strAvatarPath As String = ""
                If mblnDelImg Then
                    'Delete avatar Image
                    strAvatarPath = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & mstMainInfo.strAvatar
                    basCommon.fncDeleteFile(strAvatarPath)
                    'Delete thumb Image
                    strAvatarPath = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarThumbPath & mstMainInfo.strAvatar
                    basCommon.fncDeleteFile(strAvatarPath)
                End If

                'by default, deceased is null
                .intDeceased = basConst.gcintALIVE
                '.dtDeceased = Nothing
                .strBuryPlace = Nothing

                'if died, get value
                If chkDie.Checked Then

                    .intDeceased = basConst.gcintDIED
                    'If dtpDieDay.Checked Then .dtDeceased = dtpDieDay.Value
                    '.stDeath.intDay = mstDeaDate.intDay
                    '.stDeath.intMon = mstDeaDate.intMon
                    '.stDeath.intYea = mstDeaDate.intYea
                    .stDeathSun.intDay = mstDeaDateSun.intDay
                    .stDeathSun.intMonth = mstDeaDateSun.intMonth
                    .stDeathSun.intYear = mstDeaDateSun.intYear

                    .stDeathLunar.intDay = mstDeaDateMoon.intDay
                    .stDeathLunar.intMonth = mstDeaDateMoon.intMonth
                    .stDeathLunar.intYear = mstDeaDateMoon.intYear
                    .strBuryPlace = txtBuryPlace.Text.Trim()

                    'If Not xIsValidDateRange(mstBirDate, mstDeaDate) Then
                    '    basCommon.fncMessageWarning(mcstrErrorDateBirthDecease, btnSelectCalBirth)
                    '    Return False
                    'End If

                End If

                'Remark - in REMARK tab
                '.strRemark = txtRemark.Text.Trim()
                If basCommon.fncIsBlank(txtRemark.Text.Trim()) Then .strRemark = "" Else _
                    If txtRemark.Rtf Is Nothing Then .strRemark = "" Else .strRemark = txtRemark.Rtf.Trim()

                .intCareerType = clsEnum.emInputType.GENERAL
                .intEduType = clsEnum.emInputType.GENERAL
                .intFactType = clsEnum.emInputType.GENERAL

                If rdCareerDetail.Checked Then .intCareerType = clsEnum.emInputType.DETAIL
                If rdEduDetail.Checked Then .intEduType = clsEnum.emInputType.DETAIL
                If rdFactDetail.Checked Then .intFactType = clsEnum.emInputType.DETAIL

                .strCareerGeneral = txtCareerGeneral.Text.Trim()
                .strEduGeneral = txtEduGeneral.Text.Trim()
                .strFactGeneral = txtFactGeneral.Text.Trim()

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetCtrlDataMain", ex)
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xFillNation , fill nationality combo box
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : objCombo    ComboBox
    '      PARAMS2    : intTable    Integer, table to fill
    '      MEMO       :  
    '      CREATE     : 2011/08/05  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xFillCombo(ByVal objCombo As ComboBox, ByVal emTable As clsEnum.emTable) As Boolean

        xFillCombo = False

        Dim dtTable As DataTable = Nothing

        Try
            'check null of combobox
            If objCombo Is Nothing Then Exit Function

            If emTable = clsEnum.emTable.M_NATIONALITY Then dtTable = gobjDB.fncGetNation()

            If emTable = clsEnum.emTable.M_RELIGION Then dtTable = gobjDB.fncGetReligion()

            'check null of data
            If dtTable Is Nothing Then Exit Function

            With objCombo

                .DataSource = dtTable
                .SelectedIndex = 0

                'set value for Nationality
                If emTable = clsEnum.emTable.M_NATIONALITY Then
                    .DisplayMember = mcstrNatName
                    .ValueMember = mcstrNatID

                End If

                'set value for Religion
                If emTable = clsEnum.emTable.M_RELIGION Then

                    .DisplayMember = mcstrRelName
                    .ValueMember = mcstrRelID

                End If

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCombo", ex)
        Finally
            'If dtTable IsNot Nothing Then dtTable.Dispose()
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xSetSelected 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : objComboBox ComboBox
    '      PARAMS2    : strValue String
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xSetSelected(ByVal objComboBox As ComboBox, ByVal strValue As String) As Boolean

        xSetSelected = False                'default return

        Try
            Dim strTemp As String               'temp string


            'search each item
            For i As Integer = 0 To objComboBox.Items.Count - 1

                'set index for combobox and get value
                objComboBox.SelectedIndex = i
                strTemp = objComboBox.SelectedValue.ToString()

                'if a value is match
                If String.Compare(strValue, strTemp) = 0 Then

                    'set selected index
                    objComboBox.SelectedIndex = i
                    xSetSelected = True
                    Exit Function

                End If

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSetSelected", ex)
        End Try

    End Function


#End Region


#Region "Contact"


    '   ******************************************************************
    '　　　FUNCTION   : xInserContact, Insert member's contact
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xInsertContact() As Boolean

        xInsertContact = False

        Try

            'get data from controls in CONTACT tab
            If Not xGetCtrlDataContact() Then Exit Function

            'update to database
            If Not gobjDB.fncInsertContact(Me.mstContact, False) Then Exit Function

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xInserContact", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xUpdateContact, Update contact information
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xUpdateContact() As Boolean

        xUpdateContact = False

        Try

            'get data from controls in MAIN INFORMATION tab
            If Not xGetCtrlDataContact() Then Exit Function

            'update to database
            If Not gobjDB.fncUpdateContact(Me.mstContact, False) Then Exit Function

            Return True


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xUpdateContact", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCtrlDataContact, fill data to controls
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCtrlDataContact() As Boolean

        xFillCtrlDataContact = False

        Try

            'get data to structure
            If Not gobjDB.fncGetStrucContact(Me.mintID, Me.mstContact) Then Exit Function

            'start filling
            With Me.mstContact

                'home town
                txtHometown.Text = .strHometown

                'home address
                txtAddress.Text = .strHomeAddr

                'phone 1
                txtPhone1.Text = .strPhone1

                'phone 2
                txtPhone2.Text = .strphone2

                'email 1
                txtMail1.Text = .strMail1

                'email 2
                txtMail2.Text = .strMail2

                'fax
                txtFax.Text = .strFax

                'url
                txtURL.Text = .strURL

                'IM
                txtIM.Text = .strIMNick

                'remark
                txtRemarkContact.Text = .strRemark

            End With

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xFillCtrlDataContact", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetCtrlDataContact, Get information from Controls 
    '                   in CONTACT tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetCtrlDataContact() As Boolean

        xGetCtrlDataContact = False

        Try

            'use structure to store data
            With mstContact

                'id
                .intID = Me.mintID

                'home town
                .strHometown = txtHometown.Text.Trim()

                'home address
                .strHomeAddr = txtAddress.Text.Trim()

                'phone 1
                .strPhone1 = txtPhone1.Text.Trim()

                'phone 2
                .strphone2 = txtPhone2.Text.Trim()

                'email 1
                .strMail1 = txtMail1.Text.Trim()

                'email 2
                .strMail2 = txtMail2.Text.Trim()

                'fax number
                .strFax = txtFax.Text.Trim()

                'URL
                .strURL = txtURL.Text.Trim()

                'IM nick
                .strIMNick = txtIM.Text.Trim()

                'remark
                .strRemark = txtRemarkContact.Text.Trim()

            End With

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xGetCtrlDataContact", ex)

        End Try

    End Function


#End Region


#Region "Career"


    '   ******************************************************************
    '　　　FUNCTION   : xSaveCareer, Insert member's career
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveCareer() As Boolean

        xSaveCareer = False
        'Dim dtTime(2) As Date
        Dim stTempStartDate As stCalendar
        Dim stTempEndDate As stCalendar

        Try
            'delete all record in database
            If Not gobjDB.fncDelCareer(clsEnum.emCareerType.CAREER, Me.mintID, False) Then Exit Function

            'if there's no row, do nothing
            If dgvCareer.Rows.Count < 0 Then Exit Function

            'reset struc
            Me.mstCareer = Nothing

            'loop for insert each row
            'For Each row As DataGridViewRow In dgvCareer.Rows
            For i As Integer = 0 To dgvCareer.Rows.Count - 1

                Dim row As DataGridViewRow = dgvCareer.Rows(i)

                With Me.mstCareer
                    'mem id
                    .intMemID = Me.mintID

                    'career id
                    .intCareerID = gobjDB.fncGetMaxID(clsEnum.emTable.T_FMEMBER_CAREER) + 1

                    'type
                    .intType = clsEnum.emCareerType.CAREER

                    'name
                    .strOffName = basCommon.fncCnvNullToString(row.Cells(0).Value)

                    'place
                    .strOffPlace = basCommon.fncCnvNullToString(row.Cells(1).Value)

                    'position
                    .strPosition = basCommon.fncCnvNullToString(row.Cells(2).Value)

                    'occupt
                    .strOccupt = basCommon.fncCnvNullToString(row.Cells(3).Value)

                    'get date
                    'dtTime = xGetDateFromString(basCommon.fncCnvNullToString(row.Cells(4).Value))
                    '.dtStart = dtTime(0)
                    '.dtEnd = dtTime(1)
                    xGetDateFromString(basCommon.fncCnvNullToString(row.Cells("clmTempTimeCareer").Value), stTempStartDate, stTempEndDate)
                    .intSday = stTempStartDate.intDay
                    .intSmon = stTempStartDate.intMonth
                    .intSyea = stTempStartDate.intYear

                    .intEday = stTempEndDate.intDay
                    .intEmon = stTempEndDate.intMonth
                    .intEyea = stTempEndDate.intYear

                End With

                'update to database
                If Not gobjDB.fncInsertCareer(Me.mstCareer, False) Then Exit Function

            Next


            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveCareer", ex)

        Finally
            'Erase dtTime

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddCareer2Grid, add data to gridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddCareer2Grid() As Boolean

        xAddCareer2Grid = False

        'row to add
        Dim row(5) As String

        Try
            Dim strStart As String = String.Empty
            Dim strEnd As String = String.Empty


            'read data from control
            If Not xGetCtrlDataCareer() Then Exit Function

            With Me.mstCareer

                'name
                row(0) = .strOffName

                'address
                row(1) = .strOffPlace

                'position
                row(2) = .strPosition

                'occupation
                row(3) = .strOccupt

                'format start and end date
                'If .dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, .dtStart)
                'If .dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, .dtEnd)

                strStart = basCommon.fncGetDateName("", mstSdateCareer, True, False)
                strEnd = basCommon.fncGetDateName("", mstEdateCareer, True, False)

                'time
                row(4) = strStart & " - " & strEnd
                'If .dtStart = Date.MinValue And .dtEnd = Date.MinValue Then row(4) = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then row(4) = ""

                'temptime column
                row(5) = basCommon.fncGetDateDDMMYYYY(mstSdateCareer.intDay, mstSdateCareer.intMonth, mstSdateCareer.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(mstEdateCareer.intDay, mstEdateCareer.intMonth, mstEdateCareer.intYear)

            End With

            'add to grid
            'dgvCareer.Rows.Insert(0, row)
            dgvCareer.Rows.Add(row)

            'clear temp value
            mstSdateCareer = Nothing
            mstEdateCareer = Nothing
            lblStartCareer.Text = basConst.gcstrDateUnknown
            lblEndCareer.Text = basConst.gcstrDateUnknown

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddCareer2Grid", ex)

        Finally
            Erase row

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSaveCareer2Grid, save data to gridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRowIndext Integer, index of row to update
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveCareer2Grid(ByVal intRowIndext As Integer) As Boolean

        xSaveCareer2Grid = False

        Try
            Dim strStart As String = String.Empty
            Dim strEnd As String = String.Empty


            'read data from control
            If Not xGetCtrlDataCareer() Then Exit Function

            With dgvCareer.Rows(intRowIndext)
                'name
                .Cells(0).Value = mstCareer.strOffName

                'place
                .Cells(1).Value = mstCareer.strOffPlace

                'position
                .Cells(2).Value = mstCareer.strPosition

                'occupation
                .Cells(3).Value = mstCareer.strOccupt

                'format start and end date
                'If mstCareer.dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, mstCareer.dtStart)
                'If mstCareer.dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, mstCareer.dtEnd)

                strStart = basCommon.fncGetDateName("", mstSdateCareer, True, False)
                strEnd = basCommon.fncGetDateName("", mstEdateCareer, True, False)

                .Cells(4).Value = strStart & " - " & strEnd
                'If mstCareer.dtStart = Date.MinValue And mstCareer.dtEnd = Date.MinValue Then .Cells(4).Value = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then .Cells(4).Value = ""

                .Cells(5).Value = basCommon.fncGetDateDDMMYYYY(mstSdateCareer.intDay, mstSdateCareer.intMonth, mstSdateCareer.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(mstEdateCareer.intDay, mstEdateCareer.intMonth, mstEdateCareer.intYear)

            End With

            mstSdateCareer = Nothing
            mstEdateCareer = Nothing
            lblStartCareer.Text = basConst.gcstrDateUnknown
            lblEndCareer.Text = basConst.gcstrDateUnknown

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveCareer2Grid", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetCtrlDataCareer, Get information from Controls 
    '                   in CAREER tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetCtrlDataCareer() As Boolean

        xGetCtrlDataCareer = False

        Try
            'reset structure
            Me.mstCareer = Nothing

            'use structure to store data
            With Me.mstCareer

                'office name
                .strOffName = txtOffName.Text.Trim()

                'office address
                .strOffPlace = txtOffAddr.Text.Trim()

                'position
                .strPosition = txtPosition.Text.Trim()

                'occupation
                .strOccupt = txtOccupt.Text.Trim()

                'start date
                'If dtpStartCareer.Checked Then .dtStart = dtpStartCareer.Value

                'end date
                'If dtpEndCareer.Checked Then .dtEnd = dtpEndCareer.Value

                'check validation of date
                If Not xIsValidDateRange(mstSdateCareer, mstEdateCareer) Then
                    basCommon.fncMessageWarning(mcstrErrorDate, btnStartCareer)
                    Exit Function
                End If

            End With

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xGetCtrlDataCareer", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCtrlDataCareer, fill data to gridview 
    '                   in CAREER tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCtrlDataCareer() As Boolean

        xFillCtrlDataCareer = False

        Dim dtTable As DataTable = Nothing

        Try
            Dim strTime As String
            Dim strTempTime As String

            Dim strStart As String
            Dim strEnd As String

            'Dim dtStart As Date
            'Dim dtEnd As Date

            Dim stStart As stCalendar
            Dim stEnd As stCalendar

            'get data from database
            dtTable = gobjDB.fncGetCareer(clsEnum.emCareerType.CAREER, Me.mintID)

            'check null
            If dtTable Is Nothing Then Return True

            'fill to gird
            'For Each row As DataRow In dtTable.Rows
            For i As Integer = 0 To dtTable.Rows.Count - 1

                Dim row As DataRow = dtTable.Rows(i)

                'reset variable
                stStart = Nothing
                stEnd = Nothing
                strStart = String.Empty
                strEnd = String.Empty

                'get start and end date
                'Date.TryParse(basCommon.fncCnvNullToString(row("START_DATE")), dtStart)
                'Date.TryParse(basCommon.fncCnvNullToString(row("END_DATE")), dtEnd)

                Integer.TryParse(basCommon.fncCnvNullToString(row("START_DAY")), stStart.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(row("START_MON")), stStart.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(row("START_YEA")), stStart.intYear)

                Integer.TryParse(basCommon.fncCnvNullToString(row("END_DAY")), stEnd.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(row("END_MON")), stEnd.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(row("END_YEA")), stEnd.intYear)

                'format start and end date
                'If dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, dtStart)
                'If dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, dtEnd)

                strStart = basCommon.fncGetDateName("", stStart, True, False)
                strEnd = basCommon.fncGetDateName("", stEnd, True, False)

                'build string
                strTime = strStart & " - " & strEnd
                'If dtStart = Date.MinValue And dtEnd = Date.MinValue Then strTime = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then strTime = ""

                strTempTime = basCommon.fncGetDateDDMMYYYY(stStart.intDay, stStart.intMonth, stStart.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(stEnd.intDay, stEnd.intMonth, stEnd.intYear)

                'add to grid
                dgvCareer.Rows.Add(row("OFFICE_NAME"),
                                    row("OFFICE_PLACE"),
                                    row("POSITION"),
                                    row("OCCUPATION"),
                                    strTime,
                                    strTempTime)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCtrlDataCareer", ex)
        Finally
            If dtTable IsNot Nothing Then dtTable.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCareerFromCell, fill data from cell
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : e   DataGridViewCellEventArgs
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCareerFromCell(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean

        xFillCareerFromCell = False

        Dim dtDate(2) As Date

        Try
            Dim intRowIndex As Integer

            'get current index
            intRowIndex = e.RowIndex

            'if there's no row selected
            If intRowIndex < 0 Then Exit Function

            'set mode to EDIT
            Me.mintCareerMode = clsEnum.emMode.EDIT

            'disable datetime picker
            'dtpStartCareer.Checked = False
            'dtpEndCareer.Checked = False

            'set value to struc -> text box
            With Me.mstCareer
                'name
                .strOffName = basCommon.fncCnvNullToString(dgvCareer.Item(0, intRowIndex).Value)
                txtOffName.Text = .strOffName

                'place
                .strOffPlace = basCommon.fncCnvNullToString(dgvCareer.Item(1, intRowIndex).Value)
                txtOffAddr.Text = .strOffPlace

                'position
                .strPosition = basCommon.fncCnvNullToString(dgvCareer.Item(2, intRowIndex).Value)
                txtPosition.Text = .strPosition

                'occupation
                .strOccupt = basCommon.fncCnvNullToString(dgvCareer.Item(3, intRowIndex).Value)
                txtOccupt.Text = .strOccupt

                'time
                xGetDateFromString(basCommon.fncCnvNullToString(dgvCareer.Item("clmTempTimeCareer", intRowIndex).Value), mstSdateCareer, mstEdateCareer)
                lblStartCareer.Text = basCommon.fncGetDateName("", mstSdateCareer, True, False)
                lblEndCareer.Text = basCommon.fncGetDateName("", mstEdateCareer, True, False)

                'dtDate = xGetDateFromString(basCommon.fncCnvNullToString(dgvCareer.Item(4, intRowIndex).Value))
                '.dtStart = dtDate(0)
                '.dtEnd = dtDate(1)

                ''set start date
                'If .dtStart > Date.MinValue Then

                '    dtpStartCareer.Value = .dtStart
                '    dtpStartCareer.Checked = True

                'End If

                ''set end date
                'If .dtEnd > Date.MinValue Then

                '    dtpEndCareer.Value = .dtEnd
                '    dtpEndCareer.Checked = True

                'End If

            End With

            Return True


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCareerFromCell", ex)

        Finally
            Erase dtDate

        End Try

    End Function

#End Region


#Region "Education"


    '   ******************************************************************
    '　　　FUNCTION   : xSaveEdu, Insert member's edu
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveEdu() As Boolean

        xSaveEdu = False
        'Dim dtTime(2) As Date
        Dim stTempStartDate As stCalendar
        Dim stTempEndDate As stCalendar

        Try
            'delete all record in database
            If Not gobjDB.fncDelCareer(clsEnum.emCareerType.EDU, Me.mintID, False) Then Exit Function

            'if there's no row, do nothing
            If dgvEdu.Rows.Count < 0 Then Exit Function

            'reset struc
            Me.mstEdu = Nothing

            'loop for insert each row
            'For Each row As DataGridViewRow In dgvEdu.Rows
            For i As Integer = 0 To dgvEdu.Rows.Count - 1

                Dim row As DataGridViewRow = dgvEdu.Rows(i)

                With Me.mstEdu
                    'mem id
                    .intMemID = Me.mintID

                    'career id
                    .intCareerID = gobjDB.fncGetMaxID(clsEnum.emTable.T_FMEMBER_CAREER) + 1

                    'type
                    .intType = clsEnum.emCareerType.EDU

                    'name
                    .strOffName = basCommon.fncCnvNullToString(row.Cells(0).Value)

                    'get date
                    xGetDateFromString(basCommon.fncCnvNullToString(row.Cells("clmTempTimeEdu").Value), stTempStartDate, stTempEndDate)
                    .intSday = stTempStartDate.intDay
                    .intSmon = stTempStartDate.intMonth
                    .intSyea = stTempStartDate.intYear

                    .intEday = stTempEndDate.intDay
                    .intEmon = stTempEndDate.intMonth
                    .intEyea = stTempEndDate.intYear

                    'dtTime = xGetDateFromString(basCommon.fncCnvNullToString(row.Cells(1).Value))
                    '.dtStart = dtTime(0)
                    '.dtEnd = dtTime(1)

                    'remark
                    .strRemark = basCommon.fncCnvNullToString(row.Cells(2).Value)

                End With

                'update to database
                If Not gobjDB.fncInsertCareer(Me.mstEdu, False) Then Exit Function

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveEdu", ex)

        Finally
            'Erase dtTime

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddEdu2Grid, add data to gridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/02  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddEdu2Grid() As Boolean

        xAddEdu2Grid = False

        'row to add
        Dim row(3) As String

        Try
            Dim strStart As String = String.Empty
            Dim strEnd As String = String.Empty


            'read data from control
            If Not xGetCtrlDataEdu() Then Exit Function

            With Me.mstEdu

                'name
                row(0) = .strOffName

                'format start and end date
                'If .dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, .dtStart)
                'If .dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, .dtEnd)

                strStart = basCommon.fncGetDateName("", mstSdateEdu, True, False)
                strEnd = basCommon.fncGetDateName("", mstEdateEdu, True, False)

                'time
                row(1) = strStart & " - " & strEnd
                'If .dtStart = Date.MinValue And .dtEnd = Date.MinValue Then row(1) = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then row(1) = ""

                'address
                row(2) = .strRemark

                row(3) = basCommon.fncGetDateDDMMYYYY(mstSdateEdu.intDay, mstSdateEdu.intMonth, mstSdateEdu.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(mstEdateEdu.intDay, mstEdateEdu.intMonth, mstEdateEdu.intYear)

            End With

            'add to grid
            'dgvEdu.Rows.Insert(0, row)
            dgvEdu.Rows.Add(row)

            mstSdateEdu = Nothing
            mstEdateEdu = Nothing
            lblStartEdu.Text = basConst.gcstrDateUnknown
            lblEndEdu.Text = basConst.gcstrDateUnknown


            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddEdu2Grid", ex)

        Finally
            Erase row

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSaveEdu2Grid, save data to gridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRowIndext Integer, index of row to update
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveEdu2Grid(ByVal intRowIndext As Integer) As Boolean

        xSaveEdu2Grid = False


        Try
            Dim strStart As String = String.Empty
            Dim strEnd As String = String.Empty


            'read data from control
            If Not xGetCtrlDataEdu() Then Exit Function

            With dgvEdu.Rows(intRowIndext)
                'name
                .Cells(0).Value = mstEdu.strOffName

                'format start and end date
                'If mstEdu.dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, mstEdu.dtStart)
                'If mstEdu.dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, mstEdu.dtEnd)

                strStart = basCommon.fncGetDateName("", mstSdateEdu, True, False)
                strEnd = basCommon.fncGetDateName("", mstEdateEdu, True, False)

                .Cells(1).Value = strStart & " - " & strEnd
                'If mstEdu.dtStart = Date.MinValue And mstEdu.dtEnd = Date.MinValue Then .Cells(1).Value = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then .Cells(1).Value = ""

                'remark
                .Cells(2).Value = mstEdu.strRemark

                .Cells(3).Value = basCommon.fncGetDateDDMMYYYY(mstSdateEdu.intDay, mstSdateEdu.intMonth, mstSdateEdu.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(mstEdateEdu.intDay, mstEdateEdu.intMonth, mstEdateEdu.intYear)

            End With

            mstSdateEdu = Nothing
            mstEdateEdu = Nothing
            lblStartEdu.Text = basConst.gcstrDateUnknown
            lblEndEdu.Text = basConst.gcstrDateUnknown

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveEdu2Grid", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetCtrlDataEdu, Get information from Controls 
    '                   in EDU tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetCtrlDataEdu() As Boolean

        xGetCtrlDataEdu = False


        Try
            'reset structure
            Me.mstEdu = Nothing

            'use structure to store data
            With Me.mstEdu

                'school name
                .strOffName = txtSchoolName.Text.Trim()

                'remark
                .strRemark = txtRemarkEdu.Text.Trim()

                'start date
                'If dtpStartEdu.Checked Then .dtStart = dtpStartEdu.Value

                'end date
                'If dtpEndEdu.Checked Then .dtEnd = dtpEndEdu.Value

                'check validation of date
                If Not xIsValidDateRange(mstSdateEdu, mstEdateEdu) Then
                    basCommon.fncMessageWarning(mcstrErrorDate, btnStartEdu)
                    Exit Function
                End If

            End With

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xGetCtrlDataEdu", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCtrlDataEdu, fill data to gridview 
    '                   in EDU tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCtrlDataEdu() As Boolean

        xFillCtrlDataEdu = False

        Dim dtTable As DataTable = Nothing

        Try

            Dim strTime As String
            Dim strTempTime As String

            Dim strStart As String
            Dim strEnd As String

            'Dim dtStart As Date
            'Dim dtEnd As Date

            Dim stStart As stCalendar
            Dim stEnd As stCalendar

            'get data from database
            dtTable = gobjDB.fncGetCareer(clsEnum.emCareerType.EDU, Me.mintID)

            'check null
            If dtTable Is Nothing Then Return True

            'fill to gird
            'For Each row As DataRow In dtTable.Rows
            For i As Integer = 0 To dtTable.Rows.Count - 1

                Dim row As DataRow = dtTable.Rows(i)

                'reset variable
                stStart = Nothing
                stEnd = Nothing
                strStart = String.Empty
                strEnd = String.Empty

                'get start and end date
                'Date.TryParse(basCommon.fncCnvNullToString(row("START_DATE")), dtStart)
                'Date.TryParse(basCommon.fncCnvNullToString(row("END_DATE")), dtEnd)

                Integer.TryParse(basCommon.fncCnvNullToString(row("START_DAY")), stStart.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(row("START_MON")), stStart.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(row("START_YEA")), stStart.intYear)

                Integer.TryParse(basCommon.fncCnvNullToString(row("END_DAY")), stEnd.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(row("END_MON")), stEnd.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(row("END_YEA")), stEnd.intYear)

                'format start and end date
                'If dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, dtStart)
                'If dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, dtEnd)

                strStart = basCommon.fncGetDateName("", stStart, True, False)
                strEnd = basCommon.fncGetDateName("", stEnd, True, False)

                'build string
                strTime = strStart & " - " & strEnd
                'If dtStart = Date.MinValue And dtEnd = Date.MinValue Then strTime = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then strTime = ""

                strTempTime = basCommon.fncGetDateDDMMYYYY(stStart.intDay, stStart.intMonth, stStart.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(stEnd.intDay, stEnd.intMonth, stEnd.intYear)

                'add to grid
                dgvEdu.Rows.Add(row("OFFICE_NAME"),
                                    strTime,
                                    row("REMARK"),
                                    strTempTime)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCtrlDataEdu", ex)
        Finally
            If dtTable IsNot Nothing Then dtTable.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillEduFromCell, fill data from cell
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : e   DataGridViewCellEventArgs
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillEduFromCell(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean

        xFillEduFromCell = True

        Dim dtDate(2) As Date

        Try
            Dim intRowIndex As Integer

            'get current index
            intRowIndex = e.RowIndex

            'if there's no row selected
            If intRowIndex < 0 Then Exit Function

            'set mode to EDIT
            Me.mintEduMode = clsEnum.emMode.EDIT

            'disable datetime picker
            'dtpStartEdu.Checked = False
            'dtpEndEdu.Checked = False

            'set value to struc -> text box
            With Me.mstEdu
                'name
                .strOffName = basCommon.fncCnvNullToString(dgvEdu.Item(0, intRowIndex).Value)
                txtSchoolName.Text = .strOffName

                'time
                xGetDateFromString(basCommon.fncCnvNullToString(dgvEdu.Item("clmTempTimeEdu", intRowIndex).Value), mstSdateEdu, mstEdateEdu)
                lblStartEdu.Text = basCommon.fncGetDateName("", mstSdateEdu, True, False)
                lblEndEdu.Text = basCommon.fncGetDateName("", mstEdateEdu, True, False)

                'dtDate = xGetDateFromString(basCommon.fncCnvNullToString(dgvEdu.Item(1, intRowIndex).Value))
                '.dtStart = dtDate(0)
                '.dtEnd = dtDate(1)

                ''set start date
                'If .dtStart > Date.MinValue Then

                '    dtpStartEdu.Value = .dtStart
                '    dtpStartEdu.Checked = True

                'End If

                ''set end date
                'If .dtEnd > Date.MinValue Then

                '    dtpEndEdu.Value = .dtEnd
                '    dtpEndEdu.Checked = True

                'End If

                'remark
                .strRemark = basCommon.fncCnvNullToString(dgvEdu.Item(2, intRowIndex).Value)
                txtRemarkEdu.Text = .strRemark

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillEduFromCell", ex)

        Finally
            Erase dtDate

        End Try

    End Function

#End Region


#Region "Fact"


    '   ******************************************************************
    '　　　FUNCTION   : xSaveFact, Insert member's fact
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveFact() As Boolean

        xSaveFact = False
        'Dim dtTime(2) As Date
        Dim stTempStartDate As stCalendar
        Dim stTempEndDate As stCalendar

        Try
            'delete all record in database
            If Not gobjDB.fncDelFact(Me.mintID, False) Then Exit Function

            'if there's no row, do nothing
            If dgvFact.Rows.Count < 0 Then Exit Function

            'reset struc
            Me.mstFact = Nothing

            'loop for insert each row
            'For Each row As DataGridViewRow In dgvFact.Rows
            For i As Integer = 0 To dgvFact.Rows.Count - 1

                Dim row As DataGridViewRow = dgvFact.Rows(i)

                With Me.mstFact
                    'mem id
                    .intMemID = Me.mintID

                    'fact id
                    .intFactID = gobjDB.fncGetMaxID(clsEnum.emTable.T_FMEMBER_FACT) + 1

                    'name
                    .strName = basCommon.fncCnvNullToString(row.Cells(0).Value)

                    'place
                    .strPlace = basCommon.fncCnvNullToString(row.Cells(1).Value)

                    'get date
                    'dtTime = xGetDateFromString(basCommon.fncCnvNullToString(row.Cells(2).Value))
                    '.dtStart = dtTime(0)
                    '.dtEnd = dtTime(1)
                    xGetDateFromString(basCommon.fncCnvNullToString(row.Cells("clmTempTimeFact").Value), stTempStartDate, stTempEndDate)
                    .intSday = stTempStartDate.intDay
                    .intSmon = stTempStartDate.intMonth
                    .intSyea = stTempStartDate.intYear

                    .intEday = stTempEndDate.intDay
                    .intEmon = stTempEndDate.intMonth
                    .intEyea = stTempEndDate.intYear

                    'description
                    .strDesc = basCommon.fncCnvNullToString(row.Cells(3).Value)

                End With

                'update to database
                If Not gobjDB.fncInsertFact(Me.mstFact, False) Then Exit Function

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveFact", ex)

        Finally
            'Erase dtTime

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddFact2Grid, add data to gridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddFact2Grid() As Boolean

        xAddFact2Grid = False

        'row to add
        Dim row(4) As String

        Try
            Dim strStart As String = String.Empty
            Dim strEnd As String = String.Empty


            'read data from control
            If Not xGetCtrlDataFact() Then Exit Function

            With Me.mstFact

                'name
                row(0) = .strName

                'place
                row(1) = .strPlace

                'format start and end date
                'If .dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, .dtStart)
                'If .dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, .dtEnd)

                strStart = basCommon.fncGetDateName("", mstSdateFact, True, False)
                strEnd = basCommon.fncGetDateName("", mstEdateFact, True, False)

                'time
                row(2) = strStart & " - " & strEnd
                'If .dtStart = Date.MinValue And .dtEnd = Date.MinValue Then row(2) = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then row(2) = ""

                'description
                row(3) = .strDesc

                'temptime column
                row(4) = basCommon.fncGetDateDDMMYYYY(mstSdateFact.intDay, mstSdateFact.intMonth, mstSdateFact.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(mstEdateFact.intDay, mstEdateFact.intMonth, mstEdateFact.intYear)

            End With

            'add to grid
            'dgvFact.Rows.Insert(0, row)
            dgvFact.Rows.Add(row)

            mstSdateFact = Nothing
            mstEdateFact = Nothing
            lblStartFact.Text = basConst.gcstrDateUnknown
            lblEndFact.Text = basConst.gcstrDateUnknown

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddFact2Grid", ex)

        Finally
            Erase row

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xSaveFact2Grid, save data to gridview
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : intRowIndext Integer, index of row to update
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSaveFact2Grid(ByVal intRowIndext As Integer) As Boolean

        xSaveFact2Grid = False

        Try
            Dim strStart As String = String.Empty
            Dim strEnd As String = String.Empty


            'read data from control
            If Not xGetCtrlDataFact() Then Exit Function

            With dgvFact.Rows(intRowIndext)
                'name
                .Cells(0).Value = mstFact.strName

                'place
                .Cells(1).Value = mstFact.strPlace

                'format start and end date
                'If mstFact.dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, mstFact.dtStart)
                'If mstFact.dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, mstFact.dtEnd)

                strStart = basCommon.fncGetDateName("", mstSdateFact, True, False)
                strEnd = basCommon.fncGetDateName("", mstEdateFact, True, False)

                .Cells(2).Value = strStart & " - " & strEnd
                'If mstFact.dtStart = Date.MinValue And mstFact.dtEnd = Date.MinValue Then .Cells(2).Value = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then .Cells(2).Value = ""

                'description
                .Cells(3).Value = mstFact.strDesc

                .Cells(4).Value = basCommon.fncGetDateDDMMYYYY(mstSdateFact.intDay, mstSdateFact.intMonth, mstSdateFact.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(mstEdateFact.intDay, mstEdateFact.intMonth, mstEdateFact.intYear)

            End With

            mstSdateFact = Nothing
            mstEdateFact = Nothing
            lblStartFact.Text = basConst.gcstrDateUnknown
            lblEndFact.Text = basConst.gcstrDateUnknown

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSaveFact2Grid", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetCtrlDataFact, Get information from Controls 
    '                   in FACT tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetCtrlDataFact() As Boolean

        xGetCtrlDataFact = False

        Try
            'reset structure
            Me.mstFact = Nothing


            'use structure to store data
            With Me.mstFact

                'school name
                .strName = txtFactName.Text.Trim()

                'place
                .strPlace = txtFactPlace.Text.Trim()

                'description
                .strDesc = txtFactDesc.Text.Trim()

                'start date
                'If dtpStartFact.Checked Then .dtStart = dtpStartFact.Value

                'end date
                'If dtpEndFact.Checked Then .dtEnd = dtpEndFact.Value

                'check validation of date
                If Not xIsValidDateRange(mstSdateFact, mstEdateFact) Then
                    basCommon.fncMessageWarning(mcstrErrorDate, btnStartFact)
                    Exit Function
                End If

            End With

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xGetCtrlDataFact", ex)

        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillCtrlDataFact, fill data to gridview 
    '                   in FACT tab
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/04  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillCtrlDataFact() As Boolean

        xFillCtrlDataFact = False

        Dim dtTable As DataTable = Nothing

        Try
            Dim strTime As String
            Dim strTempTime As String

            Dim strStart As String
            Dim strEnd As String

            'Dim dtStart As Date
            'Dim dtEnd As Date

            Dim stStart As stCalendar
            Dim stEnd As stCalendar

            'get data from database
            dtTable = gobjDB.fncGetFact(Me.mintID)

            'check null
            If dtTable Is Nothing Then Return True

            'fill to gird
            'For Each row As DataRow In dtTable.Rows
            For i As Integer = 0 To dtTable.Rows.Count - 1

                Dim row As DataRow = dtTable.Rows(i)

                'reset variable
                stStart = Nothing
                stEnd = Nothing
                strStart = String.Empty
                strEnd = String.Empty

                'get start and end date
                'Date.TryParse(basCommon.fncCnvNullToString(row("START_DATE")), dtStart)
                'Date.TryParse(basCommon.fncCnvNullToString(row("END_DATE")), dtEnd)

                Integer.TryParse(basCommon.fncCnvNullToString(row("START_DAY")), stStart.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(row("START_MON")), stStart.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(row("START_YEA")), stStart.intYear)

                Integer.TryParse(basCommon.fncCnvNullToString(row("END_DAY")), stEnd.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(row("END_MON")), stEnd.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(row("END_YEA")), stEnd.intYear)

                'format start and end date
                'If dtStart > Date.MinValue Then strStart = String.Format(basConst.gcstrDateFormat2, dtStart)
                'If dtEnd > Date.MinValue Then strEnd = String.Format(basConst.gcstrDateFormat2, dtEnd)

                strStart = basCommon.fncGetDateName("", stStart, True, False)
                strEnd = basCommon.fncGetDateName("", stEnd, True, False)

                'build string
                strTime = strStart & " - " & strEnd
                'If dtStart = Date.MinValue And dtEnd = Date.MinValue Then strTime = ""
                If basCommon.fncIsBlank(strStart) And basCommon.fncIsBlank(strEnd) Then strTime = ""

                strTempTime = basCommon.fncGetDateDDMMYYYY(stStart.intDay, stStart.intMonth, stStart.intYear) & " - " & basCommon.fncGetDateDDMMYYYY(stEnd.intDay, stEnd.intMonth, stEnd.intYear)

                'add to grid
                dgvFact.Rows.Add(row("FACT_NAME"), _
                                    row("FACT_PLACE"), _
                                    strTime, _
                                    row("DESCRIPTION"), _
                                    strTempTime)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillCtrlDataFact", ex)
        Finally
            If dtTable IsNot Nothing Then dtTable.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xFillFactFromCell, fill data from cell
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : e   DataGridViewCellEventArgs
    '      MEMO       : 
    '      CREATE     : 2011/08/08  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillFactFromCell(ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) As Boolean

        xFillFactFromCell = False

        Dim dtDate(2) As Date

        Try
            Dim intRowIndex As Integer

            'get current index
            intRowIndex = e.RowIndex

            'if there's no row selected
            If intRowIndex < 0 Then Exit Function

            'set mode to EDIT
            Me.mintFactMode = clsEnum.emMode.EDIT

            'disable datetime picker
            'dtpStartFact.Checked = False
            'dtpEndFact.Checked = False

            'set value to struc -> text box
            With Me.mstFact
                'name
                .strName = basCommon.fncCnvNullToString(dgvFact.Item(0, intRowIndex).Value)
                txtFactName.Text = .strName

                'place
                .strPlace = basCommon.fncCnvNullToString(dgvFact.Item(1, intRowIndex).Value)
                txtFactPlace.Text = .strPlace

                'time
                xGetDateFromString(basCommon.fncCnvNullToString(dgvFact.Item("clmTempTimeFact", intRowIndex).Value), mstSdateFact, mstEdateFact)
                lblStartFact.Text = basCommon.fncGetDateName("", mstSdateFact, True, False)
                lblEndFact.Text = basCommon.fncGetDateName("", mstEdateFact, True, False)

                'dtDate = xGetDateFromString(basCommon.fncCnvNullToString(dgvFact.Item(2, intRowIndex).Value))
                '.dtStart = dtDate(0)
                '.dtEnd = dtDate(1)

                ''set start date
                'If .dtStart > Date.MinValue Then

                '    dtpStartFact.Value = .dtStart
                '    dtpStartFact.Checked = True

                'End If

                ''set end date
                'If .dtEnd > Date.MinValue Then

                '    dtpEndFact.Value = .dtEnd
                '    dtpEndFact.Checked = True

                'End If

                'description
                .strDesc = basCommon.fncCnvNullToString(dgvFact.Item(3, intRowIndex).Value)
                txtFactDesc.Text = .strDesc

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillFactFromCell", ex)

        Finally
            Erase dtDate

        End Try


    End Function

#End Region


#Region "Relationship"


    '   ******************************************************************
    '　　　FUNCTION   : xFillRelation, fill data to Relation datagrid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xFillRelation() As Boolean

        xFillRelation = False

        Try
            xAddParent()

            xAddHusWife()

            xAddBros()

            xAddKids()

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xFillRelation", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddParent, fill data to Relation datagrid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddParent() As Boolean

        xAddParent = False

        Dim tblData As DataTable = Nothing

        Try
            Dim strRelName As String = String.Empty                     'relationship name

            tblData = gobjDB.fncGetParent(mintID)

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1
                'get data then fill to struc
                xGetStRel(tblData, i)

                With mstRel

                    'BLOOD RELATION
                    If .intRelID = CInt(clsEnum.emRelation.NATURAL) Then
                        'FATHER
                        If .intGender = clsEnum.emGender.MALE Then
                            'get father id and set relation ship name
                            mintFather = .intMemId
                            strRelName = gcstrFather

                        End If

                        'MOTHER
                        If .intGender = clsEnum.emGender.FEMALE Then
                            'get mother id and set relation ship name
                            mintMother = .intMemId
                            strRelName = gcstrMother
                        End If

                    End If

                    'Adoptive Parent
                    If .intRelID = CInt(clsEnum.emRelation.ADOPT) Then
                        'FATHER
                        If .intGender = clsEnum.emGender.MALE Then
                            'get father id and set relation ship name
                            mintFather = .intMemId
                            strRelName = gcstrFather & " " & gcstrAdopt
                        End If

                        'MOTHER
                        If .intGender = clsEnum.emGender.FEMALE Then
                            'get mother id and set relation ship name
                            mintMother = .intMemId
                            strRelName = gcstrMother & " " & gcstrAdopt
                        End If

                    End If

                End With

                'add to grid
                xAdd2RelGrid(mstRel, strRelName)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddParent", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddHusWife, fill data to Relation datagrid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddHusWife() As Boolean

        xAddHusWife = False

        Dim tblData As DataTable = Nothing

        Try
            Dim strRelName As String = String.Empty                 'relationship name

            'read from database
            tblData = gobjDB.fncGetHusWife(mintID)

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1
                'get data then fill to struc
                xGetStRel(tblData, i)

                With mstRel

                    'set relation ship name
                    strRelName = gcstrWife

                    If .intGender = clsEnum.emGender.MALE Then strRelName = gcstrHusband

                End With

                'add to grid
                xAdd2RelGrid(mstRel, strRelName)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddHusWife", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddBros, fill data to Relation datagrid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddBros() As Boolean

        xAddBros = False

        Dim tblData As DataTable = Nothing

        Try
            Dim strRelName As String = String.Empty                     'relationship name

            'tblData = gobjDB.fncGetKids(mintFather, mintMother)
            tblData = basCommon.fncGetKids(mintFather, mintMother)

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1
                'get data then fill to struc
                xGetStRel(tblData, i, True)

                With mstRel

                    'detect query itself
                    If .intMemId = mintID Then Continue For

                    'If .dtBirth <= mstMainInfo.dtBirth Then
                    'If .intByea <= mstMainInfo.stBirth.intYea Then
                    'Start 2012/11/08 Manh Decise the realation between brothers or sisters by using Family Order (Child Order)
                    If .intFamilyOrder <= mstMainInfo.intFamilyOrder Then
                        'End 2012/11/08 Manh

                        'set relation ship name
                        strRelName = gcstrBrother & " " & gcstrBoy
                        If .intGender = clsEnum.emGender.FEMALE Then strRelName = gcstrSister & " " & gcstrGirl

                    Else

                        'set relation ship name
                        strRelName = gcstrYounger & " " & gcstrBoy
                        If .intGender = clsEnum.emGender.FEMALE Then strRelName = gcstrYounger & " " & gcstrGirl

                    End If

                End With

                'add to grid
                xAdd2RelGrid(mstRel, strRelName)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddBros", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAddKids, fill data to Relation datagrid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : none
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAddKids() As Boolean

        xAddKids = False

        Dim tblData As DataTable = Nothing

        Try
            Dim strRelName As String = String.Empty                     'relationship name

            'tblData = gobjDB.fncGetKids(mintID)
            tblData = basCommon.fncGetKids(mintID)

            If tblData Is Nothing Then Exit Function

            For i As Integer = 0 To tblData.Rows.Count - 1
                'get data then fill to struc
                xGetStRel(tblData, i, True)

                With mstRel

                    'set relation ship name
                    strRelName = gcstrKid & " " & gcstrBoy
                    If .intGender = clsEnum.emGender.FEMALE Then strRelName = gcstrKid & " " & gcstrGirl

                    If .intRelID = CInt(clsEnum.emRelation.ADOPT) Then strRelName &= " " & gcstrAdopt

                End With

                'add to grid
                xAdd2RelGrid(mstRel, strRelName)

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAddKids", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xGetStRel, fill data to Relation datagrid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : tblData DataTable, data table
    '      PARAMS2    : intRow  Integer, row to read
    '      PARAMS3    : blnGetKid   Boolean, in get kid state or not
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetStRel(ByVal tblData As DataTable, _
                                ByVal intRow As Integer, _
                                Optional ByVal blnGetKid As Boolean = False) As Boolean

        xGetStRel = False

        Try

            With tblData.Rows(intRow)
                'read from different column if GetKid mode is true
                If Not blnGetKid Then
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item("REL_FMEMBER_ID")), mstRel.intMemId)
                Else
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item("MEMBER_ID")), mstRel.intMemId)
                End If

                mstRel.strLastName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                mstRel.strMidName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                mstRel.strFirstName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))

                Integer.TryParse(basCommon.fncCnvNullToString(.Item("GENDER")), mstRel.intGender)
                'Date.TryParse(basCommon.fncCnvNullToString(.Item("BIRTH_DAY")), mstRel.dtBirth)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), mstRel.intBday)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), mstRel.intBmon)
                'Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), mstRel.intByea)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_DAY")), mstRel.stBirthDaySun.intDay)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_MON")), mstRel.stBirthDaySun.intMonth)
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("BIR_YEA")), mstRel.stBirthDaySun.intYear)
                'Start Manh 2012/11/08 Add Family Order to realationhip information
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("FAMILY_ORDER")), mstRel.intFamilyOrder)
                'End Manh 2012/11/08

                mstRel.strRemark = basCommon.fncCnvRtfToText(basCommon.fncCnvNullToString(.Item("REMARK")))

                'relation ship id - see basconst
                Integer.TryParse(basCommon.fncCnvNullToString(.Item("RELID")), mstRel.intRelID)

            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetStRel", ex)
        End Try

    End Function


    '   ******************************************************************
    '　　　FUNCTION   : xAdd2RelGrid, fill data to Relation datagrid
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : stRelInfo stRelationship, structure
    '      PARAMS2    : strRelName  String, relationship name
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xAdd2RelGrid(ByVal stRelInfo As stRelationship, ByVal strRelName As String) As Boolean

        xAdd2RelGrid = False
        Dim row(5) As String

        Try
            Dim strName As String = String.Empty
            Dim strBirth As String = String.Empty

            With stRelInfo
                'set member name
                strName = basCommon.fncRemove2Space(String.Format(basConst.gcstrNameFormat, .strLastName, .strMidName, .strFirstName))

                'birth text
                'If .dtBirth > Date.MinValue Then strBirth = String.Format(basConst.gcstrDateFormat2, .dtBirth)
                strBirth = basCommon.fncGetDateName("", .stBirthDaySun.intDay, .stBirthDaySun.intMonth, .stBirthDaySun.intYear, True)

                row(0) = basCommon.fncCnvNullToString(dgvRel.Rows.Count + 1)
                row(1) = basCommon.fncRemove2Space(strName)
                row(2) = strRelName
                row(3) = strBirth
                row(4) = .strRemark
            End With

            'add new row
            dgvRel.Rows.Add(row)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xAdd2RelGrid", ex)
        Finally
            Erase row
        End Try

    End Function


#End Region

#End Region

    Private Sub lblDelImg_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblDelImg.Click
        Try
            If basCommon.fncMessageConfirm("Ảnh đại diện của thành viên sẽ bị xóa, bạn có chắc chắn?") Then
                mblnDelImg = True
                picImage.Image = My.Resources.useradd64
                lblDelImg.Visible = False
                mstrAvatar = ""
            End If

        Catch ex As Exception

        End Try
    End Sub


    Private Sub btnWordExport_Click(sender As System.Object, e As System.EventArgs) Handles btnWordExport.Click
        Try

            If Not xSaveData(True) Then Return


            Using frmWordExport As New frmWord(Nothing)

                frmWordExport.fncExport2Word(Me.mintID)

            End Using

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "tsbExportWord_Click", ex)
        End Try
    End Sub
End Class
