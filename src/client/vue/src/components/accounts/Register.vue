<template>
    <AccountForm ref="formRef" :rules="rules" title="Pages.Register.Register"  v-model="formData">
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
                :placeholder="t('Pages.Register.ConfirmPassword')"
                @keyup.enter="handleSubmit"
                >
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-button type="primary" @click="handleSubmit" class="w-full">
            {{ t("Pages.Register.Register") }}
        </el-button>
        <div class="w-full leading-loose">
            <el-link type="primary" @click="$router.push('/login')">{{ t("Pages.Register.AlreadyRegistered") }}</el-link>
        </div>
    </AccountForm>
</template>

<script setup lang="ts">
import AccountForm from "../forms/AccountForm.vue";
import { appConfig, account } from '~/libs';
import { useConfig, useForm, useFormRules, useI18n } from '~/composables';
import PasswordStrength from './PasswordStrength.vue';
import router from '~/router';

const { t } = useI18n();

const formRules= {
    username: { required: true} ,
    email: { required: true, email: true },
    password: { required: true },
    confirmPassword: { required: true, equalTo: 'password' },
};

const refreshRules = () => {
    updateRules('password', appConfig.passwordComplexitySetting);
};

const {  } = useConfig(refreshRules);



const onSubmit = async () => {
    try {
        await account.register(formData.value.username, formData.value.email, formData.value.password);
        formRef.value.success('Pages.Register.RegisterSuccess');
        setTimeout(() => {
            router.push('/login');            
        }, 3000);

    } catch (error: any) {        
        formRef.value.error(error.message);
    }
}

const { formRef, formData,  handleSubmit } = useForm(onSubmit);
const { rules, updateRules} = useFormRules(formRules, formRef);


</script>