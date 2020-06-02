using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.algoritms
{
    class Client_Tab
    {
        private String id = "";
        private Type_ABC_XYZ abc = Type_ABC_XYZ.N;
        private Dictionary<string, Type_ABC_XYZ[]> prod = new Dictionary<string, Type_ABC_XYZ[]>();
        private Type_ABC_XYZ xyz = Type_ABC_XYZ.N;
        public void Set_id (string id)
        {
            this.id = id;
        }

        public string Get_id()
        {
            return id;
        }

        public void Set_ABC(Type_ABC_XYZ a)
        {
            this.abc = a;
        }
        public Dictionary<string, Type_ABC_XYZ[]> Get_prod()
        {
            return prod;
        }

        public void Set_prod (Dictionary<string, Type_ABC_XYZ[]> prod)
        {
            this.prod = prod;
        }

        public void Add_prod(string id_prod, Type_ABC_XYZ[] AX)
        {
            prod.Add(id_prod, AX);
        }
        public Type_ABC_XYZ Get_ABC()
        {
            return abc;
        }
        public Type_ABC_XYZ Get_XYZ()
        {
            return xyz;
        }
        public void Set_XYZ(Type_ABC_XYZ xyz)
        {
            this.xyz = xyz;
        }
    }
}
