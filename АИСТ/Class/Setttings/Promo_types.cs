using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.Setttings
{
    class Promo_types
    {
        private Client_type_disc[,] clients = new Client_type_disc[3,3];
        private Product_type_disc[,] products = new Product_type_disc[3, 3];

        private void Generate_matrix()
        {
            Type_ABC_XYZ abc;
            Type_ABC_XYZ xyz;
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
                    clients[i, j].client_type_abc = abc;
                    clients[i, j].client_type_xyz = xyz;
                    products[i, j].prod_abc = abc;
                    products[i, j].prod_xyz = xyz;
                }
            }
        }
        public void Get_auto()
        {
            Generate_matrix();
            Promo_types pt = new Promo_types();
            clients[0, 0].disc_prob = new Dictionary<Type_ABC_XYZ[], double[]>();
            clients[0, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 30, 1 });
            clients[0, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 30, 2 });
            clients[0, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 30, 2 });
            clients[0, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 10, 3 });

            clients[1, 0].disc_prob = new Dictionary<Type_ABC_XYZ[], double[]>();
            clients[1, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 30, 1 });
            clients[1, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 30, 2 });
            clients[1, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 30, 2 });
            clients[1, 0].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 10, 3 });

            clients[0, 1].disc_prob = new Dictionary<Type_ABC_XYZ[], double[]>();
            clients[0, 1].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, new double[] { 20, 2 });
            clients[0, 1].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, new double[] { 30, 3 });
            clients[0, 1].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, new double[] { 30, 2 });
            clients[0, 1].disc_prob.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, new double[] { 20, 1 });

            products[0, 0].client_prior = new Dictionary<Type_ABC_XYZ[], int>();
            products[0, 0].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, 1);
            products[0, 0].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, 2);
            products[0, 0].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, 2);
            products[0, 0].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, 3);

            products[0, 1].client_prior = new Dictionary<Type_ABC_XYZ[], int>();
            products[0, 1].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.X }, 1);
            products[0, 1].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.X }, 2);
            products[0, 1].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.A, Type_ABC_XYZ.Y }, 2);
            products[0, 1].client_prior.Add(new Type_ABC_XYZ[] { Type_ABC_XYZ.B, Type_ABC_XYZ.Y }, 3);


            //Prod_type_dis pd = new Prod_type_dis();
            //pd.prod_abc = Type_ABC.A;
            //pd.prod_xyz = Type_XYZ.X;
            //p
        }

        public Client_type_disc[,] Get_clients()
        {
            return clients;
        }
        public void Set_clients(Client_type_disc[,] clients)
        {
            this.clients = clients;
        }
        public Product_type_disc[,] Get_products()
        {
            return products;
        }
        public void Set_products(Product_type_disc[,] products)
        {
            this.products = products;
        }
    }
}
