import { isEmpty, RepositoryFactory } from "~/libs";

// 用于存储 config 与实例的映射关系
const repositoryInstances = new Map();

export function useRepository(type: string, singleton?: boolean, config?: any) {
    if (isEmpty(type)) {
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
    
    // 生成唯一的 config 键，便于存储和查找
    const configKey = JSON.stringify(finalConfig);

    // 默认使用单例模式，检查 config 是否已有实例
    if (!repositoryInstances.has(configKey)) {
        repositoryInstances.set(configKey, new repositoryClass(finalConfig));
    }
    console.log(repositoryInstances)
    return repositoryInstances.get(configKey);
}
