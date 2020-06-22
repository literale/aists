using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.algoritms
{
    class Promo
    {
        public string id_prod;
        public enums.Group group;
        public double disc;

        public Promo(string id_prod, enums.Group group, double disc)
        {
            this.id_prod = id_prod;
            this.group = group;
            this.disc = disc;
        }
        public override string ToString()
        {
            string name = "";
            if (group == enums.Group.Product)
            {
                string request = "SELECT product_name FROM products WHERE ID_product = '" + id_prod + "';";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                name = temp_dt.Rows[0].ItemArray[0].ToString();
            }
            if (group == enums.Group.Brand)
            {
                string request = "SELECT brand_name FROM brands WHERE ID_brand = '" + id_prod + "';";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                name = temp_dt.Rows[0].ItemArray[0].ToString();
            }
            if (group == enums.Group.Little_type)
            {
                string request = "SELECT name_product_type_little FROM product_type_little WHERE ID_product_type_little = '" + id_prod + "';";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                name = temp_dt.Rows[0].ItemArray[0].ToString();
            }
            if (group == enums.Group.Big_type)
            {
                string request = "SELECT name_product_type_big FROM product_type_big WHERE name_product_type_big = '" + id_prod + "';";
                DataTable temp_dt = SQL_Helper.Just_do_it(request);
                name = temp_dt.Rows[0].ItemArray[0].ToString();
            }

            return "ID "+ id_prod+ " " +name + " " + group.ToString() + " " +disc.ToString();
        }
    }
}
