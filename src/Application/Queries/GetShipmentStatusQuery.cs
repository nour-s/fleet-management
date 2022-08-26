using MediatR;

namespace Application.Queries;

public record GetShipmentStatusQuery(string Barcode) : IRequest<string>;
