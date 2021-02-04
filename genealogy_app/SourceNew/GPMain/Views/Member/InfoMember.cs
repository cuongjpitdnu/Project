using GPMain.Common;
using GPModels;
using MaterialSkin;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace GPMain.Views.Member
{
    public partial class InfoMember : BaseUserControl
    {
        private static MaterialSkinManager materialSkinmanager = MaterialSkinManager.Instance;

        public InfoMember()
        {
            InitializeComponent();
            this.BackColor = Color.White;

            foreach (Control ctl in Controls)
            {
                if (ctl is Label)
                {
                    ((Label)ctl).Font = materialSkinmanager.getFontByType(MaterialSkinManager.fontType.Subtitle1);
                }
            }
            lblhoten.Font = materialSkinmanager.getFontByType(MaterialSkinManager.fontType.H4);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RectangleF cardRectF = new RectangleF(ClientRectangle.Location, ClientRectangle.Size);
            cardRectF.X -= 0.5f;
            cardRectF.Y -= 0.5f;
            GraphicsPath cardPath = CreateRoundRect(cardRectF, 6);
            this.Region = new Region(cardPath);
            base.OnPaint(e);
        }

        public void Info(ExTMember member)
        {
            lblthehe.Text = member.ChildLevelInFamily.ToString();
            lblhoten.Text = member.Name + (member.IsDeath ? " ( ĐÃ MẤT )" : "");

            //var queryTypeName = AppManager.DBManager.GetTable<MTypeName>().CreateQuery();
            //var objTypeName = queryTypeName.Where(i => i.TypeName == member.TypeName.First().Key).FirstOrDefault();

            if (member.TypeName.Count > 0)
            {
                lbltenthuonggoi.Text = member.TypeName.First().Value ?? "";
                if (member.TypeName.Count > 1)
                    lblphapdanh.Text = member.TypeName.Last().Value ?? "";
                else
                    lblphapdanh.Text = "";
            }
            else
            {
                lbltenthuonggoi.Text = lblphapdanh.Text = "";
            }

            lblgioitinh.Text = (member.Gender == 1) ? "Nam" : (member.Gender == 2) ? "Nữ" : "Chưa rõ";
            lblngaysinh.Text = member.BirthdayShow + "  ( Âm lịch: " + member.BirthdayLunarShow + " )";
            lblnoisinh.Text = member.BirthPlace;
            lblngaymat.Text = member.DeadDaySunShow + "  ( Âm lịch: " + member.DeadDayLunarShow + " )";
            lblnoimat.Text = member.DeadPlace;

            if (!string.IsNullOrEmpty(member.Religion))
            {
                var queryRelation = AppManager.DBManager.GetTable<MReligion>().CreateQuery();
                var objRelation = queryRelation.Where(i => i.Id == member.Religion).FirstOrDefault();
                lbltongiao.Text = objRelation.RelName;
            }
            if (!string.IsNullOrEmpty(member.National))
            {
                var queryNational = AppManager.DBManager.GetTable<MNationality>().CreateQuery();
                var objNationalDefault = queryNational.Where(i => i.Id == member.National).FirstOrDefault();
                lblquoctich.Text = objNationalDefault.NatName;
            }
            lblhomtown.Text = member.HomeTown;
            lbltitlengaymat.Visible = lbltitlenoimat.Visible = lblngaymat.Visible = lblnoimat.Visible = member.IsDeath;
            this.Size = new Size(this.Size.Width, member.IsDeath ? 350 : 280);
        }

        public static GraphicsPath CreateRoundRect(float x, float y, float width, float height, float radius)
        {
            var gp = new GraphicsPath();
            gp.AddArc(x + width - (radius * 2), y, radius * 2, radius * 2, 270, 90);
            gp.AddArc(x + width - (radius * 2), y + height - (radius * 2), radius * 2, radius * 2, 0, 90);
            gp.AddArc(x, y + height - (radius * 2), radius * 2, radius * 2, 90, 90);
            gp.AddArc(x, y, radius * 2, radius * 2, 180, 90);
            gp.CloseFigure();
            return gp;
        }

        public static GraphicsPath CreateRoundRect(RectangleF rect, float radius)
        {
            return CreateRoundRect(rect.X, rect.Y, rect.Width, rect.Height, radius);
        }
    }
}