import { useDispatch, useSelector } from "react-redux"
import { addCompany, deleteCompany, fetchCompanies, selectAllCompanies, updateCompany } from "./companiesSlice"
import { useEffect, useState } from "react";
import { Layout } from '../Layout';
import { Button, Form, Input, Modal, Table } from "antd";
import { AppstoreAddOutlined, DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { useNavigate } from "react-router-dom";
import { addGame } from "../Games/gamesSlice";

export const Companies = () => {
    const [editRow, setEditRow] = useState(null);
    const [addRow, setAddRow] = useState(false);
    const [addGameKey, setAddGameKey] = useState(null);
    const [addGameState, setAddGameState] = useState(false);
    const companies = useSelector(selectAllCompanies);
    const companiesStatus = useSelector(state => state.companies.status);
    const loading = useSelector(state => state.companies.loading);
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
            title: 'Games',
            dataIndex: 'games',
            key: 'games'
        },
        {
            title: 'Workers',
            dataIndex: 'workers',
            key: 'workers'
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
                        onClick={async () => { 
                            const response = await dispatch(deleteCompany(record.key));
                            if(response.payload === true)
                                navigate(0);
                            }}>
                            <DeleteOutlined />
                        </Button>
                        <Button type='text' 
                        onClick={() => { 
                            setAddGameKey(record.key);
                            setAddGameState(true);
                            }}>
                            Add Game
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
                total: companies.TotalCount
            }
        });
    }

    useEffect(() => {
        if(companiesStatus === 'idle')
        {
            dispatch(fetchCompanies(tableParams));
            fetchData();
        }
    }, [companiesStatus, dispatch]);

    const onFinish = async (values) => {
        const response = await dispatch(updateCompany({Id: editRow, name: values.name, description: values.description}));
        if(response.payload === true)
            navigate(0);
        setEditRow(null);
    }

    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = async (values) =>{
        const response = await dispatch(addCompany({name: values.name, description: values.description}));
        if(response.payload === true)
            navigate(0);
    }

    const handleAddGame = async (values) =>{
        const response = await dispatch(addGame({companyId: addGameKey, name: values.name, description: values.description}));
        if(response.payload === true)
            navigate(0);
    }

    if(companiesStatus === 'succeeded')
    return (
        <Layout>
            <Button onClick={onAdd} type="primary" htmlType="submit">
                <AppstoreAddOutlined /> Add Company
            </Button>
            <Form form={form} onFinish={onFinish}>
                <Table dataSource={companies.Items.map((company, index) => {
                    return {
                        key: company.Id,
                        id: company.Id,
                        index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                        name: company.Name, 
                        description: company.Description,
                        games: company.Games.length !== 0 ? company.Games.map(game => game.Name).join(" ") : "Company has no games",
                        workers: company.Workers.length !== 0 ? company.Workers.map(worker => worker.Name).join(" ") : "Company has no workers",
                    }
                })}
                pagination={tableParams.pagination}
                loading={loading}
                columns={columns}
                onChange={fetchData}>
                </Table>
            </Form>
            <Modal
            open={addRow}
            title="Add Company"
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
        </Layout>
    )
}