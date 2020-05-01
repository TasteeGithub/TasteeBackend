import { Action,Reducer } from 'redux'
import { SET_AUTHENTICATED,SET_UNAUTHENTICATED} from './types';

export interface AuthenTicatedState {
    authenticated : boolean,
    name: string
}

export interface LoginInfo {
    email: string,
    password: string
}

export interface SetAuthenticatedAction {
    type: typeof SET_AUTHENTICATED,
    loginInfo: LoginInfo
}

export interface SetUnAuthenticatedAction {
    type: typeof SET_UNAUTHENTICATED,
}


export type KnownAction = SetAuthenticatedAction | SetUnAuthenticatedAction;

export const actionCreator = {
    setAuthen: (loginInfo: LoginInfo) => ({type: SET_AUTHENTICATED,loginInfo:loginInfo } as SetAuthenticatedAction ),
    setUnAuthen: () => ({type: SET_UNAUTHENTICATED } as SetUnAuthenticatedAction)
}

export const AuthenticatedReducer : Reducer<AuthenTicatedState> = (state: AuthenTicatedState | undefined,action:Action) : AuthenTicatedState => {
    if(state === undefined) return {authenticated:false, name : "Un know"}
    const incomAction = action as KnownAction;
    switch (incomAction.type) {
        case SET_AUTHENTICATED:
            return {
                authenticated: true,
                name: incomAction.loginInfo.email
            }
        case SET_UNAUTHENTICATED:
            return {
                authenticated: false,
                name : "Logouted"
            }
        default:
            return state;
    }
}
