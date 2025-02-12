import { authenticatedRequest } from "../backend/auth_service";
import type { Comment, CreateComment } from "@/app/models/forum_backend/Comment";
import type { Response } from "@/app/models/forum_backend/Response";
import { FORUM_URL } from "../backend/const_service";

export class CommentService {
    static async getPostCommentsPaged(jwt: string, postId: string, page: number, pageSize: number): Promise<Comment[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/comments/${postId}`,
                {
                    params: {
                        page: page,
                        pageSize: pageSize
                    }
                });
            const response = request.data as Response<Comment[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
    
    static async getPostComments(jwt: string, postId: string): Promise<Comment[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/comments/${postId}`);
            const response = request.data as Response<Comment[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async createComment(jwt: string, comment: CreateComment): Promise<Comment> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/comments`, comment);
            const response = request.data as Response<Comment>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}