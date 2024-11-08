<template>
    <el-table-column v-bind="$attrs">
        <template #header="{ row, column }">
            <div :class="`flex flex-row items-center w-full cursor-pointer ${order || ''}`" @click="sortChange(column.property)">
                {{ column.label }}
                <span class="caret-wrapper">
                    <i :class="`sort-caret ascending `"></i>
                    <i :class="`sort-caret descending ${order === 'descending'? 'text-primary' : ''}`"></i>
                </span>
            </div>
        </template>
        <template #default="{ row , column,index }">
            <slot name="default" :row="row" :column="column" :index="index">
                <span v-if="!isHighlight">{{ row[column.property] }}</span>
                <HighlightText v-else :text="row[column.property]" :filter="filter"></HighlightText>
            </slot>
        </template>
    </el-table-column>

</template>

<script setup lang="ts">
import HighlightText from '../HighlightText.vue';

const order = defineModel<'ascending' | 'descending' | null>('order', { default: null });
const props = defineProps({
    isHighlight:{
        type: Boolean,
        default: false
    },
    filter:{
        type: String,
        default: ''
    },
    sortChange:{
        type: Function,
        default: () => {}
    }
})
const sortChange = (property: string) => {
    order.value = order.value === 'ascending'? 'descending' : order.value === 'descending' ? null : 'ascending';
    props.sortChange(property, order.value);
}
</script>