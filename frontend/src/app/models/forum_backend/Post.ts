export interface Post {
    id: string;
    title: string;
    content: string;
    createdAt: string;
    isDeleted: boolean;
    isLocked: boolean;
}

export interface CreatePost {
    title: string;
    content: string;
    forumId: string;
}