import { Button, Form, Input, Modal} from "antd";

export const AddCommentToComment = ({addCommentToCommentState, setAddCommentToCommentState, handleAddCommentToComment}) => {
    return (
        <Modal
            open={addCommentToCommentState}
            title="Add Comment to Comment"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setAddCommentToCommentState(false);
            }}
            >
            <Form onFinish={handleAddCommentToComment}>
                <Form.Item name="commentId"
                        rules={[{
                            required:true,
                            message:'Comment ID is required',
                        },
                        {
                            pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                            message: 'Please enter a valid Game ID',
                        }
                        ]}>
                            <div>
                            Comment Id <Input />
                            </div>
                </Form.Item>
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
                <Form.Item name="text"
                        rules={[{
                            required:true,
                            message:'Please enter text',
                        },
                        {
                            min: 1,
                            max: 500,
                            message:'Text must not exceed 500 characters'
                        }
                        ]}>
                            <div>
                                Text <Input />
                            </div>
                </Form.Item>
                <Form.Item>
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setAddCommentToCommentState(false)}>
                        Add
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}