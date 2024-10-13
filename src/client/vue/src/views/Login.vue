<template>
    <div class="login-container">
        <div class="login-form">
            <h1>请登录</h1>
            <IconSelect></IconSelect>
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
                <el-button type="primary" @click="submitForm()">
                    登录
                </el-button>
                <el-alert v-if="message" :title="message" :type="alertType" show-icon closable style="margin-top: 10px;"
                    @close="clearMessage"></el-alert>
            </el-form>
        </div>
    </div>
</template>

<script lang="ts" setup>
import { inject, reactive, ref } from 'vue'
import router from '../router';
import IconSelect from '../components/forms/IconSelect.vue';

const formRef = ref(null as any);
const form = reactive({ username: '', password: '' });
const message = ref('');
const alertType = ref('');

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

// const submitForm = async () => {
//     await formRef.value.validate(async (valid: boolean) => {
//         if (!valid) return;
//         const t = inject('t') as Function;
//         try {
//             await Account.login(form.username, form.password);
//             message.value = 'LoginSuccess';
//             alertType.value = 'success';
//             let redirectPath = sessionStorage.getItem('redirectPath') || '/';
//             if (redirectPath === '/login') {
//                 redirectPath = '/';
//             }
//             router.push(redirectPath);

//         } catch (error: any) {
//             message.value = error.message;
//             alertType.value = 'error';

//         }
//     })
// }

const clearMessage = () => {
    message.value = ''; // 清空消息
    alertType.value = ''; // 清空警报类型
};

</script>


<style scoped>
.login-container {
    display: flex;
    justify-content: center;
    align-items: center;
    height: 100vh;
    overflow: hidden;
}

.login-form {
    height: 50%;
    width: 50%;
    margin: auto;

}

.login-form form {
    display: flex;
    flex-direction: column;
}
</style>
