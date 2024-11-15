<template>
    <el-table-column v-bind="$attrs">
        <template #header="{ column }">
            <div :class="`flex flex-row items-center w-full cursor-pointer ${column.property === sort?.prop && sort?.order ? sort.order : '' }`" @click="sortChange(column.property)">
                {{ column.label }}
                <span class="caret-wrapper">
                    <i :class="`sort-caret ascending ${column.property === sort?.prop && sort?.order === 'ascending'? 'text-primary' : ''}`"></i>
                    <i :class="`sort-caret descending ${column.property === sort?.prop && sort?.order === 'descending' ? 'text-primary' : ''}`"></i>
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
import { PropType } from 'vue';
import HighlightText from '../HighlightText.vue';
import { SortType } from '~/libs';

defineProps({
    isHighlight:{
        type: Boolean,
        default: false
    },
    filter:{
        type: String,
        default: ''
    },
    sort:{
        type: Object as PropType<SortType>,
    },
    sortChange:{
        type: Function,
        default: () => {}
    }
})

</script>