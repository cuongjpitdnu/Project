﻿using GP40DrawTree;
using GP40Main.Themes.Controls;
using SkiaSharp.Views.Desktop;
using System;
using System.Drawing;
using System.IO;
using static GP40Main.Core.AppConst;

namespace GP40Main.Views.MenuItems
{
    public partial class FrameItem : ItemBase
    {
        public FrameItem(DrawTreeConfig config = null) : base(config)
        {
            InitializeComponent();

            cbBorderColor.ColorControl.onColorChanged += (e) => ChangeData_Event();
            cbframe.SelectedIndexChanged += (sender, e) => ChangeData_Event();
            btnnext.Click += (sender, e) => ChangeData_Event();
            btnpre.Click += (sender, e) => ChangeData_Event();
        }

        protected override void SetConfig()
        {
            base.SetConfig();
            Config.BorderColor = ColorDrawHelper.FromColor(cbBorderColor.Color);
            Config.NumberFrame = numFrame - 1;
        }

        protected override void SetDisplayUI()
        {
            base.SetDisplayUI();

            cbBorderColor.Color = Config.BorderColor.ToDrawingColor();

            if (Directory.Exists(FolderFrameCardPath))
            {
                string[] allFrame = Directory.GetFiles(FolderFrameCardPath);
                foreach (string path in allFrame)
                {
                    string filename = GetFileName(path);
                    if (!string.IsNullOrEmpty(filename))
                        cbframe.Items.Add(filename);
                }
                if (cbframe.Items.Count > 0)
                {
                    if (Config.NumberFrame < cbframe.Items.Count)
                    {
                        cbframe.SelectedIndex = Config.NumberFrame;
                    }
                    else
                    {
                        cbframe.SelectedIndex = 0;
                    }
                }
            }
        }

        private int numFrame = 0;

        private string GetFileName(string path)
        {
            string str = path.Replace(@"\", "/");
            string[] temp = str.Split('/');
            if (temp.Length > 0)
                return temp[temp.Length - 1].Split('.')[0];
            else
                return null;
        }

        private void cbframe_ValueMemberChanged(object sender, EventArgs e)
        {
        }

        private void btnnext_Click(object sender, EventArgs e)
        {
            if (cbframe.SelectedIndex < cbframe.Items.Count - 1)
            {
                cbframe.SelectedIndex++;
                btnpre.Visible = true;
                int.TryParse(cbframe.Text, out numFrame);
            }
        }

        private void btnpre_Click(object sender, EventArgs e)
        {
            if (cbframe.SelectedIndex > 0)
            {
                cbframe.SelectedIndex--;
                btnnext.Visible = true;
                int.TryParse(cbframe.Text, out numFrame);
            }
        }

        private void cbframe_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (File.Exists(FolderFrameCardPath + cbframe.Text + ".png"))
                picframe.Image = Image.FromFile(FolderFrameCardPath + cbframe.Text + ".png");
            if (cbframe.SelectedIndex == 0)
            {
                btnpre.Visible = false;
                btnnext.Visible = true;
            }
            else if (cbframe.SelectedIndex == cbframe.Items.Count - 1)
            {
                btnnext.Visible = false;
                btnpre.Visible = true;
            }
            else
            {
                btnpre.Visible = true;
                btnnext.Visible = true;
            }
            int.TryParse(cbframe.Text, out numFrame);
        }
    }
}