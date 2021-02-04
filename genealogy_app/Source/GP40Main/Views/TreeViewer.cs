using System.Windows.Forms;
using GP40Main.Core;
using GP40DrawTree;
using GP40Tree;
using GP40Common;
using System;
using GP40Main.Views.Member;
using SkiaSharp.Views.Desktop;
using System.Drawing;
using GP40Main.Services.Navigation;
using GP40Main.Models;
using GP40Main.Utility;
using System.Linq;
using GP40Main.Themes;
using System.Collections.Generic;
using GP40Main.Themes.Controls;

namespace GP40Main.Views
{
    public partial class TreeViewer : BaseUserControl
    {

        private enum ModeDisplay
        {
            BuildTree,
            ViewTree,
        }

        private ContextMenuStripData<TMember> _contextMember;
        private DrawTreeManager _drawTree;
        private clsFInputTree _objFTree;
        private List<ItemBase> _listItemMenuConfig;

        private TMember RootMember {
            get {
                var objFamily = AppManager.DBManager.GetTable<MFamilyInfo>().CreateQuery(i => i.Id == AppManager.LoginUser.FamilyId).FirstOrDefault();
                var objRootData = !string.IsNullOrEmpty(objFamily.RootId) ? AppManager.DBManager.GetTable<TMember>().CreateQuery(i => i.Id == objFamily.RootId).FirstOrDefault() : null;

                if (objRootData.IsNotHasValue())
                {
                    objRootData = new TMember();
                }

                return objRootData;
            }
        }

        public TreeViewer(NavigationParameters parameters, AppConst.ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            InitForm();

            this.Disposed += (sender, e) =>
            {
                _contextMember.FreeMemory();
                _objFTree.FreeMemory();
                _drawTree.FreeMemory();
                _listItemMenuConfig.FreeMemory();
            };

            ////ucListMember.PanelMemberSizeChanged += (sender, e) =>
            ////{
            ////    ucListMember.NumberMemberInpage = ((MenuMember)sender).Height / UserMember.Heigh;
            ////    LoadMember();
            ////};

            //cmbTheme.SelectedIndexChanged += cmbThemes_SelectedIndexChanged;
            //btnsavetheme.Click += (sender, e) => { SaveTheme(cmbTheme.Text); };
            //btncreatenewtheme.Click += (sender, e) => { };
            ////ucListMember.ChangeFilter += new EventHandler<InfoFilter>(ChangeFilter_Event);
            //plnConfigTree.Width = 201;
            //plnConfigTree.BringToFront();
            //plnConfigTree.Visible = false;
            //InitialMenuItems();
            //LoadMember();
        }

        private void InitForm()
        {
            this.SetDefaultUI();
            this.InitDrawTree();
            this.InitMenuConfig();

            btnEditTree.Location = new Point(plnMain.Width - btnEditTree.Width, 0);
            plnTree.Dock = DockStyle.Fill;

            var arrTypeTree = clsConst.TEMPLATE_NAME;

            foreach (var typeTree in arrTypeTree)
            {
                cboTypeTree.Items.Add(typeTree);
            }

            cboTypeTree.SelectedIndex = -1;

            this.SetModeDisplay();
        }

        private void InitDrawTree()
        {
            var themeDB = AppManager.DBManager.GetTable<ThemeConfig>().CreateQuery().FirstOrDefault() ?? new ThemeConfig();
            var configTree = themeDB.ToConfig();

            _contextMember = ContextMenuStripManager.CreateForMember();
            _contextMember.ItemClickEnd += (sender, e) => ReloadTree();

            _drawTree = new DrawTreeManager(configTree);

            _drawTree.MemberDoubleClick += (sender, e) =>
            {
                var objMember = sender as clsFamilyMember;

                if (objMember.IsNotHasValue() || String.IsNullOrEmpty(objMember.miID))
                {
                    return;
                }

                var objTMember = AppManager.DBManager.GetTable<TMember>().CreateQuery(i => i.Id == objMember.miID).FirstOrDefault();

                if (objTMember.IsHasValue())
                {
                    AppManager.Navigation.ShowDialog<addMember, TMember>(new NavigationParameters(objTMember), AppConst.ModeForm.Edit);
                    ReloadTree();
                }
            };

            _drawTree.MemberRightClick += (sender, objMember) =>
            {
                if (objMember.IsHasValue() && !String.IsNullOrEmpty(objMember.miID) && _contextMember.IsHasValue())
                {
                    var objTMember = AppManager.DBManager.GetTable<TMember>().CreateQuery(i => i.Id == objMember.miID).FirstOrDefault();
                    _contextMember.Show(AppManager.GetCursorPosition(), objTMember);
                }
            };

            plnMain.BackColor = _drawTree.Config.BackgroudColor.ToDrawingColor();
        }

        private void InitMenuConfig()
        {

            var dataThemes = AppManager.DBManager.GetTable<ThemeConfig>().CreateQuery().Select(i => new DataBinding<ThemeConfig>() {
                Display = i.DisplayName,
                Value = i,
            }).ToList();

            var themeDB = dataThemes.FirstOrDefault()?.Value ?? new ThemeConfig();
            BindingHelper.Combobox(cboThemes, dataThemes, themeDB);

            var urcLineItem = new MenuItems.LineItem(_drawTree.Config);
            var urcBackgroundItem = new MenuItems.BackgroundItem(_drawTree.Config);
            var urcFrameItem = new MenuItems.FrameItem(_drawTree.Config);
            var urcTextItem = new MenuItems.TextItem(_drawTree.Config);
            var urcSpaceItem = new MenuItems.SpaceItem(_drawTree.Config);

            _listItemMenuConfig = new List<ItemBase>()
            {
                urcLineItem,
                urcBackgroundItem,
                urcFrameItem,
                urcTextItem,
                urcSpaceItem,
            };

            _listItemMenuConfig.ForEach(itemMenuConfig => itemMenuConfig.ChangeData += (config) =>ReloadTree());

            menuConfig.Add("Line", urcLineItem);
            menuConfig.Add("Background", urcBackgroundItem);
            menuConfig.Add("Frame", urcFrameItem);
            menuConfig.Add("Text", urcTextItem);
            menuConfig.Add("Space", urcSpaceItem);
        }

        private void SetModeDisplay(ModeDisplay mode = ModeDisplay.BuildTree)
        {
            switch (mode)
            {
                case ModeDisplay.ViewTree:
                    plnConfigTree.BringToFront();
                    plnControl.BringToFront();
                    btnEditTree.BringToFront();

                    plnTree.Visible = false;
                    plnTree.Controls.Clear();
                    plnTree.Controls.Add(_drawTree.Tree);
                    plnTree.Visible = true;

                    if (cboTypeTree.Items.Count > 0)
                    {
                        cboTypeTree.SelectedIndex = 0;
                    }

                    btnEditTree.Visible = true;
                    plnConfigTree.Visible = false;
                    plnControl.Visible = true;
                    break;
                case ModeDisplay.BuildTree:
                default:
                    plnTree.Controls.Clear();
                    plnTree.Visible = false;
                    _objFTree = new clsFInputTree();
                    var skTree = _objFTree.InputTree;
                    plnTree.Controls.Add(skTree);
                    cboTypeTree.SelectedIndex = -1;
                    plnTree.Visible = true;

                    btnEditTree.Visible = false;
                    plnConfigTree.Visible = false;
                    plnControl.Visible = false;
                    break;
            }
        }

        private void ReloadMenuConfig()
        {
            if (_listItemMenuConfig.IsHasValue())
            {
                _listItemMenuConfig.ForEach(itemMenuConfig => itemMenuConfig.ReloadConfig(_drawTree.Config));
            }
        }

        private void ReloadTree()
        {
            var selected = cboTypeTree.SelectedItem + "";
            clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
            Enum.TryParse(selected, out emTemplate);

            plnMain.BackColor = _drawTree.Config.BackgroudColor.ToDrawingColor();

            plnTree.BeginInvoke(new Action(() =>
            {
                var rootMember = RootMember;

                _drawTree.Tree.Dock = DockStyle.Fill;
                _drawTree.Config.DataMember = rootMember.CreateDataTree(emTemplate);
                _drawTree.Draw(rootMember.Id, emTemplate);
            }));
        }

        #region Event Form

        private void btnBuildTree_Click(object sender, EventArgs e)
        {
            SetModeDisplay(ModeDisplay.BuildTree);
        }

        private void btnTreeView_Click(object sender, EventArgs e)
        {
            SetModeDisplay(ModeDisplay.ViewTree);
        }

        private void btnEditTree_Click(object sender, EventArgs e)
        {
            plnConfigTree.BringToFront();
            plnConfigTree.Visible = true;
            //AnimationMenu();
        }

        private void btnSaveTheme_Click(object sender, EventArgs e)
        {

        }

        private void btnCloseMenuConfig_Click(object sender, EventArgs e)
        {
            plnConfigTree.Visible = false;
        }

        private void btnCenterRoot_Click(object sender, EventArgs e)
        {
            _drawTree.CenteringRoot();
        }

        private void cboTypeTree_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboTypeTree.SelectedIndex == -1 || !btnTreeView.UseAccentColor)
            {
                return;
            }

            ReloadTree();
        }

        private void cboThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = cboThemes.SelectedValue as ThemeConfig;

            if (selected.IsHasValue())
            {
                _drawTree.Config = selected.ToConfig();

                ReloadMenuConfig();

                if (cboTypeTree.SelectedIndex > -1)
                {
                    ReloadTree();
                }
            }
        }

        private void materialButton5_Click(object sender, EventArgs e)
        {
            _drawTree.ExportPdf();
        }

        #endregion Event Form




















        //int page;
        //int numPage = 0;
        //private void ChangeFilter_Event(object sender, InfoFilter filter)
        //{
        //    if (sender is Button)
        //    {
        //        Button btn = sender as Button;
        //        if (btn.Text.Equals("<") | btn.Text.Equals(">"))
        //            btn = filter.ButtonSelect;

        //        if (btn.Tag != null)
        //        {
        //            page = int.Parse(btn.Text);
        //        }
        //        else
        //        {
        //            if (btn.Name == "btnprepage")
        //            {
        //                if (numPage > 0)
        //                {
        //                    numPage--;
        //                }
        //            }
        //            else if (btn.Name == "btnnextpage")
        //            {
        //                numPage++;
        //            }
        //        }
        //        PageNumber = numPage * 5 + page - 1;
        //    }
        //    else
        //    {
        //        PageNumber = 0;
        //    }
        //    _InfoFilter = filter;
        //    LoadMember();
        //}


        //public List<ExTMember> LoadListTMember(string keyword = "", string gender = "", string liveOrDie = "", bool inClan = true)
        //{
        //    try
        //    {
        //        var intGender = ConvertHelper.CnvNullToInt(gender);
        //        var intLiveOrDie = ConvertHelper.CnvNullToInt(liveOrDie);
        //        var isDeath = intLiveOrDie == 0 ? true : false;
        //        keyword = keyword.ToLower();
        //        var tblTMember = AppManager.DBManager.GetTable<TMember>();
        //        var test = tblTMember.CreateQuery().ToList();
        //        var dtaTMember = tblTMember.CreateQuery(
        //            i => (intGender < 0 || i.Gender == intGender) // gender
        //                 && (intLiveOrDie < 0 || i.IsDeath == isDeath) // death
        //                 && (string.IsNullOrEmpty(keyword)
        //                     || i.Name.ToLower().Contains(keyword)
        //                     || i.Contact.Tel_1.ToLower().Contains(keyword)
        //                     || i.Contact.Tel_2.ToLower().Contains(keyword)
        //                     || i.Contact.Email_1.ToLower().Contains(keyword)
        //                     || i.Contact.Email_2.ToLower().Contains(keyword)
        //                     || i.Contact.Address.ToLower().Contains(keyword))
        //            ).ToList().Select(i => new ExTMember()
        //            {
        //                Id = i.Id,
        //                Name = i.Name + "",
        //                Gender = i.Gender,
        //                IsDeath = i.IsDeath,
        //                GenderShow = (i.Gender == (int)GenderMember.Male) ? "Nam" : ((i.Gender == (int)GenderMember.Female) ? "Nữ" : "Chưa rõ"),
        //                BirthdayShow = i.Birthday != null ? i.Birthday.ToDateSun() : "",
        //                BirthdayLunarShow = i.Birthday != null ? i.Birthday.ToDateMoon() : "",
        //                DeadDaySunShow = i.DeadDay != null ? i.DeadDay.ToDateSun() : "",
        //                DeadDayLunarShow = i.DeadDay != null ? i.DeadDay.ToDateMoon() : "",
        //                Tel_1 = i.Contact.Tel_1 + "",
        //                Tel_2 = i.Contact.Tel_2 + "",
        //                Email_1 = i.Contact.Email_1 + "",
        //                Email_2 = i.Contact.Email_2 + "",
        //                Address = i.Contact.Address + "",
        //                Relation = i.Relation,
        //                Religion = i.Religion,
        //                National = i.National,
        //                TypeName = i.TypeName,
        //                HomeTown = i.HomeTown,
        //                BirthPlace = i.BirthPlace,
        //                DeadPlace = i.DeadPlace
        //            }).ToList();

        //        return dtaTMember;
        //    }
        //    catch (Exception ex)
        //    {
        //        AppManager.Dialog.Error(ex.Message);
        //        AppManager.LoggerApp.Error(typeof(ListMember), ex);
        //        return null;
        //    }
        //}

        //int PageNumber = 0;
        //private async void LoadMember()
        //{
        //    //InfoFilter Filter = _InfoFilter;

        //    //List<ExTMember> ListMember = LoadListTMember(Filter.KeyWord, Filter.Gender, Filter.LiveorDie);

        //    //string root_ID = "";

        //    //using (var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>())
        //    //{
        //    //    var objUserLogin = AppManager.LoginUser;
        //    //    var objMFamilyInfo = tblMFamilyInfo.AsEnumerable().FirstOrDefault(i => i.Id == objUserLogin.FamilyId);
        //    //    root_ID = objMFamilyInfo.RootId;
        //    //}
        //    //ucListMember.Clear();
        //    //ucListMember.Maxpage = (ListMember.Count / (ucListMember.NumberMemberInpage * 4) + (ListMember.Count % ucListMember.NumberMemberInpage) != 0 ? 1 : 0) - 1;
        //    //ucListMember.Element = ListMember.Count - numPage * (ucListMember.NumberMemberInpage * 4);
        //    //ucListMember.TotalMember = ListMember.Count;

        //    //List<UserMember> arrmembers = new List<UserMember>();

        //    //for (int i = ucListMember.NumberMemberInpage * PageNumber; i < ListMember.Count & i < ucListMember.NumberMemberInpage * PageNumber + ucListMember.NumberMemberInpage; i++)
        //    //{
        //    //    UserMember um = new UserMember(ListMember[i], root_ID);
        //    //    um.MemberEdit += MemberEditEvent;
        //    //    um.MemberMouseClicked += MemberMouseClick;
        //    //    um.MemberMouseHover += MemberMouseHover;
        //    //    um.MemberMouseLeave += MemberMouseLeave;
        //    //    um.MemberDoubleClick += MemberDouleClick;
        //    //    arrmembers.Add(um);
        //    //}
        //    //ucListMember.AddRange(arrmembers.ToArray());
        //}

        //private void MemberDouleClick(object sender, ExTMember _TMember)
        //{

        //}

        //bool bMouseLeave = false;
        //private void MemberMouseHover(object sender, ExTMember _TMember)
        //{
        //    infoMember1.Info(_TMember);
        //    Point loca = infoMember1.Location;
        //    loca.Y = ((UserMember)sender).Location.Y;
        //    if (loca.Y + infoMember1.Height > plnMain.Height)
        //    {
        //        loca.Y = plnMain.Height - infoMember1.Height - 10;
        //    }
        //    infoMember1.Location = loca;
        //    infoMember1.BringToFront();
        //}

        //private void MemberMouseLeave(object sender, ExTMember _TMember)
        //{
        //    infoMember1.SendToBack();
        //}

        //private void MemberEditEvent(object sender, TMember _TMember)
        //{
        //    AppManager.Navigation.ShowDialog<addMember, TMember>(new NavigationParameters(_TMember), ModeForm.Edit);
        //}
        //private void MemberMouseClick(object sender, TMember _TMember)
        //{
        //    clsFamilyMember clsFamilyMember_Select = _drawTree.Config.DataMember[_TMember.Id] as clsFamilyMember;
        //    _drawTree.SelectedMember = clsFamilyMember_Select;
        //    _drawTree.CenteringMember(_TMember.Id);
        //}

        //private async void SaveTheme(string _Name, bool CreateNewTheme = false)
        //{
        //    ThemeConfig _ThemeConfig = new ThemeConfig();
        //    _ThemeConfig.Id = _Name;
        //    _ThemeConfig.BackgroudColor = _drawTree.Config.BackgroudColor.ToString();
        //    _ThemeConfig.SelectedMemberColor = _drawTree.Config.SelectedMemberColor.ToString();
        //    _ThemeConfig.ChildLineColor = _drawTree.Config.ChildLineColor.ToString();
        //    _ThemeConfig.SpouseLineColor = _drawTree.Config.SpouseLineColor.ToString();
        //    _ThemeConfig.TextColor = _drawTree.Config.TextColor.ToString();
        //    _ThemeConfig.BorderColor = _drawTree.Config.BorderColor.ToString();
        //    _ThemeConfig.MaleBackColor = _drawTree.Config.MaleBackColor.ToString();
        //    _ThemeConfig.FeMaleBackColor = _drawTree.Config.FeMaleBackColor.ToString();
        //    _ThemeConfig.MemberVerticalSpace = _drawTree.Config.MemberVerticalSpace;
        //    _ThemeConfig.MemberHorizonSpace = _drawTree.Config.MemberHorizonSpace;
        //    _ThemeConfig.NumberFrame = _drawTree.Config.NumberFrame;

        //    var tblDrawTreeConfig = AppManager.DBManager.GetTable<ThemeConfig>();
        //    var ret = CreateNewTheme ? await tblDrawTreeConfig.InsertOneAsync(_ThemeConfig, false) : await tblDrawTreeConfig.UpdateOneAsync(i => i.Id == cmbTheme.Text, _ThemeConfig);
        //}

        //bool bConfig = false;
        //private void AnimationMenu()
        //{
        //    ThreadPool.QueueUserWorkItem((obj) =>
        //    {
        //        this.BeginInvoke(new Action(async () =>
        //        {
        //            while (true)
        //            {
        //                if (bConfig)
        //                {
        //                    if (plnConfigTree.Width < 201)
        //                    {
        //                        plnConfigTree.Width += 20;
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //                else
        //                {
        //                    if (plnConfigTree.Width > 1)
        //                    {
        //                        plnConfigTree.Width -= 20;
        //                    }
        //                    else
        //                    {
        //                        break;
        //                    }
        //                }
        //                await Task.Delay(10);
        //            }
        //        }));
        //    });
        //}
    }
}
