using GiftOfTheGivers.Website.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "Connection string 'DefaultConnection' is not configured. Supply it as an App Service " +
        "connection string, as the environment variable ConnectionStrings__DefaultConnection, " +
        "or via 'dotnet user-secrets' — never in appsettings.json.");
}

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Account confirmation is disabled because no email sender is configured for the prototype.
// Requiring a confirmed account without a sender leaves every new registration unable to sign
// in. Re-enable this together with an IEmailSender implementation before public release.
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

// ---------------------------------------------------------------------------
// Start-up seeding: apply migrations, create the Donor and Employee roles, and
// provision the initial Employee account so that the dashboard is reachable on a
// freshly deployed database.
// ---------------------------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();

    try
    {
        // Bring the database up to the latest migration. On a new Azure SQL database this
        // creates the schema; on an existing one it is a no-op.
        var db = services.GetRequiredService<ApplicationDbContext>();
        db.Database.Migrate();

        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();

        // --- Roles ----------------------------------------------------------
        foreach (var roleName in new[] { "Donor", "Employee" })
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole(roleName));
                if (result.Succeeded)
                {
                    logger.LogInformation("Created role {Role}.", roleName);
                }
                else
                {
                    logger.LogError("Failed to create role {Role}: {Errors}",
                        roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        // --- Initial Employee account ---------------------------------------
        // Credentials are read from configuration (App Service settings, environment
        // variables or user secrets) so that no password is committed to source control.
        // If they are absent, seeding is skipped with a warning rather than falling back
        // to a hard-coded default.
        var seedEmail = app.Configuration["SeedEmployee:Email"];
        var seedPassword = app.Configuration["SeedEmployee:Password"];

        if (string.IsNullOrWhiteSpace(seedEmail) || string.IsNullOrWhiteSpace(seedPassword))
        {
            logger.LogWarning(
                "SeedEmployee:Email / SeedEmployee:Password are not configured. No Employee " +
                "account was seeded, so the Employee Dashboard will be unreachable until an " +
                "account is granted the Employee role.");
        }
        else
        {
            var employee = await userManager.FindByEmailAsync(seedEmail);

            if (employee == null)
            {
                employee = new IdentityUser
                {
                    UserName = seedEmail,
                    Email = seedEmail,
                    EmailConfirmed = true
                };

                var created = await userManager.CreateAsync(employee, seedPassword);
                if (created.Succeeded)
                {
                    logger.LogInformation("Seeded Employee account {Email}.", seedEmail);
                }
                else
                {
                    logger.LogError("Failed to create seed Employee account {Email}: {Errors}",
                        seedEmail, string.Join(", ", created.Errors.Select(e => e.Description)));
                    employee = null;
                }
            }

            if (employee != null && !await userManager.IsInRoleAsync(employee, "Employee"))
            {
                var added = await userManager.AddToRoleAsync(employee, "Employee");
                if (added.Succeeded)
                {
                    logger.LogInformation("Assigned {Email} to the Employee role.", seedEmail);
                }
                else
                {
                    logger.LogError("Failed to add {Email} to the Employee role: {Errors}",
                        seedEmail, string.Join(", ", added.Errors.Select(e => e.Description)));
                }
            }
        }
    }
    catch (Exception ex)
    {
        // Seeding must never prevent the application from starting; a failure here is logged
        // so that it is visible in Azure App Service diagnostics.
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.Run();
