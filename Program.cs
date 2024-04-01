using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Todo.AppData;
using Todo.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer("Data Source=DESKTOP-H84B3N0\\SQLEXPRESS;Initial Catalog=ToDo;Integrated Security=True;Trust Server Certificate=True")
   );
builder.Services.AddIdentity<User, IdentityRole>(
     o =>
     {
         o.Password.RequiredUniqueChars = 0;
         o.Password.RequireUppercase = false;
         o.Password.RequiredLength = 8;
         o.Password.RequireNonAlphanumeric = false;
    })
    .AddEntityFrameworkStores<AppDBContext>().AddDefaultTokenProviders();

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
