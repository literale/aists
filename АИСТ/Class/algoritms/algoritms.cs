using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using АИСТ.Class.AutoSet;

namespace АИСТ.Class.algoritms
{
    class algoritms
    {

        public void Auto()
        {
            Generate_Setttings gs = AutoSetGenerate.AutoSettings();
            File.Create("test.xml");
            List<Customers> lc = gs.customers;
            DateTime[] activ = lc[0].Get_active();
            int[] av_sum = lc[0].Get_averrage_sum();
            string[] shops = lc[0].Get_shops();
            //КЛИЕНТЫ
            string[]  clients = Get_Activ_client(activ).ToArray();
            Timer t = new Timer();
            t.Start();
            Dictionary<string, double> cl_sum = Get_Clients_by_sum(clients, av_sum, activ);
            t.Start();
            Dictionary<string, double> cl_sum1 = Get_Clients_by_sum_and_active(av_sum, activ);
            t.Stop();
            List<Client_Tab> client_tabs = client_Analitic_ABC(cl_sum);
            //client_tabs = client_Analitic_XYZ(client_tabs);



        }

        public Dictionary<string, double> Get_Clients_by_sum_and_active(int[] av_sum, DateTime[] active)
        {
            Dictionary<string, double> cust = new Dictionary<string, double>();
            string table_name = "checks";
            string request = "";
            string s1 = active[0].ToString("u");
            s1 = s1.Substring(0, 10);
            string s2 = active[1].ToString("u");
            s2 = s2.Substring(0, 10);

            request = "SELECT * FROM checks WHERE check_date > \"" + s1 + "\" AND check_date < \"" + s2 + "\";";
            DataTable temp_dt = SQL_Helper.Just_do_it(request);
            Dictionary<string, double[]> client_sum = new Dictionary<string, double[]>();
            foreach (DataRow s in temp_dt.Rows)
            {
                object[] temp = s.ItemArray;
                string id = temp[3].ToString();
                double sum = Convert.ToDouble(temp[4]);
                if (client_sum.ContainsKey(id))
                {
                    double[] sm = client_sum[id];
                    sm[0] += sum;
                    sm[1] += 1;
                }
                else
                {
                    client_sum.Add(id, new double[] { sum, 1 });
                }    
            }

            foreach (string key in client_sum.Keys)
            {
                double av = client_sum[key][0] / client_sum[key][1];
                if (av > av_sum[0] && av <= av_sum[1])
                {
                    cust.Add(key, client_sum[key][0]);
                }
            }

            return cust;
        }
        public List<string> Get_Client_Base()
        {
            DataTable allCustomers = SQL_Helper.Try_To_Connect_Full("customers");
            List<string> cust_Id = new List<string>();
            foreach (DataRow s in allCustomers.Rows)
            {
                object[] temp = s.ItemArray;
                cust_Id.Add(temp[0].ToString());
            }
            return cust_Id;
        }
        public List<string> Get_Activ_client( DateTime[] active)
        {
            List<string> activ_cust = new List<string>();
            string table_name = "checks";
            string request = "";
            string s1 = active[0].ToString("u");
            s1 = s1.Substring(0, 10);
            string s2 = active[1].ToString("u");
            s2 = s2.Substring(0, 10);
            request = "SELECT * FROM checks WHERE check_date > \"" + s1 + "\" AND check_date < \"" + s2 + "\";";
            DataTable temp_dt = SQL_Helper.Just_do_it(request);
            foreach (DataRow s in temp_dt.Rows)
            {
                object[] temp = s.ItemArray;
                activ_cust.Add(temp[3].ToString());
            }
            return activ_cust;
        }
        public Dictionary<string, double> Get_Clients_by_sum(string[] clients, int[] av_sum, DateTime[] active)
        {
            Dictionary<string, double> cust = new Dictionary<string, double>();
            string table_name = "checks";
            string request = "";
            string s1 = active[0].ToString("u");
            s1 = s1.Substring(0, 10);
            string s2 = active[1].ToString("u");
            s2 = s2.Substring(0, 10);
            foreach (String client_id in clients)
            {
               
                request = "SELECT * FROM checks WHERE ID_customer_check = " + client_id + " AND check_date > \"" + s1 + "\" AND check_date < \"" + s2 + "\";";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                double sum = 0;
                double count = 0;
                foreach (DataRow s in temp_dt.Rows)
                {
                    object[] temp = s.ItemArray;
                    sum += Convert.ToDouble(temp[4]);
                    count++; 
                }
                double av = sum / count;
                if ((av > av_sum[0] && av <= av_sum[1])&&(cust.ContainsKey(client_id)==false))
                { 
                    cust.Add(client_id, sum);
                }
            }
            return cust;
        }
        public List<Client_Tab> client_Analitic_ABC(Dictionary<string, double> cl_sum)
        {
            List<Client_Tab> ct = new List<Client_Tab>();

            cl_sum = cl_sum.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            int count = cl_sum.Count();
            double a = count * 0.8;
            double b = count * 0.15;
            double c = count * 0.05;
            int i = 0;
            Client_Tab temp;
            foreach (string id in cl_sum.Keys)
            {
                if (0<=i && i <= c)
                {
                    temp = new Client_Tab();
                    temp.Set_id(id);
                    temp.Set_ABC(enums.Type_ABC.C);
                    ct.Add(temp);
                }
                if (c < i && i <= b)
                {
                    temp = new Client_Tab();
                    temp.Set_id(id);
                    temp.Set_ABC(enums.Type_ABC.B);
                    ct.Add(temp);
                }
                if (b < i)
                {
                    temp = new Client_Tab();
                    temp.Set_id(id);
                    temp.Set_ABC(enums.Type_ABC.A);
                    ct.Add(temp);
                }
                i++;
            }

            return ct;
        }
        public List<Client_Tab> client_Analitic_XYZ(List<Client_Tab> client_tabs)
        {
            foreach (Client_Tab ct in client_tabs)
            {
                string client_id = ct.Get_id();
                string request = "SELECT * FROM checks WHERE ID_customer_check = \"" + client_id + "\";";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                Dictionary<string, double> prod_count = new Dictionary<string, double>();

            }

            return client_tabs;
        }

    }
}
