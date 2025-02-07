export interface Forum {
    id: string;
    name: string;
    description: string;
    createdAt: string;
    isDeleted: boolean;
    isQuarantined: boolean;
}

export interface CreateForum {
    name: string;
    description: string;
}