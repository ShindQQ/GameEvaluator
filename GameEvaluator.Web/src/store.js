import { configureStore } from '@reduxjs/toolkit'
import gamesReducer from './Games/gamesSlice'
import companiesReducer from './Companies/companiesSlice'
import genresSlice from './Genres/genresSlice'
import platformsSlice from './Platforms/platformsSlice'
import usersSlice from './Users/usersSlice'

export default configureStore({
  reducer: {
    games: gamesReducer,
    companies: companiesReducer,
    genres: genresSlice,
    platforms: platformsSlice,
    users: usersSlice
  },
})