<template>
    <el-dialog :title="t(title)" v-bind="$attrs" :before-close="dialogBeforeClose" destroy-on-close align-center>
        <el-form ref="formRef" :rules="rules" :model="formData" size="large" :inline-message="true"
            :validate-on-rule-change="false" :scroll-to-error="true" require-asterisk-position="right"
            :label-width="labelWidth"
            label-suffix=":"
        >
            <el-input type="hidden" v-model="formData.id" />
            <el-input type="hidden" v-model="formData.concurrencyStamp" />
            <slot name="form-items"></slot>
        </el-form>

        <template #footer>
            <slot name="dialog-footer">
                <div class="flex-1 flex items-center gap-2">
                    <MessageButton v-if="formMessage" :message="formMessage" :type="formMessageType" style="order: 100;"
                        circle class="none-border"></MessageButton>
                    <IconButton icon="fa fa-undo" @click="handleReset" style="order: 200;" circle class="none-border">
                    </IconButton>
                    <span class="flex-grow" style="order: 300;"></span>
                    <el-button style="order: 400;" @click="dialogBeforeClose">{{ t(cancelButtonText) }}</el-button>
                    <el-button type="primary" @click="onOk" style="order: 500;">{{ t(okButtonText) }}</el-button>
                </div>
            </slot>
        </template>
    </el-dialog>
</template>

<script setup lang="ts">
import { useFormMessage, useFormExpose, useI18n, dialogProps, useDialog } from '~/composables';
import MessageButton from '../buttons/MessageButton.vue';
import IconButton from '../buttons/IconButton.vue';


const props = defineProps({
    title: String,
    labelWidth: {
        type: Number,
        default: 140,
    },
    rules: Object,
    formData: {
        type: Object,
        required: true,
    },
    ...dialogProps()
})

const { t } = useI18n();
const emit = defineEmits(['update:formData', 'close']);



const { formRef, formData, formExpose } = useFormExpose(props, emit);
const { formMessage, formMessageType, clearFormMessage, formMessageExposed } = useFormMessage();
const { dialogBeforeClose, close, handleReset } = useDialog(props, emit, clearFormMessage);

defineExpose({
    ...formExpose(),
    ...formMessageExposed(),
    close: ()=>close()
});
</script>
