using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using АИСТ.Class;
using АИСТ.Class.enums;
using АИСТ.Class.Infos;
using АИСТ.Class.SQL.Tab;
using АИСТ.Forms;
using АИСТ.Properties;

namespace АИСТ
{
    public partial class Settings : Form
    {
        //GroupBox gb_lb = null;
        //GroupBox gb_tb = null;
        //GroupBox gb_bt = null;
        public Settings()
        {
            InitializeComponent();
            //this.Load += new EventHandler(Settings_Load);
            //Shown += new EventHandler(Settings_Shown);
            //temp_get_settings();

        }
        List<Label> lables = new List<Label>();
        List<TextBox> textboxs = new List<TextBox>();
        List<Button> buttons = new List<Button>();
        private void Settings_Load(object sender, EventArgs e)
        {
           
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Tab_Settings.tabs.Clear();
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
        }

      
        private void Settings_Shown(object sender, EventArgs e)
        {
           

            Dictionary<string, Tab> tabs = Tab_Settings.tabs;
            int x = 3;
            int y = 0;
            foreach (String t in tabs.Keys)
            {
                y += 20;
                Point loc = new Point(x, y);
                Label lb = new Label();    
                lb.Visible = true;
                lb.Text = t;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Name = t;
                lb.Location = loc;
                lb.Size = new Size(120, 20);
                lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                lables.Add(lb);
                gb_lb.Controls.Add(lb);


                TextBox tb = new TextBox();
                tb.Visible = true;
                tb.Text = tabs[t].tab_name;
                tb.Name = t;
                tb.Location = loc;
                tb.Size = new Size(139, 20);
                gb_tb.Controls.Add(tb);
                textboxs.Add(tb);
                

                Button bt = new Button();
                bt.Visible = true;
                bt.Text = "Проверить поля";
                bt.Name = t;
                bt.Location = loc;
                bt.Size = new Size(120, 20);
                bt.Click += Create_set_fields;
                gb_bt.Controls.Add(bt);
                buttons.Add(bt);

            }
            // temp_get_settings();
        }


        private void check_settings()
        {

        }
        private void btn_save_Click(object sender, EventArgs e)
        {
            //Form f2 = new menu();
            //f2.Show(); // отображаем Form2
            // скрываем Form1 (this - текущая форма)

            foreach(Control c in gb_tb.Controls)
            {
                Tab_Settings.tabs[c.Name].tab_name = c.Text;
            }

            Info.Update();
            try
            {
                check_settings(); //#TODO 
                comm.login();
                this.Hide();
            }
            catch
            {
                MessageBox.Show("Ошибка настроек");
               
            }
        }
        private void Create_set_fields(object sender, EventArgs e)
        {
            
            string t = ((Button)sender).Name;
            Form f = new Table_settings(t);
            f.Show();
            this.Hide();

        }

        private void Settings_Click(object sender, EventArgs e)
        {
            //Button btn = (Button)sender;
            //string t = btn.Name;
            //Form f = new Table_settings(t);
            //f.Show();
            //this.Hide();

        }
    }
}
