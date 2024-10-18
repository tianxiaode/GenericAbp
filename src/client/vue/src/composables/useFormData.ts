import {  ref } from "vue";
import { clone } from "~/libs";

export function useFormData(parentFormData: any) {
    const formData = ref<any>(parentFormData);
    const initValues = ref<any>({});

    const setInitValues = (data: any) => {
        formData.value = { ...formData.value, ...clone(data) };
        initValues.value = { ...initValues.value,  ...clone(data) };
    };

    const hasChange = () => {
        return JSON.stringify(formData.value) !== JSON.stringify(initValues.value);
    };    

    const resetData = () => {
        console.log("resetData", initValues.value, formData.value);
        formData.value = {...initValues.value };
        parentFormData.value.name = "";
    };


    const formDataExpose = () => {
        return {
            setInitValues,
            hasChange,
        };
    };

    return {
        formData,
        resetData,
        setInitValues,
        hasChange,
        formDataExpose,
    };
}
