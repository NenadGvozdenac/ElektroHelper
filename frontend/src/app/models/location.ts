export interface Location {
    ID: number;
    street: string;
    number: string;
    city: string;
    country: string;
    postal_code: string;
    user_id: number;
};

export interface CreateLocation {
    street: string;
    number: string;
    city: string;
    country: string;
    postal_code: string;
};