export interface Profile {
    id: string;
    username: string;
    role: string;
    email: string;
    isBanned: boolean;
    reasonForBan: string;
    isFollowed: boolean;
    isDeleted: boolean;
    numberOfPosts: number;
    numberOfComments: number;
    numberOfFollowers: number;
    numberOfFollowing: number;
}