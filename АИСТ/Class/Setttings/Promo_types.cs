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
        static Client_type_disc client_A = new Client_type_disc();
        static Client_type_disc client_B = new Client_type_disc();
        static Client_type_disc client_C = new Client_type_disc();
        public static Promo_types Get_auto()
        {
            Promo_types pt = new Promo_types();
            client_A.client_type = Type_ABC.A;
            client_A.disc_prob = new Dictionary<Type_XYZ, double[]>();
            client_A.disc_prob.Add(Type_XYZ.X, new double[] {80, 3 } );
            client_A.disc_prob.Add(Type_XYZ.Y, new double[] {20, 2 });
            client_A.disc_prob.Add(Type_XYZ.Z, new double[] { 0, 0 });

            client_B.client_type = Type_ABC.B;
            client_B.disc_prob = new Dictionary<Type_XYZ, double[]>();
            client_B.disc_prob.Add(Type_XYZ.X, new double[] { 20, 2 });
            client_B.disc_prob.Add(Type_XYZ.Y, new double[] { 80, 3 });
            client_B.disc_prob.Add(Type_XYZ.Z, new double[] { 0, 0 });

            client_C = null;
            //client_C.client_type = Type_ABC.C;
            //client_C.disc_prob = new Dictionary<Type_XYZ, double[]>();
            //client_C.disc_prob.Add(Type_XYZ.X, new double[] { 0, 0 });
            //client_C.disc_prob.Add(Type_XYZ.Y, new double[] { 0, 0 });
            //client_C.disc_prob.Add(Type_XYZ.Z, new double[] { 0, 0 });

            //Prod_type_dis pd = new Prod_type_dis();
            //pd.prod_abc = Type_ABC.A;
            //pd.prod_xyz = Type_XYZ.X;
            //p
            return pt;
        }

    }
}
