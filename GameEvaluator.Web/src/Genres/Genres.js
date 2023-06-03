import { useDispatch, useSelector } from "react-redux"
import { addGenre, deleteGenre, fetchGenres, selectAllGenres, updateGenre } from "./genresSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Button, Form, Input, Modal, Table, message } from "antd";
import { AppstoreAddOutlined, DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { AddGenre } from "./AddGenre";

export const Genres = () => {
    const [editRow, setEditRow] = useState(null);
    const [addRow, setAddRow] = useState(false);
    const genres = useSelector(selectAllGenres);
    const genresStatus = useSelector(state => state.genres.status);
    const loading = useSelector(state => state.genres.loading);
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
                                    description: record.description
                                });
                            }}>
                                <EditOutlined />
                            </Button>
                            <Button type='text' htmlType="submit">
                                <SaveOutlined />
                            </Button>
                        </div>
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

    const onFinish = (values) => {
        dispatch(updateGenre({Id: editRow, name: values.name, description: values.description}));
        setEditRow(null);
    }

    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = (values) =>{
        dispatch(addGenre({name: values.name, description: values.description}));
    }

    if(genresStatus === 'succeeded') 
    return (
        <Layout>
            <Button onClick={onAdd} type="primary" htmlType="submit">
                <AppstoreAddOutlined /> Add Genre
            </Button>
            <Form form={form} onFinish={onFinish}>
                <Table
                dataSource={genres.Items.map((genre, index) => {
                    return {
                        key: genre.Id,
                        id: genre.Id,
                        index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                        name: genre.Name,
                        description: genre.Description,
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
            <AddGenre addRow={addRow} setAddRow={setAddRow} handleAdd={handleAdd}/>
        </Layout>
    ); 
}