import React, { Component } from 'react';
import { Table } from 'antd';

export default class App extends Component {
    static displayName = App.name;

    static columns = [
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
            }
            
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

    constructor(props) {
        super(props);
        this.state = { games: [], loading: true };
    }

    componentDidMount() {
        this.populateGamesData();
    }

    static renderGames(games) {
        return (
            <Table dataSource={games.map((game, index) => {return {
                index: index + 1,
                name: game.Name,
                description: game.Description,
                averageRating: game.AverageRating,
                genres: game.Genres,
                companies: game.CompaniesNames,
                platforms: game.Platforms
            }})} columns={this.columns}> </Table>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading... </em></p>
            : App.renderGames(this.state.games);

        return (
            <div>
                {contents}
            </div>
        );
    }

    async populateGamesData() {
        const response = await fetch('/api/games/1/10');
        const data = await response.json();
        this.setState({ games: data.Items, loading: false });
    }
}
