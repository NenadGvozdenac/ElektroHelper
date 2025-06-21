using MediatR;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

namespace payment_backend.src.Payment.Application.Features.Payments.CreatePayment;

public class CreatePaymentHandler(IDocumentDatabaseContext context) : IRequestHandler<CreatePaymentCommand, Result<CreatedPaymentDTO>>
{
    public Task<Result<CreatedPaymentDTO>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}