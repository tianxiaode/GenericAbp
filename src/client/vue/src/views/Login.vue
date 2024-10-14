<template>    
    <div class="hero">
        <el-card style="width: 300px; margin: 0 auto;">
            <template #header>
                <div class="text-center text-2xl font-bold">{{ t("Pages.Login.Login") }}</div>
            </template>
            <el-form ref="formRef" :model="form" :rules="rules" size="large" status-icon>
                <el-form-item prop="username" clearable>
                    <el-input v-model="form.username">
                        <template #prefix>
                            <i class="fa fa-user"></i>
                        </template>
                    </el-input>
                </el-form-item>
                <el-form-item prop="password">
                    <el-input v-model="form.password" clearable show-password autocomplete="off" @keyup.enter="submitForm()">
                        <template #prefix>
                            <i class="fa fa-lock"></i>
                        </template>
                    </el-input>
                </el-form-item>
                <div class="w-full text-right pb-2">
                    <el-link type="primary" @click="$router.push('/forgot-password')">{{ t("Pages.Login.ForgotPassword") }}</el-link>
                </div>
                <el-button type="primary" @click="submitForm()" class="w-full">
                    {{ t("Pages.Login.Login") }}
                </el-button>
                <div class="w-full pt-2">
                    <el-link type="primary" @click="$router.push('/register')">{{ t("Pages.Login.NewUser") }}</el-link>    
                </div>
                <div class="w-full pt-2" v-if="message">
                    <el-alert  :title="message" :type="alertType" show-icon closable style="margin-top: 10px;"
                        @close="clearMessage"></el-alert>
                </div>
            </el-form>
            <template #footer v-if="providers.length > 0">
                <div class="flex justify-center items-center gap-2">
                    <a v-for="provider in providers" href="#" @click="loginByExternalProvider(provider.provider)" :title="provider.displayName">
                        <i  class="fab text-2xl" :class="`fa-${provider.provider.toLocaleLowerCase()}`" ></i>
                    </a>
                </div>
            </template>
        </el-card>
    </div>
</template>

<script lang="ts" setup>
import { onMounted, reactive, ref } from 'vue'
import router from '../router';
import { useI18n } from '~/composables';
import { account } from '~/libs';
import { ExternalProviderType } from '~/libs/AccountType';

const formRef = ref(null as any);
const form = reactive({ username: '', password: '' });
const message = ref('');
const alertType = ref('');

const { t } = useI18n();
const providers = ref<ExternalProviderType[]>([]);

const rules = {
    username: [
        { required: true, message: '请输入用户名', trigger: 'blur' },
    ],
    password: [
        {
            required: true,
            message: '请输入密码',
            trigger: 'blur',
        },
    ],
};

const submitForm = async () => {
    await formRef.value.validate(async (valid: boolean) => {
        if (!valid) return;
        try {
            await account.login(form.username, form.password);
            message.value = 'LoginSuccess';
            alertType.value = 'success';
            let redirectPath = sessionStorage.getItem('redirectPath') || '/';
            if (redirectPath === '/login') {
                redirectPath = '/';
            }
            router.push(redirectPath);

        } catch (error: any) {
            message.value = error.message;
            alertType.value = 'error';

        }
    })
}

const clearMessage = () => {
    message.value = ''; // 清空消息
    alertType.value = ''; // 清空警报类型
};

const loginByExternalProvider = (provider: string) => {
    console.log(provider);    
};

onMounted(() => {
    account.getExternalProviders().then(result => {
        console.log(result);
        providers.value = [
            { provider: 'google', displayName: 'Google' },
            { provider: 'GitHub', displayName: 'GitHub' },
        ];
    });
});

</script>


