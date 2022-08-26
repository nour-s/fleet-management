using Domain.Exceptions;
using Application.Persistence;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using WebApi.Models;

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
        foreach (var route in request.Delivery.Routes)
        {
            var deliveryPoint = (DeliveryPointType)route.DeliveryPoint;

            foreach (var shipment in route.Deliveries)
            {
                var isItPackage = shipment.Barcode.StartsWith("P");

                if (isItPackage)
                {
                    await HandleShipment(shipment, deliveryPoint);
                }
                else
                {
                    await HandleSack(shipment, deliveryPoint);
                }
            }
        }

        await _packageRepository.SaveChangesAsync();
        await _sackRepository.SaveChangesAsync();
        return Unit.Value;
    }

    private async Task HandleShipment(Shipment shipment, DeliveryPointType deliveryPoint)
    {
        var package = await _packageRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode, x => x.Sack!);

        if (package == null)
            return;

        package.Load();

        try
        {
            package.Unload(deliveryPoint);
        }
        catch (DomainException ex)
        {
            // package can't be unloaded to this delivery point, we just skip.
            _logger.LogInformation(ex.Message);
        }
    }

    private async Task HandleSack(Shipment shipment, DeliveryPointType deliveryPoint)
    {
        var sack = await _sackRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode, x => x.Packages);
        if (sack == null)
            return;

        sack.Load();

        try
        {
            sack.Unload(deliveryPoint);
        }
        catch (DomainException ex)
        {
            // sack can't be unloaded to this delivery point, we just skip.
            _logger.LogInformation(ex.Message);
            return;
        }
    }
}