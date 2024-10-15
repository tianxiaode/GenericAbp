import { ref } from "vue";
import { clone } from "~/libs";

export function useCrudForm(formData: any, props: any) {
    const form = ref(formData);
    const initialValues = ref<any>(clone(props.initialData));

    const setInitialValues = (newValues: T) => {
        formData.value = { ...formData.value, ...clone(newValues) };
        initialValues.value = { ...initialValues.value, ...clone(newValues) };
    };

    onMounted(() => {
        if (props.entityId) {
            api.getSingle(props.entityId).then((res: T) => {
                setInitialValues(res);
            });
        }
    });


}