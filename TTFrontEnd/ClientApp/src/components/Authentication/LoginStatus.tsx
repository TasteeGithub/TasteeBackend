import React from 'react';
import { AuthenticatedState } from '../../store/Login';
import { ApplicationState } from '../../store/index';
import { connect } from 'react-redux';

import { Link, Redirect, withRouter } from 'react-router-dom';
import { CheckAuthentication } from '../../utils/CheckAuthentication';
import Login from './Login';

interface IState {
    isLogout: boolean
}


const LoginStatus1 = withRouter(({ history }) => (

    CheckAuthentication.isAuthenticated ? (
        <div className="dropdown">
            <a className="dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><img className="avatar" src="img/user.jpg" title={this.props.name} alt="" /></a>
            <div className="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
                <a className="dropdown-item" href="profile.html"><i className="ik ik-user dropdown-icon"></i> Profile</a>
                <a className="dropdown-item" href="#"><i className="ik ik-settings dropdown-icon"></i> Settings</a>
                <a className="dropdown-item" href="#"><span className="float-right"><span className="badge badge-primary">6</span></span><i className="ik ik-mail dropdown-icon"></i> Inbox</a>
                <a className="dropdown-item" href="#"><i className="ik ik-navigation dropdown-icon"></i> Message</a>
                <a href="" onClick={() => CheckAuthentication.Sigout(() => history.push('/'))} className="dropdown-item"><i className="ik ik-power dropdown-icon"></i> Logout</a>
            </div>
        </div>
    ) : (
            <Link to="/login" component={Login}>Login in</Link>
        )
))

class LoginStatus extends React.PureComponent<AuthenticatedState, IState>{
    constructor(props: AuthenticatedState) {
        super(props);
        this.state = { isLogout: false };
    }

    handleLogout() {
        localStorage.removeItem("email");
        localStorage.removeItem("isAuthen");
        this.setState({ isLogout: true });
    }
    public render() {
        if (this.state.isLogout) {
            return <Redirect to="/login" />
        }
        return (
            <div className="dropdown">
                <a className="dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><img className="avatar" src="img/user.jpg" title={this.props.name} alt="" /></a>
                <div className="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
                    <a className="dropdown-item" href="profile.html"><i className="ik ik-user dropdown-icon"></i> Profile</a>
                    <a className="dropdown-item" href="#"><i className="ik ik-settings dropdown-icon"></i> Settings</a>
                    <a className="dropdown-item" href="#"><span className="float-right"><span className="badge badge-primary">6</span></span><i className="ik ik-mail dropdown-icon"></i> Inbox</a>
                    <a className="dropdown-item" href="#"><i className="ik ik-navigation dropdown-icon"></i> Message</a>
                    <a href="" onClick={this.handleLogout.bind(this)} className="dropdown-item"><i className="ik ik-power dropdown-icon"></i> Logout</a>
                </div>
            </div>
        );
    }
}

const mapStateToProps = (state: ApplicationState) => state.Authentication

export default connect(mapStateToProps)(LoginStatus);
//export default LoginStatus;