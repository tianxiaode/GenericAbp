<template>
    <el-form ref="formRef" :rules="rules" :model="formData" size="large" :inline-message="true"
        :validate-on-rule-change="false" :scroll-to-error="true">
        <el-form-item prop="currentPassword">
            <el-input v-model="formData.currentPassword" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Profile.currentPassword')">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-form-item prop="newPassword">
            <el-input v-model="formData.newPassword" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Profile.NewPassword')">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
            <PasswordStrength :value="formData.newPassword" />
        </el-form-item>

        <el-form-item prop="confirmPassword">
            <el-input v-model="formData.confirmPassword" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Profile.ConfirmNewPassword')" @keyup.enter="handleSubmit">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-button type="primary" @click="handleSubmit" class="w-full">
            {{ t('Components.Save') }}
        </el-button>
        <div class="w-full pt-2" v-if="formMessage">
            <el-alert :type="formMessageType" show-icon closable style="margin-top: 10px;" @close="clearFormMessage">
                <template #title>
                    <span v-html="t(formMessage)"> </span>
                </template>
            </el-alert>
        </div>
    </el-form>
</template>

<script setup lang="ts">
import { useConfig, useForm, useFormMessage, useFormRules, useI18n } from '~/composables'
import { account, appConfig } from '~/libs';
import PasswordStrength from './PasswordStrength.vue';
import router from '~/router';

const { t } = useI18n();

const formRules = {
    currentPassword: { required: true },
    newPassword: { required: true },
    confirmPassword: { required: true, equalTo: 'newPassword' }
};

const refreshRules = () => {
    updateRules('newPassword', appConfig.passwordComplexitySetting);
};

const { } = useConfig(refreshRules);


const onSubmit = async () => {
    try {
        await account.changePassword(formData.currentPassword, formData.newPassword);
        formMessage.value ='Pages.Profile.ChangePasswordSuccess';
        formMessageType.value = 'success';
        setTimeout(() => {
            router.push('/login');
        }, 3000);

    } catch (error: any) {
        formMessage.value = error.message;
        formMessageType.value = 'error';
    }
}

const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules, updateRules } = useFormRules(formRules, formRef);
const { formMessage, formMessageType, clearFormMessage } = useFormMessage();


</script>