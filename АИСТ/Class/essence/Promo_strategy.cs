using Org.BouncyCastle.Crypto.Macs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;
using АИСТ.Class.essence;

namespace АИСТ.Class.Setttings
{
    class Promo_types
    {
        public Table_for_strategy[,] clients = new Table_for_strategy[3, 3];
        public Table_for_strategy[,] products = new Table_for_strategy[3, 3];

        private void Save()
        {
           
        }
        public void Import()
        {

        }
        public void Generate_matrix()
        {
            Type_ABC_XYZ abc = new Type_ABC_XYZ();
            Type_ABC_XYZ xyz = new Type_ABC_XYZ();
            for (int i = 0; i < 3; i++)
            {
                if (i == 0)
                    abc = Type_ABC_XYZ.A;
                else if (i == 1)
                    abc = Type_ABC_XYZ.B;
                else abc = Type_ABC_XYZ.C;
                for (int j = 0; j < 3; j++)
                {
                    if (j == 0)
                        xyz = Type_ABC_XYZ.X;
                    else if (j == 1)
                        xyz = Type_ABC_XYZ.Y;
                    else xyz = Type_ABC_XYZ.Z;

                    clients[i, j] = new Table_for_strategy(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc, xyz), CustProd.Customer);
                    products[i, j] = new Table_for_strategy(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc, xyz), CustProd.Product);
                }
            }
        }
        public void Get_auto()
        {
            Generate_matrix();

            Promo_types pt = new Promo_types();

            Type_ABC_XYZ[] abc = new Type_ABC_XYZ[3] { Type_ABC_XYZ.A, Type_ABC_XYZ.B, Type_ABC_XYZ.C };
            Type_ABC_XYZ[] xyz = new Type_ABC_XYZ[3] { Type_ABC_XYZ.X, Type_ABC_XYZ.Y, Type_ABC_XYZ.Z };
            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    clients[i, j].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
                    products[i, j].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
                    int p1 = 100;
                    int[] p2 = new int[3] { 1, 2, 3 };
                    for (int k = 0; k < 3; k++)
                    {
                        for (int t = 0; t < 3; t++)
                        {
                            int p3 = r.Next(1, p1 + 1);
                            p1 = p1 - p3;
                            clients[i, j].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc[k], xyz[t]), new double[2] { p1, p2[r.Next(0, 3)] });
                            products[i, j].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc[k], xyz[t]), new double[1] { p2[r.Next(0, 3)] });
                            if (r.Next(0, 3) == 0 || p1 < 2)
                                break;
                        }
                        if (r.Next(0, 3) == 0 || p1 < 2)
                            break;
                    }
                    if (p1 < 2)
                        break;
                }


            }
        }

        public Table_for_strategy[,] Get_clients()
        {
            return clients;
        }

        public Table_for_strategy Get_client_or_prod(Type_ABC_XYZ abc, Type_ABC_XYZ xyz, CustProd custOrProd)
        {
            int i = (abc == Type_ABC_XYZ.A) ? 0 : ((abc == Type_ABC_XYZ.B) ? 1 : 2);
            int j = (abc == Type_ABC_XYZ.X) ? 0 : ((abc == Type_ABC_XYZ.Y) ? 1 : 2);
            if (custOrProd == CustProd.Customer)
                return clients[i, j];
            else return products[i, j];

        }
        public void Set_clients(Table_for_strategy[,] clients)
        {
            this.clients = clients;
        }
        public Table_for_strategy[,] Get_products()
        {
            return products;
        }
        public void Set_products(Table_for_strategy[,] products)
        {
            this.products = products;
        }

        public void AR()
        {
            Type_ABC_XYZ[] abc = new Type_ABC_XYZ[3] { Type_ABC_XYZ.A, Type_ABC_XYZ.B, Type_ABC_XYZ.C };
            Type_ABC_XYZ[] xyz = new Type_ABC_XYZ[3] { Type_ABC_XYZ.X, Type_ABC_XYZ.Y, Type_ABC_XYZ.Z };
            Random r = new Random();
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    clients[i, j].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
                    products[i, j].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
                    int p1 = 100;
                    int[] p2 = new int[3] { 1, 2, 3 };
                    for (int k = 0; k < 3; k++)
                    {
                        for (int t = 0; t < 3; t++)
                        {
                            int p3 = r.Next(1, p1 + 1);
                            p1 = p1 - p3;
                            clients[i, j].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc[k], xyz[t]), new double[] { p1, p2[r.Next(0, 3)] });
                            products[i, j].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc[k], xyz[t]), new double[] { p2[r.Next(0, 3)] });
                            if (r.Next(0, 3) == 0) break;
                        }
                    }
                }
            }

        }


    }
}
