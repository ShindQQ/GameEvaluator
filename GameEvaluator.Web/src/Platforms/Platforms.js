import { useDispatch, useSelector } from "react-redux"
import { fetchPlatforms, selectAllPlatofrms } from "./platformsSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Table } from "antd";

export const Platforms = () => {
    const platforms = useSelector(selectAllPlatofrms);
    const platformsStatus = useSelector(state => state.platforms.status);
    const loading = useSelector(state => state.platforms.loading);
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
                total: platforms.TotalCount
            }
        });
    }

    useEffect(() => {
        if(platformsStatus === 'idle')
        {
            dispatch(fetchPlatforms(tableParams));
            fetchData();
        }
    }, [platformsStatus, dispatch]);

    if(platformsStatus === 'succeeded') 
    return (
        <Layout>
            <Table dataSource={platforms.Items.map((platform, index) => {
                return {
                    key: platform.Id,
                    index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                    name: platform.Name,
                    description: platform.Description,
                }})} 
                pagination={tableParams.pagination}
                loading={loading}
                columns={columns}
                onChange={fetchData}> 
            </Table>
        </Layout>
    ); 
}