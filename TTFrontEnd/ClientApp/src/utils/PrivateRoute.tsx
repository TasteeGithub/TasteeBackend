import React from 'react'
import { Route, Redirect } from 'react-router-dom'
import { CheckAuthentication } from './CheckAuthentication';
import MainLayout from '../components/MainLayout';
import Layout from '../components/Layout';
const PrivateRoute: React.SFC<any> = ({ component: Component, ...rest }: any) => (
    <Route
        {...rest}
        render={(props) => CheckAuthentication.IsSigning() ?
            <MainLayout>
                <Component {...props}/>
            </MainLayout>
            : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />}
    />
);
export default PrivateRoute;