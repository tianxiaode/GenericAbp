<template>
    <el-table-column v-bind="$attrs" :label="label" align="center">
        <template #default="scope">
            <div class="flex justify-center items-center gap-2">
                <!-- 按钮循环渲染 -->
                <div v-for="(button, icon) in mergedButtons" :key="icon" :style="{ order: button.order }" :title="button.title">
                    <IconButton
                        v-if="!button.isVisible || button.isVisible()"
                        :type="button.type"
                        :icon="button.icon"
                        circle
                        size="small"
                        :disabled="button.isDisabled && button.isDisabled(scope.row)" 
                        @click="button.action(scope.row)"
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
import { clone, deepMerge } from '../../libs';
import IconButton from '../buttons/IconButton.vue';
import { computed } from 'vue';

// 定义 props
const props = defineProps({
    label: {
        type: String,
        default: '操作'
    },
    buttons: {
        type: Object,  // 改为对象
        default: () => ({})
    }
});

// 默认的编辑和删除按钮配置
const defaultButtons = {
    edit: {
        type: 'primary',
        icon: 'fa fa-edit',
        order: 100,
        title: '编辑',
        isVisible: () => true,  // 默认显示
        isDisabled: (row:any) => row.status === 'inactive'  // 默认禁用条件
    },
    delete: {
        type: 'danger',
        icon: 'fa fa-trash',
        order: 200,
        title: '删除',
        isVisible: () => true,
        isDisabled: (row:any) => row.isProtected === true
    }
};

// 合并自定义和默认按钮配置
const mergedButtons = computed(() => {
    // 合并每个按钮的配置，允许用户只修改部分属性
    const result = deepMerge(clone(defaultButtons), props.buttons);
    return result;
});
</script>

