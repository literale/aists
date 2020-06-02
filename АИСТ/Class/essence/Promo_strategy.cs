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
        private Table_for_strategy[,] clients = new Table_for_strategy[3,3];
        private Table_for_strategy[,] products = new Table_for_strategy[3, 3];

        private void Generate_matrix()
        {
            Type_ABC_XYZ abc  = new Type_ABC_XYZ();
            Type_ABC_XYZ xyz =  new Type_ABC_XYZ();
            for (int i = 0; i<3; i++)
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

                    clients[i, j] = new Table_for_strategy(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>( abc, xyz), CustProd.Customer);
                    products[i, j] = new Table_for_strategy(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>( abc, xyz ), CustProd.Product);
                }
            }
        }
        public void Get_auto()
        {
            Generate_matrix();
            Promo_types pt = new Promo_types();
            clients[0, 0].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
            clients[0,0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.X) , new double[] { 30, 1 });
            clients[0, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.X) , new double[] { 30, 2 });
            clients[0, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.Y), new double[] { 30, 2 });
            clients[0, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.Y), new double[] { 10, 3 });

            clients[1, 0].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
            clients[1, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.X), new double[] { 30, 1 });
            clients[1, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.C, Type_ABC_XYZ.Z), new double[] { 30, 2 });
            clients[1, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.Y), new double[] { 30, 2 });
            clients[1, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.C, Type_ABC_XYZ.Y), new double[] { 10, 3 });

            clients[0, 1].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
            clients[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.X), new double[] { 20, 2 });
            clients[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.X), new double[] { 30, 3 });
            clients[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.Y), new double[] { 30, 2 });
            clients[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.Z), new double[] { 20, 1 });

            products[0, 0].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
            products[0, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.X), new double[] { 1});
            products[0, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.X), new double[] { 2 });
            products[0, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.Z), new double[] { 2 });
            products[0, 0].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.Y), new double[] { 3 });

            products[0, 1].Set_prob_of_disc(new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>());
            products[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.X), new double[] { 1 });
            products[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.X), new double[] { 2 });
            products[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.B, Type_ABC_XYZ.Y), new double[] { 2 });
            products[0, 1].Set_one_prob_of_disc(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.C, Type_ABC_XYZ.Z), new double[] { 3 });

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


    }

    //class Promo_srategy
    //{
    //    private Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, List<T>double[]> clients = new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>();
    //    private Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]> products = new Dictionary<Tuple<Type_ABC_XYZ, Type_ABC_XYZ>, double[]>();
    //    private void Generate_matrix()
    //    {
    //        Type_ABC_XYZ abc = new Type_ABC_XYZ();
    //        Type_ABC_XYZ xyz = new Type_ABC_XYZ();
    //        for (int i = 0; i < 3; i++)
    //        {
    //            if (i == 0)
    //                abc = Type_ABC_XYZ.A;
    //            else if (i == 1)
    //                abc = Type_ABC_XYZ.B;
    //            else abc = Type_ABC_XYZ.C;
    //            for (int j = 0; j < 3; j++)
    //            {
    //                if (j == 0)
    //                    xyz = Type_ABC_XYZ.X;
    //                else if (j == 1)
    //                    xyz = Type_ABC_XYZ.Y;
    //                else xyz = Type_ABC_XYZ.Z;

    //                clients.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc, xyz), new double[2]);
    //                products.Add(new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(abc, xyz), new double[2]);

    //            }
    //        }
    //    }
    //    public void Get_auto()
    //    {
    //        Generate_matrix();
    //        Promo_types pt = new Promo_types();
    //        Tuple<Type_ABC_XYZ, Type_ABC_XYZ> t = new Tuple<Type_ABC_XYZ, Type_ABC_XYZ>(Type_ABC_XYZ.A, Type_ABC_XYZ.X);

    //        clients[t]
    //        clients[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 30, 1 });
    //        clients[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 30, 1 });
    //        clients[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 30, 2 });
    //        clients[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 30, 2 });
    //        clients[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 10, 3 });

    //        clients[1, 0].Set_prob_of_disc(new Dictionary<Type_ABC_XYZ[], double[]>());
    //        clients[1, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 30, 1 });
    //        clients[1, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 30, 2 });
    //        clients[1, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 30, 2 });
    //        clients[1, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 10, 3 });

    //        clients[0, 1].Set_prob_of_disc(new Dictionary<Type_ABC_XYZ[], double[]>());
    //        clients[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 20, 2 });
    //        clients[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 30, 3 });
    //        clients[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 30, 2 });
    //        clients[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 20, 1 });

    //        products[0, 0].Set_prob_of_disc(new Dictionary<Type_ABC_XYZ[], double[]>());
    //        products[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 1 });
    //        products[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 2 });
    //        products[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 2 });
    //        products[0, 0].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 3 });

    //        products[0, 1].Set_prob_of_disc(new Dictionary<Type_ABC_XYZ[], double[]>());
    //        products[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 1 });
    //        products[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 2 });
    //        products[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 2 });
    //        products[0, 1].Set_one_prob_of_disc(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 3 });

    //    }

    //    public Table_for_strategy[,] Get_clients()
    //    {
    //        return clients;
    //    }
    //    public void Set_clients(Table_for_strategy[,] clients)
    //    {
    //        this.clients = clients;
    //    }
    //    public Table_for_strategy[,] Get_products()
    //    {
    //        return products;
    //    }
    //    public void Set_products(Table_for_strategy[,] products)
    //    {
    //        this.products = products;
    //    }
    //}
}
