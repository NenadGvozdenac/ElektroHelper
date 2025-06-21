using MongoDB.Bson.Serialization.Attributes;
using payment_backend.src.Payment.BuildingBlocks.Core.Domain;

namespace payment_backend.src.Payment.Application.Models;

public class PaymentEntity(string userId, decimal amount, string currency, string paymentPurpose, string payee,
    string payeeAccountNumber, string referenceNumber, string paymentModel) : BaseEntity
{
    [BsonElement("userId")]
    public string UserId { get; private set; } = userId;

    [BsonElement("amount")]
    public decimal Amount { get; private set; } = amount;

    [BsonElement("currency")]
    public string Currency { get; private set; } = currency;

    [BsonElement("paymentPurpose")]
    public string PaymentPurpose { get; private set; } = paymentPurpose;

    [BsonElement("payee")]
    public string Payee { get; private set; } = payee;

    [BsonElement("payeeAccountNumber")]
    public string PayeeAccountNumber { get; private set; } = payeeAccountNumber;

    [BsonElement("referenceNumber")]
    public string ReferenceNumber { get; private set; } = referenceNumber;

    [BsonElement("paymentModel")]
    public string PaymentModel { get; private set; } = paymentModel;
}