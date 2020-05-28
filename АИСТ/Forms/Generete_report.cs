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
    public partial class Generete_report : Form
    {
        public Generete_report()
        {
            InitializeComponent();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("id");
            dt1.Columns.Add("имя");
            dt1.Columns.Add("min скидка");
            dt1.Columns.Add("max скидка");
            dt1.Rows.Add("01", "сок", "4", "15");
            dt1.Rows.Add("02", "мясо", "1", "25"); ;
            dt1.Rows.Add("03", "тыква", "10", "45");
            dataGridView1.DataSource = dt1.AsDataView();
            dataGridView1.ReadOnly = true;

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("id");
            dt2.Columns.Add("имя");
            dt2.Columns.Add("min скидка");
            dt2.Columns.Add("max скидка");
            dt2.Rows.Add("01", "аня", "1", "5");
            dt2.Rows.Add("02", "ваня", "3", "8");
            dt2.Rows.Add("03", "петя", "2", "10");
            dataGridView2.DataSource = dt2.AsDataView();
            dataGridView2.ReadOnly = true;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
