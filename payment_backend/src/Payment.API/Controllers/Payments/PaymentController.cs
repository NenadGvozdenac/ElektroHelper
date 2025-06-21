using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payment_backend.src.Payment.API.DTOs;
using payment_backend.src.Payment.Application.Features.Payments.CreatePayment;
using payment_backend.src.Payment.Application.Features.Payments.GetPaymentById;
using payment_backend.src.Payment.Application.Features.Payments.GetPayments;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure;

namespace payment_backend.src.Payment.API.Controllers.Invoices;

[ApiController]
[Route("api/payments")]
[Authorize]
public class PaymentController(IMediator mediator) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> CreatePayment([FromBody] CreatePaymentDTO createInvoiceDto)
    {
        var result = await mediator.Send(new CreatePaymentCommand(this.GetUser(),
            createInvoiceDto.Amount,
            createInvoiceDto.PaymentPurpose,
            createInvoiceDto.Payee,
            createInvoiceDto.PayeeAccountNumber,
            createInvoiceDto.ReferenceNumber,
            createInvoiceDto.PaymentModel));

        return CreateResponse(result);
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetMyPayments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetPaymentsQuery(this.GetUser(), pageNumber, pageSize));
        return CreateResponse(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetPaymentById(string id)
    {
        var result = await mediator.Send(new GetPaymentByIdQuery(id));
        return CreateResponse(result);
    }
}