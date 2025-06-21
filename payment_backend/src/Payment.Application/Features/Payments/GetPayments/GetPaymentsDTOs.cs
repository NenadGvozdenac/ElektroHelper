namespace payment_backend.src.Payment.Application.Features.Payments.GetPayments;

public class GetPaymentDTO(
    string id,
    decimal amount,
    string currency,
    string paymentPurpose,
    string payee,
    string payeeAccountNumber,
    string referenceNumber,
    string paymentModel,
    DateTime createdAt)
{
    public string Id { get; set; } = id;
    public decimal Amount { get; set; } = amount;
    public string Currency { get; set; } = currency;
    public string PaymentPurpose { get; set; } = paymentPurpose;
    public string Payee { get; set; } = payee;
    public string PayeeAccountNumber { get; set; } = payeeAccountNumber;
    public string ReferenceNumber { get; set; } = referenceNumber;
    public string PaymentModel { get; set; } = paymentModel;
    public DateTime CreatedAt { get; set; } = createdAt;
}

public class GetPaymentsDTO(
    List<GetPaymentDTO> payments)
{
    public List<GetPaymentDTO> Payments { get; init; } = payments;
}