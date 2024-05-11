import React, { useState, useEffect } from 'react';
import { Photo } from '../types.ts';

interface PhotoComponentProps {
    photoId: number;
}

const PhotoComponent: React.FC<PhotoComponentProps> = ({ photoId }) => {
    const [photo, setPhoto] = useState<Photo | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        populatePhotos();
    }, [photoId]);

    async function populatePhotos() {
        try {
            const response = await fetch(`api/photo/${photoId}`);
            if (!response.ok) {
                throw new Error('Network response was not ok.');
            }
            const data: Photo = await response.json();
            setPhoto(data);
        } catch (error: any) {
            console.error('There was a problem with the fetch operation:', error);
            setError(error.message);
        }
    }

    async function handleDelete() {
        try {
            const response = await fetch(`api/photo?id=${photoId}`, {
                method: 'DELETE'
            });
            if (!response.ok) {
                throw new Error('Failed to delete the photo.');
            }
            console.log('Photo deleted successfully');
            setPhoto(null);
        } catch (error: any) {
            console.error('Error deleting photo:', error);
            setError(error.message);
        }
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <td>
            {photo ? (
                <div className='album-container'>
                    <img className='album-photo' src={`data:${photo.contentType};base64,${photo.data}`} alt={`Photo ${photo.id}`} />
                    <p>Likes: {photo.likeCounter} | Dislikes: {photo.dislikeCounter}</p>
                    <button onClick={handleDelete}>Delete Photo</button>
                </div>
            ) : null}
        </td>
    );
}

export default PhotoComponent;
