import { Form, Modal, Table} from "antd";
import { useEffect, useState } from "react";
import { fetchComments, selectAllComments } from "./gamesSlice";
import { useDispatch, useSelector } from "react-redux";

export const GameComments = ({gameKey, commentsState, setCommentsState}) => {
    const [gameId, setGameId] = useState(null);
    const comments = useSelector(selectAllComments);
    const commentsStatus = useSelector(state => state.games.commentsStatus);
    const loading = useSelector(state => state.games.loading);
    const dispatch = useDispatch();

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
        
        dispatch(fetchComments({gameId: gameKey, tableParams:tParams}));
    }
    
    useEffect(() => {
        if(commentsStatus === 'idle' || gameId != gameKey)
        {
            setGameId(gameKey);
            fetchData();
        }
    }, [commentsStatus, dispatch, setTableParams, gameId]);

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
                <Table dataSource={comments.Items.map((comment, index) => {
                    return {
                        key: comment.Id,
                        id: comment.Id,
                        index: index - (tableParams.pagination.current - 1) * tableParams.pagination.pageSize  + 1,
                        name: comment.Name,
                        text: comment.Text,
                        gameId: comment.GameId,
                        parentCommentId: comment.ParentCommentId,
                        leftBy: comment.LeftBy,
                    }})} 
                    scroll={{
                        x: 1500,
                        y: 500
                      }}
                    pagination={tableParams.pagination}
                    loading={loading}
                    columns={ [
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
                    ]}
                    onChange={fetchData}> 
                </Table>
            </Form>
        </Modal>
    );
}