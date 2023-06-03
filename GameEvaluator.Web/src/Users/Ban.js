import { Button, Form, DatePicker, Modal} from "antd";

export const Ban = ({banState, setBanState, handleBan}) => {
    return (
        <Modal
            open={banState}
            title="Ban"
            okText="Ban"
            footer={[]}
            onCancel={() => {
                setBanState(false);
            }}
            >
            <Form onFinish={handleBan}>
                <Form.Item name="date">
                            <div>
                                Game Id <DatePicker format="YYYY-MM-DDTHH:mm:ss.SSSZ" />
                            </div>
                </Form.Item>
                <Form.Item>
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setBanState(false)}>
                        Ban
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}