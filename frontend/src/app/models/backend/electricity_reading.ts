export interface ElectricityReading {
    id: number;
    electricity_meter_id: number;
    lower_reading: string;
    upper_reading: string;
    reading_date: string;
};  

export interface CreateElectricityReading {
    electricity_meter_id: number;
    lower_reading: string;
    upper_reading: string;
}

export interface CreateElectricityReadingWithDate extends CreateElectricityReading {
    reading_date: string;
}