import { useDispatch, useSelector } from "react-redux"
import { addPlatform, deletePlatform, fetchPlatforms, selectAllPlatofrms, updatePlatform } from "./platformsSlice"
import { useEffect, useState } from "react";
import { Layout } from "../Layout";
import { Button, Form, Input, Modal, Table } from "antd";
import { AppstoreAddOutlined, DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { AddPlatform } from "./AddPlatform";

export const Platforms = () => {
    const [editRow, setEditRow] = useState(null);
    const [addRow, setAddRow] = useState(false);
    const platforms = useSelector(selectAllPlatofrms);
    const platformsStatus = useSelector(state => state.platforms.status);
    const loading = useSelector(state => state.platforms.loading);
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
                            dispatch(deletePlatform(record.key));
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
                total: platforms.TotalCount
            }
        });
        
        dispatch(fetchPlatforms(tParams));
    }

    useEffect(() => {
        if(platformsStatus === 'idle')
        {
            fetchData();
        }
    }, [platformsStatus, dispatch]);

    const onFinish = (values) => {
        dispatch(updatePlatform({Id: editRow, name: values.name, description: values.description}));
        setEditRow(null);
    }

    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = (values) =>{
        dispatch(addPlatform({name: values.name, description: values.description}));
    }

    if(platformsStatus === 'succeeded') 
    return (
        <Layout>
            <Button onClick={onAdd} type="primary" htmlType="submit">
                <AppstoreAddOutlined /> Add Platform
            </Button>
            <Form form={form} onFinish={onFinish}>
                <Table dataSource={platforms.Items.map((platform, index) => {
                    return {
                        key: platform.Id,
                        id: platform.Id,
                        index: index + (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                        name: platform.Name,
                        description: platform.Description,
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
            <AddPlatform addRow={addRow} setAddRow={setAddRow} handleAdd={handleAdd}/>
        </Layout>
    ); 
}