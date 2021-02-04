using GP40Common;
using GP40DrawTree;
using GP40Main.Core;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static GP40Main.Core.AppConst;

namespace GP40Main.Models
{
    public static class ExtendMethodModel
    {

        public static DrawTreeConfig ToConfig(this ThemeConfig theme)
        {
            var rst = new DrawTreeConfig() {
                DataMember = new Hashtable(),
            };

            if (theme != null)
            {
                rst.BackgroudColor = ColorDrawHelper.FromHtml(theme.BackgroudColor);
                rst.SelectedMemberColor = ColorDrawHelper.FromHtml(theme.SelectedMemberColor);
                rst.ChildLineColor = ColorDrawHelper.FromHtml(theme.ChildLineColor);
                rst.SpouseLineColor = ColorDrawHelper.FromHtml(theme.SpouseLineColor);
                rst.TextColor = ColorDrawHelper.FromHtml(theme.TextColor);
                rst.BorderColor = ColorDrawHelper.FromHtml(theme.BorderColor);
                rst.MaleBackColor = ColorDrawHelper.FromHtml(theme.MaleBackColor);
                rst.FeMaleBackColor = ColorDrawHelper.FromHtml(theme.FeMaleBackColor);

                rst.MemberVerticalSpace = theme.MemberVerticalSpace;
                rst.MemberHorizonSpace = theme.MemberHorizonSpace;

                rst.NumberFrame = theme.NumberFrame;
            }

            return rst;
        }

        public static clsFamilyMember ToDataDraw(this TMember objMember, clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            if (objMember == null)
            {
                return null;
            }

            var rst = new clsFamilyMember();
            rst.miID = objMember.Id;
            rst.FrameImage = clsConst.FramePath + "01.png";

            if (objMember.Gender == (int)GenderMember.Male)
            {
                rst.Gender = clsConst.ENUM_GENDER.Male;
            }
            else if (objMember.Gender == (int)GenderMember.Female)
            {
                rst.Gender = clsConst.ENUM_GENDER.FMale;
            }
            else
            {
                rst.Gender = clsConst.ENUM_GENDER.Unknown;
            }

            rst.FullName = objMember.Name;
            rst.BDayS = objMember.Birthday.DaySun > 0 ? objMember.Birthday.DaySun.ToString().PadLeft(2, '0') : "--";
            rst.BMonthS = objMember.Birthday.MonthSun > 0 ? objMember.Birthday.MonthSun.ToString().PadLeft(2, '0') : "--";
            rst.BYearS = objMember.Birthday.YearSun > 0 ? objMember.Birthday.YearSun.ToString().PadLeft(2, '0') : "--";
            rst.TemplateType = enTemplate;
            rst.lstChild = objMember.ListCHILDREN;
            rst.lstSpouse = objMember.ListSPOUSE;
            rst.lstParent = objMember.ListPARENT;

            return rst;
        }

        public static Hashtable CreateDataTree(this TMember objMember, clsConst.ENUM_MEMBER_TEMPLATE enTemplate, Dictionary<string, TMember> dataMember = null)
        {
            if (objMember == null)
            {
                return new Hashtable();
            }

            if (dataMember == null)
            {
                using (var objTable = AppManager.DBManager.GetTable<TMember>())
                {
                    dataMember = objTable.AsEnumerable().ToDictionary(i => i.Id);
                }
            }

            if (dataMember == null || dataMember.Count == 0 || string.IsNullOrEmpty(objMember.Id) || !dataMember.ContainsKey(objMember.Id))
            {
                return new Hashtable();
            }

            var hasMember = new Hashtable();
            hasMember.Add(objMember.Id, dataMember[objMember.Id].ToDataDraw(enTemplate));

            xMakeTreeRelation(hasMember, dataMember, objMember.Id, enTemplate);

            return hasMember;
        }

        private static void xMakeTreeRelation(Hashtable hasMember, Dictionary<string, TMember> dataDB, string id, clsConst.ENUM_MEMBER_TEMPLATE enTemplate)
        {
            var objMemberDraw = (clsFamilyMember)hasMember[id];

            //if (objMember.intFLevel >= intMaxFLevelCount)
            //{
            //    return;
            //}

            if (objMemberDraw.lstChild != null && objMemberDraw.lstChild.Count > 0)
            {
                var lstUpdate = new List<string>();
                var flagUpdate = false;

                foreach (var idChilden in objMemberDraw.lstChild)
                {
                    if (!dataDB.ContainsKey(idChilden))
                    {
                        flagUpdate = true;
                        continue;
                    }

                    lstUpdate.Add(idChilden);
                    hasMember.Add(idChilden, dataDB[idChilden].ToDataDraw(enTemplate));
                    xMakeTreeRelation(hasMember, dataDB, idChilden, enTemplate);
                }

                if (flagUpdate)
                {
                    objMemberDraw.lstChild = lstUpdate;
                }
            }

            if (objMemberDraw.lstSpouse != null && objMemberDraw.lstSpouse.Count > 0)
            {
                var lstUpdate = new List<string>();
                var flagUpdate = false;

                foreach (var idSpouse in objMemberDraw.lstSpouse)
                {
                    if (!dataDB.ContainsKey(idSpouse))
                    {
                        flagUpdate = true;
                        continue;
                    }

                    lstUpdate.Add(idSpouse);
                    hasMember.Add(idSpouse, dataDB[idSpouse].ToDataDraw(enTemplate));
                }

                if (flagUpdate)
                {
                    objMemberDraw.lstSpouse = lstUpdate;
                }
            }
        }
    }

    public class ExTMember : TMember
    {
        public string BirthdayShow { get; set; }
        public string BirthdayLunarShow { get; set; }
        public string DeadDaySunShow { get; set; }
        public string DeadDayLunarShow { get; set; }

        public string GenderShow { get; set; }
        public string Tel_1 { get; set; }
        public string Tel_2 { get; set; }
        public string Email_1 { get; set; }
        public string Email_2 { get; set; }
        public string Address { get; set; }
        public string RelTypeShow { get; set; }
    }

    public class ExTMemberJob : TMemberJob
    {
        public string TimeShow { get; set; }
    }

    public class ExTMemberSchool : TMemberSchool
    {
        public string TimeShow { get; set; }
    }

    public class ExTMemberEvent : TMemberEvent
    {
        public string TimeShow { get; set; }
    }

    public class ExMRelation : MRelation
    {
        public string MainRelationNameShow { get; set; }
        public string RelatedRelationNameShow { get; set; }
    }
}
