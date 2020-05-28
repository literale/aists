using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace АИСТ
{
    public partial class Generate : Form
    {
        int n = 1;
        int n2 = 1;
        public Generate()
        {
            InitializeComponent();
            button4.Enabled = false;
            button5.Enabled = false;
            listBox5.SetSelected(0, true);
            listBox6.SetSelected(3, true);
            checkedListBox5.SetItemChecked(0, true);

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
            
            checkedListBox2.Items.Add(textBox5.Text);
            int i = checkedListBox2.Items.Count;
            checkedListBox2.SetItemChecked(i - 1, true);
            listBox1.Items.Add(textBox5.Text);
            listBox1.SetSelected(0, true);
            listBox2.SetSelected(0, true);
            n++;
            textBox5.Text = "Клиенты " + n;
            button4.Enabled = true;
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox5.Text = listBox1.SelectedItem.ToString();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            checkedListBox4.Items.Add(textBox6.Text);
            int i = checkedListBox4.Items.Count;
            checkedListBox4.SetItemChecked(i - 1, true);
            listBox4.Items.Add(textBox6.Text);
            listBox4.SetSelected(0, true);
            listBox3.SetSelected(0, true);
            n2++;
            textBox6.Text = "Ассортимент " + n2;
            button5.Enabled = true;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkedListBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form f2 = new Generete_report();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }
    }
}
