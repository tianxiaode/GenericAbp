import { isEmpty } from "lodash";
import { logger } from "../utils";

export type RepositoryRegistryType ={
    type: any,
    defaultConfig?: any,
}

export class RepositoryFactory {
    static $className = 'RepositoryFactory';
    static repositoryMap: Map<string, RepositoryRegistryType> = new Map();
    static instanceMap: Map<string, any> = new Map();
    // 注册 Repository 类，并添加默认配置
    static register(registers : Record<string, RepositoryRegistryType>) {
        for (let key in registers) {
            RepositoryFactory.repositoryMap.set(RepositoryFactory.normalizeType(key), registers[key]);
        }
    }

    static get(type: string, config: any = {}){
        if(isEmpty(type)){
            logger.raise(RepositoryFactory, '[get]', `Type is required` );            
        }
        type = RepositoryFactory.normalizeType(type);
        const repositoryEntry = RepositoryFactory.repositoryMap.get(type);
        if (!repositoryEntry) {
            logger.raise(RepositoryFactory, '[get]', `Repository not found for type ${type}` );            
        }
        const finalConfig = {
            ...(repositoryEntry!.defaultConfig || {}) , ...(config || {}) };
        const key = `${type}-${JSON.stringify(finalConfig)}`;
        logger.debug(RepositoryFactory, '[get]', `Get repository for type ${type} with config ${JSON.stringify(finalConfig)}` );
        let repository = RepositoryFactory.instanceMap.get(key);
        if (!repository) {
            repository = new repositoryEntry!.type(finalConfig);
            RepositoryFactory.instanceMap.set(key, repository);
        }
        return repository;
    }

    static normalizeType(type: string): string {
        return type.toLocaleLowerCase();
    }
}

