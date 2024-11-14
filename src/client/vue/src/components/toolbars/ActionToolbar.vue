<template>
    <div class="flex flex-col gap-2">
        <div class="flex flex-row items-center gap-1">
            <slot name="action-items"></slot>
            <h3 class="title" style="order:100;">{{ title }}</h3> <!-- 显示父视图传递过来的 title -->
            <div class="spacer" style="order:200;"></div>
            <Search :filter="filter" :show-ellipsis-icon="showEllipsisIcon" :ellipsis-icon-click="toggleAdvancedSearch" 
                style="max-width: 200px; order: 300;"
            />
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
import Search from '../forms/Search.vue';
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
    },
    filter:{
        type: Function,
        default: () => {}
    }

});



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

function toggleAdvancedSearch() {
    isAdvancedSearchVisible.value = !isAdvancedSearchVisible.value;
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
