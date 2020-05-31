﻿using System;
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
        private Type_ABC abc = Type_ABC.N;
        private Dictionary<string, Type_XYZ> prod = new Dictionary<string, Type_XYZ>();
        private Type_XYZ xyz = Type_XYZ.N;
        public void Set_id (string id)
        {
            this.id = id;
        }

        public string Get_id()
        {
            return id;
        }

        public void Set_ABC (Type_ABC a)
        {
            this.abc = a;
        }

        public void Replase_prod (Dictionary<string, Type_XYZ> prod)
        {
            this.prod = prod;
        }

        public void Add_prod(string id_prod, Type_XYZ X)
        {
            prod.Add(id_prod, X);
        }
        public Type_ABC Get_ABC()
        {
            return abc;
        }
        public Type_XYZ Get_XYZ()
        {
            return xyz;
        }
        public void Set_XYZ(Type_XYZ xyz)
        {
            this.xyz = xyz;
        }
    }
}
