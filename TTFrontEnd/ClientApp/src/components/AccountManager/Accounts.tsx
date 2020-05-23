import React, { Component } from 'react';
import moment from 'moment';
import axios from 'axios';
import { Link } from 'react-router-dom';

const $ = require('jquery');
require('datatables.net-bs4');
$.DataTable = require('datatables.net');

type DataState = {
    userData:[]
}

type RowProp = {
    row:any
}
    
const Row: React.FunctionComponent<RowProp> = (props:RowProp) => {
    return (
        <tr>
            <td>{props.row.createdDate}</td>
            <td>
                <Link to={`/edit-account/${props.row.id}`}>{props.row.fullName}</Link>
            </td>
            <td>{props.row.email}</td>
            <td>{props.row.isLocked===true?"True":"False"}</td>
            <td>{props.row.status}</td>
            <td>{props.row.gender}</td>
        </tr>
        )
}

export default class Accounts extends Component<{}, DataState> {
    constructor(props: any) {
        super(props);
        this.state = { userData: [] }
    }
    el: any;
    $element: any;
    async componentDidMount() {
        let rs = await axios.get("https://localhost:44354/api/Accounts");
        const dataSet = rs.data.listData;
        this.setState({ userData: dataSet });

        this.$element = $(this.el);
        this.$element.DataTable({
            "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]],
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
                <table id="example" className="table table-bordered table-hover" style={{width:"100%"}} ref={el => this.el = el}>
                    <thead>
                        <tr>
                            <th>Created Date</th>
                            <th>Full name</th>
                            <th>Email</th>
                            <th>Is Locked</th>
                            <th>Status</th>
                            <th>Gender</th>
                        </tr>
                    </thead>
                    <tbody>
                        {
                            this.state.userData.map((x: any) =>
                                <Row row={x}/>
                                )
                        }
                        
                        

                    </tbody>
                </table>

            </div>
        );
    }
}