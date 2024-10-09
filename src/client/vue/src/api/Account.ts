import { BaseHttp, globalConfig } from "../libs"

export class Account {
    $className: string = "Account";

    static async login(username: string, password: string) {
        try {
            const user = await globalConfig.userManager?.signinResourceOwnerCredentials({
                username: username,
                password: password
            })
            if(!user){
                throw new Error("Invalid username or password!");                
            }
            BaseHttp.setToken(user.access_token);
            await globalConfig.loadConfig();            
        } catch (error:any) {
            throw error;                        
        }
    }

    static async logout() {
        try {
            await globalConfig.userManager?.signoutSilent();
            BaseHttp.setToken(null as any);
            await globalConfig.loadConfig();
        } catch (error:any) {
            throw error;                        
        }
    }
}