<template>
    <el-table-column v-bind="$attrs">
        <template #default="{row, column}">
            {{ render(row[column.property]) }}
        </template>
    </el-table-column>

</template>

<script setup lang="ts">
import { useI18n } from '~/composables';
import { formatDate } from '~/libs'
const props = defineProps({
    format: {
        type: String,
        default: 'datetime'
    }
})

const { format } = useI18n();

const render = (date: string) => {
    let f = props.format === 'datetime' ? format.value.DateTime : props.format === 'date' ? format.value.Date : props.format;
    return formatDate(date, f);
}
</script>