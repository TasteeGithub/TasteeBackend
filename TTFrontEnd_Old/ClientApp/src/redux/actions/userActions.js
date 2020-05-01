"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
//in useActions.ts file
var types_1 = require("../types");
var axios_1 = require("axios");
exports.loginUser = function (userData, history) { return function (dispatch) {
    dispatch({ type: types_1.LOADING_UI });
    axios_1.default.post('login', userData)
        .then(function (res) {
        var token = "Bearer " + res.data.token;
        localStorage.setItem('token', "Bearer " + res.data.token); //setting token to local storage
        axios_1.default.defaults.headers.common['Authorization'] = token; //setting authorize token to header in axios
        dispatch(exports.getUserData());
        dispatch({ type: types_1.CLEAR_ERRORS });
        console.log('success');
        history.push('/'); // redirecting to index page after login success
    })
        .catch(function (err) {
        console.log(err);
        dispatch({
            type: types_1.SET_ERRORS,
            payload: err.response.data
        });
    });
}; };
//for fetching authenticated user information
exports.getUserData = function () { return function (dispatch) {
    dispatch({ type: types_1.LOADING_USER });
    axios_1.default.get('/user')
        .then(function (res) {
        console.log('user data', res.data);
        dispatch({
            type: types_1.SET_USER,
            payload: res.data
        });
    }).catch(function (err) {
        console.log(err);
    });
}; };
exports.logoutUser = function () { return function (dispatch) {
    localStorage.removeItem('token');
    delete axios_1.default.defaults.headers.common['Authorization'];
    dispatch({
        type: types_1.SET_UNAUTHENTICATED
    });
    window.location.href = '/login'; // redirect to login page
}; };
//# sourceMappingURL=userActions.js.map