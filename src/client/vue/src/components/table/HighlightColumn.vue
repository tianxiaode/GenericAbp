<template>
    <el-table-column v-bind="$attrs" :prop="props.prop" >
        <template #default="scope">
            <slot name="default">
                <span v-html="highlightText(scope.row[props.prop], filterText)"></span>
            </slot>
        </template>    
    </el-table-column>
</template>

<script setup lang="ts">
import { highlightText } from '../../libs'

const props = defineProps({
    prop: {
        type: String,
        required: true
    },
    filterText: {
        type: String,
        default: ''
    }
});

</script>