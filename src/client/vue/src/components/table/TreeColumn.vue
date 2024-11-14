<template>
    <CustomSortColumn v-bind="$attrs">
        <template #default="{ row, column }">
            <TreeNode :data="row" :text="row[column.property]" :filter-text="filterText" :expand="expand"></TreeNode>
        </template>
    </CustomSortColumn>
</template>


<script setup lang="ts">
import TreeNode from '../tree/TreeNode.vue';
import CustomSortColumn from './CustomSortColumn.vue';

defineProps({
    filterText: {
        type: String,
        default: ''
    },
    expand: {
        type: Function,
        default: () => { }
    }
});
</script>