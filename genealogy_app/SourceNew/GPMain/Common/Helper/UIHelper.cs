using Microsoft.Win32;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GPMain.Common.Helper
{
    /// <summary>
    /// Meno        : Support UI/UX
    /// Create by   : 2020.07.28 AKB Nguyễn Thanh Tùng
    /// </summary>
    public static class UIHelper
    {

        #region General

        /// <summary>
        /// Meno    : Call Action Safe in Thread
        /// </summary>
        /// <param name="container"></param>
        /// <param name="action"></param>
        public static void SafeInvoke(this ContainerControl container, Action action)
        {
            if (action == null)
            {
                return;
            }

            if (container.InvokeRequired)
            {
                container.BeginInvoke(action);
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Check Mode Theme Windows 10
        /// </summary>
        /// <returns>true - Light/false - Dark</returns>
        public static bool IsLightMode()
        {
            try
            {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize"))
                {
                    if (key != null)
                    {
                        var appsUseLightTheme = key.GetValue("AppsUseLightTheme");
                        if (appsUseLightTheme != null && !appsUseLightTheme.Equals(1))
                        {
                            return false;
                        }
                    }
                }
            }
            catch
            {
            }

            return true;
        }

        #endregion General

        #region TextBox

        public static void FocusLast(this TextBoxBase textBox)
        {
            if (textBox == null) return;
            textBox.Focus();
            textBox.SelectionStart = textBox.Text.Length;
            textBox.SelectionLength = 0;
        }

        public static void FocusAndSelected(this TextBoxBase textBox)
        {
            if (textBox == null) return;
            textBox.Focus();
            textBox.SelectionStart = 0;
            textBox.SelectionLength = textBox.Text.Length;
        }

        #endregion TextBox

        #region DataGridView

        public static T GetSelectedGridRowData<T>(this DataGridView gridView) where T : class
        {
            if (gridView.CurrentCell == null || gridView.SelectedRows.Count < 1)
            {
                return null;
            }

            return gridView.SelectedRows[0].DataBoundItem as T;
        }

        public static void SetColumnEditAction<T>(this DataGridView gridView, Action<T> action) where T : class
        {
            if (gridView == null || action == null)
            {
                return;
            }

            var columnImageEdit = new DataGridViewImageColumn();
            var columnLinkEdit = new DataGridViewLinkColumn();

            columnImageEdit.HeaderText = "";
            columnImageEdit.Image = Properties.Resources.grid_icon_edit;
            columnImageEdit.Name = nameof(columnImageEdit);
            columnImageEdit.ReadOnly = true;
            columnImageEdit.Resizable = DataGridViewTriState.False;
            columnImageEdit.Width = 18;
            columnImageEdit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            columnImageEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            columnLinkEdit.HeaderText = "";
            columnLinkEdit.LinkColor = SystemColors.GrayText;
            columnLinkEdit.Name = nameof(columnLinkEdit);
            columnLinkEdit.ReadOnly = true;
            columnLinkEdit.Resizable = DataGridViewTriState.False;
            columnLinkEdit.SortMode = DataGridViewColumnSortMode.NotSortable;
            columnLinkEdit.Text = "Sửa";
            columnLinkEdit.UseColumnTextForLinkValue = true;
            columnLinkEdit.Width = 35;
            columnLinkEdit.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            columnLinkEdit.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridView.Columns.AddRange(columnImageEdit, columnLinkEdit);
            gridView.CellContentClick += (sender, e) =>
            {
                if (e.RowIndex >= 0 && (e.ColumnIndex == columnImageEdit.Index || e.ColumnIndex == columnLinkEdit.Index))
                {
                    action(gridView.GetSelectedGridRowData<T>());
                }
            };
        }




        public static void SetColumnDownloadAction<T>(this DataGridView gridView, Action<T> action) where T : class
        {
            if (gridView == null || action == null)
            {
                return;
            }
            var columnImageDownload = new DataGridViewImageColumn();
            var columnLinkDownload = new DataGridViewLinkColumn();

            columnImageDownload.HeaderText = "";
            columnImageDownload.Image = Properties.Resources.download;
            columnImageDownload.Name = "columnImageDownload";
            columnImageDownload.ReadOnly = true;
            columnImageDownload.Resizable = DataGridViewTriState.True;
            columnImageDownload.Width = 18;

            columnLinkDownload.HeaderText = "";
            columnLinkDownload.LinkColor = SystemColors.GrayText;
            columnLinkDownload.Name = "columnLinkDownload";
            columnLinkDownload.ReadOnly = true;
            columnLinkDownload.Text = "Download";
            columnLinkDownload.UseColumnTextForLinkValue = true;
            columnLinkDownload.Width = 80;

            gridView.Columns.AddRange(columnImageDownload, columnLinkDownload);
            gridView.CellContentClick += (sender, e) =>
            {
                if (e.RowIndex >= 0 && (e.ColumnIndex == columnImageDownload.Index || e.ColumnIndex == columnLinkDownload.Index))
                {
                    var rowSelected = gridView.GetSelectedGridRowData<T>();

                    if (rowSelected != null)
                    {
                        action(rowSelected);
                    }
                }
            };
        }

        public static void SetColumnRestoreAction<T>(this DataGridView gridView, Action<T> action) where T : class
        {
            if (gridView == null || action == null)
            {
                return;
            }
            var columnImageRestore = new DataGridViewImageColumn();
            var columnLinkRestore = new DataGridViewLinkColumn();

            columnImageRestore.HeaderText = "";
            columnImageRestore.Image = Properties.Resources.restore;
            columnImageRestore.Name = "columnImageRestore";
            columnImageRestore.ReadOnly = true;
            columnImageRestore.Resizable = DataGridViewTriState.True;
            columnImageRestore.Width = 18;

            columnLinkRestore.HeaderText = "";
            columnLinkRestore.LinkColor = SystemColors.GrayText;
            columnLinkRestore.Name = "columnLinkRestore";
            columnLinkRestore.ReadOnly = true;
            columnLinkRestore.Text = "Khôi phục";
            columnLinkRestore.UseColumnTextForLinkValue = true;
            columnLinkRestore.Width = 80;

            gridView.Columns.AddRange(columnImageRestore, columnLinkRestore);
            gridView.CellContentClick += (sender, e) =>
            {
                if (e.RowIndex >= 0 && (e.ColumnIndex == columnImageRestore.Index || e.ColumnIndex == columnLinkRestore.Index))
                {
                    var rowSelected = gridView.GetSelectedGridRowData<T>();

                    if (rowSelected != null)
                    {
                        action(rowSelected);
                    }
                }
            };
        }

        public static void SetColumnDeleteAction<T>(this DataGridView gridView, Action<T> action) where T : class
        {
            if (gridView == null || action == null)
            {
                return;
            }
            var columnImageDelete = new DataGridViewImageColumn();
            var columnLinkDelete = new DataGridViewLinkColumn();

            columnImageDelete.HeaderText = "";
            columnImageDelete.Image = Properties.Resources.grid_icon_delete;
            columnImageDelete.Name = "ColActionNational";
            columnImageDelete.ReadOnly = true;
            columnImageDelete.Resizable = DataGridViewTriState.True;
            columnImageDelete.Width = 18;
            columnImageDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            columnImageDelete.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            columnLinkDelete.HeaderText = "";
            columnLinkDelete.LinkColor = SystemColors.GrayText;
            columnLinkDelete.Name = "clmLinkDelete";
            columnLinkDelete.ReadOnly = true;
            columnLinkDelete.Text = "Xóa";
            columnLinkDelete.UseColumnTextForLinkValue = true;
            columnLinkDelete.Width = 35;
            columnLinkDelete.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            columnLinkDelete.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;

            gridView.Columns.AddRange(columnImageDelete, columnLinkDelete);
            gridView.CellContentClick += (sender, e) =>
            {
                if (e.RowIndex >= 0 && (e.ColumnIndex == columnImageDelete.Index || e.ColumnIndex == columnLinkDelete.Index))
                {
                    var rowSelected = gridView.GetSelectedGridRowData<T>();

                    if (rowSelected != null)
                    {
                        action(rowSelected);
                    }
                }
            };
        }
        #endregion DataGridView

        /// <summary>
        /// Struct representing a point.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        private struct POINT
        {
            public int X;
            public int Y;

            public static implicit operator Point(POINT point)
            {
                return new Point(point.X, point.Y);
            }
        }

        /// <summary>
        /// Retrieves the cursor's position, in screen coordinates.
        /// </summary>
        /// <see>See MSDN documentation for further information.</see>
        [DllImport("user32.dll")]
        private static extern bool GetCursorPos(out POINT lpPoint);

        public static Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return new Point(lpPoint.X, lpPoint.Y);
        }
    }
}
