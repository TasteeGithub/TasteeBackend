import React, { useState } from 'react';
import { AuthenticatedState, actionCreator } from '../../store/Login';
import { ApplicationState } from '../../store/index';
import { connect } from 'react-redux';
import LoginStatus from './LoginStatus'
import { LocationState } from 'history';
import { RouteComponentProps } from 'react-router-dom';
import { StaticContext, Redirect } from 'react-router';
import { CheckAuthentication } from '../../utils/CheckAuthentication';

const Login: React.FunctionComponent<RouteComponentProps<{}, StaticContext, LocationState>> = (props: RouteComponentProps<{}, StaticContext, LocationState>) => {

    let loginInfo = {
        email: "",
        password: ""
    };
    let error = "";
    const handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
    }

    const handleEmailChange = (e: React.FormEvent<HTMLInputElement>) => {
        loginInfo.email = e.currentTarget.value;
        //setEmail(e.currentTarget.value);
    }

    const handlePassChange = (e: React.FormEvent<HTMLInputElement>) => {
        //setPass(e.currentTarget.value);
        loginInfo.password = e.currentTarget.value
    }

    const handleLoginIn = async () => {
        if (loginInfo.email.length > 0 && loginInfo.password.length > 0) {
            await CheckAuthentication.Login(loginInfo);
            if (CheckAuthentication.IsSigning()) {
                const { from } = props.location.state || { from: { pathname: '/' } }
                return <Redirect to={from} />;
            }
        }
    }

    return (
        <>
            <div className="auth-wrapper">
                <div className="container-fluid h-100">
                    <div className="row flex-row h-100 bg-white">
                        <div className="col-xl-8 col-lg-6 col-md-5 p-0 d-md-block d-lg-block d-sm-none d-none">
                            <div className="lavalite-bg" style={{ backgroundImage: "url('../img/auth/login-bg.jpg')" }}>
                                <div className="lavalite-overlay"></div>
                            </div>
                        </div>
                        <div className="col-xl-4 col-lg-6 col-md-7 my-auto p-0">
                            <div className="authentication-form mx-auto">
                                <div className="logo-centered">
                                    <a href="/"><img src="../src/img/brand.svg" alt="" /></a>
                                </div>
                                <h3>Sign In to ThemeKit</h3>
                                <p>Happy to see you again!</p>
                                <form onSubmit={handleSubmit}>
                                    <div className="form-group">
                                        <input type="text" onChange={handleEmailChange} className="form-control" placeholder="Email" required />
                                        <i className="ik ik-user"></i>
                                    </div>
                                    <div className="form-group">
                                        <input type="password" onChange={handlePassChange} className="form-control" placeholder="Password" required />
                                        <i className="ik ik-lock"></i>
                                    </div>
                                    <div className="row">
                                        <div className="col text-left">
                                            <label className="custom-control custom-checkbox">
                                                <input type="checkbox" className="custom-control-input" id="item_checkbox" name="item_checkbox" value="option1" />
                                                <span className="custom-control-label">&nbsp;Remember Me</span>
                                            </label>
                                        </div>
                                        <div className="col text-right">
                                            <a href="forgot-password.html">Forgot Password ?</a>
                                        </div>
                                    </div>
                                    {error.length > 0 && <div className="alert alert-danger" role="alert">
                                        {error}
                                    </div>}
                                    <div className="sign-btn text-center">
                                        <button onClick={handleLoginIn} className="btn btn-theme">Sign In</button>
                                    </div>
                                </form>
                                <div className="register">
                                    <p>Don't have an account? <a href="#">Create an account</a></p>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </>
    );
}

export default Login;







//import React, { useState } from 'react';
//import { AuthenticatedState, actionCreator } from '../../store/Login';
//import { ApplicationState } from '../../store/index';
//import { connect } from 'react-redux';
//import LoginStatus from './LoginStatus'
//import { LocationState } from 'history';
//import { RouteComponentProps } from 'react-router-dom';
//import { StaticContext, Redirect } from 'react-router';
//import { CheckAuthentication } from '../../utils/CheckAuthentication';

//class Login extends React.Component<RouteComponentProps<{}, StaticContext, LocationState>> {
//    state = {
//        redirectToReferrer: false,
//        errors: "",
//    };

//    loginInfo = {
//        email: "",
//        password: ""
//    }
//    handleSubmit = (e: React.FormEvent<HTMLElement>) => {
//        e.preventDefault();
//    }

//    handleEmailChange = (e: React.FormEvent<HTMLInputElement>) => {
//        this.loginInfo.email = e.currentTarget.value;
//        //setEmail(e.currentTarget.value);
//    }

//    handlePassChange = (e: React.FormEvent<HTMLInputElement>) => {
//        //setPass(e.currentTarget.value);
//        this.loginInfo.password = e.currentTarget.value
//    }

//    handleLoginIn = async () => {
//        if (this.loginInfo.email.length > 0 && this.loginInfo.password.length > 0) {
//            await CheckAuthentication.Login(this.loginInfo);
//            console.debug("ldlafalsf");
//            this.setState(() => ({
//                redirectToReferrer: CheckAuthentication.isAuthenticated,
//                errors: CheckAuthentication.authenError
//            }));
//        }
//    }

//    render() {
//        const { from } = this.props.location.state || { from: { pathname: '/' } }
//        const { redirectToReferrer } = this.state
//        if (redirectToReferrer === true) {
//            return <Redirect to={from} />
//        }

//        return (
//            <>
//                <div className="auth-wrapper">
//                    <div className="container-fluid h-100">
//                        <div className="row flex-row h-100 bg-white">
//                            <div className="col-xl-8 col-lg-6 col-md-5 p-0 d-md-block d-lg-block d-sm-none d-none">
//                                <div className="lavalite-bg" style={{ backgroundImage: "url('../img/auth/login-bg.jpg')" }}>
//                                    <div className="lavalite-overlay"></div>
//                                </div>
//                            </div>
//                            <div className="col-xl-4 col-lg-6 col-md-7 my-auto p-0">
//                                <div className="authentication-form mx-auto">
//                                    <div className="logo-centered">
//                                        <a href="/"><img src="../src/img/brand.svg" alt="" /></a>
//                                    </div>
//                                    <h3>Sign In to ThemeKit</h3>
//                                    <p>Happy to see you again!</p>
//                                    <form onSubmit={this.handleSubmit}>
//                                        <div className="form-group">
//                                            <input type="text" onChange={this.handleEmailChange} className="form-control" placeholder="Email" required />
//                                            <i className="ik ik-user"></i>
//                                        </div>
//                                        <div className="form-group">
//                                            <input type="password" onChange={this.handlePassChange} className="form-control" placeholder="Password" required />
//                                            <i className="ik ik-lock"></i>
//                                        </div>
//                                        <div className="row">
//                                            <div className="col text-left">
//                                                <label className="custom-control custom-checkbox">
//                                                    <input type="checkbox" className="custom-control-input" id="item_checkbox" name="item_checkbox" value="option1" />
//                                                    <span className="custom-control-label">&nbsp;Remember Me</span>
//                                                </label>
//                                            </div>
//                                            <div className="col text-right">
//                                                <a href="forgot-password.html">Forgot Password ?</a>
//                                            </div>
//                                        </div>
//                                        {this.state.errors.length > 0 && <div className="alert alert-danger" role="alert">
//                                            {this.state.errors}
//                                        </div>}
//                                        <div className="sign-btn text-center">
//                                            <button onClick={this.handleLoginIn} className="btn btn-theme">Sign In</button>
//                                        </div>
//                                    </form>
//                                    <div className="register">
//                                        <p>Don't have an account? <a href="#">Create an account</a></p>
//                                    </div>

//                                </div>
//                            </div>
//                        </div>
//                    </div>
//                </div>
//            </>
//        );
//    }
//}

//const mapStateToProps = (state: ApplicationState) => state.Authentication
//export default connect(mapStateToProps, actionCreator)(Login);