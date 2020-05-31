using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using АИСТ.Class.AutoSet;
using АИСТ.Class.enums;

namespace АИСТ.Class.algoritms
{
    class algoritms
    {

        public void Auto()
        {
            Generate_Setttings gs = AutoSetGenerate.AutoSettings();
            File.Create("test.xml");
            List<Customers> all_customres_sets = gs.customers;
            List<Client_Tab> ct = Get_Clients_analyze(all_customres_sets);
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

                //собираем словарь объемов закупок клиентом
               // Dictionary<string, Dictionary<string, double[]>> client_prod = get_checks_1(client_checks);
                Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod = get_checks_2(client_checks);
               
                Dictionary<string, double> clients_volumes = Get_dictionary_volume(client_prod);
               List<Client_Tab> client_tabs_XYZ = client_Analitic_ABC_XYZ(cust, clients_volumes); //фнализ клиентов
                int hu = 3;


            }//для каждого сета клиента из заданых

            return client_tabs;
        }

        public Dictionary<string, double> Get_dictionary_volume(Dictionary<string, Dictionary<string, double[]>> client_prod)
        {
            Dictionary<string, double> clients_volumes = new Dictionary<string, double>();

            foreach (string client_id in client_prod.Keys)
            {
                double sum = 0;
                foreach (string prod in client_prod[client_id].Keys)
                {
                    double count = client_prod[client_id][prod][0];
                    sum += count;
                }
                clients_volumes.Add(client_id, sum);

            }
            return clients_volumes;
        }//создания словаря оюъемов закупок
        public Dictionary<string, double> Get_dictionary_volume(Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod)
        {
            Dictionary<string, double> clients_volumes = new Dictionary<string, double>();

            foreach (string client_id in client_prod.Keys)
            {
                double sum = 0;
                foreach (string prod in client_prod[client_id].Keys)
                {
                    double count = client_prod[client_id][prod].Item1;
                    sum += count;
                }
                clients_volumes.Add(client_id, sum);

            }
            return clients_volumes;
        }//создания словаря оюъемов закупок
        public List<Client_Tab> client_Analitic_ABC_XYZ(Dictionary<string, double> cl_sum, Dictionary<string, double> clients_volumes)//анализ по объемам закупок И суммам
        {
            List<Client_Tab> ct = new List<Client_Tab>();
            cl_sum = cl_sum.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);//сортирует словарь по общих возрастанию сумм
            clients_volumes = clients_volumes.OrderBy(pair => pair.Value).ToDictionary(pair => pair.Key, pair => pair.Value);//сортирует по возрастанию объемов
            Dictionary<string, Type_ABC_XYZ> abc = client_Types(cl_sum, "a");
            Dictionary<string, Type_ABC_XYZ> xyz = client_Types(clients_volumes, "z");
            foreach (string id in abc.Keys)
            {
                Client_Tab temp = new Client_Tab();
                temp.Set_id(id);
                temp.Set_ABC(abc[id]);
                temp.Set_XYZ(xyz[id]);
                ct.Add(temp);
            }

            return ct;
        }

        public Dictionary<string, Type_ABC_XYZ> client_Types(Dictionary<string, double> param, string az)
        {
            Dictionary<string, Type_ABC_XYZ> clients_types = new Dictionary<string, Type_ABC_XYZ>();
            int count = param.Count;
            int i = 0;
            double s1;
            double s2;
            Type_ABC_XYZ t3;
            Type_ABC_XYZ t2;
            Type_ABC_XYZ t1;
            if (az.Equals("a".ToLower()))
            {
                s1 = count * 0.05;
                s2 = count * 0.15;
                t1 = Type_ABC_XYZ.C;
                t2 = Type_ABC_XYZ.B;
                t3 = Type_ABC_XYZ.A;
            }
            else
            {
                s1 = count * 0.05;
                s2 = count * 0.20;
                t1 = Type_ABC_XYZ.Z;
                t2 = Type_ABC_XYZ.Y;
                t3 = Type_ABC_XYZ.X;
            }

            foreach (string id in param.Keys)
            {
                if (0 <= i && i <= s1)
                {
                    clients_types.Add(id, t1);
                }
                if (s1 < i && i <= s2)
                {
                    clients_types.Add(id, t2);
                }
                if (s2 < i)
                {
                    clients_types.Add(id, t3);
                }
                i++;
            }


            return clients_types;
        }
        public Dictionary<string, Dictionary<string, double[]>> get_checks_1(Dictionary<string, List<string>> client_checks)
        {
            Dictionary<string, Dictionary<string, double[]>> client_prod = new Dictionary<string, Dictionary<string, double[]>>();//<Ид клиента, <ид товара - <кол-во товара - цена товара>>>
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            foreach (string id_client in client_checks.Keys)//получаем список вскх покупок кдиентов из заданых чеков
            {
                Dictionary<string, double[]> prods = new Dictionary<string, double[]>();
                foreach (string check_id in client_checks[id_client])
                {
                    string request = "SELECT * FROM history WHERE ID_check_history = '" + check_id + "';";
                    DataTable temp_dt = SQL_Helper.Just_do_it(request);
                    foreach (DataRow s in temp_dt.Rows)
                    {
                        string id_prod = s.ItemArray[1].ToString();
                        double amount = Convert.ToDouble(s.ItemArray[2]);
                        double cost = Convert.ToDouble(s.ItemArray[3]);
                        if (prods.ContainsKey(id_prod))
                        {
                            double[] t = prods[id_prod];
                            prods[id_prod] = new double[] { amount + t[0], cost + t[1] };
                        }
                        else
                        {
                            prods.Add(id_prod, new double[2]);
                            prods[id_prod] = new double[] { amount, cost };
                        }
                    }
                }
                client_prod.Add(id_client, prods);
            }
            return client_prod;
        }
        public Dictionary<string, Dictionary<string, Tuple<double, double>>> get_checks_2(Dictionary<string, List<string>> client_checks)

        {
            Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod = new Dictionary<string, Dictionary<string, Tuple<double, double>>>();//<Ид клиента, <ид товара - <кол-во товара - цена товара>>>
            foreach (string id_client in client_checks.Keys)//получаем список вскх покупок кдиентов из заданых чеков
            {
                Dictionary<string, Tuple<double, double>> prods = new Dictionary<string, Tuple<double, double>>();
                foreach (string check_id in client_checks[id_client])
                {
                    string request = "SELECT * FROM history WHERE ID_check_history = '" + check_id + "';";
                    DataTable temp_dt = SQL_Helper.Just_do_it(request);
                    foreach (DataRow s in temp_dt.Rows)
                    {
                        string id_prod = s.ItemArray[1].ToString();
                        double amount = Convert.ToDouble(s.ItemArray[2]);
                        double cost = Convert.ToDouble(s.ItemArray[3]);
                        if (prods.ContainsKey(id_prod))
                        {
                            prods[id_prod] = new Tuple<double, double>(prods[id_prod].Item1 + amount, prods[id_prod].Item1 + cost);
                        }
                        else
                        {
                            prods.Add(id_prod, new Tuple<double, double>(amount, cost));
                        }
                    }
                }
                client_prod.Add(id_client, prods);
            }
            return client_prod;
        }

    }
}
