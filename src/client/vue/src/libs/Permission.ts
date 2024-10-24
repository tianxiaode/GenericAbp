import { appConfig, http } from ".";

export interface PermissionGroupItemInterface {
    name: String;
    displayName: String;
    parentName: String;
    isGranted: Boolean;
    allowedProviders: String[];
    value: String;
    grantedProviders: [
        {
            providerName: String;
            providerKey: String;
        }
    ];
}

export interface PermissionGroupInterface {
    name: String;
    displayName: String;
    displayNameKey: String;
    displayNameResource: String;
    permissions: PermissionGroupItemInterface[];
}

export interface PermissionInterface {
    entityDisplayName: String;
    groups: PermissionGroupInterface[];
}

export interface PermissionUpdateInterface {
    name: String;
    isGranted: Boolean;
}

class Permission {
    $className: string = "Permission";

    static async get(
        providerName: string,
        providerKey: string
    ): Promise<PermissionInterface> {
        try {
            return await http.get("/api/permission-management/permissions", {
                providerName,
                providerKey,
            });
        } catch (error) {
            throw new Error("Failed to get permission");
        }
    }

    static async update(
        providerName: string,
        providerKey: string,
        permissions: PermissionUpdateInterface[]
    ): Promise<void> {
        try {
            await http.put(
                `/api/permission-management/permissions/?providerName=${providerName}&providerKey=${providerKey}`,
                {
                    permissions,
                }
            );
        } catch (error) {
            throw new Error("Failed to update permission");
        }
    }

    static get getAll() {
        return appConfig.permissions;
    }

}

export const permission = Permission;
