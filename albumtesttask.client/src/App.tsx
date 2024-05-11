import React from 'react';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import AllAlbums from './Pages/AllAlbums.tsx';
import AlbumDetails from './Pages/AlbumDetails.tsx';
import Login from './Pages/Login.tsx';
import Register from './Pages/Register.tsx';
import MyAlbums from './Pages/MyAlbums.tsx';
import LogoutLink from "./Components/LogoutLink.tsx";
import { AuthorizedUser } from "./Components/AuthorizeView.tsx";
import './Components/AlbumComponent.css';

const App: React.FC = () => {
    return (
        <Router>
            <div>
                <nav className='navbar'>
                    <ul>
                        <li><Link to="/">All Albums</Link></li>
                        <li><Link to="/myAlbums">My Albums</Link></li>
                    </ul>
                    <ul className='logout-navbar'>
                        <li><span><LogoutLink>Logout <AuthorizedUser value="email" /></LogoutLink></span></li>
                    </ul>
                </nav>
                <Routes>
                    <Route path="/" element={<AllAlbums />} />
                    <Route path="/:albumId" element={<AlbumDetails />} />
                    <Route path="/myAlbums" element={<MyAlbums />} />
                    <Route path="/login" element={<Login />} />
                    <Route path="/register" element={<Register />} />
                </Routes>
            </div>
        </Router>
    );
}

export default App;

