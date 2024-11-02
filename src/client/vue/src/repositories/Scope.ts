import { EntityInterface, http, logger, Repository } from "../libs";

export interface ScopeType extends EntityInterface {
  name: string;
  description: string,
  displayName: string,
  properties: Record<string, any>,
  resources: String[],
}

export class ScopRepository extends Repository<ScopeType> {
    $className = 'ScopRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'ScopRepository initialized')
        this.entity = 'scope';
        this.resourceName = "OpenIddict";
        this.entityGroup = "";  
        this.messageField = "name";
    };

    getAll(){
        return http.get(this.readUrl + '/all');
    }


}

