export interface Post {
    id: string;
    title: string;
    content: string;
    createdAt: string;
    isDeleted: boolean;
    isLocked: boolean;
    numberOfUpvotes: number;
    numberOfDownvotes: number;
    isUpvoted: boolean;
    isDownvoted: boolean;
    numberOfComments: number;
    author: {
        id: string;
        username: string;
    }
}

export interface CreatePost {
    title: string;
    content: string;
    forumId: string;
}