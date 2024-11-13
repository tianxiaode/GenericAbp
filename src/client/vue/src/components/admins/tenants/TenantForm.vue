<template>
    <BaseDialog v-bind="formDialogProps" ref="formDialogRef">
        <el-form v-bind="formProps" :rules="rules" :model="formData" ref="formRef">
            <Input :label="t(getLabel('TenantName'))" v-model="formData.name" form-item-prop="name" />
            <Input v-if="!isEdit" :label="t(getLabel('AdminEmailAddress'))" v-model="formData.adminEmailAddress" form-item-prop="adminEmailAddress" />
            <Input v-if="!isEdit" :label="t(getLabel('AdminPassword'))" v-model="formData.adminPassword" form-item-prop="adminPassword" 
                type="password" :show-password-strength="true"
            />
            <Input v-if="!isEdit" :label="t('Pages.Profile.ConfirmNewPassword')" v-model="formData.confirmPassword" form-item-prop="confirmPassword" 
                type="password"
            />
        </el-form>
    </BaseDialog>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import BaseDialog from '~/components/dialogs/BaseDialog.vue';
import { useFormDialog, useFormRules, useI18n, useRepository } from '~/composables';
import { isEmpty } from '~/libs';
import Input from '../../forms/Input.vue';

const { t } = useI18n();
const api = useRepository('tenant');
const entityId = defineModel('entityId');
const visible = defineModel<boolean>({ default: false });
const { formData, formDialogRef, formRef,
    getLabel, formDialogProps, formProps
} = useFormDialog(api, entityId, { visible , formProps:{ labelWidth: '180px'}});

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
