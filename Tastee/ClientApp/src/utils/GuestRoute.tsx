import React from 'react'
import { Route } from 'react-router-dom'
import MainLayout from '../components/MainLayout';
import FrontLayout from '../components/FrontLayout';
//redux stuff

const GuestRoute: React.SFC<any> = ({ component: Component, ...rest }) => (
    <Route
        {...rest}
        render={(props) =>
            <FrontLayout>
                <Component {...props} />
            </FrontLayout>
        }
    />
);

export default GuestRoute