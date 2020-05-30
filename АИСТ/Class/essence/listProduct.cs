using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace АИСТ.Class.essence
{
    enum Group { Product, Brand, Little_type, Big_type }
    class listProduct
    {
        bool allow = true;
        Group type = Group.Product;
        List<string> prod = new List<string>();

    }
}
