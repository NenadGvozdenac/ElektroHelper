using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using payment_backend.src.Payment.API.Controllers.Payments;
using payment_backend.src.Payment.API.DTOs;
using payment_backend.src.Payment.Application.Features.Payments.CreatePayment;
using payment_backend.src.Payment.Application.Features.Payments.GetPayments;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;

namespace payment_tests.src.Features.Payments;

public class CommentsControllerTests
{
    private readonly IMediator _mediator;
    private readonly PaymentController _controller;

    public CommentsControllerTests()
    {
        _mediator = Substitute.For<IMediator>();
        _controller = new PaymentController(_mediator);

        // Set up mock user identity
        var context = new DefaultHttpContext();
        context.User = TestClaimsPrincipalFactory.Create("test-user-id");
        _controller.ControllerContext = new ControllerContext { HttpContext = context };
    }

    [Fact]
    public async Task CreatePaymentAsync_ReturnsSuccessResult()
    {
        // Arrange
        var createPaymentDto = new CreatePaymentDTO(100.0m, "USD", "Invoice Payment", "John Doe", "1234567890", "INV-001", "97");

        var expectedCreatedPayment = new CreatedPaymentDTO(
            id: "payment-id-123",
            amount: createPaymentDto.Amount,
            currency: createPaymentDto.Currency,
            paymentPurpose: createPaymentDto.PaymentPurpose,
            payee: createPaymentDto.Payee,
            payeeAccountNumber: createPaymentDto.PayeeAccountNumber,
            referenceNumber: createPaymentDto.ReferenceNumber,
            paymentModel: createPaymentDto.PaymentModel
        );

        var expectedResult = Result<CreatedPaymentDTO>.Success(expectedCreatedPayment);

        _mediator.Send(Arg.Any<CreatePaymentCommand>())
                 .Returns(expectedResult);

        // Act
        var result = await _controller.CreatePayment(createPaymentDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var returnedValue = Assert.IsType<Result<CreatedPaymentDTO>>(okResult.Value);
        Assert.True(returnedValue.IsSuccess);
        Assert.Equal(expectedCreatedPayment.Id, returnedValue.Value.Id);
    }

    [Fact]
    public async Task GetPaymentsAsync_ReturnsSuccessResult()
    {
        // Arrange
        var pageNumber = 1;
        var pageSize = 10;

        var payments = new List<GetPaymentDTO>
        {
            new(
                id: "payment-id-1",
                amount: 100.0m,
                currency: "USD",
                paymentPurpose: "Test 1",
                payee: "John",
                payeeAccountNumber: "1111",
                referenceNumber: "REF1",
                paymentModel: "97",
                createdAt: DateTime.UtcNow
            ),
            new(
                id: "payment-id-2",
                amount: 200.0m,
                currency: "USD",
                paymentPurpose: "Test 2",
                payee: "Jane",
                payeeAccountNumber: "2222",
                referenceNumber: "REF2",
                paymentModel: "97",
                createdAt: DateTime.UtcNow
            )
        };

        var expectedDto = new GetPaymentsDTO(payments);
        var expectedResult = Result<GetPaymentsDTO>.Success(expectedDto);

        _mediator.Send(Arg.Any<GetPaymentsQuery>())
                 .Returns(expectedResult);

        // Act
        var result = await _controller.GetMyPayments(pageNumber, pageSize);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);

        var value = Assert.IsType<Result<GetPaymentsDTO>>(okResult.Value);
        Assert.True(value.IsSuccess);
        Assert.Equal(2, value.Value.Payments.Count);
        Assert.Equal("payment-id-1", value.Value.Payments[0].Id);
    }

    [Fact]
    public async Task GetPaymentsAsync_ReturnsEmptyList_WhenNoPaymentsExist()
    {
        // Arrange
        var expectedResult = Result<GetPaymentsDTO>.Success(new GetPaymentsDTO(new List<GetPaymentDTO>()));

        _mediator.Send(Arg.Any<GetPaymentsQuery>())
                 .Returns(expectedResult);

        // Act
        var result = await _controller.GetMyPayments();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var value = Assert.IsType<Result<GetPaymentsDTO>>(okResult.Value);
        Assert.True(value.IsSuccess);
        Assert.Empty(value.Value.Payments);
    }
}