<template>
<el-switch
    style="--el-color-primary: --el-color-primary-dark-2;"
    v-model="value"
    >
    <template #active-action>
      <i class="fa fa-sun text-warning"></i>
    </template>
    <template #inactive-action>
      <i class="fa fa-moon text-info"></i>
    </template>
</el-switch>
</template>

<script setup lang="ts">
import { ref,watch } from 'vue';
import { Moon, Sunny } from '@element-plus/icons-vue';
const value = ref(false);

//这里要根据value修改html的class，所以需要监听value的变化
watch(value, (newValue) => {
    document.documentElement.classList.toggle('dark', newValue);
});
</script>