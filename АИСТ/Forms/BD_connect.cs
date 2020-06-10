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
            Info.Get_tabs();
            try
            {
                tb_server.Text = temp[1].Split(' ')[2];
                tb_name.Text = temp[2].Split(' ')[2];
            }
            catch
            {
                DialogResult result = MessageBox.Show(
                 "Сбросить настройки?",
                 "Ошибка импорта настроек",
                  MessageBoxButtons.YesNo,
                  MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1,
                 MessageBoxOptions.DefaultDesktopOnly);

                if (result == DialogResult.Yes)
                    Info.Set_defolt_file();
                Info.Set_bd();
                Info.Get_tabs();
                tb_server.Text = temp[1].Split(' ')[2];
                tb_name.Text = temp[2].Split(' ')[2];

            }
            lk = true;
            bn = true;
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
           
            login();
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
            //if (!lk)
            //{
            //    tb_server.Text = "";
            //    lk = true;
            //}
        }

        private void tb_login_TextChanged(object sender, EventArgs e)
        {
            //if (!lg)
            //{
            //    tb_login.Text = "";
            //    lg = true;
            //}
        }

        private void tb_password_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {
            if (!ps)
            {
                tb_password.Text = "";
                ps = true;
            }
        }

        private void tb_login_MouseClick(object sender, MouseEventArgs e)
        {
            if (!lg)
            {
                tb_login.Text = "";
                lg = true;
            }
        }

        private void tb_name_MouseClick(object sender, MouseEventArgs e)
        {
            if (!bn)
            {
                tb_name.Text = "";
                bn = true;
            }
        }

        private void tb_server_MouseClick(object sender, MouseEventArgs e)
        {
            if (!lk)
            {
                tb_server.Text = "";
                lk = true;
            }
        }

        private void tb_server_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }
        private void login()
        {
            Tab_Settings.Put_in_class();
            SQL_Helper.Set_Connection_String(tb_server.Text, tb_name.Text, tb_login.Text, tb_password.Text);
            Info.Set_bd(tb_server.Text, tb_name.Text);
            Form set = new Settings();
            set.Show();
            this.Hide();
        }

        private void tb_name_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }

        private void tb_login_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }

        private void tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }

        private void BD_connect_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }

    }
}
