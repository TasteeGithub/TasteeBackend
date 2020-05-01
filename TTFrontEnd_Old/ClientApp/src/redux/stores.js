"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var redux_1 = require("redux");
var redux_thunk_1 = require("redux-thunk");
var userReducer_1 = require("./reducers/userReducer");
var uiReducer_1 = require("./reducers/uiReducer");
var initialState = {};
var middleware = [redux_thunk_1.default];
var reducer = redux_1.combineReducers({
    user: userReducer_1.default,
    UI: uiReducer_1.default
});
var store = redux_1.createStore(reducer, initialState, redux_1.compose(redux_1.applyMiddleware.apply(void 0, middleware), (window.__REDUX_DEVTOOLS_EXTENSION__ &&
    window.__REDUX_DEVTOOLS_EXTENSION__())));
exports.default = store;
//# sourceMappingURL=stores.js.map