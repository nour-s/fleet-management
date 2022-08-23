using MediatR;
using WebApi.Models;

namespace Application.Commands;

public record DeliverShipmentsCommand(Delivery Delivery) : IRequest<Unit>;