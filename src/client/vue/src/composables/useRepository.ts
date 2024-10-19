import { isEmpty, RepositoryFactory } from "~/libs";

export function useRepository(type: string, singleton?: boolean, config?: any ) {
    if(isEmpty(type)){
        throw new Error("Repository type cannot be empty.");
    }
    type = type.toLocaleLowerCase();
    const repositoryEntry = RepositoryFactory.repositoryMap.get(type);
    if (!repositoryEntry) {
        throw new Error(`${type} repository is not registered.`);
    }

    const { type: repositoryClass, defaultConfig } = repositoryEntry;

    // 合并默认配置和用户传入的配置
    const finalConfig = { ...defaultConfig, ...(config || {}) };

    
    // 如果要强制新建实例，检查参数是否为 false
    if (!singleton) {
        return new repositoryClass(finalConfig);
    }
    
    // 默认使用单例模式，检查是否已有实例
    if (!repositoryEntry.instance) {
        repositoryEntry.instance = new repositoryClass(finalConfig);
    }
    return repositoryEntry.instance;
}
