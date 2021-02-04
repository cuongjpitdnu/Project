using GP40DrawTree;
using System;
using System.Windows.Forms;

namespace GP40Main.Core
{
    public class AppConst
    {
        public const string PassEncryptString = "akb@1564ltt";

        public const string FormatFormTitleBase = "{0} - {1}";

        public const string LogPath = @"\Logs\";

        public const string DatabaseJsonPath = @"\Data\";
        public const string DatabaseName = "giapha.db";
        public const string DatabasePass = "akb@wTeo7f89";

        public const string FolderFrameCardPath = @"Data\Docs\frames\";

        public enum ModeForm
        {
            None,
            New,
            Edit,
            View,
        }

        public enum GenderMember
        {
            Unknown = 0,
            Male = 1,
            Female = 2
        }

        public enum DrawThemes
        {
            Defaul1 = -1,
            Defaul2 = 0,
            Defaul3 = 1,
            User1 = 2,
            User2 = 3
        }

        public const string cstrPreFixDAD = "DAD";
        public const string cstrPreFixMOM = "MOM";
        public const string cstrPreFixHUSBAND = "HUS";
        public const string cstrPreFixWIFE = "WIF";
        public const string cstrPreFixCHILD = "CHI";
    }

    public static class UIDefault
    {
        public static void SetDefaultUI(this ContainerControl container)
        {
            if (container == null)
            {
                return;
            }

            container.BackColor = ColorDrawHelper.FromHtmlToColor("#f2f4f6");
        }

        public static void SafeInvoke(this ContainerControl container, Action action)
        {
            if (action == null)
            {
                return;
            }

            if (container.InvokeRequired)
            {
                container.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }
    }
}
