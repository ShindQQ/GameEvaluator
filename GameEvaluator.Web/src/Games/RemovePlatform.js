import { Button, Form, Input, Modal} from "antd";

export const RemovePlatform = ({removePlatformState, setRemovePlatformState, handleRemovePlatform}) => {
    return (
        <Modal
            open={removePlatformState}
            title="Remove Platform"
            okText="Remove"
            footer={[]}
            onCancel={() => {
                setRemovePlatformState(false);
            }}
            >
            <Form onFinish={handleRemovePlatform}>
                <Form.Item name="platformId"
                        rules={[{
                            required:true,
                            message:'Platform ID is required',
                        },
                        {
                            pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                            message: 'Please enter a valid Platform ID',
                        }
                        ]}>
                            <div>
                                Platform Id <Input />
                            </div>
                </Form.Item>
                <Form.Item name="companyId"
                        rules={[{
                            required:true,
                            message:'Company ID is required',
                        },
                        {
                            pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                            message: 'Please enter a valid Company ID',
                        }
                        ]}>
                            <div>
                                Company Id <Input />
                            </div>
                </Form.Item>
                <Form.Item>
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setRemovePlatformState(false)}>
                        Remove
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}