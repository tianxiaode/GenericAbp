<template>
    <el-form v-model="formData" label-width="100px">
        <el-input v-model="formData.name" placeholder="请输入名称"></el-input>
        <slot></slot>
    </el-form>
</template>

<script setup lang="ts">
import { watch } from 'vue';

const formData = defineModel<any>();

watch(formData, (newValue, oldValue) => {
    console.log('formData in TestFrom component changed', newValue, oldValue)
}, { deep: true })
</script>