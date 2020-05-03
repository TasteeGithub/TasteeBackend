//import 'bootstrap/dist/css/bootstrap.css';

import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Provider } from 'react-redux';
import registerServiceWorker from './registerServiceWorker';
import {BrowserRouter as  Router, Link, Route, Redirect, withRouter, RouteComponentProps } from 'react-router-dom';
import PrivateRoute from './utils/PrivateRoute';
import Home from './components/Home';

import Counter from './components/Counter';




import { CheckAuthentication } from './utils/CheckAuthentication';
import { ConnectedRouter } from 'connected-react-router';
import { createBrowserHistory, LocationState } from 'history';
import configureStore from './store/configureStore';
import { StaticContext } from 'react-router';
import FetchData from './components/FetchData';

// Create browser history to use in the Redux store
const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;

// Create browser history to use in the Redux store

const history = createBrowserHistory({ basename: baseUrl });

// Get the application-wide store instance, prepopulating with state from the server where available.
const store = configureStore(history);

// Get the application-wide store instance, prepopulating with state from the server where available.


const Public = () => <h2>Day la trang public</h2>
const Protected = () => <h2>Day la trang protected</h2>


class Login extends React.Component<RouteComponentProps<{}, StaticContext, LocationState>> {
    state = {
        redirectToReferrer: false
    }

    login = () => {
        CheckAuthentication.Authenticate(() => {
            this.setState(() => ({
                redirectToReferrer: true
            }))
        });
    };

    render() {
        const { from } = this.props.location.state || { from: { pathname: '/' } }
        const { redirectToReferrer } = this.state
        if (redirectToReferrer === true) {
            return <Redirect to={from} />
        }

        return (
            <div>
                <p>You must login in to view the page</p>
                <button onClick={this.login}>Login </button>
            </div>
        );
    }
}

const AuthButton = withRouter(({ history }) => (
    CheckAuthentication.isAuthenticated ? (
        <p>
            Welcom ! <button onClick={() => CheckAuthentication.Sigout(() => history.push('/'))}>Sign out</button>
        </p>
    ) : (
            <p>You are not logged in.</p>
        )
))

function AuthExample() {
    return (
        <Router>
            <div>
                <AuthButton />
                <ul>
                    <li><Link to="/counter">Counter</Link></li>
                    <li><Link to="/fetch-data">fetch</Link></li>
                </ul>
                <Route exact path="/" component={Login} />
                <Route exact path="/login" component={Login} />
                <Route exact path="/count" component={Counter} />
                <PrivateRoute component={Counter} path='/counter' />
                <PrivateRoute component={FetchData} path='/fetch-data' />
                
            </div>
        </Router>
    );
}

ReactDOM.render(
    <Provider store={store}>
        <ConnectedRouter history={history}>
            <AuthExample />
        </ConnectedRouter>
    </Provider>,
    document.getElementById('root'));

registerServiceWorker();
