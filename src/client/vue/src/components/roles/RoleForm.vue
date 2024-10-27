<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <el-form-item :label="t('AbpIdentity.DisplayName:RoleName')" prop="name">
                <el-input v-model="formData.name" :placeholder="t('AbpIdentity.DisplayName:RoleName')"
                    clearable></el-input>
            </el-form-item>

            <el-form-item :label="t('AbpIdentity.DisplayName:IsDefault')">
                <Switch v-model="formData.isDefault" />
            </el-form-item>

            <el-form-item :label="t('AbpIdentity.DisplayName:IsPublic')">
                <Switch v-model="formData.isPublic" />
            </el-form-item>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';
import { useEntityForm, useI18n, useRepository } from '~/composables';
import Switch from '../forms/Switch.vue';

const { t } = useI18n();
const roleApi = useRepository('Role');
const entityId = defineModel('entityId');
const dialogVisible = defineModel<boolean>();
const { formData, dialogTitle, formDialogProps } = useEntityForm(roleApi, entityId,dialogVisible);

const rules = {
    name: { required: true }
};





</script>
