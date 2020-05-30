using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace АИСТ.Class
{
    class Customers
    {
        private DateTime activ1;
        private DateTime activ2;
        private int min_sum;
        private int max_sum;
        private string[] Shops;
        private string name;
        public Customers(DateTime activ1, DateTime activ2, int min_sum, int max_sum, string[] Shops, string name)
        {
            this.activ1 = activ1;
            this.activ2 = activ2;
            this.min_sum = min_sum;
            this.max_sum = max_sum;
            this.Shops = Shops;
            this.name = name;

        }

        public DateTime[] Get_active()
        {
            DateTime[] dt = { activ1, activ2 };
            return dt;
        }

        public int[] Get_averrage_sum()
        {
            int[] sum = { min_sum, max_sum };
            return sum;
        }

        public string[] Get_shops()
        {
            string[] shops = Shops;
            return shops;
        }

        public string Get_name()
        {
            return name;
        }

    }
}
