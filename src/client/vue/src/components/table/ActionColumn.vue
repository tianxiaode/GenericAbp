<template>
    <el-table-column v-bind="$attrs" :label="t(label)" align="center">
        <template #default="scope">
            <div class="flex justify-center items-center gap-2">
                <!-- 按钮循环渲染 -->
                <div v-for="(button, icon) in buttonsList" :key="icon" :style="{ order: button.order }">
                    <IconButton
                        v-if="handleButtonVisibility(button,scope.row)"
                        v-bind="button"
                        size="small"
                        :disabled="handleButtonDisabled(button,scope.row)" 
                        @click="handleButtonClick(button, scope.row)"
                        :id="`button-${scope.row.id}-${button.icon}`"
                    ></IconButton>  
                </div>
                <!-- 插槽 -->
                <slot name="action" :row="scope.row" />
            </div>
        </template>
    </el-table-column>
</template>

<script setup lang="ts">
import { useActionButtons, useI18n } from '~/composables';
import IconButton from '../buttons/IconButton.vue';

// 定义 props
const props = defineProps({
    label: {
        type: String,
        default: 'Components.Action'
    },
    buttons: {
        type: Object,  // 改为对象
        default: () => ({})
    }
});

// 默认的编辑和删除按钮配置
const defaultButtons = {
    detail:{
        type: 'default',
        icon: 'fa fa-ellipsis', 
        title: 'Components.Detail',
        order: 1        
    },
    edit: {
        type: 'primary',
        icon: 'fa fa-edit',
        order: 100,
        title: 'Components.Edit',
        visible: true,  // 默认显示
        disable: false,  // 默认不禁用
    },
    delete: {
        type: 'danger',
        icon: 'fa fa-trash',
        order: 200,
        title: 'Components.Delete',
        visible: true,
        disable: false,
    }
}

const {t} = useI18n();
const { buttonsList, handleButtonClick, handleButtonDisabled, handleButtonVisibility } = useActionButtons(defaultButtons,props.buttons);


</script>

