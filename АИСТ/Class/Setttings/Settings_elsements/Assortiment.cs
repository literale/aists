using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.essence;

namespace АИСТ.Class
{
    class Assortiment
    {
       private int min_count;
       private int max_count; 
       private DateTime deliver1;
       private DateTime deliver2;
       private string name;
       //private listProductOverRules rules;
       public Assortiment(int min_count, int max_count, DateTime deliver1, DateTime deliver2, string name)
        {
            this.min_count = min_count;
            this.max_count = max_count;
            this.deliver1 = deliver1;
            this.deliver2 = deliver2;
            this.name = name;
        }
        
        public int[] Get_count()
        {
            int[] count = { min_count, max_count };
            return count;
        }

        public DateTime[] Get_deliver()
        {
            DateTime[] dt = { deliver1, deliver2 };
            return dt;
        }

        public string Get_name()
        {
            return name;
        }
        public override bool Equals(object obj)  //Перегрузка метода 
        {
            string name2 = ((Assortiment)obj).Get_name();
            return (name.Equals(name2));
        }
        //public listProductOverRules[] Get_product()
        //{
        //    listProductOverRules[] lp = product.ToArray();
        //    return lp;
        //}
    }
}
