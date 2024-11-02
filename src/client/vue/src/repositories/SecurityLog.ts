import { EntityInterface, http, isGranted, logger, Repository } from "../libs";

export interface SecurityLogType extends EntityInterface {
    applicationName: String,
    identity: string,
    action: string,
    userId: string,
    userName: string,
    tenantName: string,
    clientId: string,
    correlationId: string,
    clientIpAddress: string,
    browserInfo: String,
}
export class SecurityLogRepository extends Repository<SecurityLogType> {

    $className = 'SecurityLogRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'SecurityLogRepository initialized')
        this.entity = 'SecurityLog';
        this.resourceName = "AbpIdentity";
        this.entityGroup = "";  
        this.messageField = "action";
    };

    get canManagePermissions(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}`);
    }

    getAllApplicationNames = (filter?: string) =>{ 
        return http.get(this.getUrl() + `/application-names/?filter=${filter}`);
    }

    getAllIdentities = (filter?: string) =>{
        return http.get(this.getUrl() + `/identities/?filter=${filter}`);
    }

    getAllActions = (filter?: string) =>{
        return http.get(this.getUrl() + `/actions/?filter=${filter}`);
    }

    getAllUserNames = (filter?: string) =>{
        return http.get(this.getUrl() + `/user-names/?filter=${filter}`);
    }

    getAllClientIds = (filter?: string) => {
        return http.get(this.getUrl() + `/client-ids/?filter=${filter}`);
    }

    getAllCorrelationIds =(filter?: string) =>{
        return http.get(this.getUrl() + `/correlation-ids/?filter=${filter}`);
    }


}


