import { EntityInterface, Repository, http } from "../libs";
import { SiteType } from "./Site";

export interface PatientInfoType extends EntityInterface {
  name: string;
  gender: string;
  dateOfBirth: string;
  diagnosis: string;
  phoneNumber: string;
  address: string;
  siteId: string;
  site: SiteType
}

class PatientInfoRepository extends Repository<PatientInfoType> {

    getAutocompleteList(filter: string){
        return http.get(this.getUrl('GET') + '/?filter=' + filter);
    }
}


export const patientInfoApi = new PatientInfoRepository({
    entity: "patientInfos",
    resourceName: "PointToPoint",
    messageField: 'name',
    // 其他配置可以在这里添加，例如 apiPrefix 等
});

