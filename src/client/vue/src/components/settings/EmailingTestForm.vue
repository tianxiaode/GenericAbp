<template>
    <el-form ref="formRef" :model="formData" :label-width="300" label-suffix=":" :rules="rules" size="large"
        :validate-on-rule-change="false" :scroll-to-error="true"
        require-asterisk-position="right">
        <el-form-item prop="senderEmailAddress" :label="t('AbpSettingManagement.SenderEmailAddress')">
            <el-input v-model="formData.senderEmailAddress" />
        </el-form-item>
        <el-form-item prop="targetEmailAddress" :label="t('AbpSettingManagement.TargetEmailAddress')">
            <el-input v-model="formData.targetEmailAddress" />
        </el-form-item>
        <el-form-item prop="subject" :label="t('AbpSettingManagement.Subject')">
            <el-input v-model="formData.subject" />
        </el-form-item>
        <el-form-item prop="body" :label="t('AbpSettingManagement.Body')">
            <el-input type="textarea" :rows="5" v-model="formData.body" />
        </el-form-item>
        <div class="flex items-center justify-between gap-4 mt-4">
            <el-button type="primary" @click="handleSubmit">{{ t('AbpSettingManagement.Send') }}</el-button>
            <FormMessage ref="formMessage" class="flex-1" />
        </div>
    </el-form>
</template>


<script setup lang="ts">
import { ref } from 'vue';
import { useI18n, useFormRules, useForm } from '~/composables';
import FormMessage from '../forms/FormMessage.vue';
import {  settingManagement } from '~/libs';
const { t } = useI18n();

const formMessage = ref<any>(null);


const onSubmit = async () => {
    try {
        await settingManagement.sendTestEmail(formData.value);
        formMessage.value.success('AbpSettingManagement.SuccessfullySent');
    } catch (error: any) {
        formMessage.value.error(error.message);
    }

};
const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules } = useFormRules({
    senderEmailAddress: { required: true, email: true },
    targetEmailAddress: { required: true, email: true },
    subject: { required: true},
    body: {required: true}
}, formRef);


</script>