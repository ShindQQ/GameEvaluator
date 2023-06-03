import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { message } from "antd";

export const fetchGenres = createAsyncThunk(
    'fetchGenres',
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

export const deleteGenre = createAsyncThunk(
    'deleteGenre',
    async (genreId, thunkAPI) => {
        const response = await fetch(`/api/Genres/${genreId}`, {
            method: "DELETE",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            }
            }).then(response => {
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch(() => {
                message.error('Access Denied');
            });

        return response;
    }
)

export const updateGenre = createAsyncThunk(
    'updateGenre',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Genres/${values.Id}`, {
            method: "PATCH",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            body: JSON.stringify({Name:values.name, Description:values.description})
            }).then(response => {
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch(() => {
                message.error('Access Denied');
            });

        return response;
    }
)

export const addGenre = createAsyncThunk(
    'addGenre',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Genres/`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            body: JSON.stringify({Name:values.name, Description:values.description})
            }).then(response => {
                if(response.status !== 200)
                    throw new Error('Access Denied');
    
                return true;
            }).catch(() => {
                message.error('Access Denied');
            });

        return response;
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
        builder.addCase(deleteGenre.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(deleteGenre.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(deleteGenre.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(updateGenre.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(updateGenre.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(updateGenre.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(addGenre.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(addGenre.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(addGenre.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllGenres = state => state.genres.data
export default genresSlice.reducer;