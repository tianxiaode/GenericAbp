<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <Input :label="t(getLabel('RoleName'))" v-model="formData.name" form-item-prop="name" />
            <Switch v-model="formData.isDefault" :label="t(getLabel('IsDefault'))" form-item-prop="isDefault" />
            <Switch v-model="formData.isPublic" :label="t(getLabel('IsPublic'))" form-item-prop="isPublic" />
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../../dialogs/FormDialog.vue';
import { useEntityForm, useI18n, useRepository } from '~/composables';
import Switch from '../../forms/Switch.vue';
import Input from '../../forms/Input.vue';

const { t } = useI18n();
const roleApi = useRepository('Role');
const entityId = defineModel('entityId');
const dialogVisible = defineModel<boolean>();
const { formData, dialogTitle, formDialogProps, getLabel } = useEntityForm(roleApi, entityId, dialogVisible);

const rules = {
    name: { required: true }
};

</script>
