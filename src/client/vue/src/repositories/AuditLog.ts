import { EntityInterface, http, isGranted, logger, Repository } from "../libs";

export interface AuditLogType extends EntityInterface {
    applicationName: String,
    userId: string,
    userName: string,
    tenantName: string,
    clientId: string,
    correlationId: string,
    clientIpAddress: string,
    browserInfo: string,
    impersonatorUserName: string,
    impersonatorUserId: string,
    impersonatorTenantId: string,
    impersonatorTenantName: string,
    executionTime: string,
    executionDuration: number,
    clientName: string,
    httpMethod: string,
    url: string,
    exceptions: string,
    comments: string,
    httpStatusCode: number,
    actions: string,
}


export class AuditLogRepository extends Repository<AuditLogType> {

    $className = 'AuditLogRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'AuditLogRepository initialized')
        this.entity = 'AuditLog';
        this.resourceName = "AbpAuditLogging";
        this.entityGroup = "";  
        this.messageField = "url";
        this.labelPrefix = "AuditLog"
    };

    get canManagePermissions(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}`);
    }

    getAllApplicationNames = (filter?: string) =>{ 
        return http.get(this.getUrl() + `/application-names/?filter=${filter}`);
    }

    getAllUrls = (filter?: string) =>{
        return http.get(this.getUrl() + `/urls/?filter=${filter}`);
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


