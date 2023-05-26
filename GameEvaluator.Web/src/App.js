import { useEffect, useState } from "react";
import AnimatedRoutes from "./AnimatedRoutes"
import { Layout } from "./Layout"
import { Login } from "./Login/Login";

const App = () => {
    if (localStorage.getItem("auth") != null && new Date().getTime() - localStorage.getItem("auth") > 36000000) {
        localStorage.removeItem("auth");
        localStorage.removeItem("isAuthenticated");
    }

    const [isAuthenticated, setIsAuthenticated] = useState(
        () => JSON.parse(localStorage.getItem('auth')) == null ? false : true
    );

    useEffect(()=>{
        localStorage.setItem("isAuthenticated", JSON.stringify(isAuthenticated));
    }, [isAuthenticated]);

    if(isAuthenticated)
        return (
            <Layout>
                <AnimatedRoutes />
            </Layout>
        )
    else 
        return <Login />
}

export default App;