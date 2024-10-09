import { EntityInterface, http, Repository } from "../libs";

export interface SiteType extends EntityInterface {
  name: string;
  townStreetName: string
}

class SiteRepository extends Repository<SiteType> {

    getTownStreets(filter: string){
        return http.get(this.getUrl('GET') + '/town-streets?filter=' + filter);
    }

    getAutocompleteList(filter: string){
        return http.get(this.getUrl('GET') + '/?filter=' + filter);
    }


}

export const siteApi = new SiteRepository({
    entity: "sites",
    resourceName: "PointToPoint",
    messageField: 'name',
    // 其他配置可以在这里添加，例如 apiPrefix 等
});

