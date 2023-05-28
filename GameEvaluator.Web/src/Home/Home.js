import Typography from 'antd/es/typography/Typography';
import {Layout} from '../Layout';
import Title from 'antd/es/typography/Title';
import Paragraph from 'antd/es/typography/Paragraph';
import Link from 'antd/es/typography/Link';
import { Divider } from 'antd';

export const Home = () => {
    return (
        <Layout>
            <Typography>
                <Title level={2}>
                    Website for GameEvaluatorAPI
                </Title>
                <Paragraph>
                    Using this website you can manipulate data in database, send requests to API.
                </Paragraph>
                <Paragraph>
                    You can manipulate next data:
                    <ul>
                        <li>
                            <Link href="/companies">Companies</Link>
                        </li>
                        <li>
                            <Link href="/games">Games</Link>
                        </li>
                        <li>
                            <Link href="/genres">Genres</Link>
                        </li>
                        <li>
                            <Link href="/platforms">Platforms</Link>
                        </li>
                        <li>
                            <Link href="/users">Users</Link>
                        </li>
                    </ul>
                </Paragraph>
                <Divider />
            </Typography>
        </Layout>
    );
}