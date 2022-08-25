using Application.Commands;
using Application.Persistence;
using Application.Queries;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Models;

namespace Application;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(this IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddTransient<IRequestHandler<DeliverShipmentsCommand, Unit>, DeliverShipmentsCommandHandler>();
        services.AddTransient<IRequestHandler<GetDeliveryStatusQuery, Delivery>, GetDeliveryStatusHandler>();

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("AppDbContext")
                .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        });

        // Create a new scope to retrieve scoped services
        using (var scope = services.BuildServiceProvider().CreateScope())
        {
            // Run the database migration automatically for local environments
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            dbContext.Database.EnsureCreated();

            // Seed the database with some test data
            Seed(dbContext);
        }
        return services;
    }

    static void Seed(AppDbContext dbContext)
    {

    }
}