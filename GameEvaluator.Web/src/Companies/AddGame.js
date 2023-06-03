import { Button, Form, Input, Modal} from "antd";

export const AddGame = ({addGameState, setAddGameState, handleAddGame}) => {
    return (
        <Modal
            open={addGameState}
            title="Add Game"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setAddGameState(false);
            }}
            >
            <Form onFinish={handleAddGame}>
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
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setAddGameState(false)}>
                        Add
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}