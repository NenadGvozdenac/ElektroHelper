import axios from "axios";
import { FORUM_URL } from "../backend/const_service";
import type { Response } from "@/app/models/forum_backend/Response";

export class RssService {
    static async getRssFeed(): Promise<string> {
        try {
            const request = await axios.get(`${FORUM_URL}/rss`);
            const response = request.data as Response<string>;
            return response.value;
        } catch (error) {
            console.error(error);
            throw error;
        }
    }
}