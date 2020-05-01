import React from 'react';
import {AuthenticatedState} from '../../store/Login';
import {ApplicationState} from '../../store/index';
import { connect } from 'react-redux';

class LoginStatus extends React.Component<AuthenticatedState>{

    public render(){
        return (
            <div className="row">
                <div className="col-md-6">
                    <h2>
                        IsAuthend: {this.props.authenticated?'true':'false'}
                    </h2>
                    <h2>
                        Name: {this.props.name}
                    </h2>
                </div>
            </div>
        );
    }
}

const mapStateToProps = (state: ApplicationState) => state.Authentication


export default connect(mapStateToProps)(LoginStatus);