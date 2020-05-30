using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.essence;
using АИСТ.Class.Setttings;

namespace АИСТ.Class.AutoSet
{
    class AutoSetGenerate
    {
        public static Generate_Setttings AutoSettings()
        {
            
            Promo_types pt = Promo_types.Get_auto();
            List<listProduct> lp = new List<listProduct>();
            List<string> prod = new List<string>();
            DataTable dt = SQL_Helper.Try_To_Connect_Full("brands");
            //List<string> s1 = new List<string>();
            Random r = new Random();
            int count = dt.Rows.Count;
            for(int i = 0; i < 500; i++)
            {
                int j = r.Next(0, count);
                prod.Add(dt.Rows[j].ItemArray[0].ToString());
            }
            //foreach (DataRow s in dt.Rows)
            //{
            //    object[] shop_string = s.ItemArray;
            //    s1.Add(shop_string[0].ToString());
            //}
            lp.Add(new listProduct(true, enums.Group.Brand, prod));
            Customers c = new Customers(DateTime.Now.AddDays(-90), DateTime.Now, 1000, 2000, new string[] { "1", "2", "3" }, "c1");
            Assortiment a = new Assortiment(0, 50, DateTime.Now.AddDays(-90), DateTime.Now, new string[] { "1", "2", "3" }, "a1", lp);
            List<Assortiment> la = new List<Assortiment>();
            la.Add(a);
            List<Customers> lc = new List<Customers>();
            lc.Add(c);
            Generate_Setttings gs = new Generate_Setttings(DateTime.Now, DateTime.Now.AddDays(15), 5, 15, la,lc, pt);

            return gs;
        }


    }
}
