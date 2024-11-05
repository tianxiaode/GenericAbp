import { RepositoryFactory } from "~/libs";


export function useRepository(type: string, config?: any) {

    return RepositoryFactory.get(type, config);
}
