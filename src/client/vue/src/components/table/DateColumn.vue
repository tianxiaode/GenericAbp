<template>
            <el-table-column v-bind="$attrs" :prop="props.prop">
                <template #default="scope">
                    {{ render(scope.row[prop]) }}
                </template>
            </el-table-column>            

</template>

<script setup lang="ts">
import { formatDate } from '../../libs'
const props = defineProps({
    format: {
        type: String,
        default: 'yyyy-MM-dd'
    },
    prop: {
        type: String,
        default: 'lockoutEnd'
    }
})

const render = (date: string) => {
    return formatDate(date, props.format);
}
</script>