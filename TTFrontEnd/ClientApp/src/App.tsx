import * as React from 'react';

import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect } from 'react-redux'
import { BrowserRouter as Router, Link, Route, Redirect, withRouter, RouteComponentProps } from 'react-router-dom';
import './custom.css'
import Login from './components/Authentication/Login';
import { AuthenticatedState, actionCreator } from './store/Login';
import { ApplicationState } from './store';
import Main from './components/Main';
import PrivateRoute from './utils/PrivateRoute';
import { CheckAuthentication } from './utils/CheckAuthentication';

type AppProps = AuthenticatedState & typeof actionCreator;

const AuthButton = withRouter(({ history }) => (
    CheckAuthentication.isAuthenticated ? (
        <p>
            Welcom ! <button onClick={() => CheckAuthentication.Sigout(() => history.push('/'))}>Sign out</button>
        </p>
    ) : (
            <p>You are not logged in.</p>
        )
))

const App: React.FunctionComponent<AppProps> = (props: AppProps) => {
    return (
        <Router>
            <Route exact path="/" component={Home} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/count" component={Counter} />
            <Route exact path="/main" component={Main} />
            <PrivateRoute component={Counter} path='/counter' />
            <Route component={FetchData} path='/fetch-data' />
        </Router>
    );
};

export default connect((state: ApplicationState) => state.Authentication, actionCreator)(App);