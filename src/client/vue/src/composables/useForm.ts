// useForm.ts
import { onMounted, ref } from "vue";
import { clone } from "../libs";

export function useForm<T>(initialData: T, api: any, props: any) {
    
    const formRef = ref<Exp>(null);
    const formData = ref<T>(clone(initialData));
    const initialValues = ref<T>(clone(initialData));

    // Handle form submission
    const handleSubmit = async () => {
        return formData.value.id ? await api.update(formData.value) : await api.create(formData.value);
    };

    // Check if form data has changed
    const hasChanged = () => {
        return (
            JSON.stringify(initialValues.value) !==
            JSON.stringify(formData.value)
        );
    };

    // Reset the form to its initial values
    const resetForm = () => {
        formData.value = clone(initialValues.value);
    };

    // Validate the form
    const validateForm = () => {
        return new Promise((resolve) => {
            formRef.value?.validate((valid: boolean) => {
                if (valid) {
                    resolve(true); // 验证通过，解析 Promise
                } else {
                    resolve(false); // 验证未通过，解析 Promise
                }
            });
        });
    };

    // Set new initial values (for editing)
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

    return {
        initialValues,
        formRef,
        formData,
        handleSubmit,
        hasChanged,
        resetForm,
        validateForm,
        setInitialValues,
    };
}
