<template>
    <div class="hero">
        <el-card style="width: 400px; max-width: 95%;">
            <template #header>
                <div class="text-center text-2xl font-bold">{{ t(title) }}</div>
            </template>
            <el-form ref="formRef" v-bind="$attrs" :model="formData" size="large" :inline-message="true" :validate-on-rule-change="false" :scroll-to-error="true">
                <slot></slot>
                <div class="w-full pt-2" v-if="formMessage">
                    <el-alert :type="formMessageType" show-icon closable style="margin-top: 10px;"
                        @close="clearFormMessage">
                        <template #title>
                            <span v-html="t(formMessage)">    </span>
                        </template>
                    </el-alert>
                </div>
            </el-form>
            <template #footer v-if="$slots.footer">
                <slot name="footer"></slot>
            </template>
        </el-card>
    </div>
</template>

<script setup lang="ts">
import {  useFormExpose,useFormMessage,useI18n } from '~/composables';

const { t } = useI18n();
const props = defineProps({
    title: String,
    formData: {
        type: Object,
        required: true,
    },
})
const emit = defineEmits(['update:formData']);
const { formRef, formData, formExpose } = useFormExpose(props, emit);
const { formMessage, formMessageType, clearFormMessage,formMessageExposed } = useFormMessage();

defineExpose({
    ...formExpose(),
    ...formMessageExposed()
});
</script>


<style lang="scss" scoped>
.el-form .el-form-item__label{
    display: none;
}
</style>

