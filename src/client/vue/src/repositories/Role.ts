import { EntityInterface, http, Repository } from "../libs";

export interface RoleType extends EntityInterface {
  name: string;
  isDefault: boolean,
  isStatic: boolean,
  isPublic: boolean,
}

class RoleRepository extends Repository<RoleType> {
    newTitle = 'AbpIdentity.NewRole'
    editTitle = 'AbpIdentity.EditRole'

    getAll(){
        return http.get(this.getUrl('GET') + '/all');
    }


}

export const roleApi = new RoleRepository({
    entity: "roles",
    resourceName: "AbpIdentity",
    entityGroup: 'identity',
    messageField: 'name',
    // 其他配置可以在这里添加，例如 apiPrefix 等
});

