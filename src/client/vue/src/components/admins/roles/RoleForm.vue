<template>
    <BaseDialog v-bind="formDialogProps" ref="formDialogRef">
        <el-form v-bind="formProps" :rules="rules" :model="formData" ref="formRef">
            <Input :label="t(getLabel('RoleName'))" v-model="formData.name" form-item-prop="name" />
            <Switch v-model="formData.isDefault" :label="t(getLabel('IsDefault'))" form-item-prop="isDefault" />
            <Switch v-model="formData.isPublic" :label="t(getLabel('IsPublic'))" form-item-prop="isPublic" />
        </el-form>
    </BaseDialog>
</template>

<script setup lang="ts">
import BaseDialog from '~/components/dialogs/BaseDialog.vue';
import { useFormDialog, useI18n, useRepository } from '~/composables';
import Switch from '../../forms/Switch.vue';
import Input from '../../forms/Input.vue';

const { t } = useI18n();
const api = useRepository('Role');
const entityId = defineModel('entityId');

const visible = defineModel<boolean>({ default: false });
const { formData, formDialogRef, formRef,
    getLabel, formDialogProps, formProps
} = useFormDialog(api, entityId, { visible });

const rules = {
    name: { required: true }
};

</script>
