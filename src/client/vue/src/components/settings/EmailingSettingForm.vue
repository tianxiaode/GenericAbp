<template>
    <el-form ref="formRef" :model="formData" :label-width="300" label-suffix=":" :rules="rules" size="large"
        :validate-on-rule-change="false" :scroll-to-error="true"
        require-asterisk-position="right">
        <el-form-item prop="defaultFromAddress" :label="t('AbpEmailing.DisplayName:Abp.Mailing.DefaultFromAddress')">
            <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.DefaultFromAddress')" placement="right">
                <el-input v-model="formData.defaultFromAddress" clearable />
            </el-tooltip>
        </el-form-item>
        <el-form-item prop="defaultFromDisplayName" :label="t('AbpEmailing.DisplayName:Abp.Mailing.DefaultFromDisplayName')">
            <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.DefaultFromDisplayName')" placement="right">
                <el-input v-model="formData.defaultFromDisplayName" clearable />
            </el-tooltip>
        </el-form-item>
        <el-form-item prop="smtpHost" :label="t('AbpEmailing.DisplayName:Abp.Mailing.Smtp.Host')">
            <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.Smtp.Host')" placement="right">
                <el-input v-model="formData.smtpHost" clearable />
            </el-tooltip>
        </el-form-item>
        <el-form-item prop="smtpPort" :label="t('AbpEmailing.DisplayName:Abp.Mailing.Smtp.Port')">
            <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.Smtp.Port')" placement="right">
                <el-input-number v-model="formData.smtpPort" :min="1" :max="65535" />
            </el-tooltip>
        </el-form-item>
        <el-form-item prop="smtpUserName" :label="t('AbpEmailing.DisplayName:Abp.Mailing.Smtp.UserName')">
            <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.Smtp.UserName')" placement="right">
                <el-input v-model="formData.smtpUserName" clearable />
            </el-tooltip>
        </el-form-item>
        <el-form-item prop="smtpPassword" :label="t('AbpEmailing.DisplayName:Abp.Mailing.Smtp.Password')">
            <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.Smtp.Password')" placement="right">
                <el-input type="password" v-model="formData.smtpPassword" clearable show-password autocomplete="off" />
            </el-tooltip>
        </el-form-item>
        <el-form-item prop="smtpDomain" :label="t('AbpEmailing.DisplayName:Abp.Mailing.Smtp.Domain')">
            <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.Smtp.Domain')" placement="right">
                <el-input v-model="formData.smtpDomain" clearable />
            </el-tooltip>
        </el-form-item>
        <div>
            <el-checkbox v-model="formData.smtpEnableSsl">
                <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.Smtp.EnableSsl')" placement="right">
                    {{ t('AbpEmailing.DisplayName:Abp.Mailing.Smtp.EnableSsl') }}
                </el-tooltip>
            </el-checkbox>
        </div>
        <div>
            <el-checkbox v-model="formData.smtpUseDefaultCredentials">
                <el-tooltip :content="t('AbpEmailing.Description:Abp.Mailing.Smtp.UseDefaultCredentials')"
                    placement="right">
                    {{ t('AbpEmailing.DisplayName:Abp.Mailing.Smtp.UseDefaultCredentials') }}
                </el-tooltip>
            </el-checkbox>
        </div>
        <div class="flex items-center justify-between gap-4 mt-4">
            <el-button type="primary" @click="handleSubmit">{{ t('Components.Save') }}</el-button>
            <FormMessage ref="formMessage" class="flex-1" />
        </div>
    </el-form>
</template>


<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useI18n, useFormRules, useForm } from '~/composables';
import FormMessage from '../forms/FormMessage.vue';
import {  settingManagement } from '~/libs';
const { t } = useI18n();

const formMessage = ref<any>(null);


const onSubmit = async () => {
    try {
        await settingManagement.updateEmailingSettings(formData.value);
        formMessage.value.success('Message.SaveSuccess');
    } catch (error: any) {
        formMessage.value.error(error.message);
    }

};

const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules } = useFormRules({
    defaultFromAddress: { required: true, email: true },
    defaultFromDisplayName: { required: true},
    smtpPort: { required: true, min: 1, max:65535 }
}, formRef);

onMounted(() => {
    if (settingManagement.canManageIdentitySettings) {
        settingManagement.getEmailingSettings().then((res: any) => {
            formData.value = { ...res };
        });
    }
});

</script>