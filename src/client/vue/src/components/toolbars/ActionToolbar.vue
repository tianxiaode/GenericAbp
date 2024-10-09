<template>
    <div class="flex flex-col gap-2">
        <div class="flex flex-row gap-2">
            <slot name="action-items"></slot>
            <h3 class="title" style="order:100;">{{ title }}</h3> <!-- 显示父视图传递过来的 title -->
            <div class="spacer" style="order:200;"></div>
            <el-input placeholder="请输入搜索内容" prefix-icon="el-icon-search" v-model="filterQuery"
                @input="handleFilterInput" style="max-width: 200px; order:300;" clearable>
                <template #prefix>
                    <el-icon><font-awesome-icon icon="search"></font-awesome-icon></el-icon>
                </template>
                <template #suffix>
                    <el-icon v-if="showEllipsisIcon" @click="toggleAdvancedSearch" class="cursor-pointer">
                        <font-awesome-icon icon="ellipsis"></font-awesome-icon>
                    </el-icon>
                </template>
            </el-input>
            <div class="spacer" style="order: 400;"></div>
            <div v-for="(button, icon) in mergedButtons" :key="icon" :style="{ order: button.order }"
                :title="button.title">
                <IconButton v-if="button.isVisible && button.isVisible()" :type="button.type" :icon="button.icon" circle
                    :disabled="button.isDisabled && button.isDisabled()" @click="button.action()">
                </IconButton>

            </div>

        </div>

        <div v-if="isAdvancedSearchVisible" class="grid cols-4 gap-2">
            <slot name="advanced-search-items"></slot>
        </div>

        <div v-if="selectedCount > 0" class="row">
            <span>选择总数: {{ selectedCount }}</span>
            <span>选择了: {{ selectedItems.join(', ') }}</span>
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref,  computed, } from 'vue';
import IconButton from '../buttons/IconButton.vue';
import { clone, deepMerge } from '../../libs';
// 定义 props 接收父组件传递的标题、是否显示新建按钮、是否显示 ellipsis 图标
const props = defineProps({
    title: { type: String, required: true },
    showEllipsisIcon: { type: Boolean, default: false },
    buttons: {
        type: Object,  // 改为对象
        default: () => ({})
    }

});

const emit = defineEmits(['filter']);

// 默认按钮配置
const defaultButtons = {
    create: {
        type: 'success',
        icon: 'plus',
        order: 500,
        title: '新建',
        isVisible: () => true,  // 默认显示
        isDisabled: () => false  // 默认禁用条件
    }
};

// 合并自定义和默认按钮配置
const mergedButtons = computed(() => {
    // 合并每个按钮的配置，允许用户只修改部分属性
    const result = deepMerge(clone(defaultButtons), props.buttons);
    return result;
});

const isAdvancedSearchVisible = ref(false);
const selectedCount = ref(0);
const selectedItems = ref<string[]>([]);
const filterQuery = ref(''); // 新增搜索查询变量

function toggleAdvancedSearch() {
    isAdvancedSearchVisible.value = !isAdvancedSearchVisible.value;
}


// 处理搜索框输入事件
const handleFilterInput = () => {
    emit('filter', filterQuery.value); // 触发 search 事件并传递当前搜索值
}
</script>

<style scoped>
.title {
    margin: 0;
    /* 去掉默认的外边距 */
    padding: 0;
    /* 去掉默认的内边距 */
    font-size: 32px;
    /* 根据需要设置字体大小 */
    line-height: 40px;
}
</style>
