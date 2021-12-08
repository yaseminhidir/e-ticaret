using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Addresses = new HashSet<Address>();
            CustomerTokens = new HashSet<CustomerToken>();
            Favs = new HashSet<Fav>();
            Orders = new HashSet<Order>();
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Password { get; set; }
        public string Salt { get; set; }
        public DateTime? DateAdded { get; set; }
        public int Id { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<CustomerToken> CustomerTokens { get; set; }
        public virtual ICollection<Fav> Favs { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
