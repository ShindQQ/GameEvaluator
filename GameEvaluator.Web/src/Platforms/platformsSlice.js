import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { message } from "antd";

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

export const deletePlatform = createAsyncThunk(
    '/api/Platforms/',
    async (platformId, thunkAPI) => {
        const response = await fetch(`/api/Platforms/${platformId}`, {
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

export const updatePlatform = createAsyncThunk(
    '/api/Platforms/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Platforms/${values.Id}`, {
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

export const addPlatform = createAsyncThunk(
    '/api/Platforms/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Platforms/`, {
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