import { Button, Form, Input, Modal} from "antd";

export const AddWorker = ({addWorkerState, setAddWorkerState, handleAddWorker}) => {
    return (
        <Modal
            open={addWorkerState}
            title="Add Worker"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setAddWorkerState(false);
            }}
            >
            <Form onFinish={handleAddWorker}>
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
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setAddWorkerState(false)}>
                        Add
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}