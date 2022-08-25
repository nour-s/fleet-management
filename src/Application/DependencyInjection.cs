using System.Reflection;
using Application.Commands;
using Application.Persistence;
using Application.Queries;
using Domain.Models;
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
        services.AddMediatR(Assembly.GetExecutingAssembly());
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
        var sacks = dbContext.Sacks;
        var packages = dbContext.Packages;

        var sack = new Sack("C725799", DeliveryPointType.DistributionCentre);
        sack.AddPackage(new Package("P8988000122", DeliveryPointType.DistributionCentre, 26));
        sack.AddPackage(new Package("P8988000126", DeliveryPointType.DistributionCentre, 50));

        sacks.Add(sack);

        var sack2 = new Sack("C725800", DeliveryPointType.TransferCentre);
        sack2.AddPackage(new Package("P9988000128", DeliveryPointType.TransferCentre, 55));
        sack2.AddPackage(new Package("P9988000129", DeliveryPointType.TransferCentre, 28));

        sacks.Add(sack2);


        packages.Add(new Package("P7988000121", (DeliveryPointType)1, 5));
        packages.Add(new Package("P7988000122", (DeliveryPointType)1, 5));
        packages.Add(new Package("P7988000123", (DeliveryPointType)1, 9));
        packages.Add(new Package("P8988000120", (DeliveryPointType)2, 33));
        packages.Add(new Package("P8988000121", (DeliveryPointType)2, 17));
        packages.Add(new Package("P8988000123", (DeliveryPointType)2, 35));
        packages.Add(new Package("P8988000124", (DeliveryPointType)2, 1));
        packages.Add(new Package("P8988000125", (DeliveryPointType)2, 200));
        packages.Add(new Package("P9988000126", (DeliveryPointType)3, 15));
        packages.Add(new Package("P9988000127", (DeliveryPointType)3, 16));
        packages.Add(new Package(" P9988000130", (DeliveryPointType)3, 17));

        dbContext.SaveChanges();
    }
}