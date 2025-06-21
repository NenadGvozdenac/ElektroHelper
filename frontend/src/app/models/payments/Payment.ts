export interface Payment {
    id: string;
    amount: number;
    currency: string;
    paymentPurpose: string;
    payee: string;
    payeeAccountNumber: string;
    referenceNumber: string;
    paymentModel: string;
    createdAt: Date;
}

export interface Payments {
    payments: Payment[];
}

export interface MakePayment {
    amount: number;
    currency: string;
    paymentPurpose: string;
    payee: string;
    payeeAccountNumber: string;
    referenceNumber: string;
    paymentModel: string;
}