using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.Setttings
{
    class Client_type_disc
    {
        public Type_ABC_XYZ client_type_abc;
        public Type_ABC_XYZ client_type_xyz;
        public Dictionary<Type_ABC_XYZ[], double[]> disc_prob;
    }
    class Product_type_disc
    {
        public Type_ABC_XYZ prod_abc;
        public Type_ABC_XYZ prod_xyz;
        public Dictionary<Type_ABC_XYZ[], int> client_prior;
        public string name;
    }
}
