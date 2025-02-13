export interface Post {
    id: string;
    title: string;
    content: string;
    createdAt: string;
    isDeleted: boolean;
    isLocked: boolean;
    upvotes: number;
    downvotes: number;
    isUpvoted: boolean;
    isDownvoted: boolean;
    comments: number;
    author: {
        id: string;
        username: string;
    }
    forum: {
        id: string;
        name: string;
    }
}

export interface CreatePost {
    title: string;
    content: string;
    forumId: string;
}