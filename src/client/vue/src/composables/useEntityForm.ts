import { onMounted, ref } from "vue";
import { i18n } from "~/libs";
import { useConfirm } from "./useConfirm";

export function useEntityForm(api: any, props: any) {
    const dialogTitle = ref<string>("");
    const formRef = ref<any>(null as any);
    const formData = ref<any>({});
    const dialogVisible = ref(props.visible);
    const initValues = ref<any>({});
    const { confirm } = useConfirm();

    const submitForm = async () => {
        formRef.value?.validate(async (valid: boolean) => {
            if (!valid) return;
            try {
                const result = props.entityId
                    ? await api.update(formData.value)
                    : await api.create(formData.value);
                formRef.value?.success("Message.SaveSuccessAndClose");
                setTimeout(() => {
                    formRef.value?.close();
                }, 3000);
                return result;
            } catch (err: any) {
                formRef.value?.error(err.message);
            }
        });
    };

    const resetForm = () => {
        formData.value = initValues.value;
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

    

    onMounted(() => {
        if (props.entityId) {
            api.getEntity(props.entityId).then((res: any) => {
                formData.value = { ...res };
                initValues.value = { ...res };
                dialogTitle.value = api.updateTitle;
            });
        } else {
            formData.value = {};
            initValues.value = {};
            dialogTitle.value = api.createTitle;
        }
    });

    return {
        formRef,
        formData,
        initValues,
        dialogVisible,
        dialogTitle,
        checkChange,
        resetForm,
        submitForm,
    };
}
