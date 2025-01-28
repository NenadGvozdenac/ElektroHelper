import type { CreateLocation } from "./location";

export interface ElectricityMeter {
    ID: number;
    location_id: number;
    date_of_registration: string;
};

export interface CreateElectricityMeter {
    location_id: number;
}

export interface CreateLocationWithMeter extends CreateLocation {
    hasElectricityMeter: boolean;
}