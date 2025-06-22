using MediatR;
using payment_backend.src.Payment.Application.Models;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

namespace payment_backend.src.Payment.Application.Features.Payments.GetPayments;

public class GetPaymentsHandler(IDocumentDatabaseContext documentDatabaseContext) : IRequestHandler<GetPaymentsQuery, Result<GetPaymentsDTO>>
{
    public async Task<Result<GetPaymentsDTO>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserDTO.Id;

        var payments = (await documentDatabaseContext.GetCollection<PaymentEntity>("payments", request.PageNumber, request.PageSize))
            .Where(payment => payment.UserId == userId)
            .OrderByDescending(payment => payment.CreatedAt)
            .ToList();

        var paymentDtos = payments.Select(payment => new GetPaymentDTO(
            payment.Id,
            payment.Amount,
            payment.Currency,
            payment.PaymentPurpose,
            payment.Payee,
            payment.PayeeAccountNumber,
            payment.ReferenceNumber,
            payment.PaymentModel.ToString(),
            payment.CreatedAt
        )).ToList();

        return Result<GetPaymentsDTO>.Success(new GetPaymentsDTO(paymentDtos));
    }
}