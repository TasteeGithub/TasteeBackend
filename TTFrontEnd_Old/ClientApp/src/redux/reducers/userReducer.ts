import { SET_USER, SET_AUTHENTICATED, SET_UNAUTHENTICATED, LOADING_USER } from '../types'

const initialState = {
    authenticated: false,
    credentials: {},
    loading: false
}

export default function (state = initialState,action:any) {
    switch (action.type) {
        case SET_AUTHENTICATED:
            return {
                ...state, authenticated: true
            };
        case SET_UNAUTHENTICATED:
            return state;
        case SET_USER:
            return {
                authenticated: true,
                loading: false,
                ...action.payload
            }
        default:
    }
}