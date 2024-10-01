using System.Globalization;
using BmsKhameleon.Core.Domain.IdentityEntities;
using BmsKhameleon.Core.Domain.RepositoryContracts;
using BmsKhameleon.Core.ServiceContracts;
using BmsKhameleon.Core.Services;
using BmsKhameleon.Infrastructure.DbContexts;
using BmsKhameleon.Infrastructure.Repositories;
using BmsKhameleon.UI.Factories;
using BmsKhameleon.UI.Handlers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IdentityDbContext = BmsKhameleon.Infrastructure.DbContexts.IdentityDbContext;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
});

builder.Services.AddHsts(options =>
{
    options.Preload = true;
    options.IncludeSubDomains = true;
    options.MaxAge = TimeSpan.FromDays(365);
});

//DbContexts
if (builder.Environment.IsDevelopment())
{
    builder.Services.AddDbContext<AccountDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });

    builder.Services.AddDbContext<IdentityDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
}
else
{
    builder.Services.AddDbContext<AccountDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("AspSmarterConnectionString"));
    });
    builder.Services.AddDbContext<IdentityDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("AspSmarterConnectionString"));
    });
}


//Identity
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true; 
    options.Password.RequireNonAlphanumeric = false; 
    options.Password.RequireUppercase = true; 
    options.Password.RequiredLength = 6; 
})
    .AddEntityFrameworkStores<AccountDbContext>()
    .AddDefaultTokenProviders()
    .AddUserStore<UserStore<ApplicationUser, ApplicationRole, IdentityDbContext, Guid>>()
    .AddRoleStore<RoleStore<ApplicationRole, IdentityDbContext, Guid>>();

//Authorization
builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
    options.LoginPath = "/";
    //options.AccessDeniedPath = "/Authentication/Authentication";
    options.SlidingExpiration = true;
});


//services
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<ITransactionsService, TransactionsService>();
builder.Services.AddScoped<IMonthlyBalancesService, MonthlyBalancesService>();

//repositories
builder.Services.AddScoped<IAccountsRepository, AccountsRepository>();
builder.Services.AddScoped<ITransactionsRepository, TransactionsRepository>();
builder.Services.AddScoped<IMonthlyBalancesRepository, MonthlyBalancesRepository>();

//factories
builder.Services.AddTransient<IUpdateTransactionHandler, DepositCashUpdateTransactionHandler>();
builder.Services.AddTransient<IUpdateTransactionHandler, DepositChequeUpdateTransactionHandler>();
builder.Services.AddTransient<IUpdateTransactionHandler, WithdrawCashUpdateTransactionHandler>();
builder.Services.AddTransient<IUpdateTransactionHandler, WithdrawChequeUpdateTransactionHandler>();
builder.Services.AddTransient<UpdateTransactionHandlerFactory>();

//build
var app = builder.Build();

// Ensure the app uses PH culture
var supportedCultures = new[] { "en-PH" };
var localizationOptions = new RequestLocalizationOptions()
    .SetDefaultCulture(supportedCultures[0])
    .AddSupportedCultures(supportedCultures)
    .AddSupportedUICultures(supportedCultures);
app.UseRequestLocalization(localizationOptions);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
