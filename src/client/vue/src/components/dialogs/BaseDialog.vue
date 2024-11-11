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
                    <MessageButton ref="messageRef" style="order: 100;" circle class="none-border"></MessageButton>
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
import { PropType, ref } from 'vue';

const messageRef = ref<any>();
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

defineExpose({
    success(message: string){
        messageRef.value.success(message);
    },
    error(message: string){
        messageRef.value.error(message);
    },
    clear(){
        me
    }
})

const { t } = useI18n();
</script>
