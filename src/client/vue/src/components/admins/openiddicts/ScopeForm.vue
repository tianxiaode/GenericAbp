<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <Input v-model="formData.name" :label="t(getLabel('name'))" form-item-prop="name" />
            <Input v-model="formData.displayName" :label="t(getLabel('displayName'))" form-item-prop="displayName" />
            <Input type="textarea" :rows="3" v-model="formData.description" :label="t(getLabel('description'))" form-item-prop="description" />
            <div class="flex flex-row gap-4 " style="height: 300px; ">
                <fieldset class="flex-1 stretch ">
                    <legend class="text-lg font-bold">{{ t('OpenIddict.Scope:Resources') }}</legend>
                    <ValueListInput v-model="formData.resources" class="h-full">
                    </ValueListInput>
                </fieldset>
            </div>

        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../../dialogs/FormDialog.vue';

import { useEntityForm, useRepository, useFormRules, useI18n } from '~/composables'
import ValueListInput from '../../forms/ValueListInput.vue';
import Input from '../../forms/Input.vue';

const { t } = useI18n();
const api = useRepository('scope');
const entityId = defineModel('entityId');
const dialogVisible = defineModel<boolean>();


const formRules = {
    name: { required: true }
};


const { formRef, formData, dialogTitle, formDialogProps, getLabel } = useEntityForm(api, entityId,dialogVisible,{
    initData:{
        resources: []
    }
});
const { rules } = useFormRules(formRules, formRef);




</script>
