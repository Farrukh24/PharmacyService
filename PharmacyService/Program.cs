using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PharmacyService.Authentication;
using PharmacyService.Contracts.Interfaces;
using PharmacyService.Contracts.Models;
using PharmacyService.DataAccess;
using PharmacyService.DataAccess.Repositories;
using PharmacyService.Services.Implementations;
using Serilog;
using System;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Configure services
ConfigureServices(builder.Services, builder.Configuration);

var app = builder.Build();

ConfigureMiddleware(app);


// Seed roles and users
await SeedRolesAndUsersAsync(app);

app.Run();

// Configuration methods

void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    // Configure Serilog
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.ClearProviders();
        loggingBuilder.AddSerilog(dispose: true);
    });

    // Add DbContext
    services.AddDbContext<AppDbContext>(opt =>
        opt.UseNpgsql(configuration.GetConnectionString("MyPostgresConnection"),
        options => options.MigrationsAssembly("PharmacyService.DataAccess")));

    // Identity configuration
    services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Configure Identity options
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = false;

        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

    // Authentication
    services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
        options.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
        options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
    }).AddCookie();

    // Authorization
    services.AddAuthorization();

    // Add other services
    services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    services.AddScoped(typeof(IGenericService<,>), typeof(GenericService<,>));
    services.AddScoped<IAuthService, AuthService>();
    services.AddAutoMapper(typeof(Program));

    services.AddControllers();
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo
        {
            Title = "Pharmacy Service API",
            Version = "v1",
            Description = "Using this API, service provides the ability to work with several service!",
            Contact = new OpenApiContact
            {
                Name = "Farrukh Kholmatov",
                Email = "Farrukhjon.Kholmatov@hotmail.com",
            },
        });
    });
}

void ConfigureMiddleware(WebApplication app)
{
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PharmacyService V1");
            c.DisplayRequestDuration(); // Show request duration in Swagger UI
        });
    }

    app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();
}


async Task SeedRolesAndUsersAsync(WebApplication app)
{
    using (var scope = app.Services.CreateScope())
    {
        var serviceProvider = scope.ServiceProvider;
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var authService = serviceProvider.GetRequiredService<IAuthService>();

        await authService.InitializeRolesAsync();
        await authService.InitializeUsersAsync(userManager);
    }
}
