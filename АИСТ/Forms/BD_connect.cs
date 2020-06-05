using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using АИСТ.Class;

namespace АИСТ.Forms
{
    public partial class BD_connect : Form
    {
        bool lk = false;
        bool lg = false;
        bool ps = false;
        bool bn = false;
        /// <summary>
        /// инициализауия
        /// </summary>
        public BD_connect()
        {
            InitializeComponent();
            List<string> temp = Info.get_bd();
            tb_server.Text = temp[1].Split(' ')[2];
            tb_name.Text = temp[2].Split(' ')[2];
            lk = true;
            bn = true;
        }

        /// <summary>
        /// Пытаемся залогиниться в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OK_Click(object sender, EventArgs e)
        {
        }

        private void BD_connect_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
            ifrm.Enabled = true;
            ifrm.Update();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            //пытаемся установить соединение
            SQL_Helper.Set_Connection_String(tb_server.Text, tb_name.Text, tb_login.Text, tb_password.Text);
            //DataTable dt = SQL_Helper.TryToConnect_Full("users");
            try
            {
                DataTable dt = SQL_Helper.Try_To_Connect_Full("users");
                Info.Set_bd(tb_server.Text, tb_name.Text);
                Form ifrm = Application.OpenForms[0];
                ifrm.Show();
                ifrm.Enabled = true;
                ifrm.Update();
                ifrm.Text = "Подключено";
                Tab_Settings.Load_info();
                BD_connect.ActiveForm.Close();

            }
            catch (MySql.Data.MySqlClient.MySqlException)
            {
                MessageBox.Show("Ошибка доступа к базе данных");
            }
        }

        private void tb_name_TextChanged(object sender, EventArgs e)
        {
            //if (!bn)
            //{
            //    tb_name.Text = "";
            //    bn = true;
            //}
        }

        private void tb_server_TextChanged(object sender, EventArgs e)
        {
            if (!lk)
            {
                tb_server.Text = "";
                lk = true;
            }
        }

        private void tb_login_TextChanged(object sender, EventArgs e)
        {
            if (!lg)
            {
                tb_login.Text = "";
                lg = true;
            }
        }

        private void tb_password_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (!ps)
            {
                tb_password.Text = "";
                ps = true;
            }
        }
    }
}
