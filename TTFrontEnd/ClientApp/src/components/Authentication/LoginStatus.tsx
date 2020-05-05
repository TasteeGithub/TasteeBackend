import React from 'react';
import { Link, Redirect, withRouter } from 'react-router-dom';
import { CheckAuthentication } from '../../utils/CheckAuthentication';

const LoginStatus: React.FunctionComponent = () => {
    const handleLogout = () => {
        CheckAuthentication.Sigout(() => <Redirect to="/" />);
    }
    if (CheckAuthentication.IsSigning()) {
        return (
            <div className="dropdown">
                <a className="dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><img className="avatar" src="img/user.jpg"
                    title={"Get info from token"} alt="" /></a>
                <div className="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
                    <a className="dropdown-item" href="profile.html"><i className="ik ik-user dropdown-icon"></i> Profile</a>
                    <a className="dropdown-item" href="#"><i className="ik ik-settings dropdown-icon"></i> Settings</a>
                    <a className="dropdown-item" href="#"><span className="float-right"><span className="badge badge-primary">6</span></span><i className="ik ik-mail dropdown-icon"></i> Inbox</a>
                    <a className="dropdown-item" href="#"><i className="ik ik-navigation dropdown-icon"></i> Message</a>
                    <a href="" onClick={handleLogout} className="dropdown-item"><i className="ik ik-power dropdown-icon"></i> Logout</a>
                </div>
            </div>
        );
    }
    else {
        return (

            <div className="dropdown">
                <Link className="dropdown-toggle" to="/login" id="userDropdown"><img className="avatar" src="img/user2.jpg"
                    title={"Sign In"} alt="" /></Link>

            </div>)
    }
}

export default LoginStatus;