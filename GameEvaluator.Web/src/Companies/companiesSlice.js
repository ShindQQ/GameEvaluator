import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { message } from "antd";

export const fetchCompanies = createAsyncThunk(
    'getCompanies',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Companies/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`);
        return await response.json();
    }
)

export const deleteCompany = createAsyncThunk(
    'deleteCompany',
    async (companyId, thunkAPI) => {
        const response = await fetch(`/api/Companies/${companyId}`, {
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

export const updateCompany = createAsyncThunk(
    'updateCompany',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Companies/${values.Id}`, {
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

export const addCompany = createAsyncThunk(
    'addCompany',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Companies/`, {
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

export const removeGameFromCompany = createAsyncThunk(
    'removeGameFromCompany',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Companies/${values.companyId}/games/${values.gameId}`, {
            method: "DELETE",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + JSON.parse(localStorage.getItem('auth')).AccessToken
            },
            }).then(response => {
                if(response.status === 500)
                    throw new Error('Company does not have this game');
                if(response.status !== 204)
                    throw new Error('Access Denied');
    
                return true;
            }).catch((error) => {
                message.error(error.message);
            });

        return response;
    }
)

export const addWorker = createAsyncThunk(
    'addWorker',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Companies/${values.companyId}/workers/${values.workerId}`, {
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

export const removeWorker = createAsyncThunk(
    'removeWorker',
    async (values, thunkAPI) => {
        const response = await fetch(`/api/Companies/${values.companyId}/workers/${values.workerId}`, {
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

export const companiesSlice = createSlice({
    name: 'companies',
    initialState: {
        status: 'idle',
        loading: false,
        data: {}
    },
    reducers: {

    },
    extraReducers: (builder) => {
        builder.addCase(fetchCompanies.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(fetchCompanies.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'succeeded'
            state.data = action.payload
        }).addCase(fetchCompanies.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(updateCompany.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(updateCompany.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(updateCompany.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(deleteCompany.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(deleteCompany.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(deleteCompany.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(addCompany.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(addCompany.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(addCompany.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(removeGameFromCompany.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(removeGameFromCompany.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(removeGameFromCompany.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(addWorker.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(addWorker.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(addWorker.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
        builder.addCase(removeWorker.pending, (state, action) => {
            state.loading = true;
            state.status = 'loading'
        }).addCase(removeWorker.fulfilled, (state, action) => {
            state.loading = false;
            state.status = 'idle'
        }).addCase(removeWorker.rejected, (state, action) => {
           state.loading = false;
           state.status = 'failed'
        })
    }
})

export const selectAllCompanies = state => state.companies.data
export default companiesSlice.reducer;