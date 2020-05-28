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
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            Form f2 = new Generate();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form f2 = new report();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form f2 = new Settings();
            f2.Show(); // отображаем Form2
            this.Hide(); // скрываем Form1 (this - текущая форма)
        }

        private void menu_Load(object sender, EventArgs e)
        {

        }

        private void menu_EnabledChanged(object sender, EventArgs e)
        {

        }
    }
}
