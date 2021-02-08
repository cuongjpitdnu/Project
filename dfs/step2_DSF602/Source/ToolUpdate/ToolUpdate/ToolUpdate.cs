using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace ToolUpdate
{
    public partial class frmUpdate : Form
    {
        public const string FILE_ZIP_NAME = "NewVersion.zip";
        public const string FOLDER_BACKUP = "Version";
        public const string FOLDER_EXTRART_FILE = "NewVersion";
        public const string FILE_SETUP_APP = "DSF602.exe";
        public const string FILE_SETUP_UPDATE = "ToolUpdate.exe";

        public string zipPath = Application.StartupPath + @"\" + FILE_ZIP_NAME;
        public string extrartPath = Application.StartupPath + @"\" + FOLDER_EXTRART_FILE + "_" + DateTime.Now.ToString("ddMM");
        public string backupPath = Application.StartupPath + @"\Backup\" + FOLDER_BACKUP + "_" + FileVersionInfo.GetVersionInfo(Application.StartupPath + @"\" + FILE_SETUP_APP).FileVersion;

        public frmUpdate()
        {
            InitializeComponent();
        }

        public frmUpdate(string dataMsg)
        {
            InitializeComponent();

            try
            {
                if (dataMsg == "UPDATE")
                {
                    lblInfo.Text = "Please wait while the program is updating...";
                    BackupFile();
                    ProcessUpdate().Start();
                }
                else
                {
                    lblInfo.Text = "Please wait while the program is restoring...";
                    ProcessRestore(dataMsg).Start();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        private Thread ProcessUpdate()
        {
            return new Thread(() =>
            {
                Thread.Sleep(3000);

                try
                {
                    if (File.Exists(zipPath))
                    {
                        Directory.CreateDirectory(extrartPath);
                        ZipFile.ExtractToDirectory(zipPath, extrartPath);
                    }

                    try
                    {

                        UpdateVersion();
                    }
                    catch (Exception ex)
                    {
                        BackupVersion();
                        throw ex;
                    }


                    File.Delete(zipPath);
                    Directory.Delete(extrartPath, true);

                    this.Hide();

                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = Application.StartupPath + @"\" + FILE_SETUP_APP;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        Environment.Exit(0);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            });
        }

        private Thread ProcessRestore(string versionNameFolder)
        {
            return new Thread(() =>
            {
                Thread.Sleep(3000);

                try
                {
                    try
                    {
                        RestoreVersion(versionNameFolder);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                    using (Process myProcess = new Process())
                    {
                        myProcess.StartInfo.UseShellExecute = false;
                        myProcess.StartInfo.FileName = Application.StartupPath + @"\" + FILE_SETUP_APP;
                        myProcess.StartInfo.CreateNoWindow = true;
                        myProcess.Start();
                        Environment.Exit(0);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            });
        }

        private void BackupFile()
        {
            try
            {
                DirectoryCopy(Application.StartupPath, backupPath, true);
                File.Delete(backupPath + @"\" + FILE_ZIP_NAME);
                File.Delete(backupPath + @"\" + FILE_SETUP_UPDATE);

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void UpdateVersion()
        {
            try
            {
                DirectoryCopy(extrartPath, Application.StartupPath, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void BackupVersion()
        {
            try
            {
                DirectoryCopy(extrartPath, Application.StartupPath, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private void RestoreVersion(string versionNameFolder)
        {
            var pathFolderRestore = Application.StartupPath + @"\Backup\" + versionNameFolder;
            try
            {
                DirectoryCopy(pathFolderRestore, Application.StartupPath, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);
            try
            {
                if (!dir.Exists)
                {
                    throw new DirectoryNotFoundException(
                        "Source directory does not exist or could not be found: "
                        + sourceDirName);
                }

                DirectoryInfo[] dirs = dir.GetDirectories();
                // If the destination directory doesn't exist, create it.
                if (!Directory.Exists(destDirName))
                {
                    Directory.CreateDirectory(destDirName);
                }

                // Get the files in the directory and copy them to the new location.
                FileInfo[] files = dir.GetFiles();
                var totalFile = files.Count();
                var index = 0;
                foreach (FileInfo file in files)
                {
                    if (file.Name == "Newtonsoft.Json.dll")
                    {
                        continue;
                    }
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, true);

                    progressBar1.Value = Convert.ToInt32((index * 100) / totalFile);
                }

                //If copying subdirectories, copy them and their contents to new location.
                if (copySubDirs)
                {
                    foreach (DirectoryInfo subdir in dirs)
                    {
                        if (subdir.Name == "Backup")
                        {
                            continue;
                        }
                        if (subdir.Name == "Data")
                        {
                            continue;
                        }
                        string temppath = Path.Combine(destDirName, subdir.Name);
                        DirectoryCopy(subdir.FullName, temppath, copySubDirs);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
