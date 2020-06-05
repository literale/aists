using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АИСТ.Class
{
    static class Tab_Settings
    {
        private static string email;
        private static Dictionary<string, Tabs> all_tabs = new Dictionary<string, Tabs>();
        public static void temp_set_email()
        {
            email = "ESGdiplom2020shop@yandex.ru";
        }
        public static void Load_info()
        {
            Dictionary<string, Tabs> tabs = Info.Get_tabs();
            List<string> t = Info.get_bd();
            for (int i = 0; i< t.Count; i++)
            {
                if (t[i].Contains("Tstart"))
                {
                    i++;
                    string name = t[i];
                    Tabs tab = new Tabs(name.Split(' ')[2]);
                    i++;
                    while (!t[i].Contains("Tend"))
                    {
                        tab.Add_fields(t[i].Split(' ')[0], t[i].Split(' ')[2]);
                        i++;
                    }
                    all_tabs.Add(name.Split(' ')[0], tab);
                }
            }
            Info.Set_tabs(tabs);
        }
        public static Dictionary<string, Tabs> Get_tabs()
        {
            return all_tabs;
        }

    }

}





