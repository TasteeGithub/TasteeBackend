import * as React from 'react';
import { RouteComponentProps, useParams, Redirect, useHistory } from 'react-router-dom';
import { useState, useEffect } from 'react';
import axios from 'axios';
import Role from './Role';
import SetPassword from './SetPassword'

export interface IValues {
    id: string,
    email: string,
    phoneNumber: string,
    fullName: string,
    createdDate: number,
    status: string
    roleId:string
}

const EditAccount: React.FunctionComponent<RouteComponentProps> = () => {
    var history = useHistory();
    const { id } = useParams();
    const [values, setValues] = useState({} as IValues);
    const [isSuccess, setSuccess] = useState(false);

    useEffect(() => {
        getData();
    }, []);

    function formatDate(date: string) {
        var d = new Date(date),
            month = '' + (d.getMonth() + 1),
            day = '' + d.getDate(),
            year = d.getFullYear();

        if (month.length < 2)
            month = '0' + month;
        if (day.length < 2)
            day = '0' + day;

        return [year, month, day].join('-');
    }

    const getData = async () => {
        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
            const result = await axios.get(`https://localhost:44354/api/operators/detail/${id}`);
            await setValues(result.data);

            if (!result.data.id) history.push("/");
            return;
        }
    }
    const handleSubmit = async (event: any) => {
        event.preventDefault();
        event.persist();

        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
            axios.put(`https://localhost:44354/api/operators`, values)
                .then(result => {
                    if (result.data.successful) {
                        setSuccess(true);
                    }
                    else
                        alert(result.data.error);
                }).catch(e => {
                    alert(e.response);
                });
        }
    };

    const handleChange = async (e: React.FormEvent<HTMLInputElement>) => {
        switch (e.currentTarget.name) {
            case "fullName":
                await setValues({ ...values, fullName: e.currentTarget.value });
                break;
            case "phone":
                await setValues({ ...values, phoneNumber: e.currentTarget.value });
                break;
        }
    }

    const handleStatusChange = async (e: React.FormEvent<HTMLSelectElement>) => {
        await setValues({ ...values, status: e.currentTarget.value });
    }

    const handleGetRole = async (roleId: string) => {
                await setValues({ ...values, roleId: roleId });
    }
    if (isSuccess)
        return <Redirect to="/accounts" />
    else
        return (
            <>
                <div className="card">
                <div className="card-body">
                    <form className="forms-sample" onSubmit={handleSubmit}>
                        <div className="form-group row">
                            <label htmlFor="exampleInputEmail2" className="col-sm-3 col-md-2 col-form-label">
                                Email
                        </label>
                            <div className="col-sm-9 col-md-4">
                                <input
                                    required
                                    name="email"
                                    type="email"
                                    className="form-control"
                                    id="exampleInputEmail2"
                                    placeholder="Email"
                                    readOnly
                                    value={values.email}
                                    maxLength={50}
                                />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label
                                htmlFor="exampleInputUsername2"
                                className="col-sm-3 col-md-2 col-form-label">
                                Full name
                            </label>
                            <div className="col-sm-9 col-md-4">
                                <input
                                    required
                                    name="fullName"
                                    type="text"
                                    className="form-control"
                                    id="exampleInputUsername2"
                                    placeholder="Full name"
                                    value={values.fullName}
                                    onChange={handleChange}
                                    maxLength={200}
                                />
                            </div>
                        </div>

                        <div className="form-group row">
                            <label htmlFor="exampleInputMobile" className="col-sm-3 col-md-2 col-form-label">
                                Phone
                            </label>
                            <div className="col-sm-9 col-md-4">
                                <input
                                    required
                                    name="phone"
                                    type="text"
                                    className="form-control"
                                    id="exampleInputMobile"
                                    placeholder="Mobile number"
                                    value={values.phoneNumber}
                                    onChange={handleChange}
                                    maxLength={15}
                                />
                            </div>
                        </div>

                        <form className="form-group row">
                            <label className="col-sm-3 col-md-2 col-form-label">Status</label>
                            <div className="col-sm-9 col-md-4">
                                <select className="form-control" name="selectStatus" onChange={handleStatusChange}>
                                    <option selected={values.status === "Active"} value="Active">Active</option>
                                    <option selected={values.status === "Inactive"} value="Inactive">Inactive</option>
                                    <option selected={values.status === "Locked"} value="Locked">Locked</option>
                                    <option selected={values.status === "Closed"} value="Closed">Closed</option>
                                </select>
                            </div>
                        </form>
                        
                        <div className="form-group row">
                            <label className="col-sm-3 col-md-2 col-form-label">Role</label>
                            <div className="col-sm-9 col-md-4">
                                <Role GetRole={handleGetRole} SelectedRoleId={values.roleId} />
                            </div>
                        </div>
                        <div className="form-group row">
                            <label className="col-sm-3 col-md-2 col-form-label"></label>
                            <div className="col-sm-9 col-md-4">
                                    <button type="submit" className="btn btn-primary mr-2"><i className="ik ik-save"/>Save</button>
                                    <button type="button" className="btn btn-link" data-toggle="modal" data-target="#setPassword">Set password</button>
                            </div>
                        </div>
                    </form>
                </div>
                </div>
                <SetPassword accountId={id} modalId="setPassword" email={values.email} fullName={values.fullName} />
            </>
        );
}
export default EditAccount;