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

    public Task<Unit> Handle(DeliverShipmentsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}