<template>
    <AccountForm ref="formRef" :rules="rules" title="Pages.ResetPassword.ResetPassword" :form-data="formData">
        <div class="text-sm text-gray-500 mb-4">
            {{ t('Pages.ResetPassword.ResetPasswordInformation') }}
        </div>
        <el-form-item prop="password">
            <el-input v-model="formData.password" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Register.Password')">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
            <PasswordStrength :value="formData.password" />
        </el-form-item>

        <el-form-item prop="confirmPassword">
            <el-input v-model="formData.confirmPassword" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Register.ConfirmPassword')"
                @keyup.enter="handleSubmit"
                >
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-button  type="primary" @click="handleSubmit" class="w-full"
        >
            {{ t('Components.Save')}} 
        </el-button>
        <div class="w-full pt-2">
            <el-link type="primary" @click="$router.push('/login')">{{ t("Pages.ForgotPassword.BackToLogin") }}</el-link>
        </div>

    </AccountForm>
</template>

<script setup lang="ts">
import AccountForm from '../forms/AccountForm.vue';
import { useConfig, useForm, useFormRules, useI18n } from '~/composables'
import { account,  appConfig } from '~/libs';
import PasswordStrength from './PasswordStrength.vue';
import router from '~/router';

const { t } = useI18n();
const queryString = window.location.search;
const params = new URLSearchParams(queryString);
const userId = params.get('userId') || params.get('userid') || '';
const resetToken = params.get('resetToken') || '';

const formRules = {
    password: { required: true },
    confirmPassword: { required: true, equalTo: 'password' }
};

const refreshRules = () => {
    updateRules('password', appConfig.passwordComplexitySetting);
};

const {  } = useConfig(refreshRules);


const onSubmit = async () => {
    try {
        await account.resetPassword(userId, resetToken, formData.value.password);
        formRef.value.success('Pages.ResetPassword.ResetPasswordSuccess');
        setTimeout(() => {
            router.push('/login');
        }, 3000);

    } catch (error: any) {
        formRef.value.error(error.message);
    }
}

const { formRef, formData,  handleSubmit } = useForm( onSubmit);
const { rules, updateRules} = useFormRules(formRules, formRef);


</script>