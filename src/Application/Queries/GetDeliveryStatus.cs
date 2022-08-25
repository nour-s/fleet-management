using MediatR;
using WebApi.Models;

namespace Application.Queries;

public record GetDeliveryStatus(Delivery Delivery) : IRequest<Delivery>;
