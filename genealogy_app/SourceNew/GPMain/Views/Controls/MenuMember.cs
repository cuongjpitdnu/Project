using GPCommon;
using GPConst;
using GPMain.Common;
using GPMain.Common.Helper;
using GPMain.Properties;
using GPMain.Views.Tree.Build;
using GPModels;
using MaterialSkin;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GPMain.Views.Controls
{
    public partial class MenuMember : UserControl
    {
        public EventHandler<ExTMember> Reload;
        public event EventHandler<ExTMember> MenuMemberMouseClick;
        public override Color BackColor
        {
            get => base.BackColor;
            set
            {
                base.BackColor = value;
                if (panelfilter != null) panelfilter.BackColor = value;
                if (flpMember != null) flpMember.BackColor = value;
            }
        }
        private bool oddMember = false;
        private bool initComplete = false;
        private string sGender = "";
        private string sLiveOrDie = "";

        Panel upBorderUserMember;
        Panel downBorderUserMember;
        Panel leftBorderUserMember;
        Panel rightBorderUserMember;
        public MenuMember()
        {
            InitializeComponent();
            upBorderUserMember = new Panel()
            {
                Size = new Size(AppManager.MenuMemberBuffer.UserMemberWidth + 8, 2),
                BackColor = Color.Red
            };

            downBorderUserMember = new Panel()
            {
                Size = new Size(AppManager.MenuMemberBuffer.UserMemberWidth + 8, 2),
                BackColor = Color.Red
            };

            leftBorderUserMember = new Panel()
            {
                Size = new Size(2, AppManager.MenuMemberBuffer.UserMemberHeight + 8),
                BackColor = Color.Red
            };

            rightBorderUserMember = new Panel()
            {
                Size = new Size(2, AppManager.MenuMemberBuffer.UserMemberHeight + 8),
                BackColor = Color.Red
            };

            Control[] listControl = new Control[4];
            listControl[0] = upBorderUserMember;
            listControl[1] = downBorderUserMember;
            listControl[2] = leftBorderUserMember;
            listControl[3] = rightBorderUserMember;

            pnlLayoutMember.SuspendLayout();
            pnlLayoutMember.Controls.AddRange(listControl);
            pnlLayoutMember.ResumeLayout();

            upBorderUserMember.BringToFront();
            downBorderUserMember.BringToFront();
            leftBorderUserMember.BringToFront();
            rightBorderUserMember.BringToFront();

            flpPage.Left = (panelfilter.Width - flpPage.Width) / 2;
            flpPage.SizeChanged += (sender, e) =>
            {
                flpPage.Left = (panelfilter.Width - flpPage.Width) / 2;
            };
        }

        public void LoadConfig()
        {
            try
            {
                this.BeginInvoke(new Action(() =>
                {
                    BindingHelper.Combobox(cmbGender, GenerateData.GetListGender());
                    BindingHelper.Combobox(cmbLiveOrDie, GenerateData.GetListMemberStatus());
                    this.Invoke(new Action(() =>
                    {
                        SetBinding();
                    }));
                    SetEvent();
                }));
            }
            catch { }
        }

        private void SetBinding()
        {
            lbltotalmember.DataBindings.Add("Text", AppManager.MenuMemberBuffer, "StrTotalMember");

            btnpage1.DataBindings.Add("Text", AppManager.MenuMemberBuffer, "Text1");
            btnpage2.DataBindings.Add("Text", AppManager.MenuMemberBuffer, "Text2");
            btnpage3.DataBindings.Add("Text", AppManager.MenuMemberBuffer, "Text3");
            btnpage4.DataBindings.Add("Text", AppManager.MenuMemberBuffer, "Text4");

            btnpage1.DataBindings.Add("ForeColor", AppManager.MenuMemberBuffer, "ForeColor1");
            btnpage2.DataBindings.Add("ForeColor", AppManager.MenuMemberBuffer, "ForeColor2");
            btnpage3.DataBindings.Add("ForeColor", AppManager.MenuMemberBuffer, "ForeColor3");
            btnpage4.DataBindings.Add("ForeColor", AppManager.MenuMemberBuffer, "ForeColor4");

            btnpage1.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "Visible1");
            btnpage2.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "Visible2");
            btnpage3.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "Visible3");
            btnpage4.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "Visible4");

            btnnextpage.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "ButtonNextVisible");
            btnprepage.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "ButtonPreVisible");

            upBorderUserMember.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "MemberSelected");
            downBorderUserMember.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "MemberSelected");
            leftBorderUserMember.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "MemberSelected");
            rightBorderUserMember.DataBindings.Add("Visible", AppManager.MenuMemberBuffer, "MemberSelected");

            upBorderUserMember.DataBindings.Add("Location", AppManager.MenuMemberBuffer, "UpFrame");
            downBorderUserMember.DataBindings.Add("Location", AppManager.MenuMemberBuffer, "DownFrame");
            leftBorderUserMember.DataBindings.Add("Location", AppManager.MenuMemberBuffer, "LeftFrame");
            rightBorderUserMember.DataBindings.Add("Location", AppManager.MenuMemberBuffer, "RightFrame");
        }

        Timer timerDelayMouseWheel = new Timer();
        public void SetEvent()
        {
            btnpage1.Click += CreateListMemberCardInPageIndex_Event;
            btnpage2.Click += CreateListMemberCardInPageIndex_Event;
            btnpage3.Click += CreateListMemberCardInPageIndex_Event;
            btnpage4.Click += CreateListMemberCardInPageIndex_Event;

            btnnextpage.Click += ChangePage_Event;
            btnprepage.Click += ChangePage_Event;
            cmbGender.SelectedIndexChanged += FilterMember_Event;
            cmbLiveOrDie.SelectedIndexChanged += FilterMember_Event;
            timerDelayMouseWheel.Interval = 300;
            timerDelayMouseWheel.Tick += (sender, e) =>
            {
                timerDelayMouseWheel.Stop();
                CreateListMemberCardInPageIndex(true);
            };
            pnlLayoutMember.MouseWheel += (sender, e) =>
            {
                if (AppManager.MenuMemberBuffer.PageIndex < AppManager.MenuMemberBuffer.MaxPage || AppManager.MenuMemberBuffer.PageIndex > 1)
                    timerDelayMouseWheel.Stop();
                int pageTemp = AppManager.MenuMemberBuffer.PageIndex;
                if (e.Delta < 0)
                {
                    if (AppManager.MenuMemberBuffer.PageIndex < AppManager.MenuMemberBuffer.MaxPage)
                    {
                        AppManager.MenuMemberBuffer.Page = (AppManager.MenuMemberBuffer.PageIndex + 1) / 4 +
                                                          ((AppManager.MenuMemberBuffer.PageIndex + 1) % 4 != 0 ? 1 : 0);
                        AppManager.MenuMemberBuffer.PageIndex++;
                        AppManager.MenuMemberBuffer.MemberSelected = false;
                        Application.DoEvents();
                    }
                }
                else if (e.Delta > 0)
                {
                    if (AppManager.MenuMemberBuffer.PageIndex > 1)
                    {
                        AppManager.MenuMemberBuffer.Page = (AppManager.MenuMemberBuffer.PageIndex - 1) / 4 +
                                                          ((AppManager.MenuMemberBuffer.PageIndex - 1) % 4 != 0 ? 1 : 0);
                        AppManager.MenuMemberBuffer.PageIndex--;
                        AppManager.MenuMemberBuffer.MemberSelected = false;
                        Application.DoEvents();
                    }
                }
                if (pageTemp != AppManager.MenuMemberBuffer.PageIndex)
                {
                    CreateListMemberCardInPageIndex(true);
                }
            };
        }
        private void flpMember_SizeChanged(object sender, EventArgs e)
        {
            AppManager.MenuMemberBuffer.NumberShowMember = (flpMember.Height + 5) / 95;//85 + 10   
            AppManager.MenuMemberBuffer.MemberSelected = false;
            oddMember = ((flpMember.Height + 5) % 95) > 0.2;
            CreateListMemberCardInPageIndex(true);
        }
        Stopwatch sw = new Stopwatch();
        private void txtKeyword_TextChanged(object sender, EventArgs e)
        {
            if (!timerDelaySearchByKeyword.Enabled)
                timerDelaySearchByKeyword.Start();
            sw.Restart();
        }
        /****************************************************Các hàm lấy dữ liệu và tìm kiếm thành viên**********************************************************/
        public void InitialListMember(bool reload = false)
        {
            using (var memberHelper = new MemberHelper())
            {
                txtKeyword.Text = string.Empty;
                if (cmbGender.Items.Count > 0)
                {
                    cmbGender.SelectedIndex = 0;
                }
                if (cmbLiveOrDie.Items.Count > 0)
                {
                    cmbLiveOrDie.SelectedIndex = 0;
                }

                if ((AppManager.MenuMemberBuffer.ListAllMember.Count > 0 && reload) || (AppManager.MenuMemberBuffer.ListAllMember.Count == 0))
                {
                    AppManager.MenuMemberBuffer.ListAllMember.Clear();
                    AppManager.MenuMemberBuffer.ListAllMember = memberHelper.FindExTMemberOutDictionary("", "", "");
                }
                //if ((AppManager.MenuMemberBuffer.ListMember.Count > 0 && reload) || (AppManager.MenuMemberBuffer.ListMember.Count == 0))
                //{
                //    AppManager.MenuMemberBuffer.ListMember.Clear();
                //    AppManager.MenuMemberBuffer.ListMember = AppManager.MenuMemberBuffer.ListAllMember;
                //}
                flpPage.Left = (panelfilter.Width - flpPage.Width) / 2;
                var tblMFamilyInfo = AppManager.DBManager.GetTable<MFamilyInfo>();
                if (tblMFamilyInfo != null)
                {
                    var rootMember = tblMFamilyInfo.FirstOrDefault(i => i.Id == AppManager.LoginUser.FamilyId);
                    if (rootMember != null)
                    {
                        AppManager.MenuMemberBuffer.RootID = rootMember.RootId;
                    }
                }
                initComplete = true;
            }
        }

        public MemberCardForMenuMember[] RenderUserMember(List<ExTMember> ListMember)
        {
            using (var memberHelper = new MemberHelper())
            {
                if (ListMember.Count == 0) return new MemberCardForMenuMember[0];
                List<MemberCardForMenuMember> ListUserMember = new List<MemberCardForMenuMember>();
                MemberCardForMenuMember[] arrUserMember = new MemberCardForMenuMember[ListMember.Count];
                var lstFamilyHead = memberHelper.ListFamilyHead;
                int cnt = 0;
                ListMember.ForEach(member =>
                  {
                      MemberCardForMenuMember um = new MemberCardForMenuMember(member, AppManager.MenuMemberBuffer.RootID, lstFamilyHead.Contains(member.Id));
                      um.MemberMouseClicked += UserMemberMouseClick_Event;
                      um.RequestReload += (sender, mem) =>
                      {
                          if (Reload != null)
                          {
                              Reload?.Invoke(sender, memberHelper.TMemberToExTMember(mem));
                          }
                      };
                      arrUserMember[cnt++] = um;
                  });
                return arrUserMember;
            }
        }

        private Dictionary<string, ExTMember> FindMember(string keyword = "", string sgender = "", string sliveOrDie = "", bool inClan = true)
        {
            keyword = string.IsNullOrEmpty(keyword) ? "" : keyword;
            sgender = string.IsNullOrEmpty(sgender) ? "" : sgender;
            sliveOrDie = string.IsNullOrEmpty(sliveOrDie) ? "" : sliveOrDie;

            int gender = -1;
            int liveOrDie = -1;
            gender = sgender.Equals(AppConst.Gender.Male) ? 0 : sgender.Equals(AppConst.Gender.Female) ? 1 : sgender.Equals(AppConst.Gender.Unknow) ? 2 : -1;
            liveOrDie = sliveOrDie.Equals(AppConst.Status.Alive) ? 1 : sliveOrDie.Equals(AppConst.Status.IsDeath) ? 0 : -1;
            var isDeath = liveOrDie == 0 ? true : false;

            var listMember = AppManager.MenuMemberBuffer.ListAllMember.Where(i => ((i.Value.Name ?? "").ToLower() == keyword.ToLower()
                                                                                   || (i.Value.Name ?? "").ToLower().Trim().Contains(keyword.ToLower().Trim())
                                                                                   || (i.Value.Name ?? "").ToLower().IndexOf(keyword.ToLower()) != -1
                                                                                   || (i.Value.Contact.Tel_1 ?? "").ToLower().Contains(keyword)
                                                                                   || (i.Value.Contact.Tel_2 ?? "").ToLower().Contains(keyword)
                                                                                   || (i.Value.Contact.Email_1 ?? "").ToLower().Contains(keyword)
                                                                                   || (i.Value.Contact.Email_2 ?? "").ToLower().Contains(keyword)
                                                                                   || (i.Value.Contact.Address ?? "").ToLower().Contains(keyword)) &&
                                                                      (gender == -1 || i.Value.Gender == gender) &&
                                                                      (liveOrDie == -1 || i.Value.IsDeath == isDeath)).Select(x => x.Value).ToDictionary(x => x.Id);
            return listMember == null ? new Dictionary<string, ExTMember>() : listMember;
        }
        /********************************************************************************************************************************************************/
        /************************************************Khai báo biến và các hàm dùng trong phân trang**********************************************************/
        #region Khai báo, định nghĩa hàm dùng trong phân trang

        //làm mới lại danh sách thành viên theo tìm kiếm và chỉ số trang
        public void CreateListMemberCardInPageIndex(bool reload = false)
        {
            try
            {
                AppManager.MenuMemberBuffer.MemberSelected = false;
                if (!initComplete || AppManager.MenuMemberBuffer.NumberShowMember == 0 || AppManager.MenuMemberBuffer.ListMember.Count == 0)
                {
                    if (AppManager.MenuMemberBuffer.ListMember.Count == 0)
                    {
                        flpMember.Controls.Clear();
                    }
                    return;
                }
                if (AppManager.MenuMemberBuffer.PageIndex > 0)
                {
                    var member = AppManager.MenuMemberBuffer.TotalMember - (AppManager.MenuMemberBuffer.PageIndex - 1) * AppManager.MenuMemberBuffer.NumberShowMember;
                    member = member < 0 ? 0 : member;
                    int iOddMember = oddMember ? 1 : 0;
                    MemberCardForMenuMember[] listUserMember = RenderUserMember(AppManager.MenuMemberBuffer.ListMember.Values.ToList().GetRange((AppManager.MenuMemberBuffer.PageIndex - 1) * AppManager.MenuMemberBuffer.NumberShowMember, member >= AppManager.MenuMemberBuffer.NumberShowMember + iOddMember ? AppManager.MenuMemberBuffer.NumberShowMember + iOddMember : member));
                    flpMember.SuspendLayout();
                    flpMember.Controls.Clear();
                    flpMember.Controls.AddRange(listUserMember);
                    flpMember.BackgroundImage = null;
                    flpMember.ResumeLayout();
                }
            }
            catch { }
        }
        /********************************************************************************************************************************************************/
        /*******************************************************************Các hàm sự kiện**********************************************************************/
        private void UserMemberMouseClick_Event(object sender, ExTMember member)
        {
            var um = sender as MemberCardForMenuMember;
            if (sender == null || flpMember.Controls.Count == 0)
            {
                return;
            }
            foreach (MemberCardForMenuMember c in flpMember.Controls)
            {
                c.IsSelected = c.MemberInfo.Id == member.Id;
            }
            //AppManager.MenuMemberBuffer.MemberCurrent = member;
            AppManager.MenuMemberBuffer.FrameSelectedLocation = um.Location;
            AppManager.MenuMemberBuffer.MemberSelected = true;
            if (MenuMemberMouseClick != null)
            {
                this.BeginInvoke(MenuMemberMouseClick, new object[] { sender, member });
            }
        }

        private void CreateListMemberCardInPageIndex_Event(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int pageIndex = 1;
            int.TryParse(btn.Text, out pageIndex);
            AppManager.MenuMemberBuffer.PageIndex = Math.Abs(pageIndex);
            CreateListMemberCardInPageIndex(true);
            AppManager.MenuMemberBuffer.MemberSelected = false;
        }

        private void ChangePage_Event(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn == btnnextpage)
            {
                if (AppManager.MenuMemberBuffer.Page < AppManager.MenuMemberBuffer.MaxPages)
                    AppManager.MenuMemberBuffer.Page++;
            }
            else if (btn == btnprepage)
            {
                if (AppManager.MenuMemberBuffer.Page > 0)
                    AppManager.MenuMemberBuffer.Page--;
            }
            AppManager.MenuMemberBuffer.PageIndex = (AppManager.MenuMemberBuffer.Page - 1) * 4 + 1;
            CreateListMemberCardInPageIndex(true);
        }
        bool bFirstEvent = true;
        private void FilterMember_Event(object sender, EventArgs e)
        {
            if (AppManager.MenuMemberBuffer.ListAllMember.Count == 0) return;
            string tuKhoa = txtKeyword.Text;
            string gender = cmbGender.Text;
            string trangThai = cmbLiveOrDie.Text;

            AppManager.MenuMemberBuffer.ListMember.Clear();
            AppManager.MenuMemberBuffer.ListMember = FindMember(tuKhoa, gender, trangThai);
            AppManager.MenuMemberBuffer.MemberSelected = false;

            CreateListMemberCardInPageIndex(true);
            bFirstEvent = false;
        }

        private void timerDelayFilter_Tick(object sender, EventArgs e)
        {
            if (sw.ElapsedMilliseconds > 500)
            {
                FilterMember_Event(sender, e);
                sw.Reset();
                sw.Stop();
                timerDelaySearchByKeyword.Stop();
            }
        }

        private void MenuMember_Load(object sender, EventArgs e)
        {
            LoadConfig();
        }

        private void btnprepage_MouseHover(object sender, EventArgs e)
        {
            btnprepage.BackgroundImage = Resources.back_page2;
        }

        private void btnprepage_MouseLeave(object sender, EventArgs e)
        {
            btnprepage.BackgroundImage = Resources.back_page;
        }

        private void btnnextpage_MouseHover(object sender, EventArgs e)
        {
            btnnextpage.BackgroundImage = Resources.next_page2;
        }

        private void btnnextpage_MouseLeave(object sender, EventArgs e)
        {
            btnnextpage.BackgroundImage = Resources.next_page;
        }

        private void panelfilter_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnpage1_ForeColorChanged(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            if (btn.ForeColor == Color.Red)
            {
                btn.BackgroundImage = Resources.focus;
            }
            else
            {
                btn.BackgroundImage = null;
            }
        }

        private void lbltotalmember_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion
        /********************************************************************************************************************************************************/
    }

    public class InfoFilter
    {
        private string _Gender = "";
        private string _LiveorDie = "";
        private string _KeyWord = "";
        public int Page { get; set; }
        public Button ButtonSelect { get; set; }
        public string Gender { get => _Gender; set => _Gender = value; }
        public string LiveorDie { get => _LiveorDie; set => _LiveorDie = value; }
        public string KeyWord { get => _KeyWord; set => _KeyWord = value; }
    }
}