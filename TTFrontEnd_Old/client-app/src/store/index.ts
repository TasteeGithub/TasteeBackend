
import {AuthenTicatedState,AuthenticatedReducer} from './Login';

export interface ApplicationState {
    Authentication: AuthenTicatedState | undefined    
}

export const reducers = {
    Authentication: AuthenticatedReducer
};


export interface AppThunkAction<TAction> {
    (dispatch:(action:TAction) => void, getState: () => ApplicationState) : void;
}