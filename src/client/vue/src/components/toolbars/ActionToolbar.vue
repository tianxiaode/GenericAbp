<template>
    <div class="flex flex-col gap-2">
        <div class="flex flex-row items-center gap-1">
            <slot name="action-items"></slot>
            <h3 class="title" style="order:100;">{{ title }}</h3> <!-- 显示父视图传递过来的 title -->
            <div class="spacer" style="order:200;"></div>
            <el-input :placeholder="t('Components.Search')" prefix-icon="el-icon-search" v-model="filterQuery"
                @input="handleFilterInput" style="max-width: 200px; order:300;" clearable>
                <template #prefix>                    
                    <el-icon><i class="fa fa-search"></i></el-icon>
                </template>
                <template #suffix>
                    <el-icon v-if="showEllipsisIcon" @click="toggleAdvancedSearch" class="cursor-pointer">
                        <i class="fa fa-ellipsis-v"></i>
                    </el-icon>
                </template>
            </el-input>
            <div class="spacer" style="order: 400;"></div>
            <div v-for="(button) in buttonsList" :style="{ order: button.order }">
                <IconButton v-if="handleButtonVisibility(button)" 
                v-bind="button"
                :disabled="handleButtonDisabled(button)" @click="handleButtonClick(button)">
                </IconButton>

            </div>

        </div>

        <div v-if="isAdvancedSearchVisible" class="grid cols-4 gap-2">
            <slot name="advanced-search-items"></slot>
        </div>

        <transition name="fade" mode="out-in">
        <div v-if="selected.length > 0" class="flex flex-row items-center gap-1 my-2">
            <slot name="selected-actions">
                <div class="flex-1">{{ t("Components.SelectedMessage", { count: selected.length }) }}</div>
            </slot>
        </div>
        </transition>
    </div>
</template>

<script setup lang="ts">
import { ref,  } from 'vue';
import IconButton from '../buttons/IconButton.vue';
import { useActionButtons, useI18n } from '~/composables';
// 定义 props 接收父组件传递的标题、是否显示新建按钮、是否显示 ellipsis 图标
const props = defineProps({
    title: { type: String, default: '' },
    showEllipsisIcon: { type: Boolean, default: false },
    buttons: {
        type: Object,  // 改为对象
        default: () => ({})
    },
    selected:{
        type: Array,
        default: () => []
    }

});



const emit = defineEmits(['filter']);
const {t} = useI18n();

// 默认按钮配置
const defaultButtons = {
    create: {
        type: 'success',
        icon: 'fa fa-plus font-size-6',
        order: 500,
        title: 'Components.New',
        visible: true,  // 默认显示
        disable: false  // 默认禁用条件
    }
};

const { buttonsList, handleButtonClick, handleButtonDisabled, handleButtonVisibility } = useActionButtons(defaultButtons,props.buttons);
const isAdvancedSearchVisible = ref(false);
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
