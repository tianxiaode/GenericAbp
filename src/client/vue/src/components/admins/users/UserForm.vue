<template>
    <BaseDialog v-bind="formDialogProps" ref="formDialogRef">
        <el-form v-bind="formProps" :rules="rules" :model="formData" ref="formRef">
            <el-tabs v-model="activeTab" style="min-height: 540px;">
                <el-tab-pane :label="t('AbpIdentity.UserInformations')" name="basic">
                    <Input :label="t(getLabel('userName'))" v-model="formData.userName" form-item-prop="userName" 
                        :disabled="!!entityId && !appConfig.isUserNameUpdateEnabled" />
                    <Input :label="t(getLabel('email'))" v-model="formData.email" form-item-prop="email"
                        :disabled="!!entityId && !appConfig.isEmailUpdateEnabled" />
                    <Input type="password" :label="t(getLabel('password'))" v-model="formData.password" :show-password-strength="true" 
                        form-item-prop="password" 
                    />
                    <Input type="password" :label="t('Pages.Register.ConfirmPassword')" v-model="formData.confirmPassword" 
                        form-item-prop="confirmPassword" 
                    />
                    <Input :label="t(getLabel('name'))" v-model="formData.name" form-item-prop="name" />
                    <Input :label="t(getLabel('surname'))" v-model="formData.surname" form-item-prop="surname" />
                    <Input :label="t(getLabel('phoneNumber'))" v-model="formData.phoneNumber" form-item-prop="phoneNumber" />
                    <!-- <el-form-item></el-form-item> -->
                    <div class="grid cols-2 gap-2">
                        <Switch v-model="formData.isActive" :label="t(getLabel('isActive'))" form-item-prop="isActive" />
                        <Switch v-model="formData.lockoutEnabled" :label="t(getLabel('lockoutEnabled'))" form-item-prop="lockoutEnabled" />
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
        </el-form>
    </BaseDialog>
</template>

<script setup lang="ts">
import BaseDialog from '~/components/dialogs/BaseDialog.vue';
import { useConfig, useFormDialog, useRepository, useFormRules, useI18n } from '~/composables'
import { appConfig } from '~/libs';
import { onMounted, ref } from 'vue';
import Switch from '../../forms/Switch.vue';
import Input from '../../forms/Input.vue';

const activeTab = ref('basic');
const allRoles = ref([] as string[]);
const { t } = useI18n();
const entityId = defineModel('entityId');
const api = useRepository('user');
const roleApi = useRepository('role');
const visible = defineModel<boolean>({ default: false });
const allowedManageRoles = api.canManageRoles;
const formRules = {
    userName: { required: true },
    email: { required: true, email: true },
    password: { required: !entityId.value },
    confirmPassword: { equalTo: 'password' }
};

const { formData, formDialogRef, formRef,
    getLabel, formDialogProps, formProps
} = useFormDialog(api, entityId, { 
    visible, 
    dialogProps: {
        width: '800px' ,
    },
    initData:{
        isActive: true,
        lockoutEnabled:true
    },
    loadAdditionalData: allowedManageRoles
});


const { rules, updateRules } = useFormRules(formRules, formRef);


const refreshRules = () => {
    updateRules({
        password: {
            required: !entityId.value,
            ...appConfig.passwordComplexitySetting
        },
    })
};

const { } = useConfig(refreshRules);

onMounted(() => {
    roleApi.getAll().then((res: any) => {
        allRoles.value = res.items.map((role: any) => role.name);
    });
});

</script>
