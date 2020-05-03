import React from 'react'
import { Route,Redirect,RouteProps } from 'react-router-dom'
import { CheckAuthentication } from './CheckAuthentication';

const PrivateRoute: React.SFC<any> = ({ component: Component,...rest }: any) => (
    
    <Route
        {...rest}
        render={(props) => CheckAuthentication.isAuthenticated ? <Component {...props} />
            : <Redirect to={{ pathname: '/login', state: { from: props.location } }} />}
    />
);
export default PrivateRoute;