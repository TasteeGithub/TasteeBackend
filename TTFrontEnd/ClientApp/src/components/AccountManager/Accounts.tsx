import React, { Component } from 'react';
import axios from 'axios';
const $ = require('jquery');
require('datatables.net-bs4');
$.DataTable = require('datatables.net');

export default class Accounts extends Component {
    el: any;
    $element: any;
    async componentDidMount() {
        //console.log(this.el);

        let rs = await axios.get("/api/Accounts");
        const dataSet = rs.data.listData;

        this.$element = $(this.el);

        this.$element.DataTable({
            data: dataSet,
            columns: [
                {
                    title: "My Id",
                    data: "id",
                    render: function (data: any, type: any, row: any) {
                        return "<a href='http://xyz.b/?page=" + data + "'>" + row.id + " </a>"
                    }
                },
                {
                    title: "Full name", data: "fullName"
                },
                { title: "Email", data: "email" },
                { title: "Created Date", data: "createdDate" },
                { title: "Is Locked", data: "isLocked" },
                {
                    title: "Status", data: "status",
                    render: (data: any, type: any, row: any, meta: any) => {
                        return '<label class="badge badge-success">' + data + '</label>';
                    }
                },
            ],
            "lengthMenu": [[2, 10, 25, 50, -1], [2, 10, 25, 50, "All"]],
            scrollY: 200
        });
    }
    componentWillUnmount() {
        if (this.$element.Datatable) {
            this.$element.Datatable.destroy();
        }
    }
    
    render() {
        return (
            <div>
                <table className="table table-bordered table-hover" ref={el => this.el = el}>
                </table>
            </div>
        );
    }
}