<template>
    <BaseDialog v-bind="$attrs" ref="dialogRef" class="tree-select-dialog">
        <Tree :api="api" :display-field="displayField" v-model="selected" :filter-node="filterNode" />
    </BaseDialog>
</template>

<script setup lang="ts">
import Tree from '../tree/Tree.vue';
import BaseDialog from './BaseDialog.vue';


const selected = defineModel();
const dialogRef = defineModel<any>('dialogRef');
defineProps({
    api: {
        type: Object,
        required: true
    },
    displayField: {
        type: String,
        required: true
    },
    filterNode: {
        type: Object
    }
});    
</script>

<style lang="scss">
.tree-select-dialog {
    .el-tree{
        min-height: 500px;
        max-height: 500px;
        overflow-y: auto;
    }
}
</style>