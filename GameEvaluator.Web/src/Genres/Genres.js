import { useDispatch, useSelector } from "react-redux"
import { deleteGenre, fetchGenres, selectAllGenres } from "./genresSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Button, Table } from "antd";
import { DeleteOutlined } from "@ant-design/icons";


export const Genres = () => {
    const genres = useSelector(selectAllGenres);
    const genresStatus = useSelector(state => state.genres.status);
    const loading = useSelector(state => state.genres.loading);
    const dispatch = useDispatch();

    const columns = [
        {
            title: 'Number',
            dataIndex: 'index',
            key: 'number',
            sorter: (a, b) => a.index - b.index
        },
        {
            title: 'Name',
            dataIndex: 'name',
            key: 'name'
        },
        {
            title: 'Description',
            dataIndex: 'description',
            key: 'description'
        },
        {
            title: 'Actions',
            render: (_, record) => {
                return (
                    <>
                    <Button type='text' danger={true}
                    onClick={() => { 
                        dispatch(deleteGenre(record.key));
                        }}>
                        <DeleteOutlined />
                    </Button>
                    </>
                )
            }
        }
    ];

    const [tableParams, setTableParams] = useState({
        pagination:{
            current: 1,
            pageSize: 5,
            showSizeChanger: true, 
            pageSizeOptions: ['2', '5','10', '20', '30']
        },
    });

    const fetchData = (params) => {
        var tParams;
        if(params != null)
        {
            tParams = {
                pagination: {
                    current: params.current,
                    pageSize: params.pageSize,
                },
            };
            setTableParams(tParams);
        }
        else
        {
            tParams = tableParams;
        }
        
        setTableParams({
            ...tableParams,
            pagination: {
                current: tParams.pagination.current,
                pageSize: tParams.pagination.pageSize,
                showSizeChanger: true, 
                pageSizeOptions: ['2', '5','10', '20', '30'],
                total: genres.TotalCount
            }
        });
    }

    useEffect(() => {
        if(genresStatus === 'idle')
        {
            dispatch(fetchGenres(tableParams));
            fetchData();
        }
    }, [genresStatus, dispatch]);

    if(genresStatus === 'succeeded') 
    return (
        <Layout>
            <Table dataSource={genres.Items.map((genre, index) => {
                return {
                    key: genre.Id,
                    index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                    name: genre.Name,
                    description: genre.Description,
                }})} 
                pagination={tableParams.pagination}
                loading={loading}
                columns={columns}
                onChange={fetchData}> 
            </Table>
        </Layout>
    ); 
}