<template>
    <el-dialog ref="dialogRef" :title="t(title)" v-model="dialogVisible" :before-close="beforeDialogClose"
        @close="dialogClose" destroy-on-close>
        <el-form ref="formRef" :rules="rules" :model="formData" size="large" :inline-message="true"
            :validate-on-rule-change="false" :scroll-to-error="true" require-asterisk-position="right">
            <el-input type="hidden" v-model="formData.id" />
            <el-input type="hidden" v-model="formData.concurrencyStamp" />
            <slot name="form-items"></slot>
        </el-form>

        <template #footer>
            <slot name="dialog-footer">
                <MessageButton v-if="formMessage" :message="formMessage" :type="formMessageType" style="order: 100;"
                    circle class="none-border"></MessageButton>
                <IconButton icon="fa fa-undo" @click="resetForm" style="order: 200;" circle class="none-border">
                </IconButton>
                <span class="flex-grow" style="order: 300;"></span>
                <el-button style="order: 400;" @click="beforeDialogClose">{{ t(cancelButtonText) }}</el-button>
                <el-button type="primary" @click="onOk" style="order: 500;">{{ t(okButtonText) }}</el-button>
            </slot>
        </template>
    </el-dialog>
</template>

<script setup lang="ts">
import { useFormMessage, useFormExpose, useI18n, dialogProps, useDialogExpose } from '~/composables';
import MessageButton from '../buttons/MessageButton.vue';
import IconButton from '../buttons/IconButton.vue';
import { ElMessageBox } from 'element-plus';


const props = defineProps({
    title: String,
    rules: Object,
    modelValue: {
        type: Object,
        required: true,
    },
    ...dialogProps()
})

const { t } = useI18n();
const emit = defineEmits(['update:modelValue']);

const onBeforeClose = async () => {
    return new Promise((resolve) => {
        if (!hasChange()) {
            return resolve(true);
        }
        ElMessageBox.confirm(
            t.value('AbpUi.AreYouSureYouWantToCancelEditingWarningMessage'),
            t.value('AbpUi.AreYouSure')).then(() => {
                return resolve(true);
            }).catch(() => {
                return resolve(false);
            })
    })
}

const onAfterClose = () => {
    clearFormMessage();
}


const { formRef, formData, hasChange, resetForm, formExpose } = useFormExpose(props, emit);
const { formMessage, formMessageType, clearFormMessage, formMessageExposed } = useFormMessage();
const { dialogRef, dialogVisible, beforeDialogClose, dialogClose } = useDialogExpose(props, onBeforeClose, onAfterClose);


defineExpose({
    ...formExpose(),
    ...formMessageExposed()
});
</script>
