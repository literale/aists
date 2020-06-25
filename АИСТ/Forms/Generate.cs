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
        bool update_assort = false;
        bool update_cust = false;
        bool touch_cust = false;
        bool touch_assort = false;
        string last_cust = "";
        string last_assort = "";
        bool start = false;
        bool delete = false;
        int index = 0;
        Group g = Group.Big_type;
        Tab_names t = Tab_names.product_type_big;
        int name = 1;
        static Collect_settings c_set = Info.temp_settings;
        
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
            Reset_assort();
            Reset_cust();
            dateTimePicker2.Value = DateTime.Now.AddDays(20).Date;

        }
        private void Reset_cust()
        {
            dt_min_active.Value = DateTime.Now.AddDays(-10).Date;
            dt_max_active.Value = DateTime.Now.Date;
            tb_min_sum.Text = "0";
            tb_max_sum.Text = "10000";
            touch_cust = false;
        }
        private void Reset_assort()
        {
            dt_min_Deliver.Value = DateTime.Now.AddDays(-10).Date;
            dt_max_Deliver.Value = DateTime.Now.Date;
            tb_min_count.Text = "0";
            tb_max_count.Text = "100";
            touch_assort = false;
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
            if (!start)
            {
                Form ifrm = Application.OpenForms[0];
                ifrm.Show();
            }
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
                start = true;
                this.Close();
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
                Info.Set_promo(promos, a, gs);
                Form gr = new Generete_report();
                start = true;
                this.Close();
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

        private void button2_Click_1(object sender, EventArgs e)
        {
            Save_assort();
        }

        private void CM_assort_Opening(object sender, CancelEventArgs e)
        {

        }

        private void изменитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Update_assort();
        }

        private void Save_assort()
        {
            int min_count = Convert.ToInt32(tb_min_count.Text);
            int max_count = Convert.ToInt32(tb_max_count.Text);
            DateTime deliver1 = dt_min_Deliver.Value;
            DateTime deliver2 = dt_max_Deliver.Value;
            string name = tb_name_assort.Text;
            Assortiment assort = new Assortiment(min_count, max_count, deliver1, deliver2, name);
            if (min_count > max_count || deliver1 > deliver2)
            {
                MessageBox.Show("Недопустимое значение границ");
                return;
            }
            if (update_assort)
            {
                c_set.assortiments.Remove(assort);
                lb_assort.Items.RemoveAt(lb_assort.Items.IndexOf(last_assort));
                update_assort = false;
            }
            if (c_set.assortiments.Contains(assort))
            {
                MessageBox.Show("Недопустимое имя");
                return;
            }

            c_set.assortiments.Add(assort);
            tb_name_assort.Text = "Ассортимент" + (c_set.assortiments.Count+1).ToString();
            lb_assort.Items.Add(name);
            Reset_assort();
        }
        private void Update_assort()
        {
            
            if (touch_assort)
            {
                if (MessageBox.Show("Сохранить изменения?", "Сохранить?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Save_assort();
                }
            }
            update_assort = true;
            Assortiment assort = c_set.assortiments[lb_assort.SelectedIndex];
            last_assort = assort.Get_name();
            tb_min_count.Text = assort.Get_count()[0].ToString();
            tb_max_count.Text = assort.Get_count()[1].ToString();
            dt_min_Deliver.Value = assort.Get_deliver()[0];
            dt_max_Deliver.Value = assort.Get_deliver()[1];
            tb_name_assort.Text = assort.Get_name();
            touch_assort = false;
        }
        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            delete = true;
            
            c_set.assortiments.Remove(c_set.assortiments[lb_assort.SelectedIndex]);
            update_assort = false;
            lb_assort.Items.Remove(lb_assort.SelectedItem);
            delete = false;
            Reset_assort();
        }

        private void lb_assort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_assort.SelectedIndex != -1)
            {
                CM_assort.Show(Cursor.Position);
                //if (!delete)
                //    Update_assort();
                //else
                //    Reset_assort();
            }
        }


        private void Touch_ass(object sender, EventArgs e)
        {
            touch_assort = true;
        }

        private void Touch_cust(object sender, EventArgs e)
        {
            touch_cust = true;
        }

        private void lb_assort_MouseClick(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    CM_assort.Show(Cursor.Position);
            //}
        }

        private void lb_assort_MouseDown(object sender, MouseEventArgs e)
        {
            //if (e.Button == MouseButtons.Right)
            //{
            //    if (lb_assort.SelectedIndex > 0)
            //        CM_assort.Show(Cursor.Position);
            //}
        }

        private void CM_client_Opening(object sender, CancelEventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Save_cust();
        }

        private void Save_cust()
        {
            int min_sum = Convert.ToInt32(tb_min_sum.Text);
            int max_sum = Convert.ToInt32(tb_max_sum.Text);
            DateTime dt_act1 = dt_min_active.Value;
            DateTime dt_act2 = dt_max_active.Value;
            string name = tb_name_cust.Text;
            Customers cust = new Customers(dt_act1, dt_act2, min_sum, max_sum,  name);
            if (min_sum > max_sum || dt_act1 > dt_act2)
            {
                MessageBox.Show("Недопустимое значение границ");
                return;
            }
            if (update_cust)
            {
                c_set.customers.Remove(cust);
                lb_client.Items.RemoveAt(lb_client.Items.IndexOf(last_cust));
                update_cust = false;
            }
            if (c_set.customers.Contains(cust))
            {
                MessageBox.Show("Недопустимое имя");
                return;
            }

            c_set.customers.Add(cust);
            tb_name_cust.Text = "Клиенты" + (c_set.customers.Count + 1).ToString();
            lb_client.Items.Add(name);
            Reset_cust();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            Update_client();
        }

        private void Update_client()
        {

            if (touch_cust)
            {
                if (MessageBox.Show("Сохранить изменения?", "Сохранить?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
                {
                    Save_cust();
                }
            }
            update_cust = true;
            Customers cust = c_set.customers[lb_client.SelectedIndex];
            last_cust = cust.Get_name();
            tb_min_sum.Text = cust.Get_averrage_sum()[0].ToString();
            tb_max_sum.Text = cust.Get_averrage_sum()[1].ToString();
            dt_min_active.Value = cust.Get_active()[0];
            dt_max_active.Value= cust.Get_active()[1];
            tb_name_cust.Text = cust.Get_name();
            touch_cust = false;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            delete = true;

            c_set.customers.Remove(c_set.customers[lb_client.SelectedIndex]);
            update_cust = false;
            lb_client.Items.Remove(lb_client.SelectedItem);
            delete = false;
            Reset_cust();
        }

        private void lb_client_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lb_client.SelectedIndex != -1)
            {
                CM_cust.Show(Cursor.Position);
                //if (!delete)
                //    Update_client();
                //else
                //    Reset_cust();
            }
        }

        private void изменитьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Update_assort();
        }

        private void button7_Click(object sender, EventArgs e)
        {
           // Info.G
        }
    }
}
