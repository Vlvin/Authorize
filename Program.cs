using Authorize.DBModels;
using Authorize.Scripts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var config = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(options => 
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
// }).AddJwtBearer(options => {
//     options.SaveToken = true;
//     options.Audience = options.Authority = config["JWT:ValidAudience"];
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidIssuer = config["JWT:ValidIssuer"],
//         ValidAudience = config["JWT:ValidAudience"],
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Secret"] ?? throw new InvalidOperationException("JWT:Secret is not set"))),
//     };
}).AddCookie(JwtBearerDefaults.AuthenticationScheme);
builder.Services.AddAuthorization();

var connectionString = config.GetConnectionString("Default");
builder.Services.AddDbContext<AuthorizeContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddIdentity<User, IdentityRole>(options => {
    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = false;
    }).AddEntityFrameworkStores<AuthorizeContext>();
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
}
app.UseHsts();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
