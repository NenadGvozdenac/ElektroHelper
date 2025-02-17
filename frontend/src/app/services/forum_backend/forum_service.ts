import type { CreateForum, Forum } from "@/app/models/forum_backend/Forum";
import type { Response } from "@/app/models/forum_backend/Response";
import { FORUM_URL } from "../backend/const_service";
import { authenticatedRequest } from "../backend/auth_service";
import type { UserData, UserRegister } from "@/app/models/backend/user";

export class ForumService {
    static async registerUser(jwt: string): Promise<UserData> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/users`);
            const response = request.data as Response<UserData>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getForums(jwt: string): Promise<Forum[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/forums`);
            const response = request.data as Response<Forum[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async createForum(jwt: string, createForum: CreateForum): Promise<Forum> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/forums`, createForum);
            const response = request.data as Response<Forum>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getForum(jwt: string, forumId: string): Promise<Forum> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/forums/${forumId}`);
            const response = request.data as Response<Forum>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async quarantineForum(jwt: string, forumId: string) {
        try {
            await authenticatedRequest(jwt).post(`${FORUM_URL}/forums/quarantine/${forumId}`);
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
    static async unquarantineForum(jwt: string, forumId: string) {
        try {
            await authenticatedRequest(jwt).post(`${FORUM_URL}/forums/unquarantine/${forumId}`);
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}