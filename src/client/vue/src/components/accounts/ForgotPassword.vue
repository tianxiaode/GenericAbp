<template>
    <AccountForm ref="formRef" :rules="rules" title="Pages.ForgotPassword.ForgotPassword" v-model="formData">
        <div class="text-sm text-gray-500 mb-4">
            {{ t('Pages.ForgotPassword.SendPasswordResetLinkInformation') }}
        </div>
        <el-form-item prop="email" clearable>
            <el-input v-model="formData.email" :placeholder="t('Pages.ForgotPassword.Email')"
                @keyup.enter="handleSubmit">
                <template #prefix>
                    <i class="fa fa-at"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-button ref="submitBtn" type="primary" @click="handleSubmit" class="w-full" :disabled="disabled">
            {{ t(buttonText).replace('{0}', resendCount.toString()) }}
        </el-button>
        <div class="w-full leading-loose">
            <el-link type="primary" @click="$router.push('/login')">{{ t("Pages.ForgotPassword.BackToLogin")
                }}</el-link>
        </div>

    </AccountForm>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import AccountForm from '../forms/AccountForm.vue';
import { useForm, useFormRules, useI18n } from '~/composables'
import { account, LocalStorage } from '~/libs';

const { t } = useI18n();
const disabled = ref(false);
let resendCount: number = parseInt(LocalStorage.getItem('resendCount') || '0');
let resendInterval: any = undefined;
const buttonText = ref('Pages.ForgotPassword.Send')

const formRules = {
    email: { required: true, email: true },
};

const handleResend = () => {
    disabled.value = true;
    resendCount--;
    LocalStorage.setItem('resendCount', resendCount.toString());
    resendInterval = setInterval(() => {
        resendCount--;
        buttonText.value = t.value('pages.forgotPassword.resend').replace('{0}', resendCount.toString());
        LocalStorage.setItem('resendCount', resendCount.toString());
        if (resendCount === 0) {
            localStorage.removeItem('resendCount');
            buttonText.value = t.value('pages.forgotPassword.send');
            disabled.value = false;
            clearInterval(resendInterval as number);
        }
    }, 1000);
};


if (resendCount > 0) {
    handleResend();
}


const onSubmit = async () => {
    try {
        disabled.value = true;
        await account.sendPasswordResetCode(formData.value.email);
        resendCount = 300;
        formRef.value.success('Pages.ForgotPassword.PasswordResetLinkSent');
        handleResend();

    } catch (error: any) {
        disabled.value = false;
        formRef.value.error(error.message);
    }
}

const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules } = useFormRules(formRules, formRef);


</script>