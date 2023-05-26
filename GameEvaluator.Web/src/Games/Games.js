import { useEffect, useState } from "react";
import { Table } from 'antd';
import { fetchGames, selectAllGames } from "./gamesSlice";
import { useSelector } from "react-redux";
import { useDispatch } from "react-redux";

export const Games = () =>
{
    const games = useSelector(selectAllGames);
    const gamesStatus = useSelector(state => state.games.status);
    const loading = useSelector(state => state.games.loading);
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
            title: 'Average Rating',
            key: 'averageRating',
            render: payload => {
                return <p>{!payload.AverageRating && payload.averageRating != 0 ? "No users" : payload.averageRating}</p>
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

    if(gamesStatus === 'succeeded') 
    return (
        <Table dataSource={games.Items.map((game, index) => {
            return {
            key: game.Id,
            index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
            name: game.Name,
            description: game.Description,
            averageRating: game.AverageRating,
            genres: game.Genres,
            companies: game.CompaniesNames,
            platforms: game.Platforms
        }})} 
        pagination={tableParams.pagination}
        loading={loading}
        columns={columns}
        onChange={fetchData}> </Table>
    ); 
}