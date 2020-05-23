import * as React from 'react';
import { RouteComponentProps, useParams } from 'react-router-dom';
import { useState, useEffect } from 'react';
import axios from 'axios';

export interface IValues {
    id: string,
    email: string,
    phoneNumber: string,
    fullName: string,
    createdDate: number ,
    isLocked: string,
    birthday: number,
    gender: string ,
    address: string,
    userLevel: string,
    avatar: string,
    status: string
}

const EditAccount: React.FunctionComponent<RouteComponentProps> = () => {
    const { id } = useParams();
    const [values, setValues] = useState({} as IValues);
    useEffect(() => {
        getData();
    }, []);

    const getData = async () => {
        const customer = await axios.get(`https://localhost:44354/api/accounts/detail/${id}`);
        await setValues(customer.data);
    }
    const handleSubmit = (event: any) => {
        event.persist();
    };

    const handleChange = (event: any) => {
        event.persist();
    };

    return (
        <div className="card">
            <div className="card-body">
                <form className="forms-sample" onSubmit={handleSubmit}>
                    <div className="form-group row">
                        <label htmlFor="exampleInputEmail2" className="col-sm-3 col-form-label">
                            Email
                        </label>
                        <div className="col-sm-9">
                            <input
                                required
                                name="email"
                                type="email"
                                className="form-control"
                                id="exampleInputEmail2"
                                placeholder="Email"
                                value={values.email}
                                readOnly={true}
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
                                required
                                name="fullName"
                                type="text"
                                className="form-control"
                                id="exampleInputUsername2"
                                placeholder="Full name"
                                value={values.fullName}
                                onChange={handleChange}
                            />
                        </div>
                    </div>
                    <div className="form-group row">
                        <label htmlFor="exampleInputMobile" className="col-sm-3 col-form-label">
                            Phone
                            </label>
                        <div className="col-sm-9">
                            <input
                                required
                                name="phone"
                                type="text"
                                className="form-control"
                                id="exampleInputMobile"
                                placeholder="Mobile number"
                                value={values.phoneNumber}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="form-group row">
                        <label htmlFor="inputAddress" className="col-sm-3 col-form-label">
                            Address
                            </label>
                        <div className="col-sm-9">
                            <input
                                required
                                name="address"
                                type="text"
                                className="form-control"
                                id="inputAddress"
                                placeholder="Address"
                                value={values.address}
                                onChange={handleChange}
                            />
                        </div>
                    </div>

                    <div className="form-group row">
                        <label htmlFor="inputBithday" className="col-sm-3 col-form-label">
                            Birthday
                            </label>
                        <div className="col-sm-9">
                            <input
                                required
                                name="birthday"
                                type="date"
                                className="form-control"
                                id="inputBithday"
                                placeholder="Birthday"
                                value={values.birthday}
                                onChange={handleChange}
                            />
                        </div>
                    </div>
                    <div className="form-radio row">
                        <label className="col-sm-3 col-form-label">Gender</label>
                        <div className="col-sm-9">
                            <div className="radio radio-inline">
                                <label>
                                    <input type="radio" value="Female" name="radioGender"
                                        onChange={handleChange} />
                                    <i className="helper"></i>Female
                                    </label>
                            </div>
                            <div className="radio radio-inline">
                                <label>
                                    <input type="radio" value="Male" name="radioGender"
                                        onChange={handleChange} />
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
                            <button type="submit"className="btn btn-primary mr-2">OK</button>
                        </div>
                    </div>
                </form>
            </div>
        </div>
    );
}
export default EditAccount;