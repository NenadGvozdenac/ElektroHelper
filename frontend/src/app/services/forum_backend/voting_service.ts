import type { Downvote, DownvoteComment, Upvote, UpvoteComment } from "@/app/models/forum_backend/Voting";
import type { Response } from "@/app/models/forum_backend/Response";
import { authenticatedRequest } from "../backend/auth_service";
import { FORUM_URL } from "../backend/const_service";

export class VotingService {
    static async upvotePost(jwt: string, postId: string) {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/rating/upvote/post?postId=${postId}`);

            const response = request.data as Response<Upvote>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async downvotePost(jwt: string, postId: string) {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/rating/downvote/post?postId=${postId}`);
            const response = request.data as Response<Downvote>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async deleteDownvotePost(jwt: string, postId: string) {
        try {
            const request = await authenticatedRequest(jwt).delete(`${FORUM_URL}/rating/downvote/post?postId=${postId}`);
            const response = request.data as Response<Downvote>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
    
    static async deleteUpvotePost(jwt: string, postId: string) {
        try {
            const request = await authenticatedRequest(jwt).delete(`${FORUM_URL}/rating/upvote/post?postId=${postId}`);
            const response = request.data as Response<Upvote>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async deleteDownvoteComment(jwt: string, commentId: string): Promise<DownvoteComment> {
        try {
            const request = await authenticatedRequest(jwt).delete(`${FORUM_URL}/rating/downvote/comment?commentId=${commentId}`);
            const response = request.data as Response<DownvoteComment>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
    static async deleteUpvoteComment(jwt: string, commentId: string): Promise<UpvoteComment> {
        try {
            const request = await authenticatedRequest(jwt).delete(`${FORUM_URL}/rating/upvote/comment?commentId=${commentId}`);
            const response = request.data as Response<UpvoteComment>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
    static async downvoteComment(jwt: string, commentId: string): Promise<DownvoteComment> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/rating/downvote/comment?commentId=${commentId}`);
            const response = request.data as Response<DownvoteComment>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
    static async upvoteComment(jwt: string, commentId: string): Promise<UpvoteComment> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/rating/upvote/comment?commentId=${commentId}`);
            const response = request.data as Response<UpvoteComment>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}