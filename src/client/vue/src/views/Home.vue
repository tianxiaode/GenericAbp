<template>
    {{ t('Home') }}
    <el-alert type="success" >success</el-alert>
    <el-alert type="warning" >warning</el-alert>
    <el-alert type="error" >error</el-alert>
    <el-alert type="info" >info</el-alert>
    <el-button class="w-full" type="primary mb-4" @click="accountForm.success('Success Message')">Change ValueList</el-button>
</template>

<script setup lang="ts">
import ValueListInput from '~/components/forms/ValueListInput.vue';
import {  useI18n } from '../composables'
import { onMounted, ref,watch } from 'vue';
import PropertyInput from '~/components/forms/PropertyInput.vue';
import AccountForm from '~/components/forms/AccountForm.vue';
const { t } = useI18n();


const valueList = ref({})
const accountForm = ref<any>();

watch(valueList, (newValue) => {
  console.log(newValue)
}, { deep: true })
</script>