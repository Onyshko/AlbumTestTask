import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Album } from '../types.ts';
import PhotoComponent from '../Components/PhotoComponent';

interface PhotoUploadData {
    id: number;
    data: string;
    contentType: string;
    likeCounter: number;
    dislikeCounter: number;
    albumId: number;
    likes: Array<{ id: number; actionType: string; userId: string; photoId: number }>;
}

const AlbumDetails: React.FC = () => {
    const { albumId } = useParams<'albumId'>();
    const [album, setAlbum] = useState<Album | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [isUploading, setIsUploading] = useState(false);

    useEffect(() => {
        fetchAlbum();
    }, [albumId]);

    async function fetchAlbum() {
        if (albumId) {
            try{
                const response = await fetch(`api/album/${albumId}`);
                if (!response.ok) {
                    throw new Error('Network response was not ok.');
                }
                const data: Album = await response.json();
                setAlbum(data);
            } catch (error: any) {
                console.error('There was a problem with the fetch operation:', error);
                setError(error.message);
            }
        }
    };

    function handleUploadClick() {
        setIsUploading(true);
    }

    async function handleFileSelect(event: React.ChangeEvent<HTMLInputElement>) {
        const file = event.target.files?.[0];
        if (file) {
            const contentType = file.type;
            const base64 = await convertToBase64(file);

            sendPhotoToAPI({
                id: 0,
                data: base64,
                contentType: contentType,
                likeCounter: 0,
                dislikeCounter: 0,
                albumId: parseInt(albumId!, 10),
                likes: []
            });
        }
    }

    function convertToBase64(file: File): Promise<string> {
        return new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onloadend = () => {
                if (typeof reader.result === 'string') {
                    const base64String = reader.result
                        .replace(/^data:.+;base64,/, '');
                    resolve(base64String);
                } else {
                    reject(new Error('Expected reader.result to be a string'));
                }
            };
            reader.onerror = error => reject(error);
            reader.readAsDataURL(file);
        });
    }

    async function sendPhotoToAPI(photoData: PhotoUploadData) {
        try {
            const response = await fetch('api/photo', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(photoData)
            });
            if (!response.ok) throw new Error('Failed to upload photo.');
            const result = await response.json();
            console.log('Uploaded successfully:', result);

            fetchAlbum();
        } catch (error: any) {
            console.error('Error uploading photo:', error);
        }
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    if (!album) {
        return <div>Loading...</div>;
    }

    return (
        <div className='centered-container'>
            <h1>Album: {album.name}</h1>
            <button onClick={handleUploadClick}>Upload Photo</button>
            {isUploading && (
                <input type="file" accept="image/*" onChange={handleFileSelect} />
            )}
            <table>
                <tbody>
                    <tr>
                        {album.photos.map(photo => (
                            <PhotoComponent key={photo.id} photoId={photo.id} />
                        ))}
                    </tr>
                </tbody>
            </table>
        </div>
    );
};

export default AlbumDetails;
