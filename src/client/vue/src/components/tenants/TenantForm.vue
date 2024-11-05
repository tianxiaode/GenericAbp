<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <Input :label="t(getLabel('TenantName'))" v-model="formData.name" form-item-prop="name" />
            <Input v-if="!isEdit" :label="t(getLabel('AdminEmailAddress'))" v-model="formData.adminEmailAddress" form-item-prop="adminEmailAddress" />
            <Input v-if="!isEdit" :label="t(getLabel('AdminPassword'))" v-model="formData.adminPassword" form-item-prop="adminPassword" 
                type="password" :show-password-strength="true"
            />
            <Input v-if="!isEdit" :label="t('Pages.Profile.ConfirmNewPassword')" v-model="formData.confirmPassword" form-item-prop="confirmPassword" 
                type="password"
            />
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import { computed } from 'vue';
import FormDialog from '../dialogs/FormDialog.vue';
import { useEntityForm, useFormRules, useI18n, useRepository } from '~/composables';
import { isEmpty } from '~/libs';
import Input from '../forms/Input.vue';

const { t } = useI18n();
const api = useRepository('tenant');
const entityId = defineModel('entityId');
const dialogVisible = defineModel<boolean>();
const { formRef, formData, dialogTitle, formDialogProps,getLabel } = useEntityForm(api, entityId, dialogVisible);
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
