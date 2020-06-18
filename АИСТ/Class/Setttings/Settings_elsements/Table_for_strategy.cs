using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.essence
{
     class Table_for_strategy
    {
        private Tuple<Type_ABC_XYZ, Type_ABC_XYZ> type_pair;
        private Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> prob_of_discount_for;
        private string name;
        public Table_for_strategy(Tuple<Type_ABC_XYZ, Type_ABC_XYZ> type_pair, Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> prob_of_discount_for, CustProd custProd)
        {
            this.type_pair = type_pair;
            this.prob_of_discount_for = prob_of_discount_for;
            name = custProd.ToString() + " type " + type_pair.Item1.ToString() + " " + type_pair.Item2.ToString();
        }
        public Table_for_strategy(Tuple<Type_ABC_XYZ, Type_ABC_XYZ> type_pair, CustProd custProd, Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> prob_of_discount_for)
        {
            this.type_pair = type_pair;
            this.prob_of_discount_for = new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>();
            name = custProd.ToString() + " type " + type_pair.Item1.ToString() + " " + type_pair.Item2.ToString();
            this.prob_of_discount_for = prob_of_discount_for;
        }

        public void Set_prob_of_disc(Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> prob_of_discount_for)
        {
            this.prob_of_discount_for = prob_of_discount_for;
        }

        public void Set_one_prob_of_disc(Tuple<Type_ABC_XYZ, Type_ABC_XYZ> abcxyz, double[] prob_of_discount)
        {
            prob_of_discount_for.Add(abcxyz, prob_of_discount);
        }

        public Tuple<Type_ABC_XYZ, Type_ABC_XYZ> Get_type_pair()
        {
            return type_pair;
        }
        public Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> Get_prob_of_discount_for()
        {
            return prob_of_discount_for;
    }
        public double[] Get_prob_of_discount_for_one(Tuple<Type_ABC_XYZ, Type_ABC_XYZ> abcxyz)
        {
            if (prob_of_discount_for.ContainsKey(abcxyz))
                return prob_of_discount_for[abcxyz];
            else
                return new double[] { -1 };
        }
        public string Get_name()
        {
            return name;
        }

    }
   
}
