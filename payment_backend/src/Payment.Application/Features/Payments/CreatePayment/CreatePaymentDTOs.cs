namespace payment_backend.src.Payment.Application.Features.Payments.CreatePayment;

public record CreatedPaymentDTO(
    Guid Id,                     // ID placanja
    decimal Amount,             // Kolicina
    string PaymentPurpose,      // Svrha placanja
    string Payee,               // Primalac
    string PayeeAccountNumber,  // Racun primaoca
    string ReferenceNumber,     // Poziv na broj
    string PaymentModel         // Model placanja
);