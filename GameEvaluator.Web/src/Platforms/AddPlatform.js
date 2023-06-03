import { Button, Form, Input, Modal} from "antd";

export const AddPlatform = ({addRow, setAddRow, handleAdd}) => {
    return (
        <Modal
            open={addRow}
            title="Add Platform"
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
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setAddRow(false)}>
                        Add
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}