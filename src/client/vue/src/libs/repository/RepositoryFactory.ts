import { Repository } from "./Repository";

export type RepositoryRegistryType ={
    type: any,
    defaultConfig?: any,
    instance?: Repository<any>
}

export class RepositoryFactory {
    static repositoryMap: Map<string, RepositoryRegistryType> = new Map();

    // 注册 Repository 类，并添加默认配置
    static register(registers : Record<string, RepositoryRegistryType>) {
        for (const key in registers) {
            RepositoryFactory.repositoryMap.set(key.toLocaleLowerCase(), registers[key]);
        }
    }
}

