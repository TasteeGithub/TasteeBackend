import React from 'react'
import { Route } from 'react-router-dom'
import MainLayout from '../components/MainLayout';
import Layout from '../components/Layout';
//redux stuff

const GuestRoute: React.SFC<any> = ({ component: Component, ...rest }) => (
    <Route
        {...rest}
        render={(props) =>
            <MainLayout>
                <Component {...props} />
            </MainLayout>
        }
    />
);

export default GuestRoute