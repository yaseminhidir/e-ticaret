using System;
using System.Collections.Generic;

#nullable disable

namespace ETicaret.Core.Models
{
    public partial class Product
    {
        public Product()
        {
            CartItems = new HashSet<CartItem>();
            Favs = new HashSet<Fav>();
            OrderDetails = new HashSet<OrderDetail>();
            ProductCategories = new HashSet<ProductCategory>();
            ProductImages = new HashSet<ProductImage>();
        }

        public int Id { get; set; }
        public string Model { get; set; }
        public int? Quantity { get; set; }
        public int? ManufacturerId { get; set; }
        public decimal? Price { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime DateModified { get; set; }
        public int? StockStatuId { get; set; }
        public string Name { get; set; }
        public bool IsDeleted { get; set; }
        public string Description { get; set; }
        public string Information { get; set; }
        public int? Discount { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }
        public virtual StockStatus StockStatu { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public virtual ICollection<Fav> Favs { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductCategory> ProductCategories { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
    }
}
