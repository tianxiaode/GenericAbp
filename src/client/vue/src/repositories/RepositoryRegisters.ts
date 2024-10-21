import { RepositoryRegistryType } from "~/libs";
import { RoleRepository } from "./Role";
import { UserRepository } from "./User";

export const repositoryRegisters: Record<string, RepositoryRegistryType> = {
    'Role': {
        type: RoleRepository
    },
    'user': {
        type: UserRepository
    }
}