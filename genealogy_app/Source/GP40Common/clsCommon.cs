using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GP40Common
{
    static public class clsCommon
    {
        public static MemberCardTemplFull mcTemplFull = new MemberCardTemplFull();
        public static MemberCardTemplShort mcTemplShort = new MemberCardTemplShort();
        public static MemberCardTemplVeryShort mcTemplVeryShort = new MemberCardTemplVeryShort();
        public static MemberCardTemplTall mcTemplTall = new MemberCardTemplTall();
        public static MemberCardTemplInput mcTemplInput = new MemberCardTemplInput();

        public static void InitDataDirectory()
        {
            if (!Directory.Exists(clsConst.DataPath)) Directory.CreateDirectory(clsConst.DataPath);
            if (!Directory.Exists(clsConst.MemberCardPath)) Directory.CreateDirectory(clsConst.MemberCardPath);                     

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

        static public string Convert2String(object objValue)
        {
            if (objValue == null) return "";
            return objValue.ToString();
        }

        static public bool IsNumber(object objValue)
        {
            double dblNum;
            if (!double.TryParse(Convert2String(objValue), out dblNum))
                return false;

            return true;
        }

        static public int ConvertToInt(object objValue)
        {
            if (objValue == null) return 0;

            if (IsNumber(objValue)) return Convert.ToInt32(objValue);

            return 0;
        }

        static public int ConvertToInt(TextBox txtValue)
        {
            return ConvertToInt(txtValue.Text);
        }
    }
}


