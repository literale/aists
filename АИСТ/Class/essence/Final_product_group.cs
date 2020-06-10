using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.algoritms
{
    public class Final_product_group
    {
        public List<string> ids_prods = new List<string>();
        public double prob_by_client = 0;
        public double disc_size_by_client = 0;
        public double prior_by_good = 0;
        public enums.Group group;

        public Final_product_group(List<string> ids_prods, double prob_by_client, double disc_size_by_client, double prior_by_good, enums.Group group)
        {
            this.ids_prods = ids_prods;
            this.prior_by_good = prior_by_good;
            this.disc_size_by_client = disc_size_by_client;
            this.prob_by_client = prob_by_client;
            this.group = group;
        }
    }
}
