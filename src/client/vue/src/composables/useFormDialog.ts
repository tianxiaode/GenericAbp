import { onMounted, ref,reactive } from "vue";
import { clone } from "~/libs";

export function useFormDialog(api: any, props: any) {
    const dialogTitle = ref<string>("");
    const formRef = ref<any>(null as any);
    const formData = ref<any>({});
    const dialogVisible = ref(props.visible);


    const submitForm = async () => {
        formRef.value?.validate(async (valid:boolean)=>{
            if(!valid) return;
            try {
                const result = await submitForm();
                formRef.value?.success('SaveSuccessAndClose');
                return result;
            } catch (err:any) {
                formRef.value?.error(err.message);
            }
        })

        if (!api) return;
        return props.entityId
            ? api.update(formData.value)
            : api.create(formData.value);
    };


    onMounted(() => {
        if (props.entityId) {
            api.getEntity(props.entityId).then((res: any) => {
                console.log('onMounted getEntity',res, clone(res));
                formRef.value.setInitValue(res);
                dialogTitle.value = api.updateTitle;
            });
        }else{
            formRef.value.setInitValue({});
            dialogTitle.value = api.createTitle;
        }
    });

    return {
        formRef,
        formData,
        dialogVisible,
        dialogTitle,
        submitForm
    }
}
