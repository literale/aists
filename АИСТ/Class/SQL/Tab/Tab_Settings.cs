using Google.Protobuf;
using Renci.SshNet.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using АИСТ.Class.enums;
using АИСТ.Class.SQL.Tab;

namespace АИСТ.Class
{
    static class Tab_Settings
    {
        private static string email;
        private static Dictionary<string, Tabs> tabs_from_file = new Dictionary<string, Tabs>();
        public static Dictionary<string, Tab> tabs = new Dictionary<string, Tab>();

        public static void set_email()
        {
            email = "ESGdiplom2020shop@yandex.ru";
        }
        public static void Load_info()
        {
            Dictionary<string, Tabs> tabs = Info.Get_tabs();
            List<string> t = Info.get_bd();
            for (int i = 0; i< t.Count; i++)
            {
                if (t[i].Contains("Tstart"))
                {
                    i++;
                    string name = t[i];
                    Tabs tab = new Tabs(name.Split(' ')[2]);
                    i+=2;
                    while (!t[i].Contains("Tend"))
                    {
                       tab.Add_fields(t[i].Split(' ')[0], t[i].Split(' ')[2]);

                        i++;
                    }
                    tabs_from_file[name.Split(' ')[0]] = tab;
                    // tabs_from_file.Add(name.Split(' ')[0], tab);
                }
            }
            Info.Set_tabs(tabs);
        }
        public static Dictionary<string, Tabs> Get_tabs()
        {
            return tabs_from_file;
        }
        public static Tabs Get_tab(string tab)
        {
            return tabs_from_file[tab];
        }

        public static void Put_in_class()
        {
            Load_info();
            foreach (string tab in tabs_from_file.Keys)
            {
                Tabs t = tabs_from_file[tab];
                Dictionary<string, string> f = t.Get_fielsd();
                if (tab.Equals(Tab_names.product.ToString()))
                {
                    Tab_products tb = new Tab_products(t.Get_name(), f["id"], f["name"], f["type"], f["brand_id"], f["product_cost"], f["image"]);                    
                    tabs[Tab_names.product.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.users.ToString()))
                {
                    Tab_users tb = new Tab_users(t.Get_name(), f["id"], f["FIO"], f["login"], f["password"], f["email"], f["ID_users_settings"]);
                    tabs[Tab_names.users.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.brands.ToString()))
                {
                    Tab_brands tb = new Tab_brands(t.Get_name(), f["ID_brand"], f["brand_name"], f["brand_counrty"], f["Image_brand"]);
                    tabs[Tab_names.brands.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.checks.ToString()))
                {
                    Tab_checks tb = new Tab_checks(t.Get_name(), f["ID_check"], f["ID_shop_check"], f["check_date"], f["ID_customer_check"], f["check_total_sum"]);
                    tabs[Tab_names.checks.ToString()]= tb;
                }
                else if (tab.Equals(Tab_names.customers.ToString()))
                {
                    Tab_customers tb = new Tab_customers(t.Get_name(), f["ID_customer"], f["FIO_customer"], f["email_customer"]);
                    tabs[Tab_names.customers.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.history.ToString()))
                {
                    Tab_history tb = new Tab_history(t.Get_name(), f["ID_check_history"], f["produc_ID_history"], f["product_amount"], f["product_price"]);
                    tabs[Tab_names.history.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.key.ToString()))
                {
                    Tab_key tb = new Tab_key(t.Get_name(), f["ID_key"], f["key"], f["IV"]);
                    tabs[Tab_names.key.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.product_on_store.ToString()))
                {
                    Tab_product_on_store tb = new Tab_product_on_store(t.Get_name(), f["ID_product_store"], f["ID_shop_store"], f["product_amount"], f["product_price"], f["last_shipment"]);
                    tabs[Tab_names.product_on_store.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.product_type_big.ToString()))
                {
                    Tab_product_type_big tb = new Tab_product_type_big(t.Get_name(), f["ID_product_type_big"], f["name_product_type_big"], f["image_big_type"]);
                    tabs[Tab_names.product_type_big.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.product_type_little.ToString()))
                {
                    Tab_product_type_little tb = new Tab_product_type_little(t.Get_name(), f["ID_product_type_little"], f["ID_product_type_bigger"], f["name_product_type_little"], f["image_little_type"]);
                    tabs[Tab_names.product_type_little.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.promo_full.ToString()))
                {
                    Tab_promo_full tb = new Tab_promo_full(t.Get_name(), f["ID_promo_full"], f["ID_customer_dis"], f["ID_type_group"], f["ID_product_dis"],
                        f["CODE"], f["discount"], f["used"], f["image_pomo"], f["IDs_shops_list"]);
                    tabs[Tab_names.promo_full.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.promo_history.ToString()))
                {
                    Tab_promo_history tb = new Tab_promo_history(t.Get_name(), f["CODE_promo_hist"], f["product_count_promo_his"], f["ID_shop_promo_his"]);
                    tabs[Tab_names.promo_history.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.promo_info.ToString()))
                {
                    Tab_promo_info tb = new Tab_promo_info(t.Get_name(), f["ID_promo"], f["discount_date_start"], f["discount_date_finish"], f["IDs_shops_list_promo"]);
                    tabs[Tab_names.promo_info.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.settings.ToString()))
                {
                    Tab_settings tb = new Tab_settings(t.Get_name(), f["ID_settings"], f["host"]);
                    tabs[Tab_names.settings.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.shops.ToString()))
                {
                    Tab_shops tb = new Tab_shops(t.Get_name(), f["ID_shop"], f["shop_address"], f["shop_city"]);
                    tabs[Tab_names.shops.ToString()] = tb;
                }
                else if (tab.Equals(Tab_names.type_group.ToString()))
                {
                    Tab_type_group tb = new Tab_type_group(t.Get_name(), f["id_type_group"], f["name_type_group"]);
                    tabs[Tab_names.type_group.ToString()] = tb;
                }

            }
        }


    }

}





