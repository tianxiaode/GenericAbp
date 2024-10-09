<template>
    <FormDialog ref="formDialog" v-bind="$attrs" :submit="handleSubmit" :check-change="hasChanged"
        :reset-form="resetForm" :validate-form="validateForm">
        <template #form-items>
            <el-form :rules="rules" ref="formRef" :model="formData" label-width="100px" show-status-icon>
                <el-input v-model="formData.id" type="hidden" />
                <el-input v-model="formData.concurrencyStamp" type="hidden" />
                <div class="form-container">
                    <el-form-item label="用户名" prop="userName">
                        <el-input v-model="formData.userName" placeholder="请输入用户名称" clearable
                            :readonly="!!props.entityId" :class="{ 'is-readonly': !!props.entityId }"></el-input>
                    </el-form-item>
                    <el-form-item label="电子邮件" prop="email">
                        <el-input v-model="formData.email" placeholder="请输入电子邮件" clearable></el-input>
                    </el-form-item>
                    <el-form-item label="密码" prop="password">
                        <el-input type="password" v-model="formData.password" placeholder="请输入密码" clearable
                            show-password autocomplete="off"></el-input>
                    </el-form-item>
                    <el-form-item label="确认密码" prop="confirmPassword">
                        <el-input type="password" v-model="formData.confirmPassword" placeholder="请输入确认密码" clearable
                            show-password autocomplete="off"></el-input>
                    </el-form-item>
                    <el-form-item label="姓名" prop="surname">
                        <el-input v-model="formData.name" placeholder="请输入姓名" clearable></el-input>
                    </el-form-item>
                    <el-form-item label="手机号码" prop="phoneNumber">
                        <el-input v-model="formData.name" placeholder="请输入手机号码" clearable></el-input>
                    </el-form-item>

                    <el-form-item label="启用">
                        <el-switch v-model="formData.isActive" active-text="是" inactive-text="否"></el-switch>
                    </el-form-item>

                    <el-form-item label="账户锁定">
                        <el-switch v-model="formData.lockoutEnabled" active-text="是" inactive-text="否"></el-switch>
                    </el-form-item>
                </div>
                <h4>角色</h4>
                <el-checkbox-group v-model="formData.roleNames">
                    <div class="grid cols-4 gap-2">
                        <el-checkbox v-for="role in allRoles" :value="role" :key="role" :label="role">
                            {{ role }}
                        </el-checkbox>
                    </div>
                </el-checkbox-group>
            </el-form>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';
import { UserType, userApi, roleApi } from '../../repositories';
import { useForm } from '../../composables/useForm'
import { globalConfig, isEmpty } from '../../libs';
import { onMounted, ref } from 'vue';

const allRoles = ref([] as string[]);

const props = defineProps({
    entityId: {
        type: String,
        default: ''
    }
});

const { formRef, formData, hasChanged, handleSubmit, resetForm, validateForm, setInitialValues } = useForm<UserType>({
    isActive: true,
    lockoutEnabled: true,
    roleNames: []
} as any, userApi, props);


const passwordRules = globalConfig.passwordComplexitySetting;

const rules = {
    userName: [
        { required: !props.entityId, message: '请输入用户名', trigger: 'blur' }
    ],
    email: [
        { required: true, message: '请输入电子邮件', trigger: 'blur' },
        { type: 'email', message: '请输入正确的电子邮件', trigger: 'blur' }
    ],
    password: [
        { required: !props.entityId, message: '请输入密码', trigger: 'blur' },
        { min: passwordRules.minLength, message: '密码长度至少6位', trigger: 'blur' },
        {
            validator: (_: any, value: any, callback: any) => {
                if (props.entityId && isEmpty(value)) callback();
                if (passwordRules.requireDigit && !/\d/.test(value)) {
                    callback('密码必须包含数字!');
                } else if (passwordRules.requireLowercase && !/[a-z]/.test(value)) {
                    callback('密码必须包含小写字母!');
                } else if (passwordRules.requireUppercase && !/[A-Z]/.test(value)) {
                    callback('密码必须包含大写字母!');
                } else if (passwordRules.requireNonAlphanumeric && !/[^a-zA-Z0-9]/.test(value)) {
                    callback('密码必须包含非字母数字字符!');
                } else {
                    callback();
                }
            }
            , trigger: 'blur'
        }
    ],
    confirmPassword: [
        { required: !props.entityId, message: '请输入确认密码', trigger: 'blur' },
        {
            validator: (_: any, value: any, callback: any) => {
                if (value !== formData.value.password) {
                    callback('两次输入的密码不一致!');
                } else {
                    callback();
                }
            }, trigger: 'blur'
        }
    ],

};

onMounted(() => {
    roleApi.getAll().then( (res:any) => {
        allRoles.value = res.items.map((item: any) => item.name);
    });
    if (props.entityId) {
        userApi.getRoles(props.entityId).then((res: any) => {
            setInitialValues({roleNames: res.items.map((item: any) => item.name)} as any)
        });
    }
});



</script>

<style lang="css" scoped>
.form-container {
    display: flex;
    flex-wrap: wrap;
}

.form-container .el-form-item {
    flex: 1 1 45%;
    /* 每个表单项占45%的宽度 */
    margin-right: 20px;
    /* 右侧间距 */
}

/* 如果需要将最后一列的右侧空白去掉 */
.form-container .el-form-item:nth-child(odd) {
    margin-right: 0;
}

</style>