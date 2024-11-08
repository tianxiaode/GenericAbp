<template>
    <el-table-column v-bind="$attrs">
        <template #default="{row, column, index}">
            <slot name="default" :row="row" :column="column" :index="index" :filter="filterText"  >
                <span v-html="highlightText(row[column.property], filterText)"></span>
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