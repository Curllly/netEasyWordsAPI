using System.Text;
using EasyWordsAPI.Data;
using EasyWordsAPI.Models;
using EasyWordsAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("sqlite")!;
builder.Services.AddDbContext<EasyWordsContext>(opt => opt.UseSqlite(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuer = true, // провереяем издателя
            ValidateAudience = false, // не проверяем аудиторию
            ValidateLifetime = true, // проверяем время жизни
            ValidateIssuerSigningKey = true, // проверяем ключ
            ValidIssuer = "easywords",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.Default.GetBytes(TokenService.Key))
        };
    });
builder.Services
    .AddIdentity<IdentityUser<int>, IdentityRole<int>>(opt =>
    {
        opt.Password.RequiredLength = 8;
        opt.Password.RequireDigit = true;
        opt.Password.RequireLowercase = true;
        opt.Password.RequireUppercase = true;
        opt.User.RequireUniqueEmail = true;
    })
.AddEntityFrameworkStores<EasyWordsContext>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roles = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    if (!roles.Roles.Any())
    {
        await roles!.CreateAsync(new IdentityRole<int> { Name = "admin" });
        await roles!.CreateAsync(new IdentityRole<int> { Name = "client" });
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
