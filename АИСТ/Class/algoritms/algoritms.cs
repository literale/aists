using Google.Protobuf.WellKnownTypes;
using Org.BouncyCastle.Asn1.X509.Qualified;
using Renci.SshNet.Security;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using АИСТ.Class.AutoSet;
using АИСТ.Class.enums;
using АИСТ.Class.essence;
using АИСТ.Class.Setttings;
using АИСТ.Forms;
using System.Drawing;
using System.Text;

namespace АИСТ.Class.algoritms
{
    class algoritms
    {
        System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
        RichTextBox rtb = new RichTextBox();
        Group[] g = new Group[4] { Group.Product, Group.Brand, Group.Little_type, Group.Big_type, };
        List<Dictionary<string, double>> diction_sum_value;//продажи товаров, 8 словарей товар-брэнд-подтим-тип каждый сумма и объем продаж
        Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod;//покупки клиентов кол-вл сумма
        Dictionary<Tuple<string, Group>, double> all_prods_and_group_amount_on_store;//товар на складе (в том числе по группам)

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

            
            rtb.Text += "-Начат импорт настроек-" + '\n';
            rtb.Refresh();
            Generate_Setttings gs = AutoSetGenerate.AutoSettings();
            DateTime analiz_border = gs.analiz_border;
          //  File.Create("test.xml");
            List<Customers> all_customres_sets = gs.customers;
            List<Assortiment> all_assortiment_sets = gs.assortiments;
            listProductOverRules rules = gs.rules;
            rtb.Text += "-Импортированы настройки-" + '\n';
            rtb.Refresh();

            rtb.Text += "-Начат процесс анализа клиентов-" + '\n';
            rtb.Refresh();
            List<Client_Tab> client_tabs = Get_Clients_analyze(all_customres_sets);
            rtb.Text += "-Клиенты проанализированы- \n ";
            rtb.Refresh();

            rtb.Text += "-Начат процесс анализа товаров-" + '\n';
            rtb.Refresh();
            Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> prodTabs = Get_Prod_analyze(all_assortiment_sets, rules, analiz_border);
         //   List<Prod_tab> prod_tabs = Get_Prod_analyze(all_assortiment_sets, rules, analiz_border);
            rtb.Text += "-Товары проанализированы-\n ";
            rtb.Refresh();

            rtb.Text += "-Начат процесс генерации предложений-\n ";
            myStopwatch.Start();
            Dictionary<string, List<Final_product_group>> summary = Get_summary_tables(prodTabs, client_tabs, gs); //получаем сводную таблицу для клиентов (только доступные им товары)
            Object[] d = Get_comparable_lists(summary, diction_sum_value, client_prod, all_prods_and_group_amount_on_store);
            Dictionary<string, List<Promo>> promos =  Generate(d, gs);
            rtb.Text += "-Начат процесс отправки предложений-\n ";
            myStopwatch.Start();
            Generate_mails(promos);
            // Mail_it(promos);
            myStopwatch.Stop();
            myStopwatch.Reset();
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
                //client_prod; //<Ид клиента, <ид товара - <кол-во товара - цена товара>>>
                Dictionary<string, double> clients_volumes;// объемы закупок клиента
                //Dictionary<string, Dictionary<string, double[]>> client_prod;
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
                        double amount = Convert.ToDouble(s.ItemArray[2]); //кол-во
                        double cost = Convert.ToDouble(s.ItemArray[3]);//сумма
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

        public Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> Get_Prod_analyze(List<Assortiment> all_assortiment_sets, listProductOverRules rules, DateTime analiz_border)
        {
            List<Prod_tab> prod_Tabs = new List<Prod_tab>(); //таблица отдельных товаров и группировок     
            Dictionary<Tuple<string, Group>, double> all_prods = Get_All_simple_products(all_assortiment_sets);
            rtb.Text += "   Составлена основная выборка товаров \n ";
            rtb.Refresh();
            rtb.Text += "       Начата обработка исключений \n ";
            rtb.Refresh();
            all_prods = Set_over_rules_prod(rules, all_prods);
            all_prods_and_group_amount_on_store = new Dictionary<Tuple<string, Group>, double>(all_prods);
            rtb.Text += "       Составлена полная выборка \n ";
            rtb.Refresh();
            //формируем два списка по суммам и обхемам закупок
            rtb.Text += "       Начат анализ товаров \n ";
            rtb.Refresh();
            //prod_Tabs = prod_Analitic_ABC_XYZ(all_prods, analiz_border);
            Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> prodTabs = prod_Analitic_ABC_XYZ(all_prods, analiz_border);
            rtb.Text += "   анализ товар закончен \n ";
            rtb.Refresh();
            return prodTabs;
        }
        public Dictionary<Tuple<string, Group>, double> Get_All_simple_products(List<Assortiment> all_assortiment_sets)
        {
            Dictionary<Tuple<string, Group>, double> temp_diction_prod = new Dictionary<Tuple<string, Group>, double>(); //хранит рабочие значения, что б удобно удалять
            foreach (Assortiment assortiment in all_assortiment_sets)
            {
                rtb.Text += "   Начата сборка из сета товаров " + assortiment.Get_name() + "\n ";
                List<Prod_tab> prod_Tabs_set = new List<Prod_tab>(); //заполняется в конце
                int[] count = assortiment.Get_count();
                string[] deliver = new string[]
                {
                     assortiment.Get_deliver()[0].ToString("u").Substring(0, 10) ,
                     assortiment.Get_deliver()[1].ToString("u").Substring(0, 10)
                };
                string[] shops = assortiment.Get_shops();
                ///сначала получаем скписок продуктов по ассортименту, потом удаляем/добавляем все овергруппы
                string request = "SELECT ID_product_store, product_amount FROM product_on_store WHERE " +
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
                    temp_diction_prod.Add(new Tuple<string, Group>(temp[0].ToString(), Group.Product), Convert.ToDouble(temp[1].ToString()));
                }
                rtb.Text += "   Закончена сборка из сета товаров " + assortiment.Get_name() + "\n ";
            }
            return temp_diction_prod;
        }
        public Dictionary<Tuple<string, Group>, double> Set_over_rules_prod(listProductOverRules rules, Dictionary<Tuple<string, Group>, double> all_prods)
        {
            
            foreach (KeyValuePair<Tuple<string, Group>, bool> kvp in rules.Get_rules())
            {
                if (kvp.Key.Item2 == Group.Product)
                {
                        if (!kvp.Value)
                        {

                        bool b = all_prods.Remove(new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2));
                        bool c = b;
                    }
                }
                if (kvp.Key.Item2 == Group.Brand)
                {
                    string request = "SELECT ID_product FROM products WHERE brand_ID = '" + kvp.Key.Item1 + "';";
                    DataTable temp_dt = SQL_Helper.Just_do_it(request);
                    if (kvp.Value)
                    {
                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 0;
                    }
                    foreach (DataRow row in temp_dt.Rows)
                    {
                        if (kvp.Value)
                        {
                            if (all_prods.ContainsKey(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)))
                            {
                                all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] += all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)];
                            }
                            else
                            {
                                request = "SELECT product_amount FROM product_on_store WHERE ID_product_store = '" + row.ItemArray[0].ToString() + "';";
                                DataTable am = SQL_Helper.Just_do_it(request);
                                if (am.Rows.Count > 0)
                                {
                                    all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] += Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                    all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)] = Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                }
                            }
                        }
                        else
                        {
                            bool b = all_prods.Remove(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product));//удалим так же все продукты этой группы
                            bool c = b;
                        }

                    }
                }
                if (kvp.Key.Item2 == Group.Little_type)
                {
                    if (kvp.Value)
                    {
                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 0;
                    }
                    string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_little = '" + kvp.Key.Item1 + "';";
                    DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                    foreach (DataRow row2 in temp_dt2.Rows)
                    {
                        request = "SELECT ID_product FROM products WHERE type_little_name = '" + row2.ItemArray[0] + "';";
                        DataTable temp_dt = SQL_Helper.Just_do_it(request);
                        if (kvp.Value)
                        {
                            all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 0;
                        }
                        foreach (DataRow row in temp_dt.Rows)
                        {
                            if (kvp.Value)
                            {
                                if (all_prods.ContainsKey(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)))
                                {
                                    all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] += all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)];
                                }
                                else
                                {
                                    request = "SELECT product_amount FROM product_on_store WHERE ID_product_store = '" + row.ItemArray[0].ToString() + "';";
                                    
                                    DataTable am = SQL_Helper.Just_do_it(request);
                                    if (am.Rows.Count > 0)
                                    {
                                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] += Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                        all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)] = Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                    }
                                }
                            }
                            else
                            {
                                bool b = all_prods.Remove(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product));//удалим так же все продукты этой группы
                                bool c = b;
                            }

                        }
                    }
                }
                if (kvp.Key.Item2 == Group.Big_type)
                {
                    if (kvp.Value)
                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 1;

                    string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_bigger = '" + kvp.Key.Item1 + "';";
                    DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                    foreach (DataRow row2 in temp_dt2.Rows)
                    {
                        request = "SELECT ID_product FROM products WHERE type_little_name = '" + row2.ItemArray[0].ToString() + "';";
                        DataTable temp_dt = SQL_Helper.Just_do_it(request);
                        if (kvp.Value)
                        {
                            all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] = 0;
                        }
                        foreach (DataRow row in temp_dt.Rows)
                        {
                            if (kvp.Value)
                            {
                                if (all_prods.ContainsKey(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)))
                                {
                                    all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] += all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)];
                                }
                                else
                                {
                                    request = "SELECT product_amount FROM product_on_store WHERE ID_product_store = '" + row.ItemArray[0].ToString() + "';";
                                    DataTable am = SQL_Helper.Just_do_it(request);
                                    if (am.Rows.Count > 0)
                                    {
                                        all_prods[new Tuple<string, Group>(kvp.Key.Item1, kvp.Key.Item2)] += Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                        all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)] = Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                    }
                                }
                            }
                            else
                            {
                                bool b = all_prods.Remove(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product));//удалим так же все продукты этой группы
                                bool c = b;
                            }

                        }
                    }

                
                }
            }

            return all_prods;
        }
        public Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> prod_Analitic_ABC_XYZ(Dictionary<Tuple<string, Group>, double> all_prods, DateTime analiz_border)
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
            diction_sum_value = new List<Dictionary<string, double>>();
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
                int y = 9;
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
            //List<Prod_tab> prod_Tabs = Get_prod_tabs(temp_abcxyz);
            Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> prodTabs = new Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>>();
            prodTabs = Get_prod_list(temp_abcxyz, prodTabs);
            myStopwatch2.Stop();
            return prodTabs;
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
        public Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> Get_prod_list(List<Dictionary<string, Type_ABC_XYZ>> temp_abcxyz, Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> prodTabs)
        {
           // List<Prod_tab> prod_Tabs = new List<Prod_tab>();
            List<Group> g = new List<Group>() { Group.Product, Group.Brand, Group.Little_type, Group.Big_type };
            int j = 0;
            for (int i = 0; i < 8; i += 2)
            {
                foreach (string id in temp_abcxyz[i].Keys)
                {
                    //Tuple<Group, Type_ABC_XYZ[]> key = new Tuple<Group, Type_ABC_XYZ[]>(g[j], new Type_ABC_XYZ[2] { temp_abcxyz[i][id], temp_abcxyz[i+1][id] } );
                    Tuple < Type_ABC_XYZ, Type_ABC_XYZ > a = new Tuple < Type_ABC_XYZ, Type_ABC_XYZ >( temp_abcxyz[i][id], temp_abcxyz[i + 1][id] );
                    if (prodTabs.ContainsKey(new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[j], a)))
                    {
                        prodTabs[new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[j], a)].Add(id);
                    }
                    else
                    {

                        List<string> tl = new List<string>();
                        tl.Add(id);
                        prodTabs.Add(new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[j], a), tl);
                    }
                }
                j++;
            }
            return prodTabs;
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
            if (param.Count < 4)
            {
                int k = 0;
                foreach (string id in param.Keys)
                {
                    if (k==0)
                        clients_types.Add(id, t1);
                    if (k==1)
                        clients_types.Add(id, t2);
                    if (k == 2)
                        clients_types.Add(id, t3);
                    k++;
                }
            }
            else
            {
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
            }

            return clients_types;
        }

        //--------------------------------Генерация----------------------------------------------//
        public Dictionary<string, List<Final_product_group>> Get_summary_tables(Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> prodTabs, List<Client_Tab> client_tabs, Generate_Setttings gs)
        {
           // string file = "";
            Table_for_strategy[,] clients = gs.promo_type.Get_clients();//двумерный массив АВС-XYZ
            Table_for_strategy[,] products = gs.promo_type.Get_products();
            Dictionary<string, List<Final_product_group>> summary = new Dictionary<string, List<Final_product_group>>();
            List<Client_Tab> client_tabs_clear = new List<Client_Tab>(client_tabs);
            rtb.Text += "   Начат процесс создания сводных таблиц\n ";
            rtb.Refresh();
            foreach (Client_Tab client in client_tabs)
            {
                Type_ABC_XYZ abc = client.Get_ABC();
                Type_ABC_XYZ xyz = client.Get_XYZ(); //тип клиента
                Table_for_strategy table_strat_client = Get_client_or_prod(abc, xyz, clients); //строка из матрицы стратегий для этого клиента
                if (table_strat_client.Get_prob_of_discount_for().Count == 0)   
                {
                    client_tabs_clear.Remove(client);
                    continue;//сбрасываем, если для этих клиентов не предусмотрена акция
                }
                Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> goods_for_this_client = table_strat_client.Get_prob_of_discount_for(); //если не пусто - собираем стратегии
                List<Final_product_group> s = new List<Final_product_group>();
                foreach (Tuple<Type_ABC_XYZ, Type_ABC_XYZ> type in goods_for_this_client.Keys) //для каждого типа товаров допустимого для клиента
                {
                    Table_for_strategy table_strat_goods = Get_client_or_prod(type.Item1, type.Item2, products);//строка из матрицы стратегий для этого типа продукта
                    double[] this_type_of_goods_available_for_this_client = table_strat_goods.Get_prob_of_discount_for_one(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc,xyz));
                    if (this_type_of_goods_available_for_this_client[0] == -1) continue; //проверяем, дпусти ли данный тип клиента для данного типа продукта
                    //если да - добавляем все продукты этого типа в итог

                    for (int i = 0; i < 4; i++)
                    {
                       
                        if (prodTabs.ContainsKey(new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[i], type)))
                        {
                            List<string> temp = prodTabs[new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[i], type)];
                            List<string> temp2 = new List<string>();
                            foreach (string id in temp)
                            {
                                if (client.Get_prod().ContainsKey(id))
                                    temp2.Add(id);

                            }
                            if (temp2.Count > 0)
                            s.Add(new Final_product_group(temp2, goods_for_this_client[type][0], goods_for_this_client[type][1], this_type_of_goods_available_for_this_client[0], g[i] ));
                        }
                    }
                }

                if (s.Count > 0)
                    summary.Add(client.Get_id(),s);
                int gffguyik = 0;
            }
            rtb.Text += "   Сводные таблицы собраны \n ";
            rtb.Refresh();
            return summary;
        }
        public Table_for_strategy Get_client_or_prod(Type_ABC_XYZ abc, Type_ABC_XYZ xyz, Table_for_strategy[,] custOrProd)
        {
            int i = (abc == Type_ABC_XYZ.A) ? 0 : ((abc == Type_ABC_XYZ.B) ? 1 : 2);
            int j = (xyz == Type_ABC_XYZ.X) ? 0 : ((xyz == Type_ABC_XYZ.Y) ? 1 : 2);
            return custOrProd[i, j];
        }
        public Object[] Get_comparable_lists(Dictionary<string, List<Final_product_group>> summary, List<Dictionary<string, double>> diction_sum_value, Dictionary<string, Dictionary<string, Tuple<double, double>>> client_sells, Dictionary<Tuple<string, Group>, double> all_prods_and_group_amount_on_store)
        {
            Object[] d;//Массив, который вернет эта функция
            //Dictionary<string, Dictionary<Group, List<Product_for_list_client>>> comparable_lists_clients = new Dictionary<string, Dictionary<Group, List<Product_for_list_client>>>();
            Dictionary<string,List<Product_for_list_client>> comparable_lists_clients = new Dictionary<string, List<Product_for_list_client>>();
            Dictionary<Tuple<string, Group>, Product_for_list_shop> comparable_lists_goods = new Dictionary<Tuple<string, Group>, Product_for_list_shop>();
            d = new object[2] { comparable_lists_clients, comparable_lists_goods};
            rtb.Text += "   Начато составление общих пространтв наложения \n ";
            rtb.Refresh();
            rtb.Text += "       Начато составление опространства наложения товаров \n ";
            rtb.Refresh();
            for (int i = 0; i< 8; i+=2)//составляем фулный лист товаров
            {
                Group gr = g[i / 2];
                Dictionary<string, double> sum = diction_sum_value[i];
                Dictionary<string, double> value = diction_sum_value[i+1];
                foreach (string id in sum.Keys)
                {
                    Product_for_list_shop temp_list = new Product_for_list_shop();
                    temp_list.prod_id = id;
                    temp_list.sell_value = value[id];
                    temp_list.sum = sum[id];
                    temp_list.amount_on_store = all_prods_and_group_amount_on_store[new Tuple<string, Group>(id, gr)];
                    comparable_lists_goods.Add(new Tuple<string, Group>(id, gr), temp_list);
                }
                
            }
            rtb.Text += "       Закончено составление опространства наложения товаров \n ";
            rtb.Refresh();
            rtb.Text += "       Начато составление опространства наложения клиентов \n ";
            rtb.Refresh();
            myStopwatch.Restart();
            myStopwatch.Start();
            foreach (string client in summary.Keys)//составляем сравниемый лист для всех клиентов
            {
                List<Product_for_list_client> temp_d = new List<Product_for_list_client>();
                comparable_lists_clients.Add(client, temp_d);
                foreach (Final_product_group prod in summary[client])
                {
                    foreach (string id_prod in prod.ids_prods)
                    {
                        Product_for_list_client temp_list = new Product_for_list_client();
                        temp_list.prod_id = id_prod;
                        temp_list.purchase_value = client_sells[client][id_prod].Item1;
                        temp_list.sum = client_sells[client][id_prod].Item2;
                        temp_list.disc_size_by_client = prod.disc_size_by_client;
                        temp_list.prob_by_client = prod.prob_by_client;
                        temp_list.prior_by_good = prod.prior_by_good;
                        temp_list.g = prod.group;
                        temp_d.Add(temp_list);
                    }

                }
                //организовать удаление пустых листов брэндов и т.д
            }
            myStopwatch.Stop();
            rtb.Text += "       Закончено составление опространства наложения клиентов \n ";
            rtb.Refresh();
            return d;
        }   
        public Dictionary<string, List<Promo>> Generate(Object[] d, Generate_Setttings gs)
        {
            rtb.Text += "   Начато наложение пространтв \n ";
            rtb.Refresh();
           // Dictionary<string, Dictionary<Group, List<Product_for_list_client>>> comparable_lists_clients = (Dictionary<string, Dictionary<Group, List<Product_for_list_client>>>)d[0];
            Dictionary<Tuple<string, Group>, Product_for_list_shop> comparable_lists_goods = (Dictionary<Tuple<string, Group>, Product_for_list_shop>)d[1];
            Dictionary<string, List<Product_for_list_client>> comparable_lists_clients = (Dictionary<string, List<Product_for_list_client>>)d[0];

            Random r = new Random();
            comparable_lists_clients = comparable_lists_clients.OrderByDescending(pair => pair.Value.Count).ToDictionary(pair => pair.Key, pair => pair.Value);
            // comparable_lists_clients = comparable_lists_clients.OrderByDescending(pair => pair.Value.First().Value.Count).ToDictionary(pair => pair.Key, pair => pair.Value);//сортирует клиентов по кол-ву доступных покупок
            List<string> s = new List<string>(comparable_lists_clients.Keys);

            // Dictionary<string, Dictionary<Group, List<Product_for_list_client>>> temp = new Dictionary<string, Dictionary<Group, List<Product_for_list_client>>>();
            Dictionary<string, List<Product_for_list_client>> temp = new Dictionary<string, List<Product_for_list_client>>();
            // Dictionary<string, List<Product_for_list_client>> temp2 = new Dictionary<string, List<Product_for_list_client>>();
            Dictionary<string, List<Product_for_list_client>> temp2 = new Dictionary<string, List<Product_for_list_client>>();

            for (int i = 0; i< 5; i++)
            {
                //t k = r.Next(0, s.Count);
                temp.Add(s[i], comparable_lists_clients[s[i]]);
                int j = s.Count - i-1;
                temp.Add(s[j], comparable_lists_clients[s[j]]);
                int k = s.Count / 2 - i;
                temp.Add(s[k], comparable_lists_clients[s[k]]);
            }
             int[] discount = new int[2] { gs.min_discount, gs.max_discount };
            Dictionary<string, List<Promo>> promos = new Dictionary<string, List<Promo>>();


            foreach (string client in comparable_lists_clients.Keys)
            {
                // promos.Add(client, new List<Promo>());
                Dictionary<Tuple<string, Group>, Product_for_list_shop> temp_goods = new Dictionary<Tuple<string, Group>, Product_for_list_shop>();
                foreach (Product_for_list_client pc in comparable_lists_clients[client])
                {
                    temp_goods.Add(new Tuple<string, Group>(pc.prod_id, pc.g), comparable_lists_goods[new Tuple<string, Group>(pc.prod_id, pc.g)]);
                }

                temp_goods = temp_goods.OrderByDescending(pair => pair.Value.sum).ToDictionary(pair => pair.Key, pair => pair.Value);

                //temp[client] = client.OrderByDescending(pair => pair).ToDictionary(pair => pair.Key, pair => pair.Value);
                List<Product_for_list_client> temp_client_goods = comparable_lists_clients[client].OrderByDescending(pair => pair.sum).ToList();
                ///напоминалка
                ///temp = new Dictionary<string, List<Product_for_list_client>>(); - проды клиента
                ///temp_goods = new Dictionary<Tuple<string, Group>, Product_for_list_shop>(); - проды магазинв
                if (temp_goods.Count == 0 || temp_client_goods.Count == 0) continue;
                List<Promo> temp_p = new List<Promo>();
                promos.Add(client, temp_p);
                double disc_spread = discount[1] - discount[0];
                double disc_step = disc_spread / 3;
                disc_step = Math.Round(disc_step, 1);

                //if (temp_goods.Count == temp_client_goods.Count || temp_client_goods.Count < temp_goods.Count)
                //{
                //    foreach (Product_for_list_client k in temp_client_goods)
                //    {
                //        double disc = k.disc_size_by_client * disc_step + discount[1];
                //        temp_p.Add(new Promo(k.prod_id, k.g, disc));
                //    }
                //}

                //else
                //{

                Dictionary<Tuple<string, Group>, double> prods_c = new Dictionary<Tuple<string, Group>, double>();//товары - скидка
                Dictionary<double, List<Tuple<string, Group>>> prods_s = new Dictionary<double, List<Tuple<string, Group>>>();//приоритет - товар
                Dictionary<double, List <Tuple<string, Group>>>  distribution_full = new Dictionary<double, List<Tuple<string, Group>>>();//вероятность - товар
                //Dictionary<double, List<string>> prods_c = new Dictionary<double, List<string>>();
                //Dictionary<double, List<string>> prods_s = new Dictionary<double, List<string>>();
                //Dictionary<double, int> distribution_full = new Dictionary<double, int>();
                foreach (Product_for_list_client k in temp_client_goods)
                {
                    if (distribution_full.ContainsKey(k.prob_by_client))
                    {
                        distribution_full[k.prob_by_client].Add(new Tuple<string, Group>(k.prod_id, k.g));
                    }
                    else
                    {
                        distribution_full[k.prob_by_client] = new List<Tuple<string, Group>>();
                        distribution_full[k.prob_by_client].Add(new Tuple<string, Group>(k.prod_id, k.g));
                        
                    }
                    prods_c.Add(new Tuple<string, Group>(k.prod_id, k.g), k.disc_size_by_client);
                   // if (prods_c.ContainsKey(new Tuple<string, Group>(k.prod_id, k.g)))
                   // {
                   //     prods_c[new Tuple<string, Group>(k.prod_id, k.g)].Add(k.disc_size_by_client);
                   // }
                   //else
                   // {
                   //     prods_c[k.disc_size_by_client] = new List<string>();
                   //     prods_c[k.disc_size_by_client].Add(k.disc_size_by_client);
                   // }
                    if (prods_s.ContainsKey(k.prior_by_good))
                    {
                        prods_s[k.prior_by_good].Add(new Tuple<string, Group>(k.prod_id, k.g));
                    }
                    else
                    {
                        prods_s[k.prior_by_good] = new List<Tuple<string, Group>>();
                        prods_s[k.prior_by_good].Add(new Tuple<string, Group>(k.prod_id, k.g));
                    }

                }
                Dictionary<double, int> distribution_for_disc = new Dictionary<double, int>();
                //List<Product_for_list_client> temp_client_goods Напоминалка. сортированый список по честному приоритету
                ///temp_goods = new Dictionary<Tuple<string, Group>, Product_for_list_shop>(); - проды магазинв
                foreach (double prob in distribution_full.Keys)//получаем сколько скидок каждого типа получим
                {
                    int h = 0;
                    if (distribution_full.Keys.Count == 1)
                    {
                        h = 5;
                    }
                    int hh = 0;
                    foreach (Tuple<string, Group> t in distribution_full[prob])
                    {
                        hh++;
                    }
                    h = Convert.ToInt32(hh * (prob / 100));
                    if (h < 5) h = 5;

                    distribution_for_disc.Add(prob, h);
                }
                //соберем список по совместному приоритету (например сумма и приоритет)
                Dictionary<Tuple<string, Group>, Tuple<double, double>> full_prior = new Dictionary<Tuple<string, Group>, Tuple<double, double>>();

                foreach (double prob in prods_s.Keys)
                {
                    foreach (Tuple<string, Group> t_prod in prods_s[prob])
                    {
                        foreach (Product_for_list_client k in temp_client_goods)
                        {
                            if ((t_prod.Item1.Equals(k.prod_id))&&(t_prod.Item2.Equals(k.g)))
                            {
                                full_prior.Add(t_prod, new Tuple<double, double>(prob, k.sum));
                                continue;
                            }
                        }

                    }

                }


                //Dictionary<Tuple<string, Group>, double> prods_c = new Dictionary<Tuple<string, Group>, double>();//товары - скидка
                //Dictionary<double, List<Tuple<string, Group>>> prods_s = new Dictionary<double, List<Tuple<string, Group>>>();//приоритет - товар
                //Dictionary<double, List<Tuple<string, Group>>> distribution_full = new Dictionary<double, List<Tuple<string, Group>>>();//вероятность - товар
                //Dictionary<Tuple<string, Group>, Tuple<double, double>> full_prior = new Dictionary<Tuple<string, Group>, Tuple<double, double>>();
                full_prior = full_prior.OrderByDescending(pair => pair.Value.ToValueTuple()).ToDictionary(pair => pair.Key, pair => pair.Value);

                foreach (double prob in distribution_for_disc.Keys)//для кадой вероятносит
                {
                    List<Tuple<string, Group>> prods = distribution_full[prob];//список товаров с этой вероятностью
                    for (int i = 0; i < distribution_for_disc[prob]; i++) //кол-во товаров с этой вероятностью
                    {
                        int f = distribution_for_disc[prob];
                        bool brk = false;
                        Tuple<string, Group> delete_this = new Tuple<string, Group>("", Group.Product);
                        foreach(Tuple<string, Group> prod in full_prior.Keys)
                        {
                            if (prods.Contains(prod))
                            {
                                //double disc = k.disc_size_by_client * disc_step + discount[1];
                                //temp_p.Add(new Promo(k.prod_id, k.g, disc));
                                double disc = discount[0] + disc_step * prods_c[prod];
                                double spesial_step = 10*((double)i / distribution_for_disc[prob]);
                                disc = Convert.ToInt32(disc - spesial_step);
                                if (disc < discount[0]) disc = discount[0];
                                temp_p.Add(new Promo(prod.Item1, prod.Item2, disc));
                                brk = true;
                                delete_this = prod;
                                break;
                            }
                        }
                        full_prior.Remove(delete_this);

                    }
                }


            }


                int gy = 8;
            //}
            rtb.Text += "   Закончено наложение пространтв \n ";
            rtb.Refresh();
            return promos;
        }
        //--------------------------------отправка----------------------------------------------//

        public void Generate_mails(Dictionary<string, List<Promo>> promos)
        {
            string client = promos.First().Key;
            System.IO.StreamWriter wrt = new StreamWriter("output.html");
            List<Promo> promo = promos.First().Value;
            string text = "<h2 align=\"center\">Дорогой клиент, мы рады, что вы с нами!</h2>";
            
            string request = "SELECT FIO_customer, email_customer FROM customers WHERE ID_customer = '" + client + "';";
            DataTable temp_dt = SQL_Helper.Just_do_it(request);
            string mail = temp_dt.Rows[0].ItemArray[1].ToString();
            string name = temp_dt.Rows[0].ItemArray[0].ToString();
            text += "<p align=\"center\"> И специально для вас, <b>" + name +"</b>, мы подготовили эти уникальные предложения.</p>";
            text += "<p align=\"center\"> Что бы воспользоваться ими - просто покажите на кассе эти <b>штрихкоды!</b></p>";
            string path = Directory.GetCurrentDirectory();
            string[] d = Directory.GetDirectories(path);
            path += "\\promos" + DateTime.Now.ToString().Replace(':', '.');
            Directory.CreateDirectory(path);
            System.Drawing.Imaging.ImageFormat image_format = System.Drawing.Imaging.ImageFormat.Bmp;
            foreach (Promo p in promo)
            {
                int discount = (int)p.disc;
                string prod_name = " ";
                string image = "";
                
                switch (p.group)
                {
                    case Group.Product:
                        {
                            request = "SELECT product_name, image_prod, type_little_name FROM products WHERE ID_product = '" + p.id_prod + "';";
                            temp_dt = SQL_Helper.Just_do_it(request);
                            prod_name = temp_dt.Rows[0].ItemArray[0].ToString();
                            image = temp_dt.Rows[0].ItemArray[1].ToString();
                            string little_type = temp_dt.Rows[0].ItemArray[2].ToString();
                            string big_type = "";
                            if (image.Length < 1)
                            {
                                request = "SELECT image_little_type, ID_product_type_bigger FROM product_type_little WHERE name_product_type_little = '" + temp_dt.Rows[0].ItemArray[2].ToString() + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                image = temp_dt.Rows[0].ItemArray[0].ToString();
                                if (image.Length < 1)
                                {
                                    request = "SELECT image_big_type, name_product_type_big FROM product_type_big WHERE ID_product_type_big = '" + temp_dt.Rows[0].ItemArray[1].ToString() + "';";
                                    temp_dt = SQL_Helper.Just_do_it(request);
                                    image = temp_dt.Rows[0].ItemArray[0].ToString();
                                    big_type = temp_dt.Rows[0].ItemArray[1].ToString();
                                    if (image.Length < 1)
                                    {
                                        image = "no_image.png";
                                    }
                                }
                                string input = p.group + " " + p.id_prod + " " + discount + "% " +client;
                                string this_path = path + "\\" + p.id_prod + p.group.ToString() +".bmp";

                                Code128 c = new Code128(input);
                                Bitmap image_this = c.get_img();
                                //Bitmap strih = new Bitmap(image_this, new Size(image_this.Width, 100));
                                Bitmap strih_t = new Bitmap(image_this, new Size(image_this.Width, 120));
                                Bitmap strih2 = DrawWatermark(strih_t, input);
                                strih2.Save(this_path, image_format);
                                // strih2.Save(this_path, imf);
                                ///TODO: Красиво оформить текст  
                                ///Ferplast_Россия_Амуниция-для-питомцев_Клетка-для-грызунов_1шт prod_name 
                                ///little_type - Вино например
                                ///image - картинка товара
                                ///big_type - Алкогольл
                                ///this_path -штрихкод в btm
                                //this_path = Convert.ToBase64String(Encoding.UTF8.GetBytes(strih2.ToString().ToCharArray()));
                                //this_path = Convert.ToBase64String(Encoding.UTF8.GetBytes(this_path.Replace('\\', '/')));
                                //Image image_b = Image.FromFile(@"this_path");
                                //System.IO.MemoryStream memoryStream = new System.IO.MemoryStream();
                                //image_b.Save(memoryStream, System.Drawing.Imaging.ImageFormat.Bmp);
                                //byte[] b = memoryStream.ToArray();
                                //this_path = Convert.ToBase64String(Encoding.UTF8.GetBytes(strih2.ToString().ToCharArray()));
                                //this_path = this_path.Replace('\\', '/');
                                //data:image/png;base64
                                System.Drawing.Image temp = System.Drawing.Image.FromFile(this_path);
                                System.Drawing.ImageConverter converter = new ImageConverter();
                                String imgString = Convert.ToBase64String((byte[])converter.ConvertTo(temp, typeof(byte[])));
                                imgString = "data:image/png;base64," + imgString;
                                text += "<table align=\"center\" width=80% border=\"1\">";
                                text += "<tr><td><img src=\"" + image + "\" width=50% height=60%></td><td><img src=\"" + imgString + "\" width=50% height=60%></td></tr>";
                                //text += "<tr><td><img src=\"" + image + "\" width=50% height=60%></td><td><img src=\"" + imgString + "\" width=50% height=60%></td></tr>";
                                text += "<tr><td colspan=\"2\"><p align=\"center\">" + prod_name + "</p></td></tr></table>";

                            }
                            break;
                        }
                    case Group.Brand:
                        {

                            break;
                        }
                    case Group.Little_type:
                        {

                            break;
                        }
                    case Group.Big_type:
                        {

                            break;
                        }
                }

            }
            wrt.WriteLine(text);
            wrt.Close();
            Mail_it(text, mail);


        }
        public void Mail_it(string text, string mail)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            MailAddress from = new MailAddress("ESGdiplom2020shop@yandex.ru", "Торговая сеть N");
            // кому отправляем
            MailAddress to = new MailAddress(mail);
            // создаем объект сообщения
            MailMessage m = new MailMessage(from, to);
            // тема письма
            m.Subject = "Специально для Вас от N!";
            // текст письма
            //string text = "<h2>Уважаемый клиент</h2>" +
            //    "<br><img src=\"https://www.gemboxsoftware.com/email/examples/104/content/save-email-in-cs-vb.png \"> </br>";
            m.Body = text;
            // письмо представляет код html
            m.IsBodyHtml = true;
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential("ESGdiplom2020shop@yandex.ru", "Literal696Ll!");
            smtp.EnableSsl = true;
            smtp.Send(m);
            Console.Read();
        }
        private Bitmap DrawWatermark(Bitmap originalImage, string text)
        {
            Bitmap bitmap = new Bitmap(originalImage.Width, originalImage.Height);
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.DrawImage(originalImage, new Rectangle(0, 0, originalImage.Width, originalImage.Height));

                float xText = originalImage.Width;
                float yText = 20;
                float fontSize = 11;

                gr.FillRectangle(new SolidBrush(Color.White), 0, originalImage.Height-20, xText, yText);
                gr.DrawString(text, new Font("Segoe UI", 11, FontStyle.Bold), new SolidBrush(Color.Black), 40, originalImage.Height-20);
               // gr.DrawString(text, new Font("Segoe UI", fontSize, FontStyle.Bold), new SolidBrush(Color.DodgerBlue), xText, yText);
                return bitmap;
            }
        }

    }
}
