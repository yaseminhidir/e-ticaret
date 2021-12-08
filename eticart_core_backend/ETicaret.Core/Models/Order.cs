using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public decimal? TotalPrice { get; set; }
        public int? AddressId { get; set; }
        public int OrderStatuId { get; set; }
        public DateTime? Date { get; set; }

        public virtual Address Address { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual OrderStatus OrderStatu { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
