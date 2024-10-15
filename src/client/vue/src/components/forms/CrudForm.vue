<template>
    <el-form ref="formRef" :model="formData" v-bind="$attrs">
        <el-input type="hidden" v-if="formData.id" :value="formData.id" />
        <el-input type="hidden" v-if="formData.concurrencyStamp" :value="formData.concurrencyStamp" />
        <slot></slot>
    </el-form>
</template>


<script setup lang="ts">
import { onMounted } from 'vue';
import {  useFormData, useFormExpose } from '~/composables';
import { EntityInterface, Repository } from '~/libs';

const props = defineProps({
    entityId: String,
    api: Object,
    formData: Object,
})

const { formRef, formExpose } = useFormExpose();
const { formData, setInitValues,  formDataExpose } = useFormData(props.formData)


defineExpose({
    ...formDataExpose,
    ...formExpose
});

onMounted(() => {
    if (props.entityId) {
        (props.api as Repository<EntityInterface>).getEntity(props.entityId).then((res: EntityInterface) => {
            setInitValues(res);
        });
    }
});

</script>