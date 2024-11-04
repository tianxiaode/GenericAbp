<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <el-tabs v-model="activeTab" style="min-height: 540px;">
                <el-tab-pane :label="t('AbpIdentity.UserInformations')" name="basic">
                    <Input :label="t(getLabel('userName'))" v-model="formData.userName" clearable
                        :disabled="!!entityId && !appConfig.isUserNameUpdateEnabled" />
                    <Input :label="t(getLabel('email'))" v-model="formData.email" clearable
                        :disabled="!!entityId && !appConfig.isEmailUpdateEnabled" />
                    <el-form-item :label="t(getLabel('password'))" prop="password">
                        <el-input type="password" v-model="formData.password" :placeholder="t(getLabel('password'))"
                            clearable show-password autocomplete="off"></el-input>
                        <PasswordStrength :value="formData.password" />
                    </el-form-item>
                    <el-form-item :label="t('Pages.Register.ConfirmPassword')" prop="confirmPassword">
                        <el-input type="password" v-model="formData.confirmPassword"
                            :placeholder="t('Pages.Register.ConfirmPassword')" clearable show-password
                            autocomplete="off"></el-input>
                    </el-form-item>
                    <el-form-item prop="name" :label="t(getLabel('name'))">
                        <el-input v-model="formData.name" clearable autocomplete="off"
                            :placeholder="t(getLabel('name'))">
                        </el-input>
                    </el-form-item>
                    <el-form-item prop="surname" :label="t(getLabel('surname'))">
                        <el-input v-model="formData.surname" clearable autocomplete="off"
                            :placeholder="t(getLabel('surname'))">
                        </el-input>
                    </el-form-item>
                    <el-form-item prop="phoneNumber" :label="t(getLabel('phoneNumber'))">
                        <el-input v-model="formData.phoneNumber" clearable autocomplete="off"
                            :placeholder="t(getLabel('phoneNumber'))">
                        </el-input>
                    </el-form-item>
                    <!-- <el-form-item></el-form-item> -->
                    <div class="grid cols-2 gap-2">
                        <el-form-item :label="t(getLabel('isActive'))">
                            <Switch v-model="formData.isActive" />
                        </el-form-item>
                        <el-form-item :label="t(getLabel('lockoutEnabled'))">
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
        confirmPassword: { required: !entityId.value }
    })
};

const { } = useConfig(refreshRules);

onMounted(() => {
    roleApi.getAll().then((res: any) => {
        allRoles.value = res.items.map((role: any) => role.name);
    });
});

</script>
