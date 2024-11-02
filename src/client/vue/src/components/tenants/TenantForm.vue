<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <el-form-item :label="t('AbpTenantManagement.DisplayName:TenantName')" prop="name">
                <el-input v-model="formData.name" :placeholder="t('AbpTenantManagement.DisplayName:TenantName')"
                    clearable></el-input>
            </el-form-item>
            <el-form-item v-if="!isEdit" :label="t('AbpTenantManagement.DisplayName:AdminEmailAddress')"
                prop="adminEmailAddress">
                <el-input v-model="formData.adminEmailAddress"
                    :placeholder="t('AbpTenantManagement.DisplayName:AdminEmailAddress')" clearable></el-input>
            </el-form-item>
            <el-form-item v-if="!isEdit" :label="t('AbpTenantManagement.DisplayName:AdminPassword')"
                prop="adminPassword">
                <el-input v-model="formData.adminPassword"
                    :placeholder="t('AbpTenantManagement.DisplayName:AdminPassword')" clearable type="password"
                    show-password></el-input>
                <PasswordStrength :value="formData.adminPassword" />
            </el-form-item>
            <el-form-item v-if="!isEdit" prop="confirmPassword" :label="t('Pages.Profile.ConfirmNewPassword')">
                <el-input v-model="formData.confirmPassword" clearable show-password autocomplete="off"
                    :placeholder="t('Pages.Profile.ConfirmNewPassword')">
                </el-input>
            </el-form-item>

        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import { computed, ref } from 'vue';
import FormDialog from '../dialogs/FormDialog.vue';
import PasswordStrength from '../accounts/PasswordStrength.vue'
import { useEntityForm, useFormRules, useI18n, useRepository } from '~/composables';
import { isEmpty } from '~/libs';

const { t } = useI18n();
const api = useRepository('tenant');
const entityId = defineModel('entityId');
const dialogVisible = defineModel<boolean>();
const { formRef, formData, dialogTitle, formDialogProps } = useEntityForm(api, entityId, dialogVisible);
const isEdit = computed(() => {
    updateRules({
        name: { required: true },
        adminEmailAddress: {},
        adminPassword: {},
        confirmPassword: {}
    })
    return !isEmpty(entityId.value);
})
const defaultRules = {
    name: { required: true },
    adminEmailAddress: { required: true, email: true },
    adminPassword: { required: true },
    confirmPassword: { equalTo: 'adminPassword' }
};

const { rules, updateRules } = useFormRules(defaultRules, formRef);
</script>
