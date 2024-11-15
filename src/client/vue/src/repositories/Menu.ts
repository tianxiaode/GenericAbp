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
    multilingual: Record<string, string>,
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
        this.labelPrefix = 'Menu';
        this.isTree = true;
    };

    get canManagePermissions(): boolean {
        return isGranted(`${this.resourceName}.${this.entityPlural}`);
    }


    getShowList = (name: string) =>{ 
        return http.get(this.getUrl() + `/show/${name}`);
    }

    getAllGroups = () =>{
        return http.get(this.getUrl() + `/groups`);
    }

    getPermissions = (id: string) =>{
        return http.get(this.getUrl() + `/${id}/permissions`);
    }

    updatePermissions = (id: string, permissions: any) =>{  
        return http.put(this.getUrl() + `/${id}/permissions`, permissions);
    }

    move = (id: string, parentId: string) =>{
        return http.put(this.getUrl() + `/${id}/move/${parentId}`);
    }

    copy = (id: string, parentId: string) =>{
        return http.put(this.getUrl() + `/${id}/copy/${parentId}`);
    }


}


