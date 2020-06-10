using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using System.Threading.Tasks;
using АИСТ.Class.enums;
using АИСТ.Class.essence;
using АИСТ.Class.Setttings;

namespace АИСТ.Class.AutoSet
{
    class AutoSetGenerate
    {
        public static Generate_Setttings AutoSettings()
        {

            Promo_types pt = new Promo_types();
            pt.Get_auto();
            listProductOverRules over_rules = new listProductOverRules();
            over_rules = AddRandomRules(Group.Brand, over_rules, 50);
            over_rules = AddRandomRules(Group.Big_type, over_rules, 3);
            over_rules = AddRandomRules(Group.Little_type, over_rules, 10);
            over_rules = AddRandomRules(Group.Product, over_rules, 1000);
            List<string> shops = new List<string>();
            shops.Add("1");
            shops.Add("2");
            Customers c = new Customers(DateTime.Now.AddDays(-90), DateTime.Now, 1000, 20000,  "c1");
            Assortiment a = new Assortiment(0, 50, DateTime.Now.AddDays(-90), DateTime.Now, "a1");

            Customers c1 = new Customers(DateTime.Now.AddDays(-180), DateTime.Now, 5000, 60000, "c2");
            Assortiment a1 = new Assortiment(0, 150, DateTime.Now.AddDays(-90), DateTime.Now, "a2");
            List<Assortiment> la = new List<Assortiment>();
            la.Add(a);
            la.Add(a1);
            List<Customers> lc = new List<Customers>();
            lc.Add(c);
            lc.Add(c1);
            Generate_Setttings gs = new Generate_Setttings(DateTime.Now, DateTime.Now.AddDays(15), 5, 15, la,lc, pt, over_rules, DateTime.Now.AddDays(-360), shops);

            return gs;
        }

        private static listProductOverRules AddRandomRules(Group group, listProductOverRules over_rules, int count)
        {
            List<string> pieses = new List<string>();
            DataTable dt = new DataTable();
            if (group == Group.Brand) dt = SQL_Helper.Try_To_Connect_Full("brands");
            if (group == Group.Little_type) dt = SQL_Helper.Try_To_Connect_Full("product_type_little");
            if (group == Group.Big_type) dt = SQL_Helper.Try_To_Connect_Full("product_type_big");
            if (group == Group.Product) dt = SQL_Helper.Try_To_Connect_Full("products");
            Random r = new Random();
            int size = dt.Rows.Count;

            for (int i = 0; i < count; i++)
            {
                int j = r.Next(0, count);
                pieses.Add(dt.Rows[j].ItemArray[0].ToString());
            }
            foreach (string p in pieses)
            {
                over_rules.Add_rule(new Tuple<string, Group>(p, group), true);
            }
            for (int i = 0; i < count; i++)
            {
                int j = r.Next(0, count);
                pieses.Add(dt.Rows[j].ItemArray[0].ToString());
            }
            foreach (string p in pieses)
            {
                over_rules.Add_rule(new Tuple<string, Group>(p, group), false);
            }

            return over_rules;
        }

    }
}
