<template>
    <AccountForm ref="formRef" :rules="rules" :message="message" :alert-type="alertType" title="Pages.Register.Register"
        :model="formData" :clear-message="clearMessage">
        <template #footer>
            <ExternalProviders />
        </template>
    </AccountForm>
</template>

<script setup lang="ts">
import { useAccountForm } from "~/composables";
import ExternalProviders from './ExternalProviders.vue';
import AccountForm from "./AccountForm.vue";
import { computed } from "vue";
import { account, RegisterType } from '~/libs';


const rules = computed(() =>  ({
    
}));

const submitForm = async (data: any) => {

};

const {formRef, formData,  message, alertType, clearMessage} = useAccountForm<RegisterType>(submitForm);
</script>