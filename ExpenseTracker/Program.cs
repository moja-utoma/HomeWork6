using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // додай контролери (обов'язково)
builder.Services.AddEndpointsApiExplorer(); // для Swagger
builder.Services.AddSwaggerGen();

var conString = builder.Configuration.GetConnectionString("ExpenseDB") ??
     throw new InvalidOperationException("Connection string 'ExpenseDB'" +
    " not found.");

builder.Services.AddDbContext<AppContext>(options =>
    options.UseNpgsql(conString));

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
