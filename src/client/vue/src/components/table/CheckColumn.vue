<template>
    <el-table-column v-bind="$attrs"  align="center" :prop="props.prop">
        <template #default="scope">
            <el-checkbox v-model="scope.row[props.prop]" @change="checkChange(scope.row)"></el-checkbox>
        </template>
    </el-table-column>

</template>

<script setup lang="ts">

const props = defineProps({
    prop: {
        type: String,
        required: true
    },
    checkChange: {
        type: Function,
        default: () => {}
    }
});

</script>