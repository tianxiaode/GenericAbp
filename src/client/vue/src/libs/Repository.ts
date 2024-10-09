import { BaseClass } from "./BaseClass";
import { http } from "./http";
import { camelCaseToDash, isEmpty, isObject, logger } from "./utils";

export interface EntityInterface {
    id: string | number | null;
    tenantId: string;
    concurrencyStamp: string;
    extraProperties?: Record<string, any>;
    creationTime?: string,
    creatorId?: string,
    lastModificationTime?: string,
    lastModifierId?: string,
    isDeleted: boolean,
    deleterId: string,
    deletionTime: string,
    entityVersion: Number,
  }

export interface ListResult<T extends EntityInterface> {
    items: T[];
}

export interface PagedListResult<T extends EntityInterface>
    extends ListResult<T> {
    totalCount: number;
}

export interface RepositoryGlobalConfig {
    apiPrefix?: string;
    createMethod?: string;
    readMethod?: string;
    updateMethod?: string;
    deleteMethod?: string;
    pageSizes?: number[];
    send?: SendFunctionType;
    useQueryParamForId?: boolean;
    sortFieldParamName?: string;
    sortOrderParamName?: string;
    useCache?: boolean;
    filterParamName?: string;
    pageKey?: string;
    pageSizeParamName?: string;
    skipCountParamName?: string;
    idKey?: string;
    deleteConfirmHandler: (message: string) => Promise<boolean>;
    messageHandler?: (message: string, type: string) => void;
}

export interface RepositoryConfig<T extends EntityInterface>
    extends Omit<RepositoryGlobalConfig, "deleteConfirmHandler"> {
    entity?: string;
    entityGroup?: string;
    resourceName?: string;
    createUrl?: string;
    readUrl?: string;
    updateUrl?: string;
    deleteUrl?: string;
    pageSize?: number;
    messageField?: string;
    localFilter?: boolean;
    remoteFilter?: boolean;
    remoteSort?: boolean;
    localFilterFields?: string[];
    beforeLoad?: BeforeLoadType | undefined;
    afterLoad?: AfterLoadType<T> | undefined;
    beforeCreate?: BeforeCreateType<T> | undefined;
    afterCreate?: AfterCreateType<T> | undefined;
    beforeUpdate?: BeforeUpdateType<T> | undefined;
    afterUpdate?: AfterUpdateType<T> | undefined;
    beforeDelete?: BeforeDeleteType | undefined;
    afterDelete?: AfterDeleteType | undefined;
    beforeBatchDelete?: BeforeBatchDeleteType | undefined;
    afterBatchDelete?: AfterBatchDeleteType | undefined;
}

export type BeforeLoadType = (
    url: string,
    method: string,
    params: any
) => boolean;
export type AfterLoadType<T> = (records: T[], total: number) => T[];
export type SendFunctionType = (
    url: string,
    method: string,
    data?: any,
    params?: any
) => Promise<any>;
export type BeforeCreateType<T> = (entity: T) => boolean;
export type AfterCreateType<T> = (entity: T) => void;
export type BeforeUpdateType<T> = (entity: T) => boolean;
export type AfterUpdateType<T> = (entity: T) => void;
export type BeforeDeleteType = (id: string | number) => boolean;
export type AfterDeleteType = (id: string | number) => void;
export type BeforeBatchDeleteType = (ids: (string | number)[]) => boolean;
export type AfterBatchDeleteType = (ids: (string | number)[]) => void;

export class Repository<T extends EntityInterface> extends BaseClass {
    $className = "Repository";
    static globalConfig: RepositoryGlobalConfig;
    static instance: Repository<any>;
    private cache = new Map<string, T[]>();
    private createUrl: string;
    private readUrl: string;
    private updateUrl: string;
    private deleteUrl: string;
    private createMethod: string;
    private readMethod: string;
    private updateMethod: string;
    private deleteMethod: string;
    private pageSizes: number[];
    private pageSize: number;
    private apiPrefix: string;
    private useQueryParamForId: boolean = false;
    private sortFieldParamName: string;
    private sortOrderParamName: string;
    private useCache: boolean;
    private filterParamName: string;
    private pageKey: string;
    private pageSizeParamName: string;
    private skipCountParamName: string;
    private idKey: string;
    private messageField: string;
    private localFilterFields: string[] = [];
    private remoteFilter: boolean;
    private remoteSort: boolean;
    private beforeLoad: BeforeLoadType | undefined;
    private afterLoad: AfterLoadType<T> | undefined;
    private beforeCreate: BeforeCreateType<T> | undefined;
    private afterCreate: AfterCreateType<T> | undefined;
    private beforeUpdate: BeforeUpdateType<T> | undefined;
    private afterUpdate: AfterUpdateType<T> | undefined;
    private beforeDelete: BeforeDeleteType | undefined;
    private afterDelete: AfterDeleteType | undefined;
    private beforeBatchDelete: BeforeBatchDeleteType | undefined;
    private afterBatchDelete: AfterBatchDeleteType | undefined;
    private send: SendFunctionType;

    private page: number = 1;
    private total: number = 0;
    private pages: number = 0;
    private filterValue: string = "";
    private search: any = {};
    private sortField: string = "";
    private sortOrder: string = "";
    private records: T[] = [];
    private originalRecords: T[] = [];

    entity: string;
    entityGroup: string;
    resourceName: string;

    constructor(config: RepositoryConfig<T> = {}) {
        super();
        const globalConfig = Repository.globalConfig || {};
        this.apiPrefix = config.apiPrefix || globalConfig.apiPrefix || "";
        this.entity = config.entity || "";
        this.entityGroup = config.entityGroup || "";
        this.resourceName = config.resourceName || "";
        this.createUrl = config.createUrl || "";
        this.readUrl = config.readUrl || "";
        this.updateUrl = config.updateUrl || "";
        this.deleteUrl = config.deleteUrl || "";
        this.createMethod =
            config.createMethod || globalConfig.createMethod || "POST";
        this.readMethod = config.readMethod || globalConfig.readMethod || "GET";
        this.updateMethod =
            config.updateMethod || globalConfig.updateMethod || "PUT";
        this.deleteMethod =
            config.deleteMethod || globalConfig.deleteMethod || "DELETE";
        this.pageSizes = config.pageSizes ||
            globalConfig.pageSizes || [10, 20, 50, 100];
        this.pageSize = config.pageSize || this.pageSizes[0];
        this.sortFieldParamName =
            config.sortFieldParamName ||
            globalConfig.sortFieldParamName ||
            "sorting";
        this.sortOrderParamName =
            config.sortOrderParamName || globalConfig.sortOrderParamName || "";
        this.useCache = config.useCache || false;
        this.filterParamName =
            config.filterParamName || globalConfig.filterParamName || "filter";
        this.pageKey = config.pageKey || "page";
        this.pageSizeParamName =
            config.pageSizeParamName ||
            globalConfig.pageSizeParamName ||
            "pageSize";
        this.skipCountParamName =
            config.skipCountParamName ||
            globalConfig.skipCountParamName ||
            "skipCount";
        this.idKey = config.idKey || globalConfig.idKey || "id";
        this.messageField = config.messageField || "";
        this.remoteFilter = config.remoteFilter || true;
        this.remoteSort = config.remoteSort || true;
        this.localFilterFields = config.localFilterFields || [];
        this.beforeLoad = config.beforeLoad || undefined;
        this.afterLoad = config.afterLoad || undefined;
        this.beforeCreate = config.beforeCreate || undefined;
        this.afterCreate = config.afterCreate || undefined;
        this.beforeUpdate = config.beforeUpdate || undefined;
        this.afterUpdate = config.afterUpdate || undefined;
        this.beforeDelete = config.beforeDelete || undefined;
        this.afterDelete = config.afterDelete || undefined;
        this.beforeBatchDelete = config.beforeBatchDelete || undefined;
        this.afterBatchDelete = config.afterBatchDelete || undefined;
        this.useQueryParamForId = config.useQueryParamForId || false;
        this.send =
            config.send || globalConfig.send || this.defaultSendFunction;
    }

    static initGlobalConfig = (config: RepositoryGlobalConfig) => {
        Repository.globalConfig = config;
    };

    public setPage = (page: number): void => {
        this.page = page;
        this.load();
    };

    public filter = (filter: string): void => {
        this.filterValue = filter;
        if (this.remoteFilter) {
            this.load().then(() => {
                this.fireEvent("filter", [
                    this.filterValue,
                    this.records,
                    this.total,
                    this.pages,
                    this.page,
                    this.search,
                    this.sortField,
                    this.sortOrder,
                ]);
            });
            return;
        }
        this.localFilter(filter);
    };

    public localFilter = (filter: string) => {
        this.records = this.originalRecords.slice();
        if (isEmpty(filter)) {
            this.total = this.records.length;
            this.pages = Math.ceil(this.total / this.pageSize);
            if (!this.remoteSort) {
                this.localSort(this.sortField, this.sortOrder, false);
            }
            this.fireEvent("filter", [
                this.filterValue,
                this.records,
                this.total,
                this.pages,
                this.page,
                this.search,
                this.sortField,
                this.sortOrder,
            ]);
            return;
        }
        this.records = this.records.filter((record: any) => {
            return this.localFilterFields.some(
                (field) =>
                    record[field] &&
                    record[field]
                        .toString()
                        .toLowerCase()
                        .includes(filter.toLowerCase())
            );
        });
        this.total = this.records.length;
        this.pages = Math.ceil(this.total / this.pageSize);
        if (!this.remoteSort) {
            this.localSort(this.sortField, this.sortOrder, false);
        }
        this.fireEvent("filter", [
            this.filterValue,
            this.records,
            this.total,
            this.pages,
            this.page,
            this.search,
            this.sortField,
            this.sortOrder,
        ]);
    };

    public getPageSizes = (): number[] => {
        return this.pageSizes;
    };

    public setPageSize = (size: number): void => {
        this.pageSize = size;
        this.load();
    };

    public getPageSize = (): number => {
        return this.pageSize;
    };

    public getPage = (): number => {
        return this.page;
    };

    public getFilter = (): string => {
        return this.filterValue;
    };

    public getSearch = (): any => {
        return this.search;
    };

    public getSortField = (): string => {
        return this.sortField;
    };

    public getSortDirection = (): string => {
        return this.sortOrder;
    };

    public getRecords = (): T[] => {
        return this.records;
    };

    public getTotal = (): number => {
        return this.total;
    };

    public getPages = (): number => {
        return this.pages;
    };

    public setSearch = (search: any): void => {
        this.search = search;
        this.load();
    };

public addSearch = (key: any, value?: any): void => {
    try {
        if (isObject(key)) {
            for (const k in key) {
                if (key.hasOwnProperty(k)) {
                    const v = key[k];
                    if (isEmpty(v)) {
                        delete this.search[k];
                    } else {
                        this.search[k] = v;
                    }
                }
            }
        } else if (!isEmpty(value)) {
            this.search[key as string] = value;
        } else {
            delete this.search[key as string];
        }
        
        this.load();
    } catch (error) {
        logger.error(this, "[addSearch]", "Error updating search:", error);
    }
}

    public setSort = (field: string, order: string): void => {
        this.sortField = field;
        this.sortOrder = order;
        if (this.remoteSort) {
            this.load().then(() => {
                this.fireEvent("sort", [
                    this.sortField,
                    this.sortOrder,
                    this.records,
                    this.total,
                    this.pages,
                    this.page,
                    this.filterValue,
                    this.search,
                ]);
            });
            return;
        }
        this.localSort(field, order);
    };

    public localSort = (
        field: string,
        order: string,
        fireEvent: boolean = true
    ) => {
        if (field && order) {
            this.records.sort((a: any, b: any) => {
                return order === "asc"
                    ? a[field].localeCompare(b[field])
                    : b[field].localeCompare(a[field]);
            });
        }
        if (fireEvent) {
            this.fireEvent("sort", [
                this.sortField,
                this.sortOrder,
                this.records,
                this.total,
                this.pages,
                this.page,
                this.filterValue,
                this.search,
            ]);
        }
    };

    public load = async (force: boolean = false): Promise<void> => {
        const method = this.readMethod;
        const url = this.getUrl(method);
        const params = this.getParams();
        const cacheKey = `${url}-${JSON.stringify(params)}`;

        if (this.useCache && !force && this.cache.has(cacheKey)) {
            this.records = this.cache.get(cacheKey) as T[];
            return;
        }

        if (this.beforeLoad && !this.beforeLoad(url, method, params)) {
            return;
        }

        try {
            const data = await this.send(url, method, undefined, params);
            this.originalRecords = data.items;
            this.records = data.items;
            this.total = data.totalCount;
            this.pages = Math.ceil(this.total / this.pageSize);

            if (this.afterLoad) {
                this.records = this.afterLoad(this.records, this.total) as T[];
            }

            this.fireEvent("load", [
                this.records,
                this.total,
                this.pages,
                this.page,
                this.filterValue,
                this.search,
                this.sortField,
                this.sortOrder,
            ]);

            if (this.useCache) {
                this.cache.set(cacheKey, this.records);
            }
        } catch (error) {
            logger.error(this, "[load]", "Error loading data:", error);
        }
    };

    public create = async (entity: T): Promise<void> => {
        if (this.beforeCreate && !this.beforeCreate(entity)) {
            return;
        }

        const url = this.getUrl("create");
        const method = this.createMethod;

        try {
            const data = await this.send(url, method, entity);
            this.fireEvent("add", [data]);

            if (this.afterCreate) {
                this.afterCreate(entity);
            }

            this.load(true);
            return data;
        } catch (error) {
            throw error;
        }
    };

    public update = async (entity: any): Promise<void> => {
        if (this.beforeUpdate && !this.beforeUpdate(entity)) {
            return;
        }

        const url = this.getUrl("update", entity.id);
        const method = this.updateMethod;

        try {
            const data = await this.send(url, method, entity);
            this.fireEvent("update", [data]);

            if (this.afterUpdate) {
                this.afterUpdate(entity);
            }

            this.load(true);
            return data;
        } catch (error) {
            logger.error(this, "[update]", "Error updating entity:", error);
        }
    };

    public delete = async (id: string | number): Promise<void> => {
        if (this.beforeDelete && !this.beforeDelete(id)) {
            return;
        }

        let messages = this.getDeleteMessage([id]);
        const confirmHandler = Repository.globalConfig.deleteConfirmHandler;
        if (confirmHandler && !(await confirmHandler(messages.join("<br>")))) {
            return;
        }

        const url = this.getUrl("delete", id);
        const method = this.deleteMethod;

        try {
            const data = await this.send(url, method);
            this.fireEvent("delete", [id, data]);

            if (this.afterDelete) {
                this.afterDelete(id);
            }

            if (Repository.globalConfig.messageHandler) {
                Repository.globalConfig.messageHandler(
                    "Message.DeleteSuccess",
                    "success"
                );
            }

            this.load(true);
        } catch (error) {
            logger.error(this, "Error deleting entity:", error);
        }
    };

    public batchDelete = async (ids: (string | number)[]): Promise<void> => {
        if (this.beforeBatchDelete && !this.beforeBatchDelete(ids)) {
            return;
        }

        const url = this.getUrl("delete");
        const method = this.deleteMethod;

        try {
            await this.send(url, method, ids);
            this.fireEvent("batchDelete", [ids]);

            if (this.afterBatchDelete) {
                this.afterBatchDelete(ids);
            }

            if (Repository.globalConfig.messageHandler) {
                Repository.globalConfig.messageHandler(
                    "Message.DeleteSuccess",
                    "success"
                );
            }

            this.load(true);
        } catch (error) {
            logger.error(this, "Error batch deleting entities:", error);
        }
    };

    public getSingle = async (id: string | number): Promise<T> => {
        const url = this.getUrl("read", id);
        const method = this.readMethod;

        try {
            const data = await this.send(url, method);
            return data;
        } catch (error) {
            logger.error(this, "Error getting single entity:", error);
            throw error;
        }
    };

    protected defaultSendFunction: SendFunctionType = async (
        url,
        method,
        data,
        params
    ) => {
        if (params) {
            url += `?${Object.keys(params)
                .map((key) => `${key}=${params[key]}`)
                .join("&")}`;
        }
        if (method === "GET") {
            return await http.get(url);
        }
        if (method === "POST") {
            return await http.post(url, data);
        }
        if (method === "PUT") {
            return await http.put(url, data);
        }
        if (method === "DELETE") {
            return await http.delete(url);
        }
        if (method === "PATCH") {
            return await http.patch(url, data);
        }
        logger.debug(this, `Unsupported method: ${method}`);
        throw new Error(`Unsupported method: ${method}`);
    };

    protected getUrl = (action: string, id?: string | number): string => {
        if (action === "create" && this.createUrl) return this.createUrl;
        if (action === "read" && this.readUrl) return this.readUrl;
        if (action === "update" && this.updateUrl) return this.updateUrl;
        if (action === "delete" && this.deleteUrl) return this.deleteUrl;

        let url = `${this.apiPrefix}`;
        if (this.entityGroup) {
            url += `/${this.entityGroup}`;
        }
        url += `/${camelCaseToDash(this.entity)}`;
        if (id && !this.useQueryParamForId) {
            url += `/${id}`;
        }
        return url;
    };

    protected getParams = (): any => {
        const params: any = {};
        if (this.sortField && this.sortOrder) {
            if (this.sortOrderParamName) {
                params[this.sortFieldParamName] = this.sortField;
                params[this.sortOrderParamName] = this.sortOrder;
            } else {
                params[
                    this.sortFieldParamName
                ] = `${this.sortField} ${this.sortOrder}`;
            }
        }
        if (this.page) {
            params[this.pageKey] = this.page;
        }
        if (this.pageSize) {
            params[this.pageSizeParamName] = this.pageSize;
        }
        if (this.skipCountParamName) {
            params[this.skipCountParamName] = (this.page - 1) * this.pageSize;
        }
        if (this.filterValue) {
            params[this.filterParamName] = this.filterValue;
        }
        if (this.search) {
            Object.assign(params, this.search);
        }
        return params;
    };

    protected getDeleteMessage(ids: (string | number)[]): (string | number)[] {
        const messages: string[] = [];
        const messageField = this.messageField;
        this.records.forEach((record: any) => {
            if (ids.includes(record[this.idKey])) {
                let message = record[messageField];
                if (!isEmpty(message)) {
                    messages.push(message);
                }
            }
        });
        return messages.length > 0 ? messages : ids;
    }

    public destroy = (): void => {
        this.cache.clear();
        this.records = [];
        this.originalRecords = [];
        this.page = 1;
        this.total = 0;
        this.pages = 0;
        this.filterValue = "";
        this.search = {};
        this.sortField = "";
        this.sortOrder = "";
        this.beforeLoad = undefined;
        this.afterLoad = undefined;
        this.send = this.defaultSendFunction;
        this.beforeCreate = undefined;
        this.afterCreate = undefined;
        this.beforeUpdate = undefined;
        this.afterUpdate = undefined;
        this.beforeDelete = undefined;
        this.afterDelete = undefined;
        this.beforeBatchDelete = undefined;
        this.afterBatchDelete = undefined;
        super.destroy();
    };
}
