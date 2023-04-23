using eShopFinalProject.Data.Infrastructure.Interface;
using eShopFinalProject.Data.Infrastructure;
using eShopFinalProject.Services.Colors;
using eShopFinalProject.API;
using Microsoft.EntityFrameworkCore;
using eShopFinalProject.Data.EF;
using eShopFinalProject.Utilities.Common;
using eShopFinalProject.Services.Categories;
using eShopFinalProject.Services.Brands;
using eShopFinalProject.Services.Coupons;
using eShopFinalProject.Services.Products;
using Microsoft.AspNetCore.Identity;
using eShopFinalProject.Data.Entities;
using eShopFinalProject.Services.Users;
using Microsoft.OpenApi.Models;
using FluentValidation.AspNetCore;
using eShopFinalProject.Utilities.ViewModel.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using eShopFinalProject.Services.Jwts;
using eShopFinalProject.Services.JwtService;
using System.Text;
using eShopFinalProject.Services.Blogs;
using eShopFinalProject.Services.Enqs;
using eShopFinalProject.Services.Uploads;
using eShopFinalProject.Utilities.ViewModel.Mails;
using Microsoft.Extensions.Configuration;
using eShopFinalProject.Services.Mails;
using eShopFinalProject.Services.Images;
using eShopFinalProject.Services.Carts;
using eShopFinalProject.Services.Orders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShop Solution", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                  {
                    {
                      new OpenApiSecurityScheme
                      {
                        Reference = new OpenApiReference
                          {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                          },
                          Scheme = "oauth2",
                          Name = "Bearer",
                          In = ParameterLocation.Header,
                        },
                        new List<string>()
                      }
                    });
});

builder.Services.AddDbContext<eShopDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("eShopSolutionDb")));

builder.Services.AddIdentity<AppUser, AppRole>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.User.RequireUniqueEmail = true;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;

    options.SignIn.RequireConfirmedEmail = true;
})
    .AddEntityFrameworkStores<eShopDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<ICouponService, CouponService>();
builder.Services.AddTransient<IColorService, ColorService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<IBlogService, BlogService>();
builder.Services.AddTransient<IEnqService, EnqService>();
builder.Services.AddTransient<IJwtService, JwtService>();
builder.Services.AddTransient<IImageService, ImageService>();
builder.Services.AddTransient<IMailService, MailService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IOrderService, OrderService>();

builder.Services.AddTransient<IAppRoleRepository, AppRoleRepository>();
builder.Services.AddTransient<IAppUserRepository, AppUserRepository>();
builder.Services.AddTransient<IBlogRepository, BlogRepository>();
builder.Services.AddTransient<IBrandRepository, BrandRepository>();
builder.Services.AddTransient<ICategoryRepository, CategoryRepository>();
builder.Services.AddTransient<IColorRepository, ColorRepository>();
builder.Services.AddTransient<ICouponRepository, CouponRepository>();
builder.Services.AddTransient<IOrderRepository, OrderRepository>();
builder.Services.AddTransient<ICartRepository, CartRepository>();
builder.Services.AddTransient<IProductInColorRepository, ProductInColorRepository>();
builder.Services.AddTransient<IProductInOrderRepository, ProductInOrderRepository>();
builder.Services.AddTransient<IProductInCartRepository, ProductInCartRepository>();
builder.Services.AddTransient<IProductInWishRepository, ProductInWishRepository>();
builder.Services.AddTransient<IProductRatingRepository, ProductRatingRepository>();
builder.Services.AddTransient<IProductRepository, ProductRepository>();
builder.Services.AddTransient<IEnqRepository, EnqRepository>();
builder.Services.AddTransient<IImageRepository, ImageRepository>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();

builder.Services
    .AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
            )
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddCors(options => options.AddPolicy("corsapp",
    policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()
    ));
builder.Services.Configure<MailSettings>(builder.Configuration.GetSection("MailSettings"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseMiddleware<EShopExceptionFilterAttribute>();

app.UseCors("corsapp");
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();

app.MapControllers();

app.Run();
