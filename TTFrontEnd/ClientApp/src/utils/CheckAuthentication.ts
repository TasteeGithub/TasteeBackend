import axios from 'axios';
import jwtDecode from 'jwt-decode';

export interface LoginInfo {
    email: string,
    password: string
}

export const CheckAuthentication = {
    isAuthenticated: false,
    authenError: "",
    async Login(loginInfo: LoginInfo) {

        let response = await axios.post("/api/Accounts/Login", {
            email: loginInfo.email,
            password: loginInfo.password
        });

        if (response.data.successful) {
            const token = `Bearer ${response.data.token}`;
            localStorage.setItem('token', `Bearer ${token}`);
            axios.defaults.headers.common['Authorization'] = token;
            this.isAuthenticated = true;
        }
        else {
            this.authenError = response.data.error;
            console.log(response.data.error);
        }
    },

    IsSigning(): boolean {
        const authToken = localStorage.token;
        if (authToken != null) {
            const decodeToken: any = jwtDecode(authToken);
            if (decodeToken.exp * 1000 > Date.now()) {
                this.isAuthenticated = true;
            }
            else {
                this.Sigout();
            }
        }
        else {
            this.isAuthenticated = false;
        }

        return this.isAuthenticated;
    },
    Sigout() {
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization'];
        this.isAuthenticated = false;
    }
}