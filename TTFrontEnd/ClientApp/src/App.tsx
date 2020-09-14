import * as React from 'react';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect } from 'react-redux'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import './custom.css'
import Login from './components/Authentication/Login';
import { AuthenticatedState, actionCreator } from './store/Login';
import { ApplicationState } from './store';
import PrivateRoute from './utils/PrivateRoute';
import Accounts from './components/OperatorManager/Accounts';
import CreateAccount from './components/OperatorManager/CreateAccount';
import EditAccount from './components/OperatorManager/EditAccount';
import GuestRoute from './utils/GuestRoute';
import LoginRoute from './utils/LoginRoute';
import Dashboard from './components/Dashboard';
import ChangePassword from './components/OperatorManager/ChangePassword';
import CustomLayout from './components/MerchantLayout/CustomLayout';
import Users from './components/UserManager/Users';
import EditUser from './components/UserManager/EditUser';
import Brands from './components/BrandManager/Brands';
import CreateBrand from './components/BrandManager/CreateBrand';
import FrontEnd from './components/FrontEnd';

type AppProps = AuthenticatedState & typeof actionCreator;

const App: React.FunctionComponent<AppProps> = (props: AppProps) => {
    return (
        <Router>
            <Switch>
                <GuestRoute exact path="/" component={FrontEnd} />
                <PrivateRoute path="/dashboard" component={Dashboard} />
                <LoginRoute path="/login" component={Login} />
                <PrivateRoute path="/change-password" component={ChangePassword} />

                //Operator
                <PrivateRoute component={Accounts} path='/accounts/:startDateIndex?' />
                <PrivateRoute path="/create-account" component={CreateAccount} />
                <PrivateRoute path="/edit-account/:id?" component={EditAccount} />


                <GuestRoute component={FetchData} path='/fetch-data/:startDateIndex?' />
                <PrivateRoute path="/counter" component={Counter} />
                
                <PrivateRoute path="/layout" component={CustomLayout} />

                <PrivateRoute component={Users} path='/users/:startDateIndex?' />
                <PrivateRoute path="/edit-user/:id?" component={EditUser} />

                <PrivateRoute component={Brands} path='/brands' />
                <PrivateRoute path="/create-brand" component={CreateBrand} />

            </Switch>
        </Router>
    );
};

export default connect((state: ApplicationState) => state.Authentication, actionCreator)(App);