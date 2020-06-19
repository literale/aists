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
        Dictionary<string, Tabs> tabs = Tab_Settings.Get_tabs();
        bool range_click = false;
        bool click_table = false;
        bool click_type = false;
        int index = 0;
        Group g = Group.Big_type;
        Tab_names t = Tab_names.product_type_big;
        int name = 1;

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
            UD_allow.SelectedIndex = 0;
            UD_id.SelectedIndex = 0;
            UD_type.SelectedIndex = 0;
           // checkedListBox5.SetItemChecked(0, true);
            // Load_on_exept_fоrm();
            LoadRules();
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
            if (UD_type.SelectedIndex == 0)
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

        private void checkedListBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
        //    bool allow = (UD_allow.SelectedIndex == 0) ? false : true;
        //    Group g = (UD_type.SelectedIndex == 0) ? Group.Big_type: ((UD_type.SelectedIndex == 1) ? Group.Little_type : ((UD_type.SelectedIndex == 2) ? Group.Brand : Group.Product));
        //    string s = checkedListBox5.SelectedItem.ToString();
        //    lRules.Add_rule(new Tuple<bool, Group>(allow, g), s);
        }

        private void domainUpDown3_SelectedItemChanged(object sender, EventArgs e)
        {
            click_table = true;
            LoadRules();
        }
        
        private void LoadRules()
        {
           
            bool allow = (UD_allow.SelectedIndex == 0) ? false : true;
           
            string table = tabs[t.ToString().ToLower()].Get_name(); 
            string request = "SELECT * FROM " + table + ";";
            DataTable dt = SQL_Helper.Just_do_it(request);

            // string s = UD_id.Text;
            //string[] ids = s.Split(' ')[1].Split('-');

            //if (index < UD_id.SelectedIndex)
            //    index -= 1;
            //else if (index > UD_id.SelectedIndex) index += 1;
            int id1 = index * 50;
            int id2 = id1 + 50;
            int i = 0;
            for (i = 0; i < checkedListBox5.Items.Count; i++)
            {
                if (checkedListBox5.GetItemChecked(i))
                {
                    lRules.Add_rule(new Tuple<bool, Group>(allow, g), dt.Rows[i+id1].ItemArray[0].ToString());
                }
                i++;
            }
            checkedListBox5.Items.Clear();
            g = (UD_type.SelectedIndex == 0) ? Group.Big_type : ((UD_type.SelectedIndex == 1) ? Group.Little_type : ((UD_type.SelectedIndex == 2) ? Group.Brand : Group.Product));
            t = (UD_type.SelectedIndex == 0) ? Tab_names.product_type_big : ((UD_type.SelectedIndex == 1) ? Tab_names.product_type_little : ((UD_type.SelectedIndex == 2) ? Tab_names.brands : Tab_names.product));
            name = (UD_type.SelectedIndex == 0) ? 1 : ((UD_type.SelectedIndex == 1) ? 2 : ((UD_type.SelectedIndex == 2) ? 1 : 1));
            table = tabs[t.ToString().ToLower()].Get_name();
            request = "SELECT * FROM " + table + ";";
            dt = SQL_Helper.Just_do_it(request);
            index = UD_id.SelectedIndex;
            id1 = index * 50;
            id2 = id1 + 50;
            if (!range_click)
            {
                // s = UD_id.Text;
                // string[] ids = s.Split(' ')[1].Split('-');
                id1 = 0;
                id2 = 50;
                int h = 0;
                UD_id.Items.RemoveRange(0, UD_id.Items.Count);
                i = 0;
                for (i = 0; i < dt.Rows.Count; i += 50)
                {
                    UD_id.Items.Add("id: " + id1 + "-" + id2);
                    id1 += 50;
                    id2 += 50;
                }
                UD_id.SelectedIndex = 0;
                id1 = 0;
                id2 = 50;
            }
            int j = 0;
            for ( i = id1; i< id2 && i<dt.Rows.Count; i++)
            {
                checkedListBox5.Items.Add(dt.Rows[i].ItemArray[name].ToString());
                checkedListBox5.SetItemChecked(j, false);
                    j++;
            }

            if (lRules.Get_rule(new Tuple<bool, Group>(allow, g)) != null)
            {

                for( i=0; i< checkedListBox5.Items.Count; i++)
                {
                    string cb_id = checkedListBox5.Items[i].ToString();
                    string field = (UD_type.SelectedIndex == 0) ? "name_product_type_big" : ((UD_type.SelectedIndex == 1) ? "name_product_type_little" : ((UD_type.SelectedIndex == 2) ? "brand_name" : "name"));
                    dt = SQL_Helper.Just_do_it("Select * FROM "+ table + " WHERE " + tabs[t.ToString().ToLower()].Get_field(field)+ " = '"+ cb_id+"';");
                    List<string> temp = lRules.Get_rule(new Tuple<bool, Group>(allow, g));
                    string t_id = dt.Rows[0].ItemArray[0].ToString();
                    if (temp.Contains(t_id))
                    {
                        checkedListBox5.SetItemChecked( i, true);
                    }
                }
            }
            checkedListBox5.MultiColumn = true;
            range_click = false;
            click_table = false;
            click_type = false;

        }

        private void domainUpDown1_SelectedItemChanged(object sender, EventArgs e)
        {
            click_type = true;
            LoadRules();
        }

        private void domainUpDown2_SelectedItemChanged(object sender, EventArgs e)
        {
            //string s = UD_id.Text;
            //string[] ids = s.Split(' ')[1].Split('-');
            // UD_id.Text = "id: " + Convert.ToInt32(ids[0] + 50) + "-" + Convert.ToInt32(ids[1] + 50);
            range_click = true;
            LoadRules();
        }
        //private listProductOverRules GetRules()
        //{
        //    listProductOverRules lRules = new listProductOverRules();

        //    return lRules;
        //}
    }
}
