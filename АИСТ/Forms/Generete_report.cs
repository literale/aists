using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
        string path = "";
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
                checkBox1.Checked = true;
                checkBox1.Enabled = false;
            }
            if (admin)
            {
                button1.Visible = true;
            }
            path = Directory.GetCurrentDirectory() + "\\temp_promo.txt";
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {

            // InitializeComponent();
            Algoritm a = Info.Get_alg();
            bool test = Info.Is_test();
            //Generate_Setttings gs = Info.Get_settings();
            Dictionary<string, List<Promo>> promos = Info.Get_prom();
            if (Test(textBox1.Text, textBox2.Text))
            {
                MessageBox.Show("Успешно авторизовано");
                if (Directory.Exists(path)) File.Delete(path);
                a.Generate_mails(promos, test, textBox1.Text, textBox2.Text);
            }
            else
            {
                MessageBox.Show("Ошибка автотризации email");
            }
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
            foreach (string client in promos.Keys)
            {
                double min = 100;
                double max = 0;
                foreach (Promo p in promos[client])
                {
                    if (min > p.disc) min = p.disc;
                    if (max < p.disc) max = p.disc;
                    Tuple<string, Group> key = new Tuple<string, Group>(p.id_prod, p.group);
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
            foreach (Tuple<string, Group> key in prods_stat.Keys)
            {
                string name = "";
                if (key.Item2 == Group.Product)
                {
                    string request = "SELECT product_name FROM products WHERE ID_product = '" + key.Item1 + "';";
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
        private bool Test(string email_shop, string password)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            //


            try {
                MailAddress from = new MailAddress(email_shop, "Торговая сеть N");

                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
                MailMessage m = new MailMessage(from, from);

                m.Subject = "Специально для Вас от N!";
                m.Body = "test";

                m.IsBodyHtml = true;

                smtp.Credentials = new NetworkCredential(email_shop, password);
                smtp.EnableSsl = true;
                smtp.Send(m); }
            catch
            {
                return false;
            }
            return true;

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (button1.Text.Equals("Сохранить редакцию"))
            {
                Save_file();
                button1.Text = "Редактировать";
            }
            else
            {
                Do_file();
                button1.Text = "Сохранить редакцию";
            }


        }

        private void Do_file()
        {

            List<string> text = new List<string>();
            Dictionary<string, List<Promo>> promos = Info.Get_prom();
            foreach (String cust in promos.Keys)
            {
                text.Add("Start_Cust");
                DataTable dt = SQL_Helper.Just_do_it("SELECT FIO_customer FROM customers WHERE ID_customer = '" + cust + "';");
                string name = dt.Rows[0].ItemArray[0].ToString();
                text.Add("Cust " + cust + " " + name);
                foreach (Promo p in promos[cust])
                {
                    text.Add(p.ToString());
                }
                text.Add("");
            }
            File.WriteAllLines(path, text.ToArray());
            System.Diagnostics.Process.Start(path);
        }

        private void Save_file()
        {
            Dictionary<string, List<Promo>> promos = new Dictionary<string, List<Promo>>();

            string[] temp_info = File.ReadAllLines(path);
           for (int i = 0; i< temp_info.Length; i++)
            {
                string cust = "";
                List<Promo> p = new List<Promo>();
                if (temp_info[i].Equals("Start_Cust"))
                {
                    i++;
                    cust = temp_info[i].Split(' ')[1];
                    p = new List<Promo>();
                    i++;
                    while (!temp_info[i].Equals(""))
                    {
                        string[] t = temp_info[i].Split(' ');
                        Group g = t[3].Equals("Brand") ? Group.Brand : (t[3].Equals("Little_type") ? Group.Little_type : (t[3].Equals(Group.Big_type) ? Group.Big_type : Group.Product) );
                        p.Add(new Promo(t[1], g, Convert.ToDouble(t[4])));
                        i++;
                    }
                }
                promos.Add(cust, p);
            }
            File.Delete(path);
            Set_report(promos, Info.Get_settings());
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            Info.Set_test(checkBox1.Checked);
        }
    }
}
