<template>
    <div v-bind="$attrs" :class="['block', type]">
        <p class="title">{{  title }}</p>
        <p class="content">{{  content }}</p>
    </div>
</template>

<script setup lang="ts">

defineProps({
    type:{
        type: String,
        default: 'info'
    },
    title: {
        type: String,
        default: 'Block Title'
    },
    content: {
        type: String,
        default: 'Block Content'
    }
})
</script>

<style lang="scss" scoped>
.block {
    border-radius: 4px;
    padding: 8px 16px;
    margin: 20px 0;

    .title {
        font-weight: 700;
    }
    .content {
        font-size: 0.9em;
    }
    &.info {
        background-color: rgba(var(--el-color-info-bg-rgb));
        border-left: 5px solid rgba(var(--el-color-info-rgb));
    }

    &.success {
        background-color: rgba(var(--el-color-success-rgb));
        border-left: 5px solid rgba(var(--el-color-success-rgb));
    }

    &.warning {
        background-color: rgba(var(--el-color-warning-rgb));
        border-left: 5px solid rgba(var(--el-color-warning-rgb));
    }

    &.error {
        background-color: rgba(var(--el-color-danger-rgb));
        border-left: 5px solid rgba(var(--el-color-danger-rgb));
    }
}
</style>