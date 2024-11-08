import { onMounted, ref } from "vue";
import {  deepMerge, i18n, logger } from "~/libs";
import { useConfirm } from "./useConfirm";
import { useLabel } from "./useLabel";

declare type EntityFormConfigType = {
    initData?: any;
    props?: {
        dialogProps?: any;
        labelWidth?: string;
        cancelText?: string;
        okText?: string;
    };
    loadAdditionalData?: boolean;
    beforeGetData?: (id: any) => boolean;
    afterGetData?: (data: any) => void;
    beforeSubmit?: (data: any) => Promise<boolean>;
    afterSubmit?: (data: any, response: any) => Promise<void>;
    beforeClose?: () => Promise<boolean>;
    afterClose?: (data:any) => Promise<void>;
};

export function useEntityForm(
    api: any,
    entityId: any,
    dialogVisible: any,
    config?: EntityFormConfigType
) {
    const dialogTitle = ref<string>("");
    const dialogRef = ref<any>(null);
    const formRef = ref<any>(null as any);
    const formData = ref<any>({});
    const messageRef = ref<any>(null);
    const initValues = ref<any>({});
    const { confirm } = useConfirm();

    const { getLabel } = useLabel(api);

    config = config || ({} as EntityFormConfigType);

    const formDialogProps = () => {
        return {
            dialogRef: dialogRef,
            title: dialogTitle,
            formRef: formRef,
            messageRef: messageRef,
            beforeClose: async () => {
                await beforeCLose();
            },
            resetClick: () => {
                resetForm();
            },
            cancelClick: () => {
                beforeCLose();
            },
            okClick: () => {
                submitForm();
            },
            ...config.props,
        };
    };

    const beforeCLose = async () => {
        if (config.beforeClose && (await config.beforeClose()) === false) {
            return;
        }
        const allowClose = await checkChange();
        if (!allowClose) return;
        close();
    };

    const close = (data?: any) => {
        messageRef.value.clear();
        dialogVisible.value = false;
        config.afterClose && config.afterClose(data);
    };

    const submitForm = async () => {
        if (
            config.beforeSubmit &&
            (await config.beforeSubmit(formData.value)) === false
        ) {
            return;
        }
        formRef.value?.validate(async (valid: boolean) => {
            if (!valid) return;
            logger.debug(
                "[useEntityForm][submitForm]",
                "formData:",
                formData.value
            );
            try {
                const result = entityId.value
                    ? await api.update(formData.value)
                    : await api.create(formData.value);
                messageRef.value?.success("Message.SaveSuccessAndClose");
                config.afterSubmit &&
                    config.afterSubmit(formData.value, result);
                setTimeout(() => {
                    close(result);
                }, 3000);
                return result;
            } catch (err: any) {
                messageRef.value?.error(err.message);
            }
        });
    };

    const resetForm = () => {
        formData.value = initValues.value;
    };

    const setInitValues = (data: any) => {
        logger.debug("[useEntityForm][setInitValues]", data);
        initValues.value = deepMerge(initValues.value, data);
        formData.value = deepMerge(formData.value, initValues.value);
    };

    const checkChange = async () => {
        return new Promise<boolean>((resolve) => {
            const hasChange =
                JSON.stringify(formData.value) !==
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

    onMounted(async () => {
        if (entityId.value) {
            try {
                if (
                    config.beforeGetData &&
                    config.beforeGetData(entityId.value) === false
                )
                    return;
                const loadAdditionalData =
                    config.loadAdditionalData === true ? true : false;
                let data = await api.getEntity(
                    entityId.value,
                    loadAdditionalData
                );
                dialogTitle.value = api.updateTitle;
                setInitValues(data);
                if (config.afterGetData) {
                    config.afterGetData(data);
                }
            } catch (e) {
                logger.error("[useEntityForm][onMounted]", e);
            }
        } else {
            setInitValues(config.initData || {});
            dialogTitle.value = api.createTitle;
        }
    });

    return {
        dialogTitle,
        dialogRef,
        formRef,
        formData,
        messageRef,
        initValues,
        formDialogProps: formDialogProps(),
        getLabel,
        checkChange,
        resetForm,
        submitForm,
        setInitValues,
    };
}
