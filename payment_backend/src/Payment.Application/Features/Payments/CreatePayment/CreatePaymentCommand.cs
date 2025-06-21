using MediatR;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure;

namespace payment_backend.src.Payment.Application.Features.Payments.CreatePayment;

public record CreatePaymentCommand(UserDTO UserDTO, decimal Amount, string Currency,
    string PaymentPurpose, string Payee, string PayeeAccountNumber,
    string ReferenceNumber, string PaymentModel) : IRequest<Result<CreatedPaymentDTO>>;