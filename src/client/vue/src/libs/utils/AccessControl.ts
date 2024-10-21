import { isEmpty, uncapitalize } from "./String";
import { appConfig } from "../AppConfig";


export const isGranted = (
    resourceName: string,
    entity: string,
    permission: string = ''
) => {
    const permissions = appConfig.getPermissions(resourceName, entity);
    permission = uncapitalize(permission);
    if(isEmpty(permission)) return permissions['default'] || false;
    return permissions[permission] || false;
};

export const allowedRead = (resourceName: string, entity: string) => {
    return isGranted(resourceName, entity,'read');
};

export const allowedCreate = (resourceName: string, entity: string) => {
    return isGranted(resourceName, entity, 'create');
};

export const allowedEdit = (resourceName: string, entity: string) => {
    return isGranted(resourceName, entity, 'edit');
};

export const allowedDelete = (resourceName: string, entity: string) => {
    return isGranted(resourceName, entity, 'delete');
};