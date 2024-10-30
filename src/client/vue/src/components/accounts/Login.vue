<template>
    <AccountForm ref="formRef" :rules="rules" title="Pages.Login.Login" v-model="formData">
        <el-form-item prop="username" clearable>
            <el-input v-model="formData.username" :placeholder="t('Pages.Login.UserNameAndEmail')">
                <template #prefix>
                    <i class="fa fa-user"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-form-item prop="password">
            <el-input v-model="formData.password" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Login.Password')" @keyup.enter="handleSubmit">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <div class="w-full text-right mb-2 leading-loose">
            <el-link type="primary" @click="$router.push('/forgot-password')">{{ t("Pages.Login.ForgotPassword")
                }}</el-link>
        </div>
        <el-button type="primary" @click="handleSubmit" class="w-full">
            {{ t("Pages.Login.Login") }}
        </el-button>
        <div class="w-full leading-loose">
            <el-link type="primary" @click="$router.push('/register')">{{ t("Pages.Login.NewUser") }}</el-link>
        </div>            
    </AccountForm>
</template>

<script setup lang="ts">
import AccountForm from '../forms/AccountForm.vue';
import { useForm, useFormRules, useI18n } from '~/composables'
import { account, LocalStorage } from '~/libs';
import router from '~/router';


const { t } = useI18n();
const formRules = {
    username: { required: true },
    password: { required: true },
};

const onSubmit = async () => {
    try {
        await account.login(formData.value.username, formData.value.password);
        formRef.value.success('Pages.login.LoginSuccess');
        setTimeout(() => {
            let redirectPath = LocalStorage.getRedirectPath() || '/';
            if (redirectPath === '/login') {
                redirectPath = '/';
            }
            router.push(redirectPath);
            localStorage.removeItem('redirectPath');
        }, 2000);

    } catch (error: any) {
        formRef.value.error(error.message);
    }
}

const { formRef, formData, handleSubmit } = useForm(onSubmit);
const { rules } = useFormRules(formRules, formRef);


</script>