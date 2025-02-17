import type { Post } from "@/app/models/forum_backend/Post";
import type { Response } from "@/app/models/forum_backend/Response";
import { authenticatedRequest } from "../backend/auth_service";
import { FORUM_URL } from "../backend/const_service";

export class SearchService {
    static async search(jwt: string, query: string): Promise<Post[]> {
        try {
            const request = await authenticatedRequest(jwt).get(`${FORUM_URL}/search/search`, {
                params: {
                    query: query,
                    page: 1,
                    pageSize: 5
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