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

        /// <summary>
        /// запись в таблицу, строка должна иметь значения всех столбцов в формате "('v1', 'v2')"
        /// </summary>
        /// <param name="string_to_write"></param> - строка со значениями для записи
        /// <param name="table"></param> - таблица для записи
        public static void WriteInTable(string table, Dictionary<string, string> column_value)
        {
            //string connection_string = Info.connection_string;
            connection.Open();
            string request = "INSERT INTO " + table + " (";

            int count = 0;
            foreach (String column in column_value.Keys)
            {
                request += column;
                if (count < column_value.Count - 1) request += ", ";
                else request += ") ";
                count++;
            }
            count = 0;
            request += "VALUES (";
            foreach (String column in column_value.Keys)
            {
                request += "'" + column_value[column] + "'";
                if (count < column_value.Count - 1) request += ", ";
                else request += ") ";
                count++;
            }
            request += ";";
            //Console.WriteLine(request);
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            connection.Close();

        }

        /// <summary>
        /// проверяет, есть ли значение в таблице
        /// </summary>
        /// <param name="table"></param> таблица откуда берем
        /// <param name="key_names"></param> Названия полей
        /// <param name="key_values"></param> Значения полей
        /// request SELECT * FROM brands WHERE idBrands = 01 AND Brand_name = "jjj";
        /// <returns></returns>
        public static int TableHaveKey(string table, Dictionary<string, string> column_value, string id_column_name)
        {
            string request = "SELECT * FROM " + table + " WHERE";
            int i = 0;
            foreach (String column in column_value.Keys)
            {
                request += " " + column + " = '" + column_value[column] + "'";
                if (i + 1 < column_value.Count) request += " AND ";
                i++;
            }
            request += ";";
            connection.Open();
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            connection.Close();

            if (temp_dtable.Rows.Count > 0)
            {
                return Convert.ToInt32(temp_dtable.Rows[0][id_column_name]);

            }

            else return 0;
        }

        /// <summary>
        /// Скоролько уникальных значений в столбце в таблице
        /// </summary>
        /// <param name="table"></param> таблица
        /// <param name="key"></param> столбец
        /// <returns></returns>
        public static int HowMuchRows(string table, string key)
        {
            int id = 0;
            connection.Open();
            string request = "SELECT COUNT('" + key + "') FROM " + table;
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            foreach (DataRow s in temp_dtable.Rows)
            {
                object[] fields = s.ItemArray;
                foreach (object o in fields)
                {
                    connection.Close();
                    return Convert.ToInt32(o);
                }
            }

            connection.Close();
            return id;
        }

        public static DataTable TryToGetStrings(string table, Dictionary<string, string> column_value)
        {
            connection.Open();
            string request = "SELECT * FROM " + table + " WHERE";
            int i = 0;
            foreach (String column in column_value.Keys)
            {
                request += " " + column + " = '" + column_value[column] + "'";
                if (i + 1 < column_value.Count) request += " AND ";
                i++;
            }
            request += ";";

            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            connection.Close();
            return temp_dtable;
        }

        public static object[] ReturnRandom(string table, Dictionary<string, string> column_value, Random r)
        {
            connection.Open();
            string request = "SELECT * FROM " + table + " WHERE";
            int i = 0;
            foreach (String column in column_value.Keys)
            {
                request += " " + column + " = '" + column_value[column] + "'";
                if (i + 1 < column_value.Count) request += " AND ";
                i++;
            }
            request += ";";

            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            connection.Close();
            i = r.Next(1, temp_dtable.Rows.Count);
            object[] temp_array = temp_dtable.Rows[i].ItemArray;
            return temp_array;
        }

        public static object[] ReturnRandom(string table, Random r)
        {
            connection.Open();
            string request = "SELECT * FROM " + table + ";";
            int i = 0;

            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            connection.Close();
            i = r.Next(1, temp_dtable.Rows.Count);
            object[] temp_array = temp_dtable.Rows[i].ItemArray;
            return temp_array;
        }

        public static void UpdateString(string table, Dictionary<string, string> new_value, Dictionary<string, string> keys)
        {
            connection.Open();
            string request = "UPDATE " + table + " SET";
            int i = 0;
            foreach (String column in new_value.Keys)
            {
                request += " " + column + " = '" + new_value[column] + "'";
                if (i + 1 < new_value.Count) request += ",";
                i++;
            }
            request += " WHERE";
            i = 0;
            foreach (String column in keys.Keys)
            {
                request += " " + column + " = '" + keys[column] + "'";
                if (i + 1 < keys.Count) request += " AND ";
                i++;
            }
            request += ";";

            MySqlCommand new_command = new MySqlCommand(request, connection);
            new_command.ExecuteReader();
            connection.Close();

            //            //UPDATE `bd_shop`.`checks`
            //            SET
            //`ID_check` = <{ ID_check: }>,
            //`ID_shop_check` = <{ ID_shop_check: }>,
            //`check_date` = <{ check_date: }>,
            //`ID_customer_check` = <{ ID_customer_check: }>,
            //`check_total_sum` = <{ check_total_sum: }>
            // WHERE `ID_check` = <{ expr}> AND `ID_shop_check` = <{ expr}>;//
        }

        public static DataTable Just_do_it(string request)
        {
            connection.Open();
            MySqlCommand new_command = new MySqlCommand(request, connection);
            MySqlDataReader data_reader = new_command.ExecuteReader();
            DataTable temp_dtable = new DataTable();
            temp_dtable.Load(data_reader);
            connection.Close();
            return temp_dtable;
        }


    }

}
