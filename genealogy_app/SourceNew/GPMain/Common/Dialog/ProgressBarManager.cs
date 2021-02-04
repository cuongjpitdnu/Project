using GPMain;
using GPMain.Common.Dialog;
using GPMain.Views;
using System;
using System.Threading;

namespace GPMain.Core
{
    public class ProgressBarManager : IDisposable
    {
        private static readonly object _sync = new object();

        public int count;
        public int total;
        public int mintPercent;
        public bool mblnComplete = false;
        public bool mblnErr = false;
        public Thread mobjProcessThread = null;
        public Thread mobjProgressBarThread = null;
        public string mblnComfirmClose = "Bạn có muốn dừng xử lý?";
        public WaitForm waitForm;
        private bool disposedValue;

        public int Percent
        {
            get
            {
                lock (_sync)
                {
                    return mintPercent;
                }
            }
            set
            {
                lock (_sync)
                {
                    mintPercent = value;

                    if (mintPercent >= waitForm.Maximum)
                    {
                        mintPercent = waitForm.Maximum;
                        this.CompleteProcess = true;
                    }
                }
            }
        }

        public bool CompleteProcess
        {
            get
            {
                return mblnComplete;
            }
            set
            {
                mblnComplete = value;
            }
        }

        public string ComfirmClose
        {
            get
            {
                return mblnComfirmClose;
            }
            set
            {
                mblnComfirmClose = value;
            }
        }

        public Thread ProcessThread
        {
            get
            {
                return mobjProcessThread;
            }
            set
            {
                mobjProcessThread = value;
            }
        }

        public bool ErrorProcess
        {
            get
            {
                return mblnErr;
            }
            set
            {
                mblnErr = value;
            }
        }

        public void fncCreateProgressBar(string strTitle = "")
        {
            waitForm = new WaitForm();
            waitForm.Maximum = 100;

            if (!string.IsNullOrWhiteSpace(strTitle))
            {
                waitForm.Title = strTitle;
            }

            mobjProgressBarThread = new Thread(xSetProcessBar);
        }

        public string Title
        {
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    waitForm.Title = value;
                }
            }
        }

        public void fncStartProgressBar(string titleBar = "", bool blnShowDialog = true)
        {
            try
            {
                mintPercent = 0;
                mblnComplete = false;
                mblnErr = false;


                using (var frmStart = new OnlyShowBarForm())
                {
                    //frmStart.ControlBox = false;
                    if (!string.IsNullOrEmpty(titleBar))
                    {
                        //frmStart.TitleBar = titleBar;
                        waitForm.TitleBar = titleBar;
                    }
                    frmStart.AddPage(waitForm);

                    mobjProgressBarThread.Start();

                    if (mobjProcessThread != null)
                    {
                        mobjProcessThread.Start();
                    }

                    if (blnShowDialog)
                    {
                        frmStart.ShowDialog();
                    }
                    else
                    {
                        frmStart.Show();
                    }
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ProgressBarManager), ex);
            }
        }

        public int fncCalculatePercent(int intCurrent, int intMax)
        {
            int percent = 0;

            try
            {
                count = intCurrent;
                total = intMax;
                percent = (int)Math.Ceiling(((decimal)intCurrent / intMax) * 100);

                if (percent > 100)
                {
                    percent = 100;
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ProgressBarManager), ex);
            }

            return percent;
        }

        public int fncCalculatePercent2(int intCurrent, int intMax)
        {
            int percent = 0;

            try
            {
                count = intCurrent;
                total = intMax;
                percent = (int)(((decimal)intCurrent / intMax) * 100);

                if (percent > 100)
                {
                    percent = 100;
                }
            }
            catch (Exception ex)
            {
                AppManager.Dialog.Error(ex.Message);
                AppManager.LoggerApp.Error(typeof(ProgressBarManager), ex);
            }

            return percent;
        }

        public void xSetProcessBar()
        {
            try
            {
                while (!mblnComplete)
                {
                    if (mblnErr)
                    {
                        return;
                    }
                    lock (_sync)
                    {
                        waitForm.fncUpdateProgressBar(mintPercent, count, total);
                    }
                }
                waitForm.fncUpdateProgressBar(waitForm.Maximum);
                waitForm.Close();
            }
            catch (Exception ex)
            {
                AppManager.LoggerApp.Error(typeof(ProgressBarManager), ex);
            }
        }

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
        // ~clsProgressBar()
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
    }
}