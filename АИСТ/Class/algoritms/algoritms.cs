using Renci.SshNet.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using АИСТ.Class.AutoSet;
using АИСТ.Class.enums;
using АИСТ.Class.essence;
using АИСТ.Forms;

namespace АИСТ.Class.algoritms
{
    class algoritms
    {
        System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
        RichTextBox rtb = new RichTextBox();
        public void Auto()
        {
            Form f2 = new Process();
            f2.Show(); // отображаем Form2

            foreach (Control ctrl in f2.Controls)
            {
                if (ctrl is RichTextBox)
                {
                    rtb = (RichTextBox)ctrl;
                }
            }
            rtb.Text = "";
            rtb.Refresh();

            myStopwatch.Start();
            rtb.Text += "-Начат импорт настроек-" + '\n';
            rtb.Refresh();
            Generate_Setttings gs = AutoSetGenerate.AutoSettings();
            DateTime analiz_border = gs.analiz_border;
            File.Create("test.xml");
            List<Customers> all_customres_sets = gs.customers;
            List<Assortiment> all_assortiment_sets = gs.assortiments;
            listProductOverRules rules = gs.rules;
            rtb.Text += "-Импортированы настройки-" + '\n';
            rtb.Refresh();
            myStopwatch.Stop();
            myStopwatch.Reset();

            myStopwatch.Start();
            rtb.Text += "-Начат процесс анализа клиентов-" + '\n';
            rtb.Refresh();
            List<Client_Tab> client_tabs = Get_Clients_analyze(all_customres_sets);
            rtb.Text += "-Клиенты проанализированы- \n ";
            rtb.Refresh();
            myStopwatch.Stop();
            myStopwatch.Reset();

            myStopwatch.Start();
            rtb.Text += "-Начат процесс анализа товаров-" + '\n';
            rtb.Refresh();
            List<Prod_tab> prod_tabs = Get_Prod_analyze(all_assortiment_sets, rules, analiz_border);
            rtb.Text += "-Товары проанализированы-\n ";
            rtb.Refresh();
            myStopwatch.Stop();
            myStopwatch.Reset();
            int i = 0;
        }


        //--------------------------------КЛИЕНТЫ----------------------------------------------//
        public List<Client_Tab> Get_Clients_analyze(List<Customers> all_customres_sets)
        {
            List<Client_Tab> client_tabs = new List<Client_Tab>();

            foreach (Customers customer_set in all_customres_sets)//для каждого сета клиента из заданых
            {
                rtb.Text += "   Начат процесс анализ сета клиентов " + customer_set.Get_name() + "\n ";
                rtb.Refresh();
                string request = "";
                string[] active = new string[]
                {
                     customer_set.Get_active()[0].ToString("u").Substring(0, 10) ,
                     customer_set.Get_active()[1].ToString("u").Substring(0, 10)
                };
                int[] av_sum = customer_set.Get_averrage_sum(); //границы средней суммы покупок для данного сета
                Dictionary<string, double[]> client_sum = new Dictionary<string, double[]>(); //ид клиента, сумма покупок киента + кол-во чеков
                Dictionary<string, List<string>> client_checks = new Dictionary<string, List<string>>();//ид клиента, чеки клиента
                Dictionary<string, double> cust = new Dictionary<string, double>(); // чистый словарь с клиентами и суммой покупок
                Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod; //<Ид клиента, <ид товара - <кол-во товара - цена товара>>>
                Dictionary<string, double> clients_volumes;// объемы закупок клиента
                // Dictionary<string, Dictionary<string, double[]>> client_prod;
                //List<Client_Tab> client_tabs_XYZ;//сборный анализ клиентов и их покупок

                rtb.Text += "       Начато составления списка клиенто по " + customer_set.Get_name() + "\n ";
                rtb.Refresh();
                //Реквест с временем и магазинами
                request = "SELECT * FROM checks WHERE check_date > \"" + active[0] + "\" AND check_date < \"" + active[1] + "\" AND (";
                foreach (string shop in customer_set.Get_shops())
                {
                    request += "ID_shop_check = '" + shop + "' OR ";
                }
                request = request.Substring(0, request.Length - 3) + ");";
                //получаем выборку активных клиентов по нудным магазинам
                DataTable temp_dt = SQL_Helper.Just_do_it(request);

                rtb.Text += "       Начато составление списка покупок клиентов по " + customer_set.Get_name() + "\n ";
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
                //client_prod = get_checks_1(client_checks);//<Ид клиента, <ид товара - [кол-во товара - цена товара]>>
               
               
                rtb.Text += "       Начат анализ клиентов " + customer_set.Get_name() + "\n ";
                rtb.Refresh();
                client_prod = get_checks_2(client_checks); //<Ид клиента, <ид товара - <кол-во товара - цена товара>>> меньше памяти есть в прпоцессе
                clients_volumes = Get_dictionary_volume(client_prod);// объемы закупок клиента
                client_tabs = client_Analitic_ABC_XYZ(cust, clients_volumes); //фнализ клиентов
                rtb.Text += "       Начат анализ покупок клиентов " + customer_set.Get_name() + "\n ";
                rtb.Refresh();
                client_tabs = prod_analitic_abc_xyz(client_tabs, client_prod);
                int hu = 3;
                //определяем типы закупок для клиента
                rtb.Text += "   Закончен процесс анализ сета клиентов " + customer_set.Get_name() + "\n ";
                rtb.Refresh();

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
        }//создания словаря оюъемам и суммам закупок
        public List<Client_Tab> client_Analitic_ABC_XYZ(Dictionary<string, double> cl_sum, Dictionary<string, double> clients_volumes)//анализ по объемам закупок И суммам
        {
            List<Client_Tab> ct = new List<Client_Tab>();
            cl_sum = cl_sum.OrderBy(pair => Convert.ToDouble(pair.Value)).ToDictionary(pair => pair.Key, pair => pair.Value);//сортирует словарь по общих возрастанию сумм
            clients_volumes = clients_volumes.OrderBy(pair => Convert.ToDouble(pair.Value)).ToDictionary(pair => pair.Key, pair => pair.Value);//сортирует по возрастанию объемов
            Dictionary<string, Type_ABC_XYZ> abc = obj_Types(cl_sum, "a");
            Dictionary<string, Type_ABC_XYZ> xyz = obj_Types(clients_volumes, "z");
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
                            prods[id_prod] = new double[] { t[0] + amount, t[1] + cost * amount };
                        }
                        else
                        {
                            prods.Add(id_prod, new double[2]);
                            prods[id_prod] = new double[] { amount, cost * amount };
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
                            prods[id_prod] = new Tuple<double, double>(prods[id_prod].Item1 + amount, prods[id_prod].Item2 + cost);
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
        public List<Client_Tab> prod_analitic_abc_xyz(List<Client_Tab> client_tabs, Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod)
        {
            foreach (Client_Tab ct in client_tabs)
            {
                string client_id = ct.Get_id();
                Dictionary<string, Type_ABC_XYZ[]> prod = new Dictionary<string, Type_ABC_XYZ[]>();
                //XYZ – доля типа товара в закупках клиента
                //ABC – суммы закупок товара
                Dictionary<string, double> volume = new Dictionary<string, double>();
                Dictionary<string, double> sum = new Dictionary<string, double>();
                Dictionary<string, Tuple<double, double>> prods = client_prod[client_id];
                //собираем словари продуктов дл клиента, потом по любому из них добавляем (соединяем) словари в слиент_таб
                foreach (string id_prod in prods.Keys)
                {
                    Tuple<double, double> one_prod = prods[id_prod]; //кол-во товара - цена товара (суммарная)
                    volume.Add(id_prod, one_prod.Item1);
                    sum.Add(id_prod, one_prod.Item2);
                }
                Dictionary<string, Type_ABC_XYZ> abc = obj_Types(sum, "a");
                Dictionary<string, Type_ABC_XYZ> xyz = obj_Types(volume, "z");
                foreach (string id in abc.Keys)
                {
                    prod.Add(id, new Type_ABC_XYZ[] { abc[id], xyz[id] });
                }
                ct.Set_prod(prod);
            }

            return client_tabs;
        }
        //--------------------------------ПРОДУКТЫ---------------------------------------------//

        public List<Prod_tab> Get_Prod_analyze(List<Assortiment> all_assortiment_sets, listProductOverRules rules, DateTime analiz_border)
        {
            List<Prod_tab> prod_Tabs = new List<Prod_tab>(); //таблица отдельных товаров и группировок     
            Dictionary<Tuple<string, Group>, double> all_prods = Get_All_simple_products(all_assortiment_sets);
            rtb.Text += "   Составлена основная выборка товаров \n ";
            rtb.Refresh();
            rtb.Text += "       Начата обработка исключений \n ";
            rtb.Refresh();
            all_prods = Set_over_rules_prod(rules, all_prods);
            rtb.Text += "       Составлена полная выборка \n ";
            rtb.Refresh();
            //формируем два списка по суммам и обхемам закупок
            rtb.Text += "       Начат анализ товаров \n ";
            rtb.Refresh();
            prod_Tabs = prod_Analitic_ABC_XYZ(all_prods, analiz_border);
            rtb.Text += "   анализ товар закончен \n ";
            rtb.Refresh();
            return prod_Tabs;
        }
        public Dictionary<Tuple<string, Group>, double> Get_All_simple_products(List<Assortiment> all_assortiment_sets)
        {
            Dictionary<Tuple<string, Group>, double> temp_diction_prod = new Dictionary<Tuple<string, Group>, double>(); //хранит рабочие значения, что б удобно удалять
            foreach (Assortiment assortiment in all_assortiment_sets)
            {
                rtb.Text += "Начата сборка из сета товаров " + assortiment.Get_name() + "\n ";
                List<Prod_tab> prod_Tabs_set = new List<Prod_tab>(); //заполняется в конце
                int[] count = assortiment.Get_count();
                string[] deliver = new string[]
                {
                     assortiment.Get_deliver()[0].ToString("u").Substring(0, 10) ,
                     assortiment.Get_deliver()[1].ToString("u").Substring(0, 10)
                };
                string[] shops = assortiment.Get_shops();
                ///сначала получаем скписок продуктов по ассортименту, потом удаляем/добавляем все овергруппы
                string request = "SELECT ID_product_store FROM product_on_store WHERE " +
                    "( last_shipment > \"" + deliver[0] + "\" AND last_shipment < \"" + deliver[1] + "\") " +
                    "AND ( product_amount > \"" + count[0] + "\" AND product_amount < \"" + count[1] + "\") AND (";
                foreach (string shop in shops)
                {
                    request += "ID_shop_store = '" + shop + "' OR ";
                }
                request = request.Substring(0, request.Length - 3) + ");";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                //Мы получили выборку всех подходящих апродуктов. Добавим их в слварь
                foreach (DataRow row in temp_dt.Rows)
                {
                    object[] temp = row.ItemArray;
                    temp_diction_prod.Add(new Tuple<string, Group>(temp[0].ToString(), Group.Product), 1);
                }
                rtb.Text += "Закончена сборка из сета товаров " + assortiment.Get_name() + "\n ";
            }
            return temp_diction_prod;
        }
        public Dictionary<Tuple<string, Group>, double> Set_over_rules_prod(listProductOverRules rules, Dictionary<Tuple<string, Group>, double> all_prods)
        {
            foreach (KeyValuePair<Tuple<string, Group>, bool> kvp in rules.Get_rules())
            {
                if (kvp.Key.Item2 == Group.Product)
                {
                    if (kvp.Value)
                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 1;
                    else
                    {

                        bool b = all_prods.Remove(new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2));
                        bool c = b;
                    }
                }
                if (kvp.Key.Item2 == Group.Brand)
                {
                    if (kvp.Value)
                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 1;
                    else
                    {
                        string request = "SELECT ID_product FROM products WHERE brand_ID = '" + kvp.Key.Item1 + "';";
                        DataTable temp_dt = SQL_Helper.Just_do_it(request);
                        foreach (DataRow row in temp_dt.Rows)
                        {
                            bool b = all_prods.Remove(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product));
                            bool c = b;
                            //all_prods.Remove(row.ItemArray[0].ToString());//удалим так же все продукты этой группы
                        }
                    }
                }
                if (kvp.Key.Item2 == Group.Little_type)
                {
                    if (kvp.Value)
                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 1;
                    else
                    {
                        string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_little = '" + kvp.Key.Item1 + "';";
                        DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                        foreach (DataRow row in temp_dt2.Rows)
                        {
                            request = "SELECT ID_product FROM products WHERE type_little_name = '" + row.ItemArray[0] + "';";
                            DataTable temp_dt = SQL_Helper.Just_do_it(request);
                            foreach (DataRow row2 in temp_dt.Rows)
                            {
                                bool b = all_prods.Remove(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product));
                                bool c = b;
                            }
                        }
                    }
                }
                if (kvp.Key.Item2 == Group.Big_type)
                {
                    if (kvp.Value)
                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 1;
                    else
                    {
                        string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_bigger = '" + kvp.Key.Item1 + "';";
                        DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                        foreach (DataRow row in temp_dt2.Rows)
                        {
                            request = "SELECT ID_product FROM products WHERE type_little_name = '" + row.ItemArray[0] + "';";
                            DataTable temp_dt = SQL_Helper.Just_do_it(request);
                            foreach (DataRow row2 in temp_dt.Rows)
                            {
                                bool b = all_prods.Remove(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product));
                                bool c = b;
                            }
                        }
                    }
                }
            }
            return all_prods;
        }
        public List<Prod_tab> prod_Analitic_ABC_XYZ(Dictionary<Tuple<string, Group>, double> all_prods, DateTime analiz_border)
        {

            rtb.Text += "           Начато составления списка для анализа \n ";
            rtb.Refresh();
            Dictionary<string, double[]> all_sells = new Dictionary<string, double[]>(); //все проданые товары - колвоб прибыль
                                                                                         //  Hashtable all_sells = new Hashtable(new Dictionary<string, double[]>());
            string request = "SELECT ID_check FROM checks WHERE check_date > \"" + analiz_border.ToString("u").Substring(0, 10) + "\";";
            DataTable checks_full = SQL_Helper.Just_do_it(request);
            rtb.Text += "               Начато составление списка для анализа продаж \n ";
            rtb.Refresh();
          
            foreach (DataRow check in checks_full.Rows)
            {
                request = "SELECT * FROM history WHERE ID_check_history = '" + check.ItemArray[0].ToString() + "';";
                DataTable prods = SQL_Helper.Just_do_it(request);
                foreach (DataRow p in prods.Rows)
                {
                    string id = p.ItemArray[1].ToString();
                    double amount = Convert.ToDouble(p.ItemArray[2].ToString());
                    double sum = Convert.ToDouble(p.ItemArray[2].ToString()) * Convert.ToDouble(p.ItemArray[3].ToString());

                    if (all_sells.ContainsKey(id))
                    {
                        double[] t = all_sells[id];
                        all_sells[id] = new double[] { t[0] + amount, t[1] + amount * sum };
                    }
                    else
                    {
                        all_sells.Add(id, new double[] { amount, sum });
                    }
                }
            }
            all_sells = all_sells.OrderBy(pair => Convert.ToInt32(pair.Key)).ToDictionary(pair => pair.Key, pair => pair.Value);

            Dictionary<Tuple<string, Group>, double> cleared_prods = new Dictionary<Tuple<string, Group>, double>(all_prods);//из этого словаря удаляем обработанные элементы для ускорения работы

            rtb.Text += "                   Собираем словарь товаров \n ";
            rtb.Refresh();
            List<Dictionary<string, double>> diction_sum_value = new List<Dictionary<string, double>>();
            for (int i = 0; i < 8; i++)
            {
                diction_sum_value.Add(new Dictionary<string, double>());
            }
            foreach (Tuple<string, Group> prod in all_prods.Keys)
            {
                if (prod.Item2 == Group.Product)
                {
                    cleared_prods.Remove(prod);
                    if (all_sells.ContainsKey(prod.Item1))
                    {
                        diction_sum_value[0].Add(prod.Item1, all_sells[prod.Item1][0]);
                        diction_sum_value[1].Add(prod.Item1, all_sells[prod.Item1][1]);
                    }
                }
            }


            rtb.Text += "                   Собираем словарь брэндов \n ";
            rtb.Refresh();
            all_prods = new Dictionary<Tuple<string, Group>, double>(cleared_prods);
            foreach (Tuple<string, Group> brand in all_prods.Keys)
            {
                if (brand.Item2 == Group.Brand)
                {
                    cleared_prods.Remove(brand);
                    request = "SELECT ID_product FROM products WHERE brand_ID = '" + brand + "';";
                    DataTable prods = SQL_Helper.Just_do_it(request);
                    diction_sum_value[2].Add(brand.Item1, 0);
                    diction_sum_value[3].Add(brand.Item1, 0);
                    foreach (DataRow prod in prods.Rows)
                    {
                        string id = prod.ItemArray[0].ToString();
                        if (all_sells.ContainsKey(id))
                        {
                            diction_sum_value[2][brand.Item1] += all_sells[id][0];
                            diction_sum_value[3][brand.Item1] += all_sells[id][1];
                        }
                    }
                }
            }


            rtb.Text += "                   Собираем словарь малых типов \n ";
            rtb.Refresh();
            all_prods = new Dictionary<Tuple<string, Group>, double>(cleared_prods);
            foreach (Tuple<string, Group> lt in all_prods.Keys)
            {
                if (lt.Item2 == Group.Little_type)
                {
                    cleared_prods.Remove(lt);
                    request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_little = '" + lt.Item1 + "';";
                    DataTable name = SQL_Helper.Just_do_it(request);
                    string lt_name = name.Rows[0].ItemArray[0].ToString();
                    request = "SELECT ID_product FROM products WHERE type_little_name = '" + lt_name + "';";
                    DataTable prods = SQL_Helper.Just_do_it(request);
                    diction_sum_value[4].Add(lt.Item1, 0);
                    diction_sum_value[5].Add(lt.Item1, 0);
                    foreach (DataRow prod in prods.Rows)
                    {
                        string id = prod.ItemArray[0].ToString();
                        if (all_sells.ContainsKey(id))
                        {
                            diction_sum_value[4][lt.Item1] += all_sells[id][0];
                            diction_sum_value[5][lt.Item1] += all_sells[id][1];
                        }
                    }
                }
            }

            rtb.Text += "                   Собираем словарь больших типов \n ";
            rtb.Refresh();
            all_prods = new Dictionary<Tuple<string, Group>, double>(cleared_prods);
            foreach (Tuple<string, Group> bt in all_prods.Keys)
            {
                if (bt.Item2 == Group.Big_type)
                {
                    cleared_prods.Remove(bt);
                    request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_bigger = '" + bt.Item1 + "';";
                    DataTable names = SQL_Helper.Just_do_it(request);
                    diction_sum_value[6].Add(bt.Item1, 0);
                    diction_sum_value[7].Add(bt.Item1, 0);
                    foreach (DataRow r_name in names.Rows)
                    {
                        string lt_name = r_name.ItemArray[0].ToString();
                        request = "SELECT ID_product FROM products WHERE type_little_name = '" + lt_name + "';";
                        DataTable prods = SQL_Helper.Just_do_it(request);
                        foreach (DataRow prod in prods.Rows)
                        {
                            string id = prod.ItemArray[0].ToString();
                            if (all_sells.ContainsKey(id))
                            {
                                diction_sum_value[6][bt.Item1] += all_sells[id][0];
                                diction_sum_value[7][bt.Item1] += all_sells[id][1];
                            }
                        }
                    }

                }
            }



            rtb.Text +="            Сортируем словари \n ";
            rtb.Refresh();
            for (int i = 0; i < 8; i++)
            {
                diction_sum_value[i] = diction_sum_value[i].OrderBy(pair => Convert.ToDouble(pair.Value)).ToDictionary(pair => pair.Key, pair => pair.Value);
            }

            rtb.Text += "   Анализируем товары \n ";
            rtb.Refresh();

            //////////// ТУТ МЫ ЗАКОНЧИЛИ
            System.Diagnostics.Stopwatch myStopwatch2 = new System.Diagnostics.Stopwatch();
            myStopwatch2.Start();
            List<Dictionary<string, Type_ABC_XYZ>> temp_abcxyz = new List<Dictionary<string, Type_ABC_XYZ>>();

            for (int i = 0; i < 8; i++)
            {
                if (i == 0 || i%2 == 0) temp_abcxyz.Add(obj_Types(diction_sum_value[i], "a"));
                else temp_abcxyz.Add(obj_Types(diction_sum_value[i], "x"));
            }
            List<Prod_tab> prod_Tabs = Get_prod_tabs(temp_abcxyz);
            myStopwatch2.Stop();
            return prod_Tabs;
        }

        public List<Prod_tab> Get_prod_tabs(List<Dictionary<string, Type_ABC_XYZ>> temp_abcxyz)
        {
            List<Prod_tab> prod_Tabs = new List<Prod_tab>();
            List<Group> g = new List<Group>() { Group.Product, Group.Brand, Group.Little_type, Group.Big_type };
            int j = 0;
            for (int i = 0; i < 8; i+=2)
            {
                foreach( string id in temp_abcxyz[i].Keys)
                {
                    Prod_tab temp = new Prod_tab();
                    temp.Set_id(id);
                    temp.Set_ABC(temp_abcxyz[i][id]);
                    temp.Set_XYZ(temp_abcxyz[i+1][id]);
                    temp.Set_type(g[j]);
                    prod_Tabs.Add(temp);
                }
                j++;
            }
            return prod_Tabs;
        }


        //--------------------------------ОБЩЕЕ---------------------------------------------//

        public Dictionary<string, Type_ABC_XYZ> obj_Types(Dictionary<string, double> param, string az )
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

    }
}
