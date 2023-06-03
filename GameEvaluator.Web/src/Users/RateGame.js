import { Button, Form, Input, InputNumber, Modal} from "antd";

export const RateGame = ({ratingState, setRatingState, handleRating, setRating}) => {
    return (
        <Modal
            open={ratingState}
            title="Rate Game"
            okText="Rate"
            footer={[]}
            onCancel={() => {
                setRatingState(false);
            }}
            >
            <Form onFinish={handleRating}>
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
                <Form.Item name="rating">
                            <div>
                                Rating <InputNumber min={1} max={10} defaultValue={5} onChange={(value) => setRating(value)}/>
                            </div>
                </Form.Item>
                <Form.Item>
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setRatingState(false)}>
                        Rate
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}