﻿using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public class AppRoleRepository : BaseRepository<AppRole>
    {
        public AppRoleRepository(eShopDbContext context) : base(context){}
    }
}
