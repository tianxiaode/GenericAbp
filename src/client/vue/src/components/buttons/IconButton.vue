<template>
    <el-button v-bind="$attrs">
        <template #icon>
            <i :class="icon"></i>
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
