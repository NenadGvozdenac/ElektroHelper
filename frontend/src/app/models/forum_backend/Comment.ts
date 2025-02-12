export interface Comment {
    comment: CommentData;
    isUpvoted: boolean;
    isDownvoted: boolean;
    author: {
        id: string;
        username: string;
    }
}

interface CommentData {
    id: string;
    content: string;
    createdAt: string;
    isDeleted: boolean;
    numberOfUpvotes: number;
    numberOfDownvotes: number;
}

export interface CreateComment {
    content: string;
    postId: string;
}