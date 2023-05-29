import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

export const fetchPlatforms = createAsyncThunk(
    '/api/Platforms',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Platforms/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`, {
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

export const platformsSlice = createSlice({
    name: 'platforms',
    initialState: {
        status: 'idle',
        loading: false,
        data: {}
    },
    reducers: {

    },
    extraReducers: (builder) => {
        builder.addCase(fetchPlatforms.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(fetchPlatforms.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'succeeded'
            state.data = action.payload
        }).addCase(fetchPlatforms.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllPlatofrms = state => state.platforms.data
export default platformsSlice.reducer;