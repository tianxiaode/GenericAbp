import { ref } from "vue";
import { logger, PermissionGroupInterface } from "~/libs";
import { useDialog } from "./useDialog";

export function usePermissionsDialog(api: any, provideKeyField: string, config: any = {}) {
    const entity = ref<any>(null);
    const initPermissions = ref<any[]>([]);
    const submit = async (values: PermissionGroupInterface[]) => {
        try {
            const newPermissions = getSubmitValue(values);
            console.log(newPermissions)
            await api.updatePermissions(entity.value[provideKeyField], newPermissions);
            dialogRef.value?.success("Message.SaveSuccessAndClose");
            setTimeout(() => {
                entity.value = null;
                dialogVisible.value = false;
            }, 3000);
        } catch (e: any) {
            dialogRef.value.error(e.message);
            logger.error("[usePermissionsDialog][okClick]", e);
        }
    };

    const checkChange = ()=>{
        return JSON.stringify(initPermissions.value)!== JSON.stringify(getSubmitValue(dialogData.value))
    }

    const getSubmitValue = (values: PermissionGroupInterface[]) => {
        return config.getSubmitValue 
        ? config.getSubmitValue(values) : 
        values.flatMap(group => group.permissions.map(permission => ({ name: permission.name, isGranted: permission.isGranted })));    
    }


    const {
        dialogVisible,
        dialogTitle,
        dialogTitlePrefix,
        dialogData,
        dialogProps,
        dialogRef,
        setInitValues,
    } = useDialog({
        submit,
        checkChange,
        ...config,
    });

    const permissionsShowDialog = async (row: any) => {
        const data = await api.getPermissions(row[provideKeyField]);
        entity.value = row;
        const groups = data.groups.sort((a: any, b: any) => a.displayName.localeCompare(b.displayName)); 
        initPermissions.value = getSubmitValue(groups);
        setInitValues(groups);
        dialogTitlePrefix.value = "Components.PermissionsSomething",
        dialogTitle.value = row[api.messageField];
        dialogVisible.value = true;
    };

    return {
        permissionsDialogRef: dialogRef,
        permissionsDialogVisible: dialogVisible,
        permissionsDialogData: dialogData,
        permissionsDialogProps: dialogProps,
        permissionsShowDialog,
        permissionsSetInitValues: setInitValues,
    };
}
