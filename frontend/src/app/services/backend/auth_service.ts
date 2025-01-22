import axios from "axios";
import { API_URL } from "./const_service";
import type { User } from "@/app/models/user";

// TODO: Implement the AuthService class
class AuthService {
    static login(email: string, password: string, rememberme: boolean): Promise<void> {
        return Promise.resolve();
    }

    static logout(): Promise<void> {
        return Promise.resolve();
    }

    static register(name: string, surname: string, email: string, phone: string, password: string, confirmPassword: string): Promise<void> {
        return Promise.resolve();
    }

    static refreshToken(): Promise<void> {
        return Promise.resolve();
    }

    static getCurrentUser(): Promise<User> {
        // Not implemented
        return Promise.resolve({ id: 0, name: "", surname: "", email: "", phone: "" });
    }
}