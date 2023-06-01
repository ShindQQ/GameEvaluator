import React, { Component, useEffect, useState } from 'react';
import { Menu } from 'antd';
import { HomeOutlined, FundOutlined, TeamOutlined, BugOutlined, FireOutlined, GlobalOutlined, PoweroffOutlined } from '@ant-design/icons';
import { Link, useLocation, useNavigate } from 'react-router-dom';

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
                    <div style={{width:'90vw', padding: '0px 10px'}}>
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
    const location = useLocation();
    const [current, setCurrent] = useState(location.pathname === '/' ? '/' : location.pathname);

    useEffect(() => {
        if(location){
            if(current !== location.pathname){
                setCurrent(location.pathname)
            }
        }
    }, [location, current])

    const onClick = (e) => {
        if(e.key=='signout') {
            localStorage.removeItem("auth");
            localStorage.removeItem("isAuthenticated");
            navigate('/login');
        }
        else {
            setCurrent(e.key);
        }
    }

    return (
        <Menu
        onClick={onClick}
            selectedKeys={[current]}
            style={{minWidth:'10%', fontSize:'18px'}}
            items={[
                { label: <Link to='/'>Home</Link>, key:'/', icon:<HomeOutlined /> },
                { label: <Link to='/companies'>Companies</Link>, key:'/companies', icon:<FundOutlined /> },
                { label: <Link to='/games'>Games</Link>, key:'/games', icon:<BugOutlined /> },
                { label: <Link to='/genres'>Genres</Link>, key:'/genres', icon:<FireOutlined /> },
                { label: <Link to='/platforms'>Platforms</Link>, key:'/platforms', icon:<GlobalOutlined /> },
                { label: <Link to='/users'>Users</Link>, key:'/users', icon:<TeamOutlined /> },
                { label: "Signout", key: 'signout', danger:true, icon:<PoweroffOutlined /> }
            ]}>
        </Menu>
    );
}