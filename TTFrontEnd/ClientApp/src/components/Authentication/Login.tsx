import React, { useState } from 'react';
import { AuthenTicatedState, actionCreator } from '../../store/Login';
import { ApplicationState } from '../../store/index';
import { connect } from 'react-redux';
import LoginStatus from './LoginStatus'

type AuthenProps = AuthenTicatedState & typeof actionCreator;

const Login: React.FunctionComponent<AuthenProps> = (props: AuthenProps) => {
    const handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
    }

    const [email, setEmail] = useState("");
    const [pass, setPass] = useState("");
    const handleEmailChange = (e: React.FormEvent<HTMLInputElement>) => {
        setEmail(e.currentTarget.value);
    }

    const handlePassChange = (e: React.FormEvent<HTMLInputElement>) => {
        setPass(e.currentTarget.value);
    }
    const handleLoginIn = () => {
        props.setAuthen(
            {
                email: email, password: pass
            });
    }
    const handleLoginOut = () => {
        props.setUnAuthen();
    }
    return (
        <>
            <LoginStatus />
            <div className="row">
                <div className="col-md-6">
                    <div className="card" style={{minHeight:"260px"}}>
                        <div className="card-header"><h3>Login in</h3></div>
                        <div className="card-body">
                            <form className="forms-sample" onSubmit={handleSubmit}>
                                
                                <div className="form-group row">
                                    <label htmlFor="exampleInputEmail2" className="col-sm-3 col-form-label">Email</label>
                                    <div className="col-sm-9">
                                        <input type="email" onChange={handleEmailChange} className="form-control is-valid"
                                            id="exampleInputEmail2" placeholder="Email" />
                                    </div>
                                </div>

                                <div className="form-group row">
                                    <label htmlFor="exampleInputPassword2" className="col-sm-3 col-form-label">Password</label>
                                    <div className="col-sm-9">
                                        <input type="password" onChange={handlePassChange} className="form-control" id="exampleInputPassword2" placeholder="Password" />
                                    </div>
                                </div>
                                <div className="text-center">
                                    <button type="submit" onClick={handleLoginIn} className="btn btn-primary mr-2">Login</button>
                                    <button className="btn btn-light" onClick={handleLoginOut}>Log out</button>
                                </div>
                                
                            </form>
                        </div>
                    </div>

                </div>
            </div>
        </>
    );
}

const mapStateToProps = (state: ApplicationState) => state.Authentication
export default connect(mapStateToProps, actionCreator)(Login);