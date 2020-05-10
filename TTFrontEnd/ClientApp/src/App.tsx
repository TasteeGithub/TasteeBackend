import * as React from 'react';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect } from 'react-redux'
import { BrowserRouter as Router, Route, Switch } from 'react-router-dom';
import './custom.css'
import Login from './components/Authentication/Login';
import { AuthenticatedState, actionCreator } from './store/Login';
import { ApplicationState } from './store';
import MainLayout from './components/MainLayout';
import PrivateRoute from './utils/PrivateRoute';
import Accounts from './components/AccountManager/Accounts';
import CreateAccount from './components/AccountManager/CreateAccount';

type AppProps = AuthenticatedState & typeof actionCreator;

const App: React.FunctionComponent<AppProps> = (props: AppProps) => {
    return (
        <Router>
            <Switch>
                <Route path="/login" component={Login} />
                <MainLayout>
                    <Route exact path="/" component={Home} />
                    <Route component={Accounts} path='/accounts/:startDateIndex?' />
                    <Route path="/create-account" component={CreateAccount} />
                    <Route component={FetchData} path='/fetch-data/:startDateIndex?' />
                    <PrivateRoute path="/counter" component={Counter} />

                </MainLayout>
            </Switch>
        </Router>
    );
};

export default connect((state: ApplicationState) => state.Authentication, actionCreator)(App);