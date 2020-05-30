using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace АИСТ.Class
{
    class SQL_Helper
    {
        //public static string connection_string = "server=localhost; " +
        //           "user=" + "admin" + "; " +
        //           "database=" + "bd_shop" + "; " +
        //           "password=" + "diplom2020";
        public static string connection_string = "";
        //private string login = "admin";
        //private string name = "bd_shop";
        //private string server = "localhost";
        //private string password = "diplom2020";
        //private string login = "";
        //private string name = "";
        //private string server = "";
        //private string password = "";
        private static MySqlConnection connection;

        public static void Set_Connection_String(string server, string bd_name, string login, string password)
        {
            connection_string = "server=" + server +"; " +
                "user=" + login + "; " +
                "database=" + bd_name + "; " +
                "password=" + password;
            setConnection(connection_string);
        }
        public static void CloseConnection()
        {
            connection.Close();
        }
        public static void setConnection(string connection_string)
        {
            connection = new MySqlConnection(connection_string);
        }
        public static DataTable Try_To_Connect_Full(string table)
        {
            //string connection_string = Info.connection_string;
            connection.Open();
            string request = "SELECT * FROM " + table + ";";
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            connection.Close();
            return temp_dtable;
        }

        public static DataTable Get_columns(string table)
        {
            connection.Open();
            //SELECT COLUMN_NAME FROM all_tab_columns WHERE TABLE_NAME='Н_КВАЛИФИКАЦИИ';
            string request = "SHOW COLUMNS FROM `"+table+"`;";
            //string request = "SELECT * FROM " + table + ";";
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            //List<string> s1 = new List<string>();
            //foreach (DataRow s in temp_dtable.Rows)
            //{
            //    object[] shop_string = s.ItemArray;
            //    s1.Add(shop_string[0].ToString());
            //}
            connection.Close();
            return temp_dtable;
        }

        
    }

}
