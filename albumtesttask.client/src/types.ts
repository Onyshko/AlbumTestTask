export interface Like {
    id: number;
    actionType: string;
    userId: string;
    photoId: number;
}

export interface Photo {
    id: number;
    data: string;
    contentType: string;
    likeCounter: number;
    dislikeCounter: number;
    albumId: number;
    likes: Like[];
}

export interface Album {
    id: number;
    name: string;
    userId: string;
    photos: Photo[];
}
