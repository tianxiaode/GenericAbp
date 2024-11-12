<template>
    <el-dialog v-bind="{
        destroyOnClose: true,
        alignCenter: true,
        ...$attrs        
    }" :title="t(titlePrefix, {title: titleRef.value})" v-model="dialogVisible.value">
        <slot name="default"></slot>
        <template #footer>
            <slot name="dialog-footer">
                <div class="flex-1 flex items-center gap-2">
                    <slot name="dialog-actions"></slot>
                    <FormMessage v-if="formMessage" :message="formMessage" :message-type="formMessageType" :message-params="formMessageParams" type="button" :button-props="{style: 'order:100;'}" ></FormMessage>
                    <IconButton link icon="fa fa-undo" @click="resetClick" style="order: 200;"
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
import { useFormMessageExpose, useI18n } from '~/composables';
import IconButton from '../buttons/IconButton.vue';
import { PropType } from 'vue';
import FormMessage from '../forms/FormMessage.vue';


const dialogVisible = defineModel<any>('visible');
defineProps({
    titlePrefix: {
        type: String,
        default: ''    
    },
    titleRef:{
        type: Object,
        default: () => ({ value: '' })
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
        default: 'Components.Save'
    },
    okClick: {
        type: Function as PropType<() => void>,
        default: () => { }
    }
})

const { formMessage, formMessageType, formMessageParams, formMessageExpose} = useFormMessageExpose();

defineExpose({
    ...formMessageExpose()
})

const { t } = useI18n();
</script>
