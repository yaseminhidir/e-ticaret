using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class CustomerToken
    {
        public string Token { get; set; }
        public int? CustomerId { get; set; }
        public int Id { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
