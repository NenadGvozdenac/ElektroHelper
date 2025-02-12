import type { CreatePost, Post } from "@/app/models/forum_backend/Post";
import type { Response } from "@/app/models/forum_backend/Response";
import { authenticatedRequest } from "../backend/auth_service";
import { FORUM_URL } from "../backend/const_service";

export class PostService {
    static async createPost(jwt: string, createPost: CreatePost): Promise<Post> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/posts`, createPost);
            return request.data as Post;
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

    static async getForumPostsPaged(jwt: string, forumId: string, page: number, pageSize: number): Promise<Post[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/posts/forum/${forumId}`,
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

    static async getPost(jwt: string, postId: string): Promise<Post> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/posts/${postId}`);
            const response = request.data as Response<Post>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}