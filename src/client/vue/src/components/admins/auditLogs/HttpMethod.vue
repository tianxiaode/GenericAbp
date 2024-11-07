<template>  
    <span :class="`ml-2 font-bold  text-${value.toLowerCase()}`">{{ value }}</span>
</template>

<script setup lang="ts">

defineProps({
    value: {
        type: String,
        required: true,
        default: 'GET'
    }
})

</script>

<style lang="scss">
.text-get{
    color: var(--el-color-info);
}
.text-post,
.text-put,
.text-patch
{
    color: var(--el-color-primary);
}
.text-delete{
    color: var(--el-color-danger);
}
</style>