import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { message } from "antd";

export const fetchGames = createAsyncThunk(
    '/api/Games',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Games/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`);
        return await response.json();
    }
)

export const deleteGame = createAsyncThunk(
    '/api/Games/',
    async (gameId, thunkAPI) => {
        const response = await fetch(`/api/Games/${gameId}`, {
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

export const updateGame = createAsyncThunk(
    '/api/Games/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Games/${values.Id}`, {
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

export const gamesSlice = createSlice({
    name: 'games',
    initialState: {
        status: 'idle',
        loading: false,
        data: {} 
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
            state.data = action.payload
        }).addCase(fetchGames.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllGames = state => state.games.data
export default gamesSlice.reducer;