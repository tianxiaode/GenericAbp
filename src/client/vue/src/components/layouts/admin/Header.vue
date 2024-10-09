<template>
    <el-header class="header">
        <div :class="['logo-wrap', { collapse }]">
            <img src='/logo.png' alt="Logo" class="logo-icon" />
            <span v-if="!collapse" class="logo-text">管理</span> <!-- 当不折叠时显示文字 -->
        </div>
        <el-icon v-if="showCollapseToggleIcon" class="trigger" @click="toggleCollapse" :rotate="collapse ? 'el-icon-s-fold' : 'el-icon-s-unfold'">
            <template v-if="collapse">
                <Expand />
            </template>
            <template v-else>
                <Fold />
            </template>
        </el-icon>
        <div class="spacer"></div>
    </el-header>

</template>

<script setup lang="ts">
import { ref } from 'vue';
import { Fold, Expand } from '@element-plus/icons-vue';

defineProps({
    showCollapseToggleIcon: {
        type: Boolean,
        default: false
    }
})
const collapse = ref(false);

const toggleCollapse = () => {
    collapse.value =!collapse.value
}
</script>

<style lang="scss" scoped>
.header {
    height: 60px;
    background-color: #333;
    color: #fff;
    line-height: 60px;
    padding: 0 20px;
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.logo-wrap {
    display: flex;
    align-items: center;
    /* 垂直居中对齐 */
    font-size: 24px;
    font-weight: bold;
    width: 200px;
    color: #fff;
    transition: width 0.3s;
    /* 添加平滑过渡效果 */
}

.logo-icon {
    width: 30px;
    /* 设置图标的宽度 */
    height: 30px;
    /* 设置图标的高度 */
    margin-right: 8px;
    background: transparent;
    /* 图标与文字之间的间距 */
}

.logo-wrap.collapse {
    width: 60px;
    justify-content: center;
    /* 折叠时水平居中对齐 */
}
</style>