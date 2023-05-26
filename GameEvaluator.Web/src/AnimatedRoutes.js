
import { Route, Routes, useLocation } from 'react-router-dom';
import { AnimatePresence } from 'framer-motion';
import AppRoutes from './AppRoutes';

function AnimatedRoutes() {
    const location = useLocation();

    return (
        <AnimatePresence>
            <Routes location={location} key={location.pathName}>
                {AppRoutes.map((route, index) => {
                    const {element, ...rest} = route;
                    return <Route key={index} {...rest} element={element} />;
                })}
            </Routes>
        </AnimatePresence>
    );
}

export default AnimatedRoutes;