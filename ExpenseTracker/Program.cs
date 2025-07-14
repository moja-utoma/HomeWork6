using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var conString = builder.Configuration.GetConnectionString("ExpenseDB") ??
     throw new InvalidOperationException("Connection string 'ExpenseDB'" +
    " not found.");

builder.Services.AddDbContext<AppContext>(options =>
    options.UseNpgsql(conString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/", () =>
{

});

app.Run();
