import { RepositoryRegistryType } from "~/libs";
import { RoleRepository } from "./Role";
import { UserRepository } from "./User";
import { ApplicationRepository } from "./Application";
import { ScopRepository } from "./Scope";
import { TenantRepository } from "./Tenant";

export const repositoryRegisters: Record<string, RepositoryRegistryType> = {
    'Role': {
        type: RoleRepository
    },
    'user': {
        type: UserRepository
    },
    'application': {
        type: ApplicationRepository
    },
    'scope':{
        type: ScopRepository
    },
    tenant:{
        type: TenantRepository
    }
}