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
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
var __generator = (this && this.__generator) || function (thisArg, body) {
    var _ = { label: 0, sent: function() { if (t[0] & 1) throw t[1]; return t[1]; }, trys: [], ops: [] }, f, y, t, g;
    return g = { next: verb(0), "throw": verb(1), "return": verb(2) }, typeof Symbol === "function" && (g[Symbol.iterator] = function() { return this; }), g;
    function verb(n) { return function (v) { return step([n, v]); }; }
    function step(op) {
        if (f) throw new TypeError("Generator is already executing.");
        while (_) try {
            if (f = 1, y && (t = op[0] & 2 ? y["return"] : op[0] ? y["throw"] || ((t = y["return"]) && t.call(y), 0) : y.next) && !(t = t.call(y, op[1])).done) return t;
            if (y = 0, t) op = [op[0] & 2, t.value];
            switch (op[0]) {
                case 0: case 1: t = op; break;
                case 4: _.label++; return { value: op[1], done: false };
                case 5: _.label++; y = op[1]; op = [0]; continue;
                case 7: op = _.ops.pop(); _.trys.pop(); continue;
                default:
                    if (!(t = _.trys, t = t.length > 0 && t[t.length - 1]) && (op[0] === 6 || op[0] === 2)) { _ = 0; continue; }
                    if (op[0] === 3 && (!t || (op[1] > t[0] && op[1] < t[3]))) { _.label = op[1]; break; }
                    if (op[0] === 6 && _.label < t[1]) { _.label = t[1]; t = op; break; }
                    if (t && _.label < t[2]) { _.label = t[2]; _.ops.push(op); break; }
                    if (t[2]) _.ops.pop();
                    _.trys.pop(); continue;
            }
            op = body.call(thisArg, _);
        } catch (e) { op = [6, e]; y = 0; } finally { f = t = 0; }
        if (op[0] & 5) throw op[1]; return { value: op[0] ? op[1] : void 0, done: true };
    }
};
Object.defineProperty(exports, "__esModule", { value: true });
var React = require("react");
var axios_1 = require("axios");
var react_router_1 = require("react-router");
;
var CreateAccount = /** @class */ (function (_super) {
    __extends(CreateAccount, _super);
    function CreateAccount(props) {
        var _this = _super.call(this, props) || this;
        _this.accountInfo = {
            birthday: "",
            email: "",
            fullName: "",
            password: "",
            phoneNumber: "",
            address: "",
            rePassword: "",
            gender: "",
        };
        _this.handleSubmit = function (e) {
            e.preventDefault();
        };
        _this.CreateAccount = function () { return __awaiter(_this, void 0, void 0, function () {
            var accountModel, resp;
            return __generator(this, function (_a) {
                switch (_a.label) {
                    case 0:
                        accountModel = {
                            email: this.accountInfo.email,
                            password: this.accountInfo.password,
                            confirmPassword: this.accountInfo.rePassword,
                            fullName: this.accountInfo.fullName,
                            phoneNumber: this.accountInfo.phoneNumber,
                            isLocked: false,
                            birthday: this.accountInfo.birthday,
                            gender: this.accountInfo.gender,
                            address: this.accountInfo.address,
                            role: "User",
                            userLevel: 1,
                            merchantLevel: 10,
                            avatar: "xyz",
                            status: "Pending"
                        };
                        return [4 /*yield*/, axios_1.default.post("https://localhost:44354/api/accounts", accountModel)];
                    case 1:
                        resp = _a.sent();
                        alert(resp.data.successful);
                        if (resp.data.successful)
                            return [2 /*return*/, React.createElement(react_router_1.Redirect, { to: { pathname: '/accounts' } })];
                        else
                            alert(resp.data.error);
                        return [2 /*return*/];
                }
            });
        }); };
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
                case "address":
                    _this.accountInfo.address = e.currentTarget.value;
                    break;
                case "birthday":
                    _this.accountInfo.birthday = e.currentTarget.value;
                    break;
                case "radioGender":
                    _this.accountInfo.gender = e.currentTarget.value;
                    _this.setState({
                        selectedOption: e.currentTarget.value
                    });
                    break;
            }
        };
        _this.state = { selectedOption: "Female" };
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
                            React.createElement("input", { name: "password", type: "password", className: "form-control", id: "exampleInputPassword2", placeholder: "Password", onChange: this.handleChange }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "exampleInputConfirmPassword2", className: "col-sm-3 col-form-label" }, "Re Password"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "rePassword", type: "password", className: "form-control", id: "exampleInputConfirmPassword2", placeholder: "Password", onChange: this.handleChange }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "exampleInputMobile", className: "col-sm-3 col-form-label" }, "Phone"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "phone", type: "text", className: "form-control", id: "exampleInputMobile", placeholder: "Mobile number", onChange: this.handleChange }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "inputAddress", className: "col-sm-3 col-form-label" }, "Phone"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "address", type: "text", className: "form-control", id: "inputAddress", placeholder: "Address", onChange: this.handleChange }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { htmlFor: "inputBithday", className: "col-sm-3 col-form-label" }, "Birthday"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { name: "birthday", type: "date", className: "form-control", id: "inputBithday", placeholder: "Birthday", onChange: this.handleChange }))),
                    React.createElement("div", { className: "form-radio row" },
                        React.createElement("label", { className: "col-sm-3 col-form-label" }, "Gender"),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("div", { className: "radio radio-inline" },
                                React.createElement("label", null,
                                    React.createElement("input", { type: "radio", value: "Female", name: "radioGender", checked: this.state.selectedOption === "Female", onChange: this.handleChange }),
                                    React.createElement("i", { className: "helper" }),
                                    "Female")),
                            React.createElement("div", { className: "radio radio-inline" },
                                React.createElement("label", null,
                                    React.createElement("input", { type: "radio", value: "Male", name: "radioGender", checked: this.state.selectedOption === "Male", onChange: this.handleChange }),
                                    React.createElement("i", { className: "helper" }),
                                    "Male")))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { className: "col-sm-3 col-form-label", htmlFor: "inputavatar" }),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("input", { type: "file", id: "inputavatar", name: "avatar" }))),
                    React.createElement("div", { className: "form-group row" },
                        React.createElement("label", { className: "col-sm-3 col-form-label" }),
                        React.createElement("div", { className: "col-sm-9" },
                            React.createElement("button", { type: "submit", onClick: this.CreateAccount, className: "btn btn-primary mr-2" }, "OK")))))));
    };
    return CreateAccount;
}(React.PureComponent));
exports.default = CreateAccount;
//# sourceMappingURL=CreateAccount.js.map