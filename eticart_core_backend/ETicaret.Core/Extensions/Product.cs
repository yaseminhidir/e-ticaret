using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Models
{
    public partial class Product
    {
        public decimal? CalculatedPrice
        {
            get
            {
                var newPrice = Price-((Price* Discount) /100);
                return newPrice;
            }
        }

        private bool SerializeOrderDetail { get; set; } = false;

        public bool ShouldSerializeOrderDetail()
        {
            return SerializeOrderDetail;
        }
    }
}
