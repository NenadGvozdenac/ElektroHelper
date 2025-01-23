import axios from "axios";
import { API_URL } from "./const_service";
import type { User, UserData, UserLogin, UserRegister } from "@/app/models/user";
import type { Tokens } from "@/app/models/token";
import { goToLoginScreen } from "@/app/routes";

const setTokens = (tokens: Tokens) => {
    localStorage.setItem("accessToken", tokens.token);
    localStorage.setItem("refreshToken", tokens.refresh_token);
}

export const getRefreshToken = (): string | null => {
    return localStorage.getItem("refreshToken");
}

export const removeTokens = () => {
    localStorage.removeItem("accessToken");
    localStorage.removeItem("refreshToken");
}

export const isUserLoggedIn = async (): Promise<boolean> => {
    return await getAccessToken() !== null;
}

export const getUserData = async (): Promise<UserData | null> => {
    const token = await getAccessToken();
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
            return response.data.data as Tokens;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async logout(): Promise<void> {
        try {
            const response = await axios.post(`${API_URL}/logout`, {
                refresh_token: getRefreshToken()
            });
            removeTokens();
        } catch(error) {
            console.error(error);
            throw error;
        }
    }

    static async register(registerData: UserRegister): Promise<Tokens> {
        try {
            const response = await axios.post(`${API_URL}/register`, registerData);
            setTokens(response.data.data as Tokens);
            return response.data.data as Tokens;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getNewAccessToken(refresh_token: string): Promise<Tokens> {
        try {
            const response = await axios.post(`${API_URL}/refresh_token`, {
                refresh_token
            });
            setTokens(response.data.data as Tokens);
            return response.data.data as Tokens;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}

export const getAccessToken = async (): Promise<string | null> => {
    var jwt = localStorage.getItem("accessToken");

    if (!jwt) {
        return null;
    }

    const payload = jwt.split(".")[1];
    const decoded = atob(payload);
    const data = JSON.parse(decoded);

    // If token is not expired, return it
    if (data.exp >= Date.now() / 1000) {
        return jwt;
    }

    const refresh_token = getRefreshToken();

    removeTokens();

    if (!refresh_token) {
        goToLoginScreen();
        return null;
    }

    const newToken = await AuthService.getNewAccessToken(refresh_token);

    return newToken.token;
}