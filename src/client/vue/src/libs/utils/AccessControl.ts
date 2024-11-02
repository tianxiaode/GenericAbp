import { isEmpty, capitalize } from "./String";
import { appConfig } from "../AppConfig";


export const isGranted = (...args: string[]) => {
    const permissions = appConfig.permissions;
    if(isEmpty(permissions)) return false;
    let hasPermission = false;
    for (const permission of args) {
        //将每一个权限先根据.拆分，然后将每个部件头字母大写，再用.连接起来，最后从permissions中取出对应的权限
        const permissionArray = permission.split('.');
        let permissionName = '';
        for (const part of permissionArray) {
            permissionName += capitalize(part) + '.';
        }
        if(permissions[permissionName.slice(0, -1)]) {
            hasPermission = true;
            break;
        }    
    }
    return hasPermission;
};

