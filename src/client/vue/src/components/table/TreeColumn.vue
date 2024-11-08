<template>
    <CustomSortColumn v-bind="$attrs">
        <template #default="{ row, column }">
            <div class="flex flex-row items-center gap-1 w-full box-border">
                <div :style="`padding-left: ${(row.code.split('.').length - 1) * 16}px ;`"></div>
                <div v-if="!row.leaf" class="cursor-pointer">
                    <i v-if="row.expanded === true" class="fa fa-caret-down w-4 h-4" @click="expand(row, false)"></i>
                    <i v-else class="fa fa-caret-right w-4 h-4" @click="expand(row, true)"></i>
                </div>
                <HighlightText class="flex-1" :text="row[column.property]" :filter="filterText"></HighlightText>
            </div>
        </template>
    </CustomSortColumn>
</template>


<script setup lang="ts">
import HighlightText from '../HighlightText.vue';
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