<template>
    <el-button v-bind="$attrs">
        <template #icon>
            <font-awesome-icon :icon="icon" />
        </template>
    </el-button>
</template>

<script setup lang="ts">

defineProps({
    icon: {
        type: String,
        required: true
    }
});

</script>
