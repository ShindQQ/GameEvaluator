import { useDispatch, useSelector } from "react-redux"
import { deleteUser, fetchUsers, selectAllUsers, updateUser } from "./usersSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Button, Form, Input, Table } from "antd";
import { DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

export const Users = () => {
    const [editRow, setEditRow] = useState(null);
    const users = useSelector(selectAllUsers);
    const usersStatus = useSelector(state => state.users.status);
    const loading = useSelector(state => state.users.loading);
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const [form] = Form.useForm();

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
             render: (text, record)=>{
                if(editRow === record.key){
                    return (
                        <Form.Item name="name"
                        rules={[{
                            required:true,
                            message:'Please enter name',
                        },
                        {
                            min: 3,
                            max: 20,
                            message:'Name should have from 3 to 20 characters'
                        }
                        ]}>
                            <Input />
                        </Form.Item>
                    )
                }
                else{
                    return <p>{text}</p>
                }
            }
        },
        {
            title: 'Email',
            dataIndex: 'email',
            render: (text, record)=>{
                if(editRow === record.key){
                    return (
                        <Form.Item name="email"
                        rules={[
                        {
                            type: 'email',
                            message: 'The input is not valid E-mail!',
                        },{
                            required:true,
                            message:'Please enter email',
                        },
                        {
                            min: 10,
                            max: 20,
                            message:'Email should have from 10 to 20 characters'
                        }
                        ]}>
                            <Input />
                        </Form.Item>
                    )
                }
                else{
                    return <p>{text}</p>
                }
            }
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
        {
            title: 'Actions',
            render: (_, record) => {
                return (
                    <>
                        <div>
                            <Button type='text' 
                            onClick={() => {
                                setEditRow(record.key);
                                form.setFieldsValue({
                                    name: record.name,
                                    email: record.email
                                });
                            }}>
                                <EditOutlined />
                            </Button>
                            <Button type='text' htmlType="submit">
                                <SaveOutlined />
                            </Button>
                        </div>
                        <Button type='text' danger={true}
                        onClick={async () => { 
                            const response = await dispatch(deleteUser(record.key));
                            if(response.payload === true)
                                navigate(0);
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

    const onFinish = async (values) => {
        const response = await dispatch(updateUser({Id: editRow, name: values.name, email: values.email}));
        if(response.payload === true)
            navigate(0);
        setEditRow(null);
    }

    if(usersStatus === 'succeeded') 
    return (
        <Layout>
            <Form form={form} onFinish={onFinish}>
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
            </Form>
        </Layout>
    ); 
}