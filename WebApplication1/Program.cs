using BookStoreApi.Interface;
using BookStoreApi.Models;
using BookStoreApi.Service;
using BookStoreApi.Services;
using BookStoreApi.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.Configure<BookStoreDatabaseSettings>(
    builder.Configuration.GetSection("BookStoreDatabase"));

//builder.Services.AddSingleton<BooksService>();
//builder.Services.AddScope<IRepository<Book>, MongoRepository<Book>>();
//builder.Services.AddSingleton<IRepository<Magazine>, MongoRepository<Magazine>>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(MongoRepository<>));
builder.Services.AddScoped<IBooksService, BooksService>();
builder.Services.AddScoped<IMagazineService, MagazineService>();
builder.Services.AddScoped<IAllService, AllService>();
builder.Services.AddScoped<IValidator<Book>, BookValidator>();
builder.Services.AddScoped<IValidator<Magazine>, MagazineValidator>();
builder.Services.AddScoped<IValidator<CombinedDTOModels>, CombinedDTOValidator>();
//builder.Services.AddScoped<ValidationContext<Book>>();
//builder.Services.AddScoped<ValidationContext<Magazine>>();
//builder.Services.AddScoped<ValidationContext<CombinedDTOModels>>();
//builder.Services.AddScoped<IValidator<>, >();



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
