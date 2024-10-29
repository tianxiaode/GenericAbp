<template>
    <div class="hero">
        <el-card style="width: 400px; max-width: 95%;">
            <template #header>
                <div class="text-center text-2xl font-bold">{{ t(title) }}</div>
            </template>
            <TenantSwitch :error="error" :success="success" ></TenantSwitch>
            <el-form ref="formRef" v-bind="$attrs" :model="formData" size="large" :inline-message="true" :validate-on-rule-change="false" :scroll-to-error="true">
                <slot></slot>
                <FormMessage v-model="formMessage" v-model:form-message-type="formMessageType" :message-params="formMessageParams"></FormMessage>
            </el-form>
            <template #footer v-if="$slots.footer">
                <slot name="footer"></slot>
            </template>
        </el-card>
    </div>
</template>

<script setup lang="ts">
import {  useFormExpose,useFormMessageExpose,useI18n } from '~/composables';
import FormMessage from './FormMessage.vue';
import TenantSwitch from './TenantSwitch.vue';

const { t } = useI18n();
const formData = defineModel();
defineProps({
    title: String,
})
const { formRef, formExpose } = useFormExpose();
const { formMessage, formMessageType,formMessageParams, formMessageExpose, error,success} = useFormMessageExpose();
defineExpose({
    ...formExpose(),
    ...formMessageExpose()
});
</script>


<style lang="scss" scoped>
.el-form .el-form-item__label{
    display: none;
}
</style>

