<template>  
    <el-table-column v-bind="$attrs">
        <template #default="{row, column}">            
        <div class="flex flex-row items-center gap-1 w-full box-border">
            <div :style="`padding-left: ${(row.code.split('.').length -1) * 16}px ;`"></div>
            <div v-if="!row.leaf" class="cursor-pointer"  >
                <i v-if="row.expanded === true" class="fa fa-caret-down w-4 h-4" @click="expand(row, false)"></i>
                <i v-else class="fa fa-caret-right w-4 h-4" @click="expand(row, true)"></i>
            </div>
            <div class="flex-1" v-html="highlightText(row[column.property], filterText)"></div>
        </div>
        </template>
    </el-table-column>
</template>


<script setup lang="ts">
import { highlightText } from '~/libs'

defineProps({
    filterText: {
        type: String,
        default: ''
    },
    expand: {
        type: Function,
        default: () => {}
    }
});
</script>