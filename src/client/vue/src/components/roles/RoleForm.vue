<template>
    <FormDialog ref="formRef"  :form-data="formData"  :rules="rules" :on-ok="submitForm"
        :title = "dialogTitle" v-bind="$attrs" :reset="resetForm"  :before-close="checkChange"
    >
        <template #form-items>
            <el-form-item :label="t('AbpIdentity.DisplayName:RoleName')" prop="name">
                <el-input v-model="formData.name" :placeholder="t('AbpIdentity.DisplayName:RoleName')" clearable></el-input>
            </el-form-item>

            <el-form-item :label="t('AbpIdentity.DisplayName:IsDefault')">
                <el-switch v-model="formData.isDefault" :active-text="t('Components.Yes')" :inactive-text="t('Components.No')"></el-switch>
            </el-form-item>

            <el-form-item :label="t('AbpIdentity.DisplayName:IsPublic')">
                <el-switch v-model="formData.isPublic" :active-text="t('Components.Yes')" :inactive-text="t('Components.No')"></el-switch>
            </el-form-item>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';
import { useEntityForm,useI18n,useRepository } from '~/composables';

const { t } = useI18n();
const roleApi = useRepository('Role');
const props = defineProps({
    visible: {
        type: Boolean,
        default: false
    },
    entityId: {
        type: String,
        default: ''
    }
});

const { formRef, formData, dialogTitle, resetForm, submitForm, checkChange } = useEntityForm(roleApi, props);

const rules = {
    name: [
        { required: true, message: '请输入角色名称', trigger: 'blur' }
    ]
};





</script>
