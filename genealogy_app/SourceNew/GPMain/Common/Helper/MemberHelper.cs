using GP40Common;
using GPConst;
using GPMain.Common;
using GPMain.Core;
using GPMain.Views.Member;
using GPModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMain.Common.Helper
{
    /// <summary>
    /// Meno: Get,set infomation member
    /// Create by: Nguyên Văn Hải
    /// </summary>
    public class MemberHelper : IDisposable
    {
        #region Lấy danh sách thành viên kết quả trả về kiểu List
        public List<ExTMember> FindMember(string Keyword, string Gender, string Status)
        {
            try
            {
                Keyword = string.IsNullOrEmpty(Keyword) ? "" : Keyword;
                Gender = string.IsNullOrEmpty(Gender) ? "" : Gender;
                Status = string.IsNullOrEmpty(Status) ? "" : Status;
                string keyword = Keyword;
                int intGender = Gender.Equals(AppConst.Gender.Male) ? 0 : Gender.Equals(AppConst.Gender.Female) ? 1 : Gender.Equals(AppConst.Gender.Unknow) ? 2 : -1;
                int intLiveOrDie = Status.Equals(AppConst.Status.Alive) ? 1 : Status.Equals(AppConst.Status.IsDeath) ? 0 : -1;
                bool isDeath = intLiveOrDie == 0 ? true : false;
                keyword = keyword.ToLower();
                var tblTMember = AppManager.DBManager.GetTable<TMember>();

                var dtaTMember = tblTMember.ToList(
                    i => (intGender < 0 || i.Gender == intGender) // gender
                         && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death                 
                         && (string.IsNullOrEmpty(keyword)
                             || (i.Name ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Address ?? "").ToLower().Contains(keyword))
                    )
                    .Select(i => new ExTMember()
                    {
                        Id = i.Id,
                        Name = i.Name + "",
                        ShowName = string.IsNullOrEmpty(i.ShowName) ? CreateShowName(i.Name) : i.ShowName,
                        Gender = i.Gender,
                        IsDeath = i.IsDeath,
                        GenderShow = (i.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((i.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                        BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                        BirthdayLunarShow = i.Birthday != null ? i.Birthday.ToDateMoon() : "",
                        DeadDaySunShow = i.DeadDay != null ? i.DeadDay.ToDateSun() : "",
                        DeadDayLunarShow = i.DeadDay != null ? i.DeadDay.ToDateMoon() : "",
                        Tel_1 = i.Contact.Tel_1 + "",
                        Tel_2 = i.Contact.Tel_2 + "",
                        Email_1 = i.Contact.Email_1 + "",
                        Email_2 = i.Contact.Email_2 + "",
                        Address = i.Contact.Address + "",
                        Relation = i.Relation,
                        Religion = i.Religion,
                        National = i.National,
                        TypeName = i.TypeName,
                        HomeTown = i.HomeTown,
                        BirthPlace = i.BirthPlace,
                        DeadPlace = i.DeadPlace,
                        ListCHILDREN = i.ListCHILDREN,
                        ListPARENT = i.ListPARENT,
                        ListSPOUSE = i.ListSPOUSE,
                        AvatarImg = i.AvatarImg,
                        LevelInFamily = i.LevelInFamily,
                        LevelInFamilyOfFather = i.LevelInFamilyOfFather,
                        LevelInFamilyOfMother = i.LevelInFamilyOfMother,
                        LevelInFamilyForShow = (i.LevelInFamily.ToString().PadLeft(2, '0') + (!string.IsNullOrEmpty(i.LevelInFamilyOfFather) ? $".{i.LevelInFamilyOfFather}" : "") + (!string.IsNullOrEmpty(i.LevelInFamilyOfMother) ? $".{i.LevelInFamilyOfMother}" : "")),
                        ChildLevelInFamily = i.ChildLevelInFamily,
                        strBackColor = i.strBackColor,
                        strForeColor = i.strForeColor,
                        UseDefaultColor = i.UseDefaultColor,
                        ShowStepChild = i.ShowStepChild,
                        InRootTree = i.InRootTree,
                        SpouseInRootTree = i.SpouseInRootTree,
                        RootID = i.RootID
                    })
                    .OrderBy(x => x.LevelInFamilyForShow)
                    .ToList();

                return dtaTMember;
            }
            catch
            {
                return new List<ExTMember>();
            }
        }

        public List<ExTMember> FindMember(string Keyword, int intGender, int intLiveOrDie)
        {
            try
            {
                string keyword = string.IsNullOrEmpty(Keyword) ? "" : Keyword;
                bool isDeath = intLiveOrDie == 0 ? true : false;
                keyword = keyword.ToLower();
                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var dtaTMember = tblTMember.ToList(
                    i => (intGender < 0 || i.Gender == intGender) // gender
                         && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death                 
                         && (string.IsNullOrEmpty(keyword)
                             || (i.Name ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Address ?? "").ToLower().Contains(keyword))
                    )
                    .Select(i => new ExTMember()
                    {
                        Id = i.Id,
                        Name = i.Name + "",
                        ShowName = string.IsNullOrEmpty(i.ShowName) ? CreateShowName(i.Name) : i.ShowName,
                        Gender = i.Gender,
                        IsDeath = i.IsDeath,
                        GenderShow = (i.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((i.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                        Birthday = i.Birthday,
                        DeadDay = i.DeadDay,
                        BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                        BirthdayLunarShow = i.Birthday != null ? i.Birthday.ToDateMoon() : "",
                        DeadDaySunShow = i.DeadDay != null ? i.DeadDay.ToDateSun() : "",
                        DeadDayLunarShow = i.DeadDay != null ? i.DeadDay.ToDateMoon() : "",
                        Tel_1 = i.Contact.Tel_1 + "",
                        Tel_2 = i.Contact.Tel_2 + "",
                        Email_1 = i.Contact.Email_1 + "",
                        Email_2 = i.Contact.Email_2 + "",
                        Address = i.Contact.Address + "",
                        Relation = i.Relation,
                        Religion = i.Religion,
                        National = i.National,
                        TypeName = i.TypeName,
                        HomeTown = i.HomeTown,
                        BirthPlace = i.BirthPlace,
                        DeadPlace = i.DeadPlace,
                        ListCHILDREN = i.ListCHILDREN,
                        ListPARENT = i.ListPARENT,
                        ListSPOUSE = i.ListSPOUSE,
                        AvatarImg = i.AvatarImg,
                        LevelInFamily = i.LevelInFamily,
                        LevelInFamilyOfFather = i.LevelInFamilyOfFather,
                        LevelInFamilyOfMother = i.LevelInFamilyOfMother,
                        LevelInFamilyForShow = (i.LevelInFamily.ToString().PadLeft(2, '0') + (!string.IsNullOrEmpty(i.LevelInFamilyOfFather) ? $".{i.LevelInFamilyOfFather}" : "") + (!string.IsNullOrEmpty(i.LevelInFamilyOfMother) ? $".{i.LevelInFamilyOfMother}" : "")),
                        ChildLevelInFamily = i.ChildLevelInFamily,
                        strBackColor = i.strBackColor,
                        strForeColor = i.strForeColor,
                        UseDefaultColor = i.UseDefaultColor,
                        ShowStepChild = i.ShowStepChild,
                        Job = i.Job,
                        Event = i.Event,
                        School = i.School,
                        Contact = i.Contact,
                        InRootTree = i.InRootTree,
                        SpouseInRootTree = i.SpouseInRootTree,
                        RootID = i.RootID
                    })
                    .OrderBy(x => x.LevelInFamilyForShow)
                    .ToList();
                return dtaTMember;
            }
            catch
            {
                return new List<ExTMember>();
            }
        }

        public List<TMember> FindTMember(string Keyword = "", string Gender = "", string Status = "")
        {
            try
            {
                Keyword = string.IsNullOrEmpty(Keyword) ? "" : Keyword;
                Gender = string.IsNullOrEmpty(Gender) ? "" : Gender;
                Status = string.IsNullOrEmpty(Status) ? "" : Status;
                string keyword = Keyword;
                int intGender = Gender.Equals(AppConst.Gender.Male) ? 0 : Gender.Equals(AppConst.Gender.Female) ? 1 : Gender.Equals(AppConst.Gender.Unknow) ? 2 : -1;
                int intLiveOrDie = Status.Equals(AppConst.Status.Alive) ? 1 : Status.Equals(AppConst.Status.IsDeath) ? 0 : -1;
                bool isDeath = intLiveOrDie == 0 ? true : false;
                keyword = keyword.ToLower();
                var tblTMember = AppManager.DBManager.GetTable<TMember>();
                var dtaTMember = tblTMember.ToList(
                    i => (intGender < 0 || i.Gender == intGender) // gender
                         && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death                 
                         && (string.IsNullOrEmpty(keyword)
                             || (i.Name ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Address ?? "").ToLower().Contains(keyword))
                    );
                return dtaTMember;
            }
            catch
            {
                return new List<TMember>();
            }
        }
        #endregion

        #region Lấy danh sách thành viên kết quả trả về kiểu Dictionary
        public Dictionary<string, TMember> FindTMemberOutDictionary(string Keyword = "", string Gender = "", string Status = "")
        {
            try
            {
                Keyword = string.IsNullOrEmpty(Keyword) ? "" : Keyword;
                Gender = string.IsNullOrEmpty(Gender) ? "" : Gender;
                Status = string.IsNullOrEmpty(Status) ? "" : Status;
                string keyword = Keyword;
                int intGender = Gender.Equals(AppConst.Gender.Male) ? 0 : Gender.Equals(AppConst.Gender.Female) ? 1 : Gender.Equals(AppConst.Gender.Unknow) ? 2 : -1;
                int intLiveOrDie = Status.Equals(AppConst.Status.Alive) ? 1 : Status.Equals(AppConst.Status.IsDeath) ? 0 : -1;
                bool isDeath = intLiveOrDie == 0 ? true : false;
                keyword = keyword.ToLower();
                var tblTMember = AppManager.DBManager.GetTable<TMember>();

                var dtaTMember = tblTMember.ToList(
                    i => (intGender < 0 || i.Gender == intGender) // gender
                         && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death                 
                         && (string.IsNullOrEmpty(keyword)
                             || (i.Name ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Address ?? "").ToLower().Contains(keyword))
                    ).ToDictionary(x => x.Id);

                return dtaTMember;
            }
            catch
            {
                return new Dictionary<string, TMember>();
            }
        }
        public Dictionary<string, ExTMember> FindExTMemberOutDictionary(string Keyword = "", string Gender = "", string Status = "")
        {
            try
            {
                Keyword = string.IsNullOrEmpty(Keyword) ? "" : Keyword;
                Gender = string.IsNullOrEmpty(Gender) ? "" : Gender;
                Status = string.IsNullOrEmpty(Status) ? "" : Status;

                string keyword = Keyword;
                int intGender = Gender.Equals(AppConst.Gender.Male) ? 0 : Gender.Equals(AppConst.Gender.Female) ? 1 : Gender.Equals(AppConst.Gender.Unknow) ? 2 : -1;
                int intLiveOrDie = Status.Equals(AppConst.Status.Alive) ? 1 : Status.Equals(AppConst.Status.IsDeath) ? 0 : -1;
                bool isDeath = intLiveOrDie == 0 ? true : false;

                keyword = keyword.ToLower();

                var tblTMember = AppManager.DBManager.GetTable<TMember>();

                var dtaTMember = tblTMember.CreateQuery(
                    i => (intGender < 0 || i.Gender == intGender) // gender
                         && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death                 
                         && (string.IsNullOrEmpty(keyword)
                             || (i.Name ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Tel_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_1 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Email_2 ?? "").ToLower().Contains(keyword)
                             || (i.Contact.Address ?? "").ToLower().Contains(keyword))
                    )
                    .ToEnumerable()
                    .Select(i => new ExTMember()
                    {
                        Id = i.Id,
                        Name = i.Name + "",
                        ShowName = string.IsNullOrEmpty(i.ShowName) ? CreateShowName(i.Name) : i.ShowName,
                        Gender = i.Gender,
                        IsDeath = i.IsDeath,
                        GenderShow = (i.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((i.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                        BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
                        BirthdayLunarShow = i.Birthday != null ? i.Birthday.ToDateMoon() : "",
                        DeadDaySunShow = i.DeadDay != null ? i.DeadDay.ToDateSun() : "",
                        DeadDayLunarShow = i.DeadDay != null ? i.DeadDay.ToDateMoon() : "",
                        Contact = i.Contact,
                        Tel_1 = i.Contact.Tel_1 + "",
                        Tel_2 = i.Contact.Tel_2 + "",
                        Email_1 = i.Contact.Email_1 + "",
                        Email_2 = i.Contact.Email_2 + "",
                        Address = i.Contact.Address + "",
                        Relation = i.Relation,
                        Religion = i.Religion,
                        National = i.National,
                        TypeName = i.TypeName,
                        HomeTown = i.HomeTown,
                        BirthPlace = i.BirthPlace,
                        DeadPlace = i.DeadPlace,
                        ListCHILDREN = i.ListCHILDREN,
                        ListPARENT = i.ListPARENT,
                        ListSPOUSE = i.ListSPOUSE,
                        AvatarImg = i.AvatarImg,
                        LevelInFamily = i.LevelInFamily,
                        LevelInFamilyOfFather = i.LevelInFamilyOfFather,
                        LevelInFamilyOfMother = i.LevelInFamilyOfMother,
                        LevelInFamilyForShow = i.LevelInFamily.ToString().PadLeft(2, '0') + (!string.IsNullOrEmpty(i.LevelInFamilyOfFather) ? $".{i.LevelInFamilyOfFather}" : "") + (!string.IsNullOrEmpty(i.LevelInFamilyOfMother) ? $".{i.LevelInFamilyOfMother}" : ""),
                        ChildLevelInFamily = i.ChildLevelInFamily,
                        strBackColor = i.strBackColor,
                        strForeColor = i.strForeColor,
                        UseDefaultColor = i.UseDefaultColor,
                        ShowStepChild = i.ShowStepChild,
                        InRootTree = i.InRootTree,
                        SpouseInRootTree = i.SpouseInRootTree,
                        RootID = i.RootID
                    })
                    .OrderBy(x => x.LevelInFamilyForShow)
                    .ToDictionary(x => x.Id);
                return dtaTMember;
            }
            catch
            {
                return new Dictionary<string, ExTMember>();
            }
        }
        #endregion

        #region Lấy thông tin thành viên
        public ExTMember GetExTMemberByID(string iD, string gender = "")
        {
            int iGender = gender.Equals(AppConst.Gender.Male) ? 0 : (gender.Equals(AppConst.Gender.Female) ? 1 : (gender.Equals(AppConst.Gender.Unknow) ? 2 : -1));

            var member = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == iD && (iGender == -1 || i.Gender == iGender));

            if (member == null)
            {
                return null;
            }

            return new ExTMember()
            {
                Id = member.Id,
                Name = member.Name,
                ShowName = string.IsNullOrEmpty(member.ShowName) ? CreateShowName(member.Name) : member.ShowName,
                Gender = member.Gender,
                IsDeath = member.IsDeath,
                GenderShow = (member.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((member.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                BirthdayShow = member.Birthday != null ? member.Birthday.ToDateSun() : "",
                BirthdayLunarShow = member.Birthday != null ? member.Birthday.ToDateMoon() : "",
                DeadDaySunShow = member.DeadDay != null ? member.DeadDay.ToDateSun() : "",
                DeadDayLunarShow = member.DeadDay != null ? member.DeadDay.ToDateMoon() : "",
                Tel_1 = member.Contact.Tel_1 + "",
                Tel_2 = member.Contact.Tel_2 + "",
                Email_1 = member.Contact.Email_1 + "",
                Email_2 = member.Contact.Email_2 + "",
                Address = member.Contact.Address + "",
                Relation = member.Relation,
                Religion = member.Religion,
                National = member.National,
                TypeName = member.TypeName,
                HomeTown = member.HomeTown,
                BirthPlace = member.BirthPlace,
                DeadPlace = member.DeadPlace,
                ListCHILDREN = member.ListCHILDREN,
                ListPARENT = member.ListPARENT,
                ListSPOUSE = member.ListSPOUSE,
                AvatarImg = member.AvatarImg,
                LevelInFamily = member.LevelInFamily,
                LevelInFamilyOfFather = member.LevelInFamilyOfFather,
                LevelInFamilyOfMother = member.LevelInFamilyOfMother,
                LevelInFamilyForShow = (member.LevelInFamily.ToString().PadLeft(2, '0') + (!string.IsNullOrEmpty(member.LevelInFamilyOfFather) ? $".{member.LevelInFamilyOfFather}" : "") + (!string.IsNullOrEmpty(member.LevelInFamilyOfMother) ? $".{member.LevelInFamilyOfMother}" : "")),
                ChildLevelInFamily = member.ChildLevelInFamily,
                strBackColor = member.strBackColor,
                strForeColor = member.strForeColor,
                UseDefaultColor = member.UseDefaultColor,
                ShowStepChild = member.ShowStepChild,
                Job = member.Job,
                Event = member.Event,
                School = member.School,
                Contact = member.Contact,
                InRootTree = member.InRootTree,
                SpouseInRootTree = member.SpouseInRootTree,
                RootID = member.RootID
            };
        }

        public TMember GetTMemberByID(string iD, string gender = "")
        {
            int iGender = gender.Equals(AppConst.Gender.Male) ? 0 : (gender.Equals(AppConst.Gender.Female) ? 1 : (gender.Equals(AppConst.Gender.Unknow) ? 2 : -1));
            var member = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == iD && (iGender == -1 || i.Gender == iGender));
            return member;
        }

        public ExTMember TMemberToExTMember(TMember member)
        {
            if (member == null) return null;
            return new ExTMember()
            {
                Id = member.Id,
                Name = member.Name,
                ShowName = string.IsNullOrEmpty(member.ShowName) ? CreateShowName(member.Name) : member.ShowName,
                Gender = member.Gender,
                IsDeath = member.IsDeath,
                GenderShow = (member.Gender == (int)EmGender.Male) ? AppConst.Gender.Male : ((member.Gender == (int)EmGender.FeMale) ? AppConst.Gender.Female : AppConst.Gender.Unknow),
                BirthdayShow = member.Birthday != null ? member.Birthday.ToDateSun() : "",
                BirthdayLunarShow = member.Birthday != null ? member.Birthday.ToDateMoon() : "",
                DeadDaySunShow = member.DeadDay != null ? member.DeadDay.ToDateSun() : "",
                DeadDayLunarShow = member.DeadDay != null ? member.DeadDay.ToDateMoon() : "",
                Tel_1 = member.Contact.Tel_1 + "",
                Tel_2 = member.Contact.Tel_2 + "",
                Email_1 = member.Contact.Email_1 + "",
                Email_2 = member.Contact.Email_2 + "",
                Address = member.Contact.Address + "",
                Relation = member.Relation,
                Religion = member.Religion,
                National = member.National,
                TypeName = member.TypeName,
                HomeTown = member.HomeTown,
                BirthPlace = member.BirthPlace,
                DeadPlace = member.DeadPlace,
                ListCHILDREN = member.ListCHILDREN,
                ListPARENT = member.ListPARENT,
                ListSPOUSE = member.ListSPOUSE,
                AvatarImg = member.AvatarImg,
                LevelInFamily = member.LevelInFamily,
                LevelInFamilyOfFather = member.LevelInFamilyOfFather,
                LevelInFamilyOfMother = member.LevelInFamilyOfMother,
                LevelInFamilyForShow = (member.LevelInFamily.ToString().PadLeft(2, '0') + (!string.IsNullOrEmpty(member.LevelInFamilyOfFather) ? $".{member.LevelInFamilyOfFather}" : "") + (!string.IsNullOrEmpty(member.LevelInFamilyOfMother) ? $".{member.LevelInFamilyOfMother}" : "")),
                ChildLevelInFamily = member.ChildLevelInFamily,
                strBackColor = member.strBackColor,
                strForeColor = member.strForeColor,
                UseDefaultColor = member.UseDefaultColor,
                ShowStepChild = member.ShowStepChild,
                Job = member.Job,
                Event = member.Event,
                School = member.School,
                Contact = member.Contact,
                InRootTree = member.InRootTree,
                SpouseInRootTree = member.SpouseInRootTree,
                RootID = member.RootID
            };
        }

        public void GetParent(ExTMember mem, out ExTMember father, out ExTMember mother)
        {
            ExTMember memberFather = null;
            ExTMember memberMother = null;
            if (mem.ListPARENT == null || mem.ListPARENT.Count == 0)
            {
                memberFather = new ExTMember() { Name = AppConst.NameDefaul.Father };
                memberMother = new ExTMember() { Name = AppConst.NameDefaul.Mother };
            }
            else
            {
                memberFather = GetExTMemberByID(mem.ListPARENT[0], AppConst.Gender.Male);
                memberMother = GetExTMemberByID(mem.ListPARENT[0], AppConst.Gender.Female);

                if (memberFather == null)
                {
                    memberFather = mem.ListPARENT.Count > 1 ? GetExTMemberByID(mem.ListPARENT[1]) : new ExTMember() { Name = AppConst.NameDefaul.Father };
                }
                if (memberMother == null)
                {
                    memberMother = mem.ListPARENT.Count > 1 ? GetExTMemberByID(mem.ListPARENT[1]) : new ExTMember() { Name = AppConst.NameDefaul.Mother };
                }
            }
            father = memberFather;
            mother = memberMother;
        }

        public void GetParent(TMember mem, out TMember father, out TMember mother)
        {
            TMember memberFather = null;
            TMember memberMother = null;
            if (mem.ListPARENT == null || mem.ListPARENT.Count == 0)
            {
                memberFather = new TMember() { Name = AppConst.NameDefaul.Father };
                memberMother = new TMember() { Name = AppConst.NameDefaul.Mother };
            }
            else
            {
                memberFather = GetExTMemberByID(mem.ListPARENT[0], AppConst.Gender.Male);
                memberMother = GetExTMemberByID(mem.ListPARENT[0], AppConst.Gender.Female);

                if (memberFather == null)
                {
                    memberFather = mem.ListPARENT.Count > 1 ? GetExTMemberByID(mem.ListPARENT[1]) : new TMember() { Name = AppConst.NameDefaul.Father };
                }
                if (memberMother == null)
                {
                    memberMother = mem.ListPARENT.Count > 1 ? GetExTMemberByID(mem.ListPARENT[1]) : new TMember() { Name = AppConst.NameDefaul.Mother };
                }
            }
            father = memberFather;
            mother = memberMother;
        }
        #endregion

        #region Cập nhật thông tin thứ bậc các thành viên trong dòng họ
        public bool UpdateLevelInFamily(ProgressBarManager progressBar = null, TMember member = null)
        {
            var rootMember = RootTMember;
            if (member != null)
            {
                rootMember = member;
            }
            if (rootMember == null)
            {
                if (progressBar != null)
                {
                    progressBar.total = 1;
                    progressBar.count = 1;
                    progressBar.Percent = progressBar.fncCalculatePercent(progressBar.count, progressBar.total);
                }
                return false;
            }

            rootMember.InRootTree = true;
            rootMember.RootID = rootMember.Id;

            if (rootMember.ListPARENT.Count > 0)
            {
                GetParent(rootMember, out TMember father, out TMember mother);

                int cntSpouse = -1;
                if (mother != null)
                {
                    cntSpouse = father.ListSPOUSE.IndexOf(mother.Id) + 1;
                }

                rootMember.LevelInFamilyOfMother = $"[{(cntSpouse < 1 ? "_" : cntSpouse.ToString().PadLeft(2, '0'))}.{(rootMember.ChildLevelInFamily < 1 ? "_" : rootMember.ChildLevelInFamily.ToString().PadLeft(2, '0'))}]";
            }
            else
            {
                rootMember.LevelInFamilyOfMother = "";
            }

            List<string> lstMemberIDInRootTree = new List<string>();

            lstMemberIDInRootTree.Add(rootMember.Id);

            var tabTMember = AppManager.DBManager.GetTable<TMember>();

            tabTMember.UpdateOne(m => m.Id == rootMember.Id, rootMember);

            Dictionary<string, TMember> allMember = FindTMemberOutDictionary();

            if (allMember.ContainsKey(rootMember.Id))
            {
                allMember[rootMember.Id].InRootTree = rootMember.InRootTree;
                allMember[rootMember.Id].SpouseInRootTree = rootMember.SpouseInRootTree;
            }

            if (progressBar != null)
            {
                progressBar.total = allMember.Count;
                progressBar.count = 0;
                progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
            }

            UpdateParent(allMember, lstMemberIDInRootTree, rootMember, progressBar);

            UpdateChildren(allMember, lstMemberIDInRootTree, rootMember, progressBar);

            var lstMembeOutOfInRootTree = allMember.Values.Where(i => !lstMemberIDInRootTree.Contains(i.Id)).ToList();

            lstMembeOutOfInRootTree.ForEach(mem =>
            {
                if (allMember.ContainsKey(mem.Id))
                {
                    mem.LevelInFamily = -1000;
                    mem.LevelInFamilyOfFather = "";
                    mem.LevelInFamilyOfMother = "";
                    mem.InRootTree = mem.SpouseInRootTree = false;
                    mem.RootID = "";
                    tabTMember.UpdateOne(i => i.Id == mem.Id, mem);

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                    }
                }
            });

            return true;
        }

        //Cập nhật thứ bậc của cha/ mẹ của thành viên chính
        private void UpdateParent(Dictionary<string, TMember> allMember, List<string> lstMemberIDInRootTree, TMember member, ProgressBarManager progressBar)
        {
            Common.Database.ReposityLiteTable<TMember> tabTMember = AppManager.DBManager.GetTable<TMember>();
            var lstParent = member.ListPARENT;
            lstParent.ForEach(mem =>
            {
                if (allMember.ContainsKey(mem))
                {
                    var parent = allMember[mem];
                    parent.LevelInFamily = member.LevelInFamily - 1;
                    parent.InRootTree = member.InRootTree;
                    parent.RootID = member.RootID;
                    tabTMember.UpdateOne(i => i.Id == mem, parent);

                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                    }

                    lstMemberIDInRootTree.Add(mem);

                    UpdateParent(allMember, lstMemberIDInRootTree, parent, progressBar);
                }
            });
        }

        //Cập bậc thứ bậc của các con của thành viên chính
        private bool UpdateChildren(Dictionary<string, TMember> allMember, List<string> lstMemberIDInRootTree, TMember member, ProgressBarManager progressBar)
        {
            Common.Database.ReposityLiteTable<TMember> tabTMember = AppManager.DBManager.GetTable<TMember>();
            var listSpouse = member.ListSPOUSE;
            if (listSpouse == null)
            {
                listSpouse = new List<string>();
            }

            List<string> lstChildOfSpouse = new List<string>();

            if (listSpouse.Count != 0)
            {
                int cntSpouse = 1;
                listSpouse.ForEach(spouse =>
                {
                    TMember tMember = new TMember();
                    if (allMember.ContainsKey(spouse))
                    {
                        tMember = allMember[spouse];
                        allMember[spouse].InRootTree = tMember.InRootTree = false;
                        allMember[spouse].SpouseInRootTree = tMember.SpouseInRootTree = true;

                        tMember.RootID = member.RootID;

                        tMember.LevelInFamily = member.LevelInFamily;
                        tMember.LevelInFamilyOfFather = member.LevelInFamilyOfFather;
                        tMember.LevelInFamilyOfMother = (!string.IsNullOrEmpty(member.LevelInFamilyOfMother) ? $"{member.LevelInFamilyOfMother}." : "") + $"{cntSpouse.ToString().PadLeft(2, '0')}";

                        tabTMember.UpdateOne(m => m.Id == tMember.Id, tMember);

                        lstMemberIDInRootTree.Add(spouse);
                    }
                    if (progressBar != null)
                    {
                        progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                    }
                    //update level in family of child
                    var listChild = tMember.ListCHILDREN;

                    if (listChild == null)
                    {
                        listChild = new List<string>();
                    }

                    lstChildOfSpouse.AddRange(listChild);

                    if (listChild.Count != 0)
                    {
                        listChild.ForEach(child =>
                        {
                            TMember tMemberChild = new TMember();
                            if (allMember.ContainsKey(child))
                            {
                                tMemberChild = allMember[child];
                                tMemberChild.LevelInFamily = member.LevelInFamily + 1;

                                tMemberChild.RootID = member.RootID;

                                tMemberChild.LevelInFamilyOfFather = (!string.IsNullOrEmpty(member.LevelInFamilyOfFather) ? $"{member.LevelInFamilyOfFather}." : "") + $"{member.LevelInFamilyOfMother}";
                                tMemberChild.LevelInFamilyOfMother = $"[{cntSpouse.ToString().PadLeft(2, '0')}.{(tMemberChild.ChildLevelInFamily < 1 ? "_" : tMemberChild.ChildLevelInFamily.ToString().PadLeft(2, '0'))}]";
                                allMember[child].InRootTree = tMemberChild.InRootTree = true;
                                allMember[child].SpouseInRootTree = tMemberChild.SpouseInRootTree = false;
                                tabTMember.UpdateOne(m => m.Id == tMemberChild.Id, tMemberChild);

                                lstMemberIDInRootTree.Add(child);
                            }

                            if (progressBar != null)
                            {
                                progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
                            }

                            UpdateChildren(allMember, lstMemberIDInRootTree, tMemberChild, progressBar);
                        });
                    }
                    cntSpouse++;
                });
            }
            else
            {
                var listChild = member.ListCHILDREN;
                if (listChild != null)
                {
                    listChild.ForEach(child =>
                    {
                        UpdateInfoLevelChildren(allMember, lstMemberIDInRootTree, member, child, progressBar);
                    });
                }
            }

            var lstChildOfMember = member.ListCHILDREN.Where(id => !lstChildOfSpouse.Contains(id) && allMember.ContainsKey(id) && allMember[id] != null).ToList();

            lstChildOfMember.ForEach(child =>
            {
                UpdateInfoLevelChildren(allMember, lstMemberIDInRootTree, member, child, progressBar);
            });

            return true;
        }

        //Cập nhật thứ bậc của 1 con
        private void UpdateInfoLevelChildren(Dictionary<string, TMember> allMember, List<string> lstMemberIDInRootTree, TMember member, string child, ProgressBarManager progressBar)
        {
            Common.Database.ReposityLiteTable<TMember> tabTMember = AppManager.DBManager.GetTable<TMember>();
            TMember tMemberChild = allMember[child];
            tMemberChild.LevelInFamily = member.LevelInFamily + 1;
            tMemberChild.RootID = member.RootID;
            tMemberChild.LevelInFamilyOfFather = (!string.IsNullOrEmpty(member.LevelInFamilyOfFather) ? $"{member.LevelInFamilyOfFather}." : "") + $"{member.LevelInFamilyOfMother}";
            tMemberChild.LevelInFamilyOfMother = $"[_.{(tMemberChild.ChildLevelInFamily < 1 ? "_" : tMemberChild.ChildLevelInFamily.ToString().PadLeft(2, '0'))}]";
            allMember[child].InRootTree = tMemberChild.InRootTree = true;
            allMember[child].SpouseInRootTree = tMemberChild.SpouseInRootTree = true;
            tabTMember.UpdateOne(m => m.Id == tMemberChild.Id, tMemberChild);
            if (progressBar != null)
            {
                progressBar.Percent = progressBar.fncCalculatePercent2(++progressBar.count, progressBar.total);
            }
            lstMemberIDInRootTree.Add(child);
            UpdateChildren(allMember, lstMemberIDInRootTree, tMemberChild, progressBar);
        }
        #endregion

        #region Lấy thông tin thành viên là tôt phụ
        public TMember RootTMember
        {
            get
            {
                var objFamily = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                if (objFamily == null) return null;

                var objRootData = !string.IsNullOrEmpty(objFamily.RootId) ? AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objFamily.RootId) : null;
                if (objRootData == null) return null;

                objRootData.LevelInFamily = objFamily.FamilyLevel;

                return objRootData;
            }
        }

        public ExTMember RootExTMember
        {
            get
            {
                var objFamily = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                if (objFamily == null) return null;

                var objRootData = !string.IsNullOrEmpty(objFamily.RootId) ? AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objFamily.RootId) : null;
                if (objRootData == null) return null;

                objRootData.LevelInFamily = objFamily.FamilyLevel;

                return TMemberToExTMember(objRootData);
            }
        }
        #endregion

        #region Lấy danh sách tộc trưởng
        public List<string> ListFamilyHead
        {
            get
            {
                var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                var objUserLogin = AppManager.LoginUser;
                var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
                if (objMFamilyInfo == null)
                {
                    return new List<string>();
                }
                return objMFamilyInfo.ListFamilyHead == null ? new List<string>() : objMFamilyInfo.ListFamilyHead;
            }
        }
        #endregion

        //Cập nhật lại thứ tự của các con theo vợ của cha
        public List<string> UpdateListChildren(string memberID)
        {
            ExTMember objRoot = GetExTMemberByID(memberID);
            if (objRoot == null) return new List<string>();
            /*Tạo danh sách các con với thứ tự theo các bà vợ*/
            List<string> lstChild = new List<string>();
            var lstSpouse = objRoot.ListSPOUSE;
            if (lstSpouse == null || lstSpouse.Count == 0)
            {
                lstChild = objRoot.ListCHILDREN;
            }
            else
            {
                if (lstSpouse.Count == 1)//Có 1 vợ
                {
                    var spouse = GetExTMemberByID(lstSpouse[0]);
                    if (spouse != null)
                    {
                        var lstTemp = spouse.ListCHILDREN;
                        if (lstTemp == null || lstTemp.Count == 0)//Vợ không có con
                        {
                            lstChild = objRoot.ListCHILDREN;
                        }
                        else//Vợ có con
                        {
                            var temp1 = objRoot.ListCHILDREN.Where(x => !lstChild.Contains(x)).ToList();
                            var temp2 = objRoot.ListCHILDREN.Where(x => lstChild.Contains(x)).ToList();
                            lstChild.AddRange(temp1);
                            lstChild.AddRange(temp2);
                        }
                    }
                    else
                    {
                        lstChild = objRoot.ListCHILDREN;
                    }
                }
                else // Có trên 1 vợ
                {
                    var spouse = GetExTMemberByID(lstSpouse[0]);
                    if (spouse != null)
                    {
                        var lstTemp = spouse.ListCHILDREN;
                        if (!(lstTemp == null || lstTemp.Count == 0))
                        {
                            lstChild.AddRange(lstTemp);
                        }
                    }

                    objRoot.ListCHILDREN.ForEach(x =>
                    {
                        var objTemp = GetExTMemberByID(x);
                        if (objTemp != null)
                        {
                            if (objTemp.ListPARENT.Count == 1)
                            {
                                lstChild.Add(x);
                            }
                        }
                    });

                    lstSpouse.ForEach(x =>
                    {
                        if (x != lstSpouse[0])
                        {
                            ExTMember objTemp = GetExTMemberByID(x);
                            lstChild.AddRange(objTemp.ListCHILDREN);
                        };
                    });
                }
            }
            return lstChild;
            /*************************************************/
        }

        //Tạo tên hiển thị trên thẻ thành viên tử tên đầy đủ
        public string CreateShowName(string FullName)
        {
            string fullName = FullName;
            string showName = "";
            if (fullName.Length <= 30)
            {
                showName = fullName;
            }
            else
            {
                string[] arrTemp = fullName.Split(' ');
                if (arrTemp.Length == 1)
                {
                    showName = fullName;
                }
                else
                {
                    string firstName = arrTemp[0];
                    string midName = "";
                    for (int i = 1; i < arrTemp.Length - 1; i++)
                    {
                        string text = arrTemp[i];
                        midName += $"{text.Substring(0, 1).ToUpper()}.";
                    }
                    string lastName = arrTemp[arrTemp.Length - 1];
                    showName = $"{firstName} {midName.Substring(0, midName.Length - 1)} {lastName}";
                }
            }
            return showName;
        }

        #region Thêm, xóa tổ phụ, tộc trưởng
        public bool AddRootMember(string memberID)
        {
            if (string.IsNullOrEmpty(memberID)) return true;
            var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
            var objUserLogin = AppManager.LoginUser;
            var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
            // update rootId with memberId
            if (objMFamilyInfo == null)
            {
                objMFamilyInfo = new MFamilyInfo();
            }
            objMFamilyInfo.RootId = memberID;
            return tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo);
        }
        public bool RemoveRootMember(string memberID)
        {
            if (string.IsNullOrEmpty(memberID)) return true;
            var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
            var objUserLogin = AppManager.LoginUser;
            var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
            // update rootId with memberId
            if (objMFamilyInfo.RootId == memberID)
            {
                objMFamilyInfo.RootId = "";
                return tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo);
            }
            return false;
        }
        public bool AddFamilyHead(string memberID)
        {
            if (string.IsNullOrEmpty(memberID)) return true;
            var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
            var objUserLogin = AppManager.LoginUser;
            var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
            // add new member to list family head
            if (objMFamilyInfo.ListFamilyHead == null)
            {
                objMFamilyInfo.ListFamilyHead = new List<string>();
            }
            if (objMFamilyInfo.ListFamilyHead.Contains(memberID))
            {
                return false;
            }
            objMFamilyInfo.ListFamilyHead.Add(memberID);
            objMFamilyInfo.CurrentFamilyHead = memberID;
            return tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo);
        }
        public bool RemoveFamilyHead(string memberID)
        {
            if (string.IsNullOrEmpty(memberID)) return true;
            var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
            var objUserLogin = AppManager.LoginUser;
            var objMFamilyInfo = tblMFamilyInfo.CreateQuery(i => i.Id == objUserLogin.FamilyId).FirstOrDefault();
            // remove member to list family head
            if (objMFamilyInfo.ListFamilyHead == null)
            {
                return true;
            }
            if (objMFamilyInfo.ListFamilyHead.Contains(memberID))
            {
                objMFamilyInfo.ListFamilyHead.Remove(memberID);
                // update new current family head
                objMFamilyInfo.CurrentFamilyHead = (objMFamilyInfo.ListFamilyHead != null) ? objMFamilyInfo.ListFamilyHead[objMFamilyInfo.ListFamilyHead.Count - 1] : "";
            }
            return tblMFamilyInfo.UpdateOne(i => i.Id == objMFamilyInfo.Id, objMFamilyInfo);
        }
        #endregion

        #region Dispose Object
        private bool disposedValue;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MemberHelper()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
