using Domain.Exceptions;
using Application.Persistence;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using WebApi.Models;
using ConsoleTables;

namespace Application.Commands;

public class DeliverShipmentsCommandHandler : IRequestHandler<DeliverShipmentsCommand>
{
    private readonly IRepository<Sack> _sackRepository;

    private readonly IRepository<Package> _packageRepository;

    private readonly ILogger<DeliverShipmentsCommandHandler> _logger;

    public DeliverShipmentsCommandHandler(IRepository<Sack> sackRepository, IRepository<Package> packageRepository, ILogger<DeliverShipmentsCommandHandler> logger)
    {
        _sackRepository = sackRepository;
        _packageRepository = packageRepository;
        _logger = logger;
    }

    public async Task<Unit> Handle(DeliverShipmentsCommand request, CancellationToken cancellationToken)
    {
        var domainMessages = new List<(string Barcode, string Message)>();

        foreach (var route in request.Delivery.Routes)
        {
            var deliveryPoint = (DeliveryPointType)route.DeliveryPoint;

            foreach (var shipment in route.Deliveries)
            {
                var isItPackage = shipment.Barcode.StartsWith("P");

                try
                {
                    if (isItPackage)
                    {
                        await HandleShipment(shipment, deliveryPoint);
                    }
                    else
                    {
                        await HandleSack(shipment, deliveryPoint);
                    }
                }
                catch (DomainException ex)
                {
                    domainMessages.Add((shipment.Barcode, ex.Message));
                }
            }
        }

        await _packageRepository.SaveChangesAsync();
        await _sackRepository.SaveChangesAsync();

        LogInvalidOperations(domainMessages);

        return Unit.Value;
    }

    private async Task HandleShipment(Shipment shipment, DeliveryPointType deliveryPoint)
    {
        var package = await _packageRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode, x => x.Sack!);

        if (package == null)
            return;

        package.Load();

        package.Unload(deliveryPoint);
    }

    private async Task HandleSack(Shipment shipment, DeliveryPointType deliveryPoint)
    {
        var sack = await _sackRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode, x => x.Packages);
        if (sack == null)
            return;

        sack.Load();

        sack.Unload(deliveryPoint);
    }

    private void LogInvalidOperations(IEnumerable<(string Barcode, string Message)> domainMessages)
    {
        ConsoleTable
            .From(domainMessages.Select(x => new { x.Barcode, x.Message }))
                .Write(Format.Alternative);
    }
}