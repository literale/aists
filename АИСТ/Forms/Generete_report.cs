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
using АИСТ.Class.enums;
using АИСТ.Class.essence;

namespace АИСТ
{
    public partial class Generete_report : Form
    {
        private bool click_password = true;

        public Generete_report()
        {
            InitializeComponent();
            Algoritm a = Info.Get_alg();
            bool admin = Info.Is_test();
            Generate_Setttings gs = Info.Get_settings();
            Dictionary<string, List<Promo>> promos = Info.Get_prom();
            Set_report(promos, gs);
            if (Info.Is_test())
            {
                textBox3.Visible = true;
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            InitializeComponent();
            Algoritm a = Info.Get_alg();
            bool test = Info.Is_test();
            //Generate_Setttings gs = Info.Get_settings();
            Dictionary<string, List<Promo>> promos = Info.Get_prom();
            a.Generate_mails(promos, test, textBox1.Text, textBox2.Text);

        }

        private void Set_report(Dictionary<string, List<Promo>> promos, Generate_Setttings gs)
        {
            start_date.Text = gs.start.ToString("u");
            stop_date.Text = gs.end.ToString("u");
            prods_all.Text = "";
            clients_all.Text = promos.Count().ToString();
            min_dis.Text = "0";
            max_dis.Text = "0";
            double mind = 100;
            double maxd = 0;
            

            DataTable dt_prods = new DataTable();
            dt_prods.Columns.Add("id");
            dt_prods.Columns.Add("имя");
            dt_prods.Columns.Add("min скидка");
            dt_prods.Columns.Add("max скидка");
            dt_prods.Columns.Add("кол-во предложений");
            dataGridView1.ReadOnly = true;
            DataTable dt_clients = new DataTable();
            dt_clients.Columns.Add("id");
            dt_clients.Columns.Add("имя");
            dt_clients.Columns.Add("min скидка");
            dt_clients.Columns.Add("max скидка");
            dt_clients.Columns.Add("кол-во предложений");
            DataTable dt = new DataTable();
            dataGridView2.ReadOnly = true;
            Dictionary<Tuple<string, Group>, Prod_for_report> prods_stat = new Dictionary<Tuple<string, Group>, Prod_for_report>();
            foreach(string client in promos.Keys)
            {
                double min = 100;
                double max = 0;
                foreach(Promo p in promos[client])
                {
                    if (min > p.disc) min = p.disc;
                    if (max < p.disc) max = p.disc;
                    Tuple<string, Group> key =new  Tuple<string, Group>(p.id_prod, p.group);
                    if (prods_stat.ContainsKey(key))
                    {
                        prods_stat[key].count++;
                        if (prods_stat[key].min > p.disc) prods_stat[key].min = p.disc;
                        if (prods_stat[key].max < p.disc) prods_stat[key].max = p.disc;
                    }
                    else
                    {
                        prods_stat[key] = new Prod_for_report();
                        prods_stat[key].count++;
                        if (prods_stat[key].min > p.disc) prods_stat[key].min = p.disc;
                        if (prods_stat[key].max < p.disc) prods_stat[key].max = p.disc;
                    }
                }
                dt = SQL_Helper.Just_do_it("SELECT FIO_customer FROM customers WHERE ID_customer = '" + client + "';");
                string name = dt.Rows[0].ItemArray[0].ToString();
                dt_clients.Rows.Add(client, name, min, max, promos[client].Count);
                if (mind > min) mind = min;
                if (maxd < max) maxd = max;

            }
            foreach(Tuple<string, Group> key in prods_stat.Keys)
             {
                string name = "";
                if (key.Item2 == Group.Product)
                {
                   string   request = "SELECT product_name FROM products WHERE ID_product = '" + key.Item1 + "';";
                   DataTable temp_dt = SQL_Helper.Just_do_it(request);
                   name = temp_dt.Rows[0].ItemArray[0].ToString();
                }
                if (key.Item2 == Group.Brand)
                {
                    string request = "SELECT brand_name FROM brands WHERE ID_brand = '" + key.Item1 + "';";
                    DataTable temp_dt = SQL_Helper.Just_do_it(request);
                    name = temp_dt.Rows[0].ItemArray[0].ToString();
                }
                if (key.Item2 == Group.Little_type)
                {
                    string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_little = '" + key.Item1 + "';";
                    DataTable temp_dt = SQL_Helper.Just_do_it(request);
                    name = temp_dt.Rows[0].ItemArray[0].ToString();
                }
                if (key.Item2 == Group.Big_type)
                {
                    string request = "SELECT name_product_type_big FROM product_type_big WHERE name_product_type_big = '" + key.Item1 + "';";
                    DataTable temp_dt = SQL_Helper.Just_do_it(request);
                    name = temp_dt.Rows[0].ItemArray[0].ToString();
                }
                dt_prods.Rows.Add(key.Item1, name, prods_stat[key].min, prods_stat[key].max, prods_stat[key].count);
            }
            int id_promo = SQL_Helper.HowMuchRows("promo_info", "ID_promo");
            dt = SQL_Helper.Just_do_it("SELECT IDs_shops_list_promo FROM promo_info WHERE ID_promo = '" + id_promo + "';");

            string[] sh = dt.Rows[0].ItemArray[0].ToString().Split(' ');
            foreach (string s in sh)
            { if (gs.shops.Contains(s))
                shops_all.Items.Add(s);
            }
            prods_all.Text = prods_stat.Count.ToString();
            dataGridView1.DataSource = dt_prods.AsDataView();
            dataGridView2.DataSource = dt_clients.AsDataView();
            min_dis.Text = mind.ToString();
            max_dis.Text = maxd.ToString();
        }

        private void Generete_report_Load(object sender, EventArgs e)
        {
           
        }
        private void tst()
        {

            //DataTable dt1 = new DataTable();
            //dt1.Columns.Add("id");
            //dt1.Columns.Add("имя");
            //dt1.Columns.Add("min скидка");
            //dt1.Columns.Add("max скидка");
            //dt1.Rows.Add("01", "сок", "4", "15");
            //dt1.Rows.Add("02", "мясо", "1", "25"); ;
            //dt1.Rows.Add("03", "тыква", "10", "45");
            //dataGridView1.DataSource = dt1.AsDataView();
            //dataGridView1.ReadOnly = true;

            //DataTable dt2 = new DataTable();
            //dt2.Columns.Add("id");
            //dt2.Columns.Add("имя");
            //dt2.Columns.Add("min скидка");
            //dt2.Columns.Add("max скидка");
            //dt2.Rows.Add("01", "аня", "1", "5");
            //dt2.Rows.Add("02", "ваня", "3", "8");
            //dt2.Rows.Add("03", "петя", "2", "10");
            //dataGridView2.DataSource = dt2.AsDataView();
            //dataGridView2.ReadOnly = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            

        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            if (click_password)
            {
                textBox1.Text = "";
                click_password = false;
            }
        }

        private void textBox2_Click(object sender, EventArgs e)
        {
            if (click_password)
            {
                textBox2.Text = "";
                click_password = false;
            }
        }
    }
}
