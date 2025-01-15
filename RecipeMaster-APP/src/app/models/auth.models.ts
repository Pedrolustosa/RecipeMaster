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
    email: string;
    userName: string;
}
