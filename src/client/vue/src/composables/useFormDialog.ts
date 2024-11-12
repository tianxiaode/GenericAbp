import { onMounted, ref } from "vue";
import { useDialog, UseDialogConfig } from "./useDialog";
import { logger } from "~/libs";
import { useLabel } from "./useLabel";

declare interface UseFormDialogConfig extends UseDialogConfig {
    initData?: any;
    formProps?: any;
    loadAdditionalData?: boolean;
    beforeGetData?: (id: any) => boolean;
    afterGetData?: (data: any) => void;
    beforeSubmit?: (data: any) => Promise<boolean>;
    afterSubmit?: (data: any, response: any) => Promise<void>;
    beforeClose?: () => Promise<boolean>;
    afterClose?: (data:any) => Promise<void>;
};

export function useFormDialog(
    api: any,
    entityId: any,
    config:UseFormDialogConfig={}) {

    const formRef = ref<any>(null);
    const { getLabel } = useLabel(api);

    const submit = async () => {
        if (
            config.beforeSubmit &&
            (await config.beforeSubmit(dialogData.value)) === false
        ) {
            return;
        }
        formRef.value?.validate(async (valid: boolean) => {
            if (!valid) return;
            logger.debug(
                "[useFormDialog][submit]",
                "formData:",
                dialogData.value
            );
            try {
                const result = entityId.value
                    ? await api.update(dialogData.value)
                    : await api.create(dialogData.value);
                    dialogRef.value?.success("Message.SaveSuccessAndClose");
                config.afterSubmit &&
                    config.afterSubmit(dialogData.value, result);
                setTimeout(() => {
                    close();
                }, 3000);
                return result;
            } catch (err: any) {
                dialogRef.value?.error(err.message);
            }
        });
    };

    const {
        dialogVisible,
        dialogTitlePrefix,
        dialogData,
        dialogProps,
        dialogRef,
        setInitValues,
        close,
    } = useDialog({
        submit,
        ...config,
    });

    onMounted(async ()=>{
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
                dialogTitlePrefix.value = api.updateTitle;
                setInitValues(data);
                if (config.afterGetData) {
                    config.afterGetData(data);
                }
            } catch (e) {
                logger.error("[useEntityForm][onMounted]", e);
            }
        } else {
            setInitValues(config.initData || {});
            dialogTitlePrefix.value = api.createTitle;
        }

    })


    return {
        formDialogRef: dialogRef,
        formDialogVisible: dialogVisible,
        formData: dialogData,
        formRef,        
        formDialogProps: {
            titlePrefix: "Components.MultilingualSomething",
            ...dialogProps,
        },
        formProps:{
            size: 'large',
            inlineMessage: true,
            validateOnRuleChange: false,
            scrollToError: true,
            requireAsteriskPosition: 'right',
            labelSuffix: ':',
            labelWidth: '160px',
            ...config.formProps,
        },
        getLabel,
        formSetInitValues: setInitValues,
    };

}