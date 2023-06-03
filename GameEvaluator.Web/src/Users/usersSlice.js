import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { message } from "antd";

export const fetchUsers = createAsyncThunk(
    'fetchUsers',
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
    'deleteUser',
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
    'updateUser',
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
    'addUser',
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
    'addGame',
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
    'favorGame',
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
    'rateGame',
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
    'banUser',
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
    'addRole',
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
    'removeRole',
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
        builder.addCase(deleteUser.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(deleteUser.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(deleteUser.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(updateUser.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(updateUser.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(updateUser.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(addUser.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(addUser.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(addUser.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(addGame.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(addGame.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(addGame.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(favorGame.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(favorGame.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(favorGame.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(rateGame.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(rateGame.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(rateGame.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(banUser.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(banUser.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(banUser.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(addRole.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(addRole.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(addRole.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(removeRole.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(removeRole.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(removeRole.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllUsers = state => state.users.data
export default usersSlice.reducer;