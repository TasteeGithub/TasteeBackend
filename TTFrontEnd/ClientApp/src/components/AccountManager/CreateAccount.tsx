import * as React from 'react';
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
    address: string,
    avatar: string
};

interface IState {
    selectedGender: string,
    imgagePreviewUrl: string | undefined,
    avatarFile: any,
    isFinished: boolean
}

class CreateAccount extends React.PureComponent<{}, IState> {
    constructor(props: any) {
        super(props);
        this.state = { selectedGender: "Female", imgagePreviewUrl: "", avatarFile: null, isFinished:false };
    }

    accountInfo: AccountInfo = {
        birthday: "",
        email: "",
        fullName: "",
        password: "",
        phoneNumber: "",
        address: "",
        rePassword: "",
        gender: "Femail",
        avatar: ""
    };
    handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
    }
    
    CreateAccount = async () => {

        let formData = new FormData();
        formData.append("myFile", this.state.avatarFile, this.state.avatarFile.name);
        let rs = await axios.post("https://localhost:44354/api/accounts/uploadfile", formData);
        if (rs.status == 200) {
            this.accountInfo.avatar = rs.data.dbPath;
        }

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
            avatar: this.accountInfo.avatar,
            status: "Pending"
        }

        let resp = await axios.post("https://localhost:44354/api/accounts", accountModel);

        if (resp.data.successful)
        this.setState({
            ...this.state,
            isFinished:true
        });
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
                    selectedGender: e.currentTarget.value,
                    imgagePreviewUrl: this.state.imgagePreviewUrl,
                    avatarFile: this.state.avatarFile,
                    isFinished: this.state.isFinished
                });
                break;
        }
    }

    handleImageChange = (e: React.FormEvent<HTMLInputElement>) => {
        e.preventDefault();

        let reader = new FileReader();

        let file = e.currentTarget.files == null ? null : e.currentTarget.files[0];
        
        if (file != null) {
            reader.onloadend = () => {
                
                this.setState({
                    selectedGender: this.state.selectedGender,
                    imgagePreviewUrl: reader.result?.toString(),
                    avatarFile: file, // e.currentTarget.files === null ? null : e.currentTarget.files[0]
                    isFinished:this.state.isFinished
                });
            }
            reader.readAsDataURL(file);
        }

        
    }

    render() {
        if (this.state.isFinished) return <Redirect to="/accounts" />;
        let imagePreviewUrl = this.state.imgagePreviewUrl;
        let $imagePrivew = null;
        if (imagePreviewUrl) {
            $imagePrivew = (<div style={{paddingTop:"20px"}}><img style={{maxWidth:"400px"}} src={imagePreviewUrl} /></div>);
        }

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
                                        <input type="radio" value="Female" name="radioGender" checked={this.state.selectedGender === "Female"}
                                            onChange={this.handleChange} />
                                        <i className="helper"></i>Female
                                    </label>
                                </div>
                                <div className="radio radio-inline">
                                    <label>
                                        <input type="radio" value="Male" name="radioGender" checked={this.state.selectedGender === "Male"}
                                            onChange={this.handleChange} />
                                        <i className="helper"></i>Male
                                    </label>
                                </div>
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-sm-3 col-form-label" htmlFor="inputavatar"></label>
                            <div className="col-sm-9">
                                <input type="file" id="inputavatar" name="avatar" onChange={this.handleImageChange} />
                                {$imagePrivew}
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