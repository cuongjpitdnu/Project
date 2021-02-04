using System.IO;

namespace GP40Common
{
    static public class clsCommon
    {
        public static MemberCardTemplFull mcTemplFull = new MemberCardTemplFull();
        public static MemberCardTemplShort mcTemplShort = new MemberCardTemplShort();
        public static MemberCardTemplVeryShort mcTemplVeryShort = new MemberCardTemplVeryShort();
        public static MemberCardTemplTall mcTemplTall = new MemberCardTemplTall();
        public static MemberCardTemplInput mcTemplInput = new MemberCardTemplInput();
        public static MemberCardTemplVeryShortTurnLeft mcTemplVeryShortTurnLeft = new MemberCardTemplVeryShortTurnLeft();
        public static MemberCardTemplVeryShortTurnRight mcTemplVeryShortTurnRight = new MemberCardTemplVeryShortTurnRight();

        public static void InitDataDirectory()
        {
            //if (!Directory.Exists(clsConst.DataPath)) Directory.CreateDirectory(clsConst.DataPath);
            //if (!Directory.Exists(clsConst.MemberCardPath)) Directory.CreateDirectory(clsConst.MemberCardPath);

            if (Directory.Exists(clsConst.MemberCardPath)) Directory.Delete(clsConst.MemberCardPath, true);

            Directory.CreateDirectory(clsConst.MemberCardPath);

            for (int i = 0; i < clsConst.TEMPLATE_NAME.Length; i++)
            {
                string strTemp;
                strTemp = clsConst.MemberCardPath + clsConst.TEMPLATE_NAME[i];
                if (!Directory.Exists(strTemp)) Directory.CreateDirectory(strTemp);
            }
        }

        public static string GetMemberCardDataPath(clsConst.ENUM_MEMBER_TEMPLATE enmTemplate)
        {
            return clsConst.MemberCardPath + clsConst.TEMPLATE_NAME[(int)enmTemplate] + "\\";
        }
    }
}