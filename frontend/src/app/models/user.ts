export interface User {
    id: number;
    name: string;
    surname: string;
    email: string;
    phone: string;
}

export interface UserLogin {
    email: string;
    password: string;
    rememberme: boolean;
}

export interface UserRegister {
    name: string;
    surname: string;
    email: string;
    phone: string;
    password: string;
    confirmPassword: string;
}