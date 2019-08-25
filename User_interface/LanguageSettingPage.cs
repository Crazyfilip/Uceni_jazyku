using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace User_interface
{
    public partial class LanguageSettingPage : Form
    {
        private class Language
        {
            public String language_code { get; private set; }
            public String target_language { get; set; }
            public String users_language { get; set; }

            private Label labelUserLang;
            private Label labelTargetLang;
            private PictureBox imageLanguageFlag;

            public Language(string code, string targer_lang, string users_lang)
            {
                language_code = code;
                target_language = targer_lang;
                users_language = users_lang;
            }

            public Panel DrawLanguage()
            {
                // Image
          
                imageLanguageFlag = new PictureBox();
                imageLanguageFlag.Image = Properties.Resources.cz_flag;
                imageLanguageFlag.Location = new System.Drawing.Point(0, 0);
                imageLanguageFlag.Name = "LanguageFlag";
                imageLanguageFlag.Size = new System.Drawing.Size(100, 100);
                imageLanguageFlag.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                bool same_lang = target_language == users_language;
                // Target Language
                if (!same_lang)
                {
                    labelTargetLang = new Label();
                    labelTargetLang.Text = target_language;
                    labelTargetLang.Location = new System.Drawing.Point(110, 10);
                    labelTargetLang.Name = "labelTargetLang";
                    labelTargetLang.Size = new System.Drawing.Size(140, 40);
                }

                // User's language

                labelUserLang = new Label();
                labelUserLang.Text = users_language;
                labelUserLang.Location = new System.Drawing.Point(110, same_lang ? 40 : 60 );
                labelUserLang.Name = "labelUserLang";
                labelUserLang.Size = new System.Drawing.Size(140, 40);

                // Language panel

                Panel lang_panel = new Panel();
                lang_panel.Location = new System.Drawing.Point(10, 10);
                lang_panel.Name = "CzechLanguage"; // Will be some dynamic name;
                lang_panel.Size = new System.Drawing.Size(250, 100);
                lang_panel.TabIndex = 0;
                lang_panel.Controls.Add(imageLanguageFlag);
                if (same_lang) lang_panel.Controls.Add(labelTargetLang);
                lang_panel.Controls.Add(labelUserLang);

                return lang_panel;
            }
        }
        public LanguageSettingPage()
        {
            InitializeComponent();
            LoadLanguages();
        }

        private void LoadLanguages()
        {
            Language czech = new Language("cz", "Čeština", "Čeština");
            panelLanguages.Controls.Add(czech.DrawLanguage());
        }

        private void buttonBack_Click(object sender, EventArgs e)
        {
            Hide();
        }
    }
}
