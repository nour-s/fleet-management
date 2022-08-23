using MediatR;

namespace Application.Commands;

public class DeliverShipmentsCommandHandler : IRequestHandler<DeliverShipmentsCommand>
{
    public Task<Unit> Handle(DeliverShipmentsCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}