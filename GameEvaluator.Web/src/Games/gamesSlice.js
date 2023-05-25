import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

export const fetchGames = createAsyncThunk(
    '/api/Games',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Games/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`);
        return await response.json();
    }
)

export const gamesSlice = createSlice({
    name: 'games',
    initialState: {
        status: 'idle',
        loading: false,
        data: [] 
    },
    reducers: {
       
    },
    extraReducers: (builder) => {
        builder.addCase(fetchGames.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
          }).addCase(fetchGames.fulfilled, (state, action) => {
              state.loading = false;
              state.status = 'succeeded'
              state.data.push(action.payload)
          }).addCase(fetchGames.rejected, (state, action) => {
            state.loading = false;
            state.status = 'failed'
          })
        }
})
export const selectAllGames = state => state.games.data
export default gamesSlice.reducer;