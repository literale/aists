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
        public Promo_types_Setings()
        {
            InitializeComponent();
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
                        pers_prod.Value = (decimal)temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0];
                        disc_size.SelectedIndex = (int)temp_cl[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][1];
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
                        pers_prod.Value = 0;
                        disc_size.SelectedIndex = 0;
                        temp_cl.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[2] { (double)pers_prod.Value, disc_size.SelectedIndex });
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
                temp_prod = active_table.Get_prob_of_discount_for();
                if (temp_prod.ContainsKey(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)))
                {
                    if (load)
                    {
                        prior_cl.SelectedIndex = (int)temp_prod[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0];
                    }
                    else
                    {

                    }


                }
                else
                {
                    if (load)
                    {
                        prior_cl.SelectedIndex = 0;
                        temp_prod.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x), new double[1] { prior_cl.SelectedIndex });
                    }
                    else
                    {
                        temp_prod[new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(opp_a, opp_x)][0] = prior_cl.SelectedIndex;
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
            load_info(CustProd.Customer, false);
        }

        private void disc_size_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Customer, false);
        }

        private void prior_cl_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_info(CustProd.Product, false);
        }

      
    }
}
