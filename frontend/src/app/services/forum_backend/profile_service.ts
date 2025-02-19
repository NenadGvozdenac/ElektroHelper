import type { Response } from "@/app/models/forum_backend/Response";
import { authenticatedRequest, getAccessToken } from "../backend/auth_service";
import { FORUM_URL } from "../backend/const_service";
import type { Profile, SmallProfile } from "@/app/models/forum_backend/Profile";
import type { Following } from "@/app/models/forum_backend/Following";
import type { Post } from "@/app/models/forum_backend/Post";

export class ProfileService {
    static async getProfile(jwt: string, userId: string): Promise<Profile> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/users/${userId}`);

            const response = request.data as Response<Profile>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async followProfile(jwt: string, userId: string): Promise<Following> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/followers/follow/${userId}`);
            const response = request.data as Response<Following>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async unfollowProfile(jwt: string, userId: string): Promise<Following> {
        try {
            const request = await authenticatedRequest(jwt).post(`${FORUM_URL}/followers/unfollow/${userId}`);
            const response = request.data as Response<Following>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getProfilePostsPaged(jwt: string, userId: string, value1: number, value2: number): Promise<Post[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/posts/user/${userId}`,{
                params: {
                    page: value1,
                    pageSize: value2
                }
            });
            const response = request.data as Response<Post[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getCurrentProfile(jwt: string) {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/users/my`);
            const response = request.data as Response<Profile>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async getMyProfileFollowers(jwt: string): Promise<SmallProfile[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/followers/my`);
            const response = request.data as Response<SmallProfile[]>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }

    static async deletePost(jwt: string, postId: string): Promise<{ postId: string, isDeleted: boolean }> {
        try {
            const request = await authenticatedRequest(jwt).delete(`${FORUM_URL}/posts/${postId}`);
            const response = request.data as Response<{ postId: string, isDeleted: boolean }>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}