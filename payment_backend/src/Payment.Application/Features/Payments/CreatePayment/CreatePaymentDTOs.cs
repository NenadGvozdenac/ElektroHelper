namespace payment_backend.src.Payment.Application.Features.Payments.CreatePayment;

public class CreatedPaymentDTO
{
    public string Id { get; init; }                  // ID placanja
    public decimal Amount { get; init; }             // Kolicina
    public string Currency { get; init; }            // Valuta
    public string PaymentPurpose { get; init; }      // Svrha placanja
    public string Payee { get; init; }               // Primalac
    public string PayeeAccountNumber { get; init; }  // Racun primaoca
    public string ReferenceNumber { get; init; }     // Poziv na broj
    public string PaymentModel { get; init; }        // Model placanja

    public CreatedPaymentDTO(
        string id,
        decimal amount,
        string currency,
        string paymentPurpose,
        string payee,
        string payeeAccountNumber,
        string referenceNumber,
        string paymentModel)
    {
        Id = id;
        Amount = amount;
        Currency = currency;
        PaymentPurpose = paymentPurpose;
        Payee = payee;
        PayeeAccountNumber = payeeAccountNumber;
        ReferenceNumber = referenceNumber;
        PaymentModel = paymentModel;
    }
}