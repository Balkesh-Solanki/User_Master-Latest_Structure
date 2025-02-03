using BussinessLayer;
using DataLayer.DbScript;
using Helper.CommonHelpers;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceLayer.Implementations;
using ServiceLayer.Interfaces;
using System.Globalization;
using User_Master.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Configure Database Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))));

// Register BLL & Service Layer
builder.Services.AddScoped<UserBLL>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AuthHelper>();
builder.Services.AddScoped<ILoginService, LoginService>();

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

// Configure supported languages
var supportedCultures = new[]
{
    new CultureInfo("en"), // English
    new CultureInfo("es"), // Spanish
    new CultureInfo("fr")  // French
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new RequestCulture("en");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "User API", Version = "v1" });

    // Register the AcceptLanguageHeaderFilter
    c.OperationFilter<AcceptLanguageHeaderFilter>();
});

var app = builder.Build();

// Use request localization
var localizationOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>().Value;
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Apply localization middleware
//app.UseRequestLocalization();

app.Run();
