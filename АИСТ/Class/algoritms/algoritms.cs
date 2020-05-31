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
            List<Customers> all_customres_sets = gs.customers;

            //DateTime[] activ = lc[0].Get_active();
            //int[] av_sum = lc[0].Get_averrage_sum();
            //string[] shops = lc[0].Get_shops();////////////БЛИН ВВЕДИ ИХ В ОТБОР
            //КЛИЕНТЫ
            //List<Client_Tab> client_tabs = Get_Clients_by_sum_and_analyze_it(av_sum, activ, shops);
            //int i = 88;

            //List<Assortiment> la = gs.assortiments;
            //DateTime[] duliver = la[0].Get_deliver();


        }
        //------------------------------------------------------------------------------//
        //Отсеивает клиентов, анализирует абс,  и потом анализирует их покупки по xyz
        //public List<Client_Tab> Get_Clients_by_sum_and_analyze_it_auto(int[] av_sum, DateTime[] active, string[] shops)
        //{
            //Dictionary<string, double> cust = new Dictionary<string, double>();
            //string table_name = "checks";
            //string request = "";
            //string s1 = active[0].ToString("u");
            //s1 = s1.Substring(0, 10);
            //string s2 = active[1].ToString("u");
            //s2 = s2.Substring(0, 10);

            //// request = "SELECT * FROM checks WHERE check_date > \"" + s1 + "\" AND check_date < \"" + s2 + "\";";
            //request = "SELECT * FROM checks WHERE check_date > \"" + s1 + "\" AND check_date < \"" + s2 + "\" AND (";
            //foreach (string s in shops)
            //{
            //    request += "ID_shop_check = '"+ s+"' OR ";
            //}
            //request = request.Substring(0, request.Length - 3) + ");";
            //DataTable temp_dt = SQL_Helper.Just_do_it(request);
            //Dictionary<string, double[]> client_sum = new Dictionary<string, double[]>();
            //Dictionary<string, List<string>> client_checks = new Dictionary<string, List<string>>();
            //foreach (DataRow s in temp_dt.Rows)
            //{
            //    object[] temp = s.ItemArray;
            //    string id = temp[3].ToString();
            //    double sum = Convert.ToDouble(temp[4]);
            //    if (client_sum.ContainsKey(id))
            //    {
            //        double[] sm = client_sum[id];
            //        sm[0] += sum;
            //        sm[1] += 1;
            //        client_checks[id].Add(temp[0].ToString());
            //    }
            //    else
            //    {
            //        client_sum.Add(id, new double[] { sum, 1 });
            //        List<string> l = new List<string>();
            //        l.Add(temp[0].ToString());
            //        client_checks.Add(id, l);
            //    }
            //}//собираем чеки
            //foreach (string key in client_sum.Keys)
            //{
            //    double av = client_sum[key][0] / client_sum[key][1];
            //    if (av > av_sum[0] && av <= av_sum[1])
            //    {
            //        cust.Add(key, client_sum[key][0]);

            //    }
            //    else
            //    {
            //        client_checks.Remove(key);
            //    }
            //}//выбираем клиентов с нужными суммами
           // List<Client_Tab> client_tabs = client_Analitic_ABC_auto(cust);
            //foreach (Client_Tab ct in client_tabs)
            //{
            //    Dictionary<string, int> checks_contains = new Dictionary<string, int>();
            //    List<string> checks = client_checks[ct.Get_id()];
            //    foreach (string c_id in checks)
            //    {
            //        request = "SELECT * FROM history WHERE ID_check_history = '" + c_id + "';";
            //        temp_dt = SQL_Helper.Just_do_it(request);
            //        foreach (DataRow s in temp_dt.Rows)
            //        {
            //            string id_prod = s.ItemArray[1].ToString();

            //            if (checks_contains.ContainsKey(id_prod))
            //            {
            //                checks_contains[id_prod]+=1;

            //            }
            //            else
            //            {
            //                checks_contains.Add(id_prod, 1);
            //            }
            //        }
            //    }
            //    checks_contains = checks_contains.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
            //    int count = checks_contains.Count();
            //    double x = count * 0.75;
            //    double y = count * 0.20;
            //    double z = count * 0.05;
            //    int i = 0;
            //    foreach (string id in checks_contains.Keys)
            //    {
            //        if (0 <= i && i <= z)
            //        {
            //            ct.Add_prod(id, enums.Type_XYZ.Z);
            //        }
            //        if (z < i && i <= y)
            //        {
            //            ct.Add_prod(id, enums.Type_XYZ.Y);
            //        }
            //        if (y < i)
            //        {
            //            ct.Add_prod(id, enums.Type_XYZ.X);
            //        }
            //        i++;
            //    }
            //}//собираем и анализируем продукты
            //return client_tabs;
        //}
        //public List<Client_Tab> client_Analitic_ABC_аро(Dictionary<string, double> cl_sum)
        //{
        //    List<Client_Tab> ct = new List<Client_Tab>();

        //    cl_sum = cl_sum.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);
        //    int count = cl_sum.Count();
        //    double a = count * 0.8;
        //    double b = count * 0.15;
        //    double c = count * 0.05;
        //    int i = 0;
        //    Client_Tab temp;
        //    foreach (string id in cl_sum.Keys)
        //    {
        //        if (0<=i && i <= c)
        //        {
        //            temp = new Client_Tab();
        //            temp.Set_id(id);
        //            temp.Set_ABC(enums.Type_ABC.C);
        //            ct.Add(temp);
        //        }
        //        if (c < i && i <= b)
        //        {
        //            temp = new Client_Tab();
        //            temp.Set_id(id);
        //            temp.Set_ABC(enums.Type_ABC.B);
        //            ct.Add(temp);
        //        }
        //        if (b < i)
        //        {
        //            temp = new Client_Tab();
        //            temp.Set_id(id);
        //            temp.Set_ABC(enums.Type_ABC.A);
        //            ct.Add(temp);
        //        }
        //        i++;
        //    }

        //    return ct;
        //}
        //------------------------------------------------------------------------------//
        public List<Client_Tab> Get_Clients_analyze(List<Customers> all_customres_sets)
        {
            List<Client_Tab> client_tabs = new List<Client_Tab>();

            foreach (Customers customer_set in all_customres_sets)//для каждого сета клиента из заданых
            {
                string request = "";
                //выборка по времени покупок
                string active_start = customer_set.Get_active()[0].ToString("u");
                active_start = active_start.Substring(0, 10);
                string active_end = customer_set.Get_active()[1].ToString("u");
                active_end = active_end.Substring(0, 10);
                //Реквест с временем и магазинами
                request = "SELECT * FROM checks WHERE check_date > \"" + active_start + "\" AND check_date < \"" + active_end + "\" AND (";
                foreach (string shop in customer_set.Get_shops())
                {
                    request += "ID_shop_check = '" + shop + "' OR ";
                }
                request = request.Substring(0, request.Length - 3) + ");";
                //получаем выборку активных клиентов по нудным магазинам
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                //подготавливает словари для посчета средней суммы и ведения всех чеков клиента за нужное время
                Dictionary<string, double[]> client_sum = new Dictionary<string, double[]>(); //ид клиента, сумма покупок киента + кол-во чеков
                Dictionary<string, List<string>> client_checks = new Dictionary<string, List<string>>();//ид клиента, чеки клиента

                //собираем все чеки всех выбраных клиентов и суммируем их стоимость
                foreach (DataRow s in temp_dt.Rows) //для каждой строки из таблицы чеков //собираем чеки и суммы
                {
                    object[] temp = s.ItemArray;
                    string id = temp[3].ToString(); //ид клиента
                    double sum = Convert.ToDouble(temp[4]); //сумма одного чека
                    if (client_sum.ContainsKey(id)) //если в словаре уже есть клиент
                    {
                        double[] sm = client_sum[id]; //сумма+кол-во покупок
                        sm[0] += sum;//прибавляем сумму этого чека
                        sm[1] += 1;//увеличиваем кол-во покупок
                        client_sum[id] = sm; //обновляем данные
                        client_checks[id].Add(temp[0].ToString());//добавляем чек в список чеков
                    }
                    else // добавляем в оба словаря клиента, чеки и т.д
                    {
                        client_sum.Add(id, new double[] { sum, 1 });
                        List<string> l = new List<string>();
                        l.Add(temp[0].ToString());
                        client_checks.Add(id, l);
                    }
                }//собираем чеки и суммы

                int[] av_sum = customer_set.Get_averrage_sum(); //границы средней суммы покупок для данного сета
                Dictionary<string, double> cust = new Dictionary<string, double>(); // чистый словарь с клиентами и суммой покупок
                foreach (string key in client_sum.Keys)//выбираем клиентов с нужными суммами
                {
                    double av = client_sum[key][0] / client_sum[key][1]; //считаем ср сумму покупок клиента
                    if (av > av_sum[0] && av <= av_sum[1]) //если уклвдывается в пределы - добавляем
                    {
                        cust.Add(key, client_sum[key][0]);

                    }
                    else
                    {
                        client_checks.Remove(key); //иначе не добавляем и удаляем этого клиента из списка с чеками
                    }
                }//выбираем клиентов с нужными суммами
                List<Client_Tab> client_tabs_tmp = client_Analitic_ABC(cust);//анализирует каждого клиента и возвращает список



            }//для каждого сета клиента из заданых

            return client_tabs;
        }

        public List<Client_Tab> client_Analitic_ABC(Dictionary<string, double> cl_sum)
        {
            List<Client_Tab> ct = new List<Client_Tab>();

            cl_sum = cl_sum.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);//сортирует словарь по общих возрастанию сумм
            int count = cl_sum.Count();
            double a = count * 0.8;
            double b = count * 0.15;
            double c = count * 0.05;
            int i = 0;
            Client_Tab temp;
            foreach (string id in cl_sum.Keys)
            {
                if (0 <= i && i <= c)
                {
                    temp = new Client_Tab();
                    temp.Set_id(id);
                    temp.Set_ABC(enums.Type_ABC_XYZ.C);
                    ct.Add(temp);
                }
                if (c < i && i <= b)
                {
                    temp = new Client_Tab();
                    temp.Set_id(id);
                    temp.Set_ABC(enums.Type_ABC_XYZ.B);
                    ct.Add(temp);
                }
                if (b < i)
                {
                    temp = new Client_Tab();
                    temp.Set_id(id);
                    temp.Set_ABC(enums.Type_ABC_XYZ.A);
                    ct.Add(temp);
                }
                i++;
            }

            return ct;
        }


    }
}
