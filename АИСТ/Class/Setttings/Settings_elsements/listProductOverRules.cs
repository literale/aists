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
        Dictionary<Tuple<bool, Group>, List<string>> rules = new Dictionary<Tuple<bool, Group>, List<string>>();

        public void Add_rule (Tuple<bool, Group> id_group, string id)
        {
            if (!rules.ContainsKey(id_group))
            {
                rules[id_group] = new List<string>();
            }
            rules[id_group].Add(id);
               
        }
        public Dictionary<Tuple<bool, Group>, List<string>> Get_rules()
        {
            return rules;
        }
        public List<string> Get_rule(Tuple<bool, Group> group)
        {
            if(rules.ContainsKey(group))
            {
                List<string> t = rules[group];
                return t;
            }
            

            return null;
        }

    }
}
