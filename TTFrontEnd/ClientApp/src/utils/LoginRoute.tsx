import React from 'react'
import { Route } from 'react-router-dom'
//redux stuff

const LoginRoute: React.SFC<any> = ({ component: Component, ...rest }) => (
    <Route
        {...rest}
        render={(props) =>
                <Component {...props} />
        }
    />
);

export default LoginRoute