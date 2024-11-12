import { ref } from "vue";
import { logger } from "~/libs";
import { useDialog, UseDialogConfig } from "./useDialog";

export interface UseMultilingualDialogConfig extends UseDialogConfig {
    rowItems?: any[];
}
export function useMultilingualDialog(
    api: any,
    config: UseMultilingualDialogConfig = {}
) {
    const entity = ref<any>(null);
    const submit = async (values: any) => {
        try {
            await api.updateMultilingual(
                entity.value[api.idFieldName],
                values
            );
            dialogRef.value?.success("Message.SaveSuccessAndClose");
            setTimeout(() => {
                entity.value = null;
                dialogVisible.value = false;
            }, 3000);
        } catch (e: any) {
            dialogRef.value.error(e.message);
            logger.error("[MultilingualDialog][okClick]", e);
        }
    };

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
        ...config,
    });

    const multilingualShowDialog = async (row: any) => {
        const data = await api.getMultilingual(row[api.idFieldName]);
        entity.value = row;
        setInitValues(data);
        dialogTitlePrefix.value = "Components.MultilingualSomething",
        dialogTitle.value = row[api.messageField];
        dialogVisible.value = true;
    };

    return {
        multilingualDialogRef: dialogRef,
        multilingualDialogVisible: dialogVisible,
        multilingualDialogData: dialogData,
        multilingualDialogProps: {
            rowItems: config.rowItems,
            ...dialogProps,
        },
        multilingualShowDialog,
        multilingualSetInitValues: setInitValues,
    };
}
