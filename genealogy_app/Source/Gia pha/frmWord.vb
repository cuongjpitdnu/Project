'   ******************************************************************
'      TITLE      : EXPORT TO WORD FORM
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/09/14　AKB 
'      UPDATE     : 
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

Imports System.Xml

Public Class frmWord

    Private Structure stDocField

        Public MEMBER_ID As Integer
        Public MEMBER_NAME As Integer
        Public ALIAS_NAME As Integer
        Public BIRTH_DAY As Integer
        Public GENDER As Integer
        Public BIRTH_PLACE As Integer
        Public NATIONALITY As Integer
        Public RELIGION As Integer
        Public DECEASED As Integer
        Public DECEASED_DATE As Integer
        Public BURY_PLACE As Integer
        Public AVATAR_PATH As Integer
        Public FAMILY_ORDER As Integer
        Public MAIN_REMARK As Integer
        Public HOMETOWN As Integer
        Public HOME_ADD As Integer
        Public PHONENUM1 As Integer
        Public PHONENUM2 As Integer
        Public MAIL_ADD1 As Integer
        Public MAIL_ADD2 As Integer
        Public FAXNUM As Integer
        Public URL As Integer
        Public IMNICK As Integer
        Public CONTACT_REMARK As Integer
        Public EDUCATION As Integer
        Public CAREER As Integer
        Public FACT As Integer
        Public FAMILY As Integer
        Public GENERATION As Integer
        Public BIRTH_DAY_LUNAR As Integer
        'Public FATHER As Integer
        'Public MOTHER As Integer
        Public PARENTS As Integer           'Parents of member
        Public BROS As Integer              'Brother and sister of member
        Public DECEASED_DATE_SUN As Integer
        Public DECEASED_DATE_LUNAR As Integer
        Public SPOUSE As Integer
        Public CHILDREN As Integer

    End Structure

    Private emWordRow As stDocField
    Private emWordCol As stDocField

    Private Enum emDataItem
        MEMBER_ID
        MEMBER_NAME
        ALIAS_NAME
        BIRTH_DAY
        GENDER
        BIRTH_PLACE
        NATIONALITY
        RELIGION
        DECEASED
        DECEASED_DATE
        BURY_PLACE
        AVATAR_PATH
        FAMILY_ORDER
        MAIN_REMARK
        HOMETOWN
        HOME_ADD
        PHONENUM1
        PHONENUM2
        MAIL_ADD1
        MAIL_ADD2
        FAXNUM
        URL
        IMNICK
        CONTACT_REMARK
        EDUCATION
        CAREER
        FACT
        FAMILY
        GENERATION
        BIRTH_DAY_LUNAR
        FATHER
        MOTHER
        BROS
        DECEASED_DATE_SUN
        DECEASED_DATE_LUNAR
        SPOUSE
        CHILDREN

    End Enum

    'Private Enum emWordCol
    '    MEMBER_ID
    '    MEMBER_NAME = 3
    '    ALIAS_NAME = 1
    '    BIRTH_DAY = 3
    '    GENDER = 3
    '    BIRTH_PLACE = 3
    '    NATIONALITY = 3
    '    RELIGION = 3
    '    DECEASED = 3
    '    DECEASED_DATE = 3
    '    BURY_PLACE = 3
    '    AVATAR_PATH = 4
    '    FAMILY_ORDER = 3
    '    MAIN_REMARK = 3
    '    HOMETOWN = 3
    '    HOME_ADD = 3
    '    PHONENUM1 = 3
    '    PHONENUM2 = 3
    '    MAIL_ADD1 = 3
    '    MAIL_ADD2 = 3
    '    FAXNUM = 3
    '    URL = 3
    '    IMNICK = 3
    '    CONTACT_REMARK = 1
    '    EDUCATION = 1
    '    CAREER = 1
    '    FACT = 3
    '    FAMILY = 3
    '    GENERATION = 3
    '    BIRTH_DAY_LUNAR = 3
    '    FATHER = 1
    '    MOTHER = 1
    '    DECEASED_DATE_SUN = 3
    '    DECEASED_DATE_LUNAR = 3
    '    SPOUSE = 1
    '    CHILDREN = 2

    'End Enum

    Private mobjWord As clsWord
    Private Const mcstrClsName As String = "frmWord"                            'class name
    Private Const mcstrInitError As String = "Khởi tạo không thành công."              'form init error message
    Private Const mcstrEduFormat As String = "{0} ({1} ～　{2}) : {3}"          'format for edu string
    Private Const mcstrCareerFormat As String = "{0} ({1} ～　{2}) : {3}, {4}"  'format for career string
    Private Const mcstrFactFormat As String = "{0} ({1} ～　{2}) : {3}, {4}"        'format for fact string

    Private mtblData As DataTable                                               'data table from main form
    'Private mstrFileName As String                                             'file path to save
    Private mblnFormLoaded As Boolean = False                                   'flag to determine form fully loaded


    ''' <summary>
    ''' CONSTRUCTOR
    ''' </summary>
    ''' <param name="tblData"></param>
    ''' <remarks></remarks>
    Public Sub New(ByVal tblData As DataTable)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mtblData = tblData

    End Sub


#Region "FORM EVENTs"

    '   ******************************************************************
    '　　　FUNCTION   : frmWord_Load
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub frmWord_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            If Not xLoadInitData() Then

                basCommon.fncMessageWarning(mcstrInitError)
                Me.Close()

            End If

            xSelectListState(True)

            mblnFormLoaded = True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "frmWord_Load", ex)
        End Try
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : chkSelectAll_CheckedChanged
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub chkSelectAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkSelectAll.CheckedChanged
        Try

            If mblnFormLoaded Then xSelectListState(chkSelectAll.Checked)

            If chkSelectAll.Checked Then
                chkSelectAll.Text = "Bỏ chọn tất cả"
            Else
                chkSelectAll.Text = "Chọn tất cả"
            End If

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "chkSelectAll_CheckedChanged", ex)
        End Try
    End Sub

    Private Sub xCheckWordPlate()
        'If Not basCommon.fncSaveFileDlg(mstrFileName, "MS Word|*.doc|All Files|*.*", ".doc") Then Exit Sub
        If Not System.IO.File.Exists(xWordTemplateFile()) Then
            basCommon.fncMessageWarning("Không tìm thấy tệp tin mẫu.")
            Exit Sub
        End If
    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : btnExport_Click
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Me.Enabled = False

        Try
            xCheckWordPlate()

            If Not xReadXML() Then Exit Sub

            Cursor = Cursors.WaitCursor

            If Not xExportToWord() Then
                basCommon.fncMessageError("Xuất dữ liệu thất bại.")
                Exit Sub
            End If

            'basCommon.fncMessageInfo("Xuất dữ liệu thành viên ra file Word đã hoàn thành.")
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "btnExport_Click", ex)
            basCommon.fncMessageError("Có lỗi trong quá trình xử lý.")
        Finally
            Cursor = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : Export to word
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Public Sub fncExport2Word(ByVal intMemberID As Integer)

        Try
            xCheckWordPlate()
            If Not xReadXML() Then Exit Sub

            Cursor = Cursors.WaitCursor

            Dim dtRow As DataRow
            Dim tblDataExport As DataTable
            tblDataExport = xExportContentStructure()

            dtRow = tblDataExport.NewRow()

            'fill details to row
            xGetMainInfo(dtRow, intMemberID, 0)

            tblDataExport.Rows.Add(dtRow)

            xExport2WordFile(tblDataExport)

            Cursor = Cursors.Default
            basCommon.fncMessageInfo("Xuất dữ liệu thành viên ra file Word đã hoàn thành.")

        Catch ex As Exception

        Finally
            Cursor = Cursors.Default
        End Try


    End Sub

#End Region

    '   ******************************************************************
    '　　　FUNCTION   : xReadXML
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xReadXML() As Boolean

        xReadXML = False
        Try
            Dim m_xmld As XmlDocument
            Dim m_nodelist As XmlNodeList
            Dim m_node As XmlNode
            Dim strXmlFile As String = ""
            strXmlFile = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrDocXml
            If Not System.IO.File.Exists(strXmlFile) Then
                basCommon.fncMessageWarning("Không tìm thấy tệp thiết lập.")
                Exit Function
            End If

            'Create the XML Document
            m_xmld = New XmlDocument()
            'Load the Xml file
            m_xmld.Load(strXmlFile)
            'Get the list of name nodes 
            m_nodelist = m_xmld.SelectNodes("/FieldLocation/Field")
            'Loop through the nodes
            For Each m_node In m_nodelist

                'Get the Gender Attribute Value
                Select Case m_node.Attributes.GetNamedItem("gender").Value

                    Case "MEMBER_NAME"
                        emWordRow.MEMBER_NAME = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.MEMBER_NAME = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "ALIAS_NAME"
                        emWordRow.ALIAS_NAME = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.ALIAS_NAME = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "BIRTH_DAY"
                        emWordRow.BIRTH_DAY = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.BIRTH_DAY = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "GENDER"
                        emWordRow.GENDER = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.GENDER = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "BIRTH_PLACE"
                        emWordRow.BIRTH_PLACE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.BIRTH_PLACE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)

                    Case "NATIONALITY"
                        emWordRow.NATIONALITY = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.NATIONALITY = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "RELIGION"
                        emWordRow.RELIGION = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.RELIGION = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "DECEASED"
                        emWordRow.DECEASED = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.DECEASED = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "DECEASED_DATE"
                        emWordRow.DECEASED_DATE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.DECEASED_DATE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "BURY_PLACE"
                        emWordRow.BURY_PLACE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.BURY_PLACE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "AVATAR_PATH"
                        emWordRow.AVATAR_PATH = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.AVATAR_PATH = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "FAMILY_ORDER"
                        emWordRow.FAMILY_ORDER = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.FAMILY_ORDER = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "MAIN_REMARK"
                        emWordRow.MAIN_REMARK = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.MAIN_REMARK = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "HOMETOWN"
                        emWordRow.HOMETOWN = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.HOMETOWN = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "HOME_ADD"
                        emWordRow.HOME_ADD = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.HOME_ADD = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "PHONENUM1"
                        emWordRow.PHONENUM1 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.PHONENUM1 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "PHONENUM2"
                        emWordRow.PHONENUM2 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.PHONENUM2 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "MAIL_ADD1"
                        emWordRow.MAIL_ADD1 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.MAIL_ADD1 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "MAIL_ADD2"
                        emWordRow.MAIL_ADD2 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.MAIL_ADD2 = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "FAXNUM"
                        emWordRow.FAXNUM = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.FAXNUM = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "URL"
                        emWordRow.URL = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.URL = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "IMNICK"
                        emWordRow.IMNICK = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.IMNICK = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "CONTACT_REMARK"
                        emWordRow.CONTACT_REMARK = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.CONTACT_REMARK = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "EDUCATION"
                        emWordRow.EDUCATION = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.EDUCATION = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "CAREER"
                        emWordRow.CAREER = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.CAREER = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "FACT"
                        emWordRow.FACT = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.FACT = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "FAMILY"
                        emWordRow.FAMILY = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.FAMILY = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "GENERATION"
                        emWordRow.GENERATION = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.GENERATION = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "BIRTH_DAY_LUNAR"
                        emWordRow.BIRTH_DAY_LUNAR = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.BIRTH_DAY_LUNAR = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "PARENTS"
                        emWordRow.PARENTS = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.PARENTS = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "BROS"
                        emWordRow.BROS = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.BROS = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "DECEASED_DATE_SUN"
                        emWordRow.DECEASED_DATE_SUN = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.DECEASED_DATE_SUN = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "DECEASED_DATE_LUNAR"
                        emWordRow.DECEASED_DATE_LUNAR = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.DECEASED_DATE_LUNAR = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "SPOUSE"
                        emWordRow.SPOUSE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.SPOUSE = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)
                    Case "CHILDREN"
                        emWordRow.CHILDREN = basCommon.fncCnvToInt(m_node.ChildNodes.Item(0).InnerText)
                        emWordCol.CHILDREN = basCommon.fncCnvToInt(m_node.ChildNodes.Item(1).InnerText)

                End Select

            Next

            xReadXML = True
        Catch ex As Exception
            'Error trapping
            basCommon.fncSaveErr(mcstrClsName, "xReadXML", ex)
        End Try
    End Function

#Region "FORM METHODs"

    '   ******************************************************************
    '　　　FUNCTION   : fncShowForm
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
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
    '　　　FUNCTION   : xLoadInitData
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xLoadInitData() As Boolean

        xLoadInitData = False

        Try
            'bind data and sorting
            dgvList.DataSource = xGetDataSource(mtblData)
            'dgvList.Sort(dgvList.Columns(clmTempLevel.Name), System.ComponentModel.ListSortDirection.Ascending)

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xLoadInitData", ex)
        Finally
        End Try

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xGetDataSource
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xGetDataSource(ByVal dtTableSrc As DataTable) As DataTable

        Dim dtResult As DataTable = Nothing
        Dim row() As DataRow = Nothing
        Dim objCell() As Object

        Try
            Dim strFName As String
            Dim strMName As String
            Dim strLName As String
            Dim strAlias As String
            Dim intLevel As Integer

            If dtTableSrc Is Nothing Then Return dtResult

            dtResult = New DataTable
            dtResult.Columns.Add(clmSTT.Name)
            dtResult.Columns.Add(clmMemID.Name)
            dtResult.Columns.Add(clmName.Name)
            dtResult.Columns.Add(clmGeneration.Name)
            dtResult.Columns.Add(clmTempLevel.Name)

            row = dtTableSrc.Select("", "LEVEL ASC, MEMBER_ID")
            'row = dtTableSrc.Select("", "")
            objCell = New Object(dtResult.Columns.Count - 1) {}

            For i As Integer = 0 To row.Length - 1

                With row(i)

                    'name
                    strFName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                    strMName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                    strLName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                    strAlias = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))

                    'STT
                    objCell(0) = i + 1

                    'member id
                    objCell(1) = .Item("MEMBER_ID")

                    'full name with alias 
                    objCell(2) = basCommon.fncGetFullName(strFName, strMName, strLName, strAlias)

                    'generation
                    objCell(3) = ""
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item("LEVEL")), intLevel)
                    If intLevel > 0 Then objCell(3) = intLevel

                    'temp generation for sorting
                    objCell(4) = .Item("LEVEL")

                    dtResult.Rows.Add(objCell)

                End With

            Next

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetDatatable", ex)
        Finally
            Erase row
            Erase objCell
        End Try

        Return dtResult

    End Function

    '   ******************************************************************
    '　　　FUNCTION   : xSelectListState
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB 
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xSelectListState(ByVal blnCheckState As Boolean) As Boolean

        xSelectListState = False

        Try
            Dim chkSelectBox As DataGridViewCheckBoxCell

            'set check state for list
            For i As Integer = 0 To dgvList.Rows.Count - 1

                chkSelectBox = CType(dgvList.Item(clmSelect.Name, i), DataGridViewCheckBoxCell)

                If blnCheckState Then
                    'chkSelectBox.Value = chkSelectBox.TrueValue
                    chkSelectBox.Value = 1
                Else
                    'chkSelectBox.Value = chkSelectBox.FalseValue
                    chkSelectBox.Value = 0
                End If

            Next

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xSelectListState", ex)
        End Try

    End Function

    Private Function xExportContentStructure() As DataTable
        Dim tblDataExport As DataTable = Nothing

        tblDataExport = New DataTable()
        tblDataExport.Columns.Add("MEMBER_ID")
        tblDataExport.Columns.Add("MEMBER_NAME")
        tblDataExport.Columns.Add("ALIAS_NAME")
        tblDataExport.Columns.Add("BIRTH_DAY")
        tblDataExport.Columns.Add("GENDER")
        tblDataExport.Columns.Add("BIRTH_PLACE")
        tblDataExport.Columns.Add("NATIONALITY")
        tblDataExport.Columns.Add("RELIGION")
        tblDataExport.Columns.Add("DECEASED")
        tblDataExport.Columns.Add("DECEASED_DATE")
        tblDataExport.Columns.Add("BURY_PLACE")
        tblDataExport.Columns.Add("AVATAR_PATH")
        tblDataExport.Columns.Add("FAMILY_ORDER")
        tblDataExport.Columns.Add("MAIN_REMARK")
        tblDataExport.Columns.Add("HOMETOWN")
        tblDataExport.Columns.Add("HOME_ADD")
        tblDataExport.Columns.Add("PHONENUM1")
        tblDataExport.Columns.Add("PHONENUM2")
        tblDataExport.Columns.Add("MAIL_ADD1")
        tblDataExport.Columns.Add("MAIL_ADD2")
        tblDataExport.Columns.Add("FAXNUM")
        tblDataExport.Columns.Add("URL")
        tblDataExport.Columns.Add("IMNICK")
        tblDataExport.Columns.Add("CONTACT_REMARK")

        tblDataExport.Columns.Add("EDUCATION")
        tblDataExport.Columns.Add("CAREER")
        tblDataExport.Columns.Add("FACT")
        tblDataExport.Columns.Add("FAMILY")
        tblDataExport.Columns.Add("GENERATION")

        tblDataExport.Columns.Add("BIRTH_DAY_LUNAR")
        tblDataExport.Columns.Add("FATHER")
        tblDataExport.Columns.Add("MOTHER")
        tblDataExport.Columns.Add("BROS")
        tblDataExport.Columns.Add("DECEASED_DATE_SUN")
        tblDataExport.Columns.Add("DECEASED_DATE_LUNAR")
        tblDataExport.Columns.Add("SPOUSE")
        tblDataExport.Columns.Add("CHILDREN")

        Return tblDataExport
    End Function

    Private Function xWordTemplateFile() As String
        Dim strFile As String = "D:\MemberTemp.dot"

        strFile = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrDocTemplate_new

        Return strFile
    End Function

    Private Sub xExport2WordFile(ByVal tblDataExport As DataTable)

        mobjWord = New clsWord

        mobjWord.Open(xWordTemplateFile())

        For i As Integer = 0 To tblDataExport.Rows.Count - 1

            If i > 0 Then

                Threading.Thread.Sleep(10)
                If Not mobjWord.fncCopyTable(1, i) Then Exit For

            End If

        Next i
        For i As Integer = 0 To tblDataExport.Rows.Count - 1

            'If i > 0 Then

            '    If Not mobjWord.fncCopyTable(1, i) Then Exit For

            'End If
            Threading.Thread.Sleep(10)
            If Not xExportMemInfo(tblDataExport.Rows(i), i + 1) Then Exit For
        Next

        basCommon.fncMessageInfo("Xuất dữ liệu thành viên ra file Word đã hoàn thành.")

        Dim pathSave As String = String.Empty

        If basCommon.CallSaveDialog(pathSave) Then
            mobjWord.SaveAs(pathSave)

            If IO.File.Exists(pathSave) Then
                Process.Start(pathSave)
            End If
        End If

        mobjWord.fncExit()
        mobjWord = Nothing

        Return

    End Sub

    '   ******************************************************************
    '　　　FUNCTION   : Main method
    '      MEMO       : 
    '      CREATE     : 2011/07/15  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Private Function xExportToWord() As Boolean

        xExportToWord = False

        Dim tblDataExport As DataTable = Nothing

        Try

            'tblDataExport = New DataTable("mydata")
            tblDataExport = xExportContentStructure()

            Dim dtRow As DataRow
            Dim intGeneration As Integer
            Dim intMemID As Integer
            Dim chkSelectBox As DataGridViewCheckBoxCell
            Dim strChkValue As String
            Dim blnChkStt As Boolean

            blnChkStt = False

            For i As Integer = 0 To dgvList.RowCount - 1

                chkSelectBox = CType(dgvList.Item(clmSelect.Name, i), DataGridViewCheckBoxCell)

                strChkValue = basCommon.fncCnvNullToString(chkSelectBox.Value)

                'If chkSelectBox.Value Is chkSelectBox.FalseValue Then Continue For

                If strChkValue <> "1" Then Continue For

                dtRow = tblDataExport.NewRow()
                intGeneration = basCommon.fncCnvToInt(dgvList.Item(clmGeneration.Name, i).Value)
                intMemID = basCommon.fncCnvToInt(dgvList.Item(clmMemID.Name, i).Value)

                'fill details to row
                xGetMainInfo(dtRow, intMemID, intGeneration)

                tblDataExport.Rows.Add(dtRow)

                blnChkStt = True

            Next

            'exit if there is no member selected
            If Not blnChkStt Then
                basCommon.fncMessageWarning("Không có thành viên nào được chọn.", dgvList)
                Exit Function
            End If

            xExport2WordFile(tblDataExport)

            Return True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xExportToWord", ex)

        Finally

            If tblDataExport IsNot Nothing Then tblDataExport.Dispose()

            tblDataExport = Nothing

            If Not IsNothing(mobjWord) Then

                mobjWord.fncExit()

                mobjWord = Nothing

            End If


        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : xExportMemInfo
    '      VALUE      : String
    '      PARAMS     : strInput String, input string
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xExportMemInfo(ByVal objRow As Data.DataRow, ByVal intTable As Integer) As Boolean

        xExportMemInfo = False

        Dim strPhone As String
        Try
            If IsNothing(objRow) Then Return False
            Dim strFather As String
            Dim strMother As String

            'name
            If fncCnvNullToString(objRow.Item(emDataItem.ALIAS_NAME)) <> "" Then
                mobjWord.fncSetTableCellVal(intTable, emWordRow.MEMBER_NAME, emWordCol.MEMBER_NAME, fncCnvNullToString(objRow.Item(emDataItem.MEMBER_NAME)) & vbCrLf & "(" & fncCnvNullToString(objRow.Item(emDataItem.ALIAS_NAME)) & ")")
            Else
                mobjWord.fncSetTableCellVal(intTable, emWordRow.MEMBER_NAME, emWordCol.MEMBER_NAME, fncCnvNullToString(objRow.Item(emDataItem.MEMBER_NAME)))
            End If

            mobjWord.fncSetTableCellVal(intTable, emWordRow.ALIAS_NAME, emWordCol.ALIAS_NAME, fncCnvNullToString(objRow.Item(emDataItem.ALIAS_NAME)))

            'Birthday
            mobjWord.fncSetTableCellVal(intTable, emWordRow.BIRTH_DAY, emWordCol.BIRTH_DAY, fncCnvNullToString(objRow.Item(emDataItem.BIRTH_DAY)))

            'Gender
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.GENDER, emWordCol.GENDER, fncCnvNullToString(objRow.Item("GENDER")))

            'BirthPlace
            mobjWord.fncSetTableCellVal(intTable, emWordRow.BIRTH_PLACE, emWordCol.BIRTH_PLACE, fncCnvNullToString(objRow.Item(emDataItem.BIRTH_PLACE)))

            'National
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.NATIONALITY, emWordCol.NATIONALITY, fncCnvNullToString(objRow.Item("NATIONALITY")))

            'Death Date
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.DECEASED_DATE, emWordCol.DECEASED_DATE, fncCnvNullToString(objRow.Item("DECEASED_DATE")))

            'Death Place
            mobjWord.fncSetTableCellVal(intTable, emWordRow.BURY_PLACE, emWordCol.BURY_PLACE, fncCnvNullToString(objRow.Item(emDataItem.BURY_PLACE)))

            'Show avatar
            mobjWord.fncSetTableCellImg(intTable, emWordRow.AVATAR_PATH, emWordCol.AVATAR_PATH, fncCnvNullToString(objRow.Item(emDataItem.AVATAR_PATH)), clsDefine.THUMBNAIL_H, clsDefine.THUMBNAIL_W)


            'mobjWord.fncSetTableCellVal(intTable, emWordRow.FAMILY_ORDER, emWordCol.FAMILY_ORDER, fncCnvNullToString(objRow.Item("FAMILY_ORDER")))


            'Hometown
            mobjWord.fncSetTableCellVal(intTable, emWordRow.HOMETOWN, emWordCol.HOMETOWN, fncCnvNullToString(objRow.Item(emDataItem.HOMETOWN)))

            'Address
            mobjWord.fncSetTableCellVal(intTable, emWordRow.HOME_ADD, emWordCol.HOME_ADD, fncCnvNullToString(objRow.Item(emDataItem.HOME_ADD)))

            strPhone = fncCnvNullToString(objRow.Item("PHONENUM1"))
            strPhone &= "   " & fncCnvNullToString(objRow.Item("PHONENUM2"))
            If (strPhone.Replace(" ", "") <> "") Then
                strPhone &= vbCrLf
            End If
            strPhone &= fncCnvNullToString(objRow.Item("MAIL_ADD1"))
            strPhone &= "   " & fncCnvNullToString(objRow.Item("MAIL_ADD2"))

            mobjWord.fncSetTableCellVal(intTable, emWordRow.PHONENUM1, emWordCol.PHONENUM1, strPhone)
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.PHONENUM2, emWordCol.PHONENUM2, fncCnvNullToString(objRow.Item("PHONENUM2")))
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.MAIL_ADD1, emWordCol.MAIL_ADD1, fncCnvNullToString(objRow.Item("MAIL_ADD1")))
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.MAIL_ADD2, emWordCol.MAIL_ADD2, fncCnvNullToString(objRow.Item("MAIL_ADD2")))
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.FAXNUM, emWordCol.FAXNUM, fncCnvNullToString(objRow.Item("FAXNUM")))
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.URL, emWordCol.URL, fncCnvNullToString(objRow.Item("URL")))
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.IMNICK, emWordCol.IMNICK, fncCnvNullToString(objRow.Item("IMNICK")))

            'mobjWord.fncSetTableCellVal(intTable, emWordRow.CONTACT_REMARK, emWordCol.CONTACT_REMARK, fncCnvNullToString(objRow.Item("CONTACT_REMARK")))

            'Education
            mobjWord.fncSetTableCellVal(intTable, emWordRow.EDUCATION, emWordCol.EDUCATION, fncCnvNullToString(objRow.Item(emDataItem.EDUCATION)) & vbCrLf & fncCnvNullToString(objRow.Item(emDataItem.CAREER)))


            'mobjWord.fncSetTableCellVal(intTable, emWordRow.CAREER, emWordCol.CAREER, fncCnvNullToString(objRow.Item("CAREER")))
            mobjWord.fncSetTableCellVal(intTable, emWordRow.FACT, emWordCol.FACT, fncCnvNullToString(objRow.Item(emDataItem.FACT)))
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.FAMILY, emWordCol.FAMILY, fncCnvNullToString(objRow.Item("FAMILY")))
            mobjWord.fncSetTableCellVal(intTable, emWordRow.GENERATION, emWordCol.GENERATION, fncCnvNullToString(objRow.Item(emDataItem.GENERATION)))

            mobjWord.fncSetTableCellVal(intTable, emWordRow.BIRTH_DAY_LUNAR, emWordCol.BIRTH_DAY_LUNAR, fncCnvNullToString(objRow.Item(emDataItem.BIRTH_DAY_LUNAR)))

            'Father
            strFather = fncCnvNullToString(objRow.Item(emDataItem.FATHER))
            If (strFather <> "") Then strFather = "Bố " & strFather
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.FATHER, emWordCol.FATHER, fncCnvNullToString(objRow.Item(emDataItem.FATHER)))

            'Mother
            strMother = fncCnvNullToString(objRow.Item(emDataItem.MOTHER))
            If (strMother <> "") Then strMother = "Mẹ " & strMother
            'mobjWord.fncSetTableCellVal(intTable, emWordRow.MOTHER, emWordCol.MOTHER, fncCnvNullToString(objRow.Item(emDataItem.MOTHER)))

            mobjWord.fncSetTableCellVal(intTable, emWordRow.PARENTS, emWordCol.PARENTS, strFather & vbCrLf & strMother)
            mobjWord.fncSetTableCellVal(intTable, emWordRow.BROS, emWordCol.BROS, fncCnvNullToString(objRow.Item(emDataItem.BROS)))

            'Death Date
            mobjWord.fncSetTableCellVal(intTable, emWordRow.DECEASED_DATE_SUN, emWordCol.DECEASED_DATE_SUN, fncCnvNullToString(objRow.Item(emDataItem.DECEASED_DATE_SUN)))
            mobjWord.fncSetTableCellVal(intTable, emWordRow.DECEASED_DATE_LUNAR, emWordCol.DECEASED_DATE_LUNAR, fncCnvNullToString(objRow.Item(emDataItem.DECEASED_DATE_LUNAR)))

            mobjWord.fncSetTableCellVal(intTable, emWordRow.SPOUSE, emWordCol.SPOUSE, fncCnvNullToString(objRow.Item(emDataItem.SPOUSE)))

            mobjWord.fncSetTableCellVal(intTable, emWordRow.CHILDREN, emWordCol.CHILDREN, fncCnvNullToString(objRow.Item(emDataItem.CHILDREN)))

            mobjWord.fncSetTableCellVal(intTable, emWordRow.MAIN_REMARK, emWordCol.MAIN_REMARK, fncCnvNullToString(objRow.Item(emDataItem.MAIN_REMARK)))

            xExportMemInfo = True

        Catch ex As Exception

            basCommon.fncSaveErr(mcstrClsName, "xExportMemInfo", ex)

        End Try

    End Function

    ''' <summary>
    ''' Get Member's main infor
    ''' </summary>
    ''' <param name="dtRow">Return data row</param>
    ''' <param name="intMemID">member id</param>
    ''' <param name="intGeneration">generation of member</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetMainInfo(ByRef dtRow As DataRow, ByVal intMemID As Integer, ByVal intGeneration As Integer) As Boolean

        xGetMainInfo = False

        Dim tblData As DataTable = Nothing

        Try
            Dim strFName As String
            Dim strMName As String
            Dim strLName As String
            Dim strAvatar As String
            Dim intNation As Integer
            Dim intReligion As Integer
            Dim intGender As Integer

            Dim intBday As Integer
            Dim intBmon As Integer
            Dim intByea As Integer

            Dim intBDayMoon As Integer
            Dim intBMonMoon As Integer
            Dim intBYeaMoon As Integer

            Dim intDday As Integer
            Dim intDmon As Integer
            Dim intDyea As Integer

            Dim intDDaySun As Integer
            Dim intDMonSun As Integer
            Dim intDYeaSun As Integer

            Dim intCareerType As Integer
            Dim intEduType As Integer
            Dim intFactType As Integer

            tblData = gobjDB.fncGetMemberMain(intMemID)
            If tblData Is Nothing Then Exit Function

            With tblData.Rows(0)

                'get date
                'intBday = basCommon.fncCnvToInt(.Item("BIR_DAY_SUN"))
                'intBmon = basCommon.fncCnvToInt(.Item("BIR_MON_SUN"))
                'intByea = basCommon.fncCnvToInt(.Item("BIR_YEA_SUN"))
                'intDday = basCommon.fncCnvToInt(.Item("DEA_DAY_LUNAR"))
                'intDmon = basCommon.fncCnvToInt(.Item("DEA_MON_LUNAR"))
                'intDyea = basCommon.fncCnvToInt(.Item("DEA_YEA_LUNAR"))

                intBday = basCommon.fncCnvToInt(.Item("BIR_DAY"))
                intBmon = basCommon.fncCnvToInt(.Item("BIR_MON"))
                intByea = basCommon.fncCnvToInt(.Item("BIR_YEA"))

                intBDayMoon = basCommon.fncCnvToInt(.Item("BIR_DAY_LUNAR"))
                intBMonMoon = basCommon.fncCnvToInt(.Item("BIR_MON_LUNAR"))
                intBYeaMoon = basCommon.fncCnvToInt(.Item("BIR_YEA_LUNAR"))


                intDday = basCommon.fncCnvToInt(.Item("DEA_DAY"))
                intDmon = basCommon.fncCnvToInt(.Item("DEA_MON"))
                intDyea = basCommon.fncCnvToInt(.Item("DEA_YEA"))

                intDDaySun = basCommon.fncCnvToInt(.Item("DEA_DAY_SUN"))
                intDMonSun = basCommon.fncCnvToInt(.Item("DEA_MON_SUN"))
                intDYeaSun = basCommon.fncCnvToInt(.Item("DEA_YEA_SUN"))

                strFName = basCommon.fncCnvNullToString(.Item("FIRST_NAME"))
                strMName = basCommon.fncCnvNullToString(.Item("MIDDLE_NAME"))
                strLName = basCommon.fncCnvNullToString(.Item("LAST_NAME"))
                intNation = basCommon.fncCnvToInt(.Item("NATIONALITY"))
                intReligion = basCommon.fncCnvToInt(.Item("RELIGION"))
                intGender = basCommon.fncCnvToInt(.Item("GENDER"))


                dtRow("MEMBER_ID") = basCommon.fncCnvNullToString(.Item("MEMBER_ID"))
                dtRow("MEMBER_NAME") = basCommon.fncGetFullName(strFName, strMName, strLName, "")
                dtRow("ALIAS_NAME") = basCommon.fncCnvNullToString(.Item("ALIAS_NAME"))
                dtRow("FAMILY_ORDER") = basCommon.fncCnvNullToString(.Item("FAMILY_ORDER"))
                'dtRow("BIRTH_DAY") = basCommon.fncCnvNullToString(.Item("BIRTH_DAY"))
                dtRow("BIRTH_DAY") = basCommon.fncGetDateName("", intBday, intBmon, intByea, False)
                '2017/02/25 Manh change
                dtRow("BIRTH_DAY_LUNAR") = basCommon.fncGetDateName("", intBDayMoon, intBMonMoon, intBYeaMoon, False, True) 'basCommon.fncGetSolar2LunarDateName("", intBday, intBmon, intByea)

                dtRow("GENDER") = "Nam"
                If intGender = clsEnum.emGender.FEMALE Then dtRow("GENDER") = "Nữ"

                dtRow("BIRTH_PLACE") = basCommon.fncCnvNullToString(.Item("BIRTH_PLACE"))
                dtRow("NATIONALITY") = basCommon.fncGetNationName(intNation)
                dtRow("RELIGION") = basCommon.fncGetReligionName(intReligion)
                'dtRow("DECEASED") = basCommon.fncCnvNullToString(.Item("DECEASED"))
                'dtRow("DECEASED_DATE") = basCommon.fncCnvNullToString(.Item("DECEASED_DATE"))
                dtRow("BURY_PLACE") = basCommon.fncCnvNullToString(.Item("BURY_PLACE"))

                strAvatar = basCommon.fncCnvNullToString(.Item("AVATAR_PATH"))
                If Not System.IO.File.Exists(My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & strAvatar) Then
                    dtRow("AVATAR_PATH") = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & gcstrNoAvatar
                    'dtRow("AVATAR_PATH") = gcstrNoAvatar
                Else
                    dtRow("AVATAR_PATH") = My.Application.Info.DirectoryPath & basConst.gcstrImageFolder & basConst.gcstrAvatarPath & strAvatar
                    'dtRow("AVATAR_PATH") = strAvatar
                End If

                dtRow("FAMILY_ORDER") = basCommon.fncCnvNullToString(.Item("FAMILY_ORDER"))
                dtRow("MAIN_REMARK") = basCommon.fncCnvRtfToText(basCommon.fncCnvNullToString(.Item("T_FMEMBER_MAIN.REMARK")))
                dtRow("HOMETOWN") = basCommon.fncCnvNullToString(.Item("HOMETOWN"))
                dtRow("HOME_ADD") = basCommon.fncCnvNullToString(.Item("HOME_ADD"))
                dtRow("PHONENUM1") = basCommon.fncCnvNullToString(.Item("PHONENUM1"))
                dtRow("PHONENUM2") = basCommon.fncCnvNullToString(.Item("PHONENUM2"))
                dtRow("MAIL_ADD1") = basCommon.fncCnvNullToString(.Item("MAIL_ADD1"))
                dtRow("MAIL_ADD2") = basCommon.fncCnvNullToString(.Item("MAIL_ADD2"))
                dtRow("FAXNUM") = basCommon.fncCnvNullToString(.Item("FAXNUM"))
                dtRow("URL") = basCommon.fncCnvNullToString(.Item("URL"))
                dtRow("IMNICK") = basCommon.fncCnvNullToString(.Item("IMNICK"))
                dtRow("CONTACT_REMARK") = basCommon.fncCnvNullToString(.Item("T_FMEMBER_CONTACT.REMARK"))

                intCareerType = basCommon.fncCnvToInt(.Item("CAREER_TYPE"))
                intEduType = basCommon.fncCnvToInt(.Item("EDUCATION_TYPE"))
                intFactType = basCommon.fncCnvToInt(.Item("FACT_TYPE"))

                If intCareerType = clsEnum.emInputType.GENERAL Then
                    dtRow("CAREER") = basCommon.fncCnvNullToString(.Item("CAREER"))
                Else
                    dtRow("CAREER") = xGetCareer(intMemID)
                End If

                If intEduType = clsEnum.emInputType.GENERAL Then
                    dtRow("EDUCATION") = basCommon.fncCnvNullToString(.Item("EDUCATION"))
                Else
                    dtRow("EDUCATION") = xGetEducation(intMemID) ' & vbCrLf & vbCrLf & xGetCareer(intMemID)
                End If

                If intFactType = clsEnum.emInputType.GENERAL Then
                    dtRow("FACT") = basCommon.fncCnvNullToString(.Item("FACT"))
                Else
                    dtRow("FACT") = xGetFact(intMemID)
                End If

                'dtRow("FAMILY") = xGetFamily(intMemID)
                Dim intLevel As Integer = basCommon.fncCnvToInt(.Item("LEVEL"))

                dtRow("GENERATION") = IIf(intLevel = 0, "chưa rõ", intLevel)
                If intGeneration > 0 Then dtRow("GENERATION") = intGeneration

                Dim strFather As String = ""
                Dim strMother As String = ""

                fncGetFaMoName(intMemID, strFather, strMother)

                dtRow("FATHER") = strFather
                dtRow("MOTHER") = strMother

                'Manh 2017 08 31
                dtRow("BROS") = xGetBros2Word(intMemID)

                '2017/02/25 Manh change
                dtRow("DECEASED_DATE_SUN") = basCommon.fncGetDateName("", intDDaySun, intDMonSun, intDYeaSun, False) 'basCommon.fncGetLunar2SolarDateName("", intDday, intDmon, intDyea)
                dtRow("DECEASED_DATE_LUNAR") = basCommon.fncGetDateName("", intDday, intDmon, intDyea, False, True)
                dtRow("SPOUSE") = xGetHuWi(intMemID)
                Dim strWH As String = ""
                dtRow("CHILDREN") = xGetKids_new(intMemID, intGender, strWH)
                dtRow("SPOUSE") = strWH

            End With

            Return True

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetMainInfo", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Get member's education infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetEducation(ByVal intMemID As Integer) As String

        xGetEducation = ""

        Dim tblData As DataTable = Nothing

        Try
            Dim stStart As stCalendar
            Dim stEnd As stCalendar
            Dim strStart As String
            Dim strEnd As String
            Dim strOfficeName As String
            Dim strRemark As String

            tblData = gobjDB.fncGetCareer(clsEnum.emCareerType.EDU, intMemID)

            If tblData Is Nothing Then Exit Function

            With tblData.Rows
                For i As Integer = 0 To tblData.Rows.Count - 1

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_DAY")), stStart.intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_MON")), stStart.intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_YEA")), stStart.intYear)

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_DAY")), stEnd.intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_MON")), stEnd.intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_YEA")), stEnd.intYear)

                    strOfficeName = basCommon.fncCnvNullToString(.Item(i)("OFFICE_NAME"))
                    strRemark = basCommon.fncCnvNullToString(.Item(i)("REMARK"))

                    strStart = "----"
                    strEnd = "----"
                    'If dtStart > Date.MinValue And dtStart < Date.MaxValue Then strStart = dtStart.ToShortDateString
                    'If dtEnd > Date.MinValue And dtEnd < Date.MaxValue Then strEnd = dtEnd.ToShortDateString

                    strStart = basCommon.fncGetDateName(strStart, stStart, True, False)
                    strEnd = basCommon.fncGetDateName(strEnd, stEnd, True, False)

                    xGetEducation &= String.Format(mcstrEduFormat, strOfficeName, strStart, strEnd, strRemark)

                    If (i < tblData.Rows.Count - 1) Then

                        xGetEducation &= vbCrLf

                    End If
                Next
            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetEducation", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Get member's career infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetCareer(ByVal intMemID As Integer) As String

        xGetCareer = ""

        Dim tblData As DataTable = Nothing

        Try
            Dim stStart As stCalendar
            Dim stEnd As stCalendar
            Dim strStart As String
            Dim strEnd As String
            Dim strOfficeName As String
            Dim strOccupation As String
            Dim strPosition As String

            tblData = gobjDB.fncGetCareer(clsEnum.emCareerType.CAREER, intMemID)

            If tblData Is Nothing Then Exit Function

            With tblData.Rows
                For i As Integer = 0 To tblData.Rows.Count - 1

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_DAY")), stStart.intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_MON")), stStart.intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_YEA")), stStart.intYear)

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_DAY")), stEnd.intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_MON")), stEnd.intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_YEA")), stEnd.intYear)

                    strOfficeName = basCommon.fncCnvNullToString(.Item(i)("OFFICE_NAME"))
                    strOccupation = basCommon.fncCnvNullToString(.Item(i)("OCCUPATION"))
                    strPosition = basCommon.fncCnvNullToString(.Item(i)("POSITION"))

                    strStart = "----"
                    strEnd = "----"
                    'If dtStart > Date.MinValue And dtStart < Date.MaxValue Then strStart = dtStart.ToShortDateString
                    'If dtEnd > Date.MinValue And dtEnd < Date.MaxValue Then strEnd = dtEnd.ToShortDateString

                    strStart = basCommon.fncGetDateName(strStart, stStart, True, False)
                    strEnd = basCommon.fncGetDateName(strEnd, stEnd, True, False)

                    xGetCareer &= String.Format(mcstrCareerFormat, strOfficeName, strStart, strEnd, strOccupation, strPosition)

                    If (i < tblData.Rows.Count - 1) Then

                        xGetCareer &= vbCrLf

                    End If
                Next
            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetEducation", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Get member' fact infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetFact(ByVal intMemID As Integer) As String

        xGetFact = ""

        Dim tblData As DataTable = Nothing

        Try
            Dim stStart As stCalendar
            Dim stEnd As stCalendar
            'Dim dtStart As Date
            'Dim dtEnd As Date
            Dim strStart As String
            Dim strEnd As String
            Dim strName As String
            Dim strPlace As String
            Dim strDesc As String

            tblData = gobjDB.fncGetFact(intMemID)

            If tblData Is Nothing Then Exit Function

            With tblData.Rows
                For i As Integer = 0 To tblData.Rows.Count - 1

                    'Date.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_DATE")), dtStart)
                    'Date.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_DATE")), dtEnd)

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_DAY")), stStart.intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_MON")), stStart.intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("START_YEA")), stStart.intYear)

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_DAY")), stEnd.intDay)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_MON")), stEnd.intMonth)
                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("END_YEA")), stEnd.intYear)

                    strName = basCommon.fncCnvNullToString(.Item(i)("FACT_NAME"))
                    strPlace = basCommon.fncCnvNullToString(.Item(i)("FACT_PLACE"))
                    strDesc = basCommon.fncCnvNullToString(.Item(i)("DESCRIPTION"))

                    strStart = "----"
                    strEnd = "----"
                    'If dtStart > Date.MinValue And dtStart < Date.MaxValue Then strStart = dtStart.ToShortDateString
                    'If dtEnd > Date.MinValue And dtEnd < Date.MaxValue Then strEnd = dtEnd.ToShortDateString

                    strStart = basCommon.fncGetDateName(strStart, stStart, True, False)
                    strEnd = basCommon.fncGetDateName(strEnd, stEnd, True, False)

                    xGetFact &= String.Format(mcstrFactFormat, strName, strStart, strEnd, strPlace, strDesc)

                    If (i < tblData.Rows.Count - 1) Then

                        xGetFact &= vbCrLf

                    End If

                Next
            End With

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetEducation", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Get member's family infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetFamily(ByVal intMemID As Integer) As String

        xGetFamily = ""

        Try
            'xGetFamily &= xGetFaMo(intMemID) & vbCrLf
            'xGetFamily &= xGetBros(intMemID) & vbCrLf
            'xGetFamily &= xGetHuWi(intMemID) & vbCrLf
            'xGetFamily &= xGetKids(intMemID) & vbCrLf

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetFamily", ex)
        Finally
        End Try

    End Function

    ''' <summary>
    ''' Get member' brother/sister infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetBros(ByVal intMemID As Integer) As String

        xGetBros = ""

        Dim tblData As DataTable = Nothing

        Try
            Dim intFaID As Integer
            Dim intMoID As Integer
            Dim intTemp As Integer

            'get father id
            basCommon.fncGetFaMoID(intMemID, intFaID, intMoID)

            'get list of bros
            tblData = basCommon.fncGetKids(intFaID, intMoID)
            If tblData Is Nothing Then Exit Function

            xGetBros = "Anh chị em : " & vbTab

            'concats name of each kids
            With tblData.Rows
                For i As Integer = 0 To tblData.Rows.Count - 1

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("MEMBER_ID")), intTemp)
                    xGetBros &= basCommon.fncGetMemberName(intTemp) & ", "

                Next
            End With

            'remove the last ","
            xGetBros = xGetBros.Substring(0, xGetBros.Length - 2)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetBros", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function

    ''' <summary>
    ''' Get member' brother/sister infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetBros2Word(ByVal intMemID As Integer) As String

        xGetBros2Word = ""

        Dim tblData As DataTable = Nothing
        Dim strRet As String = ""

        Try
            Dim intFaID As Integer
            Dim intMoID As Integer
            Dim intTemp As Integer
            Dim intMemberIndex As Integer
            Dim strName As String
            Dim intGender As Integer

            'get father id
            basCommon.fncGetFaMoID(intMemID, intFaID, intMoID)

            'get list of bros
            tblData = basCommon.fncGetKids(intFaID, intMoID)

            If tblData Is Nothing Then Exit Function

            strRet = ""

            'concats name of each kids
            intMemberIndex = 9999
            With tblData.Rows
                For i As Integer = 0 To tblData.Rows.Count - 1

                    Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("MEMBER_ID")), intTemp)

                    strName = fncGetFullName(.Item(i)("FIRST_NAME"), .Item(i)("MIDDLE_NAME"), .Item(i)("LAST_NAME"), .Item(i)("ALIAS_NAME"))

                    If intTemp <> intMemID Then
                        Integer.TryParse(basCommon.fncCnvNullToString(.Item(i)("GENDER")), intGender)

                        If (i < intMemberIndex) Then
                            If (intGender = clsEnum.emGender.MALE) Then
                                strName = "Anh " & strName
                            Else
                                If (intGender = clsEnum.emGender.FEMALE) Then strName = "Chị " & strName
                            End If
                        Else

                            If (intGender = clsEnum.emGender.MALE) Then
                                strName = "Em trai " & strName
                            Else
                                If (intGender = clsEnum.emGender.FEMALE) Then
                                    strName = "Em gái " & strName
                                Else
                                    strName = "Em " & strName
                                End If
                            End If



                        End If

                        'xGetBros2Word &= basCommon.fncGetMemberName(intTemp) & ", "
                        strRet &= strName

                        If (i < tblData.Rows.Count - 1) Then

                            strRet &= vbCrLf

                        End If

                    Else

                        intMemberIndex = i

                    End If

                Next
            End With

            'remove the last ","
            'xGetBros2Word = strRet.Substring(0, strRet.Length - 2)
            Return strRet

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetBros", ex)
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
            tblData = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Get member' husband/wife infor
    ''' </summary>
    ''' <param name="intMemID">member id</param>
    ''' <returns>true - success; false - fail</returns>
    ''' <remarks></remarks>
    Private Function xGetHuWi(ByVal intMemID As Integer) As String

        xGetHuWi = ""

        Dim objDict As Dictionary(Of Integer, String) = Nothing

        Try
            'get list of husband/wife
            objDict = basCommon.fncGetHusWifeList(intMemID)

            If objDict Is Nothing Then Exit Function
            If objDict.Count <= 0 Then Exit Function

            'xGetHuWi = "Vợ / chồng : " & vbTab

            For Each element As KeyValuePair(Of Integer, String) In objDict

                xGetHuWi &= basCommon.fncCnvNullToString(element.Value) & vbCrLf  '", "

            Next

            xGetHuWi = xGetHuWi.Substring(0, xGetHuWi.Length - 2)

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetHuWi", ex)
        Finally
            If objDict IsNot Nothing Then objDict.Clear()
            objDict = Nothing
        End Try

    End Function

    Private Function xGetKids_new(ByVal intMemID As Integer, ByVal intGender As Integer, ByRef strHusWife As String) As String

        xGetKids_new = ""

        Dim tblKid As DataTable = Nothing
        Dim objHusWif As Dictionary(Of Integer, String) = Nothing
        Dim strHW As String = ""
        Try
            ' ▽ 2012/11/28   AKB Quyet （変更内容）*********************************
            ''get list of bros
            'lstKid = basCommon.fncGetKidList(intMemID)
            'If lstKid Is Nothing Then Exit Function
            'If lstKid.Count <= 0 Then Exit Function

            'xGetKids = "Con : " & vbTab & vbTab

            ''concats name of each kids
            'For i As Integer = 0 To lstKid.Count - 1

            '    xGetKids &= basCommon.fncGetMemberName(lstKid(i)) & ", "

            'Next

            ''remove the last ","
            'xGetKids = xGetKids.Substring(0, xGetKids.Length - 2)

            Dim intKidId As Integer

            objHusWif = basCommon.fncGetHusWifeList(intMemID)

            For Each element As KeyValuePair(Of Integer, String) In objHusWif
                If strHW = "" Then
                    strHW = basCommon.fncGetMemberName(element.Key)
                Else
                    strHW = strHW & vbCrLf & basCommon.fncGetMemberName(element.Key)
                End If

                'If intGender = clsEnum.emGender.MALE Then
                '    xGetKids_new &= String.Format("Con bà {0} : " & vbCrLf, element.Value)
                'ElseIf intGender = clsEnum.emGender.FEMALE Then
                '    xGetKids_new &= String.Format("Con ông {0} : " & vbCrLf, element.Value)
                'Else
                '    xGetKids_new &= String.Format("Con thành viên {0} : " & vbCrLf, element.Value)
                'End If

                tblKid = basCommon.fncGetKids(intMemID, element.Key)
                If tblKid Is Nothing Then
                    'xGetKids_new &= vbTab & "Không có con." & vbCrLf
                    xGetKids_new &= "Không có con." '& vbCrLf
                    Continue For
                End If

                For i As Integer = 0 To tblKid.Rows.Count - 1

                    intKidId = basCommon.fncCnvToInt(tblKid.Rows(i)("MEMBER_ID"))
                    xGetKids_new &= basCommon.fncGetMemberName(intKidId)

                    If (i < tblKid.Rows.Count - 1) Then

                        xGetKids_new &= vbCrLf
                        strHW = strHW & vbCrLf

                    End If
                Next

            Next
            strHusWife = strHW
        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetKids", ex)
        Finally
            If tblKid IsNot Nothing Then tblKid.Dispose()
            tblKid = Nothing
            objHusWif = Nothing
        End Try

    End Function

    Private Function xGetKids(ByVal intMemID As Integer, ByVal intGender As Integer) As String

        xGetKids = ""

        Dim tblKid As DataTable = Nothing
        Dim objHusWif As Dictionary(Of Integer, String) = Nothing

        Try
            ' ▽ 2012/11/28   AKB Quyet （変更内容）*********************************
            ''get list of bros
            'lstKid = basCommon.fncGetKidList(intMemID)
            'If lstKid Is Nothing Then Exit Function
            'If lstKid.Count <= 0 Then Exit Function

            'xGetKids = "Con : " & vbTab & vbTab

            ''concats name of each kids
            'For i As Integer = 0 To lstKid.Count - 1

            '    xGetKids &= basCommon.fncGetMemberName(lstKid(i)) & ", "

            'Next

            ''remove the last ","
            'xGetKids = xGetKids.Substring(0, xGetKids.Length - 2)

            Dim intKidId As Integer

            objHusWif = basCommon.fncGetHusWifeList(intMemID)

            For Each element As KeyValuePair(Of Integer, String) In objHusWif

                If intGender = clsEnum.emGender.MALE Then
                    xGetKids &= String.Format("Con bà {0} : " & vbCrLf, element.Value)
                ElseIf intGender = clsEnum.emGender.FEMALE Then
                    xGetKids &= String.Format("Con ông {0} : " & vbCrLf, element.Value)
                Else
                    xGetKids &= String.Format("Con thành viên {0} : " & vbCrLf, element.Value)
                End If

                tblKid = basCommon.fncGetKids(intMemID, element.Key)
                If tblKid Is Nothing Then
                    xGetKids &= vbTab & "Không có con." & vbCrLf
                    Continue For
                End If

                For i As Integer = 0 To tblKid.Rows.Count - 1
                    intKidId = basCommon.fncCnvToInt(tblKid.Rows(i)("MEMBER_ID"))
                    xGetKids &= vbTab & basCommon.fncGetMemberName(intKidId) & vbCrLf
                Next

            Next

            ' △ 2012/11/28   AKB Quyet *********************************************

        Catch ex As Exception
            basCommon.fncSaveErr(mcstrClsName, "xGetKids", ex)
        Finally
            If tblKid IsNot Nothing Then tblKid.Dispose()
            tblKid = Nothing
            objHusWif = Nothing
        End Try

    End Function




#End Region

End Class