import React, { useState, useEffect } from 'react';
import AlbumComponent from '../Components/AlbumComponent';
import AuthorizeView from "../Components/AuthorizeView.tsx";

type Like = {
    id: number;
    actionType: string;
    userId: string;
    photoId: number;
};

type Photo = {
    id: number;
    data: string;
    contentType: string;
    likeCounter: number;
    dislikeCounter: number;
    albumId: number;
    likes: Like[];
};

type Album = {
    id: number;
    name: string;
    userId: string;
    photos: Photo[];
};

const AllAlbums: React.FC = () => {
    const [albums, setAlbums] = useState<Album[]>([]);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        fetchAlbums();
    }, []);

    async function fetchAlbums() {
        try {
            const response = await fetch('api/album');
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
            const data: Album[] = await response.json();
            setAlbums(data);
        } catch (error: any) {
            console.error('There was a problem with the fetch operation:', error);
            setError(error.message);
        }
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <AuthorizeView>
            <div className='centered-container'>
                <h1>All Albums</h1>
                <table>
                    <tbody>
                        <tr>
                            {albums.length > 0 ? (
                                albums.map(album => album.photos.length > 0 && (
                                    <AlbumComponent key={album.photos[0].id} photo={album.photos[0]} name={album.name} album={album} />
                                ))
                            ) : (
                                <tr><td>No albums found</td></tr>
                            )}
                        </tr>
                    </tbody>
                </table>
            </div>
        </AuthorizeView>
    );
};

export default AllAlbums;
