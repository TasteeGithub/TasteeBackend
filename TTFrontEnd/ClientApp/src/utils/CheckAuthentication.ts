import axios from 'axios';

export const CheckAuthentication = {
    isAuthenticated: false,
    authenError: "",
    Login(cb: (...args: any[]) => void,loginInfo:any) {
        axios.post("/api/Accounts/Login", {
            email: loginInfo.email,
            password: loginInfo.password
        }).then((response) => {

            if (response.data.successful) {
                const token = `Bearer ${response.data.token}`;
                localStorage.setItem('token', `Bearer ${response.data.token}`);//setting token to local storage
                axios.defaults.headers.common['Authorization'] = token;//setting authorize token to header in axios
                this.isAuthenticated = true;
                console.log(response.data.token);
                setTimeout(cb, 100); //Fake Asynch
            }
            else {
                this.authenError = response.data.error;
                console.log(response.data.error);
                setTimeout(cb, 100); //Fake Asynch
            }
        }).catch((e) => console.log(e));
    },

    IsSigning(): boolean {
        this.isAuthenticated = localStorage.token != null;
        return this.isAuthenticated;
    },

    Authenticate(cb: (...args: any[]) => void) {
        this.isAuthenticated = true;
        setTimeout(cb, 100); //Fake Asynch
    }
    ,
    Sigout(cb: (...args: any[]) => void) {
        this.isAuthenticated = false;
        localStorage.removeItem('token');
        delete axios.defaults.headers.common['Authorization']
        setTimeout(cb, 100); //fake asynch
            
    }
}