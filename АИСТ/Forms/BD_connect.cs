using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using АИСТ.Class;

namespace АИСТ.Forms
{
    public partial class BD_connect : Form
    {
        /// <summary>
        /// инициализауия
        /// </summary>
        public BD_connect()
        {
            InitializeComponent();
            List<string>  temp = Info.get_bd();
            tb_server.Text = temp[0].Split(' ')[2];
            tb_name.Text = temp[1].Split(' ')[2];
            tb_login.Text = "admin";
            //tb_name.Text = "bd_shop";
            //tb_server.Text = "localhost";
            tb_password.Text = "diplom2020";
            //tb_login.Text = "";
            //tb_name.Text = "";
            //tb_server.Text = "";
            //tb_password.Text = "";
        }

        /// <summary>
        /// Пытаемся залогиниться в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
            SQL_Helper.Set_Connection_String(tb_server.Text, tb_name.Text, tb_login.Text, tb_password.Text);
            //DataTable dt = SQL_Helper.TryToConnect_Full("users");
            try
            {
                DataTable dt = SQL_Helper.Try_To_Connect_Full("users");
            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Ошибка к базе данных");
            }
            //foreach (DataRow s in dt.Rows)
            //{
            //    object[] users_string = s.ItemArray;
            //    string id = users_string[0].ToString();
            //    string login = users_string[2].ToString();
            //    string password = users_string[3].ToString();
            //    string settings = users_string[5].ToString();
            //}
            Info.Set_bd(tb_server.Text, tb_name.Text);
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
            ifrm.Enabled = true;
            ifrm.Update();
            BD_connect.ActiveForm.Close();
        }
    }
}
