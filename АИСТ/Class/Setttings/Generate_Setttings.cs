using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.Setttings;

namespace АИСТ.Class
{
    class Generate_Setttings
    {
        public DateTime start;
        public DateTime end;
        public int min_discount;
        public int max_discount;
        public List<Assortiment> assortiments;
        public List<Customers> customers;
        public Promo_types promo_type;

        public Generate_Setttings(DateTime start, DateTime end, int min_disc, int max_dist, List<Assortiment> assortiments, List<Customers> customers, Promo_types promo_type)
        {
            this.start = start;
            this.end = end;
            this.min_discount = max_dist;
            this.max_discount = max_dist;
            this.assortiments = assortiments;
            this.customers = customers;
            this.promo_type = promo_type;
    }

    }
}
