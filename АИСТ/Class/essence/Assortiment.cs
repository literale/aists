using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.essence;

namespace АИСТ.Class
{
    class Assortiment
    {
       private DateTime activ1;
       private DateTime activ2; 
       private DateTime deliver1;
       private DateTime deliver2;
       private List<string> Shops;
       private string name;
       private List<listProduct> product;
       public Assortiment(DateTime activ1, DateTime activ2, DateTime deliver1, DateTime deliver2, List<string> Shops, string name, List<listProduct> product)
        {
            this.activ1 = activ1;
            this.activ2 = activ2;
            this.activ1 = deliver1;
            this.activ2 = deliver2;
            this.Shops = Shops;
            this.name = name;
            this.product = product;
        }
        
        public DateTime[] Get_active()
        {
            DateTime[] dt = {activ1, activ2 };
            return dt;
        }

        public DateTime[] Get_deliver()
        {
            DateTime[] dt = { deliver1, deliver2 };
            return dt;
        }

        public string[] Get_shops()
        {
            string[] shops = Shops.ToArray();
            return shops;
        }

        public string Get_name()
        {
            return name;
        }

        public listProduct[] Get_product()
        {
            listProduct[] lp = product.ToArray();
            return lp;
        }
    }
}
