﻿using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class ProductsCategory
    {
        public int Id { get; set; }
        public int? CategoryId { get; set; }
        public int? ProductId { get; set; }

        public virtual Product Product { get; set; }
    }
}
