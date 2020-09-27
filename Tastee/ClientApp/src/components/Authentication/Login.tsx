import React from 'react';
import { LocationState } from 'history';
import { RouteComponentProps } from 'react-router-dom';
import { StaticContext, Redirect } from 'react-router';
import { CheckAuthentication,LoginInfo } from '../../utils/CheckAuthentication';
interface IState {
    isLogin: boolean,
    error: string,
    loading: boolean
}
class Login extends React.Component<RouteComponentProps<{}, StaticContext, LocationState>, IState> {
    constructor(props: Readonly<RouteComponentProps<{}, StaticContext, any>>) {
        super(props);
        this.state = { isLogin: false,error:"", loading:false }
    }
    loginInfo: LoginInfo = {
        email: "",
        password: ""
    };

    handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
    }

    handleEmailChange = (e: React.FormEvent<HTMLInputElement>) => {
        this.loginInfo.email = e.currentTarget.value;
    }

    handlePassChange = (e: React.FormEvent<HTMLInputElement>) => {
        this.loginInfo.password = e.currentTarget.value
    }

    handleLoginIn = async () => {
        if (this.loginInfo.email.length > 0 && this.loginInfo.password.length > 0) {
            this.setState({...this.state, loading: true });
            await CheckAuthentication.Login(this.loginInfo);
            this.setState({ isLogin: CheckAuthentication.IsSigning(),error:CheckAuthentication.authenError,loading:false });
        }
    }

    render() {
        const { from } = this.props.location.state || { from: { pathname: '/' } }

        if (this.state.isLogin) {
            return <Redirect to={from} />
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
                                        <a href="/"><h2>TASTEE</h2></a>
                                    </div>
                                    <h3>Sign In to Tastee</h3>
                                    <form onSubmit={this.handleSubmit}>
                                        <div className="form-group">
                                            <input type="text" onChange={this.handleEmailChange} className="form-control" placeholder="Email" required />
                                            <i className="ik ik-user"></i>
                                        </div>
                                        <div className="form-group">
                                            <input type="password" onChange={this.handlePassChange} className="form-control" placeholder="Password" required />
                                            <i className="ik ik-lock"></i>
                                        </div>
                                        {this.state.error.length > 0 && <div className="alert alert-danger" role="alert">
                                            {this.state.error}
                                        </div>}
                                        {this.state.loading && 
                                            <div className="widget" style={{ boxShadow: "none" }}>
                                                <div className="widget-body">
                                                    <div className="overlay" style={{ backgroundColor: "white" }}>
                                                        <i className="ik ik-refresh-cw loading"></i>
                                                    </div>
                                                </div>
                                            </div>
                                        }

                                        <div className="sign-btn text-center">
                                            <button onClick={this.handleLoginIn} className="btn btn-theme">Sign In</button>
                                        </div>
                                    </form>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </>
        );
    }
}

export default Login;