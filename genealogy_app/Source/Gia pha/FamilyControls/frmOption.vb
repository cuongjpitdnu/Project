Option Explicit On
Option Strict Off
'   ****************************************************************** 
'      TITLE      : DRAW F-TREE OPTION
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2012/02/17　AKB　Quyet 
'      UPDATE     :  
' 
'           2012 AKB SOFTWARE 
'   ******************************************************************
Imports System.Drawing.Text
Imports System.IO

''' <summary>
''' Option class
''' </summary>
''' <remarks></remarks>
''' <Create>2012/02/17  AKB Quyet</Create>
Public Class frmOption


    Private Const mcstrClsName As String = "frmOption"                  'class name
    Private Const mcstrDefaultFrame As String = "Kiểu khung mặc định"
    Private Const mcstrFrameNumber As String = "Kiểu khung khung số "

    Private mblnChanged As Boolean = False

    Dim musrCard1 As usrMemberCard1

    ''' <summary>
    ''' The change is made
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Changed() As Boolean
        Get
            Return mblnChanged
        End Get
    End Property


    ''' <summary>
    ''' frmOption_Load
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub frmOption_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Try
            Dim usrDetail As usrMemberDetail

            'create preview card
            usrDetail = New usrMemberDetail()
            usrDetail.CardAlias = "ANH BA"
            usrDetail.CardBirth = "1900"
            usrDetail.CardContainer = usrMemCard2
            usrDetail.CardDie = "5/5 ÂL"
            usrDetail.CardImage = My.Resources.useradd64
            usrDetail.CardLevel = 5
            usrDetail.CardName = "NGUYỄN VĂN A"
            'usrDetail.CardRemark = "Tham gia cách mạng từ sớm, liệt sỹ chống Mỹ."
            'usrDetail.CardRemark = "{\rtf1\ansi\ansicpg932\deff0\deflang1033\deflangfe1041{\fonttbl{\f0\fswiss\fcharset0 Arial;}}""{\*\generator Msftedit 5.41.15.1515;}\viewkind4\uc1\pard\f0\fs20 Tham gia c\'e1ch m\u7841?ng t\u7915? s\u7899?m, li\u7879?t s\u7929? ch\u7889?ng M\u7929?.\par""}"
            usrDetail.CardRemark = "{\rtf1\ansi\ansicpg932\deff0\deflang1033\deflangfe1041{\fonttbl{\f0\fswiss\fcharset0 Arial;}}{\*\generator Msftedit 5.41.15.1515;}\viewkind4\uc1\pard\f0\fs20 Tham gia c\'e1ch m\u7841?ng t\u7915? s\u7899?m}"
            usrMemCard2.fncAddItem(usrDetail)

            'use new card, don't know why usrMemCard1 doesn't show image :(
            musrCard1 = New usrMemberCard1(1, False)
            ' ▽ 2018/03/08 AKB Nguyen Thanh Tung --------------------------------
            musrCard1.CardImage = My.Resources.useradd64
            musrCard1.CardGender = clsEnum.emGender.MALE
            musrCard1.CardName = "NGUYỄN VĂN A (Anh Ba)"
            musrCard1.Location = usrMemCard1.Location 'New Point(105, 19)
            'musrCard1.CardImage = My.Resources.useradd64
            'musrCard1.CardGender = clsEnum.emGender.MALE
            'musrCard1.CardName = "NGUYỄN VĂN A (Anh Ba)"
            'musrCard1.Location = usrMemCard1.Location 'New Point(105, 19)
            'musrCard1.Enabled = False
            ' △ 2018/03/08 AKB Nguyen Thanh Tung --------------------------------
            grbFrame.Controls.Add(musrCard1)

            ' ▽ 2018/03/02 AKB Nguyen Thanh Tung --------------------------------
            AddHandler rdCard2.CheckedChanged, AddressOf xSwitchMode
            AddHandler rdoShortHoz.CheckedChanged, AddressOf xSwitchMode
            AddHandler rdoShortVer.CheckedChanged, AddressOf xSwitchMode
            AddHandler rdCard1.CheckedChanged, AddressOf xSwitchMode

            If My.Settings.blnTypeCardShort Then

                If My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Horizontal Then
                    rdoShortHoz.Checked = True
                Else
                    rdoShortVer.Checked = True
                End If
            Else

                If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD2 Then
                    rdCard2.Checked = True
                Else
                    rdCard1.Checked = True
                End If
            End If

            'chkShowBithDay.Checked = My.Settings.blnShowBithDay
            'chkShowDieDay.Checked = My.Settings.blnShowDieDay
            'chkShowAward.Checked = My.Settings.blnShowAward

            nudHozSize.Value = My.Settings.intHozSize
            nudVerSize.Value = My.Settings.intVerSize
            chkShowLevel.Checked = My.Settings.blnShowLevel
            chkShowGender.Checked = My.Settings.blnShowGender

            Call xSwitchMode(Nothing, Nothing)

            ''load settings
            ''set card style
            'If My.Settings.intCardStyle = clsEnum.emCardStyle.CARD2 Then
            '    rdCard2.Checked = True
            '    nudHozBuffer.Enabled = False
            'End If
            ' △ 2018/03/02 AKB Nguyen Thanh Tung --------------------------------

            'set card size
            rdFrameFull.Checked = True
            If My.Settings.intCardSize = clsEnum.emCardSize.SMALL Then rdFrameCompact.Checked = True

            'set generation
            nudGeneration.Value = My.Settings.intGeneration

            nudHozBuffer.Value = My.Settings.intHozBuffer
            nudVerBuffer.Value = My.Settings.intVerBuffer
            chkShowUnknownBirthDay.Checked = My.Settings.blnShowUnknownBirthDay

            rdDeadSunCalendarShow.Checked = My.Settings.intDeadDateShowType = clsEnum.DeadDateShowType.SUN_CALENDAR
            rdDeadMoonCalendarShow.Checked = My.Settings.intDeadDateShowType = clsEnum.DeadDateShowType.MOON_CALENDAR

            Select Case My.Settings.intTypeDrawText
                Case CInt(clsEnum.emTypeDrawText.Normal)
                    rdoTextTypeNormal.Checked = True
                Case CInt(clsEnum.emTypeDrawText.RotateLeft)
                    rdoTexTypeLeft.Checked = True
                Case CInt(clsEnum.emTypeDrawText.RotateRight)
                    rdoTextTypeRight.Checked = True
            End Select

            AddHandler rdoTextTypeNormal.CheckedChanged, AddressOf eventChangeTypeDrawText
            AddHandler rdoTexTypeLeft.CheckedChanged, AddressOf eventChangeTypeDrawText
            AddHandler rdoTextTypeRight.CheckedChanged, AddressOf eventChangeTypeDrawText

            Call eventChangeTypeDrawText(Nothing, Nothing)

            ' ▽ 2018/01/31 AKB Nguyen Thanh Tung --------------------------------
            'xBingdingComboTypeShowTree()
            xBindingFontFomat()
            ' △ 2018/01/31 AKB Nguyen Thanh Tung --------------------------------

            xLoadFrame()

            grbFrame.Focus()
            rdCard1.Focus()
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmOption_Load", ex)
        End Try

    End Sub

    ''' <summary>
    ''' btnOK_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Try
            If Not basCommon.fncMessageConfirm("Các thuộc tính vẽ phả hệ đã thay đổi, bạn có muốn sử dụng những  thuộc tính này?") Then Exit Sub

            'set card style
            My.Settings.intCardStyle = CInt(clsEnum.emCardStyle.CARD1)
            If rdCard2.Checked Then My.Settings.intCardStyle = CInt(clsEnum.emCardStyle.CARD2)

            'set card size
            My.Settings.intCardSize = CInt(clsEnum.emCardSize.LARGE)
            If rdFrameCompact.Checked Then My.Settings.intCardSize = CInt(clsEnum.emCardSize.SMALL)

            'set generation
            My.Settings.intGeneration = nudGeneration.Value

            'save frame path
            If rdCard1.Checked Then
                Dim objItem As clsListItem = CType(cboFrameType.SelectedItem, clsListItem)
                My.Settings.strCard1Bg = CStr(objItem.Tag)
            End If

            My.Settings.intHozBuffer = nudHozBuffer.Value
            My.Settings.intVerBuffer = nudVerBuffer.Value
            My.Settings.blnShowUnknownBirthDay = chkShowUnknownBirthDay.Checked
            My.Settings.intDeadDateShowType = clsEnum.DeadDateShowType.MOON_CALENDAR

            If (rdDeadSunCalendarShow.Checked) Then My.Settings.intDeadDateShowType = clsEnum.DeadDateShowType.SUN_CALENDAR

            ' ▽ 2018/01/31 AKB Nguyen Thanh Tung --------------------------------
            'Show Member
            'My.Settings.intSelectedTypeShowTree = CInt(cboTypeShowTree.SelectedValue)

            'Short Card
            My.Settings.blnTypeCardShort = (rdoShortHoz.Checked OrElse rdoShortVer.Checked)
            If rdoShortHoz.Checked Then My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Horizontal
            If rdoShortVer.Checked Then My.Settings.intSelectedTypeCardShort = clsEnum.emTypeCardShort.Vertical

            If cboFont.SelectedIndex > -1 Then

                Using objFontFamily As FontFamily = CType(cboFont.SelectedValue, FontFamily)

                    My.Settings.objFontDefaut = New Font(objFontFamily, 8.25)

                End Using
            End If

            My.Settings.blnShowBackgroupDie = chkBackgroupColorDie.Checked
            My.Settings.objColorText = lblTextColor.BackColor
            My.Settings.objColorBackgroupCard = lblBackgroupColor.BackColor
            If chkBackgroupColorDie.Checked Then My.Settings.objColorBackgroupCardDie = lblBackgroupColorDie.BackColor
            'My.Settings.blnShowBithDay = chkShowBithDay.Checked
            'My.Settings.blnShowDieDay = chkShowDieDay.Checked
            'My.Settings.blnShowAward = chkShowAward.Checked
            My.Settings.intHozSize = nudHozSize.Value
            My.Settings.intVerSize = nudVerSize.Value
            My.Settings.blnShowLevel = chkShowLevel.Checked
            My.Settings.blnShowGender = chkShowGender.Checked
            My.Settings.intTypeDrawText = GetTypeDrawText()
            ' △ 2018/01/31 AKB Nguyen Thanh Tung --------------------------------

            My.Settings.Save()

            fncSetBufferBetween2Card()

            mblnChanged = True
            Me.Close()


        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnOK_Click", ex)
        End Try

    End Sub


    ''' <summary>
    ''' btnCancel_Click
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    ''' <Create>2012/02/17  AKB Quyet</Create>
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click

        Try
            Me.Close()

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnCancel_Click", ex)
        End Try

    End Sub


    Private Sub cboFrameType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboFrameType.SelectedIndexChanged
        Try
            Dim strPath As String
            Dim objItem As clsListItem

            'get file path of frame
            objItem = CType(cboFrameType.SelectedItem, clsListItem)
            strPath = CStr(objItem.Tag)

            'if path is not valid
            If strPath = "" Or Not File.Exists(strPath) Then
                'picFrame.Image = My.Resources.pic_frame
                musrCard1.CardBackground = My.Resources.pic_frame
            Else
                'load image
                'picFrame.Image = Image.FromFile(strPath)
                musrCard1.CardBackground = Image.FromFile(strPath)
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "cboFrameType_SelectedIndexChanged", ex)
        End Try
    End Sub


    ''' <summary>
    ''' Load frame background
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function xLoadFrame() As Boolean

        xLoadFrame = False

        Try
            Dim strFolder As String
            Dim intIndex As Integer = 1

            cboFrameType.Items.Clear()
            cboFrameType.Items.Add(New clsListItem(mcstrDefaultFrame, ""))
            cboFrameType.SelectedIndex = 0

            strFolder = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrFrameFolder

            If Not Directory.Exists(strFolder) Then
                cboFrameType.SelectedIndex = 0
                Return True
            End If


            For Each file As FileInfo In New DirectoryInfo(strFolder).GetFiles

                If Not file.Name.ToLower().EndsWith(".png") Then Continue For

                'add file to combobox
                cboFrameType.Items.Add(New clsListItem(mcstrFrameNumber & intIndex, file.FullName))
                If file.FullName = My.Settings.strCard1Bg Then cboFrameType.SelectedIndex = intIndex
                intIndex += 1

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadFrame", ex)
        End Try

    End Function


    ' ▽ 2018/03/02 AKB Nguyen Thanh Tung --------------------------------
    '''' <summary>
    '''' Check box changes
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    '''' <remarks></remarks>
    'Private Sub rdCard1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdCard1.CheckedChanged

    '    If rdCard1.Checked Then
    '        cboFrameType.Enabled = True
    '        nudHozBuffer.Enabled = True
    '    Else
    '        cboFrameType.Enabled = False
    '        nudHozBuffer.Enabled = False
    '    End If

    'End Sub
    ' △ 2018/03/02 AKB Nguyen Thanh Tung --------------------------------

    Private Sub nudVerBuffer_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudVerBuffer.ValueChanged
        lblVerCm.Text = "Khoảng: " & Math.Round(nudVerBuffer.Value / 96 * 25.4, 2) & " mm"
    End Sub

    Private Sub nudHozBuffer_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles nudHozBuffer.ValueChanged
        lblHozCm.Text = "Khoảng: " & Math.Round(nudHozBuffer.Value / 96 * 25.4, 2) & " mm"
    End Sub

#Region "Add By: 2018.03.09 AKB Nguyen Thanh Tung"

    Private Enum emSelectColor
        Text
        Backgroup
        BackgroupDie
    End Enum

    '==================Event==================

    Private Sub nudHozSize_ValueChanged(sender As Object, e As EventArgs) Handles nudHozSize.ValueChanged
        lblHozSizeCm.Text = "Khoảng: " & Math.Round(nudHozSize.Value / 96 * 25.4, 2) & " mm"
    End Sub

    Private Sub nudVerSize_ValueChanged(sender As Object, e As EventArgs) Handles nudVerSize.ValueChanged
        lblVerSizeCm.Text = "Khoảng: " & Math.Round(nudVerSize.Value / 96 * 25.4, 2) & " mm"
    End Sub

    Private Sub cboFont_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboFont.SelectedIndexChanged

        If cboFont.SelectedIndex < 0 Then Exit Sub

        Dim objFontFamily As FontFamily

        Try

            objFontFamily = CType(cboFont.SelectedValue, FontFamily)
            musrCard1.lblName.Font = New Font(objFontFamily, 8.25)

        Catch ex As Exception

        Finally

            objFontFamily = Nothing

        End Try
    End Sub

    Private Sub lblTextColor_Click(sender As Object, e As EventArgs) Handles lblTextColor.Click

        xSelectColor(emSelectColor.Text)

    End Sub

    Private Sub lblBackgroupColor_Click(sender As Object, e As EventArgs) Handles lblBackgroupColor.Click

        xSelectColor(emSelectColor.Backgroup)

    End Sub

    Private Sub lblBackgroupColorDie_Click(sender As Object, e As EventArgs) Handles lblBackgroupColorDie.Click

        xSelectColor(emSelectColor.BackgroupDie)

    End Sub

    Private Sub chkBackgroupColorDie_CheckedChanged(sender As Object, e As EventArgs) Handles chkBackgroupColorDie.Click

        lblBackgroupColorDie.Enabled = chkBackgroupColorDie.Checked

    End Sub

    'Private Sub cboTypeShowTree_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTypeShowTree.SelectedIndexChanged

    '    If cboTypeShowTree.SelectedIndex < 0 Then Exit Sub

    '    Try

    '        lblNote.Text = String.Empty

    '        If CInt(cboTypeShowTree.SelectedValue) = clsEnum.emTypeShowTree.All Then

    '            lblNote.Text = "(*) Hiển thị dâu, rể và cháu ngoại"

    '        End If

    '        If CInt(cboTypeShowTree.SelectedValue) = clsEnum.emTypeShowTree.OnlyShowMember Then

    '            lblNote.Text = "(*) Không hiển thị dâu, rể và cháu ngoại"

    '        End If
    '    Catch ex As Exception
    '        basCommon.fncSaveErr(mcstrClsName, "fncBindingComboBox", ex)
    '    End Try
    'End Sub

    Private Sub xSwitchMode(sender As Object, e As EventArgs)

        grbInfoCard.Enabled = Not rdCard2.Checked
        'chkShowAward.Enabled = rdCard2.Checked
        chkShowLevel.Enabled = (rdoShortHoz.Checked OrElse rdoShortVer.Checked)
        grbCardSize.Enabled = (rdoShortHoz.Checked OrElse rdoShortVer.Checked)
        grbShowImg.Enabled = Not (rdoShortHoz.Checked OrElse rdoShortVer.Checked)
        grbShowDieDay.Enabled = Not (rdoShortHoz.Checked OrElse rdoShortVer.Checked)
        grbFomat.Enabled = Not rdCard2.Checked
        cboFrameType.Enabled = rdCard1.Checked
        nudHozBuffer.Enabled = Not rdCard2.Checked
        chkShowUnknownBirthDay.Enabled = rdCard1.Checked
        nudVerSize.Enabled = rdoShortVer.Checked
        nudHozSize.Enabled = rdoShortHoz.Checked
        grbTypeTextDisplay.Enabled = rdoShortHoz.Checked
    End Sub

    '============Private Function=============

    Private Function xSelectColor(ByVal emMode As emSelectColor,
                                  Optional objColor As Color? = Nothing) As Boolean

        xSelectColor = False

        Dim lblSelect As Label = Nothing

        Select Case emMode
            Case emSelectColor.Text
                lblSelect = lblTextColor
            Case emSelectColor.Backgroup
                lblSelect = lblBackgroupColor
            Case emSelectColor.BackgroupDie
                lblSelect = lblBackgroupColorDie
        End Select

        If IsNothing(lblSelect) Then Exit Function

        If IsNothing(objColor) Then

            dlgColor.Color = lblSelect.BackColor

            If dlgColor.ShowDialog() = DialogResult.OK Then

                lblSelect.BackColor = dlgColor.Color

            End If

        Else

            lblSelect.BackColor = objColor

        End If

        Select Case emMode
            Case emSelectColor.Text
                musrCard1.lblName.ForeColor = lblSelect.BackColor
            Case emSelectColor.Backgroup
                musrCard1.BackColor = lblSelect.BackColor
        End Select
    End Function

    'Private Function xBingdingComboTypeShowTree() As Boolean

    '    xBingdingComboTypeShowTree = False

    '    Dim lstData As New List(Of clsDataSourceComboBox)

    '    lstData.Add(New clsDataSourceComboBox() With {
    '        .Display = "Đầy đủ",
    '        .Value = CInt(clsEnum.emTypeShowTree.All)
    '    })

    '    lstData.Add(New clsDataSourceComboBox() With {
    '        .Display = "Rút gọn",
    '        .Value = CInt(clsEnum.emTypeShowTree.OnlyShowMember)
    '    })

    '    lstData.Add(New clsDataSourceComboBox() With {
    '        .Display = "Nam",
    '        .Value = CInt(clsEnum.emTypeShowTree.OnlyShowMale)
    '    })

    '    basCommon.fncBindingComboBox(cboTypeShowTree, lstData, My.Settings.intSelectedTypeShowTree)
    'End Function

    Public Function xBindingFontFomat() As Boolean

        Try

            Using installedFont As New InstalledFontCollection()

                Dim lstData As New List(Of clsDataSourceComboBox)

                For Each objFont As FontFamily In installedFont.Families

                    lstData.Add(New clsDataSourceComboBox() With {
                        .Display = objFont.Name,
                        .Value = objFont
                    })

                Next

                basCommon.fncBindingComboBox(cboFont, lstData, My.Settings.objFontDefaut.FontFamily)
            End Using

            xSelectColor(emSelectColor.Text, My.Settings.objColorText)
            xSelectColor(emSelectColor.Backgroup, My.Settings.objColorBackgroupCard)
            xSelectColor(emSelectColor.BackgroupDie, My.Settings.objColorBackgroupCardDie)
            chkBackgroupColorDie.Checked = My.Settings.blnShowBackgroupDie
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "fncBindingComboBox", ex)
        End Try
    End Function

    Private Function GetTypeDrawText() As Integer

        If Not grbTypeTextDisplay.Enabled Then
            Return My.Settings.intTypeDrawText
        End If

        If rdoTextTypeNormal.Checked Then
            Return CInt(clsEnum.emTypeDrawText.Normal)
        End If

        If rdoTexTypeLeft.Checked Then
            Return CInt(clsEnum.emTypeDrawText.RotateLeft)
        End If

        If rdoTextTypeRight.Checked Then
            Return CInt(clsEnum.emTypeDrawText.RotateRight)
        End If
    End Function

    Private Sub eventChangeTypeDrawText(sender As Object, e As EventArgs)
        If rdoTextTypeNormal.Checked Then
            nudHozSize.Minimum = 30
            nudHozSize.Maximum = 300
        Else
            nudHozSize.Minimum = 5
            nudHozSize.Maximum = 80
        End If
    End Sub
#End Region
End Class