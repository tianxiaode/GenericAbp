<template>
    <el-table-column v-bind="$attrs"  align="center">
        <template #default="{row, column}">
            <el-checkbox v-model="row[column.property]" @change="checkChange(row)"></el-checkbox>
        </template>
    </el-table-column>

</template>

<script setup lang="ts">

defineProps({
    checkChange: {
        type: Function,
        default: () => {}
    }
});

</script>