import { useDispatch, useSelector } from "react-redux"
import { deleteCompany, fetchCompanies, selectAllCompanies } from "./companiesSlice"
import { useEffect, useState } from "react";
import { Layout } from '../Layout';
import { Button, Table } from "antd";
import { DeleteOutlined } from "@ant-design/icons";

export const Companies = () => {
    const companies = useSelector(selectAllCompanies);
    const companiesStatus = useSelector(state => state.companies.status);
    const loading = useSelector(state => state.companies.loading);
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
            title: 'Description',
            dataIndex: 'description',
            key: 'description'
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
                    <Button type='text' danger={true}
                    onClick={() => { 
                        dispatch(deleteCompany(record.key));
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

    if(companiesStatus === 'succeeded')
    return (
        <Layout>
            <Table dataSource={companies.Items.map((company, index) => {
                return {
                    key: company.Id,
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
        </Layout>
    )
}