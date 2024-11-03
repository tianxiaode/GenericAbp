<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <el-form-item :label="t('MenuManagement.Menu:Parent')" prop="parentId">
                <Select v-model="formData.parentId" :method="api.getList"
                    :value-field="'id'" :display-field="'name'"
                    placeholder="MenuManagement.Menu:Parent"></Select>
            </el-form-item>
            <el-form-item :label="t('MenuManagement.Menu:GroupName')" prop="groupName">
                <Select v-model="formData.groupName" :method="api.getAllGroups" allow-create
                    placeholder="MenuManagement.Menu:GroupName"></Select>
            </el-form-item>
            <el-form-item prop="name" :label="t('MenuManagement.Menu:Name')">
                <el-input v-model="formData.name" clearable autocomplete="off"
                    :placeholder="t('MenuManagement.Menu:Name')">
                </el-input>
            </el-form-item>
            <el-form-item :label="t('MenuManagement.Menu:Icon')" prop="icon">
                <IconSelect v-model="formData.icon"></IconSelect>
            </el-form-item>
            <el-form-item :label="t('MenuManagement.Menu:Router')" prop="router">
                <el-input v-model="formData.router" :placeholder="t('MenuManagement.Menu:Router')" clearable></el-input>
            </el-form-item>
            <el-form-item prop="order" :label="t('MenuManagement.Menu:Order')">
                <el-input v-model="formData.order" clearable autocomplete="off"
                    :placeholder="t('MenuManagement.Menu:Order')">
                </el-input>
            </el-form-item>
            <el-form-item :label="t('MenuManagement.Menu:IsEnabled')">
                <Switch v-model="formData.isEnabled" />
            </el-form-item>
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../dialogs/FormDialog.vue';

import { useEntityForm, useRepository, useI18n } from '~/composables'
import Switch from '../forms/Switch.vue';
import Select from '../forms/Select.vue';
import IconSelect from '../forms/IconSelect.vue';

const { t } = useI18n();
const entityId = defineModel('entityId');
const api = useRepository('menu');
const dialogVisible = defineModel<boolean>();
const rules = {
    name: { required: true },
    groupName: { required: true },
};


const { formData, dialogTitle, formDialogProps } = useEntityForm(api, entityId, dialogVisible, {
    initData:{
        isEnabled: true,
    },
    afterGetData(data: any){
        console.log(data);
    }
});




</script>