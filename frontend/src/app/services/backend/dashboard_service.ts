import axios, { type AxiosInstance } from "axios";
import { API_URL } from "./const_service";
import type { CreateElectricityMeter, ElectricityMeter } from "@/app/models/electricity_meter";
import type { CreateElectricityReading, CreateElectricityReadingWithDate, ElectricityReading } from "@/app/models/electricity_reading";
import type { CreateLocation, Location } from "@/app/models/location";

function authenticatedRequest(jwt: string): AxiosInstance {
    return axios.create({
        headers: {
            Authorization: `Bearer ${jwt}`,
        },
    });
}

export class DashboardService {
    static async getLocationsForUser(jwt: string): Promise<Location[]> {
        try {
            const response = await authenticatedRequest(jwt).get(`${API_URL}/locations`);
            return response.data.data as Location[];
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getMetersForUser(jwt: string): Promise<ElectricityMeter[]> {
        try {
            const response = await authenticatedRequest(jwt).get(`${API_URL}/electricity_meters/user`);
            return response.data.data as ElectricityMeter[];
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getReadingsForUser(jwt: string): Promise<ElectricityReading[]> {
        try {
            const response = await authenticatedRequest(jwt).get(`${API_URL}/electricity_readings`);
            return response.data.data as ElectricityReading[];
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async addLocationForUser(jwt: string, location: CreateLocation): Promise<Location> {
        try {
            debugger
            const response = await authenticatedRequest(jwt).post(`${API_URL}/locations`, location);
            return response.data.data as Location;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async addMeterForUser(jwt: string, meter: CreateElectricityMeter): Promise<ElectricityMeter> {
        try {
            debugger
            const response = await authenticatedRequest(jwt).post(`${API_URL}/electricity_meters`, meter);
            return response.data.data as ElectricityMeter;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async addReadingForUser(jwt: string, reading: CreateElectricityReadingWithDate): Promise<ElectricityReading> {
        try {
            debugger
            const response = await authenticatedRequest(jwt).post(`${API_URL}/electricity_readings/date`, reading);
            return response.data.data as ElectricityReading;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

}