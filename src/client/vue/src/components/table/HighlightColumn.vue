<template>
    <el-table-column v-bind="$attrs">
        <template #default="{row, column, index}">
            <slot name="default" :row="row" :column="column" :index="index" :filter="filterText"  >
                <HighlightText :text="row[column.property]" :filter="filterText"></HighlightText>
            </slot>
        </template>    
    </el-table-column>
</template>

<script setup lang="ts">
import HighlightText from '../HighlightText.vue';

defineProps({
    filterText: {
        type: String,
        default: ''
    }
});

</script>