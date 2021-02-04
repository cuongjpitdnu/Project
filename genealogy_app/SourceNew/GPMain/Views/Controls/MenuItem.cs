using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using MaterialSkin.Controls;
using System.Threading.Tasks;

namespace GPMain.Views.Controls
{
    public partial class MenuItem : UserControl
    {
        public MenuItem()
        {
            InitializeComponent();
        }

        int NumItem = 0;

        public void Add<T>(string Title, T Content) where T : ItemBase
        {
            if (string.IsNullOrEmpty(Title) || Content == null)
            {
                return;
            }

            Content.Width = this.Width;

            KeyValuePair<string, ItemBase> control = new KeyValuePair<string, ItemBase>(Title, Content);
            ItemHeader item = new ItemHeader(NumItem, control);
            this.Controls.Add(item);
            item.Tag = NumItem;
            item.Click += new EventHandler(this.OnClick);
            item.BackColor = this.BackColor;
            item.Dock = DockStyle.Top;
            item.Show();
            NumItem++;
        }

        public void AddRange(Dictionary<string, ItemBase> ListControl)
        {
            ItemHeader[] items = new ItemHeader[ListControl.Count];
            foreach (var control in ListControl)
            {
                this.Invoke(new Action(() =>
                {
                    ItemHeader item = new ItemHeader(NumItem, control);
                    item.Tag = NumItem;
                    item.Click += new EventHandler(this.OnClick);
                    item.BackColor = this.BackColor;
                    item.Dock = DockStyle.Top;
                    items[NumItem] = item;
                    NumItem++;
                }));
            };
            this.SuspendLayout();
            this.Controls.AddRange(items);
            this.ResumeLayout();
        }

        private void OnClick(object sender, EventArgs e)
        {
            bool MenuClick = false;
            int ID = 0;
            if (sender is Panel)
            {
                Panel pnl = sender as Panel;
                if (pnl.Name.Equals("Header"))
                {
                    MenuClick = true;
                    ID = (int)pnl.Tag;
                }
            }
            else if (sender is Button)
            {
                Button btn = sender as Button;
                MenuClick = true;
                ID = (int)btn.Tag;
            }
            else if (sender is MaterialLabel)
            {
                MaterialLabel lbl = sender as MaterialLabel;
                MenuClick = true;
                ID = (int)lbl.Tag;
            }
            if (MenuClick)
            {
                foreach (Control ctl in this.Controls)
                {
                    ItemHeader uc = ctl as ItemHeader;
                    if ((int)uc.Tag == ID)
                    {
                        uc.ShowContent();
                    }
                    else
                    {
                        uc.HideContent();
                    }
                }
            }
        }
    }

    public class ItemHeader : UserControl
    {
        public int ID;

        Image imgArrowDown;
        Image imgArrowRight;
        ImageList imgLst = new ImageList();
        FontFamily[] Fonts;

        Image Base64ToImage(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }
        Button btnHeader = new Button();
        int heightControl = 0;
        public ItemHeader(int idMenu, KeyValuePair<string, ItemBase> control)
        {
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);
            this.SuspendLayout();
            ID = idMenu;
            heightControl = control.Value.Height;
            this.Height = 24;
            this.Width = control.Value.Width;
            this.BackColor = Color.SeaShell;

            Fonts = FontFamily.Families;

            imgArrowDown = Base64ToImage("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAAHQAAAB0ATGF5YEAAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAv0lEQVQ4T52SywrCMBBFZ+neTX3go74QpIgg1Aqa4q/5lyIu+gF+QuRGUiZ1MrQuziIzc84qtC+uR2st/UN2NjmN0vVjcyju0oEGHLhkjOkN5qtXlwhu4cB1gy4RLuNdL9pEmjIIDrSIJIPgCEiRmAwomS6q7FTe+JBHYjKcZJJW7h/0h+O3GJktn0CS4cB1g1hEgst414s2kaYMggMtIskgOAJSJCaD4OHhEU0GPwOPj2gyEIeeXX7ZAmn3xdIHat1IGO8AbCoAAAAASUVORK5CYII=");
            imgArrowRight = Base64ToImage("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAAAHQAAAB0ATGF5YEAAAAZdEVYdFNvZnR3YXJlAHd3dy5pbmtzY2FwZS5vcmeb7jwaAAAAtklEQVQ4T2NgcLU2YdBUvc3g4MDx//9/BmTMoKr0mMHN0RhdHBlDCDuLFgZ15fvohjC42jozyMm8x2cIgoHPEHmZdwwuLobI4nB5FA4ZhqBwwAKEDPFyMEARR+bABfEb8hbZELgkOiZoiIezHpiPLImOwYZoqt7BEAcbIvuWwclJHkUCHVNkAEVeoCgQCWjGH40ENeNLSORoBsuDCXyaCWUmBg97SwZ1lTvomsGSyvJP8Wfn/wwAxqw8EJtQZ6UAAAAASUVORK5CYII=");

            imgLst.Images.Add(imgArrowDown);
            imgLst.Images.Add(imgArrowRight);

            Panel header = new Panel();
            header.SuspendLayout();
            header.Click += OnClick;
            header.Name = "Header";
            header.Height = 24;
            header.Cursor = Cursors.Hand;

            btnHeader.Click += OnClick;
            btnHeader.FlatStyle = FlatStyle.Flat;
            btnHeader.FlatAppearance.BorderSize = 0;
            btnHeader.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnHeader.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnHeader.ImageList = imgLst;
            btnHeader.ImageIndex = 0;
            btnHeader.Size = new Size(24, 24);
            header.Controls.Add(btnHeader);
            btnHeader.Dock = DockStyle.Left;
            btnHeader.Show();

            MaterialLabel lbltitle = new MaterialLabel();
            lbltitle.Click += OnClick;
            lbltitle.Text = control.Key;
            lbltitle.FontType = MaterialSkin.MaterialSkinManager.fontType.Subtitle1;
            lbltitle.Location = new Point(35, 2);
            lbltitle.Size = new Size(200, lbltitle.Size.Height);
            header.Controls.Add(lbltitle);
            lbltitle.Show();
            header.ResumeLayout();

            this.Controls.Add(header);
            this.Controls.Add(control.Value);

            control.Value.Click += new System.EventHandler(this.OnClick);
            control.Value.Location = new Point(0, 24);
            control.Value.Show();
            header.Show();
            this.ResumeLayout();
            btnHeader.Tag = header.Tag = lbltitle.Tag = ID;
        }

        public void HideContent()
        {
            this.Height = 24;
            btnHeader.ImageIndex = 0;
        }

        public void ShowContent()
        {
            int mode = this.Height == 24 ? 1 : 0;
            this.Height = 24 + heightControl * mode;
            btnHeader.ImageIndex = mode;
        }

        [NonSerialized]
        private EventHandler fClick;
        public event EventHandler Click
        {
            add { fClick += value; }
            remove { fClick -= value; }
        }

        protected void OnClick(object sender, EventArgs e)
        {
            EventHandler handler = fClick;
            if (fClick != null)
                handler(sender, e);
        }

    }
}
