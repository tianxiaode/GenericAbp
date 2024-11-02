<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <el-tabs v-model="activeTab" style="min-height: 540px;">
                <el-tab-pane :label="t('AbpIdentity.UserInformations')" name="basic">
                    <el-form-item :label="t('AbpIdentity.DisplayName:UserName')" prop="userName">
                        <el-input v-model="formData.userName" :placeholder="t('AbpIdentity.DisplayName:UserName')"
                            clearable :readonly="!!entityId" :class="{ 'is-readonly': !!entityId }">
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
                            <el-checkbox v-for="role in allRoles" :value="role" :key="role">
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
const entityId = defineModel('entityId');
const userApi = useRepository('user');
const roleApi = useRepository('role');
const dialogVisible = defineModel<boolean>();
const allowedManageRoles = userApi.canManageRoles;
const formRules = {
    userName: { required: true },
    email: { required: true, email: true },
    password: { required: !entityId.value },
    confirmPassword: { equalTo: 'password' }
};


const { formRef,formData, dialogTitle, formDialogProps,setInitValues } = useEntityForm(userApi, entityId,dialogVisible,{
    props:{
        dialogProps:{ width: '800px'},
    }
});
const { rules, updateRules } = useFormRules(formRules, formRef);


const refreshRules = () => {
    updateRules({
        password: {
            required: !entityId.value,
            ...appConfig.passwordComplexitySetting
        },
        confirmPassword: { required: !entityId.value }
    })
};

const { } = useConfig(refreshRules);

onMounted(() => {
    roleApi.getAll().then((res: any) => {
        allRoles.value = res.items.map((role: any) => role.name);
    });
    if (entityId.value) {
        if (allowedManageRoles) {
            userApi.getRoles(entityId.value).then((res: any) => {
                let roles = res.items.map((role: any) => role.name);
                setInitValues({roleNames:[...roles]});
            });
        }
    }
});

</script>

