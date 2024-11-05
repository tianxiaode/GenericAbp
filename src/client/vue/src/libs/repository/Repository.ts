import { clone, deepMerge, isEmpty, logger } from "../utils";
import { BaseRepository } from "./BaseRepository";
import { EntityInterface } from "./RepositoryType";

export class Repository<T extends EntityInterface> extends BaseRepository<T> {
    $className = "Repository";

    set page(value: number) {
        this._page = value;
        this.load();
    }

    get page(): number {
        return this._page;
    }

    get pageSize(): number {
        return this._pageSize;
    }

    set pageSize(value: number) {
        this._pageSize = value;
        this.load();
    }

    set filter(value: string) {
        this._filter = value;
        this.load();
    }

    get filter(): string {
        return this._filter;
    }

    load = async (): Promise<void> => {
        const method = this.readMethod;
        const url = this.readUrl;
        const params = this.getParams();

        // 提前终止加载过程
        if (!this.beforeLoad(url, method, params)) return;

        try {
            const data = await this.send(url, method, undefined, params);
            this.originalRecords = data.items;
            this.records = this.afterLoad(clone(data.items), data.totalCount) as T[];
            this.total = data.totalCount;
            this.pages = Math.ceil(this.total / this.pageSize);

            this.fireEvent("load");

        } catch (error) {
            logger.error(this, "[load]", "Error loading data:", error);
            throw error;
        }
    };

    search = (...args: any[]): void => {
        let newSearch: any;
        let load: boolean = true;

        if (typeof args[0] === "object") {
            newSearch = args[0];
            load = args[1] ?? false;
        } else {
            newSearch = { [args[0]]: args[1] };
            load = args[2] ?? false;
        }

        // 清理搜索条件
        for (const key in newSearch) {
            if (isEmpty(newSearch[key])) {
                delete this._search[key];
            } else {
                this._search[key] = newSearch[key];
            }
        }

        if (load) this.load();
    };

    getEntity = async (id: string | number, loadAdditionalData?: boolean): Promise<T | undefined> => {
        const url = this.handleUrlId(this.readUrl, id);
        try {
            let data = await this.send(url, this.readMethod);
            if(loadAdditionalData === true){
                const additionalData = await this.loadAdditionalData(id);
                if (additionalData) {
                    data = deepMerge(data, additionalData);
                }    
            }
            return data as T;
        } catch (error) {
            logger.error(this, "[getEntity]", "Error getting entity:", error);
            throw error;
        }
    };

    loadAdditionalData = async (_: string | number): Promise<any> =>{
        return undefined;
    }

    getList = async (filter: any): Promise<T[]> => {
        const url = this.readUrl;
        if (typeof filter === "string") {
            filter = { filter };
        }
        try {
            const data = await this.send(
                url,
                this.readMethod,
                undefined,
                filter
            );
            return data.items;
        } catch (error) {
            logger.error(this, "[getList]", "Error getting list:", error);
            throw error;
        }
    };

    create = async (entity: T): Promise<T | undefined> => {
        if (!this.beforeCreate(entity)) return undefined;

        const url = this.createUrl;
        try {
            const data = await this.send(url, this.createMethod, entity);
            this.fireEvent("create");
            this.afterCreate(data);
            this.load();
            return data;
        } catch (error) {
            logger.error(this, "[create]", "Error creating entity:", error);
            throw error;
        }
    }

    update = async (entity: T): Promise<T | undefined> => {
        if (!this.beforeUpdate(entity)) return undefined;

        const url = this.handleUrlId(this.updateUrl, (entity as any)[this.idFieldName] as string | number);
        try {
            const data = await this.send(url, this.updateMethod, entity);
            this.fireEvent("update");
            this.afterUpdate(data);
            this.load();
            return data;
        } catch (error) {
            logger.error(this, "[update]", "Error updating entity:", error);
            throw error;
        }
    }

    delete = async (id: string | number): Promise<void> => {
        await this.performDelete(id, false);
    }

    batchDelete = async (ids: (string | number)[]): Promise<void> => {
        await this.performDelete(ids, true);
    }

    sort = (field: string, order: string) => {
        this.sortField = field;
        this.sortOrder = order;

        if (this.isRemoteSort) {
            this.load().then(() => {
                this.fireEvent("sort");
            });
        } else {
            this.localSort(field, order);
        }
    };

    localFilter = (filter: string) => {
        this.records = [...this.originalRecords];

        if (isEmpty(filter)) {
            this.updatePagination();
            if (!this.isRemoteSort) {
                this.localSort(this.sortField, this.sortOrder, false);
            }
            this.fireEvent("filter");
            return;
        }

        this.records = this.records.filter((record: any) =>
            this.localFilterFields.some((field) =>
                record[field]
                    ?.toString()
                    .toLowerCase()
                    .includes(filter.toLowerCase())
            )
        );

        this.updatePagination();

        if (!this.isRemoteSort) {
            this.localSort(this.sortField, this.sortOrder, false);
        }
        this.fireEvent("filter");
    };

    private updatePagination() {
        this.total = this.records.length;
        this.pages = Math.ceil(this.total / this.pageSize);
    }

    localSort = (field: string, order: string, fireEvent: boolean = true) => {
        if (field && order) {
            this.records.sort((a: any, b: any) =>
                order === "asc"
                    ? a[field].localeCompare(b[field])
                    : b[field].localeCompare(a[field])
            );
        }
        if (fireEvent) {
            this.fireEvent("sort");
        }
    };

    private getDeleteMessages = (
        ids: (string | number)[]
    ): (string | number)[] => {
        const messages: string[] = [];
        const messageField = this.messageField;

        this.records.forEach((record: any) => {
            if (ids.includes(record[this.idFieldName])) {
                const message = record[messageField];
                if (!isEmpty(message)) {
                    messages.push(message);
                }
            }
        });

        return messages.length > 0 ? messages : ids;
    };

    private performDelete = async (
        ids: (string | number) | (string | number)[],
        isBatch: boolean
    ): Promise<void> => {
        const validateDelete = isBatch
            ? this.beforeBatchDelete(ids as (string | number)[])
            : this.beforeDelete(ids as string | number);
        if (!validateDelete) return;

        const messages = this.getDeleteMessages(
            Array.isArray(ids) ? ids : [ids]
        );
        if (!(await this.confirmDelete(messages))) return;

        const method = this.deleteMethod;
        let url = this.deleteUrl;
        if (!isBatch) {
            url = this.handleUrlId(url, ids as string | number);
        }

        try {
            await this.send(url, method);
            this.fireEvent(isBatch ? "batchDelete" : "delete");
            if (isBatch) {
                this.afterBatchDelete(ids as (string | number)[]);
            } else {
                this.afterDelete(ids as string | number);
            }
            this.success("Message.DeleteSuccess");
            this.load();
        } catch (error) {
            logger.error(
                this,
                `[${isBatch ? "batchDelete" : "delete"}]`,
                "Error deleting entity:",
                error
            );
            throw error;
        }
    }

    //处理url中id参数
    private handleUrlId  = (url:string, id: string | number): string => {
        if (this.useQueryParamForId) {
            const separator = url.includes("?") ? "&" : "?";
            return `${url}${separator}${this.idParamName}=${id}`;
        }
        return `${url}/${id}`;
    }

    destroy(): void {
        super.destroy();
        logger.debug(this,"[destroy]", "Repository destroyed");
    }
}
