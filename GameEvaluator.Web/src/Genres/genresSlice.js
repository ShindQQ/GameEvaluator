import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

export const fetchGenres = createAsyncThunk(
    '/api/Genres',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Genres/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`, {
            method: "GET",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            }
            });

        return await response.json();
    }
)

export const genresSlice = createSlice({
    name: 'genres',
    initialState: {
        status: 'idle',
        loading: false,
        data: {}
    },
    reducers: {

    },
    extraReducers: (builder) => {
        builder.addCase(fetchGenres.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(fetchGenres.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'succeeded'
            state.data = action.payload
        }).addCase(fetchGenres.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllGenres = state => state.genres.data
export default genresSlice.reducer;