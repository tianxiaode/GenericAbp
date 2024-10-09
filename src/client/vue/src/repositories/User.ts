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
        return http.get(this.getUrl('GET') + `/${id}/roles`);
    }

    updateRoles(id: string, roles: string[]){
        return http.put(this.getUrl('PUT') + `/${id}/roles`, { roleName: roles });
    }


}

export const userApi = new UserRepository({
    entity: "users",
    resourceName: "AbpIdentity",
    entityGroup: 'identity',
    messageField: 'userName',
    // 其他配置可以在这里添加，例如 apiPrefix 等
});

