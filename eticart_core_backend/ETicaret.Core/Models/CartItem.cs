using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class CartItem
    {
        public int Id { get; set; }
        public int? ProductId { get; set; }
        public string CustomerIdentify { get; set; }
        public int? Quantity { get; set; }

        public virtual Product Product { get; set; }
    }
}
