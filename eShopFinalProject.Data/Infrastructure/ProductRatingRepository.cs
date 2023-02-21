﻿using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public class ProductRatingRepository : BaseRepository<ProductRating>
    {
        public ProductRatingRepository(eShopDbContext context) : base(context) { }
    }
}
