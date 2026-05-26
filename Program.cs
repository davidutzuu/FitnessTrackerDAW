using FitnessTrackerPAW.Application.Interfaces;
using FitnessTrackerPAW.Application.Services;
using FitnessTrackerPAW.Domain;
using FitnessTrackerPAW.Infrastructure;
using FitnessTrackerPAW.Infrastructure.Repositories;
using FitnessTrackerPAW.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// 1. Configurare Baza de Date cu RetryOnFailure (Special pentru Docker)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
    sqlOptions => sqlOptions.EnableRetryOnFailure(
        maxRetryCount: 5,
        maxRetryDelay: TimeSpan.FromSeconds(30),
        errorNumbersToAdd: null)));

// 2. Configurare Identity (Autentificare, User si Role)
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// 3. Configurare Cookie-uri (Redirectionare MVC)
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// 4. Dependency Injection (Layer-ul de Servicii si Repository)
builder.Services.AddScoped<ISupplementRepository, SupplementRepository>();
builder.Services.AddScoped<ISupplementService, SupplementService>();

// 5. Activare suport pentru API si interfata Hibrid (MVC + Razor)
builder.Services.AddControllersWithViews();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 6. Aplicarea automata a migratiilor, CREAREA ROLURILOR si A CONTULUI DE ADMIN
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>(); // Adaugat pentru contul de admin

    try
    {
        Console.WriteLine("[DOCKER-FIX] Se asteapta 15 secunde pentru initializarea completa a SQL Server...");
        System.Threading.Thread.Sleep(15000);

        Console.WriteLine("[DOCKER-FIX] Se aplica migratiile in baza de date...");
        dbContext.Database.Migrate();
        Console.WriteLine("[DOCKER-FIX] Migrarile s-au aplicat cu succes, tabelele sunt gata!");

        // ---- AICI SE CREEAZA EFECTIV ROLURILE CERUTE DE PROFESOR ----
        string[] roleNames = { "Admin", "User" };
        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new Role { Name = roleName, NormalizedName = roleName.ToUpper() });
                Console.WriteLine($"[IDENTITY] Rolul '{roleName}' a fost generat in baza de date.");
            }
        }

        // ---- CREARE CONT ADMIN DE TEST PENTRU VERIFICARE ----
        var adminEmail = "admin@test.ro";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            var newAdmin = new User { UserName = adminEmail, Email = adminEmail };
            var result = await userManager.CreateAsync(newAdmin, "ParolaAdmin123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(newAdmin, "Admin");
                Console.WriteLine($"[IDENTITY] Contul suprem {adminEmail} a fost creat cu succes!");
            }
        }
        // -------------------------------------------------------------
    }
    catch (Exception ex)
    {
        Console.WriteLine("[DOCKER-FIX] Eroare la aplicarea migratiilor sau rolurilor: " + ex.Message);
    }
}

// 7. Middlewares
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();

// Middleware-ul pentru tratarea globala a erorilor
app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

// 8. Rutele aplicatiei
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();