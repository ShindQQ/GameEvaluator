import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { message } from "antd";

export const fetchUsers = createAsyncThunk(
    '/api/Users',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Users/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`, {
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

export const deleteUser = createAsyncThunk(
    '/api/Users/',
    async (userId, thunkAPI) => {
        const response = await fetch(`/api/Users/${userId}`, {
            method: "DELETE",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            }
            }).then(response => {
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return response.json();
            }).then(() => {
                message.success("Deleted!")
            }).catch(() => {
                message.error("Access Denied!");
            });

        return response;
    }
)


export const usersSlice = createSlice({
    name: 'users',
    initialState: {
        status: 'idle',
        loading: false,
        data: {}
    },
    reducers: {

    },
    extraReducers: (builder) => {
        builder.addCase(fetchUsers.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(fetchUsers.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'succeeded'
            state.data = action.payload
        }).addCase(fetchUsers.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllUsers = state => state.users.data
export default usersSlice.reducer;