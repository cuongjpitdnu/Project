using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;

namespace Updater
{
    class Program
    {

        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;

        static void Main(string[] args)
        {
            var handle = GetConsoleWindow();

            // Hide
            ShowWindow(handle, SW_HIDE);
            StopProcess("GPMain");

            var path = AppDomain.CurrentDomain.BaseDirectory;
            var pathUnZip = path + @"\TempUpdate\UnZip\";
            var pathApp = path + @"\GPMain.exe";
            var pathBackup = path + @"\\BackupApp\\";

            try
            {
                if(args[0] == "Restore")
                {
                    copyFolder(pathBackup, path);
                }
                else
                {
                    CreateFolder(pathBackup,false);
                    copyBackup(pathUnZip, path, pathBackup);
                    copyFolder(pathUnZip, path);

                }
            }
            catch (Exception)
            {
            }
            
            if (File.Exists(pathApp))
            {
                Process.Start(pathApp);
            }
        }

        static void copyFolder(string sourcePath, string targetPath)
        {
            if (Directory.Exists(sourcePath))
            {
                var arrFiles = Directory.GetFiles(sourcePath);
                var arrDirectories = Directory.GetDirectories(sourcePath);

                foreach (var file in arrFiles)
                {
                    File.Copy(file, Path.Combine(targetPath, Path.GetFileName(file)), true);
                }

                foreach (var directory in arrDirectories)
                {
                    var folderTemp = Path.Combine(targetPath, Path.GetFileName(directory));

                    if (!Directory.Exists(folderTemp))
                    {
                        Directory.CreateDirectory(folderTemp);
                    }

                    copyFolder(directory, folderTemp);
                }
            }
        }

        static void copyBackup(string pathUnZip, string sourcePath, string targetPath)
        {
            var arrFiles = Directory.GetFiles(sourcePath);
            var arrDirectories = Directory.GetDirectories(sourcePath);


            if (Directory.Exists(pathUnZip))
            {
                var arrFilesUnZip = Directory.GetFiles(pathUnZip);
                var arrDirectoriesUnZip = Directory.GetDirectories(pathUnZip);

                foreach (var fileUnZip in arrFilesUnZip)
                {
                    foreach (var fileSrc in arrFiles)
                    {
                        if (Path.GetFileName(fileSrc) != Path.GetFileName(fileUnZip))
                        {
                            continue;
                        }
                        File.Copy(fileSrc, Path.Combine(targetPath, Path.GetFileName(fileSrc)), true);
                    }
                }

                foreach (var directoryUnZip in arrDirectoriesUnZip)
                {
                    foreach (var directorySrc in arrDirectories)
                    {
                        if (Path.GetFileName(directorySrc) != Path.GetFileName(directoryUnZip))
                        {
                            continue;
                        }
                        var folderTemp = Path.Combine(targetPath, Path.GetFileName(directorySrc));

                        if (!Directory.Exists(folderTemp))
                        {
                            Directory.CreateDirectory(folderTemp);
                        }

                        copyBackup(directoryUnZip, directorySrc, folderTemp);
                    }
                }
            }
        }

        static void StopProcess(string preccessName = "")
        {
            try
            {
                var procs = Process.GetProcesses();
                if (procs.Length < 1) return;
                foreach (var process in procs)
                {
                    if ((process.ProcessName + "").ToLower().StartsWith(preccessName))
                    {
                        process.Kill();
                    }
                }

            }
            catch (Exception) { }
        }

        private static void CreateFolder(string path,bool blnIsHidden)
        {
            fncDeleteFolder(path);
            fncCreateFolder(path, blnIsHidden);
        }
        private static bool fncDeleteFolder(string strFolderPath)
        {
            try
            {
                if (System.IO.Directory.Exists(strFolderPath))
                {
                    DirectoryInfo objDirInfo = new DirectoryInfo(strFolderPath);

                    // reset attribute before deleting
                    xSetFolderAttr(objDirInfo, FileAttributes.Normal);
                    objDirInfo.Delete(true);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
            }
        }
        private static bool xSetFolderAttr(System.IO.DirectoryInfo objDir, FileAttributes emAttr)
        {
            try
            {
                if (objDir.Exists)
                {
                    // set this folder's attribute
                    objDir.Attributes = FileAttributes.Normal;

                    // set file's attribute
                    foreach (FileInfo objFile in objDir.GetFiles())
                        objFile.Attributes = FileAttributes.Normal;

                    // set folder's attribute
                    foreach (DirectoryInfo objFolder in objDir.GetDirectories())
                        xSetFolderAttr(objFolder, emAttr);
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw ex;
            }
            finally
            {
            }
        }
        private static bool fncCreateFolder(string strFolderPath, bool blnIsHidden = true)
        {
            System.IO.DirectoryInfo objDirInfo = null;

            try
            {
                objDirInfo = new System.IO.DirectoryInfo(strFolderPath);

                // check existance of temp folder
                if (!objDirInfo.Exists)
                {
                    // create folder
                    objDirInfo.Create();

                    // set hidden
                    if (blnIsHidden)
                        objDirInfo.Attributes = System.IO.FileAttributes.Hidden;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                objDirInfo = null;
            }
        }
    }
}
