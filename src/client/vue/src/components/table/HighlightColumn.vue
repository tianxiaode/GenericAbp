<template>
    <el-table-column v-bind="$attrs">
        <template #default="scope">
            <slot name="default" :row="scope.row" :column="scope.column" :index="scope.index" :filter="filterText"  >
                <span v-html="highlightText(scope.row[scope.column.prop], filterText)"></span>
            </slot>
        </template>    
    </el-table-column>
</template>

<script setup lang="ts">
import { highlightText } from '~/libs'

defineProps({
    filterText: {
        type: String,
        default: ''
    }
});

</script>