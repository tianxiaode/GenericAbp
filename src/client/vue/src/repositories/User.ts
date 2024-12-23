import { EntityInterface, http, isGranted, logger, Repository, permission } from "../libs";

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

    loadAdditionalData = async (id: string | number): Promise<any> =>{
        logger.debug(this, '[loadAdditionalData]', 'Loading additional data for user', id);
        const roles = await this.getRoles(id);
        return { roleNames: roles.items.map( (r:any) => r.name) };
    }

    getRoles(id: string | number){
        return http.get(this.getUrl() + `/${id}/roles`);
    }

    updateRoles(id: string, roles: string[]){
        return http.put(this.getUrl() + `/${id}/roles`, { roleName: roles });
    }

    getPermissions = async (providerKey: string) => {
        return permission.get("U", providerKey);
    }

    updatePermissions = async (providerKey: string, permissions: any) => {
        return permission.update("U", providerKey, permissions);
    }

}


