using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace АИСТ.Class.SQL.Tab
{
    abstract class Tab
    {
        public Dictionary<string, string> fields = new Dictionary<string, string>();
        public string tab_name { get; set; }
    }
    // fields = new Dictionary<string, string> {{ "tab_name", tab_name },  { "", }, { "", }, { "", },{ "", },{ "", },{ "", },{ "", },{ "", },};
    class Tab_products : Tab
    {
        public Tab_products(string tab_name, string id, string name, string type, string brand_id, string product_cost, string image)
        {
            this.tab_name = tab_name;
            this.id = id;
            this.name = name;
            this.type = type;
            this.brand_id = brand_id;
            this.product_cost = product_cost;
            this.image = image;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "id", id }, { "name", name }, { "type", type }, { "brand_id", brand_id }, { "product_cost", product_cost }, { "image", image } };

        }

        public string id { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string brand_id { get; set; }
        public string product_cost { get; set; }
        public string image { get; set; }

    }

    class Tab_users : Tab
    {
        public Tab_users(string tab_name, string id, string fIO_user, string login, string password, string email, string iD_users_settings)
        {
            this.tab_name = tab_name;
            this.id = id;
            FIO = fIO_user;
            this.login = login;
            this.password = password;
            this.email = email;
            ID_users_settings = iD_users_settings;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "id", id }, { "FIO", FIO }, { "login", login }, { "password", password }, { "email", email }, { "ID_users_settings", iD_users_settings }};
        }

        public string id { get; set; }
        public string FIO { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string ID_users_settings { get; set; }
    }

    class Tab_brands : Tab
    {
        public Tab_brands(string tab_name, string ID_brand, string brand_name, string brand_counrty, string image_brand)
        {
            this.tab_name = tab_name;
            this.ID_brand = ID_brand;
            this.brand_name = brand_name;
            this.brand_counrty = brand_counrty;
            Image_brand = image_brand;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_brand", ID_brand }, { "brand_name", brand_name }, { "brand_counrty", brand_counrty }, { "Image_brand", image_brand }};
        }

        public string ID_brand { get; set; }
        public string brand_name { get; set; }
        public string brand_counrty { get; set; }
        public string Image_brand { get; set; }
}
    class Tab_checks : Tab
    {
        public Tab_checks(string tab_name, string iD_check, string iD_shop_check, string check_date, string iD_customer_check, string check_total_sum)
        {
            this.tab_name = tab_name;
            ID_check = iD_check;
            ID_shop_check = iD_shop_check;
            this.check_date = check_date;
            ID_customer_check = iD_customer_check;
            this.check_total_sum = check_total_sum;
            fields = new Dictionary<string, string> { { "ID_check", ID_check }, { "ID_shop_check", ID_shop_check }, { "check_date", check_date }, { "ID_customer_check", ID_customer_check }, { "check_total_sum", check_total_sum }};
        }

        public string ID_check { get; set; }
        public string ID_shop_check { get; set; }
        public string check_date { get; set; }
        public string ID_customer_check { get; set; }
        public string check_total_sum { get; set; }
    }
    class Tab_customers : Tab
    {
        public Tab_customers(string tab_name, string iD_customer, string fIO_customer, string email_customer)
        {
            this.tab_name = tab_name;
            this.ID_customer = iD_customer;
            this.FIO_customer = fIO_customer;
            this.email_customer = email_customer;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_customer", ID_customer }, { "FIO_customer", FIO_customer }, { "email_customer", email_customer }};
        }

        public string ID_customer { get; set; }
        public string FIO_customer { get; set; }
        public string email_customer { get; set; }
    }
    class Tab_history : Tab
    {
        public Tab_history(string tab_name, string iD_check_history, string produc_ID_history, string product_amount, string product_price)
        {
            this.tab_name = tab_name;
            ID_check_history = iD_check_history;
            this.produc_ID_history = produc_ID_history;
            this.product_amount = product_amount;
            this.product_price = product_price;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_check_history", ID_check_history }, { "produc_ID_history", produc_ID_history }, { "product_amount", product_amount }, { "product_price", product_price }};
        }

        public string ID_check_history { get; set; }
        public string produc_ID_history { get; set; }
        public string product_amount { get; set; }
        public string product_price { get; set; }
    }
    class Tab_key : Tab
    {
        public Tab_key(string tab_name, string iD_key, string key, string iV)
        {
            this.tab_name = tab_name;
            ID_key = iD_key;
            this.key = key;
            IV = iV;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_key", ID_key }, { "key", key }, { "IV", IV }};
        }
        public string ID_key { get; set; }
        public string key { get; set; }
        public string IV { get; set; }
    }
    class Tab_product_on_store : Tab
    {
        public Tab_product_on_store(string tab_name, string iD_product_store, string iD_shop_store, string product_amount, string product_price, string last_shipment)
        {
            this.tab_name = tab_name;
            ID_product_store = iD_product_store;
            ID_shop_store = iD_shop_store;
            this.product_amount = product_amount;
            this.product_price = product_price;
            this.last_shipment = last_shipment;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_product_store", ID_product_store }, 
                { "ID_shop_store", ID_shop_store }, { "product_amount", product_amount }, { "product_price", product_price }, { "last_shipment", last_shipment}};
        }

        public string ID_product_store { get; set; }
        public string ID_shop_store { get; set; }
        public string product_amount { get; set; }
        public string product_price { get; set; }
        public string last_shipment { get; set; }
    }
    class Tab_product_type_big : Tab
    {
        public Tab_product_type_big(string tab_name, string iD_product_type_big, string name_product_type_big, string image_big_type)
        {
            this.tab_name = tab_name;
            ID_product_type_big = iD_product_type_big;
            this.name_product_type_big = name_product_type_big;
            this.image_big_type = image_big_type;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_product_type_big", ID_product_type_big }, { "name_product_type_big", name_product_type_big }, { "image_big_type", image_big_type }};
        }
        public string ID_product_type_big { get; set; }
        public string name_product_type_big { get; set; }
        public string image_big_type { get; set; }
    }
    class Tab_product_type_little : Tab
    {
        public Tab_product_type_little(string tab_name, string iD_product_type_little, string iD_product_type_bigger, string name_product_type_little, string image_little_type)
        {
            this.tab_name = tab_name;
            ID_product_type_little = iD_product_type_little;
            ID_product_type_bigger = iD_product_type_bigger;
            this.name_product_type_little = name_product_type_little;
            this.image_little_type = image_little_type;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_product_type_little", ID_product_type_little }, { "ID_product_type_bigger", ID_product_type_bigger }, { "name_product_type_little", name_product_type_little }, { "image_little_type", image_little_type }};
        }
        public string ID_product_type_little { get; set; }
        public string ID_product_type_bigger  { get; set; }
        public string name_product_type_little { get; set; }
        public string image_little_type { get; set; }
    }
    class Tab_promo_full : Tab
    {
        public Tab_promo_full(string tab_name, string iD_promo_full, string iD_customer_dis, string iD_type_group, string iD_product_dis, string cODE, string discount, string used, string image_pomo, string iDs_shops_list)
        {
            this.tab_name = tab_name;
            ID_promo_full = iD_promo_full;
            ID_customer_dis = iD_customer_dis;
            ID_type_group = iD_type_group;
            ID_product_dis = iD_product_dis;
            CODE = cODE;
            this.discount = discount;
            this.used = used;
            this.image_pomo = image_pomo;
            IDs_shops_list = iDs_shops_list;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_promo_full", ID_promo_full }, { "ID_customer_dis", ID_customer_dis },
                { "ID_type_group", ID_type_group }, { "ID_product_dis", ID_product_dis }, { "CODE", CODE}, { "used", used}, { "discount",discount }, { "image_pomo",image_pomo }, { "IDs_shops_list",IDs_shops_list }};
        }

        public string ID_promo_full { get; set; }
        public string ID_customer_dis { get; set; }
        public string ID_type_group { get; set; }
        public string ID_product_dis { get; set; }
        public string CODE { get; set; }
        public string discount { get; set; }
        public string used { get; set; }
        public string image_pomo { get; set; }
        public string IDs_shops_list { get; set; }
    }
    class Tab_promo_history : Tab
    {
        public Tab_promo_history(string tab_name, string cODE_promo_hist, string product_count_promo_his, string iD_shop_promo_his)
        {
            this.tab_name = tab_name;
            CODE_promo_hist = cODE_promo_hist;
            this.product_count_promo_his = product_count_promo_his;
            ID_shop_promo_his = iD_shop_promo_his;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "CODE_promo_hist", CODE_promo_hist }, { "product_count_promo_his", product_count_promo_his }, { "ID_shop_promo_his", ID_shop_promo_his } };
        }

        public string CODE_promo_hist { get; set; }
        public string product_count_promo_his { get; set; }
        public string ID_shop_promo_his { get; set; }
    }
    class Tab_promo_info : Tab
    {
        public Tab_promo_info(string tab_name, string iD_promo, string discount_date_start, string discount_date_finish, string iDs_shops_list_promo)
        {
            this.tab_name = tab_name;
            ID_promo = iD_promo;
            this.discount_date_start = discount_date_start;
            this.discount_date_finish = discount_date_finish;
            IDs_shops_list_promo = iDs_shops_list_promo;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_promo", ID_promo }, { "discount_date_start", discount_date_start }, { "discount_date_finish", discount_date_finish }, { "IDs_shops_list_promo", IDs_shops_list_promo }};
        }
        public string ID_promo { get; set; }
        public string discount_date_start { get; set; }
        public string discount_date_finish { get; set; }
        public string IDs_shops_list_promo { get; set; }
    }
    class Tab_settings : Tab
    {
        public Tab_settings(string tab_name, string iD_settings, string host)
        {
            this.tab_name = tab_name;
            ID_settings = iD_settings;
            this.host = host;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_settings", ID_settings }, { "host", host }};
        }

        public string ID_settings { get; set; }
        public string host { get; set; }
    }

    class Tab_shops : Tab
    {
        public Tab_shops(string tab_name, string iD_shop, string shop_address, string shop_city)
        {
            this.tab_name = tab_name;
            ID_shop = iD_shop;
            this.shop_address = shop_address;
            this.shop_city = shop_city;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "ID_shop", ID_shop }, { "shop_address", shop_address }, { "shop_city", shop_city } };
        }

        public string ID_shop { get; set; }
        public string shop_address { get; set; }
        public string shop_city { get; set; }
    }
    class Tab_type_group : Tab
    {
        public Tab_type_group(string tab_name, string id_type_group, string name_type_group)
        {
            this.tab_name = tab_name;
            this.id_type_group = id_type_group;
            this.name_type_group = name_type_group;
            fields = new Dictionary<string, string> { { "tab_name", tab_name }, { "id_type_group", id_type_group }, { "name_type_group", name_type_group }};
        }

        public string id_type_group { get; set; }
        public string name_type_group { get; set; }
    }
}
