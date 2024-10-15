<template>
    <AccountForm ref="formRef" :rules="rules" title="Pages.Register.Register" :form-data="formData">
        <el-form-item prop="username" clearable>
            <el-input v-model="formData.username"             
            :placeholder="t('Pages.Register.Username')">
                <template #prefix>
                    <i class="fa fa-user"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-form-item prop="email" clearable>
            <el-input v-model="formData.email" :placeholder="t('Pages.Register.Email')">
                <template #prefix>
                    <i class="fa fa-at"></i>
                </template>
            </el-input>
        </el-form-item>
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
                :placeholder="t('Pages.Register.ConfirmPassword')">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-button type="primary" @click="handleSubmit" class="w-full">
            {{ t("Pages.Register.Register") }}
        </el-button>
        <div class="w-full pt-2">
            <el-link type="primary" @click="$router.push('/login')">{{ t("Pages.Register.AlreadyRegistered") }}</el-link>
        </div>
        <template #footer>
            <ExternalProviders />
        </template>
    </AccountForm>
</template>

<script setup lang="ts">
import ExternalProviders from './ExternalProviders.vue';
import AccountForm from "../forms/AccountForm.vue";
import { RegisterType } from '~/libs';
import { useForm, useFormRules, useI18n } from '~/composables';
import { onMounted } from 'vue';
import PasswordStrength from './PasswordStrength.vue';

const { t } = useI18n();

const formRules = {
    username: { required: true} ,
    email: { required: true, email: true },
    password: { required: true, minLength: 6 },
    confirmPassword: { required: true, minLength: 6, equalTo: 'password' },
};


const onSubmit = async () => {
    console.log(formData);
    try {
        //formRef.value.success('Pages.login.LoginSuccess');

    } catch (error: any) {
        formRef.value.error(error.message);
    }
}

const { formRef, formData,  handleSubmit } = useForm<RegisterType>(onSubmit);
const { rules} = useFormRules(formRules, formRef);




</script>