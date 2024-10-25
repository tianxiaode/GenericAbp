<template>
    <FormDialog ref="formRef" width="600px" :form-data="formData" :rules="rules" :on-ok="submitForm"
        :title="dialogTitle" v-bind="$attrs" :reset="resetForm" :before-close="checkChange" :label-width="160">
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
import PropertyInput from '../forms/PropertyInput.vue';
import ValueListInput from '../forms/ValueListInput.vue';
import { isEmpty } from '~/libs';

const { t } = useI18n();
const api = useRepository('scope');
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

const formRules = {
    name: { required: true }
};


const { formRef, formData, dialogTitle, resetForm, submitForm, checkChange } = useEntityForm(api, props);
const { rules } = useFormRules(formRules, formRef);



onMounted(() => {
    if (props.entityId) {
        api.getEntity(props.entityId).then((res: any) => {
            console.log(res);
            if (isEmpty(res.properties)) res.properties = {};
            if (isEmpty(res.resources)) res.resources = [];
            formData.value = res;
        })
    } else {
        formData.value = {
            properties: {},
            resources: []
        }
    }
});

</script>
