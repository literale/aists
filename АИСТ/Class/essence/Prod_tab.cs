using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.algoritms
{
    class Prod_tab
    {
        private String id = "";
        private Group type = Group.Product;
        private Type_ABC_XYZ abc = Type_ABC_XYZ.N;
        private Type_ABC_XYZ xyz = Type_ABC_XYZ.N;
        public void Set_id(string id)
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

        public void Set_XYZ(Type_ABC_XYZ xyz)
        {
            this.xyz = xyz;
        }

        public Type_ABC_XYZ Get_ABC()
        {
            return abc;
        }
        public Type_ABC_XYZ Get_XYZ()
        {
            return xyz;
        }

        public Group Get_type()
        {
            return type;
        }

        public void Set_type(Group type)
        {
            this.type = type;
        }
    }
}
