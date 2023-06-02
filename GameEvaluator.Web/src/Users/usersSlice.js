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

                return true;
            }).catch(() => {
                message.error('Access Denied');
            });

        return response;
    }
)

export const updateUser = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/${values.Id}`, {
            method: "PATCH",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            body: JSON.stringify({Name:values.name, Email:values.email})
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

export const addUser = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/`, {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            body: JSON.stringify({Name:values.name, Email:values.email, Password:values.password})
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

export const addGame = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/${values.userId}/games/${values.gameId}`, {
            method: "PUT",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            }).then(response => {
                if(response.status === 500)
                    throw new Error('Company does not have this worker');
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch((error) => {
                message.error(error.message);
            });

        return response;
    }
)

export const favorGame = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/${values.userId}/games/${values.gameId}/favorites`, {
            method: "PUT",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            }).then(response => {
                if(response.status === 500)
                    throw new Error('Company does not have this worker');
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch((error) => {
                message.error(error.message);
            });

        return response;
    }
)


export const rateGame = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/${values.userId}/games/${values.gameId}/ratings/${values.rating}`, {
            method: "PUT",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            }).then(response => {
                if(response.status === 500)
                    throw new Error('Company does not have this worker');
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch((error) => {
                message.error(error.message);
            });

        return response;
    }
)

export const banUser = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/ban`, {
            method: "PUT",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            body: JSON.stringify({UserId: values.userId, BanTo: values.date})
            }).then(response => {
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch((error) => {
                message.error(error.message);
            });

        return response;
    }
)

export const addRole = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/${values.userId}/roles/${values.role}`, {
            method: "PUT",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            }).then(response => {
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch((error) => {
                message.error(error.message);
            });

        return response;
    }
)

export const removeRole = createAsyncThunk(
    '/api/Users/',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Users/${values.userId}/roles/${values.role}`, {
            method: "PUT",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            }).then(response => {
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch((error) => {
                message.error(error.message);
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