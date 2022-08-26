using MediatR;
using WebApi.Models;

namespace Application.Queries;

public record GetDeliveryStatusQuery(Delivery Delivery) : IRequest<Delivery>;
