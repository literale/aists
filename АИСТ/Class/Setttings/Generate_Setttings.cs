using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;
using АИСТ.Class.essence;
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
        public listProductOverRules rules;
        public DateTime analiz_border;
        public List<string> shops;
        public Generate_Setttings(DateTime start, DateTime end, int min_disc, int max_disc, List<Assortiment> assortiments, List<Customers> customers, Promo_types promo_type, listProductOverRules rules, DateTime analiz_border, List<string> shops)
        {
            this.start = start;
            this.end = end;
            this.min_discount = min_disc;
            this.max_discount = max_disc;
            this.assortiments = assortiments;
            this.customers = customers;
            this.promo_type = promo_type;
            this.rules = rules;
            this.analiz_border = analiz_border;
            this.shops = shops;
    }

    }
}
