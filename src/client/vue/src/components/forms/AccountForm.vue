<template>
    <div class="hero">
        <el-card style="width: 400px;">
            <template #header>
                <div class="text-center text-2xl font-bold">{{ t(title) }}</div>
            </template>
            <el-form ref="formRef" v-bind="$attrs" :model="formData" size="large">
                <slot></slot>
                <div class="w-full pt-2" v-if="formMessage">
                    <el-alert :title="t(formMessage)" :type="formMessageType" show-icon closable style="margin-top: 10px;"
                        @close="clearFormMessage"></el-alert>
                </div>
            </el-form>
            <template #footer v-if="$slots.footer">
                <slot name="footer"></slot>
            </template>
        </el-card>
    </div>
</template>

<script setup lang="ts">
import {  useFormData, useFormExpose,useFormMessage,useI18n } from '~/composables';

const { t } = useI18n();
const props = defineProps({
    title: String,
    formData: Object,    
})

const { formRef, formExpose } = useFormExpose();
const { formData, formDataExpose } = useFormData(props.formData)
const { formMessage, formMessageType, clearFormMessage,formMessageExposed } = useFormMessage();

defineExpose({
    ...formDataExpose(),
    ...formExpose(),
    ...formMessageExposed()
});
</script>


<style lang="scss" scoped>
.el-form .el-form-item__label{
    display: none;
}
</style>

