import { RepositoryRegistryType } from "~/libs";
import { RoleRepository } from "./Role";

export const repositoryRegisters: Record<string, RepositoryRegistryType> = {
    'Role': {
        type: RoleRepository
    },
    'user': {
        type: RoleRepository
    }
}