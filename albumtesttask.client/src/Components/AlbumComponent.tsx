import React from 'react';
import { useNavigate  } from 'react-router-dom';
import { Photo } from '../types.ts';

type Album = {
    id: number;
    name: string;
    userId: string;
    photos: Photo[];
};

interface AlbumProps {
    photo: Photo;
    name: string;
    album: Album;
    showDeleteButton?: boolean;
    onDelete?: (albumId: number) => void;
}

const AlbumComponent: React.FC<AlbumProps> = ({ photo, name, album, onDelete, showDeleteButton = false }) => {
    let history = useNavigate();

    const openAlbum = () => {
        history(`${album.id}`);
    };

    return (
        <td>
            <div className='album-container' onClick={openAlbum}>
                <img className='album-photo' src={`data:${photo.contentType};base64,${photo.data}`} alt={`Photo ${photo.id}`} />
                <p>{name}</p>
                {showDeleteButton && onDelete && (
                    <button onClick={() => onDelete(album.id)}>Delete Album</button>
                )}
            </div>
        </td>
    );
};

export default AlbumComponent;
