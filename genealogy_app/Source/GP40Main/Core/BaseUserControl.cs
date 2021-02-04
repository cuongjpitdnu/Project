using GP40Main.Services.Navigation;
using GP40Main.Utility;
using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using static GP40Main.Core.AppConst;

namespace GP40Main.Core
{
    /// <summary>
    /// Meno        : Page Base
    /// Create by   : AKB Nguyen Thanh Tung
    /// </summary>
    public partial class BaseUserControl : UserControl
    {

        #region Design Property

        [Browsable(true), Category("Theme Setting")]
        public string TitleBar { get; set; }

        [Browsable(true), Category("Theme Setting")]
        public FormStartPosition StartPosition { get; set; } = FormStartPosition.CenterScreen;

        [Browsable(true), Category("Theme Setting")]
        public FormWindowState WindowState { get; set; } = FormWindowState.Normal;

        [Browsable(true), Category("Theme Setting")]
        public bool MinimizeBox { get; set; } = true;

        [Browsable(true), Category("Theme Setting")]
        public bool MaximizeBox { get; set; } = true;

        [Browsable(true), Category("Theme Setting")]
        public bool Sizable { get; set; } = true;

        [Browsable(true), Category("Theme Setting")]
        public string ComfirmMsgExit { get; set; } = string.Empty;

        [Browsable(true), Category("Theme Setting"), DefaultValue(null)]
        public IButtonControl AcceptButton { get; set; }

        [Browsable(true), Category("Theme Setting"), DefaultValue(null)]
        public IButtonControl CancelButton { get; set; }

        #endregion Design Property

        private NavigationParameters Params { get; set; }
        public ModeForm Mode { get; set; }

        public BaseUserControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);
        }

        public BaseUserControl(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);
        }

        public BaseUserControl(NavigationParameters parameters = null, ModeForm mode = ModeForm.None)
        {
            this.Params = parameters;
            this.Mode = mode;

            InitializeComponent();
            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);
        }

        public void Close(NavigationResult result = null)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action(() => this.Close(result)));
            }
            else
            {
                var parent = this.ParentForm as BaseForm;

                if (parent != null)
                {
                    parent.ResultData = result;
                    parent.ComfirmWhenExit = result == null;
                    parent.Close();
                }
            }
        }

        public void Close<T>(T result)
        {
            this.Close(new NavigationResult(result));
        }

        public void Close(DialogResult dialogResult)
        {
            this.Close(new NavigationResult() { Result = dialogResult });
        }

        public NavigationParameters GetParameters()
        {
            //var parent = this.ParentForm as BaseForm;
            //return parent?.Params ?? new NavigationParameters();
            return this.Params ?? new NavigationParameters();
        }

        public ModeForm GetMode()
        {
            //var parent = this.ParentForm as BaseForm;
            //return parent?.Mode ?? ModeForm.None;
            return this.Mode;
        }

        public void BindingTableToCombo<T>(ComboBox cbo, string DisplayMember, string ValueMember = "Id") where T : BaseModel
        {
            if (cbo == null || string.IsNullOrEmpty(DisplayMember) || string.IsNullOrEmpty(ValueMember))
            {
                return;
            }

            using (var tblData = AppManager.DBManager.GetTable<T>())
            {
                var data = tblData.AsEnumerable().ToList();
                BindingHelper.Combobox(cbo, data, DisplayMember, ValueMember);
            }
        }
    }
}
