using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using АИСТ.Class;
using АИСТ.Class.algoritms;
using АИСТ.Class.enums;
using АИСТ.Class.SQL.Tab;
using АИСТ.Forms;
using АИСТ.Properties;

namespace АИСТ
{
    public partial class mainForm : Form
    {
        public object Tab_settings { get; private set; }

        public mainForm()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form f2 = new menu();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            login();
           
        }

        /// <summary>
        /// Логин в бд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            Form f2 = new BD_connect();
            f2.Show(); // отображаем Form2
            this.Enabled = false;
        }

        private void mainForm_EnabledChanged(object sender, EventArgs e)
        {
          // lb_bd.Text = Info.Get_bd_name();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connection_string = "server=localhost; " +
                   "user=" + "admin" + "; " +
                   "database=" + "bd_shop" + "; " +
                   "password=" + "diplom2020";
            SQL_Helper.setConnection(connection_string);
            Algoritm a = new Algoritm();
            a.Auto();
        }

        private void mainForm_MouseEnter(object sender, EventArgs e)
        {
          //  login();
        }

        private void login()
        {
            //SQL_Helper.Get_columns("promo_full");
            string login = textBox1.Text;
            string pass = tb_password.Text;
            //   Tab_Settings.Load_info();
            //Dictionary<string, Tabs> tabs = Tab_Settings.Get_tabs();
            Tab_users tu = (Tab_users)Tab_Settings.tabs[Tab_names.users.ToString()];
            string req = "SELECT " + tu.ID_users_settings + " FROM " + tu.tab_name + " WHERE " + tu.login + " = '" + login + "' AND " + tu.passwor + " ='" + pass + "';";
            DataTable dt = SQL_Helper.Just_do_it(req);
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0].ItemArray[0].ToString().Equals("1") || dt.Rows[0].ItemArray[0].ToString().Equals("2")) Info.Set_admin(true);
                    Form f2 = new Generate();
                    // Form f2 = new menu();
                    f2.Show(); // отображаем Form2
                    this.Hide(); // скрываем Form1 (this - текущая форма)
                }
                else
                {
                    MessageBox.Show("Неверный логин или пароль");
                }
            }
        }

        private void mainForm_Click(object sender, EventArgs e)
        {
           // login();
        }

        private void tb_password_Click(object sender, EventArgs e)
        {
           
        }

        private void tb_password_KeyPress(object sender, KeyPressEventArgs e)
        {

            
        }

        private void tb_password_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }

        private void mainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }

        private void mainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keys.Enter == e.KeyCode)
            {
                login();
            }
        }
    }
}
