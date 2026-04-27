using MongoDB.Driver;
using Microsoft.Extensions.Options;
using ApiMongoTreino.Configurations;
using ApiMongoTreino.Service;
using ApiMongoTreino.Models;
using Microsoft.AspNetCore.Mvc;
using ApiMongoTreino.Service.Interface;
using ApiMongoTreino.Service.Payment.Strategies;
using ApiMongoTreino.Service.Payment.Factories;
using ApiMongoTreino.Service.Payment;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services.AddControllers(options =>
{
    options.Filters.Add<ValidationFilter>();
});

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Mongo settings
builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

// Mongo client
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    return new MongoClient(settings.ConnectionString);
});

// Collections
builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();

    var database = client.GetDatabase(settings.DatabaseName);

    return database.GetCollection<Product>("products");
});

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();

    var database = client.GetDatabase(settings.DatabaseName);

    return database.GetCollection<Customer>("customers");
});

builder.Services.AddScoped(sp =>
{
    var settings = sp.GetRequiredService<IOptions<MongoDbSettings>>().Value;
    var client = sp.GetRequiredService<IMongoClient>();

    var database = client.GetDatabase(settings.DatabaseName);

    return database.GetCollection<Request>("requests");
});

// Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IRequestService, RequestService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

builder.Services.AddScoped<PixPaymentProcessor>();
builder.Services.AddScoped<CreditCardPaymentProcessor>();
builder.Services.AddScoped<TicketPaymentProcessor>();
builder.Services.AddScoped<IPaymentProcessorFactory,PaymentProcessorFactory>();


// ✔ Notification Pattern (ESSENCIAL)
builder.Services.AddScoped<IApplicationNotificationHandler, ApplicationNotificationHandler>();

// API behavior
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();