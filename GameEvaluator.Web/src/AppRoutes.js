import { Games } from "./Games/Games";
import { Home } from "./Home/Home";

const AppRoutes = [
    {
        path: '/',
        element: <Home />
    },
    {
        path: '/games',
        element: <Games />
    }
];

export default AppRoutes;