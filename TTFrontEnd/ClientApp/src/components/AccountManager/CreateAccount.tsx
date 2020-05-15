import * as React from 'react';
import { useState } from 'react';
import { stringify } from 'querystring';
import axios from 'axios';
import { Redirect } from 'react-router';

interface AccountInfo {
    email: string,
    fullName: string,
    password: string,
    rePassword: string,
    phoneNumber: string,
    birthday: string,
    gender: string,
    address: string
};

interface IState {
    selectedOption:string
}

class CreateAccount extends React.PureComponent<{}, IState> {
    constructor(props: any) {
        super(props);
        this.state = { selectedOption: "Female" };
    }

    accountInfo: AccountInfo = {
        birthday: "",
        email: "",
        fullName: "",
        password: "",
        phoneNumber: "",
        address:"",
        rePassword: "",
        gender: "",
    };
    handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
    }
    CreateAccount = async () => {
        let accountModel = {
            email: this.accountInfo.email,
            password: this.accountInfo.password,
            confirmPassword: this.accountInfo.rePassword,
            fullName: this.accountInfo.fullName,
            phoneNumber: this.accountInfo.phoneNumber,
            isLocked: false,
            birthday: this.accountInfo.birthday,
            gender: this.accountInfo.gender,
            address: this.accountInfo.address,
            role: "User",
            userLevel: 1,
            merchantLevel: 10,
            avatar: "xyz",
            status: "Pending"
        }

        let resp = await axios.post("https://localhost:44354/api/accounts", accountModel);
        alert(resp.data.successful);
        if (resp.data.successful)
            return <Redirect to={{ pathname: '/accounts' }} />;
        else
            alert(resp.data.error);
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
            case "address":
                this.accountInfo.address = e.currentTarget.value;
                break;
            case "birthday":
                this.accountInfo.birthday = e.currentTarget.value;
                break;
            case "radioGender":
                this.accountInfo.gender = e.currentTarget.value;
                this.setState({
                    selectedOption: e.currentTarget.value
                });
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
                                    onChange={this.handleChange}
                                />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label
                                htmlFor="exampleInputConfirmPassword2"
                                className="col-sm-3 col-form-label">
                                Re Password
                            </label>
                            <div className="col-sm-9">
                                <input
                                    name="rePassword"
                                    type="password"
                                    className="form-control"
                                    id="exampleInputConfirmPassword2"
                                    placeholder="Password"
                                    onChange={this.handleChange}
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
                                    onChange={this.handleChange}
                                />
                            </div>
                        </div>

                        <div className="form-group row">
                            <label htmlFor="inputAddress" className="col-sm-3 col-form-label">
                                Phone
                            </label>
                            <div className="col-sm-9">
                                <input
                                    name="address"
                                    type="text"
                                    className="form-control"
                                    id="inputAddress"
                                    placeholder="Address"
                                    onChange={this.handleChange}
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
                                    onChange={this.handleChange}
                                />
                            </div>
                        </div>
                        <div className="form-radio row">
                            <label className="col-sm-3 col-form-label">Gender</label>
                            <div className="col-sm-9">
                                <div className="radio radio-inline">
                                    <label>
                                        <input type="radio" value="Female" name="radioGender" checked={this.state.selectedOption === "Female"}
                                            onChange={this.handleChange} />
                                        <i className="helper"></i>Female
                                    </label>
                                </div>
                                <div className="radio radio-inline">
                                    <label>
                                        <input type="radio" value="Male" name="radioGender" checked={this.state.selectedOption === "Male"}
                                            onChange={this.handleChange}/>
                                        <i className="helper"></i>Male
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-sm-3 col-form-label" htmlFor="inputavatar"></label>
                            <div className="col-sm-9">
                                <input type="file" id="inputavatar" name="avatar"/>
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