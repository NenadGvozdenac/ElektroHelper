import type { CreateForum, Forum } from "@/app/models/forum_backend/Forum";
import type { Response } from "@/app/models/forum_backend/Response";
import { FORUM_URL } from "../backend/const_service";
import { authenticatedRequest } from "../backend/auth_service";

export class ForumService {
    static async getForums(jwt: string): Promise<Forum[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/forums`);
            const response = request.data as Response<Forum[]>;
            return response.value;
        } catch(error) {
            console.error(error);
            throw error;
        }
    }

    static async createForum(jwt: string, createForum: CreateForum): Promise<Forum> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/forums`, createForum);
            const response = request.data as Response<Forum>;
            return response.value;
        } catch(error) {
            console.error(error);
            throw error;
        }
    }
}