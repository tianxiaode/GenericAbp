<template>
    {{ t('Home') }}
    <el-text>Primary - {{ hexToHsl('#3c7699') }}</el-text>
    <el-alert type="success" >success - {{ hexToHsl('#00953E') }}</el-alert>
    <el-alert type="warning" >warning - {{ hexToHsl('#e9bb41') }}</el-alert>
    <el-alert type="error" >error - {{ hexToHsl('#d80835') }}</el-alert>
    <el-alert type="info" >info - {{ hexToHsl('#9fa39a') }}</el-alert>
    <el-button class="w-full" type="primary" @click="accountForm.success('Success Message')">Change ValueList</el-button>
</template>

<script setup lang="ts">
import ValueListInput from '~/components/forms/ValueListInput.vue';
import {  useI18n } from '../composables'
import { onMounted, ref,watch } from 'vue';
import PropertyInput from '~/components/forms/PropertyInput.vue';
import AccountForm from '~/components/forms/AccountForm.vue';
import { hexToHsl } from '~/libs';
const { t } = useI18n();


const valueList = ref({})
const accountForm = ref<any>();

watch(valueList, (newValue) => {
  console.log(newValue)
}, { deep: true })
</script>