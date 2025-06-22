import type { User } from "@/app/models/backend/user";
import { API_URL } from "./const_service";
import { authenticatedRequest } from "./auth_service";

export class UserService {
    static async getActiveUser(jwt: string): Promise<User> {
        try {
            const response = await authenticatedRequest(jwt).get(`${API_URL}/user`);
            return response.data.data as User;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}