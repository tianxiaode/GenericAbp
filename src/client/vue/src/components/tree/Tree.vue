<template>
    <div v-bind="{
        class:'w-full',
        ...$attrs}" >
        <Search v-if="showSearch" :filter="filter" class="mb-2 w-full" ></Search>
        <div class="el-tree w-full">
            <div v-for="row in data" :class="[
                'el-tree-node',
                (row as any)[api.idFieldName] === currentRowId && 'is-current',
            ]" @click="selectRow(row)">
                <TreeNode :data="row" :text="(row as any)[displayField]" :expand="expandNode" :filter-text="filterText"
                    class="el-tree-node__content">
                </TreeNode>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import { useReadOnlyTree, useTree } from '~/composables';
import TreeNode from './TreeNode.vue';
import Search from '../forms/Search.vue';
import { PropType, ref } from 'vue';
import { isEmpty } from '~/libs';

const currentRow = defineModel<any>();
const currentRowId = ref('');
const props = defineProps({
    api: {
        type: Object as PropType<any>,
    },
    displayField: {
        type: String,
        default: 'name'
    },
    showSearch: {
        type: Boolean,
        default: true
    },
    filterNode:{
        type: Object
    }
})



const {
    data,
    filterText,
    filter, expandNode 
} = useReadOnlyTree(
    props.api, 
    { prop: props.displayField, order: 'ascending' },
    props.filterNode
);

const selectRow = (row: any) => {
    if(isEmpty(currentRowId.value) || currentRowId.value !== row[props.api.idFieldName]){
        currentRow.value = row;
        currentRowId.value = row[props.api.idFieldName];
        return;
    }
    currentRow.value = null;
    currentRowId.value = '';
}

</script>


<style lang="scss" scoped>
    .el-tree-node.is-current {
        background-color: var(--el-color-primary-light-9);
    }
</style>