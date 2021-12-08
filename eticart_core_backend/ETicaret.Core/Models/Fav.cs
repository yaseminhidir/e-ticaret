using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class Fav
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
