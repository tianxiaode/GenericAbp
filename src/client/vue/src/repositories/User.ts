import { EntityInterface, http, isGranted, logger, Repository } from "../libs";

export interface UserType extends EntityInterface {
    userName: String,
    name: string,
    surname: string,
    email: string,
    emailConfirmed: Boolean,
    phoneNumber: string,
    phoneNumberConfirmed: Boolean,
    isActive: Boolean,
    lockoutEnabled: Boolean,
    accessFailedCount: Number,
    lockoutEnd: String,
    lastPasswordChangeTime: String,
    password?: String,
    confirmPassword?: String,
    roleNames?: String[]
}

export class UserRepository extends Repository<UserType> {

    $className = 'UserRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'UserRepository initialized')
        this.entity = 'user';
        this.resourceName = "AbpIdentity";
        this.entityGroup = "identity";  
        this.messageField = "userName";
    };

    get canManagePermissions(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}.ManagePermissions`);
    }

    get canManageRoles(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}.update.ManageRoles`);
    }

    getRoles(id: string,){
        return http.get(this.getUrl() + `/${id}/roles`);
    }

    updateRoles(id: string, roles: string[]){
        return http.put(this.getUrl() + `/${id}/roles`, { roleName: roles });
    }

}


