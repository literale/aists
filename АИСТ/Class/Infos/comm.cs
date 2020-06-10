using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using АИСТ.Class.SQL.Tab;

namespace АИСТ.Class.Infos
{
    class comm
    {
        public static void login()
        {
            //пытаемся установить соединение
            //SQL_Helper.Set_Connection_String(tb_server.Text, tb_name.Text, tb_login.Text, tb_password.Text);
            //DataTable dt = SQL_Helper.TryToConnect_Full("users");
            try
            {
                Tab_users tu = (Tab_users)Tab_Settings.tabs[enums.Tab_names.users.ToString()];
                DataTable dt = SQL_Helper.Try_To_Connect_Full(tu.tab_name);
                Form ifrm = new mainForm();
                ifrm.Show();
                ifrm.Enabled = true;
                ifrm.Update();
                //Tab_Settings.Load_info();      
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Ошибка доступа к базе данных");
                Tab_Settings.tabs.Clear();
                Form ifrm = Application.OpenForms[0];
                ifrm.Show();
            }
        }

    }
}
