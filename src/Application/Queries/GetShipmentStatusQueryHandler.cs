using Application.Persistence;
using Domain.Models;
using MediatR;

namespace Application.Queries;

public class GetShipmentStatusQueryHandler : IRequestHandler<GetShipmentStatusQuery, string>
{
    private readonly IRepository<Sack> _sackRepository;
    private readonly IRepository<Package> _packageRepository;

    public GetShipmentStatusQueryHandler(IRepository<Sack> sackRepository, IRepository<Package> packageRepository)
    {
        _sackRepository = sackRepository;
        _packageRepository = packageRepository;
    }


    public async Task<string> Handle(GetShipmentStatusQuery request, CancellationToken cancellationToken)
    {
        if (request.Barcode.StartsWith("S"))
        {
            var sack = await _sackRepository.SingleOrDefaultAsync(x => x.Barcode == request.Barcode);
            return sack != null ? sack.State.ToString() : "NOT FOUND";
        }
        else
        {
            var package = await _sackRepository.SingleOrDefaultAsync(x => x.Barcode == request.Barcode);
            return package != null ? package.State.ToString() : "NOT FOUND";
        }
    }
}
