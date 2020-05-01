import * as React from 'react';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';

import './custom.css'
import MyGame from './components/MyGame';

import { BrowserRouter as Router, Switch, Route } from 'react-router-dom'
import Login from './components/Login';

// const App: React.FC = () => {
//     useEffect(() => {
//         CheckAuthentication();
//     },[]);

//     return (
//         <div className='App'>
//         <Provider store={store}>
//         <Router>
//         <Switch>
//         <PrivateRoute
//         exact
//         path='/'
//         component={Index} />
        
//         <GuestRoute
//         exact
//         path='/login'
//         component={Login} />
        
//         </Switch>
//         </Router>
//         </Provider>
//         </div>
//         )
// }

// export default App;

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
        <Route path='/my-game' component={MyGame} />
        <Route path='/login' component={Login} />
    </Layout>
);
