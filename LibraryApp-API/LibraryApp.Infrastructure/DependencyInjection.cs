using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using LibraryApp.Infrastructure.Data;


namespace LibraryApp.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var cs = configuration.GetConnectionString("Default");
        services.AddDbContext<LibraryDbContext>(opt =>
        opt.UseMySql(cs!, ServerVersion.AutoDetect(cs))
        );
        return services;
    }
}