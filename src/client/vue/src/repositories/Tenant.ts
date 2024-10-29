import { EntityInterface, http, isGranted, logger, Repository } from "../libs";

export interface TenantType extends EntityInterface {
  name: string;
}

export class TenantRepository extends Repository<TenantType> {
    $className = 'TenantRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'TenantRepository initialized')
        this.entity = 'tenant';
        this.resourceName = "AbpTenantManagement";
        this.entityGroup = "MultiTenancy";  
        this.messageField = "name";
    };


    getAll(){
        return http.get(this.readUrl + '/all');
    }


}

