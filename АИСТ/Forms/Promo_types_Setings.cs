using MySql.Data.MySqlClient.Memcached;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;
using АИСТ.Class;
using АИСТ.Class.enums;
using АИСТ.Class.essence;
using АИСТ.Class.Setttings;

namespace АИСТ.Forms
{
    public partial class Promo_types_Setings : Form
    {
        static Promo_types pt;
        static Collect_settings c_set = Info.temp_settings;
        
        static Table_for_strategy active_table = c_set.promo_type.clients[0, 0];
        static Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> temp_cl;
        static Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> temp_prod;
        bool now_load = false;
        public Promo_types_Setings()
        {
            InitializeComponent();
            pt = c_set.promo_type;
            //pt.intresting_cl = (cl_intresting.SelectedIndex == 0) ? true : false;
            //pt.intresting_pr = (prod_intresting.SelectedIndex == 0) ? true : false;
            //pt.comp_client = (prior_cl.SelectedIndex == 0) ? CompareType.purchase_value : CompareType.cost;
            //pt.comp_prod = (prior_prod.SelectedIndex == 0) ? CompareType.sell_value : ((prior_prod.SelectedIndex == 1) ? CompareType.cost : CompareType.amount);
            cl_intresting.SelectedIndex = (pt.intresting_cl) ? 0 : 1;
            prod_intresting.SelectedIndex = (pt.intresting_pr) ? 0 : 1;
            prior_client.SelectedIndex = (pt.comp_client.Equals(CompareType.purchase_value)) ? 0 : 1;
            prior_prod.SelectedIndex = (pt.comp_prod.Equals(CompareType.sell_value)) ? 0 : ((pt.comp_prod.Equals(CompareType.cost)) ? 1 : 2);
            client_ABC.SelectedIndex = 0;
            client_XYZ.SelectedIndex = 0;
            pr_client_ABC.SelectedIndex = 0;
            pr_client_XYZ.SelectedIndex = 0;
            prod_ABC.SelectedIndex = 0;
            prod_XYZ.SelectedIndex = 0;
            cl_prod_ABC.SelectedIndex = 0;
            cl_prod_XYZ.SelectedIndex = 0;
          //  init();
            load_info(CustProd.Customer, true);
            load_info(CustProd.Product, true);
        }

        private void импортToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

       private void load_info(CustProd cp, string this_abc, string this_xyz, string opp_abc, string opp_xyz)
        {
            Type_ABC_XYZ this_a = (this_abc.Equals("A")) ? Type_ABC_XYZ.A : (this_abc.Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
            Type_ABC_XYZ this_x = (this_xyz.Equals("X")) ? Type_ABC_XYZ.X : (this_xyz.Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
            Type_ABC_XYZ opp_a = (opp_abc.Equals("A")) ? Type_ABC_XYZ.A : (opp_abc.Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
            Type_ABC_XYZ opp_x = (opp_xyz.Equals("X")) ? Type_ABC_XYZ.X : (opp_xyz.Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
            active_table = c_set.promo_type.Get_client_or_prod(this_a, this_x, cp);
             
            if (cp.Equals(CustProd.Customer))
            {
                temp_cl = active_table.Get_prob_of_discount_for();
                client_ABC.SelectedItem = this_a.ToString();
                client_XYZ.SelectedItem = this_x.ToString();
                pr_client_ABC.SelectedItem = opp_a.ToString();
                pr_client_XYZ.SelectedItem = opp_x.ToString();
                if (temp_cl.ContainsKey(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)))
                {
                    pers_prod.Value = (decimal)temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0];
                    disc_size.SelectedIndex = (int)temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][1];
                }
                else
                {

                    pers_prod.Value = 0;
                    disc_size.SelectedIndex = 0;
                    temp_cl.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[2] { (double)pers_prod.Value, disc_size.SelectedIndex });
                }
            }
            else if (cp.Equals(CustProd.Product))
            {
                temp_prod = active_table.Get_prob_of_discount_for();
                prod_ABC.SelectedItem = this_a.ToString();
                prod_XYZ.SelectedItem = this_x.ToString();
                cl_prod_ABC.SelectedItem = opp_a.ToString();
                cl_prod_XYZ.SelectedItem = opp_x.ToString();
                if (temp_prod.ContainsKey(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)))
                {
                    prior_cl.SelectedIndex = (int)temp_prod[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0];
                }
                else
                {
                    prior_cl.SelectedIndex = 0;
                    temp_prod.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[1] { prior_cl.SelectedIndex });
                }

            }

        }

        private void load_info(CustProd cp, bool load)
        {

            if (cp.Equals(CustProd.Customer))
            {
                Type_ABC_XYZ this_a = (client_ABC.SelectedItem.Equals("A")) ? Type_ABC_XYZ.A : (client_ABC.SelectedItem.Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
                Type_ABC_XYZ this_x = (client_XYZ.SelectedItem.Equals("X")) ? Type_ABC_XYZ.X : (client_XYZ.SelectedItem.Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
                Type_ABC_XYZ opp_a = (pr_client_ABC.SelectedItem.Equals("A")) ? Type_ABC_XYZ.A : (pr_client_ABC.SelectedItem.Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
                Type_ABC_XYZ opp_x = (pr_client_XYZ.SelectedItem.Equals("X")) ? Type_ABC_XYZ.X : (pr_client_XYZ.SelectedItem.Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
                active_table = c_set.promo_type.Get_client_or_prod(this_a, this_x, cp);
                temp_cl = active_table.Get_prob_of_discount_for();
                if (temp_cl.ContainsKey(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)))
                {
                    if (load)
                    {
                        now_load = true;
                        pers_prod.Value = (decimal)temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0];
                        disc_size.SelectedIndex = (int)temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][1];
                        now_load = false;
                    }
                    else
                    {
                        temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0] = (double)pers_prod.Value;
                        temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][1] = disc_size.SelectedIndex;
                    }

                }
                else
                {
                    if (load)
                    {
                        now_load = true;
                        temp_cl.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[2] { 0, 0 });
                        pers_prod.Value = 0;
                        disc_size.SelectedIndex = 0;
                        now_load = false;
                        // temp_cl.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[2] { (double)pers_prod.Value, disc_size.SelectedIndex });
                    }
                    else
                    {
                        temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0] = (double)pers_prod.Value;
                        temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][1] = disc_size.SelectedIndex;
                    }
                }

            }
            else if (cp.Equals(CustProd.Product))
            {
                Type_ABC_XYZ this_a = (prod_ABC.SelectedItem.Equals("A")) ? Type_ABC_XYZ.A : (prod_ABC.SelectedItem.Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
                Type_ABC_XYZ this_x = (prod_XYZ.SelectedItem.Equals("X")) ? Type_ABC_XYZ.X : (prod_XYZ.SelectedItem.Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
                Type_ABC_XYZ opp_a = (cl_prod_ABC.SelectedItem.Equals("A")) ? Type_ABC_XYZ.A : (cl_prod_ABC.SelectedItem.Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
                Type_ABC_XYZ opp_x = (cl_prod_XYZ.SelectedItem.Equals("X")) ? Type_ABC_XYZ.X : (cl_prod_XYZ.SelectedItem.Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
                active_table = c_set.promo_type.Get_client_or_prod(this_a, this_x, cp);
                temp_prod = active_table.Get_prob_of_discount_for();
                if (temp_prod.ContainsKey(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)))
                {
                    if (load)
                    {
                        now_load = true;
                        prior_cl.SelectedIndex = (int)temp_prod[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0];
                        now_load = false;
                    }
                    else
                    {
                        if (prior_cl.SelectedIndex == 3) temp_prod.Remove(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x));
                        else temp_prod[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0] = prior_cl.SelectedIndex;

                    }


                }
                else
                {
                    if (load)
                    {
                        now_load = true;
                        temp_prod.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[1] { 0 });
                        prior_cl.SelectedIndex = 0;
                        now_load = false;
                        // temp_prod.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[1] { prior_cl.SelectedIndex });
                    }
                    else
                    {
                        temp_prod[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0] = prior_cl.SelectedIndex;
                        if (prior_cl.SelectedIndex == 3) temp_prod.Remove(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x));       
                        //temp_prod.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[1] { prior_cl.SelectedIndex });
                    }

                }

            }
        }

        private void client_ABC_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Customer, true);
        }

        private void client_XYZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Customer, true);
        }

        private void pr_client_ABC_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Customer, true);
        }

        private void pr_client_XYZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Customer, true);
        }

        private void prod_ABC_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Product, true);
        }

        private void prod_XYZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Product, true);
        }

        private void cl_prod_ABC_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Product, true);
        }

        private void cl_prod_XYZ_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Product, true);
        }

        private void pers_prod_ValueChanged(object sender, EventArgs e)
        {
            if(!now_load)
              load_info(CustProd.Customer, false);
        }

        private void disc_size_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!now_load)
                load_info(CustProd.Customer, false);
        }

        private void prior_cl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!now_load)
                load_info(CustProd.Product, false);
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            save();
        }

        //#TODO: вставить метод загрузки в окна
        private void импортToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (MessageBox.Show("Сохранить изменения?", "Сохранить?", MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
            {
                save();
            }
            OpenFileDialog open = new OpenFileDialog();
            string path = Directory.GetCurrentDirectory();
            path += "\\presets";
            open.InitialDirectory = path;
            open.Title = ("Открыть");
            open.Filter = "Text Document (*.txt) | *.txt| All Files (*.*)|*.*";
            if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string[] temp_info = File.ReadAllLines(open.FileName);
                import_preset(temp_info);
                cl_intresting.SelectedIndex = (pt.intresting_cl) ? 0 : 1;
                prod_intresting.SelectedIndex = (pt.intresting_pr) ? 0 : 1;
                prior_client.SelectedIndex = (pt.comp_client.Equals(CompareType.purchase_value)) ? 0 : 1;
                prior_prod.SelectedIndex = (pt.comp_prod.Equals(CompareType.sell_value)) ? 0 : ((pt.comp_prod.Equals(CompareType.cost)) ? 1 : 2);
                client_ABC.SelectedIndex = 0;
                client_XYZ.SelectedIndex = 0;
                pr_client_ABC.SelectedIndex = 0;
                pr_client_XYZ.SelectedIndex = 0;
                prod_ABC.SelectedIndex = 0;
                prod_XYZ.SelectedIndex = 0;
                cl_prod_ABC.SelectedIndex = 0;
                cl_prod_XYZ.SelectedIndex = 0;
                load_info(CustProd.Customer, true);
                load_info(CustProd.Product, true);
            }
        }
        //#TODO: вставить метод загрузки в файл
        private void save()
        {
            List<string> text = save_preset();
            SaveFileDialog save = new SaveFileDialog();
            string path = Directory.GetCurrentDirectory();
            path += "\\presets";
            save.InitialDirectory = path;
            save.Title = ("Сохранить как...");
            save.Filter = "Text Document (*.txt) | *.txt| All Files (*.*)|*.*";
            save.OverwritePrompt = true;
            if (save.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllLines(save.FileName, text.ToArray());
               
            }
        }

        public static void import_preset(string[] temp_info)
        {
            int i = 1;
            string s = temp_info[i];
            //i++;

            string master = "Client ";
            string slave = "Prod ";
            string stop = "stop_clients";
            CustProd cp = CustProd.Customer;
            for (int k = 0; k< 2; k++)
            {
                while (!temp_info[i].Contains(stop))
                {
                    s = temp_info[i];
                    if (s.Contains(master))
                    {
                        string[] cont = s.Split(' ');
                        //active_table = c_set.promo_type.Get_client_or_prod(this_a, this_x, cp);
                        //temp_cl = active_table.Get_prob_of_discount_for();
                        Type_ABC_XYZ this_a = (cont[1].Equals("A")) ? Type_ABC_XYZ.A : (cont[1].Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
                        Type_ABC_XYZ this_x = (cont[2].Equals("X")) ? Type_ABC_XYZ.X : (cont[2].Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
                        active_table = c_set.promo_type.Get_client_or_prod(this_a, this_x, cp);
                        temp_cl = active_table.Get_prob_of_discount_for();
                        i++;
                        s = temp_info[i];
                        while (s.Contains(slave))
                        {
                            s = temp_info[i];
                            cont = s.Split(' ');
                            Type_ABC_XYZ opp_a = (cont[1].Equals("A")) ? Type_ABC_XYZ.A : (cont[1].Equals("B") ? Type_ABC_XYZ.B : Type_ABC_XYZ.C);
                            Type_ABC_XYZ opp_x = (cont[2].Equals("X")) ? Type_ABC_XYZ.X : (cont[2].Equals("Y") ? Type_ABC_XYZ.Y : Type_ABC_XYZ.Z);
                            List<double> temp_array = new List<double>();
                            for (int j = 3; j< cont.Length; j++)
                            {
                                temp_array.Add(Convert.ToDouble(cont[j]));
                            }
                            temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)] = temp_array.ToArray();
                            i++;
                            s = temp_info[i];
                        }
                    }

                    //i++;
                }
                i += 2;
               master = "Prod " ;
               slave = "Client ";
               stop = "stop_prods";
               cp = CustProd.Product;
            }
            i = temp_info.Length - 2;
            s = temp_info[i];
            string[] cont2 = s.Split(' ');
            pt.intresting_cl = (cont2[1].Equals("0")) ? true : false;
            pt.comp_client = (cont2[0].Equals("purchase_value")) ? CompareType.purchase_value : CompareType.cost;

            cont2 = s.Split(' ');
            s = temp_info[i+1];
            pt.intresting_pr = (cont2[1].Equals("0")) ? true : false;  
            pt.comp_prod = (cont2[0].Equals("sell_value")) ? CompareType.sell_value : ((cont2[0].Equals("cost")) ? CompareType.cost : CompareType.amount);

        }

        public List<string> save_preset()
        {
            List<string> text = new List<string>();           
            Table_for_strategy[,] t_client = c_set.promo_type.Get_clients();
            Table_for_strategy[,] t_prods = c_set.promo_type.Get_products();
            text.Add("start_clients");
            foreach (Table_for_strategy tab in t_client)
            {
                text.AddRange(tab.ToList("Client", "Prod"));
            }
            text.Add("stop_clients");
            text.Add("start_prods");
            foreach (Table_for_strategy tab in t_prods)
            {
                text.AddRange(tab.ToList("Prod", "Client"));
            }
            text.Add("stop_prods");
            //CompareType { cost, sell_value, amount, purchase_value }
            string prior = (prior_client.SelectedIndex == 0) ? CompareType.purchase_value.ToString() : CompareType.cost.ToString();
            text.Add(prior + " "+cl_intresting.SelectedIndex);
            prior = (prior_prod.SelectedIndex == 0) ? CompareType.sell_value.ToString() : ((prior_prod.SelectedIndex == 1) ? CompareType.cost.ToString() : CompareType.amount.ToString());
            text.Add(prior  +" "+prod_intresting.SelectedIndex );
            return text;
        }

        private void prior_client_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Promo_types_Setings_FormClosed(object sender, FormClosedEventArgs e)
        {
            pt.intresting_cl = (cl_intresting.SelectedIndex == 0) ? true : false;
            pt.intresting_pr = (prod_intresting.SelectedIndex == 0) ? true : false;
            pt.comp_client = (prior_client.SelectedIndex == 0) ? CompareType.purchase_value : CompareType.cost;
            pt.comp_prod = (prior_prod.SelectedIndex == 0) ? CompareType.sell_value : ((prior_prod.SelectedIndex == 1) ? CompareType.cost : CompareType.amount);
            Form ifrm = Application.OpenForms[Application.OpenForms.Count-1];
            ifrm.Enabled = true;
        }

        private void Promo_types_Setings_FormClosing(object sender, FormClosingEventArgs e)
        {
            pt.intresting_cl = (cl_intresting.SelectedIndex == 0) ? true : false;
            pt.intresting_pr = (prod_intresting.SelectedIndex == 0) ? true : false;
            pt.comp_client = (prior_client.SelectedIndex == 0) ? CompareType.purchase_value : CompareType.cost;
            pt.comp_prod = (prior_prod.SelectedIndex == 0) ? CompareType.sell_value : ((prior_prod.SelectedIndex == 1) ? CompareType.cost : CompareType.amount);
            Form ifrm = Application.OpenForms[Application.OpenForms.Count-1];
            ifrm.Enabled = true;
        }
    }
}
