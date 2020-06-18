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
using АИСТ.Class.SQL.Tab;

namespace АИСТ.Properties
{
    public partial class Table_settings : Form
    {
        string n = "";
        public Table_settings(string name)
        {
            n = name;
          //  this.AutoSize = true;
            InitializeComponent();
            Dictionary<string, string> fields = Tab_Settings.tabs[name].fields;
            int x = 3;
            int y = 0;
            foreach (string f in fields.Keys)
            {
                y += 20;
                Point loc = new Point(x, y);
                Label lb = new Label();
                lb.Visible = true;
                lb.Text = f;
                lb.TextAlign = ContentAlignment.MiddleCenter;
                lb.Name = f;
                lb.Location = loc;
                lb.Size = new Size(127, 20);
                lb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
                gb1.Controls.Add(lb);


                TextBox tb = new TextBox();
                tb.Visible = true;
                tb.Text = fields[f];
                tb.Name = f;
                tb.Location = loc;
                tb.Size = new Size(127, 20);
                gb2.Controls.Add(tb);

            }
            y += 25;
            gb1.Size = new Size(130, y);
            gb2.Size = new Size(130, y);
            save.Location = new Point(12, y+20);
            Size = new Size(305, y+25+70);
        }

        private void save_Click(object sender, EventArgs e)
        {
            foreach (Control c in gb2.Controls)
            {
                Tab_Settings.tabs[n].fields[c.Name] = c.Text;
            }
            this.Close();
            Form ifrm = Application.OpenForms[1];
            ifrm.Show();

        }

        private void Table_settings_Load(object sender, EventArgs e)
        {

        }
    }
}
