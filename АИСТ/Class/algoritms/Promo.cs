using System;
using System.Collections.Generic;
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
    }
}
