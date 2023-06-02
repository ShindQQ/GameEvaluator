import { useDispatch, useSelector } from "react-redux"
import { addGame, addRole, addUser, banUser, deleteUser, favorGame, fetchUsers, rateGame, removeRole, selectAllUsers, updateUser } from "./usersSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Button, DatePicker, Form, Input, InputNumber, Modal, Select, Table } from "antd";
import { AppstoreAddOutlined, DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

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
                            onClick={async () => { 
                                const response = await dispatch(deleteUser(record.key));
                                if(response.payload === true)
                                    navigate(0);
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

    const onFinish = async (values) => {
        const response = await dispatch(updateUser({Id: editRow, name: values.name, email: values.email}));
        if(response.payload === true)
            navigate(0);
        setEditRow(null);
    }

    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = async (values) =>{
        const response = await dispatch(addUser({name: values.name, email: values.email, password: values.password}));
        if(response.payload === true)
            navigate(0);
    }

    const handleAddGame = async (values) =>{
        const response = await dispatch(addGame({userId: userKey, gameId: values.gameId}));
        if(response.payload === true)
            navigate(0);
    }

    const handleRating = async (values) =>{
        const response = await dispatch(rateGame({userId: userKey, gameId: values.gameId, rating: rating}));
        if(response.payload === true)
            navigate(0);
    }

    const handleBan = async (values) =>{
        const response = await dispatch(banUser({userId: userKey, date: values.date}));
        if(response.payload === true)
            navigate(0);
    }

    const handleFavor = async (values) =>{
        const response = await dispatch(favorGame({userId: userKey, gameId: values.gameId}));
        if(response.payload === true)
            navigate(0);
    }

    const handleAddRole = async (values) =>{
        const response = await dispatch(addRole({userId: userKey, role: values.role}));
        if(response.payload === true)
            navigate(0);
    }

    const handleRemoveRole = async (values) =>{
        const response = await dispatch(removeRole({userId: userKey, role: values.role}));
        if(response.payload === true)
            navigate(0);
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
            <Modal
            open={addRow}
            title="Add User"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setAddRow(false);
            }}
            onOk={() => {
                setAddRow(false);
            }}
            >
                <Form onFinish={handleAdd}>
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
                                <div>
                                    Name <Input />
                                </div>
                    </Form.Item>
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
                                <div>
                                    Email <Input />
                                </div>
                    </Form.Item>
                    <Form.Item name="password"
                            rules={[{
                                required:true,
                                message:'Please enter password',
                            },
                            {
                                min: 6,
                                max: 10,
                                message:'Password should have from 6 to 10 characters'
                            }
                            ]}>
                                <div>
                                    Password <Input />
                                </div>
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Add
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
            <Modal
            open={addGameState}
            title="Add Game"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setAddGameState(false);
            }}
            onOk={() => {
                setAddGameState(false);
            }}
            >
                <Form onFinish={handleAddGame}>
                    <Form.Item name="gameId"
                            rules={[{
                                required:true,
                                message:'Game ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Game ID',
                            }
                            ]}>
                                <div>
                                    Game Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Add
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
            <Modal
            open={ratingState}
            title="Rate Game"
            okText="Rate"
            footer={[]}
            onCancel={() => {
                setRatingState(false);
            }}
            onOk={() => {
                setRatingState(false);
            }}
            >
                <Form onFinish={handleRating}>
                    <Form.Item name="gameId"
                            rules={[{
                                required:true,
                                message:'Game ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Game ID',
                            }
                            ]}>
                                <div>
                                    Game Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item name="rating">
                                <div>
                                    Rating <InputNumber min={1} max={10} defaultValue={5} onChange={(value) => setRating(value)}/>
                                </div>
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Rate
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
            <Modal
            open={banState}
            title="Ban"
            okText="Ban"
            footer={[]}
            onCancel={() => {
                setBanState(false);
            }}
            onOk={() => {
                setBanState(false);
            }}
            >
                <Form onFinish={handleBan}>
                    <Form.Item name="date">
                                <div>
                                    Game Id <DatePicker format="YYYY-MM-DDTHH:mm:ss.SSSZ" />
                                </div>
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Ban
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
            <Modal
            open={favorState}
            title="Favor Game"
            okText="Favor"
            footer={[]}
            onCancel={() => {
                setFavorState(false);
            }}
            onOk={() => {
                setFavorState(false);
            }}
            >
                <Form onFinish={handleFavor}>
                    <Form.Item name="gameId"
                            rules={[{
                                required:true,
                                message:'Game ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Game ID',
                            }
                            ]}>
                                <div>
                                    Game Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Favor
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
            <Modal
            open={addRoleState}
            title="Add Role"
            okText="Role"
            footer={[]}
            onCancel={() => {
                setAddRoleState(false);
            }}
            onOk={() => {
                setAddRoleState(false);
            }}
            >
                <Form onFinish={handleAddRole}>
                    <Form.Item name="role">
                        <Select 
                        onChange={(value) => setRole(value)}
                        options = {[
                            {
                                label: 'User',
                                value: 'User'
                            },
                            {
                                label: 'Admin',
                                value: 'Admin'
                            },
                            {
                                label: 'Company',
                                value: 'Company'
                            },
                            {
                                label: 'SuperAdmin',
                                value: 'SuperAdmin'
                            },
                        ]}
                         />
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Add
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
            <Modal
            open={removeRoleState}
            title="Remove Role"
            okText="Role"
            footer={[]}
            onCancel={() => {
                setRemoveRoleState(false);
            }}
            onOk={() => {
                setRemoveRoleState(false);
            }}
            >
                <Form onFinish={handleRemoveRole}>
                    <Form.Item name="role">
                        <Select 
                        onChange={(value) => setRole(value)}
                        options = {[
                            {
                                label: 'User',
                                value: 'User'
                            },
                            {
                                label: 'Admin',
                                value: 'Admin'
                            },
                            {
                                label: 'Company',
                                value: 'Company'
                            },
                            {
                                label: 'SuperAdmin',
                                value: 'SuperAdmin'
                            },
                        ]}
                         />
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Remove
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
        </Layout>
    ); 
}