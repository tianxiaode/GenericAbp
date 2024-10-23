<template>
  <ul :style="listStyle" :class="listClass">
    <li v-for="item in processedData" :key="getKey(item)" :class="itemClass" :style="itemStyle" class="list-item">
      <span class="key">{{ getKey(item) }}</span>
      <span class="colon">:</span>
      <span class="value">{{ getValue(item) }}</span>
    </li>
    <li v-if="!processedData.length" class="empty-list-item">{{ emptyString }}</li>
  </ul>

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
            padding: '8px',
            border: '1px solid #ccc',
            marginBottom: '5px',
            borderRadius: '4px',
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

    console.warn('传入的数据格式不正确，预期为数组或对象');
    return []; // 数据格式不正确或为null时返回空数组
});

// 获取键值
const getKey = (item: any) => item[props.keyField] ?? props.emptyString;

// 获取值
const getValue = (item: any) => item[props.valueField] ?? props.emptyString;

</script>

<style scoped>
.list-item {
    display: flex;
    align-items: center;
}

.item-key {
    flex: 1;
    text-align: left;
}

.item-colon {
    margin: 0 8px; /* 添加一些左右间隔 */
}

.item-value {
    flex: 1;
    text-align: left;
}
</style>
