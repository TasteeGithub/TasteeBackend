"use strict";
var __assign = (this && this.__assign) || function () {
    __assign = Object.assign || function(t) {
        for (var s, i = 1, n = arguments.length; i < n; i++) {
            s = arguments[i];
            for (var p in s) if (Object.prototype.hasOwnProperty.call(s, p))
                t[p] = s[p];
        }
        return t;
    };
    return __assign.apply(this, arguments);
};
Object.defineProperty(exports, "__esModule", { value: true });
var types_1 = require("../types");
var initialState = {
    loading: false,
    errors: null
};
function default_1(state, action) {
    if (state === void 0) { state = initialState; }
    switch (action.type) {
        case types_1.SET_ERRORS:
            return __assign(__assign({}, state), { loading: false, errors: action.payload });
        case types_1.CLEAR_ERRORS:
            return __assign(__assign({}, state), { loading: false, errors: null });
        case types_1.LOADING_UI:
            return __assign(__assign({}, state), { loading: true });
        default:
            return state;
    }
}
exports.default = default_1;
//# sourceMappingURL=uiReducer.js.map