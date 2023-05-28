import { useEffect, useState } from "react";
import { Login } from "./Login/Login";
import { Navigate, Route, Routes } from "react-router-dom";
import { Games } from "./Games/Games";
import { Home } from "./Home/Home";

const App = () => {
    if (localStorage.getItem("auth") != null && new Date(localStorage.getItem("auth").Expiration) >= new Date()) {
        localStorage.removeItem("auth");
        localStorage.removeItem("isAuthenticated");
    }

    const [isAuthenticated, setIsAuthenticated] = useState(
        () => JSON.parse(localStorage.getItem('auth')) == null ? false : true
    );

    useEffect(()=>{
        localStorage.setItem("isAuthenticated", JSON.stringify(isAuthenticated));
    }, [isAuthenticated]);

    return (
        <Routes>
            <Route path="/" element={isAuthenticated ? <Home /> : <Navigate to="/login" replace />}/>
            <Route path="/login" element={<Login />}/>
            <Route path="/games" element={<Games />}/>
        </Routes>
    )
}

export default App;