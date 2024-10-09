<template>
    <FormDialog ref="formDialog" v-bind="$attrs" :submit="handleSubmit" :check-change="hasChanged"
        :reset-form="resetForm" :validate-form="validateForm">
        <template #form-items>
            <el-form :rules="rules" ref="formRef" :model="formData" label-width="100px" show-status-icon>
                <el-input v-model="formData.id" type="hidden" />
                <el-input v-model="formData.concurrencyStamp" type="hidden" />
                <el-form-item label="角色名称" prop="name">
                    <el-input v-model="formData.name" placeholder="请输入角色名称" clearable></el-input>
                </el-form-item>

                <el-form-item label="是否默认">
                    <el-switch v-model="formData.isDefault" active-text="是" inactive-text="否"></el-switch>
                </el-form-item>

                <el-form-item label="是否公开">
                    <el-switch v-model="formData.isPublic" active-text="是" inactive-text="否"></el-switch>
                </el-form-item>

            </el-form>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';
import { RoleType, roleApi } from '../../repositories';
import { useForm } from '../../composables/useForm'

const props = defineProps({
    entityId: {
        type: String,
        default: ''
    }
});

const { formRef, formData, handleSubmit, hasChanged, resetForm, validateForm } = useForm<RoleType>({} as RoleType, roleApi, props);

const rules = {
    name: [
        { required: true, message: '请输入角色名称', trigger: 'blur' }
    ]
};





</script>
