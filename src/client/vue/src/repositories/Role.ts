import { EntityInterface, http, isGranted, logger, Repository } from "../libs";

export interface RoleType extends EntityInterface {
  name: string;
  isDefault: boolean,
  isStatic: boolean,
  isPublic: boolean,
}

export class RoleRepository extends Repository<RoleType> {
    $className = 'RoleRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'RoleRepository initialized')
        this.entity = 'role';
        this.resourceName = "AbpIdentity";
        this.entityGroup = "identity";  
        this.messageField = "name";
    };

    get canManagePermissions(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}.ManagePermissions`);
    }

    getAll(){
        return http.get(this.readUrl + '/all');
    }


}

