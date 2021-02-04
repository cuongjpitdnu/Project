using System;
using System.Drawing;
using System.Windows.Forms;

namespace GPMain.Common
{

    public enum ModeForm : int
    {
        None,
        New,
        Edit,
        View,
    }

    public enum ModeDisplay
    {
        BuildTree,
        ViewTree,
    }

    public class AppConst
    {
        public const string FormatFormTitleBase = "{0} - {1} ({2})";
        public const string LogPath = @"\Logs\";
        public const string BackupAppPath = @"\\BackupApp\\";
        public const string BackupDBPath = @"\\BackupDB\\";

        public const string DatabaseJsonPath = @"\Data\";
        public const string AvatarFolderPath = @"\Data\Avatar\";
        public const string AvatarThumbnailPath = @"\Data\Avatar\thumbnail\";
        public const string AvatarRawPath = @"\Data\Avatar\raw\";
        public const string DatabaseName = "giapha.db";
        public const string DatabasePass = "akb@wTeo7f89";

        public const string FolderFrameCardPath = @"Data\Docs\frames\";
        public const string DocumentFilesPath = @"Data\DocumentFiles\";

        public const string FileVersion = @"/txtVersion.txt";
        public const string FileUpdate = @"/NewVersion.zip";

        public const string AlbumFolderPath = @"\Data\Album\";
        public const string AlbumImageFolderPath = @"\Data\Album\images\";
        public const string AlbumThumbnailFolderPath = @"\Data\Album\thumbnail\";
        public const string PageAbout = @"\Data\Docs\about.html";
        public const string PageDownLoad = @"\Data\Docs\about.html";
        public const string PageDownLoadName = @"about.html";
        public const string FormatNameThumbnailTree = "tree_{0}";

        public const string TemplateExcelForEvent = @"\EventsTemplate.xlsx";
        public const string FormatNumber = "{0:#,0}";
        public const string FormatDate = "00";
        public const string FormatLevelInFamilyShow = "00";
        public const string TitleBarFisrt = "Phần mềm quản lý gia phả - ";
        public static Color StatusBarBackColor = Color.FromArgb(251, 141, 61);
        public static Color StatusBarForeColor = Color.FromArgb(30, 48, 62);
        public const string StepChild = "Con riêng";
        public static Color PopupBackColor = Color.FromArgb(245, 255, 250);

        public static StatusBarColor StatusBarColor { get; set; } = new StatusBarColor() { BackColor = StatusBarBackColor, ForeColor = StatusBarForeColor };
        private static string[] _DayofWeek = { "Chủ Nhật", "Thứ Hai", "Thứ Ba", "Thứ Tư", "Thứ Năm", "Thứ Sáu", "Thứ Bảy" };
        public static string GetDayOfWeek(DateTime dateTime)
        {
            return _DayofWeek[(int)dateTime.DayOfWeek];
        }
        public struct Gender
        {
            public static string Male = "Nam";
            public static string Female = "Nữ";
            public static string Unknow = "Chưa rõ";
        }
        public struct Status
        {
            public static string Alive = "Còn sống";
            public static string IsDeath = "Đã mất";
        }
        public struct FontFamily
        {
            public static string Agency_FB = "Agency FB";
            public static string Algerian = "Algerian";
            public static string Arial = "Arial";
            public static string Arial_Black = "Arial Black";
            public static string Arial_Narrow = "Arial Narrow";
            public static string Arial_Rounded_MT_Bold = "Arial Rounded MT Bold";
            public static string Arial_Unicode_MS = "Arial Unicode MS";
            public static string Bahnschrift = "Bahnschrift";
            public static string Bahnschrift_Condensed = "Bahnschrift Condensed";
            public static string Bahnschrift_Light = "Bahnschrift Light";
            public static string Bahnschrift_Light_Condensed = "Bahnschrift Light Condensed";
            public static string Bahnschrift_Light_SemiCondensed = "Bahnschrift Light SemiCondensed";
            public static string Bahnschrift_SemiBold = "Bahnschrift SemiBold";
            public static string Bahnschrift_SemiBold_Condensed = "Bahnschrift SemiBold Condensed";
            public static string Bahnschrift_SemiBold_SemiConden = "Bahnschrift SemiBold SemiConden";
            public static string Bahnschrift_SemiCondensed = "Bahnschrift SemiCondensed";
            public static string Bahnschrift_SemiLight = "Bahnschrift SemiLight";
            public static string Bahnschrift_SemiLight_Condensed = "Bahnschrift SemiLight Condensed";
            public static string Bahnschrift_SemiLight_SemiConde = "Bahnschrift SemiLight SemiConde";
            public static string Baskerville_Old_Face = "Baskerville Old Face";
            public static string Bauhaus_93 = "Bauhaus 93";
            public static string Bell_MT = "Bell MT";
            public static string Berlin_Sans_FB = "Berlin Sans FB";
            public static string Berlin_Sans_FB_Demi = "Berlin Sans FB Demi";
            public static string Bernard_MT_Condensed = "Bernard MT Condensed";
            public static string Blackadder_ITC = "Blackadder ITC";
            public static string Bodoni_MT = "Bodoni MT";
            public static string Bodoni_MT_Black = "Bodoni MT Black";
            public static string Bodoni_MT_Condensed = "Bodoni MT Condensed";
            public static string Bodoni_MT_Poster_Compressed = "Bodoni MT Poster Compressed";
            public static string Book_Antiqua = "Book Antiqua";
            public static string Bookman_Old_Style = "Bookman Old Style";
            public static string Bookshelf_Symbol_7 = "Bookshelf Symbol 7";
            public static string Bradley_Hand_ITC = "Bradley Hand ITC";
            public static string Britannic_Bold = "Britannic Bold";
            public static string Broadway = "Broadway";
            public static string Brush_Script_MT = "Brush Script MT";
            public static string Calibri = "Calibri";
            public static string Calibri_Light = "Calibri Light";
            public static string Californian_FB = "Californian FB";
            public static string Calisto_MT = "Calisto MT";
            public static string Cambria = "Cambria";
            public static string Cambria_Math = "Cambria Math";
            public static string Candara = "Candara";
            public static string Candara_Light = "Candara Light";
            public static string Castellar = "Castellar";
            public static string Centaur = "Centaur";
            public static string Century = "Century";
            public static string Century_Gothic = "Century Gothic";
            public static string Century_Schoolbook = "Century Schoolbook";
            public static string Chiller = "Chiller";
            public static string Colonna_MT = "Colonna MT";
            public static string Comic_Sans_MS = "Comic Sans MS";
            public static string Consolas = "Consolas";
            public static string Constantia = "Constantia";
            public static string Cooper_Black = "Cooper Black";
            public static string Copperplate_Gothic_Bold = "Copperplate Gothic Bold";
            public static string Copperplate_Gothic_Light = "Copperplate Gothic Light";
            public static string Corbel = "Corbel";
            public static string Corbel_Light = "Corbel Light";
            public static string Courier_New = "Courier New";
            public static string Curlz_MT = "Curlz MT";
            public static string Ebrima = "Ebrima";
            public static string Edwardian_Script_ITC = "Edwardian Script ITC";
            public static string Elephant = "Elephant";
            public static string Engravers_MT = "Engravers MT";
            public static string Eras_Bold_ITC = "Eras Bold ITC";
            public static string Eras_Demi_ITC = "Eras Demi ITC";
            public static string Eras_Light_ITC = "Eras Light ITC";
            public static string Eras_Medium_ITC = "Eras Medium ITC";
            public static string Felix_Titling = "Felix Titling";
            public static string Footlight_MT_Light = "Footlight MT Light";
            public static string Forte = "Forte";
            public static string Franklin_Gothic_Book = "Franklin Gothic Book";
            public static string Franklin_Gothic_Demi = "Franklin Gothic Demi";
            public static string Franklin_Gothic_Demi_Cond = "Franklin Gothic Demi Cond";
            public static string Franklin_Gothic_Heavy = "Franklin Gothic Heavy";
            public static string Franklin_Gothic_Medium = "Franklin Gothic Medium";
            public static string Franklin_Gothic_Medium_Cond = "Franklin Gothic Medium Cond";
            public static string Freestyle_Script = "Freestyle Script";
            public static string French_Script_MT = "French Script MT";
            public static string Gabriola = "Gabriola";
            public static string Gadugi = "Gadugi";
            public static string Garamond = "Garamond";
            public static string Georgia = "Georgia";
            public static string Gigi = "Gigi";
            public static string Gill_Sans_MT = "Gill Sans MT";
            public static string Gill_Sans_MT_Condensed = "Gill Sans MT Condensed";
            public static string Gill_Sans_MT_Ext_Condensed_Bold = "Gill Sans MT Ext Condensed Bold";
            public static string Gill_Sans_Ultra_Bold = "Gill Sans Ultra Bold";
            public static string Gill_Sans_Ultra_Bold_Condensed = "Gill Sans Ultra Bold Condensed";
            public static string Gloucester_MT_Extra_Condensed = "Gloucester MT Extra Condensed";
            public static string Goudy_Old_Style = "Goudy Old Style";
            public static string Goudy_Stout = "Goudy Stout";
            public static string Haettenschweiler = "Haettenschweiler";
            public static string Harlow_Solid_Italic = "Harlow Solid Italic";
            public static string Harrington = "Harrington";
            public static string High_Tower_Text = "High Tower Text";
            public static string HoloLens_MDL2_Assets = "HoloLens MDL2 Assets";
            public static string Impact = "Impact";
            public static string Imprint_MT_Shadow = "Imprint MT Shadow";
            public static string Informal_Roman = "Informal Roman";
            public static string Ink_Free = "Ink Free";
            public static string Javanese_Text = "Javanese Text";
            public static string Jokerman = "Jokerman";
            public static string Juice_ITC = "Juice ITC";
            public static string Kristen_ITC = "Kristen ITC";
            public static string Kunstler_Script = "Kunstler Script";
            public static string Leelawadee = "Leelawadee";
            public static string Leelawadee_UI = "Leelawadee UI";
            public static string Leelawadee_UI_Semilight = "Leelawadee UI Semilight";
            public static string Lucida_Bright = "Lucida Bright";
            public static string Lucida_Calligraphy = "Lucida Calligraphy";
            public static string Lucida_Console = "Lucida Console";
            public static string Lucida_Fax = "Lucida Fax";
            public static string Lucida_Handwriting = "Lucida Handwriting";
            public static string Lucida_Sans = "Lucida Sans";
            public static string Lucida_Sans_Typewriter = "Lucida Sans Typewriter";
            public static string Lucida_Sans_Unicode = "Lucida Sans Unicode";
            public static string Magneto = "Magneto";
            public static string Maiandra_GD = "Maiandra GD";
            public static string Malgun_Gothic = "Malgun Gothic";
            public static string Malgun_Gothic_Semilight = "Malgun Gothic Semilight";
            public static string Marlett = "Marlett";
            public static string Matura_MT_Script_Capitals = "Matura MT Script Capitals";
            public static string Meiryo = "Meiryo";
            public static string Meiryo_UI = "Meiryo UI";
            public static string Microsoft_Himalaya = "Microsoft Himalaya";
            public static string Microsoft_JhengHei = "Microsoft JhengHei";
            public static string Microsoft_JhengHei_Light = "Microsoft JhengHei Light";
            public static string Microsoft_JhengHei_UI = "Microsoft JhengHei UI";
            public static string Microsoft_JhengHei_UI_Light = "Microsoft JhengHei UI Light";
            public static string Microsoft_New_Tai_Lue = "Microsoft New Tai Lue";
            public static string Microsoft_PhagsPa = "Microsoft PhagsPa";
            public static string Microsoft_Sans_Serif = "Microsoft Sans Serif";
            public static string Microsoft_Tai_Le = "Microsoft Tai Le";
            public static string Microsoft_Uighur = "Microsoft Uighur";
            public static string Microsoft_YaHei = "Microsoft YaHei";
            public static string Microsoft_YaHei_Light = "Microsoft YaHei Light";
            public static string Microsoft_YaHei_UI = "Microsoft YaHei UI";
            public static string Microsoft_YaHei_UI_Light = "Microsoft YaHei UI Light";
            public static string Microsoft_Yi_Baiti = "Microsoft Yi Baiti";
            public static string MingLiU_ExtB = "MingLiU-ExtB";
            public static string MingLiU_HKSCS_ExtB = "MingLiU_HKSCS-ExtB";
            public static string Mistral = "Mistral";
            public static string Modern_No_20 = "Modern No. 20";
            public static string Mongolian_Baiti = "Mongolian Baiti";
            public static string Monotype_Corsiva = "Monotype Corsiva";
            public static string MS_Gothic = "MS Gothic";
            public static string MS_Outlook = "MS Outlook";
            public static string MS_PGothic = "MS PGothic";
            public static string MS_Reference_Sans_Serif = "MS Reference Sans Serif";
            public static string MS_Reference_Specialty = "MS Reference Specialty";
            public static string MS_UI_Gothic = "MS UI Gothic";
            public static string MT_Extra = "MT Extra";
            public static string MV_Boli = "MV Boli";
            public static string Myanmar_Text = "Myanmar Text";
            public static string Niagara_Engraved = "Niagara Engraved";
            public static string Niagara_Solid = "Niagara Solid";
            public static string Nirmala_UI = "Nirmala UI";
            public static string Nirmala_UI_Semilight = "Nirmala UI Semilight";
            public static string NSimSun = "NSimSun";
            public static string OCR_A_Extended = "OCR A Extended";
            public static string Old_English_Text_MT = "Old English Text MT";
            public static string Onyx = "Onyx";
            public static string Palace_Script_MT = "Palace Script MT";
            public static string Palatino_Linotype = "Palatino Linotype";
            public static string Papyrus = "Papyrus";
            public static string Parchment = "Parchment";
            public static string Perpetua = "Perpetua";
            public static string Perpetua_Titling_MT = "Perpetua Titling MT";
            public static string Playbill = "Playbill";
            public static string PMingLiU_ExtB = "PMingLiU-ExtB";
            public static string Poor_Richard = "Poor Richard";
            public static string Pristina = "Pristina";
            public static string Rage_Italic = "Rage Italic";
            public static string Ravie = "Ravie";
            public static string Rockwell = "Rockwell";
            public static string Rockwell_Condensed = "Rockwell Condensed";
            public static string Rockwell_Extra_Bold = "Rockwell Extra Bold";
            public static string Script_MT_Bold = "Script MT Bold";
            public static string Segoe_MDL2_Assets = "Segoe MDL2 Assets";
            public static string Segoe_Print = "Segoe Print";
            public static string Segoe_Script = "Segoe Script";
            public static string Segoe_UI = "Segoe UI";
            public static string Segoe_UI_Black = "Segoe UI Black";
            public static string Segoe_UI_Emoji = "Segoe UI Emoji";
            public static string Segoe_UI_Historic = "Segoe UI Historic";
            public static string Segoe_UI_Light = "Segoe UI Light";
            public static string Segoe_UI_Semibold = "Segoe UI Semibold";
            public static string Segoe_UI_Semilight = "Segoe UI Semilight";
            public static string Segoe_UI_Symbol = "Segoe UI Symbol";
            public static string Showcard_Gothic = "Showcard Gothic";
            public static string SimSun = "SimSun";
            public static string SimSun_ExtB = "SimSun-ExtB";
            public static string Sitka_Banner = "Sitka Banner";
            public static string Sitka_Display = "Sitka Display";
            public static string Sitka_Heading = "Sitka Heading";
            public static string Sitka_Small = "Sitka Small";
            public static string Sitka_Subheading = "Sitka Subheading";
            public static string Sitka_Text = "Sitka Text";
            public static string Snap_ITC = "Snap ITC";
            public static string Stencil = "Stencil";
            public static string Sylfaen = "Sylfaen";
            public static string Symbol = "Symbol";
            public static string Tahoma = "Tahoma";
            public static string TeamViewer15 = "TeamViewer15";
            public static string Tempus_Sans_ITC = "Tempus Sans ITC";
            public static string Times_New_Roman = "Times New Roman";
            public static string Trebuchet_MS = "Trebuchet MS";
            public static string Tw_Cen_MT = "Tw Cen MT";
            public static string Tw_Cen_MT_Condensed = "Tw Cen MT Condensed";
            public static string Tw_Cen_MT_Condensed_Extra_Bold = "Tw Cen MT Condensed Extra Bold";
            public static string Verdana = "Verdana";
            public static string Viner_Hand_ITC = "Viner Hand ITC";
            public static string Vivaldi = "Vivaldi";
            public static string Vladimir_Script = "Vladimir Script";
            public static string Webdings = "Webdings";
            public static string Wide_Latin = "Wide Latin";
            public static string Wingdings = "Wingdings";
            public static string Wingdings_2 = "Wingdings 2";
            public static string Wingdings_3 = "Wingdings 3";
            public static string Yu_Gothic = "Yu Gothic";
            public static string Yu_Gothic_Light = "Yu Gothic Light";
            public static string Yu_Gothic_Medium = "Yu Gothic Medium";
            public static string Yu_Gothic_UI = "Yu Gothic UI";
            public static string Yu_Gothic_UI_Light = "Yu Gothic UI Light";
            public static string Yu_Gothic_UI_Semibold = "Yu Gothic UI Semibold";
            public static string Yu_Gothic_UI_Semilight = "Yu Gothic UI Semilight";
            public static string Helvetica = "Helvetica";
            public static string Roboto = "Roboto";
        }
        public static string FontName = FontFamily.Helvetica;
        public static int ScrollWidth
        {
            get
            {
                return SystemInformation.VerticalScrollBarWidth;
            }
        }
        public static int ScrollHeight
        {
            get
            {
                return SystemInformation.HorizontalScrollBarHeight;
            }
        }
        public struct NameDefaul
        {
            public static string Father = "Thêm cha";
            public static string Mother = "Thêm mẹ";
            public static string Other = "Thêm thành viên";
            public static string Husban = "Thêm chồng";
            public static string Wife = "Thêm vợ";
            public static string Child = "Thêm con";
        }
        public enum PositionUserCreate
        {
            Top_Left, Top_Right, Bottom_Left, Bottom_Right
        }
        public string DateFormat = "00";
        public static Font FontUserCreateForExportPDF { get; set; } = new Font(FontFamily.Microsoft_Sans_Serif, 36, FontStyle.Regular);
        public static Font FontFamilyInfoForExportPDF { get; set; } = new Font(FontFamily.Microsoft_Sans_Serif, 72, FontStyle.Bold);
    }

    public class StatusBarColor
    {
        public Color BackColor { get; set; }
        public Color ForeColor { get; set; }

        public static Color GetColotFromHTML(string html)
        {
            return ColorTranslator.FromHtml(html);
        }
    }
}
