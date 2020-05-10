import * as React from 'react';
const CreateAccount: React.FunctionComponent = () => {
    return (
        <div className="card">
            <div className="card-body">
                <form className="forms-sample">
                    <div className="form-group row">
                        <label htmlFor="exampleInputEmail2" className="col-sm-3 col-form-label">
                            Email
                            </label>
                        <div className="col-sm-9">
                            <input
                                type="email"
                                className="form-control"
                                id="exampleInputEmail2"
                                placeholder="Email"
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
                                type="text"
                                className="form-control"
                                id="exampleInputUsername2"
                                placeholder="Full name"
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
                            Re Password
                            </label>
                        <div className="col-sm-9">
                            <input
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
                                type="date"
                                className="form-control"
                                id="inputBithday"
                                placeholder="Birthday"
                            />
                        </div>
                    </div>

                    <div className="form-group">
                        <label className="custom-control custom-radio">
                            <input type="radio" className="custom-control-input" />
                            <span className="custom-control-label">&nbsp;Remember me</span>
                        </label>
                    </div>
                    <button type="submit" className="btn btn-primary mr-2">
                        Submit
                        </button>
                    <button className="btn btn-light">Cancel</button>
                </form>
            </div>
        </div>
    );
}

export default CreateAccount;