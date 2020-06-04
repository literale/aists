using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace АИСТ.Class
{
    static class Tab_Settings
    {
        private static string email;
        private static Dictionary<string, Tabs> all_tabs = new Dictionary<string, Tabs>();
        public static void Temp_set_tabs()
        {
            all_tabs.Add("products", new Tabs("products"));
            Dictionary<string, string> fields = new Dictionary<string, string>();
            fields.Add("ID_product", "ID_product");
            fields.Add("product_name", "product_name");
            fields.Add("type_little_name", "type_little_name");
            fields.Add("brand_ID", "brand_ID");
            fields.Add("product_cost", "product_cost");
            fields.Add("image_prod", "image_prod");
            all_tabs["products"].Set_fields(fields);

            all_tabs.Add("brands", new Tabs("brands"));
            fields = new Dictionary<string, string>();
            fields.Add("ID_brand", "ID_brand");
            fields.Add("brand_name", "brand_name");
            fields.Add("brand_counrty", "brand_counrty");
            fields.Add("Image_brand", "Image_brand");
            all_tabs["brands"].Set_fields(fields);

            all_tabs.Add("big_type", new Tabs("product_type_big"));
            all_tabs.Add("little_type", new Tabs("product_type_little"));
            all_tabs.Add("shops", new Tabs("shops"));
            all_tabs.Add("store", new Tabs("product_on_store"));
            all_tabs.Add("clients", new Tabs("customers"));
            all_tabs.Add("check", new Tabs("checks"));
            all_tabs.Add("history", new Tabs("history"));
            all_tabs.Add("promo", new Tabs("promo_full"));
            all_tabs.Add("promo_info", new Tabs("promo_info"));
            all_tabs.Add("promo_history", new Tabs("promo_history"));
            all_tabs.Add("groups", new Tabs("type_group"));

            fields.Add("", "");
        }
        public static void temp_set_email()
        {
            email = "ESGdiplom2020shop@yandex.ru";
        }

        //public void Temp_set_fields()
        //{
        //    foreach(String tab in all_tabs.Keys)
        //    {
        //        switch (tab)
        //        {
        //            case "products":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }
        //            case "":
        //                {
        //                    Tabs t = all_tabs[tab];
        //                    Dictionary<string, string> fields = t.Get_fielsd();
        //                    fields.Add("", "");
        //                    t.Set_fields(fields);
        //                    break;
        //                }


        //        }
        //    }
        //}

        public static void Temp_set_fields_auto()
        {
            foreach (String tab in all_tabs.Keys)
            {
                Tabs t = all_tabs[tab];
                Dictionary<string, string> fields = t.Get_fielsd();
                string name = t.Get_name();
                DataTable temp_dtable = SQL_Helper.Get_columns(name);
               // List<string> columns = new List<string>();
                foreach (DataRow rows in temp_dtable.Rows)
                {
                    object[] shop_string = rows.ItemArray;
                    fields.Add(shop_string[0].ToString(), shop_string[0].ToString());
                }
                t.Set_fields(fields);
            }
        }

        public static Dictionary<string, Tabs> Get_tabs()
        {
            return all_tabs;
        }
    }
}
