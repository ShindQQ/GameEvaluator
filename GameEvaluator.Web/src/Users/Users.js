import { useDispatch, useSelector } from "react-redux"
import { addGame, addRole, addUser, banUser, deleteUser, favorGame, fetchUsers, rateGame, removeRole, selectAllUsers, updateUser } from "./usersSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Button, DatePicker, Form, Input, InputNumber, Modal, Select, Table } from "antd";
import { AppstoreAddOutlined, DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { AddUser } from "./AddUser";
import { AddGame } from "./AddGame";
import { RateGame } from "./RateGame";
import { Ban } from "./Ban";
import { FavorGame } from "./FavorGame";
import { AddRole } from "./AddRole";
import { RemoveRole } from "./RemoveRole";

export const Users = () => {
    const [editRow, setEditRow] = useState(null);
    const [addRow, setAddRow] = useState(false);
    const [rating, setRating] = useState(5);
    const [banState, setBanState] = useState(false);
    const [favorState, setFavorState] = useState(false);
    const [addRoleState, setAddRoleState] = useState(false);
    const [removeRoleState, setRemoveRoleState] = useState(false);
    const [role, setRole] = useState(null);
    const [userKey, setUserKey] = useState(null);
    const [addGameState, setAddGameState] = useState(false);
    const [ratingState, setRatingState] = useState(false);
    const users = useSelector(selectAllUsers);
    const usersStatus = useSelector(state => state.users.status);
    const loading = useSelector(state => state.users.loading);
    const dispatch = useDispatch();
    const [form] = Form.useForm();

    const columns = [
        {
            title: 'Number',
            dataIndex: 'index',
            key: 'number',
            sorter: (a, b) => a.index - b.index
        },
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'id',
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
                        <div style={{display:'flex', gap: '0px', flexDirection:'row', padding: '5px' }}>
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
                        <div  style={{display:'flex', gap: '0px', flexDirection:'row', padding: '5px' }}>
                            <Button type='text' danger={true}
                            onClick={() => { 
                                    dispatch(deleteUser(record.key));
                                }}>
                                <DeleteOutlined />
                            </Button>
                        </div>
                        <div style={{display:'flex', gap: '10px', flexDirection:'row', padding: '5px' }}>
                            <Button type='default' 
                                onClick={() => { 
                                    setUserKey(record.key);
                                    setAddGameState(true);
                                }}>
                                    Add Game
                            </Button>
                            <Button type='default' 
                                onClick={() => { 
                                    setUserKey(record.key);
                                    setRatingState(true);
                                }}>
                                    Rate game
                            </Button>
                        </div>
                        <div  style={{display:'flex', gap: '10px', flexDirection:'row', padding: '5px' }}>
                            <Button type='default'
                                onClick={() => { 
                                    setUserKey(record.key);
                                    setFavorState(true);
                                }}>
                                    Favor Game
                            </Button>
                            <Button type='default' danger={true}
                                onClick={() => { 
                                    setUserKey(record.key);
                                    setBanState(true);
                                }}>
                                    Ban
                            </Button>
                            
                        </div>
                        <div  style={{display:'flex', gap: '10px', flexDirection:'row', padding: '5px' }}>
                            <Button type='default'
                                onClick={() => { 
                                    setUserKey(record.key);
                                    setAddRoleState(true);
                                }}>
                                    Add Role
                            </Button>
                            <Button type='default' danger={true}
                                onClick={() => { 
                                    setUserKey(record.key);
                                    setAddRoleState(true);
                                }}>
                                    Remove Role
                            </Button>
                        </div>
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

    const onFinish = (values) => {
        dispatch(updateUser({Id: editRow, name: values.name, email: values.email}));
        setEditRow(null);
    }

    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = (values) =>{
        dispatch(addUser({name: values.name, email: values.email, password: values.password}));
    }

    const handleAddGame = (values) =>{
        dispatch(addGame({userId: userKey, gameId: values.gameId}));
    }

    const handleRating = (values) =>{
        dispatch(rateGame({userId: userKey, gameId: values.gameId, rating: rating}));
    }

    const handleBan = (values) =>{
        dispatch(banUser({userId: userKey, date: values.date}));
    }

    const handleFavor = (values) =>{
        dispatch(favorGame({userId: userKey, gameId: values.gameId}));
    }

    const handleAddRole = (values) =>{
        dispatch(addRole({userId: userKey, role: values.role}));
    }

    const handleRemoveRole = (values) =>{
        dispatch(removeRole({userId: userKey, role: values.role}));
    }


    if(usersStatus === 'succeeded') 
    return (
        <Layout>
            <Button onClick={onAdd} type="primary" htmlType="submit">
                <AppstoreAddOutlined /> Add User
            </Button>
            <Form form={form} onFinish={onFinish}>
                <Table dataSource={users.Items.map((user, index) => {
                    return {
                        key: user.Id,
                        id: user.Id,
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
                    scroll={{
                        x: 1500,
                      }}
                    pagination={tableParams.pagination}
                    loading={loading}
                    columns={columns}
                    onChange={fetchData}> 
                </Table>
            </Form>
            <AddUser addRow={addRow} setAddRow={setAddRow} handleAdd={handleAdd} />
            <AddGame addGameState={addGameState} setAddGameState={setAddGameState} handleAddGame={handleAddGame} />
            <RateGame ratingState={ratingState} setRatingState={setRatingState} handleRating={handleRating} setRating={setRating} />
            <Ban banState={banState} setBanState={setBanState} handleBan={handleBan} />
            <FavorGame favorState={favorState} setFavorState={setFavorState} handleFavor={handleFavor} />
            <AddRole addRoleState={addRoleState} setAddRoleState={setAddRoleState} handleAddRole={handleAddRole} setRole={setRole} />
            <RemoveRole removeRoleState={removeRoleState} setRemoveRoleState={setRemoveRoleState} handleRemoveRole={handleRemoveRole} setRole={setRole} />
        </Layout>
    ); 
}