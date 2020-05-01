import React from 'react';
import {AuthenTicatedState} from '../store/Login';
import {ApplicationState} from '../store/index';
import { connect } from 'react-redux';

const Login: React.FunctionComponent<AuthenTicatedState> = (props:AuthenTicatedState) => {

    const handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
        alert("sdfsdf");
    }

    return (
        <div className="row">
            <div className="col-md-6">
                <div className="card">
                    <div className="card-header"><h3>Login in  {props.name} </h3></div>
                    <div className="card-body">
                        <form className="forms-sample" onSubmit={handleSubmit}>
                            <div className="form-group">
                                <label htmlFor="exampleInputEmail1">Email</label>
                                <input required type="email" className="form-control" id="exampleInputEmail1" placeholder="Email" />
                            </div>
                            <div className="form-group">
                                <label htmlFor="exampleInputPassword1">Password</label>
                                <input required type="password" className="form-control" id="exampleInputPassword1" placeholder="Password" />
                            </div>
                            <div className="text-center">
                                <button type="submit" className="btn btn-primary mr-2">Login</button>
                            </div>
                        </form>
                    </div>
                </div>
            </div>
        </div>
    );
}

const mapStateToProps = (state: ApplicationState) => state.Authentication


export default connect(mapStateToProps)(Login);