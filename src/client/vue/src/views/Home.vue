<template>
    {{ t('Home') }}
    <AccountForm tile="Account Form"  ref="accountForm" />
    <button @click="accountForm.success('Success Message')">Change ValueList</button>
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