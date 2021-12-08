using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.Core.Models
{
    public partial class Address
    {
      

        private bool SerializeOrders { get; set; } = false;

        public bool ShouldSerializeOrders()
        {
            return SerializeOrders;
        }


        private bool SerializeCustomer { get; set; } = false;

        public bool ShouldSerializeCustomer()
        {
            return SerializeCustomer;
        }
    }
}
