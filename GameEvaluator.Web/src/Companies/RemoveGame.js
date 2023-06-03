import { Button, Form, Input, Modal} from "antd";

export const RemoveGame = ({removeGameState, setRemoveGameState, handleRemoveGame}) => {
    return (
        <Modal
            open={removeGameState}
            title="Remove Game"
            okText="Remove"
            footer={[]}
            onCancel={() => {
                setRemoveGameState(false);
            }}
            >
                <Form onFinish={handleRemoveGame}>
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
                        <Button key="submit" htmlType="submit" type="primary" block onClick={() => setRemoveGameState(false)}>
                            Remove
                        </Button> 
                    </Form.Item>
                </Form>
        </Modal>
    );
}