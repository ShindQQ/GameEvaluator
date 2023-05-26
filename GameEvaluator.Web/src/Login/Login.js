import { Button, Form, Input, Typography, message } from "antd";
import { useState } from "react";

export function Login(){

    const [formValues, setFormValues] = useState({});
    
    const handleFormChange = (changedValues, allValues) => {
        setFormValues(allValues);
    };

    const login = () =>{
        fetch("/api/Authentication/login", {
            method: "POST",
            headers: {
                'Accept': 'application/json',
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({Email:formValues.email, Password:formValues.password})
        }).then(response => {
            if(response.status === 401)
                throw new Error('Access Denied');

            return response.json();
        }).then(auth => {
            localStorage.setItem('auth', JSON.stringify(auth));
            message.success("Logined!")
            window.location.reload(false);
        }).catch(() => {
            message.error("Email or password are incorrect!");
        });
    }

    return (
        <div style={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            width: '100vw',
            height: '100vh',
            background: 'radial-gradient(circle, rgba(255,240,118,1) 0%, rgba(252,196,98,1) 40%, rgba(255,155,115,1) 75%)'
        }}>
            <Form 
            onValuesChange={handleFormChange}
            style={{
                backgroundColor: 'rgba(255,255,255,0.33)',
                boxShadow: '0px 0px 20px 0px rgba(255,155,115,1)',
                backdropFilter: 'blur(3px)',
                padding: '8px',
                borderRadius:'8px'
            }}>
                <Typography.Title style={{textAlign:'center', color:'rgba(255,155,115,1)'}}>Login</Typography.Title>
                <Form.Item rules={[
                    {
                        required: true,
                        type:'email',
                        message: 'Please enter valid email!'
                    },
                ]}
                label={<label style={{color:'rgba(255,155,115,1)', margin:'0'}}>Email</label>} 
                name={"email"}>
                    <Input  placeholder="Enter your email!" />
                </Form.Item>
                <Form.Item rules={[
                    {
                        required: true,
                        message: 'Please enter your password!'
                    },
                ]}
                label={<label style={{color:'rgba(255,155,115,1)', margin:'0'}}>Password</label>} 
                name={"password"}>
                    <Input placeholder="Enter your password!" />
                </Form.Item>
                <Button type="primary" htmlType="submit" block 
                onClick={login}
                style={{ backgroundColor: "rgba(252,196,98,1)", border:'none'}}>
                    Login
                </Button>
            </Form>
        </div>
    );
}