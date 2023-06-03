import { Button, Form, Input, Modal} from "antd";

export const FavorGame = ({favorState, setFavorState, handleFavor}) => {
    return (
        <Modal
            open={favorState}
            title="Favor Game"
            okText="Favor"
            footer={[]}
            onCancel={() => {
                setFavorState(false);
            }}
            >
                <Form onFinish={handleFavor}>
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
                        <Button key="submit" htmlType="submit" type="primary" block onClick={() => setFavorState(false)}>
                            Favor
                        </Button> 
                    </Form.Item>
                </Form>
            </Modal>
    );
}