using GP40Common;
using System;
using System.Collections;

namespace GP40DrawTree
{
    public class GenDataMemberTest
    {
        public const string RootId = "0";
        public static Hashtable CreateData(clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            return (new GenDataMemberTest()).xCreateTree(enTemplate);
        }

        private int intMemberCount = 0;
        private int intMaxFLevelCount = 3;

        private Hashtable xCreateTree(clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            var hasMember = new Hashtable();

            clsFamilyMember objMember = xCreateMember(RootId, 1, enTemplate, clsConst.ENUM_GENDER.Male, null, 0, 1);
            hasMember.Add(objMember.miID, objMember);
            intMemberCount = 1;
            xMakeTreeRelation(hasMember, RootId, enTemplate);
            //objFTree.MakeTreeDrawWithSpouse(0, 10, 10, clsConst.ENUM_FIRSTSPOUSE_POSITION.LeftMember);

            return hasMember;
        }

        private clsFamilyMember xCreateMember(string intMemberID, int intFrame,
            clsConst.ENUM_MEMBER_TEMPLATE enTemplate, clsConst.ENUM_GENDER enGender, clsFamilyMember objFather = null, int intIndexChild = 0, int intLevel = 1)
        {
            clsFamilyMember objMember = new clsFamilyMember();

            objMember.miID = intMemberID;
            objMember.InitMemberInfo(enTemplate, enGender);

            if (objFather != null)
            {
                if (intIndexChild != 0)
                {
                    objMember.FLevel = intLevel.ToString() + ".[" + objFather.FLevel.Replace("[", "").Replace("]", "") + "]." + intIndexChild.ToString();
                }
            }
            else
            {
                objMember.FLevel = intLevel.ToString();
                if (intIndexChild != 0)
                {
                    objMember.FLevel = intLevel.ToString() + "." + intIndexChild.ToString();
                }
            }

            objMember.intFLevel = intLevel;
            //objMember.DrawingMember();
            // objMember.DrawingMemberSVG();

            return objMember;
        }

        //Automactic Make a Tree by intMaxFLevelCount, number of children is random
        private void xMakeTreeRelation(Hashtable hasMember, string intMember, clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            clsFamilyMember objMember = (clsFamilyMember)hasMember[intMember];
            if (objMember.intFLevel >= intMaxFLevelCount)
            {
                return;
            }

            Random r = new Random();
            int rChild = r.Next(1, 5); //Number of child randmom

            rChild = 5;
            for (int i = 0; i < rChild; i++)
            {
                clsFamilyMember objMemberChild = xCreateMember(intMemberCount.ToString(), 1, enTemplate, clsConst.ENUM_GENDER.Male, objMember, i + 1, objMember.intFLevel + 1);
                //objMemberChild.FLevel = objMember.FLevel + "." + (i + 1).ToString();
                objMember.lstChild.Add(objMemberChild.miID);
                hasMember.Add(objMemberChild.miID, objMemberChild);
                intMemberCount++;
            }

            int rSpouse = r.Next(1, 2); //Number of Spouse Random
            rSpouse = 3;
            for (int i = 0; i < rSpouse; i++)
            {
                clsFamilyMember objMemberSpouse = xCreateMember(intMemberCount.ToString(), 1, enTemplate, clsConst.ENUM_GENDER.FMale, null, 0, objMember.intFLevel);
                objMember.lstSpouse.Add(objMemberSpouse.miID);
                hasMember.Add(objMemberSpouse.miID, objMemberSpouse);
                intMemberCount++;
            }

            for (int i = 0; i < rChild; i++)
            {
                xMakeTreeRelation(hasMember, objMember.lstChild[i], enTemplate);
            }
        }
    }
}
