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
using АИСТ.Forms;
using АИСТ.Properties;

namespace АИСТ
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
            //this.Load += new EventHandler(Settings_Load);
            //Shown += new EventHandler(Settings_Shown);
            //temp_get_settings();

        }

        public void temp_get_settings()
        {
            Tab_Settings.temp_set_email();
            Tab_Settings.Temp_set_tabs();
            Tab_Settings.Temp_set_fields_auto();
            Dictionary<string, Tabs> d = Tab_Settings.Get_tabs();

            Set_settings(d);

        }
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void Settings_Load(object sender, EventArgs e)
        {
           // temp_get_settings();
        }

        private void Set_settings(Dictionary<string, Tabs> d)
        {
            List<Label> l = new List<Label>();
            foreach (Control ctrl in this.groupBox1.Controls)
            {
                if (ctrl is Label)
                {
                    l.Add((Label)ctrl);
                    // do something...
                }
            }

           // Label[] l  = this.Controls.OfType<Label>().ToArray();
                int i = 0;
                foreach(string k in d.Keys)
                {
                    l[i].Text = d[k].Get_name() ;
                    i++;
                }


            
        }

        private void Settings_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Form f2 = new Table_settings();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }

        private void btn_prod_Click(object sender, EventArgs e)
        {

        }

        private void Settings_Shown(object sender, EventArgs e)
        {
            //MessageBox.Show("You are in the Form.Shown event.");
            temp_get_settings();
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            Form f2 = new menu();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }
    }
}
