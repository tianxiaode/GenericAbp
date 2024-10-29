<template>
    <AccountForm ref="formRef" :rules="rules" title="Pages.ResetPassword.ResetPassword" v-model="formData">
        <div v-if="canResetPassword">
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
        <div class="w-full leading-loose">
            <el-link type="primary" @click="$router.push('/login')">{{ t("Pages.ForgotPassword.BackToLogin") }}</el-link>
        </div>
    </div>

    </AccountForm>
</template>

<script setup lang="ts">
import AccountForm from '../forms/AccountForm.vue';
import { useConfig, useForm, useFormRules, useI18n } from '~/composables'
import { account,  appConfig, isEmpty } from '~/libs';
import PasswordStrength from './PasswordStrength.vue';
import router from '~/router';
import { onMounted, ref } from 'vue';

const { t } = useI18n();
const queryString = window.location.search;
const params = new URLSearchParams(queryString);
const userId = params.get('userId') || params.get('userid') || '';
const resetToken = params.get('resetToken') || '';
const tenant = params.get('__tenant') || '';
const canResetPassword = ref<boolean>(false);
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

onMounted(async ()=>{
    try{
        if(isEmpty(userId) || isEmpty(resetToken)){
            formRef.value.error('Account.InvalidResetPasswordData');
            return;
        }
        const res = await account.verifyPasswordResetToken(userId, resetToken, tenant)
        if(res === 'false'){
            formRef.value.error('Account.InvalidResetPasswordData');
            return;
        }
        canResetPassword.value = true;
    }catch(e:any){
        formRef.value.error(e.message);
    }
        
})

</script>