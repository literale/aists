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
using АИСТ.Forms;

namespace АИСТ
{
    public partial class mainForm : Form
    {
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
            //SQL_Helper.Get_columns("promo_full");
            Form f2 = new menu();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form f2 = new BD_connect();
            f2.Show(); // отображаем Form2
            this.Enabled = false;
            //this.Hide(); // скрываем Form1 (this - текущая форма)
            //string connection_string = "";
            //SQL_Helper.setConnection(connection_string);
        }

        private void mainForm_EnabledChanged(object sender, EventArgs e)
        {
           lb_bd.Text = Info.Get_bd_name();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string connection_string = "server=localhost; " +
                   "user=" + "admin" + "; " +
                   "database=" + "bd_shop" + "; " +
                   "password=" + "diplom2020";
            SQL_Helper.setConnection(connection_string);
            algoritms a = new algoritms();
            a.Auto();
        }
    }
}
