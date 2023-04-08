using eShopFinalProject.Data.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public int TotalRating { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public TagStatus Tag { get; set; }

        //------
        public virtual Category Category { get; set; }
        public virtual Brand Brand { get; set; }
        public virtual List<ProductInOrder> ProductInOrders { get; set; }
        public virtual List<ProductInColor> ProductInColors { get; set; }
        public virtual List<ProductRating> ProductRatings { get; set; }
        public virtual List<Image> Images { get; set; }

    }
}
