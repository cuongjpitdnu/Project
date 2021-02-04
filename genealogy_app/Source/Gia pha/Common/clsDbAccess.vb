'   ****************************************************************** 
'      TITLE      : Database access 
'　　　FUNCTION   :  
'      MEMO       :  
'      CREATE     : 2011/07/14　AKB　Quyet 
'      UPDATE     :  
' 
'           2011 AKB SOFTWARE 
'   ******************************************************************

Option Explicit On
Option Strict On

Imports GiaPha.KBS_COMMON_DB
Imports System.Text
Imports Config_Gia_Pha

'   ****************************************************************** 
'　　　FUNCTION   : DB Access class 
'      MEMO       :  
'      CREATE     : 2011/07/15　AKB　Quyet 
'      UPDATE     :  
'   ******************************************************************
Public Class clsDbAccess
    Inherits clsDbCore

#Region "Structures"


    '   ******************************************************************
    '　　　FUNCTION   : User's information Structure
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stUserInfo
        Dim intUserID As Integer            'user id
        Dim strName As String               'username
        Dim strPass As String               'password
        Dim dtUpdateTime As Date            'update time
    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : Member's main information
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stMemberInfoMain

        Dim intID As Integer                    'member id - not null
        Dim strLastName As String               'last name - not null
        Dim strMidName As String                'middle name
        Dim strFirstName As String              'first name - not null
        Dim strAlias As String                  'alias

        'Dim dtBirth As Date                     'birth date
        'Dim stBirth As stCalendar
        Dim stBirthSun As stCalendar
        Dim stBirthLunar As stCalendar
        'Dim intBday As Integer
        'Dim intBmon As Integer
        'Dim intByea As Integer

        Dim intGender As Integer                'gender - 1 for male, 2 for female, 3 for unknown
        Dim strHomeTown As String               'home town
        Dim strBirthPlace As String             'birth place
        Dim strNationality As String            'nationality - default VIETNAMESE
        Dim strReligion As String               'religion - default is null
        Dim intDeceased As Integer              '1 for death

        'Dim dtDeceased As Date                  'death date
        'Dim stDeath As stCalendar
        Dim stDeathSun As stCalendar
        Dim stDeathLunar As stCalendar
        'Dim intDday As Integer
        'Dim intDmon As Integer
        'Dim intDyea As Integer

        Dim strBuryPlace As String              'burry place
        Dim intFamilyOrder As Integer           'order in family
        Dim strAvatar As String                 'path to avatar
        Dim strRemark As String                 'note
        Dim dtLastUpdate As DateTime            'last update - not null

        Dim intCareerType As clsEnum.emInputType
        Dim intEduType As clsEnum.emInputType
        Dim intFactType As clsEnum.emInputType

        Dim strCareerGeneral As String
        Dim strEduGeneral As String
        Dim strFactGeneral As String


    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : Member's Contact information
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stMemberInfoContact

        Dim intID As Integer                    'member id - not null
        Dim strHometown As String               'home town
        Dim strHomeAddr As String               'home address
        Dim strPhone1 As String                 'phone number 1
        Dim strphone2 As String                 'phone number 2
        Dim strMail1 As String                  'email address 1
        Dim strMail2 As String                  'email address 2
        Dim strFax As String                    'fax number
        Dim strURL As String                    'personal URL
        Dim strIMNick As String                 'instant message nick
        Dim strRemark As String                 'remark
        Dim dtLastUpdate As DateTime            'last update

    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : Career information
    '      MEMO       : 
    '      CREATE     : 2011/07/20  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stCareer

        Dim intMemID As Integer                 'member id
        Dim intCareerID As Integer              'career id
        Dim intType As Integer                  'type id : 1 - edu / 2 - career

        'Dim dtStart As DateTime                 'start date
        Dim intSday As Integer                  'start day
        Dim intSmon As Integer                  'start month
        Dim intSyea As Integer                  'start year

        'Dim dtEnd As DateTime                   'end date
        Dim intEday As Integer                  'end day
        Dim intEmon As Integer                  'end month
        Dim intEyea As Integer                  'end year

        Dim strOccupt As String                 'opccupation
        Dim strPosition As String               'position
        Dim strOffName As String                'office name
        Dim strOffPlace As String               'office address
        Dim strRemark As String                 'remark
        Dim dtUpdate As DateTime                'last update

    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : Fact information
    '      MEMO       : 
    '      CREATE     : 2011/08/03  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stFact

        Dim intMemID As Integer                 'member id
        Dim intFactID As Integer                'fact id
        Dim strName As String                   'fact name
        Dim strPlace As String                  'place

        'Dim dtStart As DateTime                 'start date
        Dim intSday As Integer                  'start day
        Dim intSmon As Integer                  'start month
        Dim intSyea As Integer                  'start year

        'Dim dtEnd As DateTime                   'end date
        Dim intEday As Integer                  'end day
        Dim intEmon As Integer                  'end month
        Dim intEyea As Integer                  'end year

        Dim strDesc As String                   'description
        Dim dtLastUpdate As DateTime            'last update

    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : Search
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stSearch

        Dim strKeyword As String                    'key word

        Dim intGender As Integer                    'gender 0-all, 1-male, 2-female

        Dim strOccupt As String                     'career name
        Dim strPosition As String                   'position

        Dim intDie As Integer                       'deceased or not

        Dim dtBirthFrom As Date                     'from birth date
        Dim dtBirthTo As Date                       'to birth date
        Dim dtDieFrom As Date                       'from die date
        Dim dtDieTo As Date                         'to die date

        Dim intDFday As Integer                     'die from day
        Dim intDFmon As Integer
        Dim intDFyea As Integer

        Dim intDTday As Integer                     'die to day
        Dim intDTmon As Integer
        Dim intDTyea As Integer

    End Structure


    '   ******************************************************************
    '　　　FUNCTION   : Album
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Structure stAlbum

        Dim intID As Integer                    'image id
        Dim strTitle As String                  'image title
        Dim strDesc As String                   'image description
        Dim strName As String                   'image file name

    End Structure


    '   ****************************************************************** 
    '      FUNCTION   : fncGetTable, get data from a table 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : strTableName, table name
    '      MEMO       : for checking existence of a column
    '      CREATE     : 2012/04/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetTable(ByVal strTableName As String, ByVal strColName As String) As DataTable

        fncGetTable = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= " SELECT TOP 10 [" & strColName & "]"
            strSQL &= " FROM"
            strSQL &= " " & strTableName

            Try
                objDataTable = GetTable(strSQL)
            Catch ex As Exception
                Return Nothing
            End Try

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertColumn, insert a column into table
    '      VALUE      : Boolean, true - success ; false - fail
    '      PARAMS     : strTableName, table name
    '      MEMO       : for checking existence of a column
    '      CREATE     : 2012/04/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertColumn(ByVal strTableName As String,
                                    ByVal lstColName As List(Of String),
                                    ByVal lstDataType As List(Of String),
                                    Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncInsertColumn = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "ALTER TABLE"
            strSQL &= " " & strTableName

            strSQL &= " ADD"

            For i As Integer = 0 To lstColName.Count - 1

                If i > 0 Then strSQL &= " ,"

                strSQL &= " [" & lstColName(i) & "]"
                strSQL &= " " & lstDataType(i)

            Next

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncAlterColumn, alter a column into table
    '      VALUE      : Boolean, true - success ; false - fail
    '      PARAMS     : strTableName, table name
    '      MEMO       : for checking existence of a column
    '      CREATE     : 2012/04/13  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncAlterColumn(ByVal strTableName As String,
                                    ByVal lstColName As List(Of String),
                                    ByVal lstDataType As List(Of String),
                                    Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncAlterColumn = False

        Dim blnBeginTrans As Boolean = False

        Try
            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            For i As Integer = 0 To lstColName.Count - 1

                Dim strSQL As String = ""

                strSQL &= "ALTER TABLE"
                strSQL &= " " & strTableName

                strSQL &= " ALTER COLUMN"

                strSQL &= " " & lstColName(i)
                strSQL &= " " & lstDataType(i)

                Execute(strSQL)

            Next

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function

#End Region

    '   ****************************************************************** 
    '      FUNCTION   : constructor 
    '      MEMO       :  
    '      CREATE     : 2011/07/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Sub New()

        MyBase.New()

    End Sub


    '   ******************************************************************
    '      FUNCTION   : Open DB connection
    '      VALUE      : Boolean, true - success, false - failure 
    '      PARAMS     : none 
    '      MEMO       : 
    '      CREATE     : 2011/07/14  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function Open() As Boolean

        Open = False

        Try

            Open = OpenMDB(basConst.gcstrDBNAME, Config.Decrypt(basConst.gcstrDBPASS))

        Catch ex As Exception
            Throw ex
        End Try

        Exit Function

    End Function


    Public Function fncNumOfMem() As Integer

        fncNumOfMem = basConst.gcintLimitMember + 1

        Dim tblData As DataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " COUNT(MEMBER_ID)"
            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_MAIN"

            tblData = GetTable(strSQL)

            If tblData Is Nothing Then Exit Function
            If tblData.Rows.Count = 0 Then Exit Function

            Integer.TryParse(basCommon.fncCnvNullToString(tblData.Rows(0).Item(0)), fncNumOfMem)

        Catch ex As Exception
            Throw ex
        Finally
            If tblData IsNot Nothing Then tblData.Dispose()
        End Try

    End Function


#Region "User"

    '   ****************************************************************** 
    '      FUNCTION   : fncGetUser 
    '      VALUE      : DataTable, list of user 
    '      PARAMS1    : strUser as string, username 
    '      PARAMS2    : strPass as string, password 
    '      MEMO       :  
    '      CREATE     : 2011/07/15  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetUser(Optional ByVal strUser As String = Nothing,
                                Optional ByVal strPass As String = Nothing) As DataTable

        Dim objDataTable As DataTable

        fncGetUser = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " USERID, USERNAME, PASS_WORD, LASTUPDATE"
            strSQL &= " FROM"
            strSQL &= " M_USER"

            If Not String.IsNullOrEmpty(strUser) And Not String.IsNullOrEmpty(strPass) Then

                strSQL &= " WHERE"
                strSQL &= " USERNAME =" & xStrSQLFormat(strUser)
                strSQL &= " AND"
                strSQL &= " PASS_WORD =" & xStrSQLFormat(strPass)

            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateUser 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : stUserData  structure, user's information
    '      MEMO       :  
    '      CREATE     : 2011/07/20  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateUser(ByVal stUserData As stUserInfo) As Boolean

        fncUpdateUser = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE M_USER"

            strSQL &= " SET"
            strSQL &= " USERNAME = " & xStrSQLFormat(stUserData.strName)
            strSQL &= ",PASS_WORD = " & xStrSQLFormat(stUserData.strPass.Trim())
            strSQL &= ",LASTUPDATE = NOW"

            strSQL &= " WHERE"
            strSQL &= " USERID = " & xIntSQLFormat(stUserData.intUserID)

            blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function

#End Region


#Region "Family Member"


    '   ****************************************************************** 
    '      FUNCTION   : fncGetMaxMemID 
    '      VALUE      : Integer, max member id
    '      PARAMS     : intTable    Integer, table constant
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetMaxID(ByVal emTable As clsEnum.emTable) As Integer
        fncGetMaxID = -1

        Dim objDtTable As DataTable = Nothing

        Try
            Dim strResult As String

            'get datatable
            Select Case emTable

                'table Member
                Case clsEnum.emTable.T_FMEMBER_MAIN
                    objDtTable = xMaxMemberID()

                    'table Career
                Case clsEnum.emTable.T_FMEMBER_CAREER
                    objDtTable = xMaxCareerID()

                    'table Fact
                Case clsEnum.emTable.T_FMEMBER_FACT
                    objDtTable = xMaxFactID()

                    'table M_FAMILY_IMAGE
                Case clsEnum.emTable.M_FAMILY_IMAGE
                    objDtTable = xMaxAlbumID()

                    'table M_FAMILY_HEAD
                Case clsEnum.emTable.M_FAMILY_HEAD
                    objDtTable = xMaxFHeadID()

                    'table M_ROOT
                Case clsEnum.emTable.M_ROOT
                    objDtTable = xMaxRootID()

            End Select

            If objDtTable Is Nothing Then Exit Function

            'get result
            strResult = fncCnvNullToString(objDtTable.Rows(0).Item(0))

            'convert to int
            If Not Integer.TryParse(strResult, fncGetMaxID) Then Exit Function

            Return fncGetMaxID

        Catch ex As Exception
            Throw ex
        Finally
            If objDtTable IsNot Nothing Then objDtTable.Dispose()
        End Try

    End Function


#Region "Main information"

    '   ****************************************************************** 
    '      FUNCTION   : xMaxMemberID 
    '      VALUE      : DataTable, max id
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xMaxMemberID() As DataTable

        Dim objDataTable As DataTable

        xMaxMemberID = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " MAX(MEMBER_ID)"
            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_MAIN"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetMemberMain 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intID Integer, id to get data
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetMemberMain(Optional ByVal intID As Integer = -1) As DataTable

        fncGetMemberMain = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"

            strSQL &= " T_FMEMBER_MAIN.MEMBER_ID"
            strSQL &= ",T_FMEMBER_MAIN.LAST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.MIDDLE_NAME"
            strSQL &= ",T_FMEMBER_MAIN.FIRST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.ALIAS_NAME"
            strSQL &= ",FORMAT(T_FMEMBER_MAIN.BIRTH_DAY, 'YYYY/MM/DD') AS BIRTH_DAY"
            'strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
            'strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
            'strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"
            strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
            strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"
            strSQL &= ",T_FMEMBER_MAIN.BIR_DAY_LUNAR"
            strSQL &= ",T_FMEMBER_MAIN.BIR_MON_LUNAR"
            strSQL &= ",T_FMEMBER_MAIN.BIR_YEA_LUNAR"
            strSQL &= ",T_FMEMBER_MAIN.GENDER"
            strSQL &= ",T_FMEMBER_MAIN.BIRTH_PLACE"
            strSQL &= ",T_FMEMBER_MAIN.NATIONALITY"
            strSQL &= ",T_FMEMBER_MAIN.RELIGION"
            strSQL &= ",T_FMEMBER_MAIN.DECEASED"
            strSQL &= ",FORMAT(T_FMEMBER_MAIN.DECEASED_DATE, 'YYYY/MM/DD') AS DECEASED_DATE"
            'strSQL &= ",T_FMEMBER_MAIN.DEA_DAY"
            'strSQL &= ",T_FMEMBER_MAIN.DEA_MON"
            'strSQL &= ",T_FMEMBER_MAIN.DEA_YEA"
            strSQL &= ",T_FMEMBER_MAIN.DEA_DAY_SUN"
            strSQL &= ",T_FMEMBER_MAIN.DEA_MON_SUN"
            strSQL &= ",T_FMEMBER_MAIN.DEA_YEA_SUN"
            strSQL &= ",T_FMEMBER_MAIN.DEA_DAY"
            strSQL &= ",T_FMEMBER_MAIN.DEA_MON"
            strSQL &= ",T_FMEMBER_MAIN.DEA_YEA"

            strSQL &= ",T_FMEMBER_MAIN.BURY_PLACE"
            strSQL &= ",T_FMEMBER_MAIN.AVATAR_PATH"
            strSQL &= ",T_FMEMBER_MAIN.FAMILY_ORDER"
            strSQL &= ",T_FMEMBER_MAIN.REMARK"

            strSQL &= ",T_FMEMBER_MAIN.CAREER_TYPE"
            strSQL &= ",T_FMEMBER_MAIN.EDUCATION_TYPE"
            strSQL &= ",T_FMEMBER_MAIN.FACT_TYPE"
            strSQL &= ",T_FMEMBER_MAIN.CAREER"
            strSQL &= ",T_FMEMBER_MAIN.EDUCATION"
            strSQL &= ",T_FMEMBER_MAIN.FACT"
            strSQL &= ",T_FMEMBER_MAIN.LEVEL"

            strSQL &= ",T_FMEMBER_MAIN.LASTUPDATE"
            strSQL &= ",T_FMEMBER_CONTACT.HOMETOWN"
            strSQL &= ",T_FMEMBER_CONTACT.HOME_ADD"
            strSQL &= ",T_FMEMBER_CONTACT.PHONENUM1"
            strSQL &= ",T_FMEMBER_CONTACT.PHONENUM2"
            strSQL &= ",T_FMEMBER_CONTACT.MAIL_ADD1"
            strSQL &= ",T_FMEMBER_CONTACT.MAIL_ADD2"
            strSQL &= ",T_FMEMBER_CONTACT.FAXNUM"
            strSQL &= ",T_FMEMBER_CONTACT.URL"
            strSQL &= ",T_FMEMBER_CONTACT.IMNICK"
            strSQL &= ",T_FMEMBER_CONTACT.REMARK"

            strSQL &= " FROM"

            strSQL &= " T_FMEMBER_MAIN INNER JOIN T_FMEMBER_CONTACT"
            strSQL &= " ON"
            strSQL &= " T_FMEMBER_MAIN.MEMBER_ID = T_FMEMBER_CONTACT.MEMBER_ID"

            If intID > -1 Then

                strSQL &= " WHERE"
                strSQL &= " T_FMEMBER_MAIN.MEMBER_ID = " & xIntSQLFormat(intID)

            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertMemberMain 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : stMemberInfo stMemberInfoMain, structure 
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertMemberMain(ByVal stMemberInfo As stMemberInfoMain,
                                        Optional ByVal blnUseTransaction As Boolean = True) As Boolean

        fncInsertMemberMain = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "INSERT INTO T_FMEMBER_MAIN"
            strSQL &= "("
            strSQL &= " MEMBER_ID"

            strSQL &= ",LAST_NAME"
            strSQL &= ",MIDDLE_NAME"
            strSQL &= ",FIRST_NAME"
            strSQL &= ",ALIAS_NAME"

            strSQL &= ",FAMILY_ORDER"

            'strSQL &= ",BIRTH_DAY"
            strSQL &= ",BIR_DAY"
            strSQL &= ",BIR_MON"
            strSQL &= ",BIR_YEA"

            strSQL &= ",BIR_DAY_LUNAR"
            strSQL &= ",BIR_MON_LUNAR"
            strSQL &= ",BIR_YEA_LUNAR"

            strSQL &= ",GENDER"

            strSQL &= ",BIRTH_PLACE"
            strSQL &= ",NATIONALITY"
            strSQL &= ",RELIGION"

            strSQL &= ",DECEASED"
            'strSQL &= ",DECEASED_DATE"
            'strSQL &= ",DEA_DAY"
            'strSQL &= ",DEA_MON"
            'strSQL &= ",DEA_YEA"

            strSQL &= ",DEA_DAY_SUN"
            strSQL &= ",DEA_MON_SUN"
            strSQL &= ",DEA_YEA_SUN"

            strSQL &= ",DEA_DAY"
            strSQL &= ",DEA_MON"
            strSQL &= ",DEA_YEA"


            strSQL &= ",BURY_PLACE"
            strSQL &= ",AVATAR_PATH"
            strSQL &= ",REMARK"

            strSQL &= ",CAREER_TYPE"
            strSQL &= ",EDUCATION_TYPE"
            strSQL &= ",FACT_TYPE"
            strSQL &= ",CAREER"
            strSQL &= ",EDUCATION"
            strSQL &= ",FACT"

            strSQL &= ",LASTUPDATE"

            strSQL &= ")"
            strSQL &= "VALUES"
            strSQL &= "("

            With stMemberInfo
                strSQL &= " " & xIntSQLFormat(.intID)

                strSQL &= "," & xStrSQLFormat(.strLastName)
                strSQL &= "," & xStrSQLFormat(.strMidName)
                strSQL &= "," & xStrSQLFormat(.strFirstName)
                strSQL &= "," & xStrSQLFormat(.strAlias)

                strSQL &= "," & xIntSQLFormat(.intFamilyOrder)

                'strSQL &= "," & ChangeDateFormat(.dtBirth, 0)
                'strSQL &= "," & xIntSQLFormat(.stBirth.intDay)
                'strSQL &= "," & xIntSQLFormat(.stBirth.intMon)
                'strSQL &= "," & xIntSQLFormat(.stBirth.intYea)

                '***
                strSQL &= "," & xIntSQLFormat(.stBirthSun.intDay)
                strSQL &= "," & xIntSQLFormat(.stBirthSun.intMonth)
                strSQL &= "," & xIntSQLFormat(.stBirthSun.intYear)

                strSQL &= "," & xIntSQLFormat(.stBirthLunar.intDay)
                strSQL &= "," & xIntSQLFormat(.stBirthLunar.intMonth)
                strSQL &= "," & xIntSQLFormat(.stBirthLunar.intYear)
                '***
                strSQL &= "," & xIntSQLFormat(.intGender)

                strSQL &= "," & xStrSQLFormat(.strBirthPlace)
                strSQL &= "," & xStrSQLFormat(.strNationality)
                strSQL &= "," & xStrSQLFormat(.strReligion)

                strSQL &= "," & xIntSQLFormat(.intDeceased)
                'strSQL &= "," & ChangeDateFormat(.dtDeceased, 0)
                'strSQL &= "," & xIntSQLFormat(.stDeath.intDay)
                'strSQL &= "," & xIntSQLFormat(.stDeath.intMon)
                'strSQL &= "," & xIntSQLFormat(.stDeath.intYea)
                '***
                strSQL &= "," & xIntSQLFormat(.stDeathSun.intDay)
                strSQL &= "," & xIntSQLFormat(.stDeathSun.intMonth)
                strSQL &= "," & xIntSQLFormat(.stDeathSun.intYear)

                strSQL &= "," & xIntSQLFormat(.stDeathLunar.intDay)
                strSQL &= "," & xIntSQLFormat(.stDeathLunar.intMonth)
                strSQL &= "," & xIntSQLFormat(.stDeathLunar.intYear)
                '***

                strSQL &= "," & xStrSQLFormat(.strBuryPlace)
                strSQL &= "," & xStrSQLFormat(.strAvatar)
                strSQL &= "," & xStrSQLFormat(.strRemark)

                strSQL &= "," & xIntSQLFormat(.intCareerType)
                strSQL &= "," & xIntSQLFormat(.intEduType)
                strSQL &= "," & xIntSQLFormat(.intFactType)

                strSQL &= "," & xStrSQLFormat(.strCareerGeneral)
                strSQL &= "," & xStrSQLFormat(.strEduGeneral)
                strSQL &= "," & xStrSQLFormat(.strFactGeneral)

                strSQL &= ", NOW"
            End With

            strSQL &= ")"


            If blnUseTransaction Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            'fncAddMC1ToHasTable(stMemberInfo.intID)

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateMemberMain 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : stMemberInfo  structure, user's information
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/07/20  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateMemberMain(ByVal stMemberInfo As stMemberInfoMain,
                                        Optional ByVal blnUseTransaction As Boolean = True) As Boolean

        fncUpdateMemberMain = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_MAIN"

            With stMemberInfo

                strSQL &= " SET"

                strSQL &= " LAST_NAME =" & xStrSQLFormat(.strLastName)
                strSQL &= ",MIDDLE_NAME =" & xStrSQLFormat(.strMidName)
                strSQL &= ",FIRST_NAME =" & xStrSQLFormat(.strFirstName)
                strSQL &= ",ALIAS_NAME =" & xStrSQLFormat(.strAlias)

                strSQL &= ",FAMILY_ORDER =" & xIntSQLFormat(.intFamilyOrder)

                'strSQL &= ",BIRTH_DAY =" & ChangeDateFormat(.dtBirth, 0)
                'strSQL &= ",BIR_DAY =" & xIntSQLFormat(.stBirth.intDay)
                'strSQL &= ",BIR_MON =" & xIntSQLFormat(.stBirth.intMon)
                'strSQL &= ",BIR_YEA =" & xIntSQLFormat(.stBirth.intYea)

                strSQL &= ",BIR_DAY =" & xIntSQLFormat(.stBirthSun.intDay)
                strSQL &= ",BIR_MON =" & xIntSQLFormat(.stBirthSun.intMonth)
                strSQL &= ",BIR_YEA =" & xIntSQLFormat(.stBirthSun.intYear)

                strSQL &= ",BIR_DAY_LUNAR =" & xIntSQLFormat(.stBirthLunar.intDay)
                strSQL &= ",BIR_MON_LUNAR =" & xIntSQLFormat(.stBirthLunar.intMonth)
                strSQL &= ",BIR_YEA_LUNAR =" & xIntSQLFormat(.stBirthLunar.intYear)



                strSQL &= ",GENDER =" & xIntSQLFormat(.intGender)
                strSQL &= ",BIRTH_PLACE =" & xStrSQLFormat(.strBirthPlace)

                strSQL &= ",NATIONALITY =" & xStrSQLFormat(.strNationality)
                strSQL &= ",RELIGION =" & xStrSQLFormat(.strReligion)

                strSQL &= ",DECEASED =" & xIntSQLFormat(.intDeceased)
                'strSQL &= ",DECEASED_DATE =" & ChangeDateFormat(.dtDeceased, 0)
                'strSQL &= ",DEA_DAY =" & xIntSQLFormat(.stDeath.intDay)
                'strSQL &= ",DEA_MON =" & xIntSQLFormat(.stDeath.intMon)
                'strSQL &= ",DEA_YEA =" & xIntSQLFormat(.stDeath.intYea)

                strSQL &= ",DEA_DAY_SUN =" & xIntSQLFormat(.stDeathSun.intDay)
                strSQL &= ",DEA_MON_SUN =" & xIntSQLFormat(.stDeathSun.intMonth)
                strSQL &= ",DEA_YEA_SUN =" & xIntSQLFormat(.stDeathSun.intYear)

                strSQL &= ",DEA_DAY =" & xIntSQLFormat(.stDeathLunar.intDay)
                strSQL &= ",DEA_MON =" & xIntSQLFormat(.stDeathLunar.intMonth)
                strSQL &= ",DEA_YEA =" & xIntSQLFormat(.stDeathLunar.intYear)

                strSQL &= ",BURY_PLACE =" & xStrSQLFormat(.strBuryPlace)
                strSQL &= ",AVATAR_PATH =" & xStrSQLFormat(.strAvatar)
                strSQL &= ",REMARK =" & xStrSQLFormat(.strRemark)

                strSQL &= ",CAREER_TYPE =" & xIntSQLFormat(.intCareerType)
                strSQL &= ",EDUCATION_TYPE =" & xIntSQLFormat(.intEduType)
                strSQL &= ",FACT_TYPE =" & xIntSQLFormat(.intFactType)

                strSQL &= ",CAREER =" & xStrSQLFormat(.strCareerGeneral)
                strSQL &= ",EDUCATION =" & xStrSQLFormat(.strEduGeneral)
                strSQL &= ",FACT =" & xStrSQLFormat(.strFactGeneral)

                strSQL &= ",LASTUPDATE = NOW"

                strSQL &= " WHERE"
                strSQL &= " MEMBER_ID = " & xIntSQLFormat(.intID)

            End With

            If blnUseTransaction Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            'fncUpdateMC1ToHasTable(stMemberInfo.intID)

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateAvatar, change avatar picture 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intMemID  Integer, user id
    '      PARAMS2    : strAvatar String, avatar path
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/07/20  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateAvatar(ByVal intMemID As Integer,
                              ByVal strAvatar As String,
                              Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncUpdateAvatar = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_MAIN"

            strSQL &= " SET"

            strSQL &= " AVATAR_PATH =" & xStrSQLFormat(strAvatar)
            strSQL &= ",LASTUPDATE = NOW"

            strSQL &= " WHERE"
            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateMemberMain 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : stMemberInfo  structure, user's information
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/07/20  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateFamilyFlag(ByVal intMemID As Integer,
                                        ByVal enFlag As clsEnum.emFamily_Flag,
                                        Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncUpdateFamilyFlag = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_MAIN"

            strSQL &= " SET"

            strSQL &= " FAMILY_FLAG =" & xIntSQLFormat(enFlag)
            strSQL &= ",LASTUPDATE = NOW"

            strSQL &= " WHERE"
            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelMemberMain, delete member's main information
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intMemID Integer, member id
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/11/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelMemberMain(ByVal intMemID As Integer,
                                  Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncDelMemberMain = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM T_FMEMBER_MAIN"

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            'gtblMemberCard.Remove(intMemID)

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncFixDateTimeMain, fix date time
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intMemID Integer, member id
    '      PARAMS2    : stBirth stCalendar
    '      PARAMS3    : stDeath stCalendar
    '      PARAMS4    : blnIsRollBack Boolean
    '      MEMO       :  
    '      CREATE     : 2011/11/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncFixDateTimeMain(ByVal intMemID As Integer,
                                       ByVal stBirth As stCalendar,
                                       ByVal stDeath As stCalendar,
                                       Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncFixDateTimeMain = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_MAIN"

            strSQL &= " SET"

            strSQL &= " BIRTH_DAY =" & ChangeDateFormat(Nothing, 0)
            If stBirth.intDay > 0 Then strSQL &= ",BIR_DAY =" & xIntSQLFormat(stBirth.intDay)
            If stBirth.intMonth > 0 Then strSQL &= ",BIR_MON =" & xIntSQLFormat(stBirth.intMonth)
            If stBirth.intMonth > 0 Then strSQL &= ",BIR_YEA =" & xIntSQLFormat(stBirth.intYear)

            strSQL &= ",DECEASED_DATE =" & ChangeDateFormat(Nothing, 0)
            If stDeath.intDay > 0 Then strSQL &= ",DEA_DAY =" & xIntSQLFormat(stDeath.intDay)
            If stDeath.intMonth > 0 Then strSQL &= ",DEA_MON =" & xIntSQLFormat(stDeath.intMonth)
            If stDeath.intYear > 0 Then strSQL &= ",DEA_YEA =" & xIntSQLFormat(stDeath.intYear)

            strSQL &= ",LASTUPDATE = NOW"

            strSQL &= " WHERE"
            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try
    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateMemberOrder 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : stMemberInfo  structure, user's information
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/07/20  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateMemberFamilyOrder(ByVal intID As Integer, ByVal intFamilyOrder As Integer,
                                        Optional ByVal blnUseTransaction As Boolean = True) As Boolean

        fncUpdateMemberFamilyOrder = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_MAIN"

            strSQL &= " SET"
            strSQL &= " FAMILY_ORDER =" & xIntSQLFormat(intFamilyOrder)
            strSQL &= " WHERE"
            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intID)


            If blnUseTransaction Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            'fncUpdateMC1ToHasTable(stMemberInfo.intID)

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function

#End Region


#Region "Contact"

    '   ****************************************************************** 
    '      FUNCTION   : fncGetContact 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intID Integer, id to get data
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    'Public Function fncGetContact(ByVal intID As Integer) As DataTable

    '    Dim objDataTable As DataTable

    '    fncGetContact = Nothing

    '    objDataTable = Nothing

    '    Try

    '        Dim strSQL As String = ""

    '        strSQL &= "SELECT"

    '        strSQL &= " MEMBER_ID"
    '        strSQL &= ",HOMETOWN"
    '        strSQL &= ",HOME_ADD"
    '        strSQL &= ",PHONENUM1"
    '        strSQL &= ",PHONENUM2"
    '        strSQL &= ",MAIL_ADD1"
    '        strSQL &= ",MAIL_ADD2"
    '        strSQL &= ",FAXNUM"
    '        strSQL &= ",URL"
    '        strSQL &= ",IMNICK"
    '        strSQL &= ",REMARK"
    '        strSQL &= ",LASTUPDATE"

    '        strSQL &= " FROM"
    '        strSQL &= " T_FMEMBER_CONTACT"

    '        strSQL &= " WHERE"
    '        strSQL &= " MEMBER_ID = " & xIntSQLFormat(intID)


    '        objDataTable = GetTable(strSQL)

    '        If objDataTable IsNot Nothing Then _
    '            If objDataTable.Rows.Count = 0 Then Exit Function

    '    Catch ex As Exception

    '        Throw New clsDbAException(ex.Message, ex)

    '    End Try

    '    Return objDataTable

    'End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetStrucContact 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : intID Integer, id to get infor 
    '      PARAMS2    : stMemInfo stMemberInfoMain, return structure 
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetStrucContact(ByVal intID As Integer, ByRef stMemInfo As stMemberInfoContact) As Boolean

        fncGetStrucContact = False            'default return is false

        Dim stTempData As stMemberInfoContact       'temporary structure to store information

        Dim dtTable As DataTable                    'datatable that store member's infor

        dtTable = Nothing

        Try

            'get member data
            dtTable = fncGetMemberMain(intID)

            'check for empty data
            If dtTable Is Nothing Then Exit Function

            'init value
            stTempData = Nothing

            'fill data to MainInfo structure
            With dtTable.Rows(0)

                'member id
                stTempData.intID = intID

                'home town
                stTempData.strHometown = basCommon.fncCnvNullToString(.Item("HOMETOWN"))

                'home address
                stTempData.strHomeAddr = basCommon.fncCnvNullToString(.Item("HOME_ADD"))

                'phone number 1
                stTempData.strPhone1 = basCommon.fncCnvNullToString(.Item("PHONENUM1"))

                'phone number 2
                stTempData.strphone2 = basCommon.fncCnvNullToString(.Item("PHONENUM2"))

                'email address 1
                stTempData.strMail1 = basCommon.fncCnvNullToString(.Item("MAIL_ADD1"))

                'email address 2
                stTempData.strMail2 = basCommon.fncCnvNullToString(.Item("MAIL_ADD2"))

                'fax number
                stTempData.strFax = basCommon.fncCnvNullToString(.Item("FAXNUM"))

                'URL
                stTempData.strURL = basCommon.fncCnvNullToString(.Item("URL"))

                'IM nick
                stTempData.strIMNick = basCommon.fncCnvNullToString(.Item("IMNICK"))

                'remark
                stTempData.strRemark = basCommon.fncCnvNullToString(.Item("T_FMEMBER_CONTACT.REMARK"))

            End With

            'return structure
            stMemInfo = stTempData

            Return True

        Catch ex As Exception
            Throw ex
        Finally
            If dtTable IsNot Nothing Then dtTable.Dispose()
            stTempData = Nothing
        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertContact 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : stMemberInfo stMemberInfoMain, structure 
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertContact(ByVal stMemberInfo As stMemberInfoContact,
                                     Optional ByVal blnUseTransaction As Boolean = True) As Boolean

        fncInsertContact = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "INSERT INTO T_FMEMBER_CONTACT"
            strSQL &= "("
            strSQL &= " MEMBER_ID"
            strSQL &= ",HOMETOWN"
            strSQL &= ",HOME_ADD"
            strSQL &= ",PHONENUM1"
            strSQL &= ",PHONENUM2"
            strSQL &= ",MAIL_ADD1"
            strSQL &= ",MAIL_ADD2"
            strSQL &= ",FAXNUM"
            strSQL &= ",URL"
            strSQL &= ",IMNICK"
            strSQL &= ",REMARK"
            strSQL &= ",LASTUPDATE"
            strSQL &= ")"
            strSQL &= "VALUES"
            strSQL &= "("

            With stMemberInfo
                strSQL &= " " & xIntSQLFormat(.intID)
                strSQL &= "," & xStrSQLFormat(.strHometown)
                strSQL &= "," & xStrSQLFormat(.strHomeAddr)
                strSQL &= "," & xStrSQLFormat(.strPhone1)
                strSQL &= "," & xStrSQLFormat(.strphone2)
                strSQL &= "," & xStrSQLFormat(.strMail1)
                strSQL &= "," & xStrSQLFormat(.strMail2)
                strSQL &= "," & xStrSQLFormat(.strFax)
                strSQL &= "," & xStrSQLFormat(.strURL)
                strSQL &= "," & xStrSQLFormat(.strIMNick)
                strSQL &= "," & xStrSQLFormat(.strRemark)
                strSQL &= ", NOW"
            End With

            strSQL &= ")"


            If blnUseTransaction Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateContact 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : stMemberInfo  structure, user's information
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateContact(ByVal stMemberInfo As stMemberInfoContact,
                                     Optional ByVal blnUseTransaction As Boolean = True) As Boolean

        fncUpdateContact = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_CONTACT"

            With stMemberInfo

                strSQL &= " SET"
                strSQL &= " HOMETOWN =" & xStrSQLFormat(.strHometown)
                strSQL &= ",HOME_ADD =" & xStrSQLFormat(.strHomeAddr)
                strSQL &= ",PHONENUM1 =" & xStrSQLFormat(.strPhone1)
                strSQL &= ",PHONENUM2 =" & xStrSQLFormat(.strphone2)
                strSQL &= ",MAIL_ADD1 =" & xStrSQLFormat(.strMail1)
                strSQL &= ",MAIL_ADD2 =" & xStrSQLFormat(.strMail2)
                strSQL &= ",FAXNUM =" & xStrSQLFormat(.strFax)
                strSQL &= ",URL =" & xStrSQLFormat(.strURL)
                strSQL &= ",IMNICK =" & xStrSQLFormat(.strIMNick)
                strSQL &= ",REMARK =" & xStrSQLFormat(.strRemark)
                strSQL &= ",LASTUPDATE = NOW"

                strSQL &= " WHERE"
                strSQL &= " MEMBER_ID = " & xIntSQLFormat(.intID)

            End With

            If blnUseTransaction Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelContact 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intMemID Integer, member id
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/11/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelContact(ByVal intMemID As Integer,
                                  Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncDelContact = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM T_FMEMBER_CONTACT"

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function

#End Region


#Region "Career - Education"

    '   ****************************************************************** 
    '      FUNCTION   : xMaxCareerID 
    '      VALUE      : DataTable, max id
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xMaxCareerID() As DataTable

        Dim objDataTable As DataTable

        xMaxCareerID = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " MAX(CAREER_ID)"
            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_CAREER"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetCareer 
    '      VALUE      : DataTable, table of information
    '      PARAMS1    : intType Integer, Type of Career 1 - edu / 2 - career
    '      PARAMS2    : intMemID Integer, member id
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    'Public Function fncGetCareer(ByVal intType As Integer, ByVal intMemID As Integer) As DataTable
    Public Function fncGetCareer(ByVal intType As clsEnum.emCareerType, Optional ByVal intMemID As Integer = -1) As DataTable

        Dim objDataTable As DataTable

        fncGetCareer = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"

            strSQL &= " [MEMBER_ID]"
            strSQL &= ",[CAREER_ID]"
            strSQL &= ",[CAREER_TYPE]"

            strSQL &= ",[START_DATE]"
            strSQL &= ",[START_DAY]"
            strSQL &= ",[START_MON]"
            strSQL &= ",[START_YEA]"

            strSQL &= ",[END_DATE]"
            strSQL &= ",[END_DAY]"
            strSQL &= ",[END_MON]"
            strSQL &= ",[END_YEA]"

            strSQL &= ",[OCCUPATION]"
            strSQL &= ",[POSITION]"
            strSQL &= ",[OFFICE_NAME]"
            strSQL &= ",[OFFICE_PLACE]"
            strSQL &= ",[REMARK]"
            strSQL &= ",[LASTUPDATE]"

            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_CAREER"

            strSQL &= " WHERE"

            If intMemID >= 0 Then
                strSQL &= " [MEMBER_ID] = " & xIntSQLFormat(intMemID)
                strSQL &= " AND"
            End If

            strSQL &= " CAREER_TYPE = " & xIntSQLFormat(intType)

            'strSQL &= " ORDER BY [START_DATE], [CAREER_ID]"
            strSQL &= " ORDER BY [CAREER_ID]"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertCareer 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : stCareerInfo stCareer, structure 
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertCareer(ByVal stCareerInfo As stCareer,
                                    Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncInsertCareer = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "INSERT INTO T_FMEMBER_CAREER"
            strSQL &= " ("

            strSQL &= " [MEMBER_ID]"
            strSQL &= ",[CAREER_ID]"
            strSQL &= ",[CAREER_TYPE]"

            'strSQL &= ",[START_DATE]"
            strSQL &= ",[START_DAY]"
            strSQL &= ",[START_MON]"
            strSQL &= ",[START_YEA]"

            'strSQL &= ",[END_DATE]"
            strSQL &= ",[END_DAY]"
            strSQL &= ",[END_MON]"
            strSQL &= ",[END_YEA]"

            strSQL &= ",[OCCUPATION]"
            strSQL &= ",[POSITION]"
            strSQL &= ",[OFFICE_NAME]"
            strSQL &= ",[OFFICE_PLACE]"
            strSQL &= ",[REMARK]"

            strSQL &= ",[LASTUPDATE]"

            strSQL &= " )"
            strSQL &= " VALUES"
            strSQL &= " ("

            With stCareerInfo
                strSQL &= " " & xIntSQLFormat(.intMemID)
                strSQL &= "," & xIntSQLFormat(.intCareerID)
                strSQL &= "," & xIntSQLFormat(.intType)

                'strSQL &= "," & ChangeDateFormat(.dtStart, 0)
                strSQL &= "," & xIntSQLFormat(.intSday)
                strSQL &= "," & xIntSQLFormat(.intSmon)
                strSQL &= "," & xIntSQLFormat(.intSyea)

                'strSQL &= "," & ChangeDateFormat(.dtEnd, 0)
                strSQL &= "," & xIntSQLFormat(.intEday)
                strSQL &= "," & xIntSQLFormat(.intEmon)
                strSQL &= "," & xIntSQLFormat(.intEyea)

                strSQL &= "," & xStrSQLFormat(.strOccupt)
                strSQL &= "," & xStrSQLFormat(.strPosition)
                strSQL &= "," & xStrSQLFormat(.strOffName)
                strSQL &= "," & xStrSQLFormat(.strOffPlace)
                strSQL &= "," & xStrSQLFormat(.strRemark)

                strSQL &= ",NOW"
            End With

            strSQL &= " )"


            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelCareer 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intType Integer, Type of Career 1 - edu / 2 - career
    '      PARAMS2    : intMemID Integer, member idn
    '      PARAMS3    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelCareer(ByVal intType As clsEnum.emCareerType,
                                    ByVal intMemID As Integer,
                                    Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncDelCareer = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM T_FMEMBER_CAREER"

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)
            strSQL &= " AND"
            strSQL &= " CAREER_TYPE = " & xIntSQLFormat(intType)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncFixDateTimeCareer, fix date time
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemID Integer, member id
    '      PARAMS     : intCareerID Integer, member id
    '      PARAMS     : emCareerType  clsEnum.emCareerType, member id
    '      PARAMS     : stStart stCalendar
    '      PARAMS     : stEnd   stCalendar
    '      PARAMS     : blnIsRollBack Boolean
    '      MEMO       :  
    '      CREATE     : 2011/11/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncFixDateTimeCareer(ByVal intMemID As Integer,
                                         ByVal intCareerID As Integer,
                                         ByVal emCareerType As clsEnum.emCareerType,
                                         ByVal stStart As stCalendar,
                                         ByVal stEnd As stCalendar,
                                         Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncFixDateTimeCareer = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_CAREER"

            strSQL &= " SET"

            strSQL &= " START_DATE =" & ChangeDateFormat(Nothing, 0)
            If stStart.intDay > 0 Then strSQL &= ",START_DAY =" & xIntSQLFormat(stStart.intDay)
            If stStart.intMonth > 0 Then strSQL &= ",START_MON =" & xIntSQLFormat(stStart.intMonth)
            If stStart.intMonth > 0 Then strSQL &= ",START_YEA =" & xIntSQLFormat(stStart.intYear)

            strSQL &= ",END_DATE =" & ChangeDateFormat(Nothing, 0)
            If stEnd.intDay > 0 Then strSQL &= ",END_DAY =" & xIntSQLFormat(stEnd.intDay)
            If stEnd.intMonth > 0 Then strSQL &= ",END_MON =" & xIntSQLFormat(stEnd.intMonth)
            If stEnd.intYear > 0 Then strSQL &= ",END_YEA =" & xIntSQLFormat(stEnd.intYear)

            strSQL &= ",LASTUPDATE = NOW"

            strSQL &= " WHERE"
            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)
            strSQL &= " AND CAREER_ID = " & xIntSQLFormat(intCareerID)
            strSQL &= " AND CAREER_TYPE = " & xIntSQLFormat(emCareerType)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try
    End Function


#End Region


#Region "Fact"

    '   ****************************************************************** 
    '      FUNCTION   : xMaxFactID 
    '      VALUE      : DataTable, max id
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/08/04  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xMaxFactID() As DataTable

        Dim objDataTable As DataTable

        xMaxFactID = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " MAX(FACT_ID)"
            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_FACT"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetFact 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intMemID Integer, member id
    '      MEMO       :  
    '      CREATE     : 2011/08/02  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetFact(Optional ByVal intMemID As Integer = -1) As DataTable

        Dim objDataTable As DataTable

        fncGetFact = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"

            strSQL &= " [MEMBER_ID]"
            strSQL &= ",[FACT_ID]"
            strSQL &= ",[FACT_NAME]"
            strSQL &= ",[FACT_PLACE]"

            strSQL &= ",[START_DATE]"
            strSQL &= ",[START_DAY]"
            strSQL &= ",[START_MON]"
            strSQL &= ",[START_YEA]"

            strSQL &= ",[END_DATE]"
            strSQL &= ",[END_DAY]"
            strSQL &= ",[END_MON]"
            strSQL &= ",[END_YEA]"

            strSQL &= ",[DESCRIPTION]"
            strSQL &= ",[LASTUPDATE]"

            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_FACT"

            If intMemID >= 0 Then

                strSQL &= " WHERE"
                strSQL &= " [MEMBER_ID] = " & xIntSQLFormat(intMemID)

            End If

            'strSQL &= " ORDER BY [START_DATE], [FACT_ID]"
            strSQL &= " ORDER BY [FACT_ID]"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertFact 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : stFactInfo stFact, structure 
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/08/04  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertFact(ByVal stFactInfo As stFact,
                                    Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncInsertFact = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "INSERT INTO T_FMEMBER_FACT"
            strSQL &= " ("

            strSQL &= " [MEMBER_ID]"
            strSQL &= ",[FACT_ID]"

            strSQL &= ",[FACT_NAME]"
            strSQL &= ",[FACT_PLACE]"

            'strSQL &= ",[START_DATE]"
            strSQL &= ",[START_DAY]"
            strSQL &= ",[START_MON]"
            strSQL &= ",[START_YEA]"

            'strSQL &= ",[END_DATE]"
            strSQL &= ",[END_DAY]"
            strSQL &= ",[END_MON]"
            strSQL &= ",[END_YEA]"

            strSQL &= ",[DESCRIPTION]"

            strSQL &= ",[LASTUPDATE]"

            strSQL &= " )"
            strSQL &= " VALUES"
            strSQL &= " ("

            With stFactInfo

                strSQL &= " " & xIntSQLFormat(.intMemID)
                strSQL &= "," & xIntSQLFormat(.intFactID)

                strSQL &= "," & xStrSQLFormat(.strName)
                strSQL &= "," & xStrSQLFormat(.strPlace)

                'strSQL &= "," & ChangeDateFormat(.dtStart, 0)
                strSQL &= "," & xIntSQLFormat(.intSday)
                strSQL &= "," & xIntSQLFormat(.intSmon)
                strSQL &= "," & xIntSQLFormat(.intSyea)

                'strSQL &= "," & ChangeDateFormat(.dtEnd, 0)
                strSQL &= "," & xIntSQLFormat(.intEday)
                strSQL &= "," & xIntSQLFormat(.intEmon)
                strSQL &= "," & xIntSQLFormat(.intEyea)

                strSQL &= "," & xStrSQLFormat(.strDesc)

                strSQL &= ",NOW"

            End With

            strSQL &= " )"


            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelFact 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS1    : intMemID Integer, member id
    '      PARAMS2    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/08/04  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelFact(ByVal intMemID As Integer,
                                Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncDelFact = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM T_FMEMBER_FACT"

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncFixDateTimeCareer, fix date time
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemID Integer, member id
    '      PARAMS     : intCareerID Integer, member id
    '      PARAMS     : emCareerType  clsEnum.emCareerType, member id
    '      PARAMS     : stStart stCalendar
    '      PARAMS     : stEnd   stCalendar
    '      PARAMS     : blnIsRollBack Boolean
    '      MEMO       :  
    '      CREATE     : 2011/11/14  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncFixDateTimeFact(ByVal intMemID As Integer,
                                       ByVal intFactID As Integer,
                                       ByVal stStart As stCalendar,
                                       ByVal stEnd As stCalendar,
                                       Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncFixDateTimeFact = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_FACT"

            strSQL &= " SET"

            strSQL &= " START_DATE =" & ChangeDateFormat(Nothing, 0)
            If stStart.intDay > 0 Then strSQL &= ",START_DAY =" & xIntSQLFormat(stStart.intDay)
            If stStart.intMonth > 0 Then strSQL &= ",START_MON =" & xIntSQLFormat(stStart.intMonth)
            If stStart.intMonth > 0 Then strSQL &= ",START_YEA =" & xIntSQLFormat(stStart.intYear)

            strSQL &= ",END_DATE =" & ChangeDateFormat(Nothing, 0)
            If stEnd.intDay > 0 Then strSQL &= ",END_DAY =" & xIntSQLFormat(stEnd.intDay)
            If stEnd.intMonth > 0 Then strSQL &= ",END_MON =" & xIntSQLFormat(stEnd.intMonth)
            If stEnd.intYear > 0 Then strSQL &= ",END_YEA =" & xIntSQLFormat(stEnd.intYear)

            strSQL &= ",LASTUPDATE = NOW"

            strSQL &= " WHERE"
            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemID)
            strSQL &= " AND FACT_ID = " & xIntSQLFormat(intFactID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try
    End Function

#End Region


#Region "Relationship"

    '   ****************************************************************** 
    '      FUNCTION   : fncGetRel 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intId       Integer,
    '      PARAMS     : intFather   Integer,
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     : 2012/12/10  AKB Manh ADD Role_order
    '   ******************************************************************
    Private Function xGetRel(Optional ByVal strWhere As String = "",
                             Optional ByVal strOrder As String = " TFR.MEMBER_ID") As DataTable
        xGetRel = Nothing
        Dim objDataTable As DataTable = Nothing
        Try
            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " TFR.MEMBER_ID"                          'member
            strSQL &= ",TFR.REL_FMEMBER_ID"                     'parent
            strSQL &= ",TFR.RELID"
            strSQL &= ",TFM.FAMILY_ORDER"
            strSQL &= ",TFR.ROLE_ORDER"
            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_RELATION AS TFR"
            strSQL &= " LEFT JOIN"
            strSQL &= " T_FMEMBER_MAIN AS TFM"
            strSQL &= " ON"
            strSQL &= " TFM.MEMBER_ID=TFR.MEMBER_ID"
            strSQL &= " WHERE"
            strSQL &= " 1=1"

            If strWhere <> "" Then

                If strWhere.IndexOf(" AND") <> 0 Then
                    strSQL &= " AND "
                End If
                strSQL &= strWhere

            End If

            If strOrder <> "" Then
                strSQL &= " ORDER BY " & strOrder
            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable


    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncGetRel 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intId       Integer,
    '      PARAMS     : intFather   Integer,
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetRel(Optional ByVal intId As Integer = -1,
                              Optional ByVal intFather As Integer = -1,
                              Optional ByVal intRelID As Integer = -1) As DataTable

        fncGetRel = Nothing

        Try

            Dim strWhere As String = ""

            'this part for checking a member is the ancentor
            If intId > -1 Then
                strWhere &= " AND"
                strWhere &= " TFR.MEMBER_ID = " & xIntSQLFormat(intId)
            End If

            If intFather > -1 Then
                strWhere &= " AND"
                strWhere &= " TFR.REL_FMEMBER_ID = " & xIntSQLFormat(intFather)
            End If

            If intRelID > -1 Then
                strWhere &= " AND"
                strWhere &= " TFR.RELID = " & xIntSQLFormat(intRelID)
            End If

            Return xGetRel(strWhere)

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncGetRel 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intId       Integer,
    '      PARAMS     : intFather   Integer,
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetChild(Optional ByVal strWhere As String = "TFR.RELID = 2 OR TFR.RELID = 4") As DataTable

        Return xGetRel(strWhere)

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetRel 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intId       Integer,
    '      PARAMS     : intFather   Integer,
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetChildFull(Optional ByVal intParentLeft As Integer = -1, _
                                    Optional ByVal intParentRight As Integer = -1, _
                                    Optional ByVal strOrder As String = " ROLE_ORDER ASC, D.FAMILY_ORDER ASC") As DataTable

        fncGetChildFull = Nothing
        Dim objDataTable As DataTable = Nothing
        Try
            Dim strSQL As String = ""

            strSQL &= " SELECT"
            strSQL &= " D.MEMBER_ID AS CHILD_ID"
            strSQL &= " ,D.REL_FMEMBER_ID AS SPOUSE_LEFT"
            strSQL &= " ,D.RELID"
            strSQL &= " ,D.MOTHER AS SPOUSE_RIGHT"
            strSQL &= " ,C.ROLE_ORDER AS WIFE_ORDER"
            strSQL &= " ,D.FAMILY_ORDER AS CHILD_ORDER"
            strSQL &= " ,D.MOTHER_RELID"
            strSQL &= " ,(SELECT TOP 1 TFR4.RELID FROM T_FMEMBER_RELATION AS TFR4 WHERE "
            strSQL &= "  (D.REL_FMEMBER_ID = TFR4.MEMBER_ID AND D.MOTHER = TFR4.REL_FMEMBER_ID)"
            strSQL &= "  OR (D.REL_FMEMBER_ID = TFR4.REL_FMEMBER_ID AND D.MOTHER = TFR4.MEMBER_ID)"
            strSQL &= " ) AS PARENT_RELID"
            strSQL &= " FROM"
            strSQL &= " ("
            strSQL &= " SELECT"
            strSQL &= " TFR.MEMBER_ID"
            strSQL &= " ,TFR.REL_FMEMBER_ID"
            strSQL &= " ,TFR.RELID"
            strSQL &= " ,B.MOTHER"
            strSQL &= " ,B.MOTHER_RELID"
            strSQL &= " ,(SELECT FAMILY_ORDER FROM T_FMEMBER_MAIN WHERE TFR.MEMBER_ID=T_FMEMBER_MAIN.MEMBER_ID) AS FAMILY_ORDER"
            strSQL &= " FROM [T_FMEMBER_RELATION] AS TFR"
            strSQL &= " LEFT JOIN"
            strSQL &= " ("
            strSQL &= "   SELECT"
            strSQL &= "   TFR2.MEMBER_ID"
            strSQL &= "  ,TFR2.REL_FMEMBER_ID AS MOTHER"
            strSQL &= "  ,TFR2.RELID AS MOTHER_RELID"
            strSQL &= "   FROM"
            strSQL &= "   T_FMEMBER_RELATION AS TFR2"
            strSQL &= "   WHERE (RELID=" & CStr(clsEnum.emRelation.ADOPT) & " OR RELID =" & CStr(clsEnum.emRelation.NATURAL) & ")"
            strSQL &= " ) AS B"
            strSQL &= " ON TFR.MEMBER_ID = B.MEMBER_ID AND B.MOTHER<>TFR.REL_FMEMBER_ID"
            strSQL &= " WHERE (RELID=" & CStr(clsEnum.emRelation.ADOPT) & " OR RELID =" & CStr(clsEnum.emRelation.NATURAL) & ")"
            strSQL &= " ) AS D"
            strSQL &= " LEFT JOIN"
            strSQL &= " ("
            strSQL &= " ("
            strSQL &= "  SELECT TFR3.ROLE_ORDER"
            strSQL &= " ,TFR3.REL_FMEMBER_ID"
            strSQL &= " ,TFR3.MEMBER_ID"
            strSQL &= "  FROM T_FMEMBER_RELATION AS TFR3 WHERE RELID=" & CStr(clsEnum.emRelation.MARRIAGE)
            strSQL &= " )AS C"
            strSQL &= " )ON C.REL_FMEMBER_ID = D.MOTHER AND C.MEMBER_ID= D.REL_FMEMBER_ID"
            strSQL &= " WHERE"
            strSQL &= " 1=1"

            If intParentLeft > 0 Then
                strSQL &= " AND D.REL_FMEMBER_ID = " & intParentLeft.ToString
            End If

            If intParentRight >= 0 Then
                strSQL &= " AND D.MOTHER = " & intParentRight.ToString
            End If

            If strOrder <> "" Then
                strSQL &= " ORDER BY " & strOrder
            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function

    '   ****************************************************************** 
    '      FUNCTION   : fncGetParent 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intChild Integer, id to get data
    '                 : blnAdoptRelate  Boolean, adopt relationship
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetParent(ByVal intChild As Integer, Optional ByVal blnAdoptRelate As Boolean = True) As DataTable


        Dim objDataTable As DataTable

        fncGetParent = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            'strSQL &= "SELECT DISTINCT"
            strSQL &= "SELECT"
            strSQL &= " T_FMEMBER_RELATION.REL_FMEMBER_ID"
            strSQL &= ",T_FMEMBER_MAIN.LAST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.MIDDLE_NAME"
            strSQL &= ",T_FMEMBER_MAIN.FIRST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.ALIAS_NAME"
            strSQL &= ",T_FMEMBER_MAIN.GENDER"
            strSQL &= ",T_FMEMBER_MAIN.BIRTH_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
            strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"
            strSQL &= ",T_FMEMBER_MAIN.REMARK"
            strSQL &= ",T_FMEMBER_MAIN.FAMILY_ORDER"
            strSQL &= ",T_FMEMBER_RELATION.RELID"

            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_MAIN INNER JOIN T_FMEMBER_RELATION"
            strSQL &= " ON T_FMEMBER_MAIN.MEMBER_ID = T_FMEMBER_RELATION.REL_FMEMBER_ID"

            strSQL &= " WHERE"
            strSQL &= " ("
            strSQL &= " T_FMEMBER_RELATION.RELID = " & xIntSQLFormat(CInt(clsEnum.emRelation.NATURAL))

            'option to get apdopt relationship
            If blnAdoptRelate Then
                strSQL &= " Or "
                strSQL &= " T_FMEMBER_RELATION.RELID = " & xIntSQLFormat(CInt(clsEnum.emRelation.ADOPT))
            End If

            strSQL &= " )"

            strSQL &= " AND "
            strSQL &= " T_FMEMBER_RELATION.MEMBER_ID = " & xIntSQLFormat(intChild)

            strSQL &= " ORDER BY T_FMEMBER_MAIN.GENDER, T_FMEMBER_RELATION.RELID;"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetHusWife, get husband or wife 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intMemID Integer, id to get data
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetHusWife(ByVal intMemID As Integer) As DataTable

        fncGetHusWife = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            'strSQL &= "SELECT DISTINCT"
            strSQL &= "SELECT"
            strSQL &= " T_FMEMBER_RELATION.REL_FMEMBER_ID"
            strSQL &= ",T_FMEMBER_MAIN.LAST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.MIDDLE_NAME"
            strSQL &= ",T_FMEMBER_MAIN.FIRST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.ALIAS_NAME"
            strSQL &= ",T_FMEMBER_MAIN.GENDER"
            strSQL &= ",T_FMEMBER_MAIN.BIRTH_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
            strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"
            strSQL &= ",T_FMEMBER_MAIN.REMARK"
            strSQL &= ",T_FMEMBER_MAIN.FAMILY_ORDER"
            strSQL &= ",T_FMEMBER_RELATION.RELID"
            strSQL &= ",T_FMEMBER_RELATION.ROLE_ORDER"

            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_MAIN INNER JOIN T_FMEMBER_RELATION"
            strSQL &= " ON T_FMEMBER_MAIN.MEMBER_ID = T_FMEMBER_RELATION.REL_FMEMBER_ID"

            strSQL &= " WHERE"
            strSQL &= " T_FMEMBER_RELATION.RELID = " & xIntSQLFormat(CInt(clsEnum.emRelation.MARRIAGE))
            strSQL &= " AND "
            strSQL &= " T_FMEMBER_RELATION.MEMBER_ID = " & xIntSQLFormat(intMemID)

            strSQL &= " ORDER BY T_FMEMBER_RELATION.ROLE_ORDER, T_FMEMBER_RELATION.REL_FMEMBER_ID"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetSpouseList, get list of husband and wife
    '      VALUE      : DataTable, table of information
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/11/15  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetSpouseList(ByVal strKeyword As String) As DataTable

        fncGetSpouseList = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT DISTINCT"

            strSQL &= " HUSBAND.MEMBER_ID"
            strSQL &= ",HUSBAND.LAST_NAME"
            strSQL &= ",HUSBAND.MIDDLE_NAME"
            strSQL &= ",HUSBAND.FIRST_NAME"
            strSQL &= ",HUSBAND.ALIAS_NAME"
            strSQL &= ",WIFE.MEMBER_ID"
            strSQL &= ",WIFE.LAST_NAME"
            strSQL &= ",WIFE.MIDDLE_NAME"
            strSQL &= ",WIFE.FIRST_NAME"
            strSQL &= ",WIFE.ALIAS_NAME"

            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_MAIN AS WIFE INNER JOIN "
            strSQL &= " ("
            strSQL &= " T_FMEMBER_RELATION INNER JOIN T_FMEMBER_MAIN AS HUSBAND ON T_FMEMBER_RELATION.MEMBER_ID = HUSBAND.MEMBER_ID"
            strSQL &= " )"
            strSQL &= " ON "
            strSQL &= " WIFE.MEMBER_ID = T_FMEMBER_RELATION.REL_FMEMBER_ID"

            strSQL &= " WHERE"
            strSQL &= " T_FMEMBER_RELATION.RELID = " + xIntSQLFormat(clsEnum.emRelation.MARRIAGE)
            strSQL &= " AND"
            strSQL &= " HUSBAND.GENDER = " + xIntSQLFormat(clsEnum.emGender.MALE)

            If Not basCommon.fncIsBlank(strKeyword) Then

                strSQL &= " AND"
                strSQL &= " ("
                strSQL &= fncBuildQueryLike("[HUSBAND]![LAST_NAME] & [HUSBAND]![MIDDLE_NAME] & [HUSBAND]![FIRST_NAME]", strKeyword)
                strSQL &= " OR"
                strSQL &= fncBuildQueryLike("[WIFE]![LAST_NAME] & [WIFE]![MIDDLE_NAME] & [WIFE]![FIRST_NAME]", strKeyword)
                strSQL &= " )"

            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetKids 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : intFather Integer, father id to get data
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetKids(ByVal intFaMo As Integer) As DataTable

        fncGetKids = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            'strSQL &= "SELECT DISTINCT"
            strSQL &= "SELECT"
            strSQL &= " T_FMEMBER_RELATION.MEMBER_ID"
            strSQL &= ",T_FMEMBER_MAIN.LAST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.MIDDLE_NAME"
            strSQL &= ",T_FMEMBER_MAIN.FIRST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.ALIAS_NAME"
            strSQL &= ",T_FMEMBER_MAIN.FAMILY_ORDER"
            strSQL &= ",T_FMEMBER_MAIN.GENDER"
            strSQL &= ",T_FMEMBER_MAIN.BIRTH_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
            strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"
            strSQL &= ",T_FMEMBER_MAIN.REMARK"
            strSQL &= ",T_FMEMBER_RELATION.RELID"

            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_MAIN INNER JOIN T_FMEMBER_RELATION"
            strSQL &= " ON T_FMEMBER_MAIN.MEMBER_ID = T_FMEMBER_RELATION.MEMBER_ID"

            strSQL &= " WHERE"
            strSQL &= " ("
            strSQL &= " T_FMEMBER_RELATION.RELID = " & xIntSQLFormat(CInt(clsEnum.emRelation.NATURAL))
            strSQL &= " Or "
            strSQL &= " T_FMEMBER_RELATION.RELID = " & xIntSQLFormat(CInt(clsEnum.emRelation.ADOPT))
            strSQL &= " )"

            strSQL &= " AND"

            strSQL &= " ( "
            strSQL &= " T_FMEMBER_RELATION.REL_FMEMBER_ID = " & xIntSQLFormat(intFaMo)

            'If intMother > basConst.gcintNO_MEMBER Then
            '    strSQL &= " AND"
            '    strSQL &= " T_FMEMBER_RELATION.REL_FMEMBER_ID = " & xIntSQLFormat(intMother)
            'End If

            strSQL &= " )"

            'strSQL &= " ORDER BY T_FMEMBER_RELATION.RELID, T_FMEMBER_MAIN.BIRTH_DAY"
            'strSQL &= " ORDER BY T_FMEMBER_MAIN.FAMILY_ORDER, T_FMEMBER_MAIN.BIRTH_DAY"
            strSQL &= " ORDER BY T_FMEMBER_MAIN.FAMILY_ORDER, T_FMEMBER_MAIN.BIR_YEA, T_FMEMBER_MAIN.BIR_MON, T_FMEMBER_MAIN.BIR_DAY"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertRel 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS1    : intMemID Integer, member ID 
    '      PARAMS2    : intRelMemID Integer, related member id 
    '      PARAMS3    : intRelID Integer, relation ship id 
    '      PARAMS4    : blnIsRollBack Boolean, rollback or not? 
    '      MEMO       :  
    '      CREATE     : 2011/08/29  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertRel(ByVal intMemID As Integer, _
                                    ByVal intRelMemID As Integer, _
                                    ByVal emRelType As clsEnum.emRelation, _
                                    Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncInsertRel = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim intRelID As Integer

            'determine relationship
            Select Case emRelType
                Case clsEnum.emRelation.ADOPT
                    intRelID = clsEnum.emRelation.ADOPT
                Case clsEnum.emRelation.NATURAL
                    intRelID = clsEnum.emRelation.NATURAL
                Case clsEnum.emRelation.MARRIAGE
                    intRelID = clsEnum.emRelation.MARRIAGE
            End Select

            Dim strSQL As String = ""

            strSQL &= "INSERT INTO T_FMEMBER_RELATION"
            strSQL &= " ("

            strSQL &= " [MEMBER_ID]"
            strSQL &= ",[REL_FMEMBER_ID]"
            strSQL &= ",[RELID]"
            strSQL &= ",[ROLE_ORDER]"

            strSQL &= " )"
            strSQL &= " VALUES"
            strSQL &= " ("


            strSQL &= " " & xIntSQLFormat(intMemID)
            strSQL &= "," & xIntSQLFormat(intRelMemID)
            strSQL &= "," & xIntSQLFormat(intRelID)

            If intRelID = clsEnum.emRelation.MARRIAGE Then
                strSQL &= ", " & (fncGetMaxRoleOrder(intMemID) + 1)
            Else
                strSQL &= ", 0"
            End If

            strSQL &= " )"


            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetMaxRoleOrder, get max role order 
    '      VALUE      : Integer
    '      PARAMS1    : intMemID Integer, member ID 
    '      MEMO       :  
    '      CREATE     : 2011/08/29  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetMaxRoleOrder(ByVal intMemID As Integer) As Integer

        Dim intReturn As Integer = 0

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try
            Dim strSQL As String = ""

            strSQL &= ""
            strSQL &= " SELECT COUNT(REL_FMEMBER_ID)"
            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_RELATION"
            strSQL &= " WHERE"
            strSQL &= " MEMBER_ID = " & intMemID & " "
            strSQL &= " AND"
            strSQL &= " RELID = " & clsEnum.emRelation.MARRIAGE

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

            intReturn = basCommon.fncCnvToInt(objDataTable.Rows(0)(0))

        Catch ex As Exception
            Throw New clsDbAException(ex.Message, ex)
        End Try

        Return intReturn

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelRel 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intID   Integer, member ID 
    '      PARAMS     : intFId   Integer, related member ID 
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelRel(ByVal intID As Integer, Optional ByVal intFId As Integer = -1, Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncDelRel = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM T_FMEMBER_RELATION"

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intID)

            If intFId >= 0 Then
                'delete member with a relationship
                strSQL &= " AND"
                strSQL &= " REL_FMEMBER_ID = " & xIntSQLFormat(intFId)

            Else
                'delete everything
                strSQL &= " OR"
                strSQL &= " REL_FMEMBER_ID = " & xIntSQLFormat(intID)

            End If

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    ''' <summary>
    ''' fncChangeRoleOrder - change role order
    ''' </summary>
    ''' <param name="intID"></param>
    ''' <param name="intSpouseRoleOrder">Spouse Role Order</param>
    ''' <param name="blnUseTransaction">Using Transaction or Not</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncSetSpouseRoleOrder(ByVal intID As Integer, _
                                          ByVal intRelMemID As Integer, _
                                          ByVal intSpouseRoleOrder As Integer, _
                                          ByVal emRelID As clsEnum.emRelation, _
                                          Optional ByVal blnUseTransaction As Boolean = True) As Boolean

        fncSetSpouseRoleOrder = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE T_FMEMBER_RELATION"
            strSQL &= " SET"
            strSQL &= " ROLE_ORDER = " & xIntSQLFormat(intSpouseRoleOrder)

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intID)
            strSQL &= " AND"
            strSQL &= " REL_FMEMBER_ID = " & xIntSQLFormat(intRelMemID)
            strSQL &= " AND"
            strSQL &= " RELID = " & CInt(emRelID)

            If blnUseTransaction Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


#End Region


#Region "Family Info"


    '   ****************************************************************** 
    '      FUNCTION   : xMaxFHeadID 
    '      VALUE      : DataTable, max id
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/11/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xMaxFHeadID() As DataTable

        Dim objDataTable As DataTable

        xMaxFHeadID = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " MAX(FHEAD_ID)"
            strSQL &= " FROM"
            strSQL &= " M_FAMILY_HEAD"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetFHead, get all family head member
    '      VALUE      : DataTable, table of information
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/08/16  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetFHead() As DataTable

        fncGetFHead = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " T_FMEMBER_MAIN.MEMBER_ID"
            strSQL &= ",T_FMEMBER_MAIN.LAST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.MIDDLE_NAME"
            strSQL &= ",T_FMEMBER_MAIN.FIRST_NAME"
            strSQL &= ",T_FMEMBER_MAIN.ALIAS_NAME"
            'strSQL &= ",T_FMEMBER_MAIN.BIRTH_DAY"
            'strSQL &= ",T_FMEMBER_MAIN.DECEASED_DATE"
            strSQL &= ",FORMAT(T_FMEMBER_MAIN.BIRTH_DAY, 'YYYY/MM/DD') AS BIRTH_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
            strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
            strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"

            strSQL &= ",FORMAT(T_FMEMBER_MAIN.DECEASED_DATE, 'YYYY/MM/DD') AS DECEASED_DATE"
            strSQL &= ",T_FMEMBER_MAIN.DEA_DAY"
            strSQL &= ",T_FMEMBER_MAIN.DEA_MON"
            strSQL &= ",T_FMEMBER_MAIN.DEA_YEA"

            strSQL &= ",T_FMEMBER_MAIN.REMARK"
            strSQL &= ",1 AS [" & basConst.gcstrFieldLevel & "]"

            strSQL &= " FROM"
            strSQL &= " M_FAMILY_HEAD INNER JOIN T_FMEMBER_MAIN"
            strSQL &= " ON M_FAMILY_HEAD.MEMBER_ID = T_FMEMBER_MAIN.MEMBER_ID"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertFHead, insert a family head member
    '      VALUE      : Boolean, true - success, false - fail
    '      PARAMS     : intMemberId Integer, member to set
    '      PARAMS     : blnIsRollBack Boolean, enable rolling back
    '      MEMO       :  
    '      CREATE     : 2011/08/16  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertFHead(ByVal intMemberId As Integer, Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncInsertFHead = False

        Dim objDataTable As DataTable = Nothing
        Dim blnBeginTrans As Boolean = False

        Try
            Dim intFid As Integer
            Dim strSQL As String

            intFid = fncGetMaxID(clsEnum.emTable.M_FAMILY_HEAD) + 1

            strSQL = ""
            strSQL &= "INSERT INTO M_FAMILY_HEAD"
            strSQL &= "("
            strSQL &= " FHEAD_ID"
            strSQL &= ",MEMBER_ID"
            strSQL &= ")"

            strSQL &= "VALUES"
            strSQL &= "("
            strSQL &= " " + xIntSQLFormat(intFid)
            strSQL &= "," + xIntSQLFormat(intMemberId)
            strSQL &= ")"

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelFhead 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemberID   Integer, member ID 
    '      PARAMS     : blnIsRollBack   Boolean, enable rolling back
    '      MEMO       :  
    '      CREATE     : 2011/11/11  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelFhead(ByVal intMemberID As Integer, Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncDelFhead = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM M_FAMILY_HEAD"

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemberID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetFatherSon, get table of father and son 
    '      VALUE      : DataTable, table of information
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetFatherSon(Optional ByVal blnGetAll As Boolean = False) As DataTable

        fncGetFatherSon = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT DISTINCT"
            strSQL &= " T_FMEMBER_RELATION.REL_FMEMBER_ID AS " & basConst.gcstrFieldFather
            strSQL &= ",T_FMEMBER_RELATION.MEMBER_ID AS " & basConst.gcstrFieldSon

            strSQL &= " FROM"
            strSQL &= " ("
            strSQL &= " T_FMEMBER_MAIN INNER JOIN T_FMEMBER_RELATION"
            strSQL &= " ON T_FMEMBER_MAIN.MEMBER_ID = T_FMEMBER_RELATION.REL_FMEMBER_ID"
            strSQL &= " )"

            strSQL &= " INNER JOIN T_FMEMBER_MAIN AS T_FMEMBER_MAIN_1"
            strSQL &= " ON T_FMEMBER_RELATION.MEMBER_ID = T_FMEMBER_MAIN_1.MEMBER_ID"

            strSQL &= " WHERE "

            If Not blnGetAll Then

                strSQL &= " T_FMEMBER_MAIN_1.GENDER =" & xIntSQLFormat(clsEnum.emGender.MALE)
                strSQL &= " AND"

            End If

            strSQL &= " T_FMEMBER_MAIN.GENDER =" & xIntSQLFormat(clsEnum.emGender.MALE)
            strSQL &= " AND"
            strSQL &= " T_FMEMBER_RELATION.RELID =" & xIntSQLFormat(CInt(clsEnum.emRelation.NATURAL))

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetFamilyInfo, get all family info
    '      VALUE      : DataTable, table of information
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/08/16  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetFamilyInfo() As DataTable

        fncGetFamilyInfo = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " FAMILY_NAME"
            strSQL &= ",FAMILY_HOMETOWN"
            strSQL &= ",FAMILY_ANNIVERSARY"

            strSQL &= " FROM"
            strSQL &= " M_FAMILY_INFO"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertFHead, insert a family head member
    '      VALUE      : Boolean, true - success, false - fail
    '      PARAMS     : intMemberId Integer, member to set
    '      PARAMS     : blnIsRollBack Boolean, enable rolling back
    '      MEMO       :  
    '      CREATE     : 2011/08/16  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertFamilyInfo(ByVal strFName As String, ByVal strFHometown As String, ByVal strFAnni As String) As Boolean

        fncInsertFamilyInfo = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String

            strSQL = ""
            strSQL &= "INSERT INTO M_FAMILY_INFO"
            strSQL &= "("
            strSQL &= " FAMILY_NAME"
            strSQL &= ",FAMILY_HOMETOWN"
            strSQL &= ",FAMILY_ANNIVERSARY"
            strSQL &= ")"

            strSQL &= "VALUES"
            strSQL &= "("
            strSQL &= " " + xStrSQLFormat(strFName)
            strSQL &= "," + xStrSQLFormat(strFHometown)
            strSQL &= "," + xStrSQLFormat(strFAnni)
            strSQL &= ")"

            blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateFamilyInfo, update family info 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : strFName  String, family name
    '      PARAMS     : strFHometown  String, home town
    '      PARAMS     : strFAnni  String, anniversary
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateFamilyInfo(ByVal strFName As String, ByVal strFHometown As String, ByVal strFAnni As String) As Boolean

        fncUpdateFamilyInfo = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE M_FAMILY_INFO"

            strSQL &= " SET"
            strSQL &= " FAMILY_NAME = " & xStrSQLFormat(strFName)
            strSQL &= ",FAMILY_HOMETOWN = " & xStrSQLFormat(strFHometown)
            strSQL &= ",FAMILY_ANNIVERSARY = " & xStrSQLFormat(strFAnni)

            blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


#End Region


#Region "Image Album"


    '   ****************************************************************** 
    '      FUNCTION   : xMaxAlbumID 
    '      VALUE      : DataTable, max id
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xMaxAlbumID() As DataTable

        xMaxAlbumID = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " MAX(IMAGE_ID)"
            strSQL &= " FROM"
            strSQL &= " M_FAMILY_IMAGE"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetFAlbum, get table of images in family album
    '      VALUE      : DataTable, table of information
    '      PARAMS     : 
    '      MEMO       :  
    '      CREATE     : 2011/08/18  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetFAlbum(Optional ByVal intID As Integer = -1) As DataTable

        fncGetFAlbum = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " IMAGE_ID"
            strSQL &= ",IMAGE_TITLE"
            strSQL &= ",IMAGE_DES"
            strSQL &= ",IMAGE_NAME"

            strSQL &= " FROM"

            strSQL &= " M_FAMILY_IMAGE"

            If intID > -1 Then

                strSQL &= " WHERE"
                strSQL &= " IMAGE_ID =" & xIntSQLFormat(intID)

            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncUpdateAlbum 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : stImgInfo  stAlbum, image information
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncUpdateAlbum(ByVal stImgInfo As stAlbum) As Boolean

        fncUpdateAlbum = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE M_FAMILY_IMAGE"

            strSQL &= " SET"
            strSQL &= " IMAGE_TITLE = " & xStrSQLFormat(stImgInfo.strTitle)
            strSQL &= ",IMAGE_DES = " & xStrSQLFormat(stImgInfo.strDesc)
            strSQL &= ",IMAGE_NAME = " & xStrSQLFormat(stImgInfo.strName)

            strSQL &= " WHERE"
            strSQL &= " IMAGE_ID = " & xIntSQLFormat(stImgInfo.intID)

            blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelAlbum 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intID   Integer, image ID 
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelAlbum(ByVal intID As Integer) As Boolean

        fncDelAlbum = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM M_FAMILY_IMAGE"

            strSQL &= " WHERE"

            strSQL &= " IMAGE_ID = " & xIntSQLFormat(intID)

            blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertAlbum 
    '      VALUE      : Boolean, true - success, false - failure
    '      PARAMS     : stImgInfo stAlbum, structure 
    '      MEMO       :  
    '      CREATE     : 2011/08/22  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertAlbum(ByVal stImgInfo As stAlbum) As Boolean

        fncInsertAlbum = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "INSERT INTO M_FAMILY_IMAGE"
            strSQL &= " ("

            strSQL &= " [IMAGE_ID]"
            strSQL &= ",[IMAGE_TITLE]"
            strSQL &= ",[IMAGE_DES]"
            strSQL &= ",[IMAGE_NAME]"

            strSQL &= " )"
            strSQL &= " VALUES"
            strSQL &= " ("

            With stImgInfo
                strSQL &= " " & xIntSQLFormat(.intID)
                strSQL &= "," & xStrSQLFormat(.strTitle)
                strSQL &= "," & xStrSQLFormat(.strDesc)
                strSQL &= "," & xStrSQLFormat(.strName)
            End With

            strSQL &= " )"

            blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


#End Region


#End Region


#Region "Master Tables"

    '   ****************************************************************** 
    '      FUNCTION   : fncGetNation, get nationality 
    '      VALUE      : DataTable, list of nationality
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetNation(Optional ByVal intNationID As Integer = -1) As DataTable

        fncGetNation = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " NAT_ID, NAT_NAME"
            strSQL &= " FROM"
            strSQL &= " M_NATIONALITY"

            If intNationID > -1 Then
                strSQL &= " WHERE"
                strSQL &= " NAT_ID = " & xIntSQLFormat(intNationID)
            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetReligion, get religion 
    '      VALUE      : DataTable, list of religion
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetReligion(Optional ByVal intReligionID As Integer = -1) As DataTable

        fncGetReligion = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " REL_ID, REL_NAME"
            strSQL &= " FROM"
            strSQL &= " M_RELIGION"

            If intReligionID > -1 Then
                strSQL &= " WHERE"
                strSQL &= " REL_ID = " & xIntSQLFormat(intReligionID)
            End If

            strSQL &= " ORDER BY REL_ID"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    ''' <summary>
    ''' Update Religion Value
    ''' </summary>
    ''' <param name="intID"></param>
    ''' <param name="strValue"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncUpdateReligion(ByVal intID As Integer, ByVal strValue As String) As Boolean

        fncUpdateReligion = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "UPDATE M_RELIGION"


            strSQL &= " SET"
            strSQL &= " REL_NAME =" & xStrSQLFormat(strValue)

            strSQL &= " WHERE"
            strSQL &= " REL_ID = " & xIntSQLFormat(intID)

            blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try
    End Function


    '   ****************************************************************** 
    '      FUNCTION   : xMaxRootID 
    '      VALUE      : DataTable, max id
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/12/30  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Private Function xMaxRootID() As DataTable

        Dim objDataTable As DataTable

        xMaxRootID = Nothing

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            strSQL &= "SELECT"
            strSQL &= " MAX(ROOT_ID)"
            strSQL &= " FROM"
            strSQL &= " M_ROOT"

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetRoot, get root member
    '      VALUE      : DataTable, list of religion
    '      PARAMS     : none
    '      MEMO       :  
    '      CREATE     : 2011/07/28  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetRoot(Optional ByVal blnGetAll As Boolean = False) As DataTable

        fncGetRoot = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try
            Dim intRid As Integer
            Dim strSQL As String = ""

            intRid = fncGetMaxID(clsEnum.emTable.M_ROOT)

            strSQL &= "SELECT"
            strSQL &= " ROOT_ID, MEMBER_ID"
            strSQL &= " FROM"
            strSQL &= " M_ROOT"

            If Not blnGetAll Then

                strSQL &= " WHERE"
                strSQL &= " ROOT_ID = " + xIntSQLFormat(intRid)

            End If

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncInsertRoot, set a member is a root
    '      VALUE      : Boolean, true - success, false - fail
    '      PARAMS     : intMemberId Integer, member to set
    '      PARAMS     : blnIsRollBack Boolean, enable rolling back
    '      MEMO       :  
    '      CREATE     : 2011/12/30  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncInsertRoot(ByVal intMemberId As Integer, Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncInsertRoot = False

        Dim objDataTable As DataTable = Nothing
        Dim blnBeginTrans As Boolean = False

        Try
            Dim intRid As Integer
            Dim strSQL As String

            intRid = fncGetMaxID(clsEnum.emTable.M_ROOT) + 1

            strSQL = ""
            strSQL &= "INSERT INTO M_ROOT"
            strSQL &= "("
            strSQL &= " ROOT_ID"
            strSQL &= ",MEMBER_ID"
            strSQL &= ")"

            strSQL &= "VALUES"
            strSQL &= "("
            strSQL &= " " + xIntSQLFormat(intRid)
            strSQL &= "," + xIntSQLFormat(intMemberId)
            strSQL &= ")"

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncDelRoot 
    '      VALUE      : boolean, true - success, false - failure
    '      PARAMS     : intMemberID   Integer, member ID 
    '      PARAMS     : blnIsRollBack   Boolean, enable rolling back
    '      MEMO       :  
    '      CREATE     : 2011/12/30  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncDelRoot(ByVal intMemberID As Integer, Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncDelRoot = False

        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= "DELETE FROM M_ROOT"

            strSQL &= " WHERE"

            strSQL &= " MEMBER_ID = " & xIntSQLFormat(intMemberID)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function

#End Region


#Region "Search"

    '   ****************************************************************** 
    '      FUNCTION   : fncGetSearch 
    '      VALUE      : DataTable, table of result
    '      PARAMS     : stSearchInfo stSearch, structure of keyword
    '      MEMO       :  
    '      CREATE     : 2011/08/10  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetSearch(ByVal stSearchInfo As stSearch) As DataTable

        fncGetSearch = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strDateFormat As String = "{0:0000}/{1:00}/{2:00}"
            Dim strSQL As String = ""

            With stSearchInfo


                'strSQL &= "SELECT DISTINCT"
                strSQL &= "SELECT"
                strSQL &= " T_FMEMBER_MAIN.MEMBER_ID"
                strSQL &= ",T_FMEMBER_MAIN.LAST_NAME"
                strSQL &= ",T_FMEMBER_MAIN.MIDDLE_NAME"
                strSQL &= ",T_FMEMBER_MAIN.FIRST_NAME"
                strSQL &= ",T_FMEMBER_MAIN.ALIAS_NAME"
                strSQL &= ",T_FMEMBER_MAIN.BIRTH_PLACE"
                strSQL &= ",T_FMEMBER_MAIN.BURY_PLACE"
                strSQL &= ",T_FMEMBER_CONTACT.HOME_ADD"
                strSQL &= ",T_FMEMBER_CONTACT.HOMETOWN"
                strSQL &= ",T_FMEMBER_CONTACT.PHONENUM1"
                strSQL &= ",T_FMEMBER_CONTACT.PHONENUM2"
                strSQL &= ",T_FMEMBER_CONTACT.MAIL_ADD1"
                strSQL &= ",T_FMEMBER_CONTACT.MAIL_ADD2"
                strSQL &= ",T_FMEMBER_CONTACT.URL"
                strSQL &= ",T_FMEMBER_CONTACT.IMNICK"
                strSQL &= ",T_FMEMBER_MAIN.REMARK"
                strSQL &= ",T_FMEMBER_MAIN.GENDER"
                strSQL &= ",T_FMEMBER_MAIN.DECEASED"
                strSQL &= ",T_FMEMBER_MAIN.LEVEL"
                'strSQL &= ",FORMAT(T_FMEMBER_MAIN.BIRTH_DAY, 'YYYY/MM/DD') AS BIRTH_DAY"
                strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
                strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
                strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"
                strSQL &= ",(FORMAT(T_FMEMBER_MAIN.BIR_YEA, '0000') & '/' & FORMAT(T_FMEMBER_MAIN.BIR_MON, '00') & '/' & FORMAT(T_FMEMBER_MAIN.BIR_DAY, '00')) AS [BIRTH_DAY]"

                'strSQL &= ",FORMAT(T_FMEMBER_MAIN.DECEASED_DATE, 'YYYY/MM/DD') AS DECEASED_DATE"
                strSQL &= ",T_FMEMBER_MAIN.DEA_DAY"
                strSQL &= ",T_FMEMBER_MAIN.DEA_MON"
                strSQL &= ",T_FMEMBER_MAIN.DEA_YEA"
                strSQL &= ",(FORMAT(T_FMEMBER_MAIN.DEA_YEA, '0000') & '/' & FORMAT(T_FMEMBER_MAIN.DEA_MON, '00') & '/' & FORMAT(T_FMEMBER_MAIN.DEA_DAY, '00')) AS [DECEASED_DATE]"
                'strSQL &= ",T_FMEMBER_MAIN.BIRTH_DAY"
                'strSQL &= ",T_FMEMBER_MAIN.DECEASED_DATE"

                strSQL &= " FROM"

                'strSQL &= " (T_FMEMBER_MAIN LEFT JOIN MEMBER_INFO ON T_FMEMBER_MAIN.MEMBER_ID = MEMBER_INFO.MEMBER_ID)"

                strSQL &= " (T_FMEMBER_MAIN LEFT JOIN"
                strSQL &= " ("

                strSQL &= " SELECT DISTINCT"
                strSQL &= " T_FMEMBER_MAIN.MEMBER_ID"
                strSQL &= ",[T_FMEMBER_MAIN.LAST_NAME]"
                strSQL &= " & ', ' & [T_FMEMBER_MAIN.MIDDLE_NAME]"
                strSQL &= " & ', ' & [T_FMEMBER_MAIN.FIRST_NAME]"
                strSQL &= " & ', ' & [T_FMEMBER_MAIN.ALIAS_NAME]"
                strSQL &= " & ', ' & [T_FMEMBER_MAIN.REMARK]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.HOMETOWN]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.HOME_ADD]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.PHONENUM1]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.PHONENUM2]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.MAIL_ADD1]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.MAIL_ADD2]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.FAXNUM]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.URL]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.IMNICK]"
                strSQL &= " & ', ' & [T_FMEMBER_CONTACT.REMARK]"
                strSQL &= " & ', ' & [T_FMEMBER_CAREER.OFFICE_NAME]"
                strSQL &= " & ', ' & [T_FMEMBER_CAREER.OFFICE_PLACE]"
                strSQL &= " & ', ' & [T_FMEMBER_CAREER.REMARK]"
                strSQL &= " & ', ' & [T_FMEMBER_FACT.FACT_NAME]"
                strSQL &= " & ', ' & [T_FMEMBER_FACT.FACT_PLACE]"
                strSQL &= " & ', ' & [T_FMEMBER_FACT.DESCRIPTION] AS MAIN_INFO"
                strSQL &= ",[T_FMEMBER_CAREER.OCCUPATION]"
                strSQL &= ",[T_FMEMBER_CAREER.POSITION]"
                strSQL &= " FROM"
                strSQL &= " (T_FMEMBER_CONTACT INNER JOIN "
                strSQL &= " (T_FMEMBER_CAREER RIGHT JOIN T_FMEMBER_MAIN ON T_FMEMBER_CAREER.MEMBER_ID = T_FMEMBER_MAIN.MEMBER_ID)"
                strSQL &= " ON T_FMEMBER_CONTACT.MEMBER_ID = T_FMEMBER_MAIN.MEMBER_ID) "
                strSQL &= " LEFT JOIN T_FMEMBER_FACT ON T_FMEMBER_MAIN.MEMBER_ID = T_FMEMBER_FACT.MEMBER_ID"

                strSQL &= " ) AS [MEMBER_INFO]"
                strSQL &= " ON T_FMEMBER_MAIN.MEMBER_ID = MEMBER_INFO.MEMBER_ID)"


                strSQL &= " INNER JOIN T_FMEMBER_CONTACT ON T_FMEMBER_MAIN.MEMBER_ID = T_FMEMBER_CONTACT.MEMBER_ID"

                strSQL &= " WHERE"

                'keyword
                strSQL &= fncBuildQueryLike("[MEMBER_INFO.MAIN_INFO]", .strKeyword)

                'search by gender
                strSQL &= " AND "
                Select Case .intGender

                    Case clsEnum.emGender.UNKNOW
                        strSQL &= " T_FMEMBER_MAIN.GENDER LIKE '%%'"

                    Case clsEnum.emGender.MALE
                        strSQL &= " T_FMEMBER_MAIN.GENDER = " & xIntSQLFormat(clsEnum.emGender.MALE)

                    Case clsEnum.emGender.FEMALE
                        strSQL &= " T_FMEMBER_MAIN.GENDER = " & xIntSQLFormat(clsEnum.emGender.FEMALE)

                End Select


                'deceased or not
                If .intDie = basConst.gcintDIED Then

                    'if died, search by deceased date
                    'If .dtDieFrom > Date.MinValue Then strSQL &= " AND T_FMEMBER_MAIN.DECEASED_DATE >= CDATE(" & ChangeDateFormat(.dtDieFrom, 0) & ")"
                    If .intDFday > 0 Or .intDFmon > 0 Or .intDFyea > 0 Then _
                        strSQL &= " AND (FORMAT(T_FMEMBER_MAIN.DEA_YEA, '0000') & '/' & FORMAT(T_FMEMBER_MAIN.DEA_MON, '00') & '/' & FORMAT(T_FMEMBER_MAIN.DEA_DAY, '00')) >= '" & String.Format(strDateFormat, .intDFyea, .intDFmon, .intDFday) & "'"

                    'If .dtDieTo > Date.MinValue Then strSQL &= " AND T_FMEMBER_MAIN.DECEASED_DATE <= CDATE(" & ChangeDateFormat(.dtDieTo, 0) & ")"
                    If .intDTday > 0 Or .intDTmon > 0 Or .intDTyea > 0 Then _
                        strSQL &= " AND (FORMAT(T_FMEMBER_MAIN.DEA_YEA, '0000') & '/' & FORMAT(T_FMEMBER_MAIN.DEA_MON, '00') & '/' & FORMAT(T_FMEMBER_MAIN.DEA_DAY, '00')) >= '" & String.Format(strDateFormat, .intDTyea, .intDTmon, .intDTday) & "'"

                    strSQL &= " AND"
                    strSQL &= " T_FMEMBER_MAIN.DECEASED = " & xIntSQLFormat(basConst.gcintDIED)

                End If


                'search by birth date
                If .dtBirthFrom > Date.MinValue Then
                    strSQL &= " AND"
                    'strSQL &= " T_FMEMBER_MAIN.BIRTH_DAY >= CDATE(" & ChangeDateFormat(.dtBirthFrom, 0) & ")"
                    'strSQL &= " BIRTH_DAY >= '" & String.Format(strDateFormat, .dtBirthFrom.Year, .dtBirthFrom.Month, .dtBirthFrom.Day) & "'"
                    strSQL &= " (FORMAT(T_FMEMBER_MAIN.BIR_YEA, '0000') & '/' & FORMAT(T_FMEMBER_MAIN.BIR_MON, '00') & '/' & FORMAT(T_FMEMBER_MAIN.BIR_DAY, '00')) >= '" & String.Format(strDateFormat, .dtBirthFrom.Year, .dtBirthFrom.Month, .dtBirthFrom.Day) & "'"
                End If

                If .dtBirthTo > Date.MinValue Then
                    strSQL &= " AND "
                    'strSQL &= " T_FMEMBER_MAIN.BIRTH_DAY <= CDATE(" & ChangeDateFormat(.dtBirthTo, 0) & ")"
                    'strSQL &= " BIRTH_DAY <= '" & String.Format(strDateFormat, .dtBirthTo.Year, .dtBirthTo.Month, .dtBirthTo.Day) & "'"
                    strSQL &= " (FORMAT(T_FMEMBER_MAIN.BIR_YEA, '0000') & '/' & FORMAT(T_FMEMBER_MAIN.BIR_MON, '00') & '/' & FORMAT(T_FMEMBER_MAIN.BIR_DAY, '00')) <= '" & String.Format(strDateFormat, .dtBirthTo.Year, .dtBirthTo.Month, .dtBirthTo.Day) & "'"
                End If

                'search by Occupation
                If Not basCommon.fncIsBlank(.strOccupt) Then

                    strSQL &= " AND "
                    strSQL &= fncBuildQueryLike("[MEMBER_INFO.OCCUPATION]", .strOccupt)

                End If

                'search by career position
                If Not basCommon.fncIsBlank(.strPosition) Then

                    strSQL &= " AND"
                    strSQL &= fncBuildQueryLike("[MEMBER_INFO.POSITION]", .strPosition)

                End If

            End With

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable
    End Function


    '   ****************************************************************** 
    '      FUNCTION   : fncGetQuickSearch 
    '      VALUE      : DataTable, table of result
    '      PARAMS     : stSearchInfo stSearch, structure of keyword
    '      MEMO       :  
    '      CREATE     : 2011/08/23  AKB Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function fncGetQuickSearch(ByVal stSearchInfo As stSearch) As DataTable

        fncGetQuickSearch = Nothing

        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""

            With stSearchInfo


                strSQL &= "SELECT DISTINCT"

                strSQL &= " MEMBER_ID"
                strSQL &= ",LAST_NAME"
                strSQL &= ",MIDDLE_NAME"
                strSQL &= ",FIRST_NAME"
                strSQL &= ",ALIAS_NAME"
                'strSQL &= ",BIRTH_DAY"
                'strSQL &= ",DECEASED_DATE"
                strSQL &= ",FORMAT(T_FMEMBER_MAIN.BIRTH_DAY, 'YYYY/MM/DD') AS BIRTH_DAY"
                strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
                strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
                strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"

                strSQL &= ",FORMAT(T_FMEMBER_MAIN.DECEASED_DATE, 'YYYY/MM/DD') AS DECEASED_DATE"
                strSQL &= ",T_FMEMBER_MAIN.DEA_DAY"
                strSQL &= ",T_FMEMBER_MAIN.DEA_MON"
                strSQL &= ",T_FMEMBER_MAIN.DEA_YEA"
                strSQL &= ",T_FMEMBER_MAIN.DEA_DAY_SUN"
                strSQL &= ",T_FMEMBER_MAIN.DEA_MON_SUN"
                strSQL &= ",T_FMEMBER_MAIN.DEA_YEA_SUN"
                strSQL &= ",T_FMEMBER_MAIN.FAMILY_ORDER"
                strSQL &= ",DECEASED"
                strSQL &= ",GENDER"
                strSQL &= ",NAME"
                strSQL &= ",AVATAR_PATH"
                strSQL &= ",[T_FMEMBER_MAIN.REMARK]"
                strSQL &= ",T_FMEMBER_MAIN.LEVEL"

                strSQL &= " FROM"

                'strSQL &= " QUICKSEARCH"

                strSQL &= " ("
                strSQL &= " SELECT "
                strSQL &= " T_FMEMBER_MAIN.MEMBER_ID"
                strSQL &= " ,T_FMEMBER_MAIN!LAST_NAME "
                strSQL &= " & ', ' & T_FMEMBER_MAIN!MIDDLE_NAME"
                strSQL &= " & ', ' & T_FMEMBER_MAIN!FIRST_NAME"
                strSQL &= " & ', ' & T_FMEMBER_MAIN!ALIAS_NAME AS NAME"
                strSQL &= ",T_FMEMBER_MAIN.BIRTH_DAY"
                strSQL &= ",T_FMEMBER_MAIN.BIR_DAY"
                strSQL &= ",T_FMEMBER_MAIN.BIR_MON"
                strSQL &= ",T_FMEMBER_MAIN.BIR_YEA"

                strSQL &= ",T_FMEMBER_MAIN.DECEASED_DATE"
                strSQL &= ",T_FMEMBER_MAIN.DEA_DAY"
                strSQL &= ",T_FMEMBER_MAIN.DEA_MON"
                strSQL &= ",T_FMEMBER_MAIN.DEA_YEA"
                strSQL &= ",T_FMEMBER_MAIN.DEA_DAY_SUN"
                strSQL &= ",T_FMEMBER_MAIN.DEA_MON_SUN"
                strSQL &= ",T_FMEMBER_MAIN.DEA_YEA_SUN"
                'strSQL &= ",FORMAT(T_FMEMBER_MAIN.BIRTH_DAY, 'YYYY/MM/DD') AS BIRTH_DAY"
                'strSQL &= ",FORMAT(T_FMEMBER_MAIN.DECEASED_DATE, 'YYYY/MM/DD') AS DECEASED_DATE"
                strSQL &= ",T_FMEMBER_MAIN.DECEASED"
                strSQL &= ",T_FMEMBER_MAIN.GENDER"
                strSQL &= ",T_FMEMBER_MAIN.LAST_NAME"
                strSQL &= ",T_FMEMBER_MAIN.MIDDLE_NAME"
                strSQL &= ",T_FMEMBER_MAIN.FIRST_NAME"
                strSQL &= ",T_FMEMBER_MAIN.ALIAS_NAME"
                strSQL &= ",T_FMEMBER_MAIN.AVATAR_PATH"
                strSQL &= ",T_FMEMBER_MAIN.REMARK"
                strSQL &= ",T_FMEMBER_MAIN.LEVEL"
                strSQL &= ",T_FMEMBER_MAIN.FAMILY_ORDER"
                strSQL &= " FROM"
                strSQL &= " T_FMEMBER_MAIN"
                strSQL &= " )"

                strSQL &= " WHERE"

                'keyword
                strSQL &= fncBuildQueryLike("NAME", .strKeyword)

                'search by gender
                strSQL &= " AND "
                Select Case .intGender

                    Case clsEnum.emGender.UNKNOW
                        strSQL &= " GENDER LIKE '%%'"

                    Case clsEnum.emGender.MALE
                        strSQL &= " GENDER = " & xIntSQLFormat(clsEnum.emGender.MALE)

                    Case clsEnum.emGender.FEMALE
                        strSQL &= " GENDER = " & xIntSQLFormat(clsEnum.emGender.FEMALE)

                End Select

            End With

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return objDataTable

    End Function

#End Region


#Region " メソッド "

    '   ******************************************************************
    '      FUNCTION   : 文字列型の変換 
    '      VALUE      : 戻り値　String 
    '      PARAMS     : 引数１　String  、対象文字列 
    '      MEMO       :  
    '      CREATE     : 2011/07/18  AKB     Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function ChangeStringFormat(ByVal strString As String) As String

        Dim i As Integer
        Dim strWork As String
        Dim strChar As String

        Try

            ChangeStringFormat = ""
            strWork = ""

            If InStr(1, strString, "'") = 0 Then

                'シングルクォーテーションが無い場合


                strWork = strString

            Else

                'シングルクォーテーションが有る場合


                For i = 1 To Len(strString)

                    strChar = Mid$(strString, i, 1)

                    If Asc(strChar) = 39 Then

                        strChar = "' || CHR$(39) || '"

                    End If

                    strWork = strWork & strChar

                Next i

            End If

            strWork = "'" & strWork & "'"

            Return strWork

        Catch ex As Exception

            Throw New clsDbAException("ChangeStringFormat", ex)

        End Try

    End Function


    '   ****************************************************************** 
    '      FUNCTION   : 日付型の変換  
    '      VALUE      : 戻り値　String  
    '      PARAMS     : 引数１　String  、対象文字列  
    '                 : 引数２　String  、形式フラグ　0:yyyy/MM/dd  1:yyyy/MM/dd HH:mm  
    '      MEMO       :   
    '      CREATE     : 2011/07/18  AKB     Quyet 
    '      UPDATE     :  
    '   ******************************************************************
    Public Function ChangeDateFormat(ByVal dateNow As Date, _
                                     ByVal intFlg As Integer) As String

        Try

            ChangeDateFormat = ""
            '値があるなら


            If IsNothingDate(dateNow) = False Then

                If intFlg = 0 Then

                    Return "'" + dateNow.ToString("yyyy/MM/dd") & "'"

                ElseIf intFlg = 1 Then

                    Return "'" + dateNow.ToString("yyyy/MM/dd HH:mm:ss") & "'"

                End If

            Else

                Return "Null"

            End If

        Catch ex As Exception

            Throw New clsDbAException("ChangeDateFormat", ex)

        End Try

    End Function


    '   ******************************************************************
    '      FUNCTION   : 日付型の変換(Where句用) 
    '      VALUE      : 戻り値　String 
    '      PARAMS     : 引数１　String  、対象文字列 
    '      MEMO       :  
    '      CREATE     : 2009/08/026　AKB    Quyet   
    '      UPDATE     :  
    '   ******************************************************************
    Public Function ChangeDateFormatWhere(ByVal dateNow As Date) As String

        Try

            '型キャストして返す

            Return "cdate('" & Format$(dateNow, "yyyy/MM/dd HH:mm:ss") & "')"

        Catch ex As Exception

            Throw New clsDbAException("ChangeDateFormatWhere", ex)

        End Try

    End Function


    '******************************************************************
    '   FUNCTION : SQL用文字列整形 
    '   VALUE    : Integer、整形後の文字列 
    '   PARAMS   : vintNullvalue、MinValue
    '   MEMO     : 無し 
    '   CREATE   : 2011/07/18 AKB   Quyet
    '   UPDATE   : 
    '******************************************************************   
    Private Function xIntSQLFormat(ByVal vintValue As Integer, _
                                   Optional ByVal vintNullvalue As Integer = Integer.MinValue) As String

        Try

            Dim intNum As Integer

            'ここでのNull文字とは、 Chr(0) とする。


            intNum = vintValue


            'この後はこれまでどおり。


            If intNum = vintNullvalue Then

                Return "NULL"

            Else

                Return CStr(vintValue)

            End If

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '******************************************************************
    '   FUNCTION : SQL用文字列整形 
    '   VALUE    : Integer、整形後の文字列 
    '   PARAMS   : vintNullvalue、MinValue
    '   MEMO     : 無し 
    '   CREATE   : 2011/07/18 AKB   Quyet
    '   UPDATE   : 

    '******************************************************************  
    Private Function xDblSQLFormat(ByVal vintValue As Integer, _
                                   Optional ByVal vintNullvalue As Integer = Integer.MinValue) As String

        Try

            Dim intNum As Integer

            'ここでのNull文字とは、 Chr(0) とする。


            intNum = vintValue

            'この後はこれまでどおり。


            If intNum = vintNullvalue Then

                Return "NULL"

            Else

                Return CStr(vintValue)

            End If

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '******************************************************************
    '   FUNCTION : SQL用文字列整形 
    '   VALUE    : String、整形後の文字列  
    '   PARAMS   : String、SQL文    
    '   MEMO     : 無し  
    '   CREATE   : 2010/11/11 AKB Quyet 
    '   UPDATE   :  
    '******************************************************************       
    Private Function xStrSQLFormat(ByVal vobjSQL As Object) As String

        Try

            Dim strTmp As String

            strTmp = ""

            If IsNothing(vobjSQL) Or IsDBNull(vobjSQL) Then

                strTmp = ""

            Else

                strTmp = CStr(vobjSQL)

            End If

            'Null文字が途中に入っていたら、それを取り除く


            'ここでのNull文字とは、 Chr(0) とする。


            strTmp = Me.xRemoveNullChar(strTmp)

            'この後はこれまでどおり。


            If strTmp.Length <= 0 Then
                Return "NULL"
            Else

                'strTmp = strTmp.Replace("\", "\\")
                strTmp = strTmp.Replace("[", "[[]")
                Return "'" + strTmp.Replace("'", "''") + "'"

            End If

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try


    End Function


    '******************************************************************
    '   FUNCTION : Null文字(Chr(0))が途中に入っていたら、それを取り除く


    '   VALUE    : String、整形後の文字列 
    '   PARAMS   : String、文字列 
    '   MEMO     : 無し 
    '   CREATE   : 2009/08/27 AKB   Quyet
    '   UPDATE   : 
    '******************************************************************     
    Private Function xRemoveNullChar(ByVal strValue As String) As String

        Dim strTmp As String
        Dim intIndex As Integer

        Try

            'Null文字が途中に入っていたら、それを取り除く


            'ここでのNull文字とは、 Chr(0) とする。


            strTmp = strValue

            'Nothing は "" に置き換える
            If IsNothing(strTmp) Then

                strTmp = ""

            End If


            '文字列が NULL ではないか？


            If strTmp.Length > 0 Then
                'Null文字の位置を検索
                intIndex = strTmp.IndexOf(Chr(0))

                'Null文字があったか？


                If intIndex > 0 Then
                    'あった


                    '最初の位置がNull文字ではないか？


                    If intIndex = 0 Then
                        '最初の位置がNull文字なら、空文字


                        strTmp = ""
                    Else
                        'Null文字の直前までの文字列を取得


                        strTmp = strTmp.Substring(0, intIndex)
                    End If

                End If

            End If

            Return strTmp

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

    End Function


    '   ******************************************************************
    '      FUNCTION   : xBuildQueryLike, build a string or like query
    '      VALUE      : String
    '      PARAMS1    : strField    String, field to query
    '      PARAMS2    : strQuery    String, keyword
    '      MEMO       : 
    '      CREATE     : 2011/08/10  AKB Quyet
    '      UPDATE     : 
    '   ******************************************************************
    Public Function fncBuildQueryLike(ByVal strField As String, ByVal strQuery As String) As String

        fncBuildQueryLike = ""
        Dim strArr() As String

        Try
            'trim and remove double space
            strQuery = strQuery.Trim()
            strQuery = fncRemove2Space(strQuery)
            'strQuery = xStrSQLFormat(strQuery)

            'split into array
            strArr = strQuery.Split(New Char() {" "c})

            'start concat
            For i As Integer = 0 To strArr.Length - 2

                'build to get string like : FIELDNAME Like '%abc%'
                fncBuildQueryLike &= " " & strField
                fncBuildQueryLike &= " LIKE "
                fncBuildQueryLike &= xStrSQLFormat("%" & strArr(i) & "%")
                fncBuildQueryLike &= " AND"

            Next

            'concat with the last item
            fncBuildQueryLike &= " " & strField
            fncBuildQueryLike &= " LIKE "
            fncBuildQueryLike &= xStrSQLFormat("%" & strArr(strArr.Length - 1) & "%")

            Exit Function

        Catch ex As Exception
            Throw ex
        Finally
            Erase strArr
        End Try

    End Function

#End Region


#Region "Generation"


    ''' <summary>
    ''' Get generation of a member
    ''' </summary>
    ''' <param name="intMemberID">member id</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncGetMemberGeneration(ByVal intMemberID As Integer) As Integer

        Dim intGen As Integer = basConst.gcintNONE_VALUE
        Dim objDataTable As DataTable

        objDataTable = Nothing

        Try

            Dim strSQL As String = ""
            Dim strResult As String = ""

            strSQL &= "SELECT"
            strSQL &= " T_FMEMBER_MAIN.LEVEL"
            strSQL &= " FROM"
            strSQL &= " T_FMEMBER_MAIN"
            strSQL &= " WHERE"
            strSQL &= " T_FMEMBER_MAIN.MEMBER_ID = " & xIntSQLFormat(intMemberID)

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Return intGen

            'get result
            strResult = fncCnvNullToString(objDataTable.Rows(0).Item("LEVEL"))

            'convert to int
            If Not Integer.TryParse(strResult, intGen) Then Return -1

            Return intGen

        Catch ex As Exception

            Throw New clsDbAException(ex.Message, ex)

        End Try

        Return intGen

    End Function


    ''' <summary>
    ''' Find someone is downline of someone
    ''' </summary>
    ''' <param name="intRootID">Top member</param>
    ''' <param name="intMemberID">Bottom member</param>
    ''' <param name="intSpouseID">Spouse of bottom member</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncIsDownlineOf(ByVal intRootID As Integer, ByVal intMemberID As Integer, Optional ByVal intSpouseID As Integer = -1) As Boolean

        fncIsDownlineOf = False

        Dim objDataTable As DataTable = Nothing

        Try
            Dim strSQL As String = ""

            strSQL &= " Select "
            strSQL &= " rel_fmember_id"
            strSQL &= " from"
            strSQL &= " [T_FMEMBER_RELATION]"
            strSQL &= " where"
            strSQL &= " ("
            strSQL &= " member_id = " & intMemberID
            If intSpouseID > basConst.gcintNO_MEMBER Then strSQL &= " or member_id= " & intSpouseID
            strSQL &= " ) "
            strSQL &= " and relid=2 "

            For i As Integer = 0 To 45
                strSQL = String.Format("Select rel_fmember_id from [T_FMEMBER_RELATION] where member_id in ({0})", strSQL)
            Next

            strSQL &= " and rel_fmember_id = " & intRootID

            objDataTable = GetTable(strSQL)

            If objDataTable IsNot Nothing Then _
                If objDataTable.Rows.Count = 0 Then Exit Function

            Return True

        Catch ex As Exception
            Throw ex
        End Try

    End Function


    ''' <summary>
    ''' Set generation of a member
    ''' </summary>
    ''' <param name="intGen">generation</param>
    ''' <param name="intMemberID">member id, pass -1 to set all</param>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public Function fncSetMemberGeneration(ByVal intGen As Integer, Optional ByVal intMemberID As Integer = basConst.gcintNO_MEMBER, Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncSetMemberGeneration = False

        Dim objDataTable As DataTable = Nothing
        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""

            strSQL &= " update T_FMEMBER_MAIN "
            strSQL &= " set "

            If intGen > 0 Then
                strSQL &= " T_FMEMBER_MAIN.LEVEL = " & intGen
            Else
                strSQL &= " T_FMEMBER_MAIN.LEVEL = null"
            End If

            If intMemberID > basConst.gcintNO_MEMBER Then
                strSQL &= " Where MEMBER_ID = " & intMemberID
            End If

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            Execute(strSQL)

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception

            If blnBeginTrans Then Me.RollBack()
            Throw ex

        Finally
            objDataTable = Nothing
        End Try

    End Function


    ''' <summary>
    ''' Set generation from a member
    ''' </summary>
    ''' <param name="intLevel">level deepth</param>
    ''' <param name="intGen">generation to set</param>
    ''' <param name="intStartID">start member</param>
    ''' <param name="blnStop">return true if there is no child</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function fncSetGeneration(ByVal intLevel As Integer, ByVal intGen As Integer, ByVal intStartID As Integer, ByRef blnStop As Boolean, Optional ByVal blnIsRollBack As Boolean = True) As Boolean

        fncSetGeneration = False
        Dim objDataTable As DataTable = Nothing
        Dim blnBeginTrans As Boolean = False

        Try
            Dim strSQL As String = ""
            Dim strQueryChild As String = ""
            Dim strQueryChildCore As String = ""
            Dim strTemp As String = ""
            Dim intRow As Integer

            strQueryChild &= " update T_FMEMBER_MAIN"
            strQueryChild &= " set T_FMEMBER_MAIN.LEVEL = {0}"
            strQueryChild &= " where"
            strQueryChild &= " T_FMEMBER_MAIN.member_id in ({1})"

            strQueryChildCore &= " select "
            strQueryChildCore &= " t_fmember_relation.member_id"
            strQueryChildCore &= " from"
            strQueryChildCore &= " t_fmember_relation"
            strQueryChildCore &= " where"
            strQueryChildCore &= " ("
            strQueryChildCore &= " relid = 2"
            strQueryChildCore &= " or "
            strQueryChildCore &= " relid = 4"
            strQueryChildCore &= " ) "
            strQueryChildCore &= " and "
            strQueryChildCore &= " rel_fmember_id = " & intStartID

            'build query child
            For i As Integer = 0 To intLevel - 1

                strTemp = ""
                strTemp &= " select "
                strTemp &= " t_fmember_relation.member_id"
                strTemp &= " from"
                strTemp &= " t_fmember_relation"
                strTemp &= " where"
                strTemp &= " rel_fmember_id in "
                strTemp &= " ("
                strTemp &= strQueryChildCore
                strTemp &= " ) "
                strTemp &= " and "
                strTemp &= " (relid = 2 or relid = 4)"

                strQueryChildCore = strTemp
            Next

            'build query update child level
            strSQL = String.Format(strQueryChild, intGen, strQueryChildCore)

            If blnIsRollBack Then blnBeginTrans = Me.BeginTransaction()

            intRow = Execute(strSQL)

            'build query update child's spouse
            strSQL = String.Format("update T_FMEMBER_MAIN set T_FMEMBER_MAIN.LEVEL = {0} where T_FMEMBER_MAIN.member_id in (SELECT MEMBER_ID from T_FMEMBER_RELATION where RELID=1 and REL_FMEMBER_ID in({1}))", intGen, strQueryChildCore)
            Execute(strSQL)

            If intRow <= 0 Then blnStop = True

            If blnBeginTrans Then Me.Commit()

            Return True

        Catch ex As Exception
            If blnBeginTrans Then Me.RollBack()
            Throw ex
        Finally
            If objDataTable IsNot Nothing Then objDataTable.Dispose()
            objDataTable = Nothing
        End Try

    End Function


#End Region


End Class