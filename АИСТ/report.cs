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
    public partial class report : Form
    {
        public report()
        {
            InitializeComponent();
            DataTable dt1 = new DataTable();
            dt1.Columns.Add("id");
            dt1.Columns.Add("имя");
            dt1.Columns.Add("ед продано по акции");
            dt1.Columns.Add("сумма продаж по акции");
            dt1.Rows.Add("01", "сок", "40", "1500");
            dt1.Rows.Add("02", "мясо", "100", "2500"); ;
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");
            dt1.Rows.Add("03", "тыква", "10", "4500");

            dataGridView1.DataSource = dt1.AsDataView();
            dataGridView1.ReadOnly = true;
            dataGridView1.Columns[0].Width = 30;

            DataTable dt2 = new DataTable();
            dt2.Columns.Add("id");
            dt2.Columns.Add("имя");
            dt2.Columns.Add("ед куплено по акции");
            dt2.Columns.Add("сумма покупок по акции");
            dt2.Rows.Add("01", "аня", "10", "500");
            dt2.Rows.Add("02", "ваня", "30", "800");
            dt2.Rows.Add("03", "петя", "20", "1000");
            dt2.Rows.Add("04", "петя", "20", "1000");
            dt2.Rows.Add("05", "петя", "20", "1000");
            dt2.Rows.Add("06", "петя", "20", "1000");
            dt2.Rows.Add("07", "петя", "20", "1000");
            dt2.Rows.Add("08", "петя", "20", "1000");
            dataGridView2.DataSource = dt2.AsDataView();
            dataGridView2.ReadOnly = true;
            dataGridView2.Columns[0].Width = 72;

            DataTable dt3 = new DataTable();
            dt3.Columns.Add("id");
            dt3.Columns.Add("сумма продаж по акции");
            dt3.Rows.Add("01", "12886");
            dt3.Rows.Add("02", "45799");
            dt3.Rows.Add("03", "435214");
            dataGridView3.DataSource = dt3.AsDataView();
            dataGridView3.ReadOnly = true;
            dataGridView3.Columns[0].Width = 32;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void report_Load(object sender, EventArgs e)
        {

        }
    }
}
