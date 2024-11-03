<template>
    <el-drawer v-bind="$attrs" direction="rtl" class="detail-drawer" size="50%"
        :title="`${t('Components.Detail')} - ${title}`"
    >
        <el-descriptions :column="1" size="large" border>
            <el-descriptions-item v-for="item in rowItems" :label="t(item.label)">
                <div v-if="item.render">{{  item.render(data[item.field], data, item) }}</div>
                <div v-else-if="isEmpty(item.type)"> {{  data[item.field] }}</div>
                <CheckStatus v-if="item.type === 'boolean'" :value="(data[item.field] as boolean)" />
                <List v-else-if="item.type === 'list'" :data="data[item.field]"></List>
                <div v-else-if="item.type === 'json'">
                    <span v-if="isEmpty(data[item.field])">-</span>
                    <JsonPretty v-if="!isEmpty(data[item.field])" :data="JSON.parse(data[item.field])"></JsonPretty> 
                </div>
                <div v-else-if="item.type === 'date'">{{  formatDate(data[item.field] as string, item.format || format.Date) }}</div>
                <div v-else-if="item.type === 'datetime'">{{  formatDate(data[item.field] as string, item.format || format.DateTime) }}</div>
                <div v-else-if="item.type === 'object'">
                    <Detail :data="data[item.field]" :row-items="item.rowItems" ></Detail>
                </div>
            </el-descriptions-item>
        </el-descriptions>
    </el-drawer>

</template>

<script setup lang="ts">
import { PropType } from 'vue';
import { formatDate, isEmpty } from '~/libs';
import { useI18n } from '~/composables';
import CheckStatus from './icons/CheckStatus.vue';
import List from './lists/List.vue';
import JsonPretty from 'vue-json-pretty';
import 'vue-json-pretty/lib/styles.css';

export interface RowItemType {
    type?: '' | 'boolean' | 'json' | 'date' | 'datetime' | 'list' | 'object',
    label: string,
    field: string,
    format?: string,
    render?: (value: any, data: any, item:RowItemType) => any,
    rowItems?: RowItemType[]
}

defineProps({
    title: {
        type: String,
        default: ''
    },    
    data:{
        type: Object,
        default: {}
    },
    rowItems:{
        type: Array as PropType<RowItemType[]>,
        default: () => []
    }
})

const {t, format } = useI18n();
</script>

<style lang="scss">
.detail-drawer {
    .el-descriptions__label{
        width: var(--el-descriptions-label-width, 100px);
    }    
}
</style>