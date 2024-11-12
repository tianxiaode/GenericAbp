<template>
    <div class="w-full" v-if="type !== 'button' && message">
        <el-alert :type="messageType" show-icon closable>
            <template #title>
                <span v-html="t(message, messageParams)"> </span>
            </template>
        </el-alert>
    </div>
    <el-tooltip v-if="type === 'button' && message" placement="top" :visible="visible" width="240"
        :popper-class="messageType"
        effect="form-message"
    >
        <template #content>
            <div :class="messageType === 'error' ? 'text-error' : 'text-success'">
                {{ t(message, messageParams) }}
            </div>
        </template>
            <IconButton v-bind="buttonProps" link :type="messageType === 'error' ? 'danger' : messageType" :icon="iconClass"
                @mouseenter="visible = true" @mouseleave="visible = false"
            ></IconButton>
    </el-tooltip>

</template>

<script setup lang="ts">
import { computed, PropType, ref, onMounted } from 'vue';
import { useI18n } from '~/composables';
import { isEmpty } from '~/libs';
import IconButton from '../buttons/IconButton.vue';
const { t } = useI18n();

const props = defineProps({
    message: {
        type: String
    },
    messageType: {
        type: String as PropType<'success' | 'info' | 'warning' | 'error'>,
        default: 'error'
    },
    messageParams: {
        type: Object as PropType<Record<string, any>>,
        default: () => ({})
    },
    type: {
        type: String as PropType<'button' | ''>,
        default: ''
    },
    buttonProps:{
        type: Object as PropType<Record<string, any>>,
        default: () => ({})
    }
})

const IconMap = {
    success: 'fa fa-circle-check',
    info: 'fa fa-circle-info',
    warning: 'fa fa-circle-exclamation',
    error: 'fa fa-circle-xmark'
}


const iconClass = computed(() => IconMap[props.messageType])

const visible = ref(false);


const autoClose = () => {
    visible.value = true;
    setTimeout(() => {
        visible.value = false;
    }, 2000);
}

onMounted(() => {
    if (!isEmpty(props.message)) {
        autoClose();
    }
});


</script>

<style lang="scss">
.el-popper.is-form-message{
    &.success{
        background-color: var(--el-color-success-light-9);
        .el-popper__arrow:before{
            background-color: var(--el-color-success-light-9);
        }
    }
    &.error{
        background-color: var(--el-color-error-light-9);
        .el-popper__arrow:before{
            background-color: var(--el-color-error-light-9);
        }
    }
}
</style>