namespace payment_backend.src.Payment.API.DTOs;

public record CreatePaymentDTO(
    decimal Amount,             // Kolicina
    string Currency,            // Valuta
    string PaymentPurpose,      // Svrha placanja
    string Payee,               // Primalac
    string PayeeAccountNumber,  // Racun primaoca
    string ReferenceNumber,     // Poziv na broj
    string PaymentModel         // Model placanja
);