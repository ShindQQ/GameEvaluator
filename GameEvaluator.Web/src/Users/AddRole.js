import { Button, Form, Select, Modal} from "antd";

export const AddRole = ({addRoleState, setAddRoleState, handleAddRole, setRole}) => {
    return (
        <Modal
            open={addRoleState}
            title="Add Role"
            okText="Role"
            footer={[]}
            onCancel={() => {
                setAddRoleState(false);
            }}
            >
            <Form onFinish={handleAddRole}>
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
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setAddRoleState(false)}>
                        Add
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}