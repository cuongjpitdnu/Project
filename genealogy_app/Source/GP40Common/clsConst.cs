using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GP40Common
{
    public class clsConst
    {
        public static string FramePath = @".\Docs\frames\";
        public static string AvartaPath = @".\Docs\";
        public static string DataPath = @".\Data\";
        public static string MemberCardPath = DataPath + @"\MC\";

        public static int MemberVerticalSpace = 30;
        public static int MemberHorizonSpace = 30;

        public enum ENUM_FIRSTSPOUSE_POSITION
        {
            LeftMember = 0,
            RightMember
        }

        public enum ENUM_GENDER
        {
            Male = 0,
            FMale,
            Unknown
        }

        public enum ENUM_MEMBER_TEMPLATE
        {
            MCTFull = 0,
            MCTShort,
            MCTVeryShort,
            MCTTall,
            MCTInput
        }
        public static string[] TEMPLATE_NAME = Enum.GetNames(typeof(ENUM_MEMBER_TEMPLATE));
                
        public enum ENUM_MEMBER_TEMPLATE_CONTROL
        {
            picFrame = 0,
            picImage,
            lblFullName,
            lblBirthDate,
            lblDeadDate,
            lblLevel,
            lblHomeTown
        }

        public static string[] MEMBER_CONTROL_NAME = Enum.GetNames(typeof(ENUM_MEMBER_TEMPLATE_CONTROL));

        public static string picFrame   // property
        {
            get { return MEMBER_CONTROL_NAME[(int)ENUM_MEMBER_TEMPLATE_CONTROL.picFrame]; }   // get method          
        }

        public static string picImage   // property
        {
            get { return MEMBER_CONTROL_NAME[(int)ENUM_MEMBER_TEMPLATE_CONTROL.picImage]; }   // get method          
        }

        public static string lblFullName   // property
        {
            get { return MEMBER_CONTROL_NAME[(int)ENUM_MEMBER_TEMPLATE_CONTROL.lblFullName]; }   // get method          
        }

        public static string lblBirthDate   // property
        {
            get { return MEMBER_CONTROL_NAME[(int)ENUM_MEMBER_TEMPLATE_CONTROL.lblBirthDate]; }   // get method          
        }

        public static string lblDeadDate   // property
        {
            get { return MEMBER_CONTROL_NAME[(int)ENUM_MEMBER_TEMPLATE_CONTROL.lblDeadDate]; }   // get method          
        }

        public static string lblLevel   // property
        {
            get { return MEMBER_CONTROL_NAME[(int)ENUM_MEMBER_TEMPLATE_CONTROL.lblLevel]; }   // get method          
        }
    }
}
