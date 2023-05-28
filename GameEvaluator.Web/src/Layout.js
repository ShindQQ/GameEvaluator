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
                <div style={{display: "flex", flexDirection: "row", flex:1, margin:'10px 0px'}}>
                    <RenderMenu />
                    <div style={{width:'100%', padding: '0px 10px'}}>
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
        background: 'rgba(252,196,100,1)',
        boxShadow: '0 1px 3px 0 grey',
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
        background: 'rgba(252,196,100,1)',
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