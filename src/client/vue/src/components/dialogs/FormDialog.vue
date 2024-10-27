<template>
    <el-dialog :ref="dialogRef" v-bind="dialogProps" :title="t(title)" v-model="dialogVisible"
        :beforeClose="beforeClose" destroy-on-close align-center>
        <el-form :ref="formRef" v-bind="formProps" :model="formData" size="large" :inline-message="true"
            :label-width="labelWidth" :rules="rules" :validate-on-rule-change="false" :scroll-to-error="true"
            require-asterisk-position="right" label-suffix=":">
            <el-input type="hidden" v-model="formData.id" />
            <el-input type="hidden" v-model="formData.concurrencyStamp" />
            <slot name="form-items"></slot>
        </el-form>

        <template #footer>
            <slot name="dialog-footer">
                <div class="flex-1 flex items-center gap-2">
                    <MessageButton :ref="messageRef" style="order: 100;" circle class="none-border"></MessageButton>
                    <IconButton icon="fa fa-undo" @click="resetClick" style="order: 200;" circle class="none-border"
                        title="Components.Reset">
                    </IconButton>
                    <span class="flex-grow" style="order: 300;"></span>
                    <el-button style="order: 400;" @click="cancelClick">{{ t(cancelText) }}</el-button>
                    <el-button type="primary" style="order: 500;" @click="okClick">{{ t(okText) }}</el-button>
                </div>
            </slot>
        </template>
    </el-dialog>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';
import MessageButton from '../buttons/MessageButton.vue';
import IconButton from '../buttons/IconButton.vue';
import { PropType } from 'vue';


defineProps({
    dialogRef: {
        type: Object,
        required: true
    },
    dialogProps: {
        type: Object,
        default: () => ({})
    },
    title: {
        type: String,
        default: ''
    },
    formRef: {
        type: Object,
        required: true
    },
    formProps: {
        type: Object,
        default: () => ({})
    },
    labelWidth: {
        type: String,
        default: '180px'
    },
    messageRef: {
        type: Object as PropType<any>,
        required: true
    },
    beforeClose: {
        type: Function,
        default: () => true
    },
    resetClick: {
        type: Function as PropType<() => void>,
        default: () => { }
    },
    cancelText: {
        type: String,
        default: 'Components.Cancel'
    },
    cancelClick: {
        type: Function as PropType<() => void>,
        default: () => { }
    },
    okText: {
        type: String,
        default: 'Components.Ok'
    },
    okClick: {
        type: Function as PropType<() => void>,
        default: () => { }
    }

})

const { t } = useI18n();
const formData = defineModel<any>();
const rules = defineModel<any>('rules');
const dialogVisible = defineModel<boolean>('visible')
</script>
