using MediatR;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;

namespace payment_backend.src.Payment.Application.Features.Payments.GetPaymentById;

public record GetPaymentByIdQuery(string Id) : IRequest<Result<GetPaymentDTO>>;