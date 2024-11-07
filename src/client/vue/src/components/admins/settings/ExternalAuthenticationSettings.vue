<template>
    <el-form ref="formRef" :model="formData" :label-width="300" label-suffix=":" :rules="rules" size="large"
        :validate-on-rule-change="false" :scroll-to-error="true"
        require-asterisk-position="right">
        <el-form-item prop="newUserEmailSuffix" :label="t('ExternalAuthentication.NewUserEmailSuffix')">
            <el-input v-model="formData.newUserEmailSuffix" />
        </el-form-item>
        <el-form-item prop="newUserPrefix" :label="t('ExternalAuthentication.NewUserPrefix')">
                <el-input v-model="formData.newUserPrefix" />
        </el-form-item>
        <el-collapse v-model="activeNames">            
            <el-collapse-item v-for="provider in formData.providers" :name="provider.provider">
                <template #title>
                    <div class="title">{{ provider.displayName }}  </div>
                </template>
                <el-form-item prop="provider.clientId" :label="t('ExternalAuthentication.ClientId')">
                    <el-input v-model="provider.clientId" />
                </el-form-item>
                <el-form-item prop="provider.clientSecret" :label="t('ExternalAuthentication.ClientSecret')">
                    <el-input v-model="provider.clientSecret" />
                </el-form-item>
                <el-form-item prop="provider.enabled" >
                    <el-checkbox v-model="provider.enabled" >
                        <span>{{ t('ExternalAuthentication.Enabled') }}</span>
                    </el-checkbox>
                </el-form-item>
            </el-collapse-item>
        </el-collapse>
        <div class="flex items-center justify-between gap-4 mt-4">
            <el-button type="primary" @click="handleSubmit">{{ t('Components.Save') }}</el-button>
            <FormMessage ref="formMessage" class="flex-1" />
        </div>
    </el-form>
</template>


<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useI18n, useFormRules, useForm } from '~/composables';
import FormMessage from '../../forms/FormMessage.vue';
import {  settingManagement } from '~/libs';
const { t } = useI18n();

const formMessage = ref<any>(null);
const activeNames = ref<string[]>([]);

const onSubmit = async () => {
    try {
        await settingManagement.updateExternalAuthenticationSettings(formData.value);
        formMessage.value.success('Message.SaveSuccess');
    } catch (error: any) {
        formMessage.value.error(error.message);
    }

};

const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules } = useFormRules({
    newUserEmailSuffix: {required: true},
    newUserPrefix: {required: true}
}, formRef);

onMounted(() => {
    if (settingManagement.canManageExternalAuthentication) {
        settingManagement.getExternalAuthenticationSettings().then((res: any) => {
            formData.value = { ...res };
            activeNames.value = res.providers.map((provider: any) => provider.provider);
        });
    }
});

</script>

<style scoped>
.title {
    font-size: 18px;
    font-weight: bold;
    margin-right: 10px;
    color: #333;
}
</style>