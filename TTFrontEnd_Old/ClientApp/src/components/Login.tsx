import React from 'react';

//in Login.tsx inside components
import { loginUser } from '../redux/actions/userActions'

import { connect } from 'react-redux';
import Button from 'react-bootstrap/Button';

function Login(props: any) {

    return (
        <div>
            <Button variant="danger">Dangerous</Button>
        </div>
    )
}

// this map the state to our props in this functional component
const mapStateToProps = (state: any) => ({
    user: state.user,
    UI: state.UI
});

// this map actions to our props in this functional component
const mapActionsToProps = {
    loginUser
};
export default connect(mapStateToProps, mapActionsToProps)(Login)