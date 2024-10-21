<template>
    <FormDialog ref="formRef" width="800px" :form-data="formData" :rules="rules" :on-ok="submitForm"
        :title="dialogTitle" v-bind="$attrs" :reset="resetForm" :before-close="checkChange" :label-width="160">
        <template #form-items>
            <el-tabs v-model="activeTab" style="height: 540px;">
                <el-tab-pane :label="t('AbpIdentity.UserInformations')" name="basic">
                    <el-form-item :label="t('AbpIdentity.DisplayName:UserName')" prop="userName">
                        <el-input v-model="formData.userName" :placeholder="t('AbpIdentity.DisplayName:UserName')"
                            clearable :readonly="!!props.entityId" :class="{ 'is-readonly': !!props.entityId }">
                        </el-input>
                    </el-form-item>
                    <el-form-item :label="t('AbpIdentity.DisplayName:Email')" prop="email">
                        <el-input v-model="formData.email" :placeholder="t('AbpIdentity.DisplayName:Email')"
                            clearable></el-input>
                    </el-form-item>
                    <el-form-item :label="t('AbpIdentity.DisplayName:Password')" prop="password">
                        <el-input type="password" v-model="formData.password"
                            :placeholder="t('AbpIdentity.DisplayName:Password')" clearable show-password
                            autocomplete="off"></el-input>
                        <PasswordStrength :value="formData.password" />
                    </el-form-item>
                    <el-form-item :label="t('Pages.Register.ConfirmPassword')" prop="confirmPassword">
                        <el-input type="password" v-model="formData.confirmPassword"
                            :placeholder="t('Pages.Register.ConfirmPassword')" clearable show-password
                            autocomplete="off"></el-input>
                    </el-form-item>
                    <el-form-item prop="name" :label="t('AbpIdentity.DisplayName:Name')">
                        <el-input v-model="formData.name" clearable autocomplete="off"
                            :placeholder="t('AbpIdentity.DisplayName:Name')">
                        </el-input>
                    </el-form-item>
                    <el-form-item prop="surname" :label="t('AbpIdentity.DisplayName:Surname')">
                        <el-input v-model="formData.surname" clearable autocomplete="off"
                            :placeholder="t('AbpIdentity.DisplayName:Surname')">
                        </el-input>
                    </el-form-item>
                    <el-form-item prop="phoneNumber" :label="t('AbpIdentity.DisplayName:PhoneNumber')">
                        <el-input v-model="formData.phoneNumber" clearable autocomplete="off"
                            :placeholder="t('AbpIdentity.DisplayName:PhoneNumber')">
                        </el-input>
                    </el-form-item>
                    <!-- <el-form-item></el-form-item> -->
                    <div class="grid cols-2 gap-2">
                        <el-form-item :label="t('AbpIdentity.DisplayName:IsActive')">
                            <Switch v-model="formData.isActive" />
                        </el-form-item>
                        <el-form-item :label="t('AbpIdentity.DisplayName:LockoutEnabled')">
                            <Switch v-model="formData.lockoutEnabled" />
                        </el-form-item>

                    </div>

                </el-tab-pane>
                <el-tab-pane v-if="allowedManageRoles" :label="t('AbpIdentity.Roles')" name="roles">
                    <el-checkbox-group v-model="formData.roleNames">
                        <div class="grid cols-2 gap-2">
                            <el-checkbox v-for="role in allRoles" :value="role" :key="role" :label="role">
                                {{ role }}
                            </el-checkbox>
                        </div>
                    </el-checkbox-group>

                </el-tab-pane>
            </el-tabs>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';
import PasswordStrength from '../accounts/PasswordStrength.vue';

import { useConfig, useEntityForm, useRepository, useFormRules, useI18n } from '~/composables'
import { appConfig } from '~/libs';
import { onMounted, ref } from 'vue';
import Switch from '../forms/Switch.vue';

const activeTab = ref('basic');
const allRoles = ref([] as string[]);
const { t } = useI18n();
const userApi = useRepository('user');
const roleApi = useRepository('role');
const allowedManageRoles = userApi.canManageRoles;
const props = defineProps({
    visible: {
        type: Boolean,
        default: false
    },
    entityId: {
        type: String,
        default: ''
    }
});

const formRules = {
    userName: { required: true },
    email: { required: true, email: true },
    password: { required: !props.entityId },
    confirmPassword: { equalTo: 'password' }
};


const { formRef, formData, dialogTitle, initValues, resetForm, submitForm, checkChange } = useEntityForm(userApi, props);
const { rules, updateRules } = useFormRules(formRules, formRef);


const refreshRules = () => {
    updateRules({
        password: {
            required: !props.entityId,
            ...appConfig.passwordComplexitySetting
        },
        confirmPassword: { required: !props.entityId }
    })
};

const { } = useConfig(refreshRules);

onMounted(() => {
    roleApi.getAll().then((res: any) => {
        allRoles.value = res.items.map((role: any) => role.name);
    });
    if (props.entityId) {
        if (allowedManageRoles) {
            userApi.getRoles(props.entityId).then((res: any) => {
                let roles = res.items.map((role: any) => role.name);
                formData.value.roleNames = [...roles];
                initValues.value.roleNames = roles;
            });
        }
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