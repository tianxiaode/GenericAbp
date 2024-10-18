<template>
    <FormDialog ref="formRef"  :form-data="formData" :rules="rules" :on-ok="submitForm"
        :title = "dialogTitle" :visible="dialogVisible"
    >
        <template #form-items>
            <el-form-item label="角色名称" prop="name">
                <el-input v-model="formData.name" placeholder="请输入角色名称" clearable></el-input>
            </el-form-item>

            <el-form-item label="是否默认">
                <el-switch v-model="formData.isDefault" active-text="是" inactive-text="否"></el-switch>
            </el-form-item>

            <el-form-item label="是否公开">
                <el-switch v-model="formData.isPublic" active-text="是" inactive-text="否"></el-switch>
            </el-form-item>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';
import { roleApi } from '~/repositories';
import { useFormDialog } from '~/composables';

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

const { formRef, formData, dialogTitle,dialogVisible, submitForm } = useFormDialog(roleApi, props);

const rules = {
    name: [
        { required: true, message: '请输入角色名称', trigger: 'blur' }
    ]
};





</script>
