import { useEffect, useState } from "react";
import { Login } from "./Login/Login";
import { Navigate, Route, Routes, useNavigate } from "react-router-dom";
import { Games } from "./Games/Games";
import { Companies } from "./Companies/Companies";
import { Home } from "./Home/Home";
import { Genres } from "./Genres/Genres";
import { Platforms } from "./Platforms/Platforms";
import { Users } from "./Users/Users";

const App = () => {
    const [isAuthenticated, setIsAuthenticated] = useState(
        () => JSON.parse(localStorage.getItem('auth')) == null ? false : true
    );
    const navigate = useNavigate();

    if (localStorage.getItem('auth') != null && new Date(JSON.parse(localStorage.getItem('auth')).Expiration) <= new Date()) {
        localStorage.removeItem('auth');
        localStorage.removeItem('isAuthenticated');
        setIsAuthenticated(false);
        navigate("/login");
    }

    useEffect(()=>{
        localStorage.setItem("isAuthenticated", JSON.stringify(isAuthenticated));
    }, [isAuthenticated]);

    return (
        <Routes>
            <Route path="/home" element={isAuthenticated ? <Home /> : <Navigate to="/login" replace />}/>
            <Route path="/login" element={<Login />}/>
            <Route path="/games" element={isAuthenticated ? <Games /> : <Navigate to="/login" replace />}/>
            <Route path="/companies" element={isAuthenticated ? <Companies /> : <Navigate to="/login" replace />}/>
            <Route path="/genres" element={isAuthenticated ? <Genres /> : <Navigate to="/login" replace />}/>
            <Route path="/platforms" element={isAuthenticated ? <Platforms /> : <Navigate to="/login" replace />}/>
            <Route path="/users" element={isAuthenticated ? <Users /> : <Navigate to="/login" replace />}/>
        </Routes>
    )
}

export default App;