<template>
    <el-form ref="formRef" :rules="rules" :model="formData" size="large" :inline-message="true"
        :validate-on-rule-change="false" :scroll-to-error="true" :label-width="180"
        require-asterisk-position="right">
        <el-form-item prop="currentPassword" :label="t('Pages.Profile.CurrentPassword')">
            <el-input v-model="formData.currentPassword" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Profile.currentPassword')">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-form-item prop="newPassword" :label="t('Pages.Profile.NewPassword')">
            <el-input v-model="formData.newPassword" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Profile.NewPassword')">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
            <PasswordStrength :value="formData.newPassword" />
        </el-form-item>

        <el-form-item prop="confirmPassword" :label="t('Pages.Profile.ConfirmNewPassword')">
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
        <FormMessage ref="formMessageRef"></FormMessage>
    </el-form>
</template>

<script setup lang="ts">
import { useConfig, useForm, useFormRules, useI18n } from '~/composables'
import { account, appConfig } from '~/libs';
import PasswordStrength from './PasswordStrength.vue';
import FormMessage from '../forms/FormMessage.vue';
import router from '~/router';
import { ref } from 'vue';

const { t } = useI18n();
const formMessageRef = ref<any>();
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
        await account.changePassword(formData.value.currentPassword, formData.value.newPassword);
        formMessageRef.value.success('Pages.Profile.ChangePasswordSuccess');
        setTimeout(() => {
            router.push('/login');
        }, 3000);

    } catch (error: any) {
        formMessageRef.value.error(error.message);
    }
}

const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules, updateRules } = useFormRules(formRules, formRef);


</script>