<template>
    <FormDialog v-bind="formDialogProps" v-model="formData" v-model:rules="rules" :title="dialogTitle"
        v-model:visible="dialogVisible">
        <template #form-items>
            <el-input type="hidden" v-model="formData.parentId"/>
            <Input :disabled="true" v-model="formData.parentName" :label="t(getLabel('Parent'))" form-item-prop="parentName" />
            <Input v-model="formData.name" form-item-prop="name" :label="t(getLabel('name'))" />
            <IconSelect v-model="formData.icon" form-item-prop="icon" :label="t(getLabel('icon'))" />
            <Input v-model="formData.router" form-item-prop="router" :label="t(getLabel('router'))" />
            <Input type="number" min="1" v-model="formData.order" :label="t(getLabel('order'))" form-item-prop="order"></Input>
            <Switch v-model="formData.isEnabled" :label="t(getLabel('isEnabled'))" form-item-prop="isEnabled" />
        </template>
    </FormDialog>
</template>

<script setup lang="ts">
import FormDialog from '../../dialogs/FormDialog.vue';

import { useEntityForm, useRepository, useI18n } from '~/composables'
import Switch from '../../forms/Switch.vue';
import IconSelect from '../../forms/IconSelect.vue';
import Input from '~/components/forms/Input.vue';
import { onMounted } from 'vue';

const { t } = useI18n();
const entityId = defineModel('entityId');
const api = useRepository('menu');
const dialogVisible = defineModel<boolean>();
const rules = {
    name: { required: true },
};



const props = defineProps({
    parent:{ type: Object, default: null }
})

const { formData, dialogTitle, formDialogProps, getLabel, setInitValues } = useEntityForm(api, entityId, dialogVisible, {
    initData:{
        isEnabled: true,
    },
});

onMounted(() => {
    setInitValues({
        parentId: props.parent?.id,
        parentName: props.parent?.name,
        order: (props.parent?.order || 0)  + 1
    })
})



</script>