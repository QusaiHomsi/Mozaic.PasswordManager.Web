using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Mozaic.PasswordManager.Web.Models.DBEntities;
using Mozaic.PasswordManager.Web.Models;
using System.Text;
using Mozaic.PasswordManager.DAL;
using Mozaic.PasswordManager.Web;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog logging
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Error()
    .WriteTo.File("logs/error-log.txt", rollingInterval: RollingInterval.Day)
    .WriteTo.MSSqlServer(
        connectionString: builder.Configuration.GetConnectionString("DefaultConnection"),
        tableName: "Logs",
        autoCreateSqlTable: true,
        restrictedToMinimumLevel: LogEventLevel.Error
    ).CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<GlobalExceptionFilter>();
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// JWT Configuration
var jwtKey = builder.Configuration["JwtConfig:Key"];
var jwtIssuer = builder.Configuration["JwtConfig:Issuer"];
var jwtAudience = builder.Configuration["JwtConfig:Audience"];

if (string.IsNullOrEmpty(jwtKey) || string.IsNullOrEmpty(jwtIssuer) || string.IsNullOrEmpty(jwtAudience))
{
    throw new InvalidOperationException("JWT configuration is missing.");
}

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});

// Build the app
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseMiddleware<JwtCookieMiddleware>();
app.UseStatusCodePages(async context =>
{
    if (context.HttpContext.Response.StatusCode == 401) // Unauthorized
    {
        context.HttpContext.Response.Redirect("/Home/Unauthorized");
    }
});
app.UseAuthentication();
app.UseAuthorization();

// Configure routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
