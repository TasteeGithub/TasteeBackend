import * as React from 'react';
import { useState } from 'react';
import { stringify } from 'querystring';

interface AccountInfo {
    email: string,
    fullName: string,
    password: string,
    rePassword: string,
    phoneNumber: string,
    birthday: string
}

class CreateAccount extends React.Component {

    accountInfo: AccountInfo = {
        birthday: "",
        email : "",
        fullName: "",
        password: "",
        phoneNumber: "",
        rePassword: ""
    };  
    handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
    }
    CreateAccount = () => {
        alert(this.accountInfo.fullName);;
    }

    handleChange = (e: React.FormEvent<HTMLInputElement>) => {
        switch (e.currentTarget.name) {
            case "email":
                this.accountInfo.email = e.currentTarget.value;
                break;
            case "fullName":
                this.accountInfo.fullName = e.currentTarget.value;
                break;
            case "password":
                this.accountInfo.password = e.currentTarget.value;
                break;
            case "rePassword":
                this.accountInfo.rePassword = e.currentTarget.value;
                break;
            case "phone":
                this.accountInfo.phoneNumber = e.currentTarget.value;
                break;
            case "birthday":
                this.accountInfo.birthday = e.currentTarget.value;
                break;
        }
    }
    render() {
        return (
            <div className="card">
                <div className="card-body">
                    <form className="forms-sample" onSubmit={this.handleSubmit}>
                        <div className="form-group row">
                            <label htmlFor="exampleInputEmail2" className="col-sm-3 col-form-label">
                                Email
                        </label>
                            <div className="col-sm-9">
                                <input
                                    name="email"
                                    type="email"
                                    className="form-control"
                                    id="exampleInputEmail2"
                                    placeholder="Email"
                                    onChange={this.handleChange}
                                />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label
                                htmlFor="exampleInputUsername2"
                                className="col-sm-3 col-form-label">
                                Full name
                            </label>
                            <div className="col-sm-9">
                                <input
                                    name="fullName"
                                    type="text"
                                    className="form-control"
                                    id="exampleInputUsername2"
                                    placeholder="Full name"
                                    onChange={this.handleChange}
                                />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label
                                htmlFor="exampleInputPassword2"
                                className="col-sm-3 col-form-label">
                                Password
                            </label>
                            <div className="col-sm-9">
                                <input
                                    name="password"
                                    type="password"
                                    className="form-control"
                                    id="exampleInputPassword2"
                                    placeholder="Password"
                                />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label
                                htmlFor="exampleInputConfirmPassword2"
                                className="col-sm-3 col-form-label">
                                Re Password {this.accountInfo.fullName}
                            </label>
                            <div className="col-sm-9">
                                <input
                                    name="rePassword"
                                    type="password"
                                    className="form-control"
                                    id="exampleInputConfirmPassword2"
                                    placeholder="Password"
                                />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label htmlFor="exampleInputMobile" className="col-sm-3 col-form-label">
                                Phone
                            </label>
                            <div className="col-sm-9">
                                <input
                                    name="phone"
                                    type="text"
                                    className="form-control"
                                    id="exampleInputMobile"
                                    placeholder="Mobile number"
                                />
                            </div>
                        </div>

                        <div className="form-group row">
                            <label htmlFor="inputBithday" className="col-sm-3 col-form-label">
                                Birthday
                            </label>
                            <div className="col-sm-9">
                                <input
                                    name="birthday"
                                    type="date"
                                    className="form-control"
                                    id="inputBithday"
                                    placeholder="Birthday"
                                />
                            </div>
                        </div>

                        <div className="form-group row">
                            <label className="col-sm-3 col-form-label"></label>
                            <div className="col-sm-9">
                                <button type="submit" onClick={this.CreateAccount} className="btn btn-primary mr-2">OK</button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        );
    }
}

export default CreateAccount;