import axios from "axios";
import { API_URL } from "./const_service";
import type { User, UserData, UserLogin, UserRegister } from "@/app/models/user";
import type { Tokens } from "@/app/models/token";

const setTokens = (tokens: Tokens) => {
    debugger
    localStorage.setItem("accessToken", tokens.token);
    localStorage.setItem("refreshToken", tokens.refresh_token);
}

export const getAccessToken = (): string | null => {
    return localStorage.getItem("accessToken");
}

export const getRefreshToken = (): string | null => {
    return localStorage.getItem("refreshToken");
}

export const removeTokens = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
}

export const isUserLoggedIn = (): boolean => {
    return !!getAccessToken();
}

export const getUserData = (): UserData | null => {
    const token = getAccessToken();
    if (token) {
        const payload = token.split(".")[1];
        const decoded = atob(payload);
        return JSON.parse(decoded);
    }
    return null;
}

// TODO: Implement the AuthService class
export class AuthService {
    static async login(loginData: UserLogin): Promise<Tokens> {
        try {
            const response = await axios.post(`${API_URL}/login`, loginData);
            setTokens(response.data.data as Tokens);
            return response.data as Tokens;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async register(registerData: UserRegister): Promise<Tokens> {
        try {
            const response = await axios.post(`${API_URL}/register`, registerData);
            setTokens(response.data.data as Tokens);
            return response.data as Tokens;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static logout(): Promise<void> {
        return Promise.resolve();
    }

    static refreshTokens(): Promise<void> {
        return Promise.resolve();
    }

    static getCurrentUser(): Promise<User> {
        // Not implemented
        return Promise.resolve({ id: 0, name: "", surname: "", email: "", phone: "" });
    }
}