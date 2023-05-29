import { useDispatch, useSelector } from "react-redux"
import { fetchUsers, selectAllUsers } from "./usersSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Table } from "antd";

export const Users = () => {
    const users = useSelector(selectAllUsers);
    const usersStatus = useSelector(state => state.users.status);
    const loading = useSelector(state => state.users.loading);
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
            title: 'Email',
            dataIndex: 'email',
            key: 'email'
        },
        {
            title: 'Games',
            dataIndex: 'games',
            key: 'games'
        },
        {
            title: 'Roles',
            dataIndex: 'roles',
            key: 'roles'
        },
        {
            title: 'Company',
            dataIndex: 'company',
            key: 'company'
        },
        {
            title: 'Banned',
            dataIndex: 'banned',
            key: 'banned'
        },
        {
            title: 'BannedAt',
            dataIndex: 'bannedAt',
            key: 'bannedAt'
        },
        {
            title: 'BannedTo',
            dataIndex: 'bannedTo',
            key: 'bannedTo'
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
                total: users.TotalCount
            }
        });
    }

    useEffect(() => {
        if(usersStatus === 'idle')
        {
            dispatch(fetchUsers(tableParams));
            fetchData();
        }
    }, [usersStatus, dispatch]);

    if(usersStatus === 'succeeded') 
    return (
        <Layout>
            <Table dataSource={users.Items.map((user, index) => {
                return {
                    key: user.Id,
                    index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                    name: user.Name,
                    email: user.Email,
                    games: user.Games.length !== 0 ? user.Games.map(game => game.Name).join(' ') : "User has no games",
                    roles: user.Roles.join(' '),
                    company: user.Company,
                    banned: user.Banned == true ? 'Banned' : 'Not Banned',
                    bannedAt: user.BannedAt != null ? user.BannedAt : 'Not Banned',
                    bannedTo: user.BannedTo != null ? user.BannedTo : 'Not Banned'
                }})} 
                pagination={tableParams.pagination}
                loading={loading}
                columns={columns}
                onChange={fetchData}> 
            </Table>
        </Layout>
    ); 
}