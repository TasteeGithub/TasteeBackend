import axios from 'axios';

export const CheckAuthentication = {
    isAuthenticated: false,
    authenError:"",
    Authenticate(cb: (...args: any[]) => void) {
        axios.post("/api/Accounts/Login", {
            email: "thunm@sendo.vn",
            password: "123123",
            rememberme: false,
            returnurl: "http://abc.com"
        }).then((response) => {

            if (response.data.successful) {
                const token = `Bearer ${response.data.token}`;
                localStorage.setItem('token', `Bearer ${response.data.token}`);//setting token to local storage
                axios.defaults.headers.common['Authorization'] = token;//setting authorize token to header in axios

                this.isAuthenticated = true;
                setTimeout(cb, 1); //Fake Asynch
                console.log(response.data.token);
            }
            else {
                this.authenError = response.data.error;
                console.log(response.data.error);
            }
        }).catch((e) => console.log(e));
    }
    ,
    Sigout(cb: (...args: any[]) => void) {
        this.isAuthenticated = false;
        setTimeout(cb, 1); //fake asynch
            
    }
}