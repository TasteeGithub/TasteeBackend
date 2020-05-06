import axios from 'axios';

export const CheckAuthentication = {
    isAuthenticated: false,
    authenError: "",
    async Login(loginInfo: any) {

        let response = await axios.post("/api/Accounts/Login", {
            email: loginInfo.email,
            password: loginInfo.password
        });

        if (response.data.successful) {
            const token = `Bearer ${response.data.token}`;
            localStorage.setItem('token', `Bearer ${token}`);
            axios.defaults.headers.common['Authorization'] = token;
            this.isAuthenticated = true;
            console.debug(token)
        }
        else {
            this.authenError = response.data.error;
            console.log(response.data.error);
        }
    },

    IsSigning(): boolean {
        this.isAuthenticated = localStorage.token != null;
        return this.isAuthenticated;
    },

    Authenticate(cb: (...args: any[]) => void) {
        this.isAuthenticated = true;
    }
    ,
    Sigout(cb: (...args: any[]) => void) {
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization'];
        this.isAuthenticated = false;
    }
}