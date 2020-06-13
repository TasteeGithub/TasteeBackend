import * as React from 'react'
import { RouteComponentProps } from 'react-router';

const Role: React.FunctionComponent = () => {

    const handleRoleChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
        alert(e.currentTarget.value);
    }
    return (
        <select className="form-control" name="selectRole" onChange={handleRoleChange}>
            <option value="Moderator">Moderator</option>
            <option value="Admin">Admin</option>
            <option value="SupperAdmin">Supper Admin</option>
        </select>
        );
}

export default Role;