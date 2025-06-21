import type { MakePayment, Payment, Payments } from "@/app/models/payments/Payment";
import { authenticatedRequest } from "../backend/auth_service";
import type { Response } from "@/app/models/forum_backend/Response";
import { PAYMENT_URL } from "../backend/const_service";

export class PaymentService {
    static async getPayments(jwt: string): Promise<Payments> {
        try {
            const request = await authenticatedRequest(jwt).get(`${PAYMENT_URL}/payments`);

            const response = request.data as Response<Payments>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async makePayment(jwt: string, makePayment: MakePayment): Promise<Payment> {
        try {
            const request = await authenticatedRequest(jwt).post(`${PAYMENT_URL}/payments`, makePayment);

            const response = request.data as Response<Payment>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}