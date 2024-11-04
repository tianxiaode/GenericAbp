<template>
    <el-form v-bind="{size: 'large', ...$attrs  }" :model="formData" :rules="rules"  ref="formRef">
        <div v-for="item in formConfig.items">

        </div>
    </el-form>
</template>

<script setup lang="ts">
import { PropType, ref } from "vue";

declare interface FormType {
    api: any; // 接口
    items: FormItemType[]; // 表单项
}

const formRef= defineModel('formRef');
const formData = defineModel();

defineProps({
    formConfig: {
        type: Object as PropType<FormType>,
        required: true,
        default: () => ({})
    },
    rules:{
        type: Object as PropType<any>,
        default: () => ({})
    }
})

</script>