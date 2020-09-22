import * as React from 'react'
import axios from 'axios';
import {useHistory } from 'react-router-dom';
import jwtDecode from 'jwt-decode';

export
    interface IProps {
    accountId: string | undefined,
    email: string,
    fullName: string,
    modalId: string
}

const SetPassword: React.FunctionComponent<IProps> = (props: IProps) => {
    const authToken = localStorage.token;
    let history = useHistory();

    if (authToken == null) {
        history.push("/");
    }
    else{
        const decodeToken: any = jwtDecode(authToken);
        if (decodeToken.role !== "Administrator" && decodeToken.role !=="SupperAdmin") {
            history.push("/");
        }
    }

    const [password, setPass] = React.useState('');
    const [rePassword, setRePass] = React.useState('');
    const [message, setMessage] = React.useState('');
    const [successMessage, setSuccessMessage] = React.useState('');
    const [isLoading, setLoading] = React.useState(false);

    const handleSubmit = (e: React.FormEvent) => {
        e.preventDefault();
        setMessage("");
        setSuccessMessage("");
        if (password === rePassword) {
            setLoading(true);
            SetPassword()
            setLoading(false);
        }
        else {
            setMessage("Password and Re Password do not match !");
        }
        
    }
    const SetPassword = async () => {

        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
        }
        else
            return false;

        var setPasswordRequest = {
            id: props.accountId,
            password: password
        }
        var response = await axios.post("/api/operators/set-password/", setPasswordRequest);
        if (response.data.successful) {
            //TODO: Close popup
            setSuccessMessage("Set password success !");
        }
        else {
            setMessage(response.data.error);
            setSuccessMessage("");
        }
    }
    const HidePopup = () => {
        //$("#setPassword" as any).modal("hide");
    }
    const handlePassword = (e: React.FormEvent<HTMLInputElement>) => {
        setPass(e.currentTarget.value);
    }
    const handleRePassword = (e: React.FormEvent<HTMLInputElement>) => {
        setRePass(e.currentTarget.value);
    }
    return (

        <div className="modal fade" id={props.modalId} tabIndex={-1} role="dialog" aria-labelledby="modalCenter" aria-hidden="true">
            <form onSubmit={handleSubmit}>
                <div className="modal-dialog modal-dialog-centered" role="document">

                    <div className="modal-content">
                        <div className="modal-header">
                            <h5 className="modal-title" id="exampleModalCenterLabel">Set password</h5>
                            <button type="button" className="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                        </div>
                        <div className="modal-body">
                            <div className="forms-sample" >
                                <div className="form-group row">
                                    <label
                                        htmlFor="email"
                                        className="col-sm-3 col-form-label">
                                        Email
                                </label>
                                    <div className="col-sm-9">
                                        <input
                                            name="email"
                                            className="form-control"
                                            id="email"
                                            readOnly
                                            value={props.email}
                                        />
                                    </div>
                                </div>

                                <div className="form-group row">
                                    <label
                                        htmlFor="fullName"
                                        className="col-sm-3 col-form-label">
                                        Full name
                                </label>
                                    <div className="col-sm-9">
                                        <input
                                            name="fullName"
                                            className="form-control"
                                            id="fullName"
                                            readOnly
                                            value={props.fullName}
                                        />
                                    </div>
                                </div>

                                <div className="form-group row">
                                    <label
                                        htmlFor="password"
                                        className="col-sm-3 col-form-label">
                                        Password
                                    </label>
                                    <div className="col-sm-9">
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
                                        htmlFor="rePassword"
                                        className="col-sm-3 col-form-label">
                                        Re Password
                                    </label>
                                    <div className="col-sm-9">
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
                                    <div className="widget" style={{ boxShadow: "none", marginBottom:0}}>
                                        <div className="widget-body">
                                            <div className="overlay" style={{ backgroundColor: "white" }}>
                                                <i className="ik ik-refresh-cw loading"></i>
                                            </div>
                                        </div>
                                    </div>
                                }
                                {message.length > 0 &&
                                    <div className="form-group row">
                                        <div className="col-sm-3">
                                        </div>
                                        <div className="col-sm-9">
                                            <label className="text-danger">
                                                {message}
                                            </label>
                                        </div>
                                    </div>
                                }
                                {successMessage.length > 0 &&
                                    <div className="form-group row">
                                        <div className="col-sm-3">
                                        </div>
                                        <div className="col-sm-9">
                                            <label className="text-success">
                                                {successMessage}
                                            </label>
                                        </div>
                                    </div>
                                }
                            </div>
                        </div>
                        <div className="modal-footer">
                            <button type="submit" disabled={isLoading} className="btn btn-primary"><i className="ik ik-save" />Save</button>
                            <button type="button" className="btn btn-secondary" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </form>
        </div>
    );
}

export default SetPassword;