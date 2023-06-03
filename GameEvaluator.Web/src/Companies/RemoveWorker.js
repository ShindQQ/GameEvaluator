import { Button, Form, Input, Modal} from "antd";

export const RemoveWorker = ({removeWorkerState, setRemoveWorkerState, handleRemoveWorker}) => {
    return (
        <Modal
            open={removeWorkerState}
            title="Remove Worker"
            okText="Remove"
            footer={[]}
            onCancel={() => {
                setRemoveWorkerState(false);
            }}
            >
            <Form onFinish={handleRemoveWorker}>
                <Form.Item name="workerId"
                        rules={[{
                            required:true,
                            message:'Worker ID is required',
                        },
                        {
                            pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                            message: 'Please enter a valid Worker ID',
                        }
                        ]}>
                            <div>
                            Worker Id <Input />
                            </div>
                </Form.Item>
                <Form.Item>
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setRemoveWorkerState(false)}>
                        Remove
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}