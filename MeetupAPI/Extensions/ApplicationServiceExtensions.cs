using MeetupAPI.Data;
using MeetupAPI.Helpers;
using Microsoft.EntityFrameworkCore;

namespace MeetupAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });
        services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
        return services;
    }
}