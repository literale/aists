using MySqlX.XDevAPI;
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
using АИСТ.Class.AutoSet;
using АИСТ.Forms;

namespace АИСТ
{
    public partial class Generate : Form
    {
        int n = 1;
        int n2 = 1;
        bool open = false;
        public Generate()
        {
            InitializeComponent();
            //button5.Enabled = false;
           if (Info.Is_admin())
            {
                запуститьВТестовомРежимеToolStripMenuItem.Enabled = true;
                расшифроватьФайлToolStripMenuItem.Enabled = true;
                зашифроватьФайлToolStripMenuItem.Enabled = false;

            }
           else
            {
                запуститьВТестовомРежимеToolStripMenuItem.Enabled = false;
                расшифроватьФайлToolStripMenuItem.Enabled = false;
                зашифроватьФайлToolStripMenuItem.Enabled = false;
            }
            checkedListBox5.SetItemChecked(0, true);
            Load_on_exept_fоrm();


        }

        public void Load_on_exept_fоrm()
        {
            checkedListBox5.MultiColumn = true;
            checkedListBox5.Items.Clear();
            string table = "";
            if (domainUpDown1.SelectedIndex == 0)
                table = "product_type_big";
            string request = "SELECT * FROM product_type_big;";
            DataTable dt = SQL_Helper.Just_do_it(request);
            foreach(DataRow dr in dt.Rows)
            {
                checkedListBox5.Items.Add(dr.ItemArray[1].ToString());

            }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form ifrm = Application.OpenForms[0];
            ifrm.Show();
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
            //checkedListBox2.Items.Add(textBox5.Text);
            //int i = checkedListBox2.Items.Count;
            //checkedListBox2.SetItemChecked(i - 1, true);
            //n++;
            //textBox5.Text = "Клиенты " + n;

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

        private void button6_Click(object sender, EventArgs e)
        {
            //checkedListBox4.Items.Add(textBox6.Text);
            //int i = checkedListBox4.Items.Count;
            //checkedListBox4.SetItemChecked(i - 1, true);
            //listBox4.Items.Add(textBox6.Text);
            //listBox4.SetSelected(0, true);
            //listBox3.SetSelected(0, true);
            //n2++;
            //textBox6.Text = "Ассортимент " + n2;
            //button5.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (!open)
            {
                Algoritm a = new Algoritm();
                a.Auto();
                Form f2 = new Generete_report();
                f2.Show(); // отображаем Form2
                this.Hide(); // скрываем Form1 (this - текущая форма)
            }
            else
            {
                MessageBox.Show("Зашифруйте файл");
            }
        }



        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void запуститьВТестовомРежимеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!open)
            {
                string connection_string = "server=localhost; " +
                       "user=" + "admin" + "; " +
                       "database=" + "bd_shop" + "; " +
                       "password=" + "diplom2020";
                SQL_Helper.setConnection(connection_string);
                Algoritm a = new Algoritm();
                Dictionary<string, List<Promo>> promos = a.Auto();
                Generate_Setttings gs = AutoSetGenerate.AutoSettings();
                Info.Set_test(true);
                Info.Set_promo(promos, a, gs);
                Form gr = new Generete_report();
                gr.Show(); // отображаем Form2
                this.Enabled = false;
            }
            else
            {
                MessageBox.Show("Зашифруйте файл");
            }

        }

        private void расшифроватьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Info.Give_me_file();
            System.Diagnostics.Process p = System.Diagnostics.Process.Start("info.txt");
            bool open = true;
            зашифроватьФайлToolStripMenuItem.Enabled = true;
            расшифроватьФайлToolStripMenuItem.Enabled = false;
            if (p.HasExited)
            {
                Info.Take_you_file();
                расшифроватьФайлToolStripMenuItem.Enabled = true;
                зашифроватьФайлToolStripMenuItem.Enabled = false;
                open = false;
            }
        }

        private void зашифроватьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Info.Take_you_file();
                расшифроватьФайлToolStripMenuItem.Enabled = true;
                зашифроватьФайлToolStripMenuItem.Enabled = false;
                open = false;
            }
            catch (Exception)
            {

                    MessageBox.Show("Невозможно сохранить файл");

            }
        }

        private void создатьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void создатьToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            Form f = new Promo_types_Setings();
            f.Show();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void пресетыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
