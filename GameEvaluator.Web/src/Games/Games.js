import { useEffect, useState } from "react";
import { Button, Form, Input, Modal, Table } from 'antd';
import { addGame, addGenre, addPlatform, deleteGame, fetchGames, removeGenre, removePlatform, selectAllGames, updateGame } from "./gamesSlice";
import { useSelector } from "react-redux";
import { useDispatch } from "react-redux";
import { Layout } from '../Layout';
import { AppstoreAddOutlined, DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { AddGame } from "./AddGame";
import { AddGenre } from "./AddGenre";
import { RemoveGenre } from "./RemoveGenre";
import { AddPlatform } from "./AddPlatform";
import { RemovePlatform } from "./RemovePlatform";

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
                            onClick={() => { 
                                dispatch(deleteGame(record.key));
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

    const onFinish = (values) => {
        dispatch(updateGame({Id: editRow, name: values.name, description: values.description}));
        setEditRow(null);
    }
    
    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = (values) =>{
       dispatch(addGame({companyId:values.companyId, name: values.name, description: values.description}));
    }
    
    const handleAddGenre = (values) =>{
        dispatch(addGenre({gameId: gameKey, companyId: values.companyId, genreId: values.genreId}));
    }

    const handleRemoveGenre = (values) =>{
        dispatch(removeGenre({gameId: gameKey, companyId: values.companyId, genreId: values.genreId}));
    }

    const handleAddPlatform = (values) =>{
        dispatch(addPlatform({gameId: gameKey, companyId: values.companyId, platformId: values.platformId}));
    }

    const handleRemovePlatform = (values) =>{
        dispatch(removePlatform({gameId: gameKey, companyId: values.companyId, platformId: values.platformId}));
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
            <AddGame addRow={addRow} setAddRow={setAddRow} handleAdd={handleAdd}/>
            <AddGenre addGenreState={addGenreState} setAddGenreState={setAddGenreState} handleAddGenre={handleAddGenre}/>
            <RemoveGenre removeGenreState={removeGenreState} setRemoveGenreState={setRemoveGenreState} handleAddGenre={handleRemoveGenre}/>
            <AddPlatform addPlatformState={addPlatformState} setAddPlatformState={setAddPlatformState} handleAddPlatform={handleAddPlatform} />
            <RemovePlatform removePlatformState={removePlatformState} setRemovePlatformState={setRemovePlatformState} handleRemovePlatform={handleRemovePlatform} />
        </Layout>
    ); 
}