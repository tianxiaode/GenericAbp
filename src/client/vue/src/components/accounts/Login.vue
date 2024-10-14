<template>
    <AccountForm ref="formRef" :rules="rules" :message="message" :alert-type="alertType" title="Pages.Login.Login"
        :model="formData" :clear-message="clearMessage">
        <el-form-item prop="username" clearable>
            <el-input v-model="formData.username" :placeholder="t('Pages.Login.UserNameAndEmail')">
                <template #prefix>
                    <i class="fa fa-user"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-form-item prop="password">
            <el-input v-model="formData.password" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Login.Password')" @keyup.enter="submitForm()">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <div class="w-full text-right pb-2">
            <el-link type="primary" @click="$router.push('/forgot-password')">{{ t("Pages.Login.ForgotPassword")
                }}</el-link>
        </div>
        <el-button type="primary" @click="handleSubmit" class="w-full">
            {{ t("Pages.Login.Login") }}
        </el-button>
        <div class="w-full pt-2">
            <el-link type="primary" @click="$router.push('/register')">{{ t("Pages.Login.NewUser") }}</el-link>
        </div>
        <template #footer>
            <ExternalProviders />
        </template>
        
    </AccountForm>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import AccountForm from './AccountForm.vue';
import { useAccountForm, useI18n } from '~/composables'
import { account, LoginType } from '~/libs';
import router from '~/router';
import { LocalStorage } from '~/libs/LocalStoreage';
import ExternalProviders from './ExternalProviders.vue';

const { t } = useI18n();

const rules =computed(() =>  ({
    username: [
        { required: true, trigger: 'blur', message: t.value('Validation.Required') },
    ],
    password: [
        { required: true, trigger: 'blur', message: t.value('Validation.Required') },
    ],
}));


const submitForm = async () => {
    try {
        await account.login(formData.value.username, formData.value.password);
        message.value = 'LoginSuccess';
        alertType.value = 'success';
        let redirectPath = LocalStorage.getItem('redirectPath') || '/';
        if (redirectPath === '/login') {
            redirectPath = '/';
        }
        router.push(redirectPath);

    } catch (error: any) {
        message.value = error.message;
        alertType.value = 'error';

    }

}

const { formRef, formData, message, alertType, handleSubmit, clearMessage } = useAccountForm<LoginType>(submitForm);


</script>