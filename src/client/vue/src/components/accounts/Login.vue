<template>
    <AccountForm ref="formRef" :rules="rules" title="Pages.Login.Login" :model="formData">
        <el-form-item prop="username" clearable>
            <el-input v-model="formData.username" :placeholder="t('Pages.Login.UserNameAndEmail')">
                <template #prefix>
                    <i class="fa fa-user"></i>
                </template>
            </el-input>
        </el-form-item>
        <el-form-item prop="password">
            <el-input v-model="formData.password" clearable show-password autocomplete="off"
                :placeholder="t('Pages.Login.Password')" @keyup.enter="formRef.value.submit">
                <template #prefix>
                    <i class="fa fa-lock"></i>
                </template>
            </el-input>
        </el-form-item>
        <div class="w-full text-right pb-2">
            <el-link type="primary" @click="$router.push('/forgot-password')">{{ t("Pages.Login.ForgotPassword")
                }}</el-link>
        </div>
        <el-button type="primary" @click="formRef.value.submit" class="w-full">
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
import { computed, ref } from 'vue';
import AccountForm from './AccountForm.vue';
import { useI18n } from '~/composables'
import { account, LocalStorage, LoginType } from '~/libs';
import router from '~/router';
import ExternalProviders from './ExternalProviders.vue';

const { t } = useI18n();
const formRef = ref<any>();
const formData = ref<LoginType>({} as LoginType);
const rules = computed(() => ({
    username: [
        { required: true, trigger: 'blur', message: t.value('Validation.Required') },
    ],
    password: [
        { required: true, trigger: 'blur', message: t.value('Validation.Required') },
    ],
}));



const login = async () => {
    try {
        await account.login(formData.value.username, formData.value.password);
        formRef.value.setMessage('Pages.login.LoginSuccess', 'success');
        let redirectPath = LocalStorage.getItem('redirectPath') || '/';
        if (redirectPath === '/login') {
            redirectPath = '/';
        }
        router.push(redirectPath);

    } catch (error: any) {
        formRef.value.setMessage(error.message, 'error');
    }


}



</script>