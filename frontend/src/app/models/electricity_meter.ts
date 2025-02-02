import type { CreateLocation } from "./location";

export interface ElectricityMeter {
    ID: number;
    location_id: number;
    meter_code: string;
    date_of_registration: string;
};

export interface CreateElectricityMeter {
    location_id: number;
    meter_code: string;
}

export interface CreateLocationWithMeter extends CreateLocation {
    hasElectricityMeter: boolean;
    meter_code?: string;
}