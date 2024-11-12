import { ref } from "vue";
import { deepMerge, i18n, logger } from "~/libs";
import { useConfirm } from "./useConfirm";

export interface UseDialogConfig {
    initData?: any,
    dialogProps?:{
        resetClick?: any;
        cancelText?: string;
        cancelClick?: any;
        okText?: string;
        okClick?: any;    
    }
    beforeClose?: any;
    afterClose?: any;
    submit?:any
}

export function useDialog(config: UseDialogConfig = {}) {
    const dialogVisible = ref(false);
    const dialogTitle = ref("");
    const dialogData = ref<any>(null);
    const dialogRef = ref<any>(null);
    const initValues = ref<any>(config?.initData || {});
    const { confirm } = useConfirm();

    const setInitValues = (data: any) => {
        logger.debug("[useDialog][setInitValues]", data);
        initValues.value = deepMerge(initValues.value || {}, data);
        dialogData.value = deepMerge({}, initValues.value);
    };

    const resetClick = () => {
        dialogData.value = deepMerge({}, initValues.value);
    }

    const checkChange = async () => {
        return new Promise<boolean>((resolve) => {
            const hasChange =
                JSON.stringify(dialogData.value) !==
                JSON.stringify(initValues.value);
            //没有改变，不做任何操作
            if (!hasChange) return resolve(true);
            //有改变，弹出确认框

            confirm(
                i18n.get("Message.HasDirtyData"),
                i18n.get("Message.ConfirmNotSavedTitle")
            )
                .then(() => {
                    return resolve(true);
                })
                .catch(() => {
                    return resolve(false);
                });
        });
    };

    const beforeClose = async () => {
        if (config.beforeClose && (await config.beforeClose()) === false) {
            return;
        }
        const allowClose = await checkChange();
        if (!allowClose) return;
        close();
    };

    const close = (data?: any) => {
        dialogRef.value.clear();
        dialogVisible.value = false;
        initValues.value = null;
        dialogData.value = null;
        dialogTitle.value = "";
        config.afterClose && config.afterClose(data);
    };

    const dialogProps = () => {
        return {
            titleRef: dialogTitle,
            visible: dialogVisible,
            beforeClose: async () => {
                await beforeClose();
            },
            resetClick: () => {
                resetClick();
            },
            cancelClick: () => {
                beforeClose();
            },
            okClick: () => {
                config.submit && config.submit(dialogData.value);
            },
            ...config.dialogProps,
        } as any;
    }

    return {
        dialogRef,
        dialogVisible,
        dialogTitle,
        dialogData,
        dialogProps: dialogProps(),
        setInitValues
    };
}
