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
import Accounts from './components/AccountManager/Accounts';
import CreateAccount from './components/AccountManager/CreateAccount';
import EditAccount from './components/AccountManager/EditAccount';
import GuestRoute from './utils/GuestRoute';
import LoginRoute from './utils/LoginRoute';
import Dashboard from './components/Dashboard';

type AppProps = AuthenticatedState & typeof actionCreator;

const App: React.FunctionComponent<AppProps> = (props: AppProps) => {
    return (
        <Router>
            <Switch>
                <GuestRoute exact path="/" component={Dashboard} />
                <LoginRoute path="/login" component={Login} />
                <PrivateRoute component={Accounts} path='/accounts/:startDateIndex?' />
                <GuestRoute path="/create-account" component={CreateAccount} />
                <GuestRoute path="/edit-account/:id?" component={EditAccount} />
                <GuestRoute component={FetchData} path='/fetch-data/:startDateIndex?' />
                <PrivateRoute path="/counter" component={Counter} />
            </Switch>
        </Router>
    );
};

export default connect((state: ApplicationState) => state.Authentication, actionCreator)(App);