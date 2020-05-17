import React, { Component } from 'react';
import axios from 'axios';
const $ = require('jquery');
require('datatables.net-bs4');
$.DataTable = require('datatables.net');

export default class Accounts extends Component {
    el: any;
    $element: any;
    async componentDidMount() {

        let rs = await axios.get("/api/Accounts");
        const dataSet = rs.data.listData;

        this.$element = $(this.el);

        this.$element.DataTable({
            data: dataSet,
            columns: [
                {
                    title: "Created Date",
                    data: "createdDate"
                },
                {
                    title: "Full name",
                    data: "fullName",
                    render: function (data: any, row: any) {
                        return "<a href='http://xyz.b/?id=" + row.id + "'>" + data + " </a>"
                    }
                },
                { title: "Email", data: "email" },
                
                {
                    title: "Is Locked",
                    data:"isLocked",
                    render: (data: any) => {
                        return "<div class='text-center'><i class='ik ik-" + (data? "lock text-red" : "unlock text-green") + "'></i></div>"
                    } 
                },
                {
                    title: "Status", data: "status",
                    render: (data: any) => {
                        return '<label class="badge badge-success">' + data + '</label>';
                    }
                },
                {
                    title: "Gender",
                    data: "gender"
                }
            ],
            "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
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