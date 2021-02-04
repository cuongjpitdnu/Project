using GP40DrawTree;
using GPCommon;
using GPConst;
using GPMain.Common.Helper;
using GPModels;
using System.Collections.Generic;
using System.Drawing;

namespace GPMain.Common
{
    public class GenerateData
    {
        #region DataDB

        public static void CreateDataMTypeName()
        {
            var mTypeName = AppManager.DBManager.GetTable<MTypeName>();

            if (!mTypeName.CreateQuery(false).Exists())
            {
                var listInsert = new List<MTypeName>();

                listInsert.Add(new MTypeName() { TypeName = "Tên thường gọi", IsDefault = true, CanDelete = false });
                listInsert.Add(new MTypeName() { TypeName = "Pháp danh", IsDefault = false, CanDelete = true });

                mTypeName.InsertBulk(listInsert);
            }
        }

        public static void CreateDataMRelation()
        {
            var mRelation = AppManager.DBManager.GetTable<MRelation>();

            if (!mRelation.CreateQuery(false).Exists())
            {
                var listInsert = new List<MRelation>();
                listInsert.Add(new MRelation { MainRelation = "DAD01", NameOfRelation = "Cha ruột", RelatedRelation = "CHI01", CanDelete = false, IsMain = true });
                listInsert.Add(new MRelation { MainRelation = "DAD02", NameOfRelation = "Cha nuôi", RelatedRelation = "CHI02", CanDelete = false, IsMain = false });
                listInsert.Add(new MRelation { MainRelation = "MOM01", NameOfRelation = "Mẹ ruột", RelatedRelation = "CHI01", CanDelete = false, IsMain = true });
                listInsert.Add(new MRelation { MainRelation = "MOM02", NameOfRelation = "Mẹ nuôi", RelatedRelation = "CHI02", CanDelete = false, IsMain = false });
                listInsert.Add(new MRelation { MainRelation = "HUS01", NameOfRelation = "Chồng", RelatedRelation = "WIF01", CanDelete = false, IsMain = true });
                listInsert.Add(new MRelation { MainRelation = "WIF01", NameOfRelation = "Vợ", RelatedRelation = "HUS01", CanDelete = false, IsMain = true });
                listInsert.Add(new MRelation { MainRelation = "CHI01", NameOfRelation = "Con ruột", RelatedRelation = "DAD01", CanDelete = false, IsMain = true });
                listInsert.Add(new MRelation { MainRelation = "CHI02", NameOfRelation = "Con nuôi", RelatedRelation = "DAD02", CanDelete = false, IsMain = false });
                listInsert.Add(new MRelation { MainRelation = "CHI03", NameOfRelation = "Con ruột", RelatedRelation = "MOM01", CanDelete = false, IsMain = true });
                listInsert.Add(new MRelation { MainRelation = "CHI04", NameOfRelation = "Con nuôi", RelatedRelation = "MOM02", CanDelete = false, IsMain = false });

                mRelation.InsertBulk(listInsert);
            }
        }

        public static void CreateDataMReligion()
        {
            var mReligion = AppManager.DBManager.GetTable<MReligion>();

            if (!mReligion.CreateQuery(false).Exists())
            {
                var listInsert = new List<MReligion>();

                listInsert.Add(new MReligion { Id = "1", RelName = "Không tôn giáo", IsDefault = true });
                listInsert.Add(new MReligion { Id = "2", RelName = "Hồi giáo", IsDefault = false });
                listInsert.Add(new MReligion { Id = "3", RelName = "Hindu", IsDefault = false });
                listInsert.Add(new MReligion { Id = "4", RelName = "Thiên chúa giáo", IsDefault = false });
                listInsert.Add(new MReligion { Id = "5", RelName = "Ấn Độ giáo", IsDefault = false });
                listInsert.Add(new MReligion { Id = "6", RelName = "Phật giáo", IsDefault = false });
                listInsert.Add(new MReligion { Id = "7", RelName = "Công giáo", IsDefault = false });
                listInsert.Add(new MReligion { Id = "8", RelName = "Tin lành", IsDefault = false });
                listInsert.Add(new MReligion { Id = "9", RelName = "Cao đài", IsDefault = false });
                listInsert.Add(new MReligion { Id = "10", RelName = "Hòa hảo", IsDefault = false });
                listInsert.Add(new MReligion { Id = "11", RelName = "Các tôn giáo khác", IsDefault = false });

                mReligion.InsertBulk(listInsert, false);
            }
        }

        public static void CreateDataMNational()
        {
            var mNational = AppManager.DBManager.GetTable<MNationality>();

            if (!mNational.CreateQuery(false).Exists())
            {
                var listInsert = new List<MNationality>();

                listInsert.Add(new MNationality { Id = "1", NatName = "Việt Nam", IsDefault = true });
                listInsert.Add(new MNationality { Id = "2", NatName = "Lào", IsDefault = false });
                listInsert.Add(new MNationality { Id = "3", NatName = "Cam Pu Chia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "4", NatName = "Thái Lan", IsDefault = false });
                listInsert.Add(new MNationality { Id = "5", NatName = "Trung Quốc", IsDefault = false });
                listInsert.Add(new MNationality { Id = "6", NatName = "A Rập", IsDefault = false });
                listInsert.Add(new MNationality { Id = "7", NatName = "Ac-mê-ni", IsDefault = false });
                listInsert.Add(new MNationality { Id = "8", NatName = "Aixơlen", IsDefault = false });
                listInsert.Add(new MNationality { Id = "9", NatName = "Anh", IsDefault = false });
                listInsert.Add(new MNationality { Id = "10", NatName = "Ba Lan", IsDefault = false });
                listInsert.Add(new MNationality { Id = "11", NatName = "Ba Tư", IsDefault = false });
                listInsert.Add(new MNationality { Id = "12", NatName = "Belarus", IsDefault = false });
                listInsert.Add(new MNationality { Id = "13", NatName = "Bồ Ðào Nha", IsDefault = false });
                listInsert.Add(new MNationality { Id = "14", NatName = "Bun-ga-ri", IsDefault = false });
                listInsert.Add(new MNationality { Id = "15", NatName = "Canada", IsDefault = false });
                listInsert.Add(new MNationality { Id = "16", NatName = "Croatia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "17", NatName = "Ðan Mạch", IsDefault = false });
                listInsert.Add(new MNationality { Id = "18", NatName = "Ðức", IsDefault = false });
                listInsert.Add(new MNationality { Id = "19", NatName = "Estonia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "20", NatName = "Hà Lan", IsDefault = false });
                listInsert.Add(new MNationality { Id = "21", NatName = "Hàn Quốc", IsDefault = false });
                listInsert.Add(new MNationality { Id = "22", NatName = "Hung-ga-ri", IsDefault = false });
                listInsert.Add(new MNationality { Id = "23", NatName = "Hy Lạp", IsDefault = false });
                listInsert.Add(new MNationality { Id = "24", NatName = "Indonesia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "25", NatName = "Lát-vi", IsDefault = false });
                listInsert.Add(new MNationality { Id = "26", NatName = "Latvia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "27", NatName = "Malaysia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "28", NatName = "Mianma", IsDefault = false });
                listInsert.Add(new MNationality { Id = "29", NatName = "Mỹ", IsDefault = false });
                listInsert.Add(new MNationality { Id = "30", NatName = "Na Uy", IsDefault = false });
                listInsert.Add(new MNationality { Id = "31", NatName = "Nga", IsDefault = false });
                listInsert.Add(new MNationality { Id = "32", NatName = "Nhật Bản", IsDefault = false });
                listInsert.Add(new MNationality { Id = "33", NatName = "Ô-xtrây-lia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "34", NatName = "Phần Lan", IsDefault = false });
                listInsert.Add(new MNationality { Id = "35", NatName = "Pháp", IsDefault = false });
                listInsert.Add(new MNationality { Id = "36", NatName = "Philippin", IsDefault = false });
                listInsert.Add(new MNationality { Id = "37", NatName = "Rumani", IsDefault = false });
                listInsert.Add(new MNationality { Id = "38", NatName = "Séc", IsDefault = false });
                listInsert.Add(new MNationality { Id = "39", NatName = "Slovenia", IsDefault = false });
                listInsert.Add(new MNationality { Id = "40", NatName = "Tây Ban Nha", IsDefault = false });
                listInsert.Add(new MNationality { Id = "41", NatName = "Thổ Nhĩ Kỳ", IsDefault = false });
                listInsert.Add(new MNationality { Id = "42", NatName = "Thuỵ Ðiển", IsDefault = false });
                listInsert.Add(new MNationality { Id = "43", NatName = "Triều Tiên", IsDefault = false });
                listInsert.Add(new MNationality { Id = "44", NatName = "U-crai-na", IsDefault = false });
                listInsert.Add(new MNationality { Id = "45", NatName = "Xéc-bi", IsDefault = false });
                listInsert.Add(new MNationality { Id = "46", NatName = "Ý", IsDefault = false });

                mNational.InsertBulk(listInsert, false);
            }
        }

        public static void CreateDataMSocialNetwork()
        {
            var mSocialNetwork = AppManager.DBManager.GetTable<MSocialNetwork>();

            if (!mSocialNetwork.CreateQuery(false).Exists())
            {
                var listInsert = new List<MSocialNetwork>();

                listInsert.Add(new MSocialNetwork { SocialName = "IMChat" });
                listInsert.Add(new MSocialNetwork { SocialName = "Facebook" });
                listInsert.Add(new MSocialNetwork { SocialName = "Zalo" });

                mSocialNetwork.InsertBulk(listInsert);
            }
        }

        public static void CreateDefaultUserLogin()
        {
            var mUser = AppManager.DBManager.GetTable<MUser>();

            if (!mUser.CreateQuery(false).Exists())
            {
                mUser.InsertOne(new MUser
                {
                    UserName = "admin",
                    Password = "admin".Sha512(),
                    Role = 1,
                });
            }
        }

        public static void CreateThemeTree()
        {
            var themeConfig = AppManager.DBManager.GetTable<ThemeConfig>();

            if (!themeConfig.CreateQuery(false).Exists())
            {
                var listInsert = new List<ThemeConfig>();

                listInsert.Add(new ThemeConfig
                {
                    DisplayName = "Chủ đề 1",
                    MaleBackColor = ColorDrawHelper.FromColor(67, 107, 149).ToString(),
                    FeMaleBackColor = ColorDrawHelper.FromColor(186, 65, 47).ToString(),
                    BackgroudColor = ColorDrawHelper.FromColor(204, 204, 204).ToString(),
                    TextColor = ColorDrawHelper.FromColor(Color.White).ToString(),
                    SpouseLineColor = ColorDrawHelper.FromColor(186, 65, 47).ToString(),
                    ChildLineColor = ColorDrawHelper.FromColor(107, 90, 0).ToString(),
                    BorderColor = ColorDrawHelper.FromColor(107, 90, 0).ToString(),
                    SelectedMemberColor = ColorDrawHelper.FromColor(67, 107, 149).ToString(),
                });

                listInsert.Add(new ThemeConfig
                {
                    DisplayName = "Chủ đề 2",
                    MaleBackColor = "#9c1f2d",
                    FeMaleBackColor = "#743042",
                    BackgroudColor = "#fab509",
                    TextColor = "#dd800b",
                    SpouseLineColor = "#3f2d3b",
                    ChildLineColor = "#3f2d3b",
                    BorderColor = "#3f2d3b",
                    SelectedMemberColor = ColorDrawHelper.FromColor(67, 107, 149).ToString(),
                });

                listInsert.Add(new ThemeConfig
                {
                    DisplayName = "Chủ đề 3",
                    MaleBackColor = ColorDrawHelper.FromColor(178, 223, 219).ToString(),
                    FeMaleBackColor = ColorDrawHelper.FromColor(178, 223, 219).ToString(),
                    BackgroudColor = ColorDrawHelper.FromColor(250, 250, 250).ToString(),
                    TextColor = ColorDrawHelper.FromColor(239, 108, 0).ToString(),
                    SpouseLineColor = ColorDrawHelper.FromColor(239, 108, 0).ToString(),
                    ChildLineColor = ColorDrawHelper.FromColor(239, 108, 0).ToString(),
                    BorderColor = ColorDrawHelper.FromColor(239, 108, 0).ToString(),
                    SelectedMemberColor = ColorDrawHelper.FromColor(67, 107, 149).ToString(),
                });

                themeConfig.InsertBulk(listInsert);
            }
        }

        #endregion DataDB

        #region DataUI

        public static List<DataBinding<int>> GetListGender()
        {
            return new List<DataBinding<int>>()
            {
                new DataBinding<int>() { Display = "Tất cả giới tính", Value = -1 },
                new DataBinding<int>() { Display = "Nam", Value = (int)EmGender.Male },
                new DataBinding<int>() { Display = "Nữ", Value = (int)EmGender.FeMale },
                new DataBinding<int>() { Display = "Chưa rõ", Value = (int)EmGender.Unknown },
            };
        }

        public static List<DataBinding<int>> GetListMemberStatus()
        {
            return new List<DataBinding<int>>()
            {
                new DataBinding<int>() { Display = "Còn sống (Đã mất)", Value = -1 },
                new DataBinding<int>() { Display = "Còn sống", Value = 1 },
                new DataBinding<int>() { Display = "Đã mất", Value = 0 },
            };
        }

        #endregion DataUI
    }
}