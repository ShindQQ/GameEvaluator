import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";

export const fetchCompanies = createAsyncThunk(
    '/api/Companies',
    async (tableParams, thunkAPI) => {
        const response = await fetch(`/api/Companies/${tableParams.pagination.current}/${tableParams.pagination.pageSize}`);
        return await response.json();
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
    }
})

export const selectAllCompanies = state => state.companies.data
export default companiesSlice.reducer;