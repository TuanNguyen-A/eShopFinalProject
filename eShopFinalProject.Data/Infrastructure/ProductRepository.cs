﻿using AutoMapper;
using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Utilities.ViewModel.Products;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public interface IProductRepository : IBaseRepository<Product>
    {
    }
    public class ProductRepository : BaseRepository<Product>, IProductRepository
    {
        public ProductRepository(eShopDbContext context) : base(context)
        {
        }

        public override async Task<List<Product>> AllAsync()
        {
            return await _context.Set<Product>()
                .Include(i => i.Brand)
                .Include(i => i.Category)
                .Include(i => i.Images)
                .Include(i => i.ProductInColors)
                .ThenInclude(pic => pic.Color)
                .Include(i => i.ProductRatings)
                .ThenInclude(i => i.User)
                .ToListAsync();
        }

        public override async Task<Product> GetAsync(int id)
        {
            return await _context.Set<Product>()
                .Include(i => i.Brand)
                .Include(i => i.Category)
                .Include(i => i.Images)
                .Include(i => i.ProductInColors)
                .ThenInclude(pic => pic.Color)
                .Include(i => i.ProductRatings)
                .ThenInclude(i => i.User)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
