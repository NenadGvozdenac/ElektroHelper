export interface Upvote {
    postId: string;
    isUpvoted: boolean;
}

export interface Downvote {
    postId: string;
    isDownvoted: boolean;
}

export interface UpvoteComment {
    commentId: string;
    isUpvoted: boolean;
}

export interface DownvoteComment {
    commentId: string;
    isDownvoted: boolean;
}