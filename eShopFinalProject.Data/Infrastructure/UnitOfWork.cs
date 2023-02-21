using eShopFinalProject.Data.EF;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Data.Infrastructure.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopFinalProject.Data.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Properties
        private eShopDbContext _context;
        private IBaseRepository<AppRole> _appRoleRepository;
        private IBaseRepository<AppUser> _appUserRepository;
        private IBaseRepository<Blog> _blogRepository;
        private IBaseRepository<Brand> _brandRepository;
        private IBaseRepository<Category> _categoryRepository;
        private IBaseRepository<Color> _colorRepository;
        private IBaseRepository<Coupon> _couponRepository;
        private IBaseRepository<Order> _orderRepository;
        private IBaseRepository<Product> _productRepository;
        private IBaseRepository<ProductInColor> _productInColorRepository;
        private IBaseRepository<ProductInOrder> _productInOrderRepository;
        private IBaseRepository<ProductRating> _productRatingRepository;
        #endregion

        #region Constructor
        public UnitOfWork(eShopDbContext context)
        {
            _context = context;
        }
        #endregion

        #region Implements
        public IBaseRepository<AppRole> AppRoleRepository
        {
            get
            {
                if (_appRoleRepository == null)
                {
                    _appRoleRepository = new AppRoleRepository(_context);
                }

                return _appRoleRepository;
            }
        }

        public IBaseRepository<AppUser> AppUserRepository
        {
            get
            {
                if (_appUserRepository == null)
                {
                    _appUserRepository = new AppUserRepository(_context);
                }

                return _appUserRepository;
            }
        }

        public IBaseRepository<Blog> BaseRepository
        {
            get
            {
                if (_blogRepository == null)
                {
                    _blogRepository = new BlogRepository(_context);
                }

                return _blogRepository;
            }
        }

        public IBaseRepository<Brand> BrandRepository
        {
            get
            {
                if (_brandRepository == null)
                {
                    _brandRepository = new BrandRepository(_context);
                }

                return _brandRepository;
            }
        }

        public IBaseRepository<Category> CategoryRepository
        {
            get
            {
                if (_categoryRepository == null)
                {
                    _categoryRepository = new CategoryRepository(_context);
                }

                return _categoryRepository;
            }
        }

        public IBaseRepository<Color> ColorRepository
        {
            get
            {
                if (_colorRepository == null)
                {
                    _colorRepository = new ColorRepository(_context);
                }

                return _colorRepository;
            }
        }

        public IBaseRepository<Coupon> CouponRepository
        {
            get
            {
                if (_couponRepository == null)
                {
                    _couponRepository = new CouponRepository(_context);
                }

                return _couponRepository;
            }
        }

        public IBaseRepository<Order> OrderRepository
        {
            get
            {
                if (_orderRepository == null)
                {
                    _orderRepository = new OrderRepository(_context);
                }

                return _orderRepository;
            }
        }

        public IBaseRepository<Product> ProductRepository
        {
            get
            {
                if (_productRepository == null)
                {
                    _productRepository = new ProductRepository(_context);
                }

                return _productRepository;
            }
        }

        public IBaseRepository<ProductInColor> ProductInColorRepository
        {
            get
            {
                if (_productInColorRepository == null)
                {
                    _productInColorRepository = new ProductInColorRepository(_context);
                }

                return _productInColorRepository;
            }
        }

        public IBaseRepository<ProductInOrder> ProductInOrderRepository
        {
            get
            {
                if (_productInOrderRepository == null)
                {
                    _productInOrderRepository = new ProductInOrderRepository(_context);
                }

                return _productInOrderRepository;
            }
        }

        public IBaseRepository<ProductRating> ProductRatingRepository
        {
            get
            {
                if (_productRatingRepository == null)
                {
                    _productRatingRepository = new ProductRatingRepository(_context);
                }

                return _productRatingRepository;
            }
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        #endregion
    }
}
