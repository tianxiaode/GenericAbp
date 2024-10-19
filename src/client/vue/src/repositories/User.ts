import { EntityInterface, http, Repository } from "../libs";

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

class UserRepository extends Repository<UserType> {

    getRoles(id: string,){
        return http.get(this.getUrl() + `/${id}/roles`);
    }

    updateRoles(id: string, roles: string[]){
        return http.put(this.getUrl() + `/${id}/roles`, { roleName: roles });
    }


}


