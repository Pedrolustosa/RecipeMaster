export interface LoginRequest {
    email: string;
    password: string;
}

export interface RegisterRequest {
    email: string;
    password: string;
}

export interface TokenResponse {
    token: string;
}

export interface User {
    id: string;
    email: string;
    userName: string;
    isActive: boolean;
}

export interface UpdateUserRequest {
    id: string;
    email: string;
    userName: string;
    isActive: boolean;
}