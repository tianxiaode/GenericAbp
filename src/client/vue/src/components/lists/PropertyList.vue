<template>
    <el-descriptions :column="1" size="small" border v-if="processedData.length > 0">
        <el-descriptions-item v-for="item in processedData" class-name="w-1/2" :label="getKey(item)">
            {{ getValue(item) }}
        </el-descriptions-item>
    </el-descriptions>
    <div v-if="!processedData.length">{{ emptyString }}</div>

</template>

<script setup lang="ts">
import { defineProps, computed } from 'vue';

const props = defineProps({
    data: {
        type: [Array, Object, null], // 允许数据为数组或对象，或者为null
        default: () => [] // 默认返回空数组
    },
    keyField: {
        type: String,
        default: 'key' // 设置默认键名
    },
    valueField: {
        type: String,
        default: 'value' // 设置默认值名
    },
    emptyString: {
        type: String,
        default: '-'
    },
    listClass: {
        type: String,
        default: ''
    },
    listStyle: {
        type: Object,
        default: () => ({
            listStyleType: 'none',
            margin: '0',
            padding: '0',
        })
    },
    itemClass: {
        type: String,
        default: ''
    },
    itemStyle: {
        type: Object,
        default: () => ({
        })
    }
});

// 处理数据
const processedData = computed(() => {
    if (Array.isArray(props.data)) {
        return props.data;
    }

    if (props.data && typeof props.data === 'object') {
        return Object.entries(props.data).map(([key, value]) => ({
            [props.keyField]: key,
            [props.valueField]: value,
        }));
    }

    return []; // 数据格式不正确或为null时返回空数组
});

// 获取键值
const getKey = (item: any) => item[props.keyField] ?? props.emptyString;

// 获取值
const getValue = (item: any) => item[props.valueField] ?? props.emptyString;

</script>
