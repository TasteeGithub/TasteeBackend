import * as React from 'react'
import { useState, useEffect } from 'react';
import axios from 'axios';

export interface IRole {
    id: string,
    name: string
}

export interface IProps {
    SelectedRoleId:string,
    GetRole: (roleId:string)=> void
}

const Role: React.FunctionComponent<IProps> = (props:IProps) => {
    useEffect(() => {
        getRoleData();
    },[]);

    const [roles, setRoles] = useState([] as IRole[]);

    const getRoleData = async () => {
        const authToken = localStorage.token;
        if (authToken != null) {
            axios.defaults.headers.common['Authorization'] = authToken;
            const result = await axios.get(`/api/Roles/`);
            setRoles(result.data);
        }
    }
    
    
    const handleRoleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        props.GetRole(e.currentTarget.value);
        //alert(e.currentTarget.value);
    }
    return (
        <select className="form-control" name="selectRole" onChange={handleRoleChange}>
            {
                roles.map(role =>
                    (<option key={role.id} selected={role.id === props.SelectedRoleId} value={role.id}>{role.name}</option>)
                    )
            }
        </select>
        );
}

export default Role;