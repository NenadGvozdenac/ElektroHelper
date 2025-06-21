using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;
using payment_backend.src.Payment.BuildingBlocks.Infrastructure.Database;

namespace payment_backend.src.Payment.API.Controllers.Invoices;

[ApiController]
[Route("api/invoices")]
[Authorize]
public class InvoiceController : BaseController
{
    private readonly IDocumentDatabaseContext _databaseContext;

    public InvoiceController(IDocumentDatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }
}