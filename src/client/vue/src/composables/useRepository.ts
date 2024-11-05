import { onUnmounted } from "vue";
import { isEmpty, logger, RepositoryFactory } from "~/libs";

export function useRepository(type: string, config?: any) {
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
    const instance = new repositoryClass(finalConfig);

    onUnmounted(()=>{
        logger.debug('[useRepository][onUnmounted]', 'destroy repository');
        instance.destroy();
    })

    return instance
}
