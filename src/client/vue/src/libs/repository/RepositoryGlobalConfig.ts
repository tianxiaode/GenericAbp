import {
    DeleteConfirmHandlerType,
    RepositoryGlobalConfigType,
    SendFunctionType,
    MessageHandlerType,
} from "./RepositoryType";

export class RepositoryGlobalConfig {
    static config: RepositoryGlobalConfigType;

    static init(config: RepositoryGlobalConfigType) {
        this.config = config;
    }

    static get apiPrefix(): string | undefined {
        return this.config.api?.prefix;
    }

    static get createMethod(): string | undefined {
        return this.config.api?.method?.create;
    }

    static get updateMethod(): string | undefined {
        return this.config.api?.method?.update;
    }

    static get deleteMethod(): string | undefined {
        return this.config.api?.method?.delete;
    }

    static get getMethod(): string | undefined {
        return this.config.api?.method?.read;
    }

    static get pageSizes(): number[] {
        return this.config.paging.sizes;
    }

    static get pageParamName(): string | undefined {
        return this.config.paging.pageParamName;
    }

    static get pageSizeParamName(): string | undefined {
        return this.config.paging.pageSizeParamName;
    }

    static get skipCountParamName(): string | undefined {
        return this.config.paging.skipCountParamName;
    }

    static get sortParamName(): string | undefined {
        return this.config.sorting?.sortParamName;
    }

    static get orderParamName(): string | undefined {
        return this.config.sorting?.orderParamName;
    }

    static get filterParamName(): string | undefined {
        return this.config.filterParamName;
    }

    static get useQueryParamForId(): boolean {
        return this.config.useQueryParamForId || false;
    }

    static get useCache(): boolean {
        return this.config.useCache || true;
    }

    static get idFieldName(): string {
        return this.config.idFieldName || "id";
    }

    static get idParamName(): string {
        return this.config.idParamName || "id";
    }

    static get send(): SendFunctionType | undefined {
        return this.config.send;
    }

    static get deleteConfirmHandler(): DeleteConfirmHandlerType | undefined {
        return this.config.deleteConfirmHandler;
    }

    static get messageHandler(): MessageHandlerType | undefined {
        return this.config.messageHandler;
    }
}
