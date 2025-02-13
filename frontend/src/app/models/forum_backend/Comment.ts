export interface Comment {
    id: string;
    content: string;
    isUpvoted: boolean;
    isDownvoted: boolean;
    isDeleted: string;
    author: {
        id: string;
        username: string;
        email: string;
        role: string;
    }
    createdAt: string;
    upvotes: number;
    downvotes: number;
}

export interface CreateComment {
    content: string;
    postId: string;
}