using MySqlX.XDevAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using АИСТ.Class;
using АИСТ.Class.algoritms;
using АИСТ.Class.AutoSet;
using АИСТ.Class.enums;
using АИСТ.Class.essence;
using АИСТ.Class.Setttings;
using АИСТ.Class.SQL.Tab;
using АИСТ.Forms;

namespace АИСТ
{
    public partial class Generate : Form
    {
        int n = 1;
        int n2 = 1;
        bool open = false;
        private listProductOverRules lRules = new listProductOverRules();
        List<string> shops = new List<string>();
        Dictionary<string, Tabs> tabs = Info.Get_tabs();
        public Generate()
        {
            InitializeComponent();
            //button5.Enabled = false;
           if (Info.Is_admin())
            {
                запуститьВТестовомРежимеToolStripMenuItem1.Enabled = true;
                расшифроватьФайлToolStripMenuItem.Enabled = true;
                зашифроватьФайлToolStripMenuItem.Enabled = false;

            }
           else
            {
                запуститьВТестовомРежимеToolStripMenuItem1.Enabled = false;
                расшифроватьФайлToolStripMenuItem.Enabled = false;
                зашифроватьФайлToolStripMenuItem.Enabled = false;
            }
            checkedListBox5.SetItemChecked(0, true);
            Load_on_exept_fоrm();
            Info.temp_settings.promo_type.Generate_matrix();
            setShops();

        }

        //TODO
        private void setShops()
        {
            Tab_shops tu = (Tab_shops)Tab_Settings.tabs[Tab_names.shops.ToString()];
            DataTable dt = SQL_Helper.Try_To_Connect_Full(tu.tab_name);
            foreach(DataRow dr in dt.Rows)
            {
                
                checkedListBox3.Items.Add(dr.ItemArray[2].ToString()+" "+ dr.ItemArray[1].ToString());
            }
        }
        public void Load_on_exept_fоrm()
        {
            checkedListBox5.MultiColumn = true;
            checkedListBox5.Items.Clear();
            string table = "";
            if (domainUpDown1.SelectedIndex == 0)
                table = "product_type_big";
            string request = "SELECT * FROM product_type_big;";
            DataTable dt = SQL_Helper.Just_do_it(request);
            foreach(DataRow dr in dt.Rows)
            {
                checkedListBox5.Items.Add(dr.ItemArray[1].ToString());
            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
           

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
       
        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }


        private void запуститьВТестовомРежимеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!open)
            {
                string connection_string = "server=localhost; " +
                       "user=" + "admin" + "; " +
                       "database=" + "bd_shop" + "; " +
                       "password=" + "diplom2020";
                SQL_Helper.setConnection(connection_string);
                Algoritm a = new Algoritm();
                Dictionary<string, List<Promo>> promos = a.Auto(AutoSetGenerate.AutoSettings());
                Generate_Setttings gs = AutoSetGenerate.AutoSettings();
                Info.Set_test(true);
                Info.Set_promo(promos, a, gs);
                Form gr = new Generete_report();
                gr.Show(); // отображаем Form2
                this.Enabled = false;
            }
            else
            {
                MessageBox.Show("Зашифруйте файл");
            }

        }

        private void расшифроватьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info.Give_me_file();
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("info.txt");
            bool open = true;
            зашифроватьФайлToolStripMenuItem.Enabled = true;
            расшифроватьФайлToolStripMenuItem.Enabled = false;
            if (p.HasExited)
            {
                Info.Take_you_file();
                расшифроватьФайлToolStripMenuItem.Enabled = true;
                зашифроватьФайлToolStripMenuItem.Enabled = false;
                open = false;
                Tab_Settings.Load_info();
            }
        }

        private void зашифроватьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Info.Take_you_file();
                расшифроватьФайлToolStripMenuItem.Enabled = true;
                зашифроватьФайлToolStripMenuItem.Enabled = false;
                open = false;
                Tab_Settings.Load_info();
            }
            catch (Exception)
            {

                    MessageBox.Show("Невозможно сохранить файл");

            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void создатьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form f = new Promo_types_Setings();
            this.Enabled = false;
            f.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void пресетыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        //TEST
        private void запуститьВТестовомРежимеToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if (!open)
            {
                string connection_string = "server=localhost; " +
                       "user=" + "admin" + "; " +
                       "database=" + "bd_shop" + "; " +
                       "password=" + "diplom2020";
                SQL_Helper.setConnection(connection_string);
                Algoritm a = new Algoritm();
                Generate_Setttings gs = AutoSetGenerate.AutoSettings();
                Dictionary<string, List<Promo>> promos = a.Auto(gs);
                
                Info.Set_test(true);
                Info.Set_promo(promos, a, gs);
                Form gr = new Generete_report();
                gr.Show(); // отображаем Form2
                this.Enabled = false;
            }
            else
            {
                MessageBox.Show("Зашифруйте файл");
            }
        }

        //TODO: дополнить строку настроек
        private void запуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!open)
            {
               // SQL_Helper.Set_Connection_String(tb_server.Text, tb_name.Text, tb_login.Text, tb_password.Text);
               // SQL_Helper.setConnection(Info.connection_string);
                Algoritm a = new Algoritm();
                Collect_settings ts = Info.temp_settings;
                shops = new List<string>();

                foreach (int i in checkedListBox3.CheckedIndices)
                {
                    shops.Add(i.ToString());
                }


                Generate_Setttings gs = new Generate_Setttings(dateTimePicker1.Value.Date, dateTimePicker2.Value.Date, (int)numericUpDown1.Value, (int)numericUpDown2.Value,
                    ts.assortiments, ts.customers, ts.promo_type, lRules, dateTimePicker7.Value.Date, shops);
                Dictionary<string, List<Promo>> promos = a.Auto(gs);
                
                Info.Set_test(false);
                Info.Set_promo(new Dictionary<string, List<Promo>>(), a, gs);
                Form gr = new Generete_report();
                gr.Show(); // отображаем Form2
                this.Enabled = false;
            }
            else
            {
                MessageBox.Show("Зашифруйте файл");
            }
        }

        private void начатьГенерациюToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            

        }

        //private listProductOverRules GetRules()
        //{
        //    listProductOverRules lRules = new listProductOverRules();

        //    return lRules;
        //}
    }
}
