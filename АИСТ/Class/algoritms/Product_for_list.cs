using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.algoritms
{
    class Product_for_list : IComparable
    {
        public string prod_id;

        public string Get_Id()
        {
            return prod_id;
        }
        public void Set_id(string prod_id)
        {
            this.prod_id = prod_id;
        }
         
        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
    class Product_for_list_client : Product_for_list
    {
        public double sum = 0;
        public double purchase_value = 0;
       
        public int CompareTo(Product_for_list_client prod, CompareType compType, bool toMax)
        {
            if (compType == CompareType.cost)
                return this.sum.CompareTo(prod.sum);

            else if (compType == CompareType.purchase_value)
                return this.purchase_value.CompareTo(prod.purchase_value);

            else
                return this.prod_id.CompareTo(prod.prod_id);
             
        }
    }
    class Product_for_list_shop : Product_for_list
    {
        public double sum = 0;
        public double amount_on_store = 0;
        public double sell_value = 0;
        public int CompareTo(Product_for_list_shop prod, CompareType compType, bool toMax)
        {
            if (compType == CompareType.cost)
                return this.sum.CompareTo(prod.sum);

            else if (compType == CompareType.amount)
                return this.amount_on_store.CompareTo(prod.amount_on_store);

            else if (compType == CompareType.sell_value)
                return this.sell_value.CompareTo(prod.sell_value);

           else return this.prod_id.CompareTo(prod.prod_id);

        }
    }
}
