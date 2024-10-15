import { ref } from "vue";
import { clone } from "~/libs";

export function useFormData(parentFormData: any) {
    const formData = ref<any>(parentFormData);
    const initValues = ref<any>({} as any);

    const formDataProps = () => {
        return {
            formData: Object,
        };
    };

    const setInitValues = (data: any) => {
        formData.value = { ...formData.value, ...clone(data) };
        initValues.value = { ...initValues.value, ...clone(data) };
    };

    const formDataExpose = () => {
        return {
            hasChange: () => {
                return (
                    JSON.stringify(formData.value) !==
                    JSON.stringify(initValues.value)
                );
            },
            resetForm: () => {
                formData.value = clone(initValues.value);
            },
        };
    };

    return { formData, setInitValues, formDataProps, formDataExpose };
}
