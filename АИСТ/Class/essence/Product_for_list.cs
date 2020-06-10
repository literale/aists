using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using АИСТ.Class.enums;

namespace АИСТ.Class.algoritms
{
    abstract class Product_for_list : IComparable
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

        abstract public int CompareTo(object obj);

    }
    class Product_for_list_client : Product_for_list
    {
        public double sum = 0;
        public double purchase_value = 0;
        public double prob_by_client = 0;
        public double disc_size_by_client = 0;
        public double prior_by_good = 0;
        public Group g = Group.Product;
        public CompareType compType = CompareType.cost;
       
        public override int CompareTo(object obj)
        {
            Product_for_list_client prod = (Product_for_list_client)obj;
            if (compType == CompareType.cost)
            {
                if (sum != prod.sum)
                    return this.sum.CompareTo(prod.sum);
                else return this.prod_id.CompareTo(prod.prod_id);
            }

            if (compType == CompareType.purchase_value)
            {
                if (purchase_value != prod.purchase_value)
                    return this.purchase_value.CompareTo(prod.purchase_value);
                else
                    return this.prod_id.CompareTo(prod.prod_id);
            }

            else
                return this.prod_id.CompareTo(prod.prod_id);
        }
    }
    class Product_for_list_shop : Product_for_list
    {
        public double sum = 0;
        public double amount_on_store = 0;
        public double sell_value = 0;
        public CompareType compType = CompareType.cost;
        public override int CompareTo(object obj)
        {
            Product_for_list_shop prod = (Product_for_list_shop)obj;
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
