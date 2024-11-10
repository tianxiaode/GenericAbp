export interface EntityInterface {
    id: string | number | null;
    tenantId: string;
    concurrencyStamp: string;
    extraProperties?: Record<string, any>;
    creationTime?: string;
    creatorId?: string;
    lastModificationTime?: string;
    lastModifierId?: string;
    isDeleted: boolean;
    deleterId: string;
    deletionTime: string;
    entityVersion: Number;
}

export interface ListResult<T extends EntityInterface> {
    items: T[];
}

export interface PagedListResult<T extends EntityInterface>
    extends ListResult<T> {
    totalCount: number;
}

export type BeforeLoadType = (url: string,method: string,params: any) => boolean;
export type AfterLoadType<T> = (records: T[], total: number) => T[];
export type SendFunctionType = (url: string,method: string,data?: any,params?: any) => Promise<any>;
export type BeforeCreateType<T> = (entity: T) => boolean;
export type AfterCreateType<T> = (entity: T) => void;
export type BeforeUpdateType<T> = (entity: T) => boolean;
export type AfterUpdateType<T> = (entity: T) => void;
export type BeforeDeleteType = (id: string | number) => boolean;
export type AfterDeleteType = (id: string | number) => void;
export type BeforeBatchDeleteType = (ids: (string | number)[]) => boolean;
export type AfterBatchDeleteType = (ids: (string | number)[]) => void;
export type DeleteConfirmHandlerType = (message: (string | number)[], isTree: boolean) => Promise<boolean>;
export type MessageHandlerType = (message: string, type: string) => void;

export interface RepositoryGlobalConfigType extends Record<string, any> {
    api?:{
        prefix?: string;
        method?:{
            create?: string;
            read?: string;
            update?: string;
            delete?: string
        }
    },
    paging:{
        sizes: number[];
        pageParamName?: string;
        pageSizeParamName?: string;
        skipCountParamName?: string;
    },
    sorting?:{
        sortParamName?: string;
        orderParamName?: string;
    }
    useQueryParamForId?: boolean;
    useCache?: boolean;
    filterParamName?: string;
    idFieldName?: string;
    idParamName?: string;
    send?: SendFunctionType;
    deleteConfirmHandler?: DeleteConfirmHandlerType;
    messageHandler?: MessageHandlerType;
}

export interface RepositoryConfig<T extends EntityInterface>
    extends RepositoryGlobalConfigType {
    entity?: string;
    entityGroup?: string;
    resourceName?: string;
    createUrl?: string;
    readUrl?: string;
    updateUrl?: string;
    deleteUrl?: string;
    createTitle?: string;
    updateTitle?: string;
    pageSize?: number;
    messageFieldName?: string;
    localFilter?: boolean;
    remoteFilter?: boolean;
    remoteSort?: boolean;
    localFilterFields?: string[];
    beforeLoad?: BeforeLoadType;
    afterLoad?: AfterLoadType<T>;
    beforeCreate?: BeforeCreateType<T>;
    afterCreate?: AfterCreateType<T>;
    beforeUpdate?: BeforeUpdateType<T>;
    afterUpdate?: AfterUpdateType<T>;
    beforeDelete?: BeforeDeleteType;
    afterDelete?: AfterDeleteType;
    beforeBatchDelete?: BeforeBatchDeleteType;
    afterBatchDelete?: AfterBatchDeleteType;
}
