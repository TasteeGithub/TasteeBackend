import * as React from 'react'
import axios from 'axios';
import { useParams } from 'react-router-dom';
import { CheckAuthentication } from '../../utils/CheckAuthentication';
import jwtDecode from 'jwt-decode';
import {useHistory} from "react-router-dom"
const ChangPassword: React.FunctionComponent = () => {
    const authToken = localStorage.token;
    var history = useHistory();
    const [password, setPass] = React.useState('');
    const [newPassword, setNewPass] = React.useState('');
    const [rePassword, setRePass] = React.useState('');
    const [message, setMessage] = React.useState('');
    const [isLoading, setLoading] = React.useState(false);
    
    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setMessage("");

        if (newPassword === rePassword) {
            setLoading(true);
            ChangePassword()
            setLoading(false);
        }
        else {
            setMessage("New Password and Re Password do not match !");
        }
    }

    const ChangePassword = async () => {
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
        }
        else
            return false;
        const decodeToken: any = jwtDecode(authToken);
        var ChangePasswordRequest = {
            id: decodeToken.userId,
            password: password,
            newPassword: newPassword
        }
        var response = await axios.put("https://localhost:44354/api/operators/change-password/", ChangePasswordRequest);
        if (response.data.successful) {
            CheckAuthentication.Sigout();
            history.push("/login");
        }
        else {
            setMessage(response.data.error);
        }
    }

    const handlePassword = (e: React.FormEvent<HTMLInputElement>) => {
        setPass(e.currentTarget.value);
    }
    const handleNewPassword = (e: React.FormEvent<HTMLInputElement>) => {
        setNewPass(e.currentTarget.value);
    }

    const handleRePassword = (e: React.FormEvent<HTMLInputElement>) => {
        setRePass(e.currentTarget.value);
    }
    return (
        <div className="card">
            <div className="card-body">
                <form onSubmit={handleSubmit}>
                    <div className="form-group row">
                        <label className="col-sm-3 col-md-2 col-form-label" htmlFor="password">
                            Password
                        </label>
                        <div className="col-sm-9 col-md-4">
                            <input
                                required
                                name="password"
                                type="password"
                                className="form-control"
                                id="password"
                                placeholder="Password"
                                maxLength={100}
                                onChange={handlePassword}
                            />
                        </div>
                    </div>
                    <div className="form-group row">
                        <label
                            htmlFor="newPassword"
                            className="col-sm-3 col-md-2 col-form-label">
                            New Password
                                </label>
                        <div className="col-sm-9 col-md-4">
                            <input
                                required
                                name="newPassword"
                                type="password"
                                className="form-control"
                                id="newPassword"
                                placeholder="Password"
                                maxLength={100}
                                onChange={handleNewPassword}
                            />
                        </div>

                    </div>
                    <div className="form-group row">
                        <label
                            htmlFor="rePassword"
                            className="col-sm-3 col-md-2 col-form-label">
                            Re Password
                                </label>
                        <div className="col-sm-9 col-md-4">
                            <input
                                required
                                name="rePassword"
                                type="password"
                                className="form-control"
                                id="rePassword"
                                placeholder="Password"
                                maxLength={100}
                                onChange={handleRePassword}
                            />
                        </div>
                    </div>
                    {isLoading &&
                        <div className="form-group row">
                        <div className="col-sm-3 col-md-2">
                        </div>
                        <div className="col-sm-9 col-md-4">
                            <div className="widget" style={{ boxShadow: "none", marginBottom: 0 }}>
                                <div className="widget-body">
                                    <div className="overlay" style={{ backgroundColor: "white" }}>
                                        <i className="ik ik-refresh-cw loading"></i>
                                    </div>
                                </div>
                            </div>
                        </div>
                        </div>
                    }
                    {message.length > 0 &&
                        <div className="form-group row">
                            <div className="col-sm-3 col-md-2">
                            </div>
                            <div className="col-sm-9 col-md-4">
                                <label className="text-danger">
                                    {message}
                                </label>
                            </div>
                        </div>
                    }

                    <div className="form-group row">
                        <label className="col-sm-3 col-md-2 col-form-label"></label>
                        <div className="col-sm-9 col-md-4">
                            <button disabled={isLoading} type="submit" className="btn btn-primary mr-2"><i className="ik ik-save" />Save</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    );

}
export default ChangPassword;