using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Models
{
    public partial class OrderDetail
    {
        private bool SerializeOrder { get; set; } = false;

        public bool ShouldSerializeOrder()
        {
            return SerializeOrder;
        }



    }
}
