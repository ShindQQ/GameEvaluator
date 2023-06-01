import { useEffect, useState } from "react";
import { Button, Form, Input, Modal, Table } from 'antd';
import { addGame, addGenre, addPlatform, deleteGame, fetchGames, removeGenre, removePlatform, selectAllGames, updateGame } from "./gamesSlice";
import { useSelector } from "react-redux";
import { useDispatch } from "react-redux";
import { Layout } from '../Layout';
import { AppstoreAddOutlined, DeleteOutlined, EditFilled, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";

export const Games = () => {
    const [editRow, setEditRow] = useState(null);
    const [addRow, setAddRow] = useState(false);
    const [gameKey, setGameKey] = useState(null);
    const [addGenreState, setAddGenreState] = useState(false);
    const [removeGenreState, setRemoveGenreState] = useState(false);
    const [addPlatformState, setAddPlatformState] = useState(false);
    const [removePlatformState, setRemovePlatformState] = useState(false);
    const games = useSelector(selectAllGames);
    const gamesStatus = useSelector(state => state.games.status);
    const loading = useSelector(state => state.games.loading);
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
            title: 'Description',
            dataIndex: 'description',
            render: (text, record)=>{
                if(editRow === record.key){
                    return (
                        <Form.Item name="description"
                        rules={[{
                            required:true,
                            message:'Please enter description',
                        },
                        {
                            min: 20,
                            max: 200,
                            message:'Description should have from 20 to 200 characters'
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
            title: 'Average Rating',
            key: 'averageRating',
            render: payload => {
                console.log(payload);
                return <p>{payload.AverageRating && payload.averageRating != 0 ? "No users" : payload.averageRating.toPrecision(2)}</p>
            },
            sorter: (a, b) => a.index - b.index
        },
        {
            title: 'Genres',
            dataIndex: 'genres',
            key: 'genres'
        },
        {
            title: 'Companies',
            dataIndex: 'companies',
            key: 'companies'
        },
        {
            title: 'Platforms',
            dataIndex: 'platforms',
            key: 'platforms'
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
                                    description: record.description
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
                                const response = await dispatch(deleteGame(record.key));
                                if(response.payload === true)
                                navigate(0);
                            }}>
                                <DeleteOutlined />
                            </Button>
                        </div>
                        <div style={{display:'flex', gap: '10px', flexDirection:'row', padding: '5px' }}>
                            <Button type='default' 
                            onClick={() => { 
                                setGameKey(record.key);
                                setAddGenreState(true);
                            }}>
                                Add Genre
                            </Button>
                            <Button type='default' danger={true}
                            onClick={() => { 
                                setGameKey(record.key);
                                setRemoveGenreState(true);
                            }}>
                                Remove Genre
                            </Button>
                        </div>
                        <div style={{display:'flex', gap: '10px', flexDirection:'row', padding: '5px' }}>
                            <Button type='default' 
                            onClick={() => { 
                                setGameKey(record.key);
                                setAddPlatformState(true);
                            }}>
                                Add Platform
                            </Button>
                            <Button type='default' danger={true}
                            onClick={() => { 
                                setGameKey(record.key);
                                setRemovePlatformState(true);
                            }}>
                                Remove Platform
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
                total: games.TotalCount
            }
        });
    }
    
    useEffect(() => {
        if(gamesStatus === 'idle')
        {
            dispatch(fetchGames(tableParams));
            fetchData();
        }
    }, [gamesStatus, dispatch]);

    const onFinish = async (values) => {
        const response = await dispatch(updateGame({Id: editRow, name: values.name, description: values.description}));
        if(response.payload === true)
            navigate(0);
        setEditRow(null);
    }
    
    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = async (values) =>{
        const response = await dispatch(addGame({companyId:values.companyId, name: values.name, description: values.description}));
        if(response.payload === true)
            navigate(0);
    }
    
    const handleAddGenre = async (values) =>{
        const response = await dispatch(addGenre({gameId: gameKey, companyId: values.companyId, genreId: values.genreId}));
        if(response.payload === true)
            navigate(0);
    }

    const handleRemoveGenre = async (values) =>{
        const response = await dispatch(removeGenre({gameId: gameKey, companyId: values.companyId, genreId: values.genreId}));
        if(response.payload === true)
            navigate(0);
    }

    const handleAddPlatform = async (values) =>{
        const response = await dispatch(addPlatform({gameId: gameKey, companyId: values.companyId, platformId: values.platformId}));
        if(response.payload === true)
            navigate(0);
    }

    const handleRemovePlatform = async (values) =>{
        const response = await dispatch(removePlatform({gameId: gameKey, companyId: values.companyId, platformId: values.platformId}));
        if(response.payload === true)
            navigate(0);
    }

    if(gamesStatus === 'succeeded') 
    return (
        <Layout>
            <Button onClick={onAdd} type="primary" htmlType="submit">
                <AppstoreAddOutlined /> Add Game
            </Button>
            <Form form={form} onFinish={onFinish}>
                <Table dataSource={games.Items.map((game, index) => {
                    return {
                        key: game.Id,
                        id: game.Id,
                        index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                        name: game.Name,
                        description: game.Description,
                        averageRating: game.AverageRating,
                        genres: game.Genres.length !== 0 ? game.Genres.join(' ') : 'There is no genres yet',
                        companies: game.CompaniesNames,
                        platforms: game.Platforms.length !== 0 ? game.Platforms.join(' ') : 'There is no platforms yet'
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
            title="Add Game"
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
                    <Form.Item name="companyId"
                            rules={[{
                                required:true,
                                message:'Company ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Company ID',
                            }
                            ]}>
                                <div>
                                    Company Id <Input />
                                </div>
                    </Form.Item>
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
                    <Form.Item name="description"
                            rules={[{
                                required:true,
                                message:'Please enter description',
                            },
                            {
                                min: 20,
                                max: 200,
                                message:'Description should have from 20 to 200 characters'
                            }
                            ]}>
                                <div>
                                    Description <Input />
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
            open={addGenreState}
            title="Add Genre"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setAddGenreState(false);
            }}
            onOk={() => {
                setAddGenreState(false);
            }}
            >
                <Form onFinish={handleAddGenre}>
                    <Form.Item name="genreId"
                            rules={[{
                                required:true,
                                message:'Genre ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Genre ID',
                            }
                            ]}>
                                <div>
                                    Genre Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item name="companyId"
                            rules={[{
                                required:true,
                                message:'Company ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Company ID',
                            }
                            ]}>
                                <div>
                                    Company Id <Input />
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
            open={removeGenreState}
            title="Remove Genre"
            okText="Remove"
            footer={[]}
            onCancel={() => {
                setRemoveGenreState(false);
            }}
            onOk={() => {
                setRemoveGenreState(false);
            }}
            >
                <Form onFinish={handleRemoveGenre}>
                    <Form.Item name="genreId"
                            rules={[{
                                required:true,
                                message:'Genre ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Genre ID',
                            }
                            ]}>
                                <div>
                                    Genre Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item name="companyId"
                            rules={[{
                                required:true,
                                message:'Company ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Company ID',
                            }
                            ]}>
                                <div>
                                    Company Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item>
                        <Button key="submit" htmlType="submit" type="primary" block >
                            Remove
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
            <Modal
            open={addPlatformState}
            title="Add Platform"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setAddPlatformState(false);
            }}
            onOk={() => {
                setAddPlatformState(false);
            }}
            >
                <Form onFinish={handleAddPlatform}>
                    <Form.Item name="platformId"
                            rules={[{
                                required:true,
                                message:'Platform ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Platform ID',
                            }
                            ]}>
                                <div>
                                    Platform Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item name="companyId"
                            rules={[{
                                required:true,
                                message:'Company ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Company ID',
                            }
                            ]}>
                                <div>
                                    Company Id <Input />
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
            open={removePlatformState}
            title="Remove Platform"
            okText="Remove"
            footer={[]}
            onCancel={() => {
                setRemovePlatformState(false);
            }}
            onOk={() => {
                setRemovePlatformState(false);
            }}
            >
                <Form onFinish={handleRemovePlatform}>
                    <Form.Item name="platformId"
                            rules={[{
                                required:true,
                                message:'Platform ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Platform ID',
                            }
                            ]}>
                                <div>
                                    Platform Id <Input />
                                </div>
                    </Form.Item>
                    <Form.Item name="companyId"
                            rules={[{
                                required:true,
                                message:'Company ID is required',
                            },
                            {
                                pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                                message: 'Please enter a valid Company ID',
                            }
                            ]}>
                                <div>
                                    Company Id <Input />
                                </div>
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