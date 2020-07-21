import React from 'react';
import { Link} from 'react-router-dom';
import { CheckAuthentication } from '../../utils/CheckAuthentication';
import jwtDecode from 'jwt-decode';

const LoginStatus: React.FunctionComponent = () => {
    const handleLogout = () => {
        CheckAuthentication.Sigout();
    }
    if (CheckAuthentication.IsSigning()) {
        var authToken = localStorage.token;
        const decodeToken: any = jwtDecode(authToken);

        return (
            <div className="dropdown">
                <a className="dropdown-toggle" href="#" id="userDropdown" role="button" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false"><img className="avatar" src="img/user.jpg"
                    title={decodeToken.fullName} alt="" />{decodeToken.fullName}</a>
                <div className="dropdown-menu dropdown-menu-right" aria-labelledby="userDropdown">
                    <Link className="dropdown-item" to={"/change-password"} ><i className="ik ik-user dropdown-icon"></i>Change Password</Link>
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