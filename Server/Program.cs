using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Client.Pages;
using Server.Components;
using Server.Components.Account;
using Server.Data;
using Common.Interaces;
using Common.DAL;
using Microsoft.AspNetCore.Authorization;
using Server.Authorization.Handlers;
using Server.Authorization.Policies;
using Server.Data.DALs;
using Common.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents()
    .AddAuthenticationStateSerialization();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
    })
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options =>
    {
        options.SignIn.RequireConfirmedAccount = true;
        options.Stores.SchemaVersion = IdentitySchemaVersions.Version3;
    })
    	
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();

// Add Authorization Handlers and Policies
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("EduEmailPolicy", policy =>
        policy.Requirements.Add(new EduEmailRequirement()));
});
 
builder.Services.AddScoped<IAuthorizationHandler, EduEmailHandler>();

// Add Data Access Layer Services
	
builder.Services.AddTransient<IUsersDAL, UsersDAL>();
builder.Services.AddTransient<IBadgesDAL, BadgesDALMock>();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

// Add default admin and roles if they don't exist
 
using (var scope = app.Services.CreateScope())
{
    // Add roles if they don't exist
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roleNames = new[] { "Admin", "Editors", "Evaluators" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            IdentityRole role = new IdentityRole(roleName);
            await roleManager.CreateAsync(role);
        }
    }
 
    // Add default admin user if they don't exist
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string? email = builder.Configuration.GetSection("Admin:Email").Value;
    string? password = builder.Configuration.GetSection("Admin:Password").Value;
 
    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password))
    {
        var user = new ApplicationUser();
        user.Email = email;
        user.UserName = email;
 
        // Optional, add if you want the account live right away without email confirmation
        // user.EmailConfirmed = true;
 
        var results = await userManager.CreateAsync(user, password);
 
        if (results.Succeeded)
        {
            await userManager.AddToRoleAsync(user, "Admin");
        }
    }
}

app.Run();
