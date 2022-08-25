using Application.Persistence;
using Domain.Models;
using MediatR;
using WebApi.Models;

namespace Application.Queries;

public class GetDeliveryStatusHandler : IRequestHandler<GetDeliveryStatusQuery, Delivery>
{
    private readonly IRepository<Package> _packageRepository;
    private readonly IRepository<Sack> _sackRepository;

    public GetDeliveryStatusHandler(IRepository<Sack> sackRepository, IRepository<Package> packageRepository)
    {
        _packageRepository = packageRepository;
        _sackRepository = sackRepository;
    }

    public async Task<Delivery> Handle(GetDeliveryStatusQuery query, CancellationToken cancellationToken)
    {
        var delivery = query.Delivery;

        foreach (var route in delivery.Routes)
        {
            var deliveryPoint = (DeliveryPointType)route.DeliveryPoint;

            foreach (var shipment in route.Deliveries)
            {
                var isItPackage = shipment.Barcode.StartsWith("P");

                if (isItPackage)
                {
                    var package = await _packageRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode, x => x.Sack!);
                    shipment.State = (int?)package?.State!;
                }
                else
                {
                    var sack = await _sackRepository.SingleOrDefaultAsync(x => x.Barcode == shipment.Barcode);
                    shipment.State = (int?)sack?.State;
                }
            }
        }

        return delivery;
    }
}
