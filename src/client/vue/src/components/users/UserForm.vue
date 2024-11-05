<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
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
                        <Switch v-model="formData.isActive" :label="t(getLabel('isActive'))" />
                        <Switch v-model="formData.lockoutEnabled" :label="t(getLabel('lockoutEnabled'))" />
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

import { useConfig, useEntityForm, useRepository, useFormRules, useI18n } from '~/composables'
import { appConfig } from '~/libs';
import { onMounted, ref } from 'vue';
import Switch from '../forms/Switch.vue';
import Input from '../forms/Input.vue';

const props = defineProps({
    api: Object as () => any,
})

const activeTab = ref('basic');
const allRoles = ref([] as string[]);
const { t } = useI18n();
const entityId = defineModel('entityId');
const roleApi = useRepository('role');
const dialogVisible = defineModel<boolean>();
const allowedManageRoles = props.api.canManageRoles;
const formRules = {
    userName: { required: true },
    email: { required: true, email: true },
    password: { required: !entityId.value },
    confirmPassword: { equalTo: 'password' }
};


const { formRef, formData, dialogTitle, formDialogProps, getLabel } = useEntityForm(props.api, entityId, dialogVisible, {
    initData: { isActive: true, lockoutEnabled: true },
    props: {
        dialogProps: { width: '800px' },
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
