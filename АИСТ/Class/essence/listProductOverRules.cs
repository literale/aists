using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using АИСТ.Class.enums;

namespace АИСТ.Class.essence
{
    
    class listProductOverRules
    {
        Dictionary<Tuple<string, Group>, bool> rules = new Dictionary<Tuple<string, Group>, bool>();

        public void Add_rule (Tuple<string, Group> id_group, bool allow)
        {
            if (rules.ContainsKey(id_group))
            {
                //тут будет форма обработки
            }
            else rules.Add(id_group, allow);
        }
        public Dictionary<Tuple<string, Group>, bool> Get_rules()
        {
            return rules;
        }
        public Tuple<Tuple<string, Group>, bool> Get_rule(Tuple<string, Group> id_group)
        {
            Tuple<Tuple<string, Group>, bool> t = new Tuple<Tuple<string, Group>, bool>(id_group, rules[id_group]);

            return t;
        }

        //bool allow = true;
        //Group type = Group.Product;
        //List<string> id_prod = new List<string>();
        //public listProductOver(bool allow, Group type, List<string> prod)
        //{
        //    this.allow = allow;
        //    this.type = type;
        //    this.id_prod = prod;
        //}
        //public bool Get_allow()
        //{
        //    return allow;
        //}

        //public Group Get_type()
        //{
        //    return type;
        //}

        //public List<string> Get_id_prod()
        //{
        //    return id_prod;
        //}

    }
}
