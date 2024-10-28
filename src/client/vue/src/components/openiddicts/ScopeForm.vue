<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <el-form-item :label="t('OpenIddict.Scope:Name')" prop="name">
                <el-input v-model="formData.name"></el-input>
            </el-form-item>
            <el-form-item :label="t('OpenIddict.Scope:DisplayName')" prop="displayName">
                <el-input v-model="formData.displayName"></el-input>
            </el-form-item>
            <el-form-item :label="t('OpenIddict.Scope:Description')" prop="description">
                <el-input type="textarea" v-model="formData.description" :row="3"></el-input>
            </el-form-item>
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
import FormDialog from '../dialogs/FormDialog.vue';

import { useEntityForm, useRepository, useFormRules, useI18n } from '~/composables'
import { onMounted } from 'vue';
import ValueListInput from '../forms/ValueListInput.vue';
import { isEmpty } from '~/libs';

const { t } = useI18n();
const api = useRepository('scope');
const entityId = defineModel('entityId');
const dialogVisible = defineModel<boolean>();


const formRules = {
    name: { required: true }
};


const { formRef, formData, dialogTitle, formDialogProps, setInitValues } = useEntityForm(api, entityId,dialogVisible);
const { rules } = useFormRules(formRules, formRef);



onMounted(() => {
    if (entityId.value) {
        api.getEntity(entityId.value).then((res: any) => {
            if (isEmpty(res.resources)) res.resources = [];
            setInitValues(res);
        })
    } else {
        setInitValues({
            resources: []
        })
    }
});

</script>
