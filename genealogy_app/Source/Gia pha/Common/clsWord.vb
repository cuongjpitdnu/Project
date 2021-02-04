'   ******************************************************************
'      TITLE      : WORD  FUNCTIONS
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2012/03/23　AKB 
'      UPDATE     : 
'
'           2012 AKB SOFTWARE
'   ******************************************************************

Option Explicit On
Option Strict Off


Imports System.IO
Imports Config_Gia_Pha

Public Class clsWord

    Private Enum emWdUnits
        wdCell  'A cell.
        wdCharacter 'A character.
        wdCharacterFormatting   'Character formatting.
        wdColumn    'A column.
        wdItem  'The selected item.
        wdLine  'A line.
        wdParagraph 'A paragraph.
        wdParagraphFormatting   'Paragraph formatting.
        wdRow   'A row.
        wdScreen    'The screen dimensions.
        wdSection   'A section.
        wdSentence  'A sentence.
        wdStory 'A story.
        wdTable 'A table.
        wdWindow    'A window.
        wdWord  'A word.

    End Enum

    Private Enum emWdGoToItem
        wdGoToSection = 0
        wdGoToPage = 1
    End Enum

    Private Enum emWdGoToDirection

        wdGoToFirst = 1

    End Enum

    Private Enum emWdBreakType

        wdPageBreak = 7

    End Enum

    Private Enum emWdRecoveryType

        wdPasteDefault = 0

    End Enum

    'https://docs.microsoft.com/en-us/dotnet/api/microsoft.office.interop.word.wdsaveformat?view=word-pia
    Private Enum WdSaveFormat
        wdFormatDocumentDefault = 16
    End Enum

    Private mobjWordApp As Object = Nothing
    Private mobjDoc As Object = Nothing

    '   ****************************************************************** 
    '      FUNCTION   : Open
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : obj Object, object to clear
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB  
    '      UPDATE     :  
    '   ******************************************************************
    Public Function Open(ByVal strFile As String) As Boolean

        Try
            'Create new Word file
            mobjWordApp = CreateObject("Word.Application")
            If strFile = "" Then
                mobjDoc = mobjWordApp.Documents.Add
            Else

                If System.IO.File.Exists(strFile) Then

                    ''mobjDoc = mobjWordApp.Documents.Open(strFile, PasswordDocument:=basConst.gcstrXltPass)
                    'mobjDoc = mobjWordApp.Documents.Add(strFile)
                    Dim strTempPath As String = Path.GetTempPath() & DateTime.Now.ToString("yyyyMMddHHmmss") & ".doc"
                    File.Copy(strFile, strTempPath)
                    mobjDoc = mobjWordApp.Documents.Open(strTempPath)

                Else

                    MsgBox("File Template(" & strFile & ")không tồn tại. Hãy kiểm tra lại.")

                    Return False

                End If

            End If

            'mobjWordApp.Visible = True
            mobjWordApp.Visible = False
            mobjWordApp.ScreenUpdating = False
            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncSetTableCellVal
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : obj Object, object to clear
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB  
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncSetTableCellVal(ByVal intTableIndex As Integer, ByVal intRow As Integer,
                                       ByVal intCell As Integer, ByVal strData As String) As Boolean

        fncSetTableCellVal = False
        Dim objTable As Object = Nothing

        Try

            objTable = mobjDoc.Tables(intTableIndex)
            If Not IsNothing(objTable.Cell(intRow, intCell)) Then
                objTable.Cell(intRow, intCell).Range.Text = strData
            End If


            fncSetTableCellVal = True

        Catch ex As Exception
            Throw ex

        Finally

            fncReleaseObject(objTable)

        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncSetTableCellImg
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : obj Object, object to clear
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB  
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncSetTableCellImg(ByVal intTableIndex As Integer, ByVal intRow As Integer,
                                       ByVal intCell As Integer, ByVal StrFile As String,
                                       Optional ByVal intHeight As Integer = 0,
                                       Optional ByVal intWidth As Integer = 0,
                                       Optional ByVal blnDelAll As Boolean = True) As Boolean

        fncSetTableCellImg = False
        Dim objTable As Object = Nothing
        Dim objPic As Object = Nothing
        Try

            objTable = mobjDoc.Tables(intTableIndex)

            If blnDelAll = True Then

                If Not IsNothing(objTable.Cell(intRow, intCell).Range.InlineShapes) Then
                    For Each shp As Object In objTable.Cell(intRow, intCell).Range.InlineShapes
                        shp.Delete()
                        fncReleaseObject(shp)
                    Next
                End If

            End If

            If System.IO.File.Exists(StrFile) Then

                objPic = objTable.Cell(intRow, intCell).Range.InlineShapes.AddPicture(FileName:=StrFile)

                If intHeight <> 0 Then
                    objPic.Height = intHeight
                End If

                If intWidth <> 0 Then
                    objPic.Width = intWidth
                End If

            End If

            fncSetTableCellImg = True

        Catch ex As Exception
            Throw ex

        Finally
            fncReleaseObject(objPic)
            fncReleaseObject(objTable)

        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncCopyTable
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : obj Object, object to clear
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB  
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncCopyTable(ByVal intCopyTableIndex As Integer, ByVal intLastTableIndex As Integer) As Boolean

        fncCopyTable = False
        Dim objSelect As Object = Nothing

        Try


            objSelect = mobjDoc.Tables(intCopyTableIndex) ' Selects the table 
            objSelect.Select()
            mobjWordApp.Selection.Copy()
            'mobjDoc.MoveDown(Microsoft.Office.Interop.Word.WdUnits.wdLine, 1) 'moves after the table
            'mobjDoc.Tables(intLastTableIndex).Select() ' Selects the table

            mobjWordApp.Selection.MoveDown(emWdUnits.wdLine, 2) 'moves after the table
            'mobjWordApp.Selection.MoveDown(emWdUnits.wdTable, 1) 'moves after the table
            mobjWordApp.Selection.InsertBreak(emWdBreakType.wdPageBreak) 'inserts new page
            'mobjDoc.InsertBreak(emWdBreakType.wdPageBreak) 'inserts new page
            mobjWordApp.Selection.PasteAndFormat(emWdRecoveryType.wdPasteDefault) 'pa
            'mobjDoc.PasteAndFormat(emWdRecoveryType.wdPasteDefault) 'pa



            fncCopyTable = True

        Catch ex As Exception

            Throw ex
        Finally
            fncReleaseObject(mobjWordApp.Selection)
            fncReleaseObject(objSelect)
        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncExit
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : obj Object, object to clear
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB  
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncExit(Optional ByVal blnShow As Boolean = False) As Boolean
        Try

            If blnShow Then
                If Not IsNothing(mobjWordApp) Then
                    'Selection.GoTo(What:=wdGoToHeading, Which:=wdGoToFirst)

                    mobjWordApp.Selection.GoTo(What:=emWdGoToItem.wdGoToPage, Which:=emWdGoToDirection.wdGoToFirst)

                    fncReleaseObject(mobjWordApp.Selection)
                    mobjWordApp.Visible = True
                End If
            End If

            If Not IsNothing(mobjWordApp) Then
                mobjWordApp.ScreenUpdating = True
            End If
            fncReleaseObject(mobjDoc)
            fncReleaseObject(mobjWordApp)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Function SaveAs(ByVal pathSave As String) As Boolean

        SaveAs = False

        Try

            mobjDoc.SaveAs(pathSave, WdSaveFormat.wdFormatDocumentDefault)

            SaveAs = True
        Catch ex As Exception

            Throw ex

        Finally
        End Try
    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncReleaseObject, clear object
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : obj Object, object to clear
    '      MEMO       :  
    '      CREATE     : 2011/08/12  AKB  
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncReleaseObject(ByVal obj As Object) As Boolean

        fncReleaseObject = False

        Try

            System.Runtime.InteropServices.Marshal.ReleaseComObject(obj)
            obj = Nothing

            Return True

        Catch ex As Exception
            obj = Nothing
        Finally
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
        End Try

    End Function

    ''' <summary>
    ''' Fill data to word
    ''' </summary>
    ''' <param name="tblData">data source</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncDoMailMerge(ByVal tblData As DataTable) As Boolean

        fncDoMailMerge = False

        Dim objWordApp As Object = Nothing
        Dim myDataDoc As Object = Nothing
        Dim myTable As Object = Nothing
        Dim objSqlDataSet As DataSet = Nothing

        Try
            Dim strTempData As String
            Dim strTemplate As String

            'Dim [Nothing] As Object = System.Reflection.Missing.Value
            objSqlDataSet = New DataSet()

            'objSqlDataSet.Tables.Add(createDataTabeSource())
            objSqlDataSet.Tables.Add(tblData)

            'Save file datacesource
            objWordApp = CreateObject("Word.Application")
            myDataDoc = objWordApp.Documents.Add

            myTable = myDataDoc.Tables.Add(objWordApp.ActiveDocument.Range(0, 0), 1, 36)

            'add table header
            myTable.Rows(1).Cells(1).Range.Text = "MEMBER_ID"
            myTable.Rows(1).Cells(2).Range.Text = "MEMBER_NAME"
            myTable.Rows(1).Cells(3).Range.Text = "ALIAS_NAME"
            myTable.Rows(1).Cells(4).Range.Text = "BIRTH_DAY"
            myTable.Rows(1).Cells(5).Range.Text = "GENDER"
            myTable.Rows(1).Cells(6).Range.Text = "BIRTH_PLACE"
            myTable.Rows(1).Cells(7).Range.Text = "NATIONALITY"
            myTable.Rows(1).Cells(8).Range.Text = "RELIGION"
            'myTable.Rows(1).Cells(9).Range.Text = "DECEASED"
            'myTable.Rows(1).Cells(10).Range.Text = "DECEASED_DATE"
            myTable.Rows(1).Cells(11).Range.Text = "BURY_PLACE"
            myTable.Rows(1).Cells(12).Range.Text = "AVATAR_PATH"
            myTable.Rows(1).Cells(13).Range.Text = "FAMILY_ORDER"
            myTable.Rows(1).Cells(14).Range.Text = "MAIN_REMARK"
            myTable.Rows(1).Cells(15).Range.Text = "HOMETOWN"
            myTable.Rows(1).Cells(16).Range.Text = "HOME_ADD"
            myTable.Rows(1).Cells(17).Range.Text = "PHONENUM1"
            myTable.Rows(1).Cells(18).Range.Text = "PHONENUM2"
            myTable.Rows(1).Cells(19).Range.Text = "MAIL_ADD1"
            myTable.Rows(1).Cells(20).Range.Text = "MAIL_ADD2"
            myTable.Rows(1).Cells(21).Range.Text = "FAXNUM"
            myTable.Rows(1).Cells(22).Range.Text = "URL"
            myTable.Rows(1).Cells(23).Range.Text = "IMNICK"
            myTable.Rows(1).Cells(24).Range.Text = "CONTACT_REMARK"
            myTable.Rows(1).Cells(25).Range.Text = "EDUCATION"
            myTable.Rows(1).Cells(26).Range.Text = "CAREER"
            myTable.Rows(1).Cells(27).Range.Text = "FACT"
            myTable.Rows(1).Cells(28).Range.Text = "FAMILY"
            myTable.Rows(1).Cells(29).Range.Text = "GENERATION"

            myTable.Rows(1).Cells(30).Range.Text = "BIRTH_DAY_LUNAR"
            myTable.Rows(1).Cells(31).Range.Text = "FATHER"
            myTable.Rows(1).Cells(32).Range.Text = "MOTHER"
            myTable.Rows(1).Cells(33).Range.Text = "DECEASED_DATE_SUN"
            myTable.Rows(1).Cells(34).Range.Text = "DECEASED_DATE_LUNAR"
            myTable.Rows(1).Cells(35).Range.Text = "SPOUSE"
            myTable.Rows(1).Cells(36).Range.Text = "CHILDREN"


            'For Each myRow As DataRow In objSqlDataSet.Tables("mydata").Rows
            For Each myRow As DataRow In tblData.Rows
                Dim wRow As Object = myTable.Rows.Add()
                wRow.Cells(1).Range.Text = myRow.Item("MEMBER_ID")
                wRow.Cells(2).Range.Text = myRow.Item("MEMBER_NAME")
                wRow.Cells(3).Range.Text = myRow.Item("ALIAS_NAME")
                wRow.Cells(4).Range.Text = myRow.Item("BIRTH_DAY")
                wRow.Cells(5).Range.Text = myRow.Item("GENDER")
                wRow.Cells(6).Range.Text = myRow.Item("BIRTH_PLACE")
                wRow.Cells(7).Range.Text = myRow.Item("NATIONALITY")
                wRow.Cells(8).Range.Text = myRow.Item("RELIGION")
                'wRow.Cells(9).Range.Text = myRow.Item("DECEASED")
                'wRow.Cells(10).Range.Text = myRow.Item("DECEASED_DATE")
                wRow.Cells(11).Range.Text = myRow.Item("BURY_PLACE")
                wRow.Cells(12).Range.Text = myRow.Item("AVATAR_PATH")
                wRow.Cells(13).Range.Text = myRow.Item("FAMILY_ORDER")
                wRow.Cells(14).Range.Text = myRow.Item("MAIN_REMARK")
                wRow.Cells(15).Range.Text = myRow.Item("HOMETOWN")
                wRow.Cells(16).Range.Text = myRow.Item("HOME_ADD")
                wRow.Cells(17).Range.Text = myRow.Item("PHONENUM1")
                wRow.Cells(18).Range.Text = myRow.Item("PHONENUM2")
                wRow.Cells(19).Range.Text = myRow.Item("MAIL_ADD1")
                wRow.Cells(20).Range.Text = myRow.Item("MAIL_ADD2")
                wRow.Cells(21).Range.Text = myRow.Item("FAXNUM")
                wRow.Cells(22).Range.Text = myRow.Item("URL")
                wRow.Cells(23).Range.Text = myRow.Item("IMNICK")
                wRow.Cells(24).Range.Text = myRow.Item("CONTACT_REMARK")
                wRow.Cells(25).Range.Text = myRow.Item("EDUCATION")
                wRow.Cells(26).Range.Text = myRow.Item("CAREER")
                wRow.Cells(27).Range.Text = myRow.Item("FACT")
                'wRow.Cells(28).Range.Text = myRow.Item("FAMILY")
                wRow.Cells(29).Range.Text = myRow.Item("GENERATION")

                wRow.Cells(30).Range.Text = myRow.Item("BIRTH_DAY_LUNAR")
                wRow.Cells(31).Range.Text = myRow.Item("FATHER")
                wRow.Cells(32).Range.Text = myRow.Item("MOTHER")
                wRow.Cells(33).Range.Text = myRow.Item("DECEASED_DATE_SUN")
                wRow.Cells(34).Range.Text = myRow.Item("DECEASED_DATE_LUNAR")
                wRow.Cells(35).Range.Text = myRow.Item("SPOUSE")
                wRow.Cells(36).Range.Text = myRow.Item("CHILDREN")

            Next

            strTempData = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrDocTempData
            strTemplate = My.Application.Info.DirectoryPath & basConst.gcstrDocsFolder & basConst.gcstrDocTemplate

            'myDataDoc.SaveAs("C:\WordData.doc") 'Save file Database 
            myDataDoc.SaveAs(strTempData) 'Save file Database 
            myDataDoc.Close()

            Dim myMailMergeDoc As Object
            myMailMergeDoc = objWordApp.Documents.Open(strTemplate, PasswordDocument:=Config.Decrypt(basConst.gcstrXltPass))

            myMailMergeDoc.MailMerge.OpenDataSource(Name:=strTempData)

            ' ▽ 2012/11/14   AKB Quyet （変更内容）*********************************
            'Dim _BuiltInProperties As Object = myMailMergeDoc.BuiltInDocumentProperties
            'If Not _BuiltInProperties Is Nothing Then
            '    _BuiltInProperties("Title").Value = "Thông tin thành viên"
            '    _BuiltInProperties("Subject").Value = txtSubject.Text.Trim()
            '    _BuiltInProperties("Author").Value = txtAuthor.Text.Trim()
            '    _BuiltInProperties("Manager").Value = txtManager.Text.Trim()
            '    _BuiltInProperties("Company").Value = txtCompany.Text.Trim()
            '    _BuiltInProperties("Category").Value = txtCategory.Text.Trim()
            '    _BuiltInProperties("Keywords").Value = txtKeyWords.Text.Trim()
            '    _BuiltInProperties("Comments").Value = txtComment.Text.Trim()
            'End If
            ' △ 2012/11/14   AKB Quyet *********************************************

            myMailMergeDoc.MailMerge.Execute()
            myMailMergeDoc.Close() 'Close file template
            myMailMergeDoc = Nothing

            objWordApp.Visible = True

            System.IO.File.Delete(strTempData) 'Delete file Datasource

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            basCommon.fncReleaseObject(objWordApp)
            objWordApp = Nothing
            myDataDoc = Nothing
        End Try
    End Function

End Class