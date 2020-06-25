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
using iTextSharp.text.html.simpleparser;
using iTextSharp.text;
using iTextSharp.text.pdf;
using SelectPdf;
using System.Runtime.InteropServices;

namespace АИСТ.Class.algoritms
{
    class Algoritm
    {
        System.Diagnostics.Stopwatch myStopwatch = new System.Diagnostics.Stopwatch();
        Label rtb = new Label();
        ProgressBar prBar = new ProgressBar();
        Group[] g = new Group[4] { Group.Product, Group.Brand, Group.Little_type, Group.Big_type, };
        List<Dictionary<string, double>> diction_sum_value;//продажи товаров, 8 словарей товар-брэнд-подтим-тип каждый сумма и объем продаж
        Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod;//покупки клиентов кол-вл сумма
        Dictionary<Tuple<string, Group>, double> all_prods_and_group_amount_on_store;//товар на складе (в том числе по группам)
        Generate_Setttings gs;
        public Dictionary<string, List<Promo>> Auto(Generate_Setttings gs)
        {
            Form f2 = new Process();
            f2.Show(); // отображаем Form2

            this.gs = gs;
            foreach (Control ctrl in f2.Controls)
            {
                if (ctrl is Label)
                {
                    rtb = (Label)ctrl;
                }
                if (ctrl is ProgressBar)
                {
                    prBar = (ProgressBar)ctrl;
                }

            }
            rtb.Text = "";
            rtb.Refresh();

            
            rtb.Text += "Начат импорт настроек" + '\n';
            rtb.Refresh();
           // gs = AutoSetGenerate.AutoSettings();
            DateTime analiz_border = gs.analiz_border;
            //File.Create("test.xml");
            List<Customers> all_customres_sets = gs.customers;
            List<Assortiment> all_assortiment_sets = gs.assortiments;
            listProductOverRules rules = gs.rules;
            rtb.Text += "Импортированы настройки" + '\n';
            rtb.Refresh();

            rtb.Text += "Начат процесс анализа клиентов" + '\n';
            rtb.Refresh();
            List<Client_Tab> client_tabs = Get_Clients_analyze(all_customres_sets);
            rtb.Text += "Клиенты проанализированы \n ";
            rtb.Refresh();

            rtb.Text += "Начат процесс анализа товаров" + '\n';
            rtb.Refresh();
            Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> catalog_prods = Get_Prod_analyze(all_assortiment_sets, rules, analiz_border);
         //   List<Prod_tab> prod_tabs = Get_Prod_analyze(all_assortiment_sets, rules, analiz_border);
            rtb.Text += "Товары проанализированы\n ";
            rtb.Refresh();

            rtb.Text += "Начат процесс генерации предложений\n ";
            rtb.Refresh();
            myStopwatch.Start();
            Dictionary<string, List<Final_product_group>> summary = Get_summary_tables(catalog_prods, client_tabs); //получаем сводную таблицу для клиентов (только доступные им товары)
            Object[] d = Get_comparable_lists(summary, diction_sum_value, client_prod, all_prods_and_group_amount_on_store);
            Dictionary<string, List<Promo>> promos =  Generate(d);
            return promos;
           
        }


        //--------------------------------КЛИЕНТЫ----------------------------------------------//
        /// <summary>
        /// Анализ клиентов
        /// </summary>
        /// <param name="all_customres_sets"></param>
        /// <returns></returns>
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
                Dictionary<string, double[]> client_sum = new Dictionary<string, double[]>(); //ид клиента, сумма покупок киента + кол-во покупок
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
                foreach (string shop in gs.shops)
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
                clients_volumes = Get_dictionary_volume(client_prod, client_checks);// объемы закупок клиента
                client_tabs = client_Analitic_ABC_XYZ(cust, clients_volumes); //фнализ клиентов
                rtb.Text += "       Начат анализ покупок клиентов " + customer_set.Get_name() + "\n ";
                rtb.Refresh();
                client_tabs = prod_analitic_abc_xyz(client_tabs, client_prod, client_checks);
                //определяем типы закупок для клиента
                rtb.Text += "   Закончен процесс анализ сета клиентов " + customer_set.Get_name() + "\n ";
                rtb.Refresh();

            }//для каждого сета клиента из заданых

            return client_tabs;
        }
        /// <summary>
        /// Определяет объемы закупок клиента
        /// </summary>
        /// <param name="client_prod"></param>
        /// <param name="client_checks"></param>
        /// <returns></returns>
        public Dictionary<string, double> Get_dictionary_volume(Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod, Dictionary<string, List<string>> client_checks)
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
                clients_volumes.Add(client_id, sum/client_checks[client_id].Count());

            }
            return clients_volumes;
        }//создания словаря оюъемам и суммам закупок
    
        /// <summary>
        /// Функцция проведения анализа
        /// </summary>
        /// <param name="cl_sum"></param>
        /// <param name="clients_volumes"></param>
        /// <returns></returns>
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
        
        /// <summary>
        /// Функция составления спика чеков клиента
        /// </summary>
        /// <param name="client_checks"></param>
        /// <returns></returns>
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

        /// Функция анализа покупок каждого клиента
        /// <param name="client_tabs"></param> итоговая таблица с клиентам, редактируемая и возвращаемая в этой функции
        /// <param name="client_prod"></param> //словарь подробных покупок клиента
        public List<Client_Tab> prod_analitic_abc_xyz(List<Client_Tab> client_tabs, Dictionary<string, Dictionary<string, Tuple<double, double>>> client_prod, Dictionary<string, List<string>> client_checks)
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
                    volume.Add(id_prod, one_prod.Item1/ client_checks[client_id].Count);
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
        /// <summary>
        /// Функция общего анализа товаров
        /// </summary>
        /// <param name="all_assortiment_sets"></param>
        /// <param name="rules"></param>
        /// <param name="analiz_border"></param>
        /// <returns></returns>
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
            Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> catalog_prods = prod_Analitic_ABC_XYZ(all_prods, analiz_border);
            rtb.Text += "   Анализ товар закончен \n ";
            rtb.Refresh();
            return catalog_prods;
        }
        /// <summary>
        /// Функция получения всех товаров из всех сетов
        /// </summary>
        /// <param name="all_assortiment_sets"></param>
        /// <returns></returns>
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
                string[] shops = gs.shops.ToArray();
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
                    if (!temp_diction_prod.ContainsKey(new Tuple<string, Group>(temp[0].ToString(), Group.Product)))
                        {
                        temp_diction_prod.Add(new Tuple<string, Group>(temp[0].ToString(), Group.Product), Convert.ToDouble(temp[1].ToString()));
                    }
                    
                }
                rtb.Text += "   Закончена сборка из сета товаров " + assortiment.Get_name() + "\n ";
            }
            return temp_diction_prod;
        }
       
       /// <summary>
       /// Функция наложения особых правил
       /// </summary>
       /// <param name="rules"></param>
       /// <param name="all_prods"></param>
       /// <returns></returns>
        public Dictionary<Tuple<string, Group>, double> Set_over_rules_prod(listProductOverRules rules, Dictionary<Tuple<string, Group>, double> all_prods)
        {
            
            foreach (KeyValuePair<Tuple<bool, Group>, List<string>> kvp in rules.Get_rules())
            {
                if (kvp.Key.Item2 == Group.Product)
                {
                    if (!kvp.Key.Item1)
                    {
                        foreach(string id in kvp.Value)
                            all_prods.Remove(new Tuple<string, Group>(id, kvp.Key.Item2));
                    }
                    else
                    {
                        string request = "SELECT product_amount FROM product_on_store WHERE ID_product_store = '" + kvp.Key.Item1 + "';";
                        DataTable temp_dt = SQL_Helper.Just_do_it(request);
                        double amount = 0;
                        foreach(DataRow dt in temp_dt.Rows)
                        {
                            amount += Convert.ToDouble(dt.ItemArray[0].ToString());
                        }
                        foreach (string id in kvp.Value)
                        {
                            if (!all_prods.ContainsKey(new Tuple<string, Group>(id, kvp.Key.Item2)))
                                all_prods.Add(new Tuple<string, Group>(id, kvp.Key.Item2), amount);
                        }
                            
                    }
                }
                if (kvp.Key.Item2 == Group.Brand)
                {
                    string request = "SELECT ID_product FROM products WHERE brand_ID = '" + kvp.Key.Item1 + "';";
                    DataTable temp_dt = SQL_Helper.Just_do_it(request);
                    if (!kvp.Key.Item1)
                    {
                        foreach (string id in kvp.Value)
                            all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] = 0;
                    }
                    foreach (DataRow row in temp_dt.Rows)
                    {
                        if (kvp.Key.Item1)
                        {
                            if (all_prods.ContainsKey(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)))
                            {
                                foreach (string id in kvp.Value)
                                    all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] += all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)];
                            }
                            else
                            {
                                request = "SELECT product_amount FROM product_on_store WHERE ID_product_store = '" + row.ItemArray[0].ToString() + "';";
                                DataTable am = SQL_Helper.Just_do_it(request);
                                if (am.Rows.Count > 0)
                                {
                                    foreach (string id in kvp.Value)
                                    {
                                        all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] += Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                    }
                                       
                                    all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)] = Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
                                }
                            }
                        }
                        else
                        {
                            bool b = all_prods.Remove(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product));//удалим так же все продукты этой группы
                        }

                    }
                }
                if (kvp.Key.Item2 == Group.Little_type)
                {
                    if (!kvp.Key.Item1)
                    {
                        foreach (string id in kvp.Value)
                            all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] = 0;
                    }
                    string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_little = '" + kvp.Key.Item1 + "';";
                    DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                    foreach (DataRow row2 in temp_dt2.Rows)
                    {
                        request = "SELECT ID_product FROM products WHERE type_little_name = '" + row2.ItemArray[0] + "';";
                        DataTable temp_dt = SQL_Helper.Just_do_it(request);
                        if (!kvp.Key.Item1)
                        {
                            foreach (string id in kvp.Value)
                                all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] = 0;
                        }
                        foreach (DataRow row in temp_dt.Rows)
                        {
                            if (kvp.Key.Item1)
                            {
                                if (all_prods.ContainsKey(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)))
                                {
                                    foreach (string id in kvp.Value)
                                        all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] += all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)];
                                }
                                else
                                {
                                    request = "SELECT product_amount FROM product_on_store WHERE ID_product_store = '" + row.ItemArray[0].ToString() + "';";
                                    
                                    DataTable am = SQL_Helper.Just_do_it(request);
                                    if (am.Rows.Count > 0)
                                    {
                                        foreach (string id in kvp.Value)
                                            all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] += Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
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
                    //if (!kvp.Key.Item1)
                    //    foreach (string id in kvp.Value)
                    //        all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] = 1;

                    string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_bigger = '" + kvp.Key.Item1 + "';";
                    DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                    foreach (DataRow row2 in temp_dt2.Rows)
                    {
                        request = "SELECT ID_product FROM products WHERE type_little_name = '" + row2.ItemArray[0].ToString() + "';";
                        DataTable temp_dt = SQL_Helper.Just_do_it(request);
                        if (!kvp.Key.Item1)
                        {
                            foreach (string id in kvp.Value)
                                all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] = 0;
                        }
                        foreach (DataRow row in temp_dt.Rows)
                        {
                            if (kvp.Key.Item1)
                            {
                                if (all_prods.ContainsKey(new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)))
                                {
                                    foreach (string id in kvp.Value)
                                        all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] += all_prods[new Tuple<string, Group>(row.ItemArray[0].ToString(), Group.Product)];
                                }
                                else
                                {
                                    request = "SELECT product_amount FROM product_on_store WHERE ID_product_store = '" + row.ItemArray[0].ToString() + "';";
                                    DataTable am = SQL_Helper.Just_do_it(request);
                                    if (am.Rows.Count > 0)
                                    {
                                        foreach (string id in kvp.Value)
                                            all_prods[new Tuple<string, Group>(id, kvp.Key.Item2)] += Convert.ToDouble(am.Rows[0].ItemArray[0].ToString());
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
        /// <summary>
        /// Функция проведения самого анализа
        /// </summary>
        /// <param name="all_prods"></param>
        /// <param name="analiz_border"></param>
        /// <returns></returns>
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
                        all_sells.Add(id, new double[] { amount/prods.Rows.Count, sum });
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
          //  Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> catalog_prods = new Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>>();
            Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>>  catalog_prods = Get_prod_list(temp_abcxyz);
            myStopwatch2.Stop();
            return catalog_prods;
        }
        /// <summary>
        /// функция составления таблиц товаров
        /// </summary>
        /// <param name="temp_abcxyz"></param>
        /// <returns></returns>
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
        /// <summary>
        /// Функция сведения словарей в единый каталог
        /// </summary>
        /// <param name="temp_abcxyz"></param> Словари
        /// <returns></returns>
        public Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> Get_prod_list(List<Dictionary<string, Type_ABC_XYZ>> temp_abcxyz)
        {
            Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> catalog_prods = new Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>>();
            List<Group> g = new List<Group>() { Group.Product, Group.Brand, Group.Little_type, Group.Big_type };
            int j = 0;
            for (int i = 0; i < 8; i += 2)
            {
                foreach (string id in temp_abcxyz[i].Keys)
                {
                    Tuple < Type_ABC_XYZ, Type_ABC_XYZ > a = new Tuple < Type_ABC_XYZ, Type_ABC_XYZ >( temp_abcxyz[i][id], temp_abcxyz[i + 1][id] );
                    if (catalog_prods.ContainsKey(new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[j], a)))
                    {
                        catalog_prods[new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[j], a)].Add(id);
                    }
                    else
                    {
                        List<string> tl = new List<string>();
                        tl.Add(id);
                        catalog_prods.Add(new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[j], a), tl);
                    }
                }
                j++;
            }
            return catalog_prods;
        }


        //--------------------------------ОБЩЕЕ---------------------------------------------//
        /// <summary>
        /// Функция определения типа объекта в разрезе ABC XYZ анализа
        /// </summary>
        /// <param name="param"></param> - словарь, содержащий в себе объекты для анализа и значение, по которому будет определяться тип
        /// <param name="az"></param> пременная, которая определяет какой анализ мы используем - ABC или XYZ
        /// <returns></returns> - Словарь, содержащий в себе объекты и их тип
        public Dictionary<string, Type_ABC_XYZ> obj_Types(Dictionary<string, double> param, string az )
        {
            Dictionary<string, Type_ABC_XYZ> clients_types = new Dictionary<string, Type_ABC_XYZ>();
            double s1;
            double s2;
            Type_ABC_XYZ t3;
            Type_ABC_XYZ t2;
            Type_ABC_XYZ t1;

            double General = 0;
            foreach (string id in param.Keys)//считаем общее
            {
                General += param[id];
            } 

            if (az.Equals("a".ToLower()))
            {
                s1 = General * 0.05;
                s2 = General * 0.15;
                t1 = Type_ABC_XYZ.C;
                t2 = Type_ABC_XYZ.B;
                t3 = Type_ABC_XYZ.A;
            }
            else
            {
                s1 = General * 0.05;
                s2 = General * 0.20;
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
                double temp = 0;
                foreach (string id in param.Keys)
                {
                    temp += param[id];

                    if (0 <= temp && temp <= s1)
                    {
                        clients_types.Add(id, t1);
                        
                    }
                    if (s1 < temp && temp <= s2)
                    {
                        clients_types.Add(id, t2);
                    }
                    if (s2 < temp)
                    {
                        clients_types.Add(id, t3);
                    }

                }
            }

            return clients_types;
        }

        //--------------------------------Генерация----------------------------------------------//
        /// <summary>
        /// Функция сведения таблиц
        /// </summary>
        /// <param name="catalog_prods"></param>
        /// <param name="client_tabs"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public Dictionary<string, List<Final_product_group>> Get_summary_tables(Dictionary<Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>, List<string>> catalog_prods, List<Client_Tab> client_tabs)
        {
            Table_for_strategy[,] clients = gs.promo_type.Get_clients();//двумерный массив АВС-XYZ клиентов
            Table_for_strategy[,] products = gs.promo_type.Get_products();//двумерный массив АВС-XYZ товаров
            Dictionary<string, List<Final_product_group>> summary = new Dictionary<string, List<Final_product_group>>(); //сводная коллекция, где для каджого клиента есть Несколько листов продуктов, с различающимися параметрами приоритета
            rtb.Text += "   Начат процесс создания сводных таблиц\n ";
            rtb.Refresh();
            foreach (Client_Tab client in client_tabs)
            {
                Type_ABC_XYZ abc = client.Get_ABC();
                Type_ABC_XYZ xyz = client.Get_XYZ(); //тип клиента
                Table_for_strategy table_strat_client = Get_client_or_prod(abc, xyz, clients); //строка из матрицы стратегий для этого клиента
                if (table_strat_client.Get_prob_of_discount_for().Count == 0)   
                {
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
                       
                        if (catalog_prods.ContainsKey(new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[i], type)))
                        {
                            List<string> temp = catalog_prods[new Tuple<Group, Tuple<Type_ABC_XYZ, Type_ABC_XYZ>>(g[i], type)];
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
            }
            rtb.Text += "   Сводные таблицы собраны \n ";
            rtb.Refresh();
            return summary;
        }
      
        
        
        /// <summary>
       /// функция получения таблиц стратегии
       /// </summary>
       /// <param name="abc"></param>
       /// <param name="xyz"></param>
       /// <param name="custOrProd"></param>
       /// <returns></returns>
        public Table_for_strategy Get_client_or_prod(Type_ABC_XYZ abc, Type_ABC_XYZ xyz, Table_for_strategy[,] custOrProd)
        {
            int i = (abc == Type_ABC_XYZ.A) ? 0 : ((abc == Type_ABC_XYZ.B) ? 1 : 2);
            int j = (xyz == Type_ABC_XYZ.X) ? 0 : ((xyz == Type_ABC_XYZ.Y) ? 1 : 2);
            return custOrProd[i, j];
        }


        /// <summary>
        /// Функуия получения сравниваемых листов
        /// </summary>
        /// <param name="summary"></param>
        /// <param name="diction_sum_value"></param>
        /// <param name="client_sells"></param>
        /// <param name="all_prods_and_group_amount_on_store"></param>
        /// <returns></returns>
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
                    temp_list.compType = gs.promo_type.comp_prod;
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
                        temp_list.compType = gs.promo_type.comp_client;
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
        


        /// <summary>
        /// Функция генерации предложений
        /// </summary>
        /// <param name="d"></param>
        /// <param name="gs"></param>
        /// <returns></returns>
        public Dictionary<string, List<Promo>> Generate(Object[] d)
        {
            rtb.Text += "   Начато наложение пространтв \n ";
            rtb.Refresh();   
            Dictionary<Tuple<string, Group>, Product_for_list_shop> comparable_lists_goods = (Dictionary<Tuple<string, Group>, Product_for_list_shop>)d[1];
            Dictionary<string, List<Product_for_list_client>> comparable_lists_clients = (Dictionary<string, List<Product_for_list_client>>)d[0];
            Random r = new Random();
            comparable_lists_clients = comparable_lists_clients.OrderByDescending(pair => pair.Value.Count).ToDictionary(pair => pair.Key, pair => pair.Value);
            List<string> s = new List<string>(comparable_lists_clients.Keys);
            int[] discount = new int[2] { gs.min_discount, gs.max_discount };
            Dictionary<string, List<Promo>> promos = new Dictionary<string, List<Promo>>();


            foreach (string client in comparable_lists_clients.Keys)
            {
              //  Dictionary<Tuple<string, Group>, Product_for_list_shop> temp_goods = new Dictionary<Tuple<string, Group>, Product_for_list_shop>();
                //для каждого товара в списке клиента ищем соотвтсвие в списке товаров
                int count  = 0;
                foreach (Product_for_list_client pc in comparable_lists_clients[client])
                {
                   count++;
                }

                if (count == 0 || comparable_lists_clients[client].Count == 0) continue;

                // List<Product_for_list_client> temp_client_goods = comparable_lists_clients[client].OrderByDescending(pair => pair.sum).ToList();
                List<Product_for_list_client> temp_client_goods = comparable_lists_clients[client];
                temp_client_goods.Sort();
                if (gs.promo_type.intresting_cl)
                    temp_client_goods.Reverse();
                //temp_goods.OrderByDescending()
               //  temp_goods = temp_goods.OrderByDescending(pair => pair.Value.sum).ToDictionary(pair => pair.Key, pair => pair.Value);
                
                List<Promo> temp_p = new List<Promo>();
                promos.Add(client, temp_p);
                double disc_spread = discount[1] - discount[0];

                double disc_step = disc_spread / 3;

                Dictionary<Tuple<string, Group>, double> prods_c = new Dictionary<Tuple<string, Group>, double>();//товары - скидка
                Dictionary<double, List<Tuple<string, Group>>> prods_s = new Dictionary<double, List<Tuple<string, Group>>>();//приоритет - товар (только товары для клиента)
                Dictionary<double, List <Tuple<string, Group>>>  distribution_full = new Dictionary<double, List<Tuple<string, Group>>>();//вероятность - товар
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
                foreach (double prob in distribution_full.Keys)//получаем сколько скидок каждого типа получим
                {
                    int h = 0;
                    if (distribution_full.Keys.Count == 1)
                    {
                        h = 5;
                    }
                    else
                    {
                        int hh = distribution_full[prob].Count();
                        h = Convert.ToInt32(hh * (prob / 100));
                        if (h < 5) h = 5;
                    }
                    distribution_for_disc.Add(prob, h);
                }

                //соберем список по совместному приоритету (например сумма и приоритет)
                Dictionary<Tuple<string, Group>, Tuple<double, double>> full_prior = new Dictionary<Tuple<string, Group>, Tuple<double, double>>();
                
                foreach(Tuple<string, Group> list_p in comparable_lists_goods.Keys)
                {
                    foreach (Product_for_list_client k in temp_client_goods)
                    {
                        {
                            if ((list_p.Item1.Equals(k.prod_id)) && (list_p.Item2.Equals(k.g)))
                                {
                                double field_p = 0;
                                if (gs.promo_type.comp_prod == CompareType.cost)
                                {
                                    field_p = comparable_lists_goods[list_p].sum;
                                }
                                else if (gs.promo_type.comp_prod == CompareType.amount)
                                {
                                    field_p = comparable_lists_goods[list_p].amount_on_store;
                                }
                                else
                                {
                                    field_p = comparable_lists_goods[list_p].sell_value;
                                }
                                double field_c = 0;
                                if (gs.promo_type.comp_client == CompareType.cost)
                                {
                                    field_c = k.sum;
                                }
                                else if (gs.promo_type.comp_client == CompareType.purchase_value)
                                {
                                    field_c = k.purchase_value;
                                }

                                full_prior.Add(list_p, new Tuple<double, double>(field_p, field_c));
                                continue;
                            }
                        }
                    }
                    }
                
               

                if (gs.promo_type.intresting_pr)
                     full_prior = full_prior.OrderByDescending(pair => pair.Value.ToValueTuple()).ToDictionary(pair => pair.Key, pair => pair.Value);
                else
                    full_prior = full_prior.OrderBy(pair => pair.Value.ToValueTuple()).ToDictionary(pair => pair.Key, pair => pair.Value);
                Dictionary<Tuple<string, Group>, Tuple<double, double>> temp_prior = new Dictionary<Tuple<string, Group>, Tuple<double, double>>(full_prior);
                ///дополнительные плюшки
                double di = 10;
                Random r1 = new Random();
                List<string> lt = new List<string>();
                List<string> b = new List<string>();
                for (int l = 0; l < 5; l++)
                {
                    Tuple<string, Group> key = temp_prior.First().Key;
                    temp_prior.Remove(key);
                    if (key.Item2 == Group.Product)
                    {
                        if (r1.Next(0, 2) == 0)
                        {
                            
                            string request = "SELECT type_little_name FROM products WHERE ID_product = '" + key.Item1 + "';";
                            DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                            request = "SELECT ID_product_type_little FROM product_type_little WHERE name_product_type_little = '" + temp_dt2.Rows[0].ItemArray[0].ToString() + "';";
                            temp_dt2 = SQL_Helper.Just_do_it(request);
                            if (!lt.Contains(temp_dt2.Rows[0].ItemArray[0].ToString()))
                            {
                                full_prior.Remove(key);
                                temp_p.Add(new Promo(temp_dt2.Rows[0].ItemArray[0].ToString(), Group.Little_type, di));
                                di--;
                            }
                            
                            
                        }
                        else
                        {
                            string request = "SELECT brand_ID FROM products WHERE ID_product = '" + key.Item1 + "';";
                            DataTable temp_dt2 = SQL_Helper.Just_do_it(request);
                            if (!b.Contains(temp_dt2.Rows[0].ItemArray[0].ToString()))
                            {
                                full_prior.Remove(key);
                                temp_p.Add(new Promo(temp_dt2.Rows[0].ItemArray[0].ToString(), Group.Brand, di));
                                di--;
                            }
                            
                            di--;

                        }
                    }
                    if (temp_prior.Count < 1)
                        break;

                }

                foreach (double prob in distribution_for_disc.Keys)//для кадой вероятносит
                {
                    List<Tuple<string, Group>> prods = distribution_full[prob];//список товаров с этой вероятностью
                    for (int i = 0; i < distribution_for_disc[prob]; i++) //кол-во товаров с этой вероятностью
                    {
                        Tuple<string, Group> delete_this = new Tuple<string, Group>("", Group.Product);
                        foreach(Tuple<string, Group> prod in full_prior.Keys)
                        {
                            if (prods.Contains(prod))
                            {
                                double disc = discount[0] + disc_step * prods_c[prod];
                                double spesial_step = 10*((double)i / distribution_for_disc[prob]);
                                disc = Convert.ToInt32(disc - spesial_step);
                                if (disc < discount[0]) disc = discount[0];
                                temp_p.Add(new Promo(prod.Item1, prod.Item2, disc));
                                delete_this = prod;
                                break;
                            }
                        }
                        full_prior.Remove(delete_this);

                    }
                }


            }


            rtb.Text += "   Закончено наложение пространтв \n ";
            rtb.Refresh();
            return promos;
        }

        //--------------------------------отправка----------------------------------------------//

            /// <summary>
            /// Функция генерации писем
            /// </summary>
            /// <param name="promos"></param>
            /// <param name="gs"></param>
            /// <param name="test"></param>
        public void Generate_mails(Dictionary<string, List<Promo>> promos, bool test, string email_shop, string password)
        {
            rtb.Text += "Начат процесс отправки предложений \n ( ВНИМАНИЕ, ПРОЦЕСС МОЖЕТ ЗАНЯТЬ ДОЛГОЕ ВРЕМЯ )\n ";
            rtb.Refresh();
            int border = promos.Count();
            if (test) border = 5;
            int id_promo = SQL_Helper.HowMuchRows("promo_info", "ID_promo") + 1;
            Dictionary<string, string> value = new Dictionary<string, string>();
            value.Add("ID_promo", id_promo.ToString());
            value.Add("discount_date_start", gs.start.ToString("u").Replace("Z", ""));
            value.Add("discount_date_finish", gs.end.ToString("u").Replace("Z", ""));
            string s = "";
            foreach (String sp in gs.shops)
            {
                s += sp + " ";
            }
            Code128 code128 = new Code128();
            string path = Directory.GetCurrentDirectory();
            string[] d = Directory.GetDirectories(path);
            path += "\\promos\\promo" + DateTime.Now.ToString().Replace(':', '.');
            value.Add("IDs_shops_list_promo", s);
            SQL_Helper.WriteInTable("promo_info", value);
            prBar.Maximum = border;
            prBar.Value = 0;
            for (int i_p = 0; i_p < border; i_p++)
            {
                string client = promos.First().Key;

                List<Promo> promo = promos.First().Value;
                string text = "<h2 align=\"center\">Дорогой клиент, мы рады, что вы с нами!</h2>";
                string request = "SELECT FIO_customer, email_customer FROM customers WHERE ID_customer = '" + client + "';";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                string mail = temp_dt.Rows[0].ItemArray[1].ToString();
                string name = temp_dt.Rows[0].ItemArray[0].ToString();
                text += "<p align=\"center\"> И специально для вас, <b>" + name + "</b>, мы подготовили " + promo.Count + " уникальных предложений </p>";
                text += "<p align=\"center\"> Что бы воспользоваться ими - просто покажите на кассе эти <b>штрихкоды!</b></p>";
                string Shops = "";
                foreach (String sh in gs.shops)
                {
                    string re = "SELECT shop_address, shop_city FROM shops WHERE ID_shop = '" + (Convert.ToInt32(sh)+1).ToString() + "';";
                    temp_dt = SQL_Helper.Just_do_it(re);
                    Shops += "Город " + temp_dt.Rows[0].ItemArray[1].ToString() + " улица " + temp_dt.Rows[0].ItemArray[0].ToString() + ", ";
                }
                Shops = Shops.Substring(0, Shops.Length - 1);
                text += "<tr><td colspan=\"2\"><p align=\"center\">" + "Ждем вас в магазинах по адресам: " + Shops + "</p></td></tr></table>";
                text += "<tr><td colspan=\"2\"><p align=\"center\">" + "C " + gs.start.ToString("d") + " по " + gs.end.ToString("d") + "</p></td></tr></table>";
                text += "<p align=\"center\"> Если письмо не отображается целиком - скачайте полную версию <b>Во вложении</b> в удобном для вас формате</p>";
                Directory.CreateDirectory(path);
                System.Drawing.Imaging.ImageFormat image_format = System.Drawing.Imaging.ImageFormat.Bmp;
                int code = 0;
                foreach (Promo p in promo)
                {
                    code++;
                    int discount = (int)p.disc;
                    string image = "";
                    string id_type = "4";
                    switch (p.group)
                    {
                        case Group.Product:
                            {
                                string prod_name = " ";
                                request = "SELECT product_name, image_prod, type_little_name, brand_ID FROM products WHERE ID_product = '" + p.id_prod + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                prod_name = temp_dt.Rows[0].ItemArray[0].ToString();
                                image = temp_dt.Rows[0].ItemArray[1].ToString();
                                string little_type = temp_dt.Rows[0].ItemArray[2].ToString();
                                string big_type = "";
                                request = "SELECT Image_brand, brand_name FROM brands WHERE ID_brand = '" + temp_dt.Rows[0].ItemArray[3].ToString() + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                string brand = temp_dt.Rows[0].ItemArray[1].ToString();
                                if (image.Length < 1)
                                    image = temp_dt.Rows[0].ItemArray[0].ToString();

                                request = "SELECT image_little_type, ID_product_type_bigger FROM product_type_little WHERE name_product_type_little = '" + little_type + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                if (image.Length < 1)
                                    image = temp_dt.Rows[0].ItemArray[0].ToString();
                                request = "SELECT image_big_type, name_product_type_big FROM product_type_big WHERE ID_product_type_big = '" + temp_dt.Rows[0].ItemArray[1].ToString() + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                if (image.Length < 1)
                                    image = temp_dt.Rows[0].ItemArray[0].ToString();

                                big_type = temp_dt.Rows[0].ItemArray[1].ToString();
                                if (image.Length < 1)
                                {
                                    image = Get_string_img("no_image.png");
                                }

                                string input = p.group + " " + p.id_prod + " " + discount + "% " + client + "  " + id_promo;
                                string this_path = path + "\\" + input + ".bmp";

                                this_path = Save_code123(input, this_path, image_format, code128);

                                String imgString = Get_string_img(this_path);
                                text += "<table align=\"center\" width=80% >";
                                text += "<tr><td width=50%  ><img src=\"" + image + "\" width=300 ></td><td width=50%><img src=\"" + imgString + "\" width=300 ></td></tr>";
                                String[] prod_n = prod_name.Split('_');
                                String all_text = "Скидка " + discount + "% На товар брэнда " + brand.Replace('-', ' ') + ": ";
                                for (int i = 2; i < prod_n.Length; i++)
                                {
                                    all_text += prod_n[i].Replace('-', ' ') + " ";
                                }
                                //Cinzano-Spumante_Венгрия_Шампанское_Белое_Сухое_1л
                                text += "<tr><td colspan=\"2\"><p align=\"center\">" + all_text + "</p></td></tr></table>";

                                break;
                            }
                        case Group.Brand:
                            {
                                id_type = "3";
                                request = "SELECT Image_brand, brand_name, brand_counrty FROM brands WHERE ID_brand = '" + p.id_prod + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                string brand = temp_dt.Rows[0].ItemArray[1].ToString();
                                image = temp_dt.Rows[0].ItemArray[0].ToString();
                                if (image.Length < 1)
                                {
                                    image = Get_string_img("no_image.png");
                                }
                                string input = p.group + " " + p.id_prod + " " + discount + "% " + client;
                                string this_path = path + "\\" + input + ".bmp";
                                this_path = Save_code123(input, this_path, image_format, code128);
                                String imgString = Get_string_img(this_path);
                                text += "<table align=\"center\" width=80% >";
                                text += "<tr><td width=50%  ><img src=\"" + image + "\" width=300 ></td><td width=50%><img src=\"" + imgString + "\" width=300 ></td></tr>";
                                String all_text = "Скидка " + discount + "% На весь товар брэнда " + brand.Replace('-', ' ') + " производства " + temp_dt.Rows[0].ItemArray[2].ToString().Replace('-', ' ');
                                text += "<tr><td colspan=\"2\"><p align=\"center\">" + all_text + "</p></td></tr></table>";
                                break;
                            }
                        case Group.Little_type:
                            {
                                id_type = "3";
                                request = "SELECT name_product_type_little, image_little_type, ID_product_type_bigger FROM product_type_little WHERE ID_product_type_little = '" + p.id_prod + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                string little_typ = temp_dt.Rows[0].ItemArray[0].ToString();
                                image = temp_dt.Rows[0].ItemArray[1].ToString();
                                string big_type = "";
                                request = "SELECT image_big_type, name_product_type_big FROM product_type_big WHERE ID_product_type_big = '" + temp_dt.Rows[0].ItemArray[2].ToString() + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                if (image.Length < 1)
                                    image = temp_dt.Rows[0].ItemArray[0].ToString();
                                big_type = temp_dt.Rows[0].ItemArray[1].ToString();
                                if (image.Length < 1)
                                {
                                    image = Get_string_img("no_image.png");
                                }

                                string input = p.group + " " + p.id_prod + " " + discount + "% " + client;
                                string this_path = path + "\\" + input + ".bmp";

                                this_path = Save_code123(input, this_path, image_format, code128);

                                String imgString = Get_string_img(this_path);
                                text += "<table align=\"center\" width=80% >";
                                text += "<tr><td width=50%  ><img src=\"" + image + "\" width=300 ></td><td width=50%><img src=\"" + imgString + "\" width=300 ></td></tr>";
                                String all_text = "Скидка " + discount + "% На все товары категории " + little_typ.Replace('-', ' ') + " из отдела " + big_type.Replace('-', ' ');
                                //Cinzano-Spumante_Венгрия_Шампанское_Белое_Сухое_1л
                                text += "<tr><td colspan=\"2\"><p align=\"center\">" + all_text + "</p></td></tr></table>";
                                break;
                            }
                        case Group.Big_type:
                            {
                                id_type = "1";
                                string big_type = "";
                                request = "SELECT image_big_type, name_product_type_big FROM product_type_big WHERE ID_product_type_big = '" + p.id_prod + "';";
                                temp_dt = SQL_Helper.Just_do_it(request);
                                if (image.Length < 1)
                                    image = temp_dt.Rows[0].ItemArray[0].ToString();
                                big_type = temp_dt.Rows[0].ItemArray[1].ToString();
                                if (image.Length < 1)
                                {
                                    image = Get_string_img("no_image.png");
                                }

                                string input = p.group + " " + p.id_prod + " " + discount + "% " + client;
                                string this_path = path + "\\" + input + ".bmp";
                                this_path = Save_code123(input, this_path, image_format, code128);
                                String imgString = Get_string_img(this_path);
                                text += "<table align=\"center\" width=80% >";
                                text += "<tr><td width=50%  ><img src=\"" + image + "\" width=300 ></td><td width=50%><img src=\"" + imgString + "\" width=300 ></td></tr>";
                                String all_text = "Скидка " + discount + "% На все товары отдела " + big_type.Replace('-', ' ');
                                //Cinzano-Spumante_Венгрия_Шампанское_Белое_Сухое_1л
                                text += "<tr><td colspan=\"2\"><p align=\"center\">" + all_text + "</p></td></tr></table>";
                                break;
                            }
                    }
                    string write = "INSERT INTO promo_full (ID_promo_full,ID_customer_dis,ID_type_group,ID_product_dis, discount , CODE, used) " +
                        "VALUES('" + id_promo + "','" + client + "','" + id_type + "','" + p.id_prod + "','" + p.disc + "','" + id_promo+client+code.ToString() + "','" + 0 + "');";
                    SQL_Helper.Just_do_it(write);
                  
                }
                string attach = path + "\\" + client + " " + name;
                string attach_html = attach + ".html";
                System.IO.StreamWriter wrt = new StreamWriter(attach_html);
                wrt.WriteLine(text);
                wrt.Close();

                //string name_f = client + " " + name + ".pdf";
                string attach_pdf = Get_pdf(text, attach);
                Mail_it(text, mail, attach_pdf, attach_html, email_shop, password);
                promos.Remove(client);
                prBar.Value++;
            }
            
            rtb.Text += "Закончен процесс отправки предложений\n ";
            rtb.Refresh();
        }


        
        /// <summary>
        /// Функция отсылки писем
        /// </summary>
        /// <param name="text"></param>
        /// <param name="mail"></param>
        /// <param name="attach_pdf"></param>
        /// <param name="attach_html"></param>
        public void Mail_it(string text, string mail, string attach_pdf, string attach_html, string email_shop, string password)
        {
            // отправитель - устанавливаем адрес и отображаемое в письме имя
            //
            MailAddress from = new MailAddress(email_shop, "Торговая сеть N");
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
            m.Attachments.Add(new Attachment(attach_html));
            m.Attachments.Add(new Attachment(attach_pdf));
            // адрес smtp-сервера и порт, с которого будем отправлять письмо
            SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
            // логин и пароль
            smtp.Credentials = new NetworkCredential(email_shop, password);
            smtp.EnableSsl = true;
            smtp.Send(m);
            Console.Read();
        }



        /// <summary>
        /// Функция наложения подписи на штрихкод
        /// </summary>
        /// <param name="originalImage"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        private Bitmap DrawWatermark(Bitmap originalImage, string text)
        {
            Bitmap bitmap = new Bitmap(originalImage.Width, originalImage.Height);
            using (Graphics gr = Graphics.FromImage(bitmap))
            {
                gr.DrawImage(originalImage, new System.Drawing.Rectangle(0, 0, originalImage.Width, originalImage.Height));

                float xText = originalImage.Width;
                float yText = 20;
                gr.FillRectangle(new SolidBrush(Color.White), 0, originalImage.Height-20, xText, yText);
                gr.DrawString(text, new System.Drawing.Font("Segoe UI", 11, FontStyle.Bold), new SolidBrush(Color.Black), 40, originalImage.Height-20);
               // gr.DrawString(text, new Font("Segoe UI", fontSize, FontStyle.Bold), new SolidBrush(Color.DodgerBlue), xText, yText);
                return bitmap;
            }
        }



        /// <summary>
        /// Функция превращения локального изображения в массив байтов
        /// </summary>
        /// <param name="this_path"></param>
        /// <returns></returns>
        private String Get_string_img(String this_path)
        {
            System.Drawing.Image temp = System.Drawing.Image.FromFile(this_path);
            System.Drawing.ImageConverter converter = new ImageConverter();
            String imgString = Convert.ToBase64String((byte[])converter.ConvertTo(temp, typeof(byte[])));
            imgString = "data:image/png;base64," + imgString;
            return imgString;
        }



        /// <summary>
        /// функция сохранения штрих-кода
        /// </summary>
        /// <param name="input"></param>
        /// <param name="this_path"></param>
        /// <param name="image_format"></param>
        /// <returns></returns>
        private String Save_code123(String input, String this_path, System.Drawing.Imaging.ImageFormat image_format, Code128 c)
        {
            string s = c.Get_code(input);
            Bitmap image_this = c.get_img();
            Bitmap strih_t = new Bitmap(image_this, new Size(image_this.Width, 120));
            Bitmap strih2 = DrawWatermark(strih_t, input);
            float f = 0.013f;
            strih2.SetResolution(f, f);
            strih2.Save(this_path, image_format);
            return this_path;
        }


        /// <summary>
        /// функция генерации пдф
        /// </summary>
        /// <param name="text"></param>
        /// <param name="attach"></param>
        /// <returns></returns>
        private string Get_pdf(string text, string attach)
        {
            //string AppPatch = Application.StartupPath;
           // string HTMLName = attach + ".html";
            string HTMLPatch = attach + ".html";   
            string PDFPatch = attach + ".pdf";
            string HTMLBody = text; // сам текст html храню в ресурсах, оттуда и пользую
            //Write text to file
            StreamWriter streamwriter = new StreamWriter(HTMLPatch);
            streamwriter.WriteLine(HTMLBody);
            streamwriter.Close();
            //end write text
            // instantiate the html to pdf converter
            HtmlToPdf converter = new HtmlToPdf();
            // convert the url to pdf
            //SelectPdf.PdfDocument doc = converter.ConvertUrl(HTMLPatch);
            SelectPdf.PdfDocument doc = converter.ConvertHtmlString(text);
            // save pdf document
            doc.Save(PDFPatch);
            // close pdf document
            doc.Close();

            return PDFPatch;
        }
    }
}

