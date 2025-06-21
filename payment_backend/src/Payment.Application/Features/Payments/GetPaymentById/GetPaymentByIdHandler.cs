using MediatR;
using payment_backend.src.Payment.Application.Models;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

namespace payment_backend.src.Payment.Application.Features.Payments.GetPaymentById;

public class GetPaymentByIdHandler(IDocumentDatabaseContext documentDatabaseContext) : IRequestHandler<GetPaymentByIdQuery, Result<GetPaymentDTO>>
{
    public async Task<Result<GetPaymentDTO>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken)
    {
        var payment = await documentDatabaseContext.GetDocumentById<PaymentEntity>("payments", request.Id);
        
        if (payment == null)
        {
            return Result<GetPaymentDTO>.Failure("Payment not found.").WithCode(404);
        }

        var paymentDto = new GetPaymentDTO(
            payment.Id,
            payment.Amount,
            payment.Currency,
            payment.PaymentPurpose,
            payment.Payee,
            payment.PayeeAccountNumber,
            payment.ReferenceNumber,
            payment.PaymentModel.ToString(),
            payment.CreatedAt
        );

        return Result<GetPaymentDTO>.Success(paymentDto);
    }
}