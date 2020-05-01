import { Action,Reducer } from 'redux'


export interface AuthenTicatedState {
    authenticated : boolean,
    name: string
}

export const AuthenticatedReducer : Reducer<AuthenTicatedState> = (state: AuthenTicatedState | undefined,action:Action) : AuthenTicatedState => {
        return {authenticated : true, name: 'Nguyen Minh Thu'};
}
