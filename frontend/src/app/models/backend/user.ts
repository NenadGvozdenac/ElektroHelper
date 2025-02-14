export interface User {
    id: number;
    name: string;
    surname: string;
    email: string;
    phone: string;
}

export interface UserData {
    userID: number;
    userName: string;
    userEmail: string;
    userRole: string;
}

export interface UserLogin {
    email: string;
    password: string;
}

export interface UserRegister {
    name: string;
    surname: string;
    email: string;
    phone: string;
    password: string;
    confirm_password: string;
}