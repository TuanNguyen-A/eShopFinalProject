﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual List<Product> Products { get; set; }
        public virtual List<Blog> Blogs { get; set; }
    }
}
