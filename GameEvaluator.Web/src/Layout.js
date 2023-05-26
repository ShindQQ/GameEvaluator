import React, { Component } from 'react';
import { Menu } from 'antd';
import { HomeOutlined, FundOutlined, TeamOutlined, BugOutlined, FireOutlined, GlobalOutlined } from '@ant-design/icons';
import { useNavigate } from 'react-router-dom';

export class Layout extends Component {
    static displayName = Layout.name;

    render() {
        return (
            <div style={{
                display: 'flex',
                flexDirection: 'column',
                flex: 1,
                height: '100vh'
            }}>
                <Header />
                <div style={{display: "flex", flexDirection: "row", flex:1}}>
                    <RenderMenu />
                    <div style={{width:'100%'}}>
                        {this.props.children}
                    </div>
                </div>
                <Footer />
            </div>
        );
    }
}

function Header(){
    return <div style={{
        height: 40,
        background: 'linear-gradient(360deg, rgba(255,240,118,1) 0%, rgba(252,196,98,1) 75%)',
        color: 'white',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        fontWeight: 'bold',
        fontSize: 24,
        marginBottom: '10px'
    }}>Header</div>
}

function Footer(){
    return <div style={{
        height: 40,
        background: 'linear-gradient(180deg, rgba(255,240,118,1) 0%, rgba(252,196,98,1) 75%)',
        color: 'white',
        display: 'flex',
        justifyContent: 'center',
        alignItems: 'center',
        fontWeight: 'bold',
        fontSize: 24
    }}>Footer</div>
}

function RenderMenu(){
    const navigate = useNavigate();
    
    return (
        <Menu
        onClick={({key})=>{
            navigate(key);
        }}
        style={{minWidth:'10%', fontSize:'18px'}}
        items={[
            { label:"Home", key:'/', icon:<HomeOutlined /> },
            { label:"Companies", key:'/companies', icon:<FundOutlined /> },
            { label:"Games", key:'/games', icon:<BugOutlined /> },
            { label:"Genres", key:'/genres', icon:<FireOutlined /> },
            { label:"Platforms", key:'/platforms', icon:<GlobalOutlined /> },
            { label:"Users", key:'/users', icon:<TeamOutlined /> },
        ]}>
        </Menu>
    );
}