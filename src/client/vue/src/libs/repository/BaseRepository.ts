import { BaseClass } from "../BaseClass";
import { http } from "../http";
import { isEmpty, Plural, logger, camelCaseToDash, capitalize } from "../utils";
import { RepositoryGlobalConfig } from "./RepositoryGlobalConfig";
import { RepositoryConfig, EntityInterface } from "./RepositoryType";

export class BaseRepository<T extends EntityInterface> extends BaseClass {
    $className = "BaseRepository";
    config: RepositoryConfig<T>;
    _pageSize: number = 10;
    _page: number = 1;
    _total: number = 0;
    _pages: number = 0;
    _filter: string = "";
    _search: any = {};
    _sortField: string = "";
    _sortOrder: string = "";
    _records: T[] = [];
    _originalRecords: T[] = [];
    _entity: string = "";
    _entityGroup: string = "";
    _resourceName: string = "";
    _messageField: string = "";

    constructor(config: RepositoryConfig<T>) {
        super();
        this.config = config;
        this._pageSize =
            config.pageSize || RepositoryGlobalConfig.pageSizes[0] || 10;
        const { entity, entityGroup, resourceName, messageField } = config;
        !isEmpty(entity) && (this.entity = entity!);
        !isEmpty(entityGroup) && (this.entityGroup = entityGroup!);
        !isEmpty(resourceName) && (this.resourceName = resourceName!);
        !isEmpty(messageField) && (this.messageField = messageField!);
        this.initialize();
    }

    initialize(){
        logger.debug(this, "[initialize]", "BaseRepository initialize");
    }

    get pageSize(): number {
        return this._pageSize;
    }


    set total(value: number) {
        this._total = value;
    }

    get total(): number {
        return this._total;
    }

    set pages(value: number) {
        this._pages = value;
    }

    get pages(): number {
        return this._pages;
    }


    set sortField(value: string) {
        this._sortField = value;
    }

    get sortField(): string {
        return this._sortField;
    }

    set sortOrder(value: string) {
        this._sortOrder = value;
    }

    get sortOrder(): string {
        return this._sortOrder;
    }

    set records(value: T[]) {
        this._records = value;
    }

    get records(): T[] {
        return this._records;
    }

    set originalRecords(value: T[]) {
        this._originalRecords = value;
    }

    get originalRecords(): T[] {
        return this._originalRecords;
    }

    set entity(value: string) {
        this._entity = value;
    }

    get entity(): string {
        return this._entity;
    }

    get entityPlural(): string {
        return Plural.get(this.entity);
    }

    set entityGroup(value: string) {
        this._entityGroup = value;
    }

    get entityGroup(): string {
        return this._entityGroup;
    }

    set resourceName(value: string) {
        this._resourceName = value;
    }

    get resourceName(): string {
        return this._resourceName;
    }

    set messageField(value: string) {
        this._messageField = value;
    }

    get messageField(): string {
        return this._messageField;
    }

    get apiPrefix(): string | undefined {
        return this.config.api?.prefix || RepositoryGlobalConfig.apiPrefix;
    }

    get createMethod(): string {
        return (
            this.config.api?.method?.create ||
            RepositoryGlobalConfig.createMethod ||
            "post"
        );
    }

    get readMethod(): string {
        return (
            this.config.api?.method?.read ||
            RepositoryGlobalConfig.getMethod ||
            "get"
        );
    }

    get updateMethod(): string {
        return (
            this.config.api?.method?.update ||
            RepositoryGlobalConfig.updateMethod ||
            "put"
        );
    }

    get deleteMethod(): string {
        return (
            this.config.api?.method?.delete ||
            RepositoryGlobalConfig.deleteMethod ||
            "delete"
        );
    }

    get pageSizes(): number[] {
        return this.config.paging?.sizes || RepositoryGlobalConfig.pageSizes;
    }

    get pageParamName(): string | undefined {
        return (
            this.config.paging?.pageParamName ||
            RepositoryGlobalConfig.pageParamName
        );
    }

    get pageSizeParamName(): string {
        return (
            this.config.paging?.pageParamName ||
            RepositoryGlobalConfig.pageParamName ||
            "maxResultCount"
        );
    }

    get skipCountParamName(): string | undefined {
        return (
            this.config.paging?.skipCountParamName ||
            RepositoryGlobalConfig.skipCountParamName
        );
    }

    get sortParamName(): string {
        return (
            this.config.sorting?.sortParamName ||
            RepositoryGlobalConfig.sortParamName ||
            "sorting"
        );
    }

    get sortOrderParamName(): string | undefined {
        return (
            this.config.sorting?.orderParamName ||
            RepositoryGlobalConfig.orderParamName
        );
    }

    get useQueryParamForId(): boolean {
        return (
            this.config.useQueryParamForId ||
            RepositoryGlobalConfig.useQueryParamForId
        );
    }

    get useCache(): boolean {
        return this.config.useCache || RepositoryGlobalConfig.useCache || true;
    }

    get filterParamName(): string {
        return (
            this.config.filterParamName ||
            RepositoryGlobalConfig.filterParamName ||
            "filter"
        );
    }

    get idFieldName(): string {
        return (
            this.config.idFieldName ||
            RepositoryGlobalConfig.idFieldName ||
            "id"
        );
    }

    get createUrl(): string {
         return this.config.createUrl || this.getUrl();
    }

    get readUrl(): string {
        return this.config.readUrl || this.getUrl();
    }

    get updateUrl(): string {
        return this.config.updateUrl || this.getUrl();
    }

    get deleteUrl(): string {
        return this.config.deleteUrl || this.getUrl();
    }

    get messageFieldName(): string {
        return this.config.messageFieldName || "displayName";
    }

    get isLocalFilter(): boolean {
        return this.config.localFilter || false;
    }

    get isRemoteFilter(): boolean {
        return this.config.remoteFilter || true;
    }

    get isRemoteSort(): boolean {
        return this.config.remoteSort || true;
    }

    get localFilterFields(): string[] {
        return this.config.localFilterFields || [];
    }

    get searchValue(): any {
        return this._search;
    }

    get createTitle(): string {
        return this.config.createTitle || "New" + capitalize(this.entity);
    }

    get updateTitle(): string {
        return this.config.updateTitle || "Edit" + capitalize(this.entity);
    }

    send = async (url: string, method: string, data?: any, params?: any) => {
        method = (method || '').toLocaleUpperCase();
        if (this.config.send) {
            return this.config.send.call(this, url, method, data, params);
        }
        if(method !== "GET" && params){
            url = this.addUrlParams(url, params);
        }
        switch (method) {
            case "GET":
                return await http.get(url, params);
            case "POST":
                return await http.post(url, data);
            case "PUT":
                return await http.put(url, data);
            case "DELETE":
                return await http.delete(url);
            case "PATCH":
                return await http.patch(url, data);
            default:
                logger.warn(this, "send", `Unsupported method ${method}`);
                throw new Error(`Unsupported method ${method}`);
        }
    };

    beforeLoad = (url: string, method: string, params: any) => {
        if (!this.config.beforeLoad) return true;
        return this.config.beforeLoad.call(this, url, method, params);
    };

    afterLoad = (records: T[], total: number): any => {
        if (!this.config.afterLoad) return records;
        return this.config.afterLoad.call(this, records, total);
    };

    beforeCreate = (record: T): boolean => {
        if (!this.config.beforeCreate) return true;
        return this.config.beforeCreate.call(this, record);
    };

    afterCreate = (record: T): void => {
        if (!this.config.afterCreate) return;
        this.config.afterCreate.call(this, record);
    };
    beforeUpdate = (record: T): boolean => {
        if (!this.config.beforeUpdate) return true;
        return this.config.beforeUpdate.call(this, record);
    };

    afterUpdate = (record: T): void => {
        if (!this.config.afterUpdate) return;
        this.config.afterUpdate.call(this, record);
    };

    beforeDelete = (id: string | number): boolean => {
        if (!this.config.beforeDelete) return true;
        return this.config.beforeDelete.call(this, id);
    };

    afterDelete = (id: string | number): void => {
        if (!this.config.afterDelete) return;
        this.config.afterDelete.call(this, id);
    };

    beforeBatchDelete = (ids: (string | number)[]): boolean => {
        if (!this.config.beforeBatchDelete) return true;
        return this.config.beforeBatchDelete.call(this, ids);
    };

    afterBatchDelete = (ids: (string | number)[]): void => {
        if (!this.config.afterBatchDelete) return;
        this.config.afterBatchDelete.call(this, ids);
    };

    confirmDelete = async (message: (string | number)[]): Promise<boolean> => {
        if(!this.config.deleteConfirmHandler) return Promise.resolve(true);
        return this.config.deleteConfirmHandler.call(this, message);
    };

    message = (message: string, type: string = "info") => {
        if (!this.config.messageHandler) return message;
        this.config.messageHandler.call(this, message, type);
    };

    success = (message: string) => {
        this.message(message, "success");
    };

    error = (message: string) => {
        this.message(message, "error");
    };

    warning = (message: string) => {
        this.message(message, "warning");
    };

    info = (message: string) => {
        this.message(message, "info");
    };

    getUrl = (): string => {
        let url = this.apiPrefix || "";
        if (this.entityGroup) {
            url += `/${this.entityGroup}`;
        }
        url += `/${camelCaseToDash(this.entityPlural)}`;
        return url;
    };

    getParams = (): any => {
        const params: any = {};
        if (this.sortField && this.sortOrder) {
            if (this.sortOrderParamName) {
                params[this.sortParamName] = this.sortField;
                params[this.sortOrderParamName] = this.sortOrder;
            } else {
                params[
                    this.sortParamName
                ] = `${this.sortField} ${this.sortOrder}`;
            }
        }
        if (this.pageParamName) {
            params[this.pageParamName] = this._page;
        }
        params[this.pageSizeParamName] = this.pageSize;
        if (this.skipCountParamName) {
            params[this.skipCountParamName] = (this._page - 1) * this.pageSize;
        }
        if (!isEmpty(this._filter)) {
            params[this.filterParamName] = this._filter;
        }
        if (this.searchValue) {
            Object.assign(params, this.searchValue);
        }
        return params;
    };

    //为url添加参数
    addUrlParams = (url: string, params: any): string => {
        if (isEmpty(params)) return url;
        const queryString = Object.keys(params)
           .map((key) => `${key}=${params[key]}`)
           .join("&");
        return `${url}?${queryString}`;
    };


    fireEvent = (name: string): void => {
        super.fireEvent(name, [
            this.records,
            this.total,
            this.pages,
            this._page,
            this._filter,
            this.searchValue,
            this.sortField,
            this.sortOrder,
        ]);
    };

    destroy(): void {
        this.records = [];
        this.originalRecords = [];
        this._search = {};
        this.config = {} as RepositoryConfig<T>;
    }
}
