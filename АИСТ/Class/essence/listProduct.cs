using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.essence
{
    
    class listProduct
    {
        bool allow = true;
        Group type = Group.Product;
        List<string> prod = new List<string>();
        public listProduct(bool allow, Group type, List<string> prod)
        {
            this.allow = allow;
            this.type = type;
            this.prod = prod;
        }

    }
}
