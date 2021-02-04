using GP40Common;
using GP40DrawTree;
using GPConst;
using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Views.Tree.Build;
using GPModels;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GPMain.Views.Tree
{
    public static class ExtendMethodModel
    {
        public static DrawTreeConfig ToConfig(this ThemeConfig theme)
        {
            var rst = new DrawTreeConfig()
            {
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
                rst.UnknowBackColor = ColorDrawHelper.FromHtml(theme.UnknowBackColor);

                rst.MemberVerticalSpace = theme.MemberVerticalSpace;
                rst.MemberHorizonSpace = theme.MemberHorizonSpace;

                rst.NumberFrame = theme.NumberFrame;

                rst.ShowBirthDayDefaul = theme.ShowBirthDayDefaul;
                rst.ShowDeathDayLunarCalendar = theme.ShowDeathDayLunarCalendar;
                rst.ShowFamilyLevel = theme.ShowFamilyLevel;
                rst.ShowGender = theme.ShowGender;
                rst.ShowImage = theme.ShowImage;
                rst.TypeTextShow = theme.TypeTextShow;
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
            rst.Id = objMember.Id;
            rst.FrameImage = clsConst.FramePath + "01.png";

            if (objMember.Gender == (int)EmGender.Male)
            {
                rst.Gender = clsConst.ENUM_GENDER.Male;
            }
            else if (objMember.Gender == (int)EmGender.FeMale)
            {
                rst.Gender = clsConst.ENUM_GENDER.FMale;
            }
            else
            {
                rst.Gender = clsConst.ENUM_GENDER.Unknown;
            }

            if (!string.IsNullOrEmpty(objMember.AvatarImg))
            {
                rst.Image = Application.StartupPath + "\\" + AppConst.AvatarThumbnailPath + "\\" + string.Format(AppConst.FormatNameThumbnailTree, objMember.AvatarImg);
            }

            MemberHelper memberHelper = new MemberHelper();
            rst.FullName = string.IsNullOrEmpty(objMember.ShowName) ? memberHelper.CreateShowName(objMember.Name) : objMember.ShowName;

            rst.BDayS = objMember.Birthday.DaySun > 0 ? objMember.Birthday.DaySun.ToString().PadLeft(2, '0') : "--";
            rst.BMonthS = objMember.Birthday.MonthSun > 0 ? objMember.Birthday.MonthSun.ToString().PadLeft(2, '0') : "--";
            rst.BYearS = objMember.Birthday.YearSun > 0 ? objMember.Birthday.YearSun.ToString().PadLeft(2, '0') : "--";

            rst.BDayM = objMember.Birthday.DayMoon > 0 ? objMember.Birthday.DayMoon.ToString().PadLeft(2, '0') : "--";
            rst.BMonthM = objMember.Birthday.MonthMoon > 0 ? objMember.Birthday.MonthMoon.ToString().PadLeft(2, '0') : "--";
            rst.BYearM = objMember.Birthday.YearMoon > 0 ? objMember.Birthday.YearMoon.ToString().PadLeft(2, '0') : "--";

            rst.DDayS = objMember.DeadDay.DaySun > 0 ? objMember.DeadDay.DaySun.ToString().PadLeft(2, '0') : "--";
            rst.DMonthS = objMember.DeadDay.MonthSun > 0 ? objMember.DeadDay.MonthSun.ToString().PadLeft(2, '0') : "--";
            rst.DYearS = objMember.DeadDay.YearSun > 0 ? objMember.DeadDay.YearSun.ToString().PadLeft(2, '0') : "--";

            rst.DDayM = objMember.DeadDay.DayMoon > 0 ? objMember.DeadDay.DayMoon.ToString().PadLeft(2, '0') : "--";
            rst.DMonthM = objMember.DeadDay.MonthMoon > 0 ? objMember.DeadDay.MonthMoon.ToString().PadLeft(2, '0') : "--";
            rst.DYearM = objMember.DeadDay.YearMoon > 0 ? objMember.DeadDay.YearMoon.ToString().PadLeft(2, '0') : "--";

            rst.TemplateType = enTemplate;
            rst.lstChild = objMember.ListCHILDREN;
            rst.lstSpouse = objMember.ListSPOUSE;
            rst.lstParent = objMember.ListPARENT;
            rst.LevelInFamily = objMember.LevelInFamily.ToString();
            rst.strBackColor = objMember.strBackColor;
            rst.strForeColor = objMember.strForeColor;
            rst.UseDefaultColor = objMember.UseDefaultColor;
            rst.ShowStepChild = objMember.ShowStepChild;
            return rst;
        }

        public static Hashtable CreateDataTree(this TMember objMember, clsConst.ENUM_MEMBER_TEMPLATE enTemplate, int MaxLevelInFamily, Dictionary<string, TMember> dataMember = null)
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

            xMakeTreeRelation(hasMember, dataMember, objMember.Id, enTemplate, MaxLevelInFamily);

            return hasMember;
        }

        private static void xMakeTreeRelation(Hashtable hasMember, Dictionary<string, TMember> dataDB, string id, clsConst.ENUM_MEMBER_TEMPLATE enTemplate, int MaxLevelInFamily)
        {
            var objMemberDraw = (clsFamilyMember)hasMember[id];
            objMemberDraw.InRootTree = true;
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

                    clsFamilyMember clsChild = dataDB[idChilden].ToDataDraw(enTemplate);
                    if (int.Parse(clsChild.LevelInFamily) > MaxLevelInFamily)
                    {
                        flagUpdate = true;
                        break;
                    }
                    lstUpdate.Add(idChilden);
                    clsChild.InRootTree = true;
                    if (!hasMember.ContainsKey(idChilden))
                    {
                        hasMember.Add(idChilden, clsChild);
                        xMakeTreeRelation(hasMember, dataDB, idChilden, enTemplate, MaxLevelInFamily);
                    }
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

                    if (!lstUpdate.Contains(idSpouse))
                    {
                        lstUpdate.Add(idSpouse);
                    }
                    if (!hasMember.ContainsKey(idSpouse))
                    {
                        hasMember.Add(idSpouse, dataDB[idSpouse].ToDataDraw(enTemplate));
                    }
                }

                if (flagUpdate)
                {
                    objMemberDraw.lstSpouse = lstUpdate;
                }
            }
        }
    }
}