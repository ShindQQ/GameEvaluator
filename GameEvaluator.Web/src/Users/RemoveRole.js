import { Button, Form, Select, Modal} from "antd";

export const RemoveRole = ({removeRoleState, setRemoveRoleState, handleRemoveRole, setRole}) => {
    return (
        <Modal
            open={removeRoleState}
            title="Remove Role"
            okText="Role"
            footer={[]}
            onCancel={() => {
                setRemoveRoleState(false);
            }}
            >
            <Form onFinish={handleRemoveRole}>
                <Form.Item name="role">
                    <Select 
                    onChange={(value) => setRole(value)}
                    options = {[
                        {
                            label: 'User',
                            value: 'User'
                        },
                        {
                            label: 'Admin',
                            value: 'Admin'
                        },
                        {
                            label: 'Company',
                            value: 'Company'
                        },
                        {
                            label: 'SuperAdmin',
                            value: 'SuperAdmin'
                        },
                    ]}
                     />
                </Form.Item>
                <Form.Item>
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setRemoveRoleState(false)}>
                        Remove
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}