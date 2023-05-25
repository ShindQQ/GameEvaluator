import { configureStore } from '@reduxjs/toolkit'
import gamesReducer from './Games/gamesSlice'

export default configureStore({
  reducer: {
    games: gamesReducer,
  },
})