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
            string[] shops = lc[0].Get_shops();////////////БЛИН ВВЕДИ ИХ В ОТБОР
            //КЛИЕНТЫ
            List<Client_Tab> client_tabs = Get_Clients_by_sum_and_active(av_sum, activ);
            int o = 89;
        }

        public List<Client_Tab> Get_Clients_by_sum_and_active(int[] av_sum, DateTime[] active)
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
            Dictionary<string, List<string>> client_checks = new Dictionary<string, List<string>>();
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
                    client_checks[id].Add(temp[0].ToString());
                }
                else
                {
                    client_sum.Add(id, new double[] { sum, 1 });
                    List<string> l = new List<string>();
                    l.Add(temp[0].ToString());
                    client_checks.Add(id, l);
                }
            }//собираем чеки
            foreach (string key in client_sum.Keys)
            {
                double av = client_sum[key][0] / client_sum[key][1];
                if (av > av_sum[0] && av <= av_sum[1])
                {
                    cust.Add(key, client_sum[key][0]);

                }
                else
                {
                    client_checks.Remove(key);
                }
            }//выбираем клиентов с нужными суммами
            List<Client_Tab> client_tabs = client_Analitic_ABC(cust);
            foreach (Client_Tab ct in client_tabs)
            {
                Dictionary<string, int> checks_contains = new Dictionary<string, int>();
                List<string> checks = client_checks[ct.Get_id()];
                foreach (string c_id in checks)
                {
                    request = "SELECT * FROM history WHERE ID_check_history = '" + c_id + "';";
                    temp_dt = SQL_Helper.Just_do_it(request);
                    foreach (DataRow s in temp_dt.Rows)
                    {
                        string id_prod = s.ItemArray[1].ToString();

                        if (checks_contains.ContainsKey(id_prod))
                        {
                            checks_contains[id_prod]+=1;

                        }
                        else
                        {
                            checks_contains.Add(id_prod, 1);
                        }
                    }
                }
                checks_contains = checks_contains.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
                int count = checks_contains.Count();
                double x = count * 0.75;
                double y = count * 0.20;
                double z = count * 0.05;
                int i = 0;
                foreach (string id in checks_contains.Keys)
                {
                    if (0 <= i && i <= z)
                    {
                        ct.Add_prod(id, enums.Type_XYZ.Z);
                    }
                    if (z < i && i <= y)
                    {
                        ct.Add_prod(id, enums.Type_XYZ.Y);
                    }
                    if (y < i)
                    {
                        ct.Add_prod(id, enums.Type_XYZ.X);
                    }
                    i++;
                }
            }//собираем и анализируем продукты
            return client_tabs;
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
       

    }
}
