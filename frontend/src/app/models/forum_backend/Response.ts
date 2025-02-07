export interface Response<T> {
    isSuccess: boolean;
    error: string;
    code: number;
    value: T;
}