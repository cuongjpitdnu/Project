using GP40Common;
using GP40DrawTree;
using GP40Tree;
using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Database;
using GPMain.Common.FamilyEvent;
using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPMain.Properties;
using GPMain.Views.Controls;
using GPMain.Views.Member;
using GPMain.Views.MenuItems;
using GPMain.Views.Tree.Build;
using GPMain.Views.Tree.MenuItems;
using GPModels;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPMain.Views.Tree
{
    public partial class TreeViewer : BaseUserControl
    {
        private ModeDisplay _Mode;
        private bool _ExpandMode = false;
        bool menuConfigInitComplete = false;
        private ContextMenuStripData<TMember> _contextMember;
        private DrawSimpleTree _drawTree;
        private DrawTreeExpand _drawExpandTree;
        private DrawInputTree _objFTree;
        private List<ItemBase> _listItemMenuConfig;
        private InfoFilter _InfoFilter;
        private TreeModeItem urcTreeModeItem;
        private LineItem urcLineItem;
        private BackgroundItem urcBackgroundItem;
        private FrameItem urcFrameItem;
        private TextItem urcTextItem;
        private SpaceItem urcSpaceItem;
        private ShowBirthDateItem urcShowBirthDayItem;
        private ShowDeathDayItem urcShowDeathDayItem;
        private OptionDrawTreeItem urcOptionDrawTreeItem;
        private BuildTree buildTree;

        private ShowLevelInFamilyItem urcShowLevelInFamilyItem;

        MemberHelper memberHelper = new MemberHelper();

        public TreeViewer(NavigationParameters parameters, ModeForm mode) : base(parameters, mode)
        {
            InitializeComponent();
            this.Disposed += (sender, e) =>
            {
                ObjectHelper.FreeMemory(ref _contextMember);
                ObjectHelper.FreeMemory(ref _objFTree);
                ObjectHelper.FreeMemory(ref _drawTree);
                ObjectHelper.FreeMemory(ref _drawExpandTree);
                ObjectHelper.FreeMemory(ref _listItemMenuConfig);
                ObjectHelper.FreeMemory(ref buildTree);
            };
        }

        private void TreeViewer_Load(object sender, EventArgs e)
        {

        }
        public override void ProcessAffterAddForm()
        {
            base.ProcessAffterAddForm();
            InitForm();
        }
        private void InitForm()
        {
            this.InitDrawTree();
            btnEditTree.Location = new Point(plnMain.Width - btnEditTree.Width, 0);
            plnTree.Dock = DockStyle.Fill;
            var arrTypeTree = clsConst.TEMPLATE_NAME;
            List<ModelBinding> dataSourceTypeTree = new List<ModelBinding>();
            int cnt = 0;
            foreach (var typeTree in arrTypeTree)
            {
                dataSourceTypeTree.Add(new ModelBinding() { Key = typeTree, Display = clsConst.TypeTreeDisplay[cnt++] });
            }
            cboTypeTree.DataSource = dataSourceTypeTree;
            cboTypeTree.ValueMember = "Key";
            cboTypeTree.DisplayMember = "Display";
            cboTypeTree.SelectedIndex = -1;
            menuMember1.MenuMemberMouseClick += MenuMemberMouseClick_Event;
            menuMember1.Reload += (sender, mem) =>
            {
                if (mem == null) return;
                menuMember1.InitialListMember(true);
                menuMember1.CreateListMemberCardInPageIndex(true);
                if (AppManager.MenuMemberBuffer.ListAllMember == null || AppManager.MenuMemberBuffer.ListAllMember.Count == 0)
                {
                    buildTree.UpdateInfo(null, true);
                }
                else
                {
                    buildTree.UpdateInfo(mem, true);
                }
                if (_Mode == ModeDisplay.ViewTree)
                {
                    if (_ExpandMode)
                    {
                        ReloadExpandTree();
                    }
                    else
                    {
                        ReloadSimpleTree();
                    }
                }
            };
            this.SetModeDisplay();
            _InfoFilter = new InfoFilter();
            menuMember1.InitialListMember(true);
            menuMember1.CreateListMemberCardInPageIndex(true);
        }

        private void InitDrawTree()
        {
            var themeDB = AppManager.DBManager.GetTable<ThemeConfig>().FirstOrDefault() ?? new ThemeConfig();
            var configTree = themeDB.ToConfig();

            _contextMember = ContextMenuStripManager.CreateForMember();
            _contextMember.ItemClickEnd += (sender, member) =>
            {
                if (member == null) return;

                menuMember1.InitialListMember(true);
                menuMember1.CreateListMemberCardInPageIndex(true);

                if (!_ExpandMode)
                {
                    ReloadSimpleTree();
                    plnTree.Visible = false;
                    plnTree.Controls.Clear();
                    plnTree.Controls.Add(_drawTree.Tree);
                    plnTree.Visible = true;
                }
                else
                {
                    ReloadExpandTree();
                    plnTree.Visible = false;
                    plnTree.Controls.Clear();
                    plnTree.Controls.Add(_drawExpandTree.Tree);
                    plnTree.Visible = true;
                }
                //buildTree.UpdateInfo(memberHelper.TMemberToExTMember(member), true);
            };

            #region Simple Tree
            _drawTree = new DrawSimpleTree(configTree);

            _drawTree.MemberDoubleClick += (sender, e) =>
            {
                var objMember = sender as clsFamilyMember;

                if (!objMember.HasValue() || String.IsNullOrEmpty(objMember.Id))
                {
                    return;
                }

                var objTMember = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objMember.Id);

                NavigationParameters param = new NavigationParameters()
                {
                    {"Default", objTMember },
                    {"TypeTree", cboTypeTree.Text }
                };

                if (objTMember.HasValue() && AppManager.Navigation.ShowDialogWithParam<addMember>(param, ModeForm.Edit, AppConst.StatusBarColor).Result == DialogResult.OK)
                {
                    ReloadSimpleTree();
                }
            };

            _drawTree.MemberRightClick += (sender, objMember) =>
            {
                if (objMember.HasValue() && !String.IsNullOrEmpty(objMember.Id) && _contextMember.HasValue())
                {
                    var objTMember = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objMember.Id);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrAddRootFamilyMember, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveRootFamilyMember, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrAddFamilyHead, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveFamilyHead, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    ContextMenuStripManager.TypeTree = cboTypeTree.Text;
                    _contextMember.Show(UIHelper.GetCursorPosition(), objTMember);
                }
            };

            _drawTree.MouseLeftClickCanvas += (sender, e) =>
            {
                //plnConfigTree.Visible = false;
                //btnEditTree.Visible = true;
                //ShowMenuMember(false);
            };

            plnMain.BackColor = _drawTree.Config.BackgroudColor.ToDrawingColor();
            #endregion

            #region Expand Tree
            _drawExpandTree = new DrawTreeExpand(configTree);

            _drawExpandTree.MemberDoubleClick += (sender, e) =>
            {
                var objMember = sender as clsFamilyMember;

                if (!objMember.HasValue() || String.IsNullOrEmpty(objMember.Id))
                {
                    return;
                }
                var objTMember = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objMember.Id);

                NavigationParameters param = new NavigationParameters()
                {
                    {"Member", objTMember },
                    {"TypeTree", cboTypeTree.Text }
                };

                if (objTMember.HasValue() && AppManager.Navigation.ShowDialogWithParam<addMember>(param, ModeForm.Edit, AppConst.StatusBarColor).Result == DialogResult.OK)
                {
                    ReloadExpandTree();
                }
            };

            _drawExpandTree.MemberRightClick += (sender, objMember) =>
            {
                if (objMember.HasValue() && !String.IsNullOrEmpty(objMember.Id) && _contextMember.HasValue())
                {
                    var objTMember = AppManager.DBManager.GetTable<TMember>().FirstOrDefault(i => i.Id == objMember.Id);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrAddRootFamilyMember, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveRootFamilyMember, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrAddFamilyHead, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    _contextMember.EnaleItem(ContextMenuStripManager.cstrRemoveFamilyHead, objMember.InRootTree && objMember.Gender == clsConst.ENUM_GENDER.Male);
                    ContextMenuStripManager.TypeTree = cboTypeTree.Text;
                    _contextMember.Show(UIHelper.GetCursorPosition(), objTMember);
                }
            };
            _drawExpandTree.MouseLeftClickCanvas += (sender, e) =>
                {
                    //plnConfigTree.Visible = false;
                    //btnEditTree.Visible = true;
                    //ShowMenuMember(false);
                };
            #endregion
        }

        private void SelectConfigMenuItem(DrawTreeConfig config)
        {
            urcLineItem.ReloadConfig(config);
            urcBackgroundItem.ReloadConfig(config);
            urcFrameItem.ReloadConfig(config);
            urcTextItem.ReloadConfig(config);
            urcSpaceItem.ReloadConfig(config);
            urcShowBirthDayItem.ReloadConfig(config);
            urcShowDeathDayItem.ReloadConfig(config);
            urcOptionDrawTreeItem.ReloadConfig(config);
            urcShowLevelInFamilyItem.ReloadConfig(config);
        }

        private void InitMenuConfig()
        {
            if (menuConfigInitComplete) return;
            this.BeginInvoke(new Action(() =>
            {
                var dataThemes = AppManager.DBManager.GetTable<ThemeConfig>().CreateQuery().Select(i => new DataBinding<ThemeConfig>()
                {
                    Display = i.DisplayName.Replace("Theme", "Chủ đề "),
                    Value = i,
                }).ToList();

                var themeDB = dataThemes.FirstOrDefault()?.Value ?? new ThemeConfig();
                BindingHelper.Combobox(cboThemes, dataThemes, themeDB);
                urcTreeModeItem = new TreeModeItem(_drawTree.Config);
                urcTreeModeItem.ChangeMode += (sender, expandMode) =>
                {
                    if (_ExpandMode != expandMode)
                    {
                        _ExpandMode = expandMode;
                        if (expandMode)
                        {
                            _drawExpandTree.Config = _drawTree.Config;
                            SelectConfigMenuItem(_drawExpandTree.Config);
                            ReloadExpandTree();
                            plnTree.Visible = false;
                            plnTree.Controls.Clear();
                            plnTree.Controls.Add(_drawExpandTree.Tree);
                            plnTree.Visible = true;
                        }
                        else
                        {
                            _drawTree.Config = _drawExpandTree.Config;
                            SelectConfigMenuItem(_drawTree.Config);
                            ReloadSimpleTree();
                            plnTree.Visible = false;
                            plnTree.Controls.Clear();
                            plnTree.Controls.Add(_drawTree.Tree);
                            plnTree.Visible = true;
                        }
                    }
                };
                urcLineItem = new LineItem(_drawTree.Config);
                urcBackgroundItem = new BackgroundItem(_drawTree.Config);
                urcFrameItem = new FrameItem(_drawTree.Config);
                urcTextItem = new TextItem(_drawTree.Config);
                urcSpaceItem = new SpaceItem(_drawTree.Config);
                urcShowBirthDayItem = new ShowBirthDateItem(_drawTree.Config);
                urcShowDeathDayItem = new ShowDeathDayItem(_drawTree.Config);
                urcOptionDrawTreeItem = new OptionDrawTreeItem(_drawTree.Config);
                urcShowLevelInFamilyItem = new ShowLevelInFamilyItem(_drawTree.Config);

                _listItemMenuConfig = new List<ItemBase>()
                {
                  urcTreeModeItem,
                  urcLineItem,
                  urcBackgroundItem,
                  urcFrameItem,
                  urcTextItem,
                  urcSpaceItem,
                  urcShowBirthDayItem,
                  urcShowDeathDayItem,
                  urcOptionDrawTreeItem,
                  urcShowLevelInFamilyItem
                };

                _listItemMenuConfig.ForEach(itemMenuConfig => itemMenuConfig.ChangeData += (config) =>
                {
                    if (_ExpandMode)
                    {
                        _drawExpandTree.Config = config;
                        ReloadExpandTree();
                    }
                    else
                    {
                        _drawTree.Config = config;
                        ReloadSimpleTree();
                    }
                });
                Dictionary<string, ItemBase> items = new Dictionary<string, ItemBase>()
                {
                     {"Chế độ vẽ cây rút gọn", urcOptionDrawTreeItem},
                     {"Định dạng ngày mất", urcShowDeathDayItem},
                     {"Định dạng ngày sinh", urcShowBirthDayItem},
                     {"Đường kẻ", urcLineItem},
                     {"Màu nền", urcBackgroundItem},
                     {"Khung ảnh", urcFrameItem},
                     {"Chữ hiển thị", urcTextItem},
                     {"Khoảng cách", urcSpaceItem},
                     {"Số đời hiển thị", urcShowLevelInFamilyItem},
                     { "Chế độ vẽ cây", urcTreeModeItem }
                };

                menuConfig.AddRange(items);
                menuConfig.Controls[0].Visible = false;
                menuConfigInitComplete = true;
            }));
        }

        private void SetModeDisplay(ModeDisplay mode = ModeDisplay.BuildTree)
        {
            _Mode = mode;
            AppManager.ModeDisplay = mode;
            switch (mode)
            {
                case ModeDisplay.ViewTree:
                    this.InitMenuConfig();
                    plnConfigTree.BringToFront();
                    plnControl.BringToFront();
                    btnEditTree.BringToFront();
                    plnTree.Visible = false;
                    plnTree.Controls.Clear();
                    if (_ExpandMode)
                    {
                        ReloadExpandTree();
                        plnTree.Controls.Add(_drawExpandTree.Tree);
                    }
                    else
                    {
                        ReloadSimpleTree();
                        plnTree.Controls.Add(_drawTree.Tree);
                    }
                    plnTree.Visible = true;
                    if (cboTypeTree.Items.Count > 0)
                    {
                        cboTypeTree.SelectedIndex = 0;
                    }
                    btnEditTree.Visible = false;
                    plnConfigTree.Visible = true;
                    plnControl.Visible = true;
                    btnhidemenumember.Visible = MainLayout.ColumnStyles[0].Width > 0;
                    btnshowmenumember.Visible = MainLayout.ColumnStyles[0].Width == 0;
                    break;
                case ModeDisplay.BuildTree:
                    plnTree.Controls.Clear();
                    btnhidemenumember.Visible = false;
                    btnshowmenumember.Visible = false;
                    MainLayout.ColumnStyles[0].Width = 260;
                    plnTree.Visible = false;

                    _objFTree = new DrawInputTree();
                    try
                    {
                        this.BeginInvoke(new Action(() =>
                        {
                            ExTMember member = AppManager.MenuMemberBuffer.MemberCurrent;
                            if (member == null)
                            {
                                var rootMember = memberHelper.TMemberToExTMember(memberHelper.RootTMember);
                                member = rootMember ?? AppManager.MenuMemberBuffer.ListMember.FirstOrDefault().Value;
                            }
                            buildTree = new BuildTree(member);
                            plnTree.SuspendLayout();
                            buildTree.SuspendLayout();
                            buildTree.MemberEdit += MemberEdit_Event;
                            buildTree.MemberSelect += MemberSelect_Event;
                            buildTree.DeleteRelationComplete += (sender, member) =>
                            {
                                if (member == null)
                                {
                                    if (AppManager.MenuMemberBuffer.ListMember == null || AppManager.MenuMemberBuffer.ListMember.Count == 0)
                                    {
                                        var temp = memberHelper.FindExTMemberOutDictionary("", "", "");
                                        if (temp.Count > 0)
                                        {
                                            member = temp.FirstOrDefault().Value;
                                        }
                                        AppManager.MenuMemberBuffer.ListAllMember = AppManager.MenuMemberBuffer.ListMember = temp;
                                    }
                                    else if (AppManager.MenuMemberBuffer.ListMember.Count > 0)
                                    {
                                        var rootMember = memberHelper.TMemberToExTMember(memberHelper.RootTMember);
                                        member = rootMember ?? AppManager.MenuMemberBuffer.ListMember.FirstOrDefault().Value;
                                    }
                                }
                                menuMember1.InitialListMember(true);
                                menuMember1.CreateListMemberCardInPageIndex(true);
                                buildTree.UpdateInfo(memberHelper.GetExTMemberByID(member.Id));
                            };
                            buildTree.ReloadMember += (sender, member) =>
                             {
                                 if (member == null || memberHelper.GetExTMemberByID(member.Id) == null)
                                 {
                                     if (AppManager.MenuMemberBuffer.ListMember == null || AppManager.MenuMemberBuffer.ListMember.Count <= 1)
                                     {
                                         var temp = memberHelper.FindExTMemberOutDictionary("", "", "");
                                         if (temp.Count > 0)
                                         {
                                             member = temp.FirstOrDefault().Value;
                                         }
                                         AppManager.MenuMemberBuffer.ListAllMember = AppManager.MenuMemberBuffer.ListMember = temp;
                                     }
                                     else if (AppManager.MenuMemberBuffer.ListMember.Count > 0)
                                     {
                                         var rootMember = memberHelper.TMemberToExTMember(memberHelper.RootTMember);
                                         member = rootMember ?? AppManager.MenuMemberBuffer.ListMember.FirstOrDefault().Value;
                                     }
                                 }
                                 menuMember1.InitialListMember(true);
                                 menuMember1.CreateListMemberCardInPageIndex(true);
                                 if (member == null)
                                 {
                                     buildTree.UpdateInfo(null, true);
                                 }
                                 else
                                 {

                                     var memberTemp = memberHelper.GetExTMemberByID(member.Id);
                                     buildTree.UpdateInfo(memberTemp, true);
                                 }
                             };
                            buildTree.Dock = DockStyle.Fill;
                            plnTree.Controls.Add(buildTree);//(_objFTree.Tree);   
                            buildTree.ResumeLayout();
                            plnTree.ResumeLayout();
                            buildTree.UpdateInfo(member);
                        }));
                    }
                    catch { }
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
            if (_listItemMenuConfig.HasValue())
            {
                _listItemMenuConfig.ForEach(itemMenuConfig => itemMenuConfig.ReloadConfig(_drawTree.Config));
            }
        }

        private void ReloadSimpleTree(TMember member = null)
        {
            var selected = cboTypeTree.SelectedValue?.ToString() + "";
            clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
            Enum.TryParse(selected, out emTemplate);

            plnMain.BackColor = _drawTree.Config.BackgroudColor.ToDrawingColor();
            try
            {
                plnTree.BeginInvoke(new Action(() =>
                {
                    var rootMember = memberHelper.RootTMember;
                    if (rootMember == null) return;
                    if (member != null)
                    {
                        rootMember = member;
                    }
                    _drawTree.Tree.Dock = DockStyle.Fill;
                    _drawTree.Config.DataMember = rootMember.CreateDataTree(emTemplate, _drawTree.Config.MaxLevelInFamily);
                    _drawTree.OffsetYCanvas = 10;
                    _drawTree.Draw(rootMember.Id, emTemplate);
                }));
            }
            catch { }
        }

        private void ReloadExpandTree(TMember member = null)
        {
            var selected = cboTypeTree.SelectedValue?.ToString() + "";
            clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
            Enum.TryParse(selected, out emTemplate);

            plnMain.BackColor = _drawExpandTree.Config.BackgroudColor.ToDrawingColor();
            try
            {
                plnTree.BeginInvoke(new Action(() =>
                {
                    var rootMember = memberHelper.RootTMember;
                    if (rootMember == null) return;
                    if (member != null)
                    {
                        rootMember = member;
                    }
                    _drawExpandTree.Tree.Dock = DockStyle.Fill;
                    _drawExpandTree.Config.DataMember = rootMember.CreateDataTree(emTemplate, _drawExpandTree.Config.MaxLevelInFamily);
                    _drawExpandTree.OffsetYCanvas = 10;
                    _drawExpandTree.Draw(rootMember.Id, emTemplate);
                }));
            }
            catch { }
        }

        private void MenuMemberMouseClick_Event(object sender, ExTMember member)
        {
            switch (_Mode)
            {
                case ModeDisplay.ViewTree:
                    var selected = cboTypeTree.SelectedValue?.ToString() + "";
                    clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
                    Enum.TryParse(selected, out emTemplate);
                    if (_ExpandMode && _drawExpandTree != null)
                    {
                        _drawExpandTree.CenteringMember(member.Id);
                    }
                    else if (!_ExpandMode && _drawTree != null)
                    {
                        _drawTree.CenteringMember(member.Id);
                    }
                    break;
                case ModeDisplay.BuildTree:
                    buildTree.UpdateInfo(member);
                    AppManager.MenuMemberBuffer.MemberCurrent = member;
                    break;
            }
        }

        private void MemberEdit_Event(object sender, ParamRequest paramRequest)
        {
            this.Cursor = Cursors.WaitCursor;
            var newParams = new NavigationParameters();
            var dicRelation = new Dictionary<string, string>();
            DialogResult dialogResult = DialogResult.Cancel;
            if (paramRequest.Mode == ModeForm.Edit)
            {
                newParams = new NavigationParameters(paramRequest.RelationMember);
                try
                {
                    dialogResult = AppManager.Navigation.ShowDialogWithParam<addMember>(newParams, ModeForm.Edit, AppConst.StatusBarColor).Result;
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(addMember), ex);
                }
            }
            else
            {
                var tblMRelation = AppManager.DBManager.GetTable<MRelation>();
                var tblTMemberRelation = AppManager.DBManager.GetTable<TMemberRelation>().ToList();

                var dataMRelation = tblMRelation.ToList(i => i.DeleteDate == null);

                if (dataMRelation != null)
                {
                    foreach (var item in dataMRelation)
                    {
                        dicRelation[item.Id] = item.MainRelation + " - " + item.RelatedRelation;
                    }
                }

                object relationTypeSelected = null;
                object relationTypeSelected2 = null;
                string gender = "";
                ExTMember subMember = paramRequest.SpouseMember;
                if (paramRequest.RelationMember.Name == AppConst.NameDefaul.Father)
                {
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_DAD}01 - {Relation.PREFIX_CHILD}01").Key;
                    gender = AppConst.Gender.Male;
                    var motherRel = tblTMemberRelation.FirstOrDefault(x => x.memberId == paramRequest.MainMember.Id && x.relType == $"{Relation.PREFIX_MOM}01");
                    if (motherRel != null)
                    {
                        var mother = memberHelper.GetExTMemberByID(motherRel.relMemberId);
                        subMember = mother;
                        relationTypeSelected2 = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_WIFE}01 - {Relation.PREFIX_HUSBAND}01").Key;
                    }
                }
                else if (paramRequest.RelationMember.Name == AppConst.NameDefaul.Mother)
                {
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_MOM}01 - {Relation.PREFIX_CHILD}01").Key;
                    gender = AppConst.Gender.Female;
                    var fatherRel = tblTMemberRelation.FirstOrDefault(x => x.memberId == paramRequest.MainMember.Id && x.relType == $"{Relation.PREFIX_DAD}01");
                    if (fatherRel != null)
                    {
                        var father = memberHelper.GetExTMemberByID(fatherRel.relMemberId);
                        subMember = father;
                        relationTypeSelected2 = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_HUSBAND}01 - {Relation.PREFIX_WIFE}01").Key;
                    }
                }
                else if (paramRequest.RelationMember.Name == AppConst.NameDefaul.Husban)
                {
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_HUSBAND}01 - {Relation.PREFIX_WIFE}01").Key;
                    gender = AppConst.Gender.Male;
                }
                else if (paramRequest.RelationMember.Name == AppConst.NameDefaul.Wife)
                {
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{Relation.PREFIX_WIFE}01 - {Relation.PREFIX_HUSBAND}01").Key;
                    gender = AppConst.Gender.Female;
                }
                else if (paramRequest.RelationMember.Name == AppConst.NameDefaul.Child)
                {
                    string sParent = $"{Relation.PREFIX_DAD}01";
                    string sChild = $"{Relation.PREFIX_CHILD}01";

                    string sParent2 = $"{Relation.PREFIX_MOM}01";
                    string sChild2 = $"{Relation.PREFIX_CHILD}03";
                    if (paramRequest.MainMember.GenderShow.Equals(AppConst.Gender.Female))
                    {
                        sParent = $"{Relation.PREFIX_MOM}01";
                        sChild = $"{Relation.PREFIX_CHILD}03";

                        sParent2 = $"{Relation.PREFIX_DAD}01";
                        sChild2 = $"{Relation.PREFIX_CHILD}01";
                    }
                    relationTypeSelected = dicRelation.FirstOrDefault(v => v.Value == $"{sChild} - {sParent}").Key;
                    relationTypeSelected2 = dicRelation.FirstOrDefault(v => v.Value == $"{sChild2} - {sParent2}").Key;
                }
                newParams.Add("type_member", paramRequest.RelationMember.Name);
                newParams.Add("sub_member", subMember);
                newParams.Add("primary_member", paramRequest.MainMember);
                newParams.Add("relation_type", relationTypeSelected);
                newParams.Add("relation_type2", relationTypeSelected2);
                newParams.Add("gender", gender);
                try
                {
                    dialogResult = AppManager.Navigation.ShowDialogWithParam<popupAddRelation>(newParams, ModeForm.New, new StatusBarColor() { BackColor = AppConst.StatusBarBackColor, ForeColor = AppConst.StatusBarForeColor }).Result;
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error(ex.Message);
                    AppManager.LoggerApp.Error(typeof(popupAddRelation), ex);
                }
            }
            if (dialogResult == DialogResult.OK)
            {
                menuMember1.InitialListMember(true);
                menuMember1.CreateListMemberCardInPageIndex(true);
                AppManager.MenuMemberBuffer.MemberCurrent = memberHelper.GetExTMemberByID(paramRequest.MainMember.Id);
                if (AppManager.MenuMemberBuffer.MemberCurrent == null)
                {
                    if (AppManager.MenuMemberBuffer.ListAllMember == null || AppManager.MenuMemberBuffer.ListAllMember.Count == 0)
                    {
                        buildTree.UpdateInfo(null);
                    }
                    else if (AppManager.MenuMemberBuffer.ListAllMember.Count > 0)
                    {
                        var rootMember = memberHelper.TMemberToExTMember(memberHelper.RootTMember);
                        buildTree.UpdateInfo(rootMember ?? AppManager.MenuMemberBuffer.ListAllMember.FirstOrDefault().Value);
                    }
                }
                else
                {
                    buildTree.UpdateInfo(AppManager.MenuMemberBuffer.MemberCurrent);
                }
            }
            this.Cursor = Cursors.Default;
        }
        private void MemberSelect_Event(object sender, ExTMember member)
        {
            buildTree.UpdateInfo(member);
        }
        #region Event Form

        private void BtnBuildTree_Click(object sender, EventArgs e)
        {
            SetModeDisplay(ModeDisplay.BuildTree);

        }

        private void BtnTreeView_Click(object sender, EventArgs e)
        {
            SetModeDisplay(ModeDisplay.ViewTree);
        }

        private void BtnEditTree_Click(object sender, EventArgs e)
        {
            plnConfigTree.BringToFront();
            plnConfigTree.Visible = true;
            //AnimationMenu();
        }

        private async void BtnSaveTheme_Click(object sender, EventArgs e)
        {
            var themeDB = AppManager.DBManager.GetTable<ThemeConfig>();
            var temp = themeDB.ToList().FirstOrDefault(i => i.DisplayName == cboThemes.Text);
            bool InsertResquest = temp == null;
            DrawTreeManager drawTree = _drawTree;
            if (_ExpandMode)
            {
                drawTree = _drawExpandTree;
            }
            ThemeConfig theme = new ThemeConfig()
            {
                BackgroudColor = drawTree.Config.BackgroudColor.ToString(),
                BorderColor = drawTree.Config.BorderColor.ToString(),
                ChildLineColor = drawTree.Config.ChildLineColor.ToString(),
                FeMaleBackColor = drawTree.Config.FeMaleBackColor.ToString(),
                MaleBackColor = drawTree.Config.MaleBackColor.ToString(),
                UnknowBackColor = drawTree.Config.UnknowBackColor.ToString(),
                MemberHorizonSpace = drawTree.Config.MemberHorizonSpace,
                MemberVerticalSpace = drawTree.Config.MemberVerticalSpace,
                NumberFrame = drawTree.Config.NumberFrame < 0 ? 0 : drawTree.Config.NumberFrame,
                SelectedMemberColor = drawTree.Config.SelectedMemberColor.ToString(),
                SpouseLineColor = drawTree.Config.SpouseLineColor.ToString(),
                TextColor = drawTree.Config.TextColor.ToString(),
                DisplayName = cboThemes.Text,
                ShowImage = drawTree.Config.ShowImage,
                ShowBirthDayDefaul = drawTree.Config.ShowBirthDayDefaul,
                ShowDeathDayLunarCalendar = drawTree.Config.ShowDeathDayLunarCalendar,
                ShowFamilyLevel = drawTree.Config.ShowFamilyLevel,
                ShowGender = drawTree.Config.ShowGender,
                TypeTextShow = drawTree.Config.TypeTextShow.ToString(),
                Id = InsertResquest ? LiteDBManager.CreateNewId() : temp.Id
            };

            var bSave = InsertResquest ? await themeDB.InsertOneAsync(theme) : await themeDB.UpdateOneAsync(i => i.Id == theme.Id, theme);
            if (bSave)
            {
                var dataThemes = AppManager.DBManager.GetTable<ThemeConfig>().CreateQuery().Select(i => new DataBinding<ThemeConfig>()
                {
                    Display = i.DisplayName,
                    Value = i,
                }).ToList();
                var themeDB2 = dataThemes.FirstOrDefault(x => x.Value.DisplayName == cboThemes.Text)?.Value ?? new ThemeConfig();
                BindingHelper.Combobox(cboThemes, dataThemes, themeDB2);
            }
        }

        private void BtnCloseMenuConfig_Click(object sender, EventArgs e)
        {
            plnConfigTree.Visible = false;
            btnEditTree.Visible = true;
        }

        private void BtnCenterRoot_Click(object sender, EventArgs e)
        {
            if (_ExpandMode)
            {
                _drawExpandTree.CenteringRoot();
            }
            else
            {
                _drawTree.CenteringRoot();
            }
        }

        private void CboTypeTree_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            if (cboTypeTree.SelectedIndex == -1 || !btnTreeView.UseAccentColor)
            {
                return;
            }
            var selected = cboTypeTree.SelectedValue.ToString();
            clsConst.ENUM_MEMBER_TEMPLATE emTemplate = clsConst.ENUM_MEMBER_TEMPLATE.MCTFull;
            Enum.TryParse(selected, out emTemplate);
            if (menuConfig.Controls.Count > 0)
            {
                if (emTemplate == clsConst.ENUM_MEMBER_TEMPLATE.MCTVeryShort)
                {
                    menuConfig.Controls[0].Visible = true;
                }
                else
                {
                    menuConfig.Controls[0].Visible = false;
                }
            }
            if (_ExpandMode)
            {
                ReloadExpandTree();
            }
            else
            {
                ReloadSimpleTree();
            }
        }

        private void CboThemes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selected = cboThemes.SelectedValue as ThemeConfig;
            if (selected.HasValue())
            {
                ReloadMenuConfig();
                if (cboTypeTree.SelectedIndex > -1)
                {
                    if (_ExpandMode)
                    {
                        int levelInFamily = _drawExpandTree.Config.MaxLevelInFamily;
                        _drawExpandTree.Config = selected.ToConfig();
                        _drawExpandTree.Config.MaxLevelInFamily = levelInFamily;
                        ReloadExpandTree();
                    }
                    else
                    {
                        int levelInFamily = _drawTree.Config.MaxLevelInFamily;
                        _drawTree.Config = selected.ToConfig();
                        _drawTree.Config.MaxLevelInFamily = levelInFamily;
                        ReloadSimpleTree();
                    }
                }
            }
        }

        //Sự kiện button xuất file PDF
        private void BtnnExportPDF_Click(object sender, EventArgs e)
        {
            NavigationResult naviResult = AppManager.Navigation.ShowDialog<OptionExportPDF>(ModeForm.None, AppConst.StatusBarColor);
            if (naviResult.Result != DialogResult.OK) return;

            InfoOptionExportPDF optionExportPDF = naviResult.GetValue<InfoOptionExportPDF>("OptionExportPDF");
            var familyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(x => x.Id == AppManager.LoginUser.FamilyId);
            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF|*.pdf",
                FileName = $"Cây phả hệ dòng họ {familyInfo?.FamilyName ?? ""} _ {(_ExpandMode ? "Cây mở rộng" : "Cây cơ bản")}.pdf"
            };
            bool bExportPDF = false;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string dateExport = DateTime.Now.ToString("HH:mm:ss");
                    AppManager.LoggerApp.Debug(typeof(Application), $"Start Export: {dateExport}");
                    AppManager.Dialog.ShowProgressBar((progressBar) =>
                       {
                           if (_ExpandMode)
                           {
                               //Xuất file PDF cây mở rộng
                               bExportPDF = _drawExpandTree.ExportPdf(sfd.FileName, progressBar, optionExportPDF);
                           }
                           else
                           {
                               //Xuất file PDF cây cơ bản
                               bExportPDF = _drawTree.ExportPdf(sfd.FileName, progressBar, optionExportPDF);
                           }
                       }, "Đang xuất file PDF...", AppConst.TitleBarFisrt + "Xuất file PDF");

                    AppManager.LoggerApp.Debug(typeof(Application), $"End Export: {dateExport}");
                    if (bExportPDF) AppManager.Dialog.Ok("Xuất file PDF thành công!!!");
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error($"Xuất file PDF thất bại!!!.\n{ex.Message}");
                    AppManager.LoggerApp.Error(typeof(Application), ex);
                }
                finally
                {
                    //Xác nhận mở file PDF vừa được tạo
                    if (bExportPDF && File.Exists(sfd.FileName) && AppManager.Dialog.Confirm("Bạn có muốn mở file vừa mới được tạo?"))
                    {
                        Process.Start(sfd.FileName);
                    }
                }
            }
        }

        //Sự kiện button xuất file SVG
        private void BtnSVG_Click(object sender, EventArgs e)
        {
            NavigationResult naviResult = AppManager.Navigation.ShowDialog<OptionExportPDF>(ModeForm.None, AppConst.StatusBarColor);
            if (naviResult.Result != DialogResult.OK) return;

            InfoOptionExportPDF optionExportPDF = naviResult.GetValue<InfoOptionExportPDF>("OptionExportPDF");
            var familyInfo = AppManager.DBManager.GetTable<MFamilyInfo>().FirstOrDefault(x => x.Id == AppManager.LoginUser.FamilyId);

            SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "SVG|*.svg",
                FileName = $"Cây phả hệ dòng họ {familyInfo?.FamilyName ?? ""} _ {(_ExpandMode ? "Cây mở rộng" : "Cây cơ bản")}.svg"
            };
            bool bExportSVG = false;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string dateExportSVG = DateTime.Now.ToString("HH:mm:ss");
                    AppManager.LoggerApp.Debug(typeof(Application), $"Start Export: {dateExportSVG}");
                    AppManager.Dialog.ShowProgressBar((progressBar) =>
                    {
                        if (_ExpandMode)
                        {
                            //Xuất file SVG cây mở rộng
                            bExportSVG = _drawExpandTree.ExportSVG(sfd.FileName, progressBar, optionExportPDF);
                        }
                        else
                        {
                            //Xuất file SVG cây cơ bản
                            bExportSVG = _drawTree.ExportSVG(sfd.FileName, progressBar, optionExportPDF);
                        }
                    }, "Đang xuất file SVG...", AppConst.TitleBarFisrt + "Xuất file SVG");

                    AppManager.LoggerApp.Debug(typeof(Application), $"End Export: {dateExportSVG}");
                    if (bExportSVG) AppManager.Dialog.Ok("Xuất file SVG thành công!!!");
                }
                catch (Exception ex)
                {
                    AppManager.Dialog.Error($"Xuất file SVG thất bại!!!.\n{ex.Message}");
                    AppManager.LoggerApp.Error(typeof(Application), ex);
                }
                finally
                {
                    //Xác nhận mở file SVG vừa được tạo
                    if (bExportSVG && File.Exists(sfd.FileName) && AppManager.Dialog.Confirm("Bạn có muốn mở file vừa mới được tạo?"))
                    {
                        Process.Start(sfd.FileName);
                    }
                }
            }
        }

        private void Btnshowmenumember_Click(object sender, EventArgs e)
        {
            ShowMenuMember(true);
        }

        private void Btnhidemenumember_Click(object sender, EventArgs e)
        {
            ShowMenuMember(false);
        }
        #endregion Event Form

        private void ShowMenuMember(bool enable)
        {
            if (enable)
            {
                MainLayout.ColumnStyles[0].Width = 260;
                btnhidemenumember.Visible = true;
                btnshowmenumember.Visible = false;
            }
            else
            {
                MainLayout.ColumnStyles[0].Width = 0;
                btnhidemenumember.Visible = false;
                btnshowmenumember.Visible = true;
            }
        }


    }
}