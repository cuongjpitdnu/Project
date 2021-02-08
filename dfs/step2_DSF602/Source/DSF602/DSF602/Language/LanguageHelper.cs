using Karambolo.PO;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Windows.Forms;

namespace DSF602.Language
{
    public class LanguageHelper
    {

        private const string DEFAULT_LANGUAGE = "en";

        public static Dictionary<string, string> POFileData { get; set; } = new Dictionary<string, string>();

        public static void SetValueOf(Control control, string key, int maxLength = 0)
        {
            var isTypeForm = control is Form;
            var valueText = GetValueOf(key);
            valueText = valueText.Trim();

            if (!isTypeForm && maxLength > 0 && valueText.Length > maxLength)
            {
                ToolTip ToolTip1 = new ToolTip();
                ToolTip1.SetToolTip(control, valueText);
                valueText = valueText.Substring(0, maxLength);
                valueText = (valueText.Length > 3 ? valueText.Substring(0, valueText.Length - 3) : "") + "...";
            }

            control.Text = valueText;
            control.Refresh();
        }

        public static void SetValueOf(DataGridViewColumn control, string key, int maxLength = 0)
        {
            var isTypeForm = control is DataGridViewColumn;
            var valueText = GetValueOf(key);
            valueText = valueText.Trim();

            if (!isTypeForm && maxLength > 0 && valueText.Length > maxLength)
            {
                valueText = valueText.Substring(0, maxLength);
                valueText = (valueText.Length > 3 ? valueText.Substring(0, valueText.Length - 3) : "") + "...";
            }
            
            control.HeaderText = valueText;
        }

        public static string GetValueOf(string key)
        {
            var objResourceManager = new ResourceManager("DSF602.Properties.Resources", Assembly.GetExecutingAssembly());
            var keyPOFile = objResourceManager.GetString(key/*, new CultureInfo(Application.CurrentCulture.Name)*/);
            if (string.IsNullOrEmpty(keyPOFile) || POFileData == null || POFileData.Count == 0 || !POFileData.ContainsKey(keyPOFile))
            {
                return keyPOFile;
            }

            return POFileData[keyPOFile];
        }

        public static void LoadDataPOFile()
        {
            POFileData.Clear();

            var pathPoFile = GetPathFilePo(DefaultLanguageValue);

            if (string.IsNullOrEmpty(pathPoFile) || !File.Exists(pathPoFile))
            {
                return;
            }
            
            using (var reader = new StreamReader(pathPoFile, Encoding.UTF8))
            {
                var parser = new POParser();
                var result = parser.Parse(reader);

                if (!result.Success)
                {
                    return;
                }

                var catalog = result.Catalog;
                //var languageName = catalog.Language.Replace('_', '-').Trim();
                var languageName = DefaultLanguageValue.Replace('_', '-');
                languageName = languageName.Replace(".po", "").Trim();
                CultureInfo ci = new CultureInfo(languageName);
                CultureInfo.DefaultThreadCurrentCulture = ci;

                foreach (var item in catalog)
                {
                    var keyLangue = item.Key.Id;
                    var key = new POKey(keyLangue);
                    var translation = catalog.GetTranslation(key);

                    POFileData.Add(keyLangue, translation);
                }
            }
        }

        public static void LoadConfigDefaultLanguage()
        {
            if (DefaultLanguageValue.Equals(DEFAULT_LANGUAGE))
            {
                return;
            }
            
            LoadDataPOFile();
        }

        public static string GetPathFilePo(string fileName = "")
        {
            return Application.StartupPath + @"\Language\" + fileName;
        }

        public static string DefaultLanguageValue {
            get
            {
                return Properties.Settings.Default.defaultLanguage;
            }
            set
            {
                Properties.Settings.Default.defaultLanguage = value;
                Properties.Settings.Default.Save();
            }
        }
    }
}
