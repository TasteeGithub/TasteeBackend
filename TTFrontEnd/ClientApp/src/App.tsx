import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect } from 'react-redux'

import './custom.css'
import Login from './components/Authentication/Login';
import { AuthenticatedState, actionCreator } from './store/Login';
import { ApplicationState } from './store';
import Main from './components/Main';

type AppProps = AuthenticatedState & typeof actionCreator;

const App: React.FunctionComponent<AppProps> = (props: AppProps) => {
    const isAuthen = localStorage.isAuthen === "true";
        if( isAuthen ){
            const email = localStorage.email;
        props.setAuthen({email:email,password:""})
    }
    if (isAuthen || props.authenticated) {
        return (
            <Main>
                <Layout>
                    <Route exact path='/' component={Home} />
                    <Route path='/counter' component={Counter} />
                    <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
                    <Route path='/login' component={Login} />
                </Layout>
            </Main>
        );
    }
    else {
        return (
            <Login />
        );
    }
};

export default connect((state: ApplicationState) => state.Authentication, actionCreator)(App);