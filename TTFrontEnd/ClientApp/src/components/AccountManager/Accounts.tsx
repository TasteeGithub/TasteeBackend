import React, { Component, ReactEventHandler } from 'react';
import moment from 'moment';
import axios from 'axios';
import { Link, Redirect, useHistory } from 'react-router-dom';
import { stringify } from 'querystring';
import { CheckAuthentication } from '../../utils/CheckAuthentication';
import { checkServerIdentity } from 'tls';

const $ = require('jquery');
require('datatables.net-bs4');
$.DataTable = require('datatables.net');

type DataState = {
    userData: [],
    isRedirectToCreate: boolean;
}
type Filter = {
    name:string
}

type RowProp = {
    row: any
}

const Row: React.FunctionComponent<RowProp> = (props: RowProp) => {
    return (
        <tr>
            <td>{props.row.createdDate}</td>
            <td>
                <Link to={`/edit-account/${props.row.id}`}>{props.row.fullName}</Link>
            </td>
            <td>{props.row.email}</td>
            <td>{props.row.isLocked === true ? "True" : "False"}</td>
            <td>{props.row.status}</td>
            <td>{props.row.gender}</td>
        </tr>
    )
}

export default class Accounts extends Component<{}, DataState> {
    constructor(props: any) {
        super(props);
        this.state = { userData: [], isRedirectToCreate:false }
    }
    el: any;
    $element: any;
    dataTable: any;
    LoadData(table : any){
        this.dataTable = table.DataTable({
            "processing": true,
            "serverSide": true,
            "filter": false, // this is for disable filter (search box)
            "bSort": false,
            "ajax":
            {
                "url": "https://localhost:44354/api/accounts/load-data/",
                "type": "POST",
                "dataType": "JSON",
                "contentType": "application/x-www-form-urlencoded",
                "crossDomain": true,
                "beforeSend": function (xhr: any) {
                    xhr.setRequestHeader("Authorization", localStorage.getItem('token'));
                },
                "error": function (a: any, b: any, c: any) {
                    alert(stringify(a));
                },
                "data": function (d:any) {
                    d.name = $("#name").val();
                    d.email = $("#email").val();
                    d.phone = $("#phone").val();
                    d.fromDate = $("#from").val();
                    d.toDate = $("#to").val();
                    d.status = $("#status").val();
                }
            },
            "columns": [
                {
                    data: "createdDate",
                    render: function (data: any, type: any, row: any) {
                        return moment(data).format("DD/MM/YYYY HH:mm:ss");
                    }
                },
                {
                    data: "fullName",
                    render: function (data: any, type: any, row: any) {
                        return "<a href='/edit-account/" + row.id + "'>" + data + " </a>"
                    }
                },
                { data: "email" },
                { data: "status" },
                { data: "gender" },
            ],
            "lengthMenu": [[10, 25, 50], [10, 25, 50]],
            //scrollY: 200
        });
    }

    async componentDidMount() {
        if (!CheckAuthentication.IsSigning())
            return;
        this.$element = $(this.el);
        this.LoadData(this.$element);
    }
    componentWillUnmount() {
        if (this.$element.Datatable) {
            this.$element.Datatable.destroy();
        }
    }

    handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        if (CheckAuthentication.IsSigning())
            this.dataTable.ajax.reload();

    }

    handleSearch = (e: React.FormEvent<HTMLSelectElement>) => {
        e.preventDefault();
        if (CheckAuthentication.IsSigning())
            this.dataTable.ajax.reload();
    }

    createAccount = ()=>{

        this.setState({ ...this.state, isRedirectToCreate: true });
        //this.isRedirectToCreate = true;
        //let path = `/create-account`;
        //let history = useHistory();
        //history.push(path);
    }

    render() {
        if (!CheckAuthentication.IsSigning())
            return <Redirect to="/Login" />
        if (this.state.isRedirectToCreate)
            return <Redirect to="/create-account" />
        return (
            <div>
                <div className="card">
                    <div className="card-body">
                        <form onSubmit={this.handleSubmit} >
                            <div className="form-group row">
                                <label htmlFor="name" className="col-sm-3 col-md-1 col-form-label">
                                    Name
                                </label>
                                <div className="col-sm-9 col-md-3">
                                    <input id="name" className="form-control" name="name"/>
                                </div>

                                <label htmlFor="from" className="col-sm-3 col-md-1 col-form-label">
                                    From
                                </label>
                                <div className="col-sm-9 col-md-3">
                                    <input id="from" className="form-control" name="from" type="date" />
                                </div>

                                <label htmlFor="to" className="col-sm-3 col-md-1 col-form-label">
                                    To
                                </label>
                                <div className="col-sm-9 col-md-3">
                                    <input id="to" className="form-control" name="to" type="date" />
                                </div>

                            </div>
                            <div className="form-group row">
                                <label htmlFor="email" className="col-sm-3 col-md-1 col-form-label">
                                    Email
                                </label>
                                <div className="col-sm-9 col-md-3">
                                    <input id="email" className="form-control" name="email" />
                                </div>

                                <label htmlFor="phone" className="col-sm-3 col-md-1 col-form-label">
                                    Phone
                                </label>
                                <div className="col-sm-9 col-md-3">
                                    <input id="phone" className="form-control" name="phone" />
                                </div>

                                <label htmlFor="status" className="col-sm-3 col-md-1 col-form-label">
                                    Status
                                </label>
                                <div className="col-sm-9 col-md-3">
                                    <select className="form-control" name="status" id="status" onChange={this.handleSearch} >
                                        <option value="">All</option>
                                        <option value="Active">Active</option>
                                        <option value="Inactive">Inactive</option>
                                        <option value="Locked">Locked</option>
                                        <option value="Closed">Closed</option>
                                    </select>
                                </div>
                            </div>
                            <div className="form-group row">
                                <div className="col-sm-12 text-right">
                                    <button type="submit" className="btn btn-primary mr-2"><i className="ik ik-search" />Search</button>
                                    <button type="reset" className="btn btn-success mr-2"><i className="ik ik-refresh-cw"/> Refresh</button>
                                    <button type="button" className="btn btn-info mr-2" onClick={this.createAccount}><i className="ik ik-plus" />
                                        Add New
                                    </button>
                                </div>
                            </div>
                        </form>
                    </div>
                </div>
                <table id="example" className="table table-bordered table-hover" style={{ width: "100%" }} ref={el => this.el = el}>
                    <thead>
                        <tr>
                            <th>Created Date</th>
                            <th>Full name</th>
                            <th>Email</th>
                            <th>Status</th>
                            <th>Gender</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            this.state.userData.map((x: any) =>
                                <Row row={x} />
                            )
                        }

                    </tbody>
                </table>

            </div>
        );
    }
}