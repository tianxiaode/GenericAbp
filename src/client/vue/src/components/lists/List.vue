<template>
  <ul
    v-if="isDataValid"
    :class="['m-0 p-0', listClass, computedTypeClass]"
    :style="listStyle"
  >
    <li
      v-for="item in data"
      :key="itemKey(item)"
      :class="itemClass"
      :style="itemStyle"
    >
      {{ itemValue(item) }}
    </li>
  </ul>
  <p v-else>{{ emptyString }}</p>
</template>

<script setup lang="ts">
import { defineProps, computed } from 'vue';

// 定义 props
const props = defineProps({
  data: {
    type: [Array, Object, null],
    default: () => [] // 默认返回空数组
  },
  keyField: {
    type: String,
    default: null
  },
  valueField: {
    type: String,
    default: null
  },
  type: {
    type: String,
    default: 'none'
  },
  emptyString: {
    type: String,
    default: '-'
  },
  listStyle: {
    type: Object,
    default: () => ({})
  },
  listClass: {
    type: String,
    default: ''
  },
  itemClass: {
    type: String,
    default: ''
  },
  itemStyle: {
    type: Object,
    default: () => ({})
  }
});

// 计算属性
const isDataValid = computed(() => Array.isArray(props.data) && props.data.length > 0);
const computedTypeClass = computed(() => {
  return props.type === 'disc' ? 'list-disc' : props.type === 'decimal' ? 'list-decimal' : 'list-none';
});

// 键值提取函数
const itemKey = (item: any) => props.keyField ? (item as any)[props.keyField] : item;

// 值提取函数
const itemValue = (item: any) => props.valueField ? (item as any)[props.valueField] : item;

</script>
