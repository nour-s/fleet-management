using Application.Persistence;
using Domain.Models;
using MediatR;

namespace Application.Commands;

public class DeliverShipmentsCommandHandler : IRequestHandler<DeliverShipmentsCommand>
{
    private readonly IRepository<Sack> _sackRepository;

    private readonly IRepository<Package> _packageRepository;

    public DeliverShipmentsCommandHandler(IRepository<Sack> sackRepository, IRepository<Package> packageRepository)
    {
        _sackRepository = sackRepository;
        _packageRepository = packageRepository;
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
                    var package = await _packageRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode, x => x.Sack!);

                    if (package == null)
                        continue;

                    package.Load();

                    try
                    {
                        package.Unload(deliveryPoint);
                    }
                    catch (ArgumentException)
                    {
                        // package can't be unloaded to this delivery point, we just skip.
                        continue;
                    }
                }
                else
                {
                    var sack = await _sackRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode, x => x.Packages);
                    if (sack == null)
                        continue;

                    sack.Load();

                    try
                    {
                        sack.Unload(deliveryPoint);
                    }
                    catch (ArgumentException)
                    {
                        // sack can't be unloaded to this delivery point, we just skip.
                        continue;
                    }
                }
            }
        }

        await _packageRepository.SaveChangesAsync();
        await _sackRepository.SaveChangesAsync();
        return Unit.Value;
    }
}