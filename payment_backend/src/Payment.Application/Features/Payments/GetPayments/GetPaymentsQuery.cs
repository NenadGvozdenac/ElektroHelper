using MediatR;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure;

namespace payment_backend.src.Payment.Application.Features.Payments.GetPayments;

public record GetPaymentsQuery(UserDTO UserDTO, int PageNumber, int PageSize) : IRequest<Result<GetPaymentsDTO>>;