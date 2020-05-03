
export const CheckAuthentication = {
    isAuthenticated: false,
    Authenticate(cb: (...args: any[]) => void) {
        this.isAuthenticated = true;
        setTimeout(cb, 1); //Fake Asynch
        
    }
    ,
    Sigout(cb: (...args: any[]) => void) {
        this.isAuthenticated = false;
        setTimeout(cb, 1); //fake asynch
            
    }
}