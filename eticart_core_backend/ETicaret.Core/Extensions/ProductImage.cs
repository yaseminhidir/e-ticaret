using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Models
{
    public partial class ProductImage
    {
        private bool SerializeProduct { get; set; } = false;

        public bool ShouldSerializeProduct()
        {
            return SerializeProduct;//true donerse json a ekliyo false dönerse eklemiyor.
        }
    }
}
