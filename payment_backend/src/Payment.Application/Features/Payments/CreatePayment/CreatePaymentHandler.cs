using MediatR;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

using payment_backend.src.Payment.Application.Models;

namespace payment_backend.src.Payment.Application.Features.Payments.CreatePayment;

public class CreatePaymentHandler(IDocumentDatabaseContext context) : IRequestHandler<CreatePaymentCommand, Result<CreatedPaymentDTO>>
{
    public async Task<Result<CreatedPaymentDTO>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        var payment = new PaymentEntity(
            request.UserDTO.Id,
            request.Amount,
            request.Currency,
            request.PaymentPurpose,
            request.Payee,
            request.PayeeAccountNumber,
            request.ReferenceNumber,
            request.PaymentModel
        );

        await context.AddDocument("payments", payment);

        var createdPaymentDto = new CreatedPaymentDTO(
            payment.Id,
            payment.Amount,
            payment.Currency,
            payment.PaymentPurpose,
            payment.Payee,
            payment.PayeeAccountNumber,
            payment.ReferenceNumber,
            payment.PaymentModel
        );

        return Result<CreatedPaymentDTO>.Success(createdPaymentDto);
    }
}