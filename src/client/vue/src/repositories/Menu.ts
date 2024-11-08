import { EntityInterface, http, isGranted, logger, Repository } from "../libs";

export interface MenuType extends EntityInterface {
    code: String,
    name: string,
    parentId: string,
    leaf: boolean,
    icon: string,
    isEnabled: boolean,
    isStatic: boolean,
    order: number,
    router: string,
    groupName: string,
    parent: Record<string, any>,
    multiLingual: Record<string, string>,
    children: MenuType[],
    hasChildren?: boolean
}

export class MenuRepository extends Repository<MenuType> {

    $className = 'MenuRepository';

    initialize() {
        logger.debug(this,'[initialize]', 'MenuRepository initialized')
        this.entity = 'Menu';
        this.resourceName = "MenuManagement";
        this.entityGroup = "";  
        this.messageField = "name";
        this.sortField = 'order'
        this.sortOrder = 'asc'
    };

    get canManagePermissions(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}`);
    }


    getByGroup = (group: string) =>{ 
        return http.get(this.getUrl() + `/by-group/${group}`);
    }

    getAllGroups = () =>{
        return http.get(this.getUrl() + `/groups`);
    }

    getMultiLingual = (id: string) =>{
        return http.get(this.getUrl() + `/${id}/multi-lingual`);
    }

    updateMultiLingual = (id: string, multiLingual: Record<string, string>) =>{
        return http.put(this.getUrl() + `/${id}/multi-lingual`, multiLingual);
    }

    getPermissions = (id: string) =>{
        return http.get(this.getUrl() + `/${id}/permissions`);
    }

    updatePermissions = (id: string, permissions: Record<string, boolean>) =>{  
        return http.put(this.getUrl() + `/${id}/permissions`, permissions);
    }

}


