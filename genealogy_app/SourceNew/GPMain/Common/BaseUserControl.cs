using GPMain.Common.Helper;
using GPMain.Common.Navigation;
using GPModels;
using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace GPMain.Common
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

        protected NavigationParameters Params { get; set; }
        protected ModeForm Mode { get; set; }

        public BaseUserControl()
        {
        }

        public BaseUserControl(IContainer container)
        {
            container.Add(this);
        }

        public BaseUserControl(NavigationParameters parameters = null, ModeForm mode = ModeForm.None)
        {
            this.Params = parameters ?? new NavigationParameters();
            this.Mode = mode;

            SetStyle(ControlStyles.DoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque | ControlStyles.SupportsTransparentBackColor, false);

            this.Load += (sender, e) => AppManager.MinimizeFootprint();
        }

        public virtual void ProcessAffterAddForm()
        {

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
                    try
                    {
                        parent.ResultData = result;
                        parent.ComfirmWhenExit = result == null;
                        parent.Close();
                    }
                    catch
                    {

                    }
                }
            }
        }

        public void Close<T>(T result)
        {
            this.Close(new NavigationResult(result) { Result = DialogResult.OK });
        }

        public void Close(DialogResult dialogResult)
        {
            this.Close(new NavigationResult() { Result = dialogResult });
        }

        public void BindingTableToCombo<T>(ComboBox cbo, string DisplayMember, string ValueMember = "Id") where T : BaseModel
        {
            if (cbo == null || string.IsNullOrEmpty(DisplayMember) || string.IsNullOrEmpty(ValueMember))
            {
                return;
            }

            using (var tblData = AppManager.DBManager.GetTable<T>())
            {
                var data = tblData.ToList();
                BindingHelper.Combobox(cbo, data, DisplayMember, ValueMember);
            }
        }
    }
}
