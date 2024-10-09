import { EntityInterface, Repository } from "../libs";
import { PatientInfoType } from "./PatientInfo";

export interface MedicalHistoryType extends EntityInterface {
    patientId: string;
    medicalDate: string;
    medicationInfo: string;
    patientInfo: PatientInfoType;
}

export const medicalHistoryApi = new Repository<MedicalHistoryType>({
    entity: "medicalHistories",
    resourceName: "PointToPoint",
    messageField: "medicationInfo",
    // 其他配置可以在这里添加，例如 apiPrefix 等
});
