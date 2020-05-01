"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var react_redux_1 = require("react-redux");
//import * as MyGameStore from '../store/MyGame';
var MyGameStore = require("../store/MyGame");
var core_1 = require("@material-ui/core");
var react_router_dom_1 = require("react-router-dom");
function MyGame(props) {
    var handleSubmit = function (e) {
        e.preventDefault();
        //setLoading(true);
        //your client side validation here
        //after success validation
        //const userData = {
        //    email: values.email,
        //    password: values.password,
        //};
        //props.loginUser(userData, props.history);
    };
    var handleChange = function (e) {
        //e.persist();
        //setValues(values => ({ ...values, [e.target.name]: e.target.value }));
    };
    return (React.createElement(core_1.Box, null,
        React.createElement(core_1.Box, null,
            React.createElement(core_1.Typography, { variant: "h4" },
                React.createElement(core_1.Box, { fontWeight: 600, letterSpacing: 3 }, "SIGN IN"))),
        React.createElement(core_1.Container, { component: "main", maxWidth: "md" },
            React.createElement(core_1.CssBaseline, null),
            React.createElement(core_1.Grid, { container: true, alignContent: "center", alignItems: "center", justify: "center", spacing: 3 },
                React.createElement(core_1.Grid, { item: true, md: 3 },
                    React.createElement("img", { alt: "My Logo" })),
                React.createElement(core_1.Grid, { item: true, md: 9 },
                    React.createElement(core_1.Paper, null,
                        React.createElement(core_1.Box, null,
                            React.createElement(core_1.TextField, { variant: "outlined", margin: "none", fullWidth: true, id: "email", label: "Email Address", name: "email", type: "email", onChange: handleChange }),
                            React.createElement(core_1.TextField, { variant: "outlined", margin: "normal", value: "", fullWidth: true, name: "password", label: "Password", type: "password", onChange: handleChange }),
                            React.createElement(core_1.Grid, { container: true },
                                React.createElement(core_1.Grid, { item: true, sm: 6, md: 6 },
                                    React.createElement(core_1.FormControlLabel, { control: React.createElement(core_1.Checkbox, { value: "remember", color: "primary" }), label: "Remember me" })),
                                React.createElement(core_1.Grid, { item: true, sm: 6, md: 6 },
                                    React.createElement(react_router_dom_1.Link, { to: "#" }, "Forgot password?"))),
                            React.createElement(core_1.Button, { type: "submit", variant: "contained", color: "primary" }, "Login"))))))));
}
;
exports.default = react_redux_1.connect(function (state) { return state.mygamecounter; }, MyGameStore.actionCreators)(MyGame);
//# sourceMappingURL=MyGame.js.map