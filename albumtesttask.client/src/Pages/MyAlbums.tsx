import React, { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';
import { Album } from '../types.ts';
import AlbumComponent from '../Components/AlbumComponent.tsx';
import AuthorizeView from "../Components/AuthorizeView.tsx";

const MyAlbums: React.FC = () => {
    const { userId } = useParams<'userId'>();
    const [albums, setAlbums] = useState<Album[]>([]);
    const [newAlbumName, setNewAlbumName] = useState('');
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        fetchAlbums();
    }, [userId]);

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

    async function deleteAlbum(albumId: number) {
        try {
            const response = await fetch(`api/album?id=${albumId}`, { method: 'DELETE' });
            if (!response.ok) {
                throw new Error('Failed to delete the album.');
            }
            setAlbums(prev => prev.filter(album => album.id !== albumId));
        } catch (error: any) {
            console.error('Error deleting album:', error);
            setError(error.message);
        }
    }

    async function createAlbum() {
        if (!newAlbumName) {
            alert('Please enter a name for the album.');
            return;
        }
        console.log(albums[0].userId);

        const newAlbum = {
            id: 0,
            name: newAlbumName,
            userId: albums[0].userId,
            photos: []
        };

        try {
            const response = await fetch('api/album', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(newAlbum)
            });

            if (!response.ok) {
                throw new Error('Failed to create the album.');
            }

            // const result = await response;
            setNewAlbumName('');
        } catch (error) {
            console.error('Error creating album:', error);
            setError('Failed to create album.');
        }
    }

    if (error) {
        return <div>Error: {error}</div>;
    }

    return (
        <AuthorizeView>
            <div className='centered-container'>
                <h1>My Albums</h1>
                <div>
                <input
                    type="text"
                    placeholder="Enter new album name"
                    value={newAlbumName}
                    onChange={e => setNewAlbumName(e.target.value)}
                />
                <button onClick={createAlbum}>Add Album</button>
                </div>
                <table>
                    <tbody>
                        <tr>
                            {albums.length > 0 ? (
                                albums.map(album => album.photos.length > 0 && (
                                    <AlbumComponent key={album.photos[0].id} photo={album.photos[0]} name={album.name} album={album} onDelete={deleteAlbum} showDeleteButton={true} />
                                ))
                            ) : (
                                <td>No albums found</td>
                            )}
                        </tr>
                    </tbody>
                </table>
            </div>
        </AuthorizeView>
    );
};

export default MyAlbums;
