"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var CreateAccount = /** @class */ (function (_super) {
    __extends(CreateAccount, _super);
    function CreateAccount() {
        var _this = _super !== null && _super.apply(this, arguments) || this;
        _this.accountInfo = {
            birthday: "",
            email: "",
            fullName: "",
            password: "",
            phoneNumber: "",
            rePassword: ""
        };
        _this.handleSubmit = function (e) {
            e.preventDefault();
        };
        _this.CreateAccount = function () {
            alert(_this.accountInfo.fullName);
            ;
        };
        _this.handleChange = function (e) {
            switch (e.currentTarget.name) {
                case "email":
                    _this.accountInfo.email = e.currentTarget.value;
                    break;
                case "fullName":
                    _this.accountInfo.fullName = e.currentTarget.value;
                    break;
                case "password":
                    _this.accountInfo.password = e.currentTarget.value;
                    break;
                case "rePassword":
                    _this.accountInfo.rePassword = e.currentTarget.value;
                    break;
                case "phone":
                    _this.accountInfo.phoneNumber = e.currentTarget.value;
                    break;
                case "birthday":
                    _this.accountInfo.birthday = e.currentTarget.value;
                    break;
            }
        };
        return _this;
    }
    CreateAccount.prototype.render = function () {
        return (React.createElement("div", { className: "card" },
            React.createElement("div", { className: "card-body" },
                React.createElement("form", { className: "forms-sample", onSubmit: this.handleSubmit },
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "exampleInputEmail2", className: "col-sm-3 col-form-label" }, "Email"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "email", type: "email", className: "form-control", id: "exampleInputEmail2", placeholder: "Email", onChange: this.handleChange }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "exampleInputUsername2", className: "col-sm-3 col-form-label" }, "Full name"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "fullName", type: "text", className: "form-control", id: "exampleInputUsername2", placeholder: "Full name", onChange: this.handleChange }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "exampleInputPassword2", className: "col-sm-3 col-form-label" }, "Password"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "password", type: "password", className: "form-control", id: "exampleInputPassword2", placeholder: "Password" }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "exampleInputConfirmPassword2", className: "col-sm-3 col-form-label" },
                            "Re Password ",
                            this.accountInfo.fullName),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "rePassword", type: "password", className: "form-control", id: "exampleInputConfirmPassword2", placeholder: "Password" }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "exampleInputMobile", className: "col-sm-3 col-form-label" }, "Phone"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "phone", type: "text", className: "form-control", id: "exampleInputMobile", placeholder: "Mobile number" }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "inputBithday", className: "col-sm-3 col-form-label" }, "Birthday"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "birthday", type: "date", className: "form-control", id: "inputBithday", placeholder: "Birthday" }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { className: "col-sm-3 col-form-label" }),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("button", { type: "submit", onClick: this.CreateAccount, className: "btn btn-primary mr-2" }, "OK")))))));
    };
    return CreateAccount;
}(React.Component));
exports.default = CreateAccount;
//# sourceMappingURL=CreateAccount.js.map