<template>
    <div class="hero">
        <el-card style="width: 400px;">
            <template #header>
                <div class="text-center text-2xl font-bold">{{ t(title) }}</div>
            </template>
            <!-- 直接绑定 $attrs 并暴露 form 的 ref -->
            <el-form v-bind="$attrs" ref="formRef" size="large">
                <slot></slot>
                <div class="w-full pt-2" v-if="message">
                    <el-alert :title="t(message)" :type="alertType" show-icon closable style="margin-top: 10px;"
                        @close="clearMessage"></el-alert>
                </div>
            </el-form>
            <template #footer v-if="$slots.footer">
                <slot name="footer"></slot>
            </template>
        </el-card>
    </div>
</template>

<script setup lang="ts">
import { useFormExpose, useI18n } from '~/composables';

// 使用 defineProps 定义传递的属性
defineProps({
    title: {
        type: String,
        default: ""
    },
    message: {
        type: String,
        default: ""
    },
    alertType: {
        type: String,
        default: ""
    },
    clearMessage: {
        type: Function,
        default: () => {}
    }
});


const { t } = useI18n();

const {formRef} = useFormExpose();
</script>
