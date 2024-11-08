<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
                <Select v-model="formData.parentId" :method="api.getList"
                    :label="t(getLabel('parent'))" form-item-prop="parentId"
                    :value-field="'id'" :display-field="'name'"
                    placeholder="MenuManagement.Menu:Parent"></Select>
            <el-form-item :label="t(getLabel('GroupName'))" prop="groupName">
                <Select v-model="formData.groupName" :method="api.getAllGroups" allow-create
                    placeholder="MenuManagement.Menu:GroupName"></Select>
            </el-form-item>
            <el-form-item prop="name" :label="t(getLabel('name'))">
                <el-input v-model="formData.name" clearable autocomplete="off"
                    :placeholder="t('MenuManagement.Menu:Name')">
                </el-input>
            </el-form-item>
            <el-form-item :label="t(getLabel('icon'))" prop="icon">
                <IconSelect v-model="formData.icon"></IconSelect>
            </el-form-item>
            <el-form-item :label="t(getLabel('router'))" prop="router">
                <el-input v-model="formData.router" :placeholder="t(getLabel('router'))" clearable></el-input>
            </el-form-item>
            <Input v-model="formData.order" :label="t(getLabel('order'))" form-item-prop="order"></Input>
            <Switch v-model="formData.isEnabled" :label="t(getLabel('isEnabled'))" />
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../../dialogs/FormDialog.vue';

import { useEntityForm, useRepository, useI18n } from '~/composables'
import Switch from '../../forms/Switch.vue';
import Select from '../../forms/Select.vue';
import IconSelect from '../../forms/IconSelect.vue';
import Input from '~/components/forms/Input.vue';

const { t } = useI18n();
const entityId = defineModel('entityId');
const api = useRepository('menu');
const dialogVisible = defineModel<boolean>();
const rules = {
    name: { required: true },
    groupName: { required: true },
};


const { formData, dialogTitle, formDialogProps, getLabel } = useEntityForm(api, entityId, dialogVisible, {
    initData:{
        isEnabled: true,
    },
    afterGetData(data: any){
        console.log(data);
    }
});




</script>