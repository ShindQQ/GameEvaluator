import { useDispatch, useSelector } from "react-redux"
import { addCompany, addWorker, deleteCompany, fetchCompanies, removeGameFromCompany, removeWorker, selectAllCompanies, updateCompany } from "./companiesSlice"
import { useEffect, useState } from "react";
import { Layout } from '../Layout';
import { Button, Form, Input, Modal, Table } from "antd";
import { AppstoreAddOutlined, DeleteOutlined, EditOutlined, SaveOutlined } from "@ant-design/icons";
import { addGame } from "../Games/gamesSlice";
import { AddCompany } from "./AddCompany";
import { AddGame } from "./AddGame";
import { RemoveGame } from "./RemoveGame";
import { AddWorker } from "./AddWorker";
import { RemoveWorker } from "./RemoveWorker";

export const Companies = () => {
    const [editRow, setEditRow] = useState(null);
    const [addRow, setAddRow] = useState(false);
    const [companyKey, setCompanyKey] = useState(null);
    const [addGameState, setAddGameState] = useState(false);
    const [removeGameState, setRemoveGameState] = useState(false);
    const [addWorkerState, setAddWorkerState] = useState(false);
    const [removeWorkerState, setRemoveWorkerState] = useState(false);
    const companies = useSelector(selectAllCompanies);
    const companiesStatus = useSelector(state => state.companies.status);
    const loading = useSelector(state => state.companies.loading);
    const dispatch = useDispatch();
    const [form] = Form.useForm();
    const [rights, setRights] = useState(
        () => {
            var auth =  JSON.parse(localStorage.getItem('auth'));
            if(auth != null && auth.Roles.find(role => role == "SuperAdmin" || role == "Admin") != null)
            {
                return true;
            }

            return false;
        }
    );

    const columns = [
        {
            title: 'Number',
            dataIndex: 'index',
            key: 'number',
            sorter: (a, b) => a.index - b.index
        },
        rights ?
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'id',
        } : {},
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
        rights ? {
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
                                dispatch(deleteCompany(record.key));
                            }}>
                                <DeleteOutlined />
                            </Button>
                        </div>
                        <div style={{display:'flex', gap: '10px', flexDirection:'row', padding: '5px'}}>
                            <Button type='default'
                            onClick={() => { 
                                setCompanyKey(record.key);
                                setAddGameState(true);
                            }}>
                                Add Game
                            </Button>
                            <Button type='default' danger={true}
                            onClick={() => { 
                                setCompanyKey(record.key);
                                setRemoveGameState(true);
                            }}>
                                Remove Game
                            </Button>
                        </div>
                        <div style={{display:'flex', gap: '10px', flexDirection:'row', padding: '5px'}}>
                            <Button type='default' 
                            onClick={() => { 
                                setCompanyKey(record.key);
                                setAddWorkerState(true);
                            }}>
                                Add Worker
                            </Button>
                            <Button type='default' danger={true}
                            onClick={() => { 
                                setCompanyKey(record.key);
                                setRemoveWorkerState(true);
                            }}>
                                Remove Worker
                            </Button>
                        </div>
                    </> 
                )
            }
        }: {}
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

        dispatch(fetchCompanies(tableParams));
    }

    useEffect(() => {
        if(companiesStatus === 'idle')
        {
            fetchData();
        }
    }, [companiesStatus, dispatch]);

    const onFinish = (values) => {
        dispatch(updateCompany({Id: editRow, name: values.name, description: values.description}));
        setEditRow(null);
    }

    const onAdd = () =>{
        setAddRow(true);
    }

    const handleAdd = (values) =>{
        dispatch(addCompany({name: values.name, description: values.description}));
    }

    const handleAddGame = (values) =>{
        dispatch(addGame({companyId: companyKey, name: values.name, description: values.description}));
    }

    const handleRemoveGame = (values) =>{
        dispatch(removeGameFromCompany({companyId: companyKey, gameId: values.gameId}));
    }

    const handleAddWorker = (values) =>{
        dispatch(addWorker({companyId: companyKey, workerId: values.workerId}));
    }

    const handleRemoveWorker = (values) =>{
        dispatch(removeWorker({companyId: companyKey, workerId: values.workerId}));
    }

    if(companiesStatus === 'succeeded')
    return (
        <Layout>
            {rights ? <Button onClick={onAdd} type="primary" htmlType="submit">
                <AppstoreAddOutlined /> Add Company
            </Button> : ''}
            <Form form={form} onFinish={onFinish}>
                <Table dataSource={companies.Items.map((company, index) => {
                    return {
                        key: company.Id,
                        id: company.Id,
                        index: index + (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                        name: company.Name, 
                        description: company.Description,
                        games: company.Games.length !== 0 ? company.Games.map(game => game.Name).join(" ") : "Company has no games",
                        workers: company.Workers.length !== 0 ? company.Workers.map(worker => worker.Name).join(" ") : "Company has no workers",
                    }
                })}
                scroll={{
                    x: 1500,
                  }}
                pagination={tableParams.pagination}
                loading={loading}
                columns={columns}
                onChange={fetchData}>
                </Table>
            </Form>
            <AddCompany addRow={addRow} setAddRow={setAddRow} handleAdd={handleAdd} />
            <AddGame addGameState={addGameState} setAddGameState={setAddGameState} handleAddGame={handleAddGame} />
            <RemoveGame removeGameState={removeGameState} setRemoveGameState={setRemoveGameState} handleRemoveGame={handleRemoveGame} />
            <AddWorker addWorkerState={addWorkerState} setAddWorkerState={setAddWorkerState} handleAddWorker={handleAddWorker} />
            <RemoveWorker removeWorkerState={removeWorkerState} setRemoveWorkerState={setRemoveWorkerState} handleRemoveWorker={handleRemoveWorker} />
        </Layout>
    )
}