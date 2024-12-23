export type ExternalProviderType = {
    clientId?: string;
    clientSecret?: string;
    provider: string;
    displayName: string;
    enabled?: boolean;
}

export type LoginType = {
    username: string;
    password: string;
}

export type RegisterType = {
    username: string;
    email: string;
    password: string;
    confirmPassword: string;
}