using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Models
{
    public partial class CartItem
    {
        public decimal? TotalPrice{
            get
            {
                decimal? CartTotalPrice = 0;
                if (Product.CalculatedPrice != null)
                {
                    CartTotalPrice = Product.CalculatedPrice * Quantity;
                   
                }
                else
                {
                    CartTotalPrice = Product.Price * Quantity;
                }
                return CartTotalPrice;
            }
}
    }
}
