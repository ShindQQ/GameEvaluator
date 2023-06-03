import { Button, Form, Input, Modal} from "antd";

export const RemoveGenre = ({removeGenreState, setRemoveGenreState, handleRemoveGenre}) => {
    return (
        <Modal
            open={removeGenreState}
            title="Remove Genre"
            okText="Remove"
            footer={[]}
            onCancel={() => {
                setRemoveGenreState(false);
            }}
            >
            <Form onFinish={handleRemoveGenre}>
                <Form.Item name="genreId"
                        rules={[{
                            required:true,
                            message:'Genre ID is required',
                        },
                        {
                            pattern: /[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}/,
                            message: 'Please enter a valid Genre ID',
                        }
                        ]}>
                            <div>
                                Genre Id <Input />
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
                    <Button key="submit" htmlType="submit" type="primary" block onClick={() => setRemoveGenreState(false)}>
                        Remove
                    </Button> 
                </Form.Item>
            </Form>
        </Modal>
    );
}