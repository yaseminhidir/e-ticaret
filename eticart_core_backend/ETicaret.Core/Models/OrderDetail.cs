using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class OrderDetail
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public int? ProductId { get; set; }
        public decimal? ProductPrice { get; set; }
        public int? Quantity { get; set; }
        public bool IsCanceled { get; set; }

        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }


    }
}
