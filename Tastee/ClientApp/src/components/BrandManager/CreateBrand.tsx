import * as React from 'react';
import { formatDate } from "../../utils/Utilities";

import axios from 'axios';
import { Redirect } from 'react-router';
import Role from '../OperatorManager/Role';
const $ = require('jquery');

interface AccountInfo {
    email: string,
    fullName: string,
    password: string,
    rePassword: string,
    phoneNumber: string,
    roleId: string
};

interface IState {
    selectedGender: string,
    imgagePreviewUrl: string | undefined,
    avatarFile: any,
    isFinished: boolean,
    birthday: Date
}

class CreateBrand extends React.PureComponent<{}, IState> {
    constructor(props: any) {
        super(props);
        this.state = { selectedGender: "Female", imgagePreviewUrl: "", avatarFile: null, isFinished: false, birthday: new Date() };
    }
    inputBirth: any;
    $inputBirth: any;
    componentDidMount = () => {
        //this.$inputBirth = this.inputBirth;

        //let dateDropper = $.fn.dateDropper; //accessing jquery function

        //$("#inputBithday").dateDropper({
        //    dropWidth: 200,
        //    dropPrimaryColor: "#1abc9c",
        //    dropBorder: "1px solid #1abc9c"
        //});

        //this.$inputBirth = $(this.inputBirth);
        //alert(this.$inputBirth.id);
        //this.$inputBirth.dateDropper({
        //    dropWidth: 200,
        //    dropPrimaryColor: "#1abc9c",
        //    dropBorder: "1px solid #1abc9c"
        //});
    }

    accountInfo: AccountInfo = {
        email: "",
        fullName: "",
        password: "",
        phoneNumber: "",
        rePassword: "",
        roleId: ""
    };
    handleSubmit = (e: React.FormEvent<HTMLElement>) => {
        e.preventDefault();
    }

    CreateAccount = async () => {
        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
        }
        else
            return;

        //if (this.state.avatarFile?.name != null) {
        //    let formData = new FormData();
        //    formData.append("myFile", this.state.avatarFile, this.state.avatarFile.name);
        //    let rs = await axios.post("/api/operators/uploadfile", formData);

        //    if (rs.status == 200) {
        //        this.accountInfo.avatar = rs.data.newFileName;
        //    }
        //    else {
        //        alert(rs.status);
        //        return;
        //    }
        //}

        let accountModel = {
            email: this.accountInfo.email,
            password: this.accountInfo.password,
            confirmPassword: this.accountInfo.rePassword,
            fullName: this.accountInfo.fullName,
            phoneNumber: this.accountInfo.phoneNumber,
            role: "",
            roleId: this.accountInfo.roleId,
            status: "Active"
        }

        axios.post("/api/operators", accountModel)
            .then((rs) => {
                if (rs.data.successful)
                    this.setState({
                        ...this.state,
                        isFinished: true
                    });
                else
                    alert(rs.data.error[0]);
            })
            .catch((e) => {
                if (e.response) {
                    var error = JSON.parse(e.response.request.response).errors;
                    let errorMessage: string = "";
                    let i = 0;
                    if (error.Email != null) {
                        for (i = 0; i < error.Email.length; i++) {
                            errorMessage += `${error.Email[i]}`
                        }
                    }

                    if (error.FullName != null) {
                        for (i = 0; i < error.FullName.length; i++) {
                            errorMessage += `${error.FullName[i]}`
                        }
                    }

                    if (error.Password != null) {
                        for (i = 0; i < error.Password.length; i++) {
                            errorMessage += `${error.Password[i]}`
                        }
                    }

                    if (error.ConfirmPassword != null) {
                        for (i = 0; i < error.ConfirmPassword.length; i++) {
                            errorMessage += `${error.ConfirmPassword[i]}`
                        }
                    }

                    if (error.PhoneNumber != null) {
                        for (i = 0; i < error.PhoneNumber.length; i++) {
                            errorMessage += `${error.PhoneNumber[i]}`
                        }
                    }

                    alert(errorMessage);
                }
                else if (e.request) {
                    alert(e + ",rs");
                }
            });

        //try {
        //    let resp = await axios.post("/api/operators", accountModel);

        //    if (resp.data.successful)
        //        this.setState({
        //            ...this.state,
        //            isFinished: true
        //        });
        //    else
        //            alert(resp.data.error);
        //} catch (e) {
        //    console.log(stringify(e));
        //}
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
        }
    }

    handleGetRole = (roleId: string) => {
        this.accountInfo.roleId = roleId;
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
                    avatarFile: file,
                    isFinished: this.state.isFinished,
                    birthday: this.state.birthday
                });
            }
            reader.readAsDataURL(file);
        }
    }

    render() {
        if (this.state.isFinished) return <Redirect to="/brands" />;
        let imagePreviewUrl = this.state.imgagePreviewUrl;
        let $imagePrivew = null;
        if (imagePreviewUrl) {
            $imagePrivew = (<div style={{ paddingTop: "20px" }}><img style={{ maxWidth: "400px" }} src={imagePreviewUrl} /></div>);
        }

        return (
            <div className="row">
                <div className="col-md-12">
                    <div className="card">
                        <form>
                            <div className="card-body">
                                <h5 className="card-title"><strong>Info</strong></h5>
                                <div className="row">
                                    <div className="col-md-6">
                                        <div className="forms-sample">
                                            <div className="form-group">
                                                <label htmlFor="inputUri">Uri</label>
                                                <input type="text" className="form-control" id="inputUri" placeholder="Uri" />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputBrandName">Brand's name</label>
                                                <input type="text" className="form-control" id="inputBrandName" placeholder="Brand's name" />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputAddress">Address</label>
                                                <input type="text" className="form-control" id="inputAddress" placeholder="Address" />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputHotline">Hotline</label>
                                                <input type="text" className="form-control" id="inputHotline" placeholder="Hotline" />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputEmail">Email</label>
                                                <input type="email" className="form-control" id="inputEmail" placeholder="Email" />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputPhone">Phone</label>
                                                <input type="text" className="form-control" id="inputPhone" placeholder="Phone" />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputHeadOffice">Head Office</label>
                                                <input type="text" className="form-control" id="inputHeadOffice" placeholder="Head Office" />
                                            </div>
                                            <div className="form-group">
                                                <label htmlFor="inputRestaurant">Input Restaurant Image</label>
                                                <input type="text" className="form-control" id="inputRestaurant" placeholder="Password" />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="col-md-6">
                                        <div className="form-group">
                                            <label htmlFor="inputLogo">Logo</label>
                                            <input type="text" className="form-control" id="inputLogo" placeholder="logo" />
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="inputCity">City</label>
                                            <select className="form-control" id="inputCity">
                                                <option value="HCM" selected={true}>Hồ Chí Minh</option>
                                                <option value="HN">Hà Nội</option>
                                                <option value="DN">Đà Nẵng</option>
                                            </select>
                                        </div>
                                        <div className="form-group">
                                            <label htmlFor="inputArea">Area</label>
                                            <select className="form-control" id="inputArea">
                                                <option value="1">District 1</option>
                                            </select>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputOpenTime1">Open time 1</label>
                                                    <select className="form-control" id="inputOpenTime1">
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option>00:45</option>
                                                        <option>01:00</option>
                                                        <option>01:15</option>
                                                        <option>23:30</option>
                                                        <option>23:45</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputCloseTime1">Close time 1</label>
                                                    <select className="form-control" id="inputCloseTime1">
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option>00:45</option>
                                                        <option>01:00</option>
                                                        <option>01:15</option>
                                                        <option>23:30</option>
                                                        <option>23:45</option>
                                                    </select>
                                                </div>
                                                
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputOpenTime2">Open time 2</label>
                                                    <select className="form-control" id="inputOpenTime2">
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option>00:45</option>
                                                        <option>01:00</option>
                                                        <option>01:15</option>
                                                        <option>23:30</option>
                                                        <option>23:45</option>
                                                    </select>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputCloseTime2">Close time 2</label>
                                                    <select className="form-control" id="inputCloseTime2">
                                                        <option>00:00</option>
                                                        <option>00:15</option>
                                                        <option>00:30</option>
                                                        <option>00:45</option>
                                                        <option>01:00</option>
                                                        <option>01:15</option>
                                                        <option>23:30</option>
                                                        <option>23:45</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </div>
                                        <div className="row">
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputMinPrice">Min Price (VNĐ)</label>
                                                    <input type="number" className="form-control" id="inputMinPrice"/>
                                                </div>
                                            </div>
                                            <div className="col-md-6">
                                                <div className="form-group">
                                                    <label htmlFor="inputMinPrice">Max Price (VNĐ)</label>
                                                    <input type="number" className="form-control" id="inputMaxPrice" />
                                                </div>
                                            </div>
                                        </div>

                                        <div className="form-group">
                                            <label htmlFor="inputMetaDescription">Meta description</label>
                                            <textarea rows={5} className="form-control" id="inputMetaDescription" placeholder="Meta Description" />
                                        </div>

                                    </div>
                                </div>
                                
                                <div className="row">
                                    <div className="col-md-6">
                                        <h5 className="card-title"><strong>SEO</strong></h5>
                                    </div>
                                    <div className="col-md-6">
                                        <h5 className="card-title"><strong>Order Info</strong></h5>
                                    </div>
                                </div>
                            </div>
                            <div className="card-footer text-right">
                                <button type="submit" className="btn btn-primary mr-2"><i className="ik ik-save" />Save</button>
                                    <button className="btn btn-light">Cancel</button>
                              </div>
                        </form>
                    </div>
                </div>
            </div>
        );
    }
}

export default CreateBrand;