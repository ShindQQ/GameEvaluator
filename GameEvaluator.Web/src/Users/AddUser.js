import { Button, Form, Input, Modal} from "antd";

export const AddUser = ({addRow, setAddRow, handleAdd}) => {
    return (
        <Modal
            open={addRow}
            title="Add User"
            okText="Add"
            footer={[]}
            onCancel={() => {
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
                <Form.Item name="email"
                    rules={[
                        {
                            type: 'email',
                            message: 'The input is not valid E-mail!',
                        },{
                            required:true,
                            message:'Please enter email',
                        },
                        {
                            min: 10,
                            max: 20,
                            message:'Email should have from 10 to 20 characters'
                        }
                        ]}>
                            <div>
                                Email <Input />
                            </div>
                </Form.Item>
                <Form.Item name="password"
                        rules={[{
                            required:true,
                            message:'Please enter password',
                        },
                        {
                            min: 6,
                            max: 10,
                            message:'Password should have from 6 to 10 characters'
                        }
                        ]}>
                            <div>
                                Password <Input />
                            </div>
                </Form.Item>
                <Form.Item>
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setAddRow(false)}>
                        Add
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}