using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Models
{
    public partial class OrderStatus
    {
      

        private bool SerializeOrders { get; set; } = false;

        public bool ShouldSerializeOrders()
        {
            return SerializeOrders;
        }
    }
}
