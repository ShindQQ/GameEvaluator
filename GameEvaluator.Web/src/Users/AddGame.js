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
                <Form.Item name="gameId"
                        rules={[{
                            required:true,
                            message:'Game ID is required',
                        },
                        {
                            pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                            message: 'Please enter a valid Game ID',
                        }
                        ]}>
                            <div>
                                Game Id <Input />
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