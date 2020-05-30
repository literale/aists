using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АИСТ.Class
{
   class Tabs
    {
        private string Tab_name;
        private Dictionary<string, string> fields = new Dictionary<string, string>();
       
        public Tabs (string name)
        {
            Tab_name = name;
        }
        public string Get_name()
        {
            return Tab_name;
        }
        public Dictionary<string, string> Get_fielsd()
        {
            return fields;
        }

        public void Set_fields(Dictionary<string, string> fields)
        {
            this.fields = fields;
        }
        
    }
}
