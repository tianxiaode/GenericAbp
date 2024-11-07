<template>
        <el-descriptions :column="1" size="large" border>
            <el-descriptions-item v-for="item in rowItems" :label="t(item.label)">
                <div v-if="item.render">{{  item.render(data[item.field], data, item) }}</div>
                <div v-else-if="isEmpty(item.type)"> {{ getValue(item, data) || '-' }}</div>
                <CheckStatus v-if="item.type === 'boolean'" :value="getValue(item, data)" />
                <List v-else-if="item.type === 'list'" :data="getValue(item, data)"></List>
                <div v-else-if="item.type === 'json'">
                    <span v-if="isEmpty(getValue(item, data))">-</span>
                    <JsonPretty v-else :data="getValue(item, data)"></JsonPretty> 
                </div>
                <div v-else-if="item.type === 'date'">{{  formatDate(getValue(item, data), item.format || format.Date) || '-' }}</div>
                <div v-else-if="item.type === 'datetime'">{{  formatDate(getValue(item, data), item.format || format.DateTime) || '-' }}</div>
                <div v-else-if="item.type === 'object'">
                    <PropertyList :data="getValue(item, data)" :row-items="item.rowItems" ></PropertyList>
                </div>
            </el-descriptions-item>
        </el-descriptions>
</template>

<script setup lang="ts">
import { PropType } from 'vue';
import { formatDate, isEmpty } from '~/libs';
import { useI18n } from '~/composables';
import CheckStatus from '../icons/CheckStatus.vue';
import List from './List.vue';
import JsonPretty from 'vue-json-pretty';
import 'vue-json-pretty/lib/styles.css';

export interface RowItemType {
    type?: '' | 'boolean' | 'json' | 'date' | 'datetime' | 'list' | 'object',
    label: string,
    field: string,
    format?: string,
    convert?: (value: any) => any,
    render?: (value: any, data: any, item:RowItemType) => any,
    rowItems?: RowItemType[]
}

defineProps({
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

const getValue = (item: RowItemType, data: any) => {
    let value = data[item.field];
    if(item.convert) return item.convert(value);
    if(item.type === 'json'){
        if(isEmpty(value)) return null;
        if(typeof value === 'object') return value;
        return JSON.parse(value);
    }
    return value;
}
</script>
