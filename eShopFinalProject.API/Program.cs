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

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<eShopDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("eShopSolutionDb")));

builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<ICouponService, CouponService>();
builder.Services.AddTransient<IColorService, ColorService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

//builder.Services.AddControllers(options => options.Filters.Add<EShopExceptionFilterAttribute>());
builder.Services.AddMvc(options =>
{
    options.Filters.Add(typeof(EShopExceptionFilterAttribute));
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

builder.Services.AddCors(options => options.AddPolicy("corsapp",
    policy => policy.WithOrigins("*").AllowAnyHeader().AllowAnyMethod()
    ));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseMiddleware<EShopExceptionFilterAttribute>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
