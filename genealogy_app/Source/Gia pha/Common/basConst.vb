'   ******************************************************************
'      TITLE      : Constant Declaration
'　　　FUNCTION   :
'      MEMO       : 
'      CREATE     : 2011/07/15　AKB　Quyet
'      UPDATE     : 
'
'           2011 AKB SOFTWARE
'   ******************************************************************
Option Explicit On
Option Strict On

'   ******************************************************************
'　　　FUNCTION   : Constant Declaration Class
'      MEMO       : 
'      CREATE     : 2011/07/15　AKB　Quyet
'      UPDATE     : 
'   ******************************************************************

Imports Config_Gia_Pha

''' <summary>
''' Date structure
''' </summary>
''' <create>2012/04/12　AKB Quyet</create>
''' <remarks></remarks>
Public Structure stCalendar

    Dim intDay As Integer
    Dim intMonth As Integer
    Dim intYear As Integer

    Public Function IsValidDate() As Boolean

        IsValidDate = False
        If (intDay <= 0 And intMonth <= 0 And intYear <= 0) Then Return False
        If (intMonth > 12) Then Return False
        Dim intDayinMonth As Integer
        intDayinMonth = DateTime.DaysInMonth(intYear, intMonth)

        If intDay > intDayinMonth Then Return False

        Return True

    End Function

    Public Function IsYearMonthDayisInputed() As Boolean

        If (intDay > 0) Then Return True
        If (intMonth > 0) Then Return True
        If (intYear > 0) Then Return True

        Return False

    End Function

End Structure

Public Structure stBasicCardInfo

    Dim strFullName As String               'full name
    Dim strFirstName As String          'Bfirst name
    Dim strMidName As String            'middle name    
    Dim strLastName As String           'last name
    Dim strAlias As String              'alias
    Dim strImgLocation As String        'image

    'Dim dtBirth As Date                 'DOB
    Dim stBirthDaySun As stCalendar

    'Dim dtDeath As Date                 'DOD
    Dim stDeadDayMoon As stCalendar
    Dim stDeadDaySun As stCalendar
    Dim strDeadLunarYearName As String

    Dim intGender As Integer            'gender
    Dim intDecease As Integer           'decease flag

End Structure

Module basConst
    'd033e22ae348aeb5660fc2140aec35850c4da997 (=admin)
    Public Const gcstrRegeditValue As String = "128c8332f28e6ae495ba0b097b940843"
    Public Const gcstrRegeditKey As String = "897356954c2cd3d41b221e3f24f99bba"
    Public Const gcstrDBPATH As String = "\Data\"                        'path to database
    Public Const gcstrImageFolder As String = "\images\"                 'path to image folder
    Public Const gcstrMemberImageFolder As String = "\images\Album"      'path to image folder
    Public Const gcstrDocsFolder As String = "\docs\"                    'path to docs folder
    Public Const gcstrFrameFolder As String = "frames"
    Public Const gcstrTempFolder As String = "\temp\"                    'path to temporary folder
    Public Const gcstrNoteFolder As String = "\notes\"                   'path to note folder
    Public Const gcstrAvatarPath As String = "\avatar\"                  'path to image files
    Public Const gcstrAvatarThumbPath As String = "\thumbnail\"          'path to image files
    Public Const gcstrAlbumPath As String = "\album\"                    'path to image files in family's album
    Public Const gcstrBackupFolder As String = "\backuptemp\"            'path to backup folder
    Public Const gcstrDocFile As String = "default.doc"                  'path to document files
    Public Const gcstrDocTempData As String = "tempdata.doc"             'path to document files - temporary data file
    Public Const gcstrDocTemplate As String = "template.doc"             'path to document files - template file
    'Public Const gcstrXltPath1 As String = "template.xlt"                'path to template file
    Public Const gcstrXltPath1 As String = "template.xls"                'path to template file
    'Public Const gcstrXltPath2 As String = "PhaHe.xlt"                   'path to template file
    Public Const gcstrXltPath2 As String = "PhaHe.xls"                   'path to template file
    Public Const gcstrXmlDoc As String = "Notes.xml"                     'path to template file
    Public Const gcstrPdfGuide As String = "guide.pdf"                   'path to guideline file
    Public Const gcstrDBNAME As String = "giaphadb.mdb"                  'database name
    Public gcstrDBPASS As String = Config.DBPass                         'database password
    Public gcstrXltPass As String = Config.XltPass                       'password for xlt template file
    Public gcstrBackupPass As String = Config.BackupPass                 'password for backup and restore file
    Public Const gcstrNoAvatar As String = "noavatar.jpg"                'no avatar file name
    Public Const gcstrDocTemplate_new As String = "TempMemberInfo.doc"             'path to document files - template file
    Public Const gcstrDocXml As String = "FieldConfig.xml"             'path to document files - template file

    Public Const gcstrEncryptFormat As String = "x2"                     'x is to produce string in hexadecimal lowercase, 2 is minimum number in output
    Public Const gcstrAlphabetFormat As String = "[^A-Za-z0-9]"          'regex to check character is not A-Z a-z 0-9
    Public Const gcstrUsrCardFileFormat As String = "{0}{1}.png"         'file format to save usercard

    Public Const gcstrProductName As String = "Phần mềm Gia Phả"            'product name

    Public Const gcstrDateFormat1 As String = "{0:MM/dd/yyyy}"           'string to format datetime
    Public Const gcstrDateFormat2 As String = "{0:dd/MM/yyyy}"           'string to format datetime
    Public Const gcstrImgFormat As String = "{0:000000}"                 'string to format image name
    Public Const gcstrNameFormat As String = "{0} {1} {2}"               'output name format
    Public Const gcstrRowFilterFormat As String = "{0} = {1} "           'row filter string format
    Public Const gcstrMemberFilter As String = " MEMBER_ID = {0}"        'filter by member
    Public Const gcstrNameWithAlias As String = "{0} ({1})"              'name with alias format

    Public Const gcstrFieldFather As String = "FATHER"                   'Father field name
    Public Const gcstrFieldSon As String = "SON"                         'Son field name
    Public Const gcstrFieldLevel As String = "LEVEL"                     'Level field name

    Public Const gcstrDefaultNation As String = "1"                      'Nationality ID of Vietnam
    Public Const gcstrDefaultRelition As String = "1"                    'Religion : Khong ro

    Public Const gcintRootID As Integer = 0                              'root member id
    Public Const gcintNO_MEMBER As Integer = 0                           'default value for no member
    Public Const gcintNONE_VALUE As Integer = -1                         'default none value

    Public Const gcstrGenderMALE As String = "Nam"                       'Gender is male
    Public Const gcstrGenderFEMALE As String = "Nữ"                      'Gender is female
    Public Const gcstrGenderUNKNOW As String = "Không rõ"                'Gender is unknow

    '2016/12/21 Manh Start Add 
    Public Const gcstrDeadDateUNKNOWText As String = "Đã Mất"                'DeadDate is Unknow

    Public Const gcstrFather As String = "Cha"                           'display text of Father
    Public Const gcstrMother As String = "Mẹ"                            'display text of Mother
    Public Const gcstrHusband As String = "Chồng"                        'display text of Husband
    Public Const gcstrWife As String = "Vợ"                              'display text of Wife
    Public Const gcstrBrother As String = "Anh"                          'display text of Brother
    Public Const gcstrSister As String = "Chị"                            'display text of Sister
    Public Const gcstrYounger As String = "Em"                           'display text of Younger brother/sister
    Public Const gcstrKid As String = "Con"                              'display text of Child
    Public Const gcstrBoy As String = "trai"                             'display text of Boy
    Public Const gcstrGirl As String = "gái"                             'display text of Girl
    Public Const gcstrAdopt As String = "nuôi"                           'display text of foster
    Public gcstrServerPass As String = Config.ServerPass

    Public Const gcintALIVE As Integer = 0                               'Member is alive
    Public Const gcintDIED As Integer = 1                                'Member died

    Public Const gcintMinYear As Integer = 1800                           'minimum year is 1800/01/25
    Public Const gcintMinMonth As Integer = 1                             '=========================
    Public Const gcintMinDay As Integer = 25                              '=========================
    Public Const gcintMaxYear As Integer = 2199                           'maximum year is 2199
    Public Const gcstrDateUnknown As String = "Chưa rõ"                    ' text of unknown date

    'Public Const gcintTypeEDU As Integer = 1                              'Education Type for T_FMEMBER_CAREER table
    'Public Const gcintTypeCAREER As Integer = 2                           'Career Type for T_FMEMBER_CAREER table

    'Public Const gcintModeADD As Integer = 0                             'Add new mode
    'Public Const gcintModeEDIT As Integer = 1                            'Edit mode
    'Public Const gcintModeVIEW As Integer = 2                            'View mode

    'Public Const gcintTblMember As Integer = 1                           'table T_FMEMBER_MAIN
    'Public Const gcintTblCareer As Integer = 2                           'table T_FMEMBER_CAREER
    'Public Const gcintTblFact As Integer = 3                             'table T_FMEMBER_FACT
    'Public Const gcintTblNation As Integer = 4                           'table M_NATIONALITY
    'Public Const gcintTblReligion As Integer = 5                         'table M_RELIGION
    'Public Const gcintTblFamilyImage As Integer = 6                      'table M_FAMILY_IMAGE

    'Public Const gcintRelMarriage As Integer = 1                         'Marriage Relationship 
    'Public Const gcintRelBlood As Integer = 2                            'Blood relationship
    'Public Const gcintRelAdopt As Integer = 4                            'Adoptive relationship

    Public Const gcintSheetNo As Integer = 1                             'default worksheet number
    Public Const gcintXlsSheetGray As Integer = 16                       'color index - gray
    Public Const gcintXlsSheetTan As Integer = 40                        'color index - tan
    Public Const gcintXlsFontWhite As Integer = 2                        'color index - white
    Public Const gcintXlsTriStateFalse As Integer = 0                    'excel constant
    Public Const gcintXlsTriStateTrue As Integer = -1                     'excel constant
    Public Const gcintLineStyleContinuous As Integer = 1                 'border line style
    Public Const gcintEdgeBorderBotton As Integer = 9                    'border edge

    Public Const gcintLimitMember As Integer = 30                        'limit the number of people in trial version
    Public Const gcintTimerInterval As Integer = 15                      'timer interval when moving object, in milisecond
    Public Const gcintAnimateTime As Integer = 24                        'num of animation time

    Public Const gcstrFindNotFound As String = "Không có thành viên nào được tìm thấy !"                  'no member found
    Public Const gcstrNoUserSelected As String = "Không có thành viên nào được chọn."                   'no member selected
    Public Const gcstrNoExcel As String = "Bạn cần cài đặt Microsoft Excel để có thể sử dụng chức năng này."      'no excel application

    Public Const gcstrFail As String = "Thất bại."                                                'fail message
    Public Const gcstrMessageConfirm As String = "Thành viên {0} sẽ bị xóa, bạn có chắc chắn?"         'message confirm when deleting a member
    Public Const gcstrDelSpouseRelation As String = "Quan hệ vợ chồng sẽ bị xóa, bạn có chắc chắn?"        'delete confirmation

    Public Const gcstrImageFilter As String = "JPG|*.jpg|PNG|*.png|BMP|*.bmp|GIF|*.gif|All files|*.*"               'Excel filter string
    Public Const gcstrExcelFilter As String = "Ms Excel|*.xls|All files|*.*"                                        'Excel filter string
    Public Const gcstrExcelExt As String = ".xls"                                                                   'Excel file extension
    Public Const gcstrBackupFileExt As String = ".gpb"                                                              'Backup file extension
    Public Const gcstrBackupFileFilter As String = "Giapha backup|*.gpb"                                            'Backup file filter
    Public Const gcstrDefaultFrame As String = "pic_frame.png"

    Public gblnDrawTreeAdvance As Boolean = False
    Public gintPercent As Integer = 0
    Public gblnIsConfirmDraw As Boolean = False
    Public Const gcstrTemplatePass As String = "giaphapwd"
    Public gdblFaChildConnWeight As Double = 3
    Public gdblParentConnWeight As Double = 4

    Public gintTreePanelDPIX As Integer
    Public gintTreePanelDPIY As Integer
    'Public gtblMemberCard As Hashtable                   'table to store drawing card

    'Thong tin phien ban 500
    'Public Const gcstrVersion As String = "AV05"
    'Public Const gcintMaxLimit As Integer = 500

    'Thong tin phien ban 1000
    'Public Const gcstrVersion As String = "AV10"
    'Public Const gcintMaxLimit As Integer = 1000

    'Thong tin phien ban khong gio han
    'Public Const gcstrVersion As String = "AV99"
    'Public Const gcintMaxLimit As Integer = 88888888

    'Start - Edit by: 2018.10.08 AKB Nguyen Thanh Tung
    'Thong tin phien ban 500
    Public Const gcstrVersion500 As String = "AV05"
    Public Const gcintMaxLimit500 As Integer = 500

    'Thong tin phien ban 1000
    Public Const gcstrVersion1000 As String = "AV10"
    Public Const gcintMaxLimit1000 As Integer = 1000

    'Thong tin phien ban khong gio han
    Public Const gcstrVersionUltimate As String = "AV99"
    Public Const gcintMaxLimitUltimate As Integer = 88888888

    'Thong tin phien ban hien tai
    Public gcintMaxLimit As Integer = gcintMaxLimit500

    Public gcstrZipPassUpdate As String = Config.ZipPassUpdate
    'End   - Edit by: 2018.10.08 AKB Nguyen Thanh Tung

    '2017/01/10 Manh ADD, when add card to panel in Windows 7, it changes the height of Card
    Public gintHeightDiff As Integer = 0

    Public Structure stCardInfo

        Dim intID As Integer                 'ID of this person
        Dim intX As Integer
        Dim intY As Integer
        Dim intLevel As Integer
        Dim intMaxRight As Integer
        Dim intMinLeft As Integer

        Dim intFatherID As Integer
        Dim intMotherID As Integer
        Dim lstChild As List(Of Integer)     'List of children
        Dim lstSpouse As List(Of Integer)    'List of Spouse
        Dim lstStepChild As List(Of Integer) 'List of Step children
        Dim lstSibling As List(Of Integer) 'List of Step children

        '2016/12/27 - Manh Start
        Dim stBasicInfo As stBasicCardInfo
        '2016/12/27 - Manh End
    End Structure

    Public Structure stExportInfo

        Dim objTreeType As Object           'S1 or A1 or S2
        Dim tblControl As Hashtable
        Dim tblMemberInfo As Hashtable
        Dim lstNormalLine As List(Of GiaPha.usrLine)
        Dim lstSpecialLine As List(Of GiaPha.usrLine)
        Dim intCardStyle As Integer
        Dim intRootID As Integer

    End Structure

End Module


Public Class clsEnum

    Public Enum emFamily_Flag

        IN_FAMILY = 1
        NOT_IN_FAMILY = 0

    End Enum

    Public Enum emGender

        MALE = 1
        FEMALE = 2
        UNKNOW = 0

    End Enum

    Public Enum RegistryLocation
        CurrentUser = 0
        Machine = 1
        Users = 2
    End Enum

    Public Enum emRelation

        NONE = -1
        MARRIAGE = 1
        NATURAL = 2
        ADOPT = 4

    End Enum

    Public Enum emMode

        ADD = 0
        EDIT = 1
        VIEW = 2

    End Enum

    Public Enum emSelect

        CASE1
        CASE2
        CASE3

    End Enum

    Public Enum emTable

        T_FMEMBER_MAIN
        T_FMEMBER_CAREER
        T_FMEMBER_FACT
        M_NATIONALITY
        M_RELIGION
        M_FAMILY_IMAGE
        M_FAMILY_HEAD
        M_ROOT

    End Enum

    Public Enum emCareerType

        EDU = 1                                         'Education Type for T_FMEMBER_CAREER table
        CAREER = 2                                      'Career Type for T_FMEMBER_CAREER table

    End Enum

    Public Enum emInputType

        DETAIL = 0
        GENERAL = 1

    End Enum

    Public Enum IMAGE

        S_THUMBNAIL_H = 16
        S_THUMBNAIL_W = 16

    End Enum

    Public Enum emMenuItem

        ADD_FA              'add father
        ADD_MO              'add mother
        ADD_FA_A            'add adopt father
        ADD_MO_A            'add adopt mother
        ADD_FM_L            'add fa/mo from list
        DEL_FM              'delete fa/mo relationship

        ADD_HW              'add hus/wife
        ADD_HW_L            'add hus/wife from list

        ADD_BRO             'add brother    
        ADD_SIS             'add sister
        ADD_BRO_Y           'add younger brother
        ADD_SIS_Y           'add younger sister
        ADD_BS_L            'add brother/sister from list

        ADD_SON             'add son
        ADD_DAU             'add daughter
        ADD_KID_A           'add adopt kid
        ADD_KID_LIST        'add kid from list

    End Enum

    Public Enum emLineDirection

        HORIZONTAL = 0

        VERTICAL = 1

    End Enum

    Public Enum emLineType

        SINGLE_LINE = 0

        DOUBLE_LINE = 1

        MULTI_LINE = 3

    End Enum

    Public Enum emCardPoint

        MID_TOP
        MID_LEFT
        MID_RIGHT
        MID_BOTTOM

    End Enum

    Public Enum AddPosition

        LEFT = 0

        CENTER = 1

    End Enum

    Public Enum emCardSize

        MINI = 0

        SMALL = 1

        MIDDLE = 2

        LARGE = 3

        ORTHER = 4

    End Enum

    Public Enum emCardStyle

        CARD1 = 1
        CARD2 = 2

    End Enum

    'Public Enum Gender

    '    UNKNOW = 0

    '    MALE = 1

    '    FEMALE = 2

    'End Enum

    Public Enum CardLevel

        GRANDPARENTS = 0

        PARENTS = 1

        CHILDREN = 2

    End Enum

    Public Enum CardAddType
        BASE_CARD = 0
    End Enum


    Public Enum SearchGrid

        NO = 0
        LEVEL = 1
        NAME = 2
        GENDER = 3
        CONTACT = 4
        HOMETOWN = 5
        B_DATE = 6
        D_DATE = 7
        NOTE_TEMP = 8
        NOTE = 9
        DIED = 10

    End Enum

    Public Enum DeadDateShowType

        SUN_CALENDAR = 1
        MOON_CALENDAR = 2

    End Enum

    '   ******************************************************************
    '　　　	Enum       : emTypeCardShort
    '      	MEMO       : Type tree card short
    '      	CREATE     : 2018/04/24 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Enum emTypeCardShort

        Horizontal = 0
        Vertical = 1

    End Enum

    '   ******************************************************************
    '　　　	Enum       : emTypeShowTree
    '      	MEMO       : Type show member in tree
    '      	CREATE     : 2018/04/24 AKB Nguyen Thanh Tung
    '      	UPDATE     : 
    '   ******************************************************************
    Public Enum emTypeShowTree

        All = 0
        OnlyShowMale = 1
        OnlyShowMember = 2

    End Enum

    Public Enum emTypeDrawText
        Normal
        RotateLeft
        RotateRight
    End Enum
End Class

'   ******************************************************************
'　　　	CLASS      : clsDataSourceComboBox
'      	MEMO       : Create Data to ComboBox 
'      	CREATE     : 2018/04/27 AKB Nguyen Thanh Tung
'      	UPDATE     : 
'   ******************************************************************
Public Class clsDataSourceComboBox
    Private _display As String
    Private _value As Object

    Public Property Display() As String
        Get
            Return _display
        End Get
        Set(value As String)
            _display = value
        End Set
    End Property

    Public Property Value() As Object
        Get
            Return _value
        End Get
        Set(value As Object)
            _value = value
        End Set
    End Property
End Class