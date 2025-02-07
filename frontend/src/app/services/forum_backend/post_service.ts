import type { Post } from "@/app/models/forum_backend/Post";
import type { Response } from "@/app/models/forum_backend/Response";
import { authenticatedRequest } from "../backend/auth_service";
import { FORUM_URL } from "../backend/const_service";

export class PostService {
    static async getPosts(jwt: string, forumId: number): Promise<Post[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/posts/${forumId}`);
            const response = request.data as Response<Post[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getAllPosts(jwt: string): Promise<Post[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/posts`);
            const response = request.data as Response<Post[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getAllPostsPaged(jwt: string, page: number, pageSize: number): Promise<Post[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/posts`,
                {
                    params: {
                        page: page,
                        pageSize: pageSize
                    }
                });
            const response = request.data as Response<Post[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}