﻿using eShopFinalProject.Utilities.ViewModel.Brands;
using eShopFinalProject.Utilities.ViewModel.Categories;
using eShopFinalProject.Utilities.ViewModel.Colors;
using eShopFinalProject.Utilities.ViewModel.Uploads;
using eShopFinalProject.Utilities.ViewModel.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace eShopFinalProject.Utilities.ViewModel.Products
{
    public class ProductVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int Sold { get; set; }
        public string Tag { get; set; }
        public List<ColorVM> Colors { get; set; }
        public BrandVM Brand { get; set; }
        public CategoryVM Category { get; set; }
        public List<ImageVM> Images { get; set; }
        public virtual List<ProductRatingForProductVM> ProductRatings { get; set; }
    }

    public class ProductRatingForProductVM
    {
        public int Star { get; set; }
        public string Comment { get; set; }
        public UserVM User { get; set; }
    }
}
