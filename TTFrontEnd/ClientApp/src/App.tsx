import * as React from 'react';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import { connect } from 'react-redux'
import { BrowserRouter as Router, Route} from 'react-router-dom';
import './custom.css'
import Login from './components/Authentication/Login';
import { AuthenticatedState, actionCreator } from './store/Login';
import { ApplicationState } from './store';
import Main from './components/Main';
import PrivateRoute from './utils/PrivateRoute';

type AppProps = AuthenticatedState & typeof actionCreator;

const App: React.FunctionComponent<AppProps> = (props: AppProps) => {
    return (
        <Router>
            <Route exact path="/" component={Home} />
            <Route exact path="/login" component={Login} />
            <Route exact path="/count" component={Counter} />
            <Route exact path="/main" component={Main} />
            <PrivateRoute component={Counter} path='/counter' />
            <Route component={FetchData} path='/fetch-data/:startDateIndex?' />
        </Router>
    );
};

export default connect((state: ApplicationState) => state.Authentication, actionCreator)(App);