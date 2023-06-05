import { Form, Modal, Table} from "antd";
import { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { fetchComments, selectAllComments } from "./usersSlice";
export const UserComments = ({userKey, commentsState, setCommentsState}) => {
    const [userId, setUserId] = useState(null);
    const comments = useSelector(selectAllComments);
    const commentsStatus = useSelector(state => state.users.commentsStatus);
    const loading = useSelector(state => state.users.loading);
    const dispatch = useDispatch();

    const columns= [
        {
            title: 'Number',
            dataIndex: 'index',
            key: 'number',
            sorter: (a, b) => a.index - b.index
        },
        {
            title: 'Id',
            dataIndex: 'id',
            key: 'id',
        },
        {
            title: 'Text',
            dataIndex: 'text',
            key: 'text',
        },
        {
            title: 'GameId',
            dataIndex: 'gameId',
            key: 'gameId',
        },
        {
            title: 'ParentCommentId',
            dataIndex: 'parentCommentId',
            key: 'parentCommentId',
        },
        {
            title: 'LeftBy',
            dataIndex: 'leftBy',
            key: 'leftBy',
        },
    ];

    const [tableParams, setTableParams] = useState({
        pagination:{
            current: 1,
            pageSize: 5,
            showSizeChanger: true, 
            pageSizeOptions: ['2', '5','10', '20', '30'],
            total: 5
        },
    });
    
    const fetchData = (params) => {
        var tParams;
        if(params != null)
        {
            tParams = {
                pagination: {
                    current: params.current,
                    pageSize: params.pageSize,
                },
            };
            setTableParams(tParams);
        }
        else
        {
            tParams = tableParams;
        }
        
        setTableParams({
            ...tableParams,
            pagination: {
                current: tParams.pagination.current,
                pageSize: tParams.pagination.pageSize,
                showSizeChanger: true, 
                pageSizeOptions: ['2', '5','10', '20', '30'],
                total: comments.TotalCount
            }
        });
        
        dispatch(fetchComments({userId: userKey, tableParams:tParams}));
    }
    
    useEffect(() => {
        if(commentsStatus === 'idle' || userId != userKey)
        {
            setUserId(userKey);
            fetchData();
        }
    }, [commentsStatus, dispatch, setTableParams, userId]);

    const expandedRowRender = (record) => {
        return <Table columns={columns} 
        expandable={{
            expandedRowRender,
            defaultExpandedRowKeys: ['0'],
          }}
        dataSource={record.childrenComments.map((comment, index) => {
            return {
                key: comment.Id,
                id: comment.Id,
                index: index + (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                name: comment.Name,
                text: comment.Text,
                gameId: comment.GameId,
                parentCommentId: comment.ParentCommentId,
                leftBy: comment.LeftBy,
                childrenComments: comment.ChildrenComments
            }})} pagination={false} />;
      };

    if(commentsStatus === 'succeeded') 
    return (
        <Modal
            width={'70vw'}
            open={commentsState}
            title="Show Comments"
            okText="Add"
            footer={[]}
            onCancel={() => {
                setCommentsState(false);
            }}
            >
            <Form>
                <Table 
                expandable={{
                    expandedRowRender,
                    defaultExpandedRowKeys: ['0'],
                  }}
                dataSource={comments.Items.map((comment, index) => {
                    return {
                        key: comment.Id,
                        id: comment.Id,
                        index: index + (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                        name: comment.Name,
                        text: comment.Text,
                        gameId: comment.GameId,
                        parentCommentId: comment.ParentCommentId,
                        leftBy: comment.LeftBy,
                        childrenComments: comment.ChildrenComments
                    }})} 
                    scroll={{
                        x: 1500,
                        y: 500
                      }}
                    pagination={tableParams.pagination}
                    loading={loading}
                    columns={columns}
                    onChange={fetchData}> 
                </Table>
            </Form>
        </Modal>
    );
}