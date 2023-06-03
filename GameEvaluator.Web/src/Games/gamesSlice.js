import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { message } from "antd";

export const fetchGames = createAsyncThunk(
    'fetchGames',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Games/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`);
        return await response.json();
    }
)

export const deleteGame = createAsyncThunk(
    'deleteGame',
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

export const addGame = createAsyncThunk(
    'addGame',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Companies/${values.companyId}/games`, {
            method: "POST",
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

export const updateGame = createAsyncThunk(
    'updateGame',
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

export const addGenre = createAsyncThunk(
    'addGenre',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Games/${values.gameId}/companies/${values.companyId}/genres/${values.genreId}`, {
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

export const removeGenre = createAsyncThunk(
    'removeGenre',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Games/${values.gameId}/companies/${values.companyId}/genres/${values.genreId}`, {
            method: "DELETE",
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

export const addPlatform = createAsyncThunk(
    'addPlatform',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Games/${values.gameId}/companies/${values.companyId}/platforms/${values.platformId}`, {
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

export const removePlatform = createAsyncThunk(
    'removePlatform',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Games/${values.gameId}/companies/${values.companyId}/platforms/${values.platformId}`, {
            method: "DELETE",
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
        builder.addCase(deleteGame.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(deleteGame.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(deleteGame.rejected, (state, action) => {
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
        builder.addCase(updateGame.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(updateGame.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(updateGame.rejected, (state, action) => {
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
        builder.addCase(removeGenre.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(removeGenre.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(removeGenre.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(addPlatform.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(addPlatform.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(addPlatform.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(removePlatform.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(removePlatform.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(removePlatform.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllGames = state => state.games.data
export default gamesSlice.reducer;