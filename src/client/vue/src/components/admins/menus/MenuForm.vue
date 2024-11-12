<template>
    <BaseDialog v-bind="formDialogProps" ref="formDialogRef">
        <el-form v-bind="formProps" :rules="rules" :model="formData" ref="formRef">
            <Input :disabled="true" v-model="formData.parentName" :label="t(getLabel('Parent'))"
                form-item-prop="parentName" />
            <Input v-model="formData.name" form-item-prop="name" :label="t(getLabel('name'))" />
            <IconSelect v-model="formData.icon" form-item-prop="icon" :label="t(getLabel('icon'))" />
            <Input v-model="formData.router" form-item-prop="router" :label="t(getLabel('router'))" />
            <Input type="number" min="1" v-model="formData.order" :label="t(getLabel('order'))"
                form-item-prop="order"></Input>
            <Switch v-model="formData.isEnabled" :label="t(getLabel('isEnabled'))" form-item-prop="isEnabled" />
        </el-form>
    </BaseDialog>
</template>

<script setup lang="ts">
import { useRepository, useI18n, useFormDialog } from '~/composables'
import Switch from '../../forms/Switch.vue';
import IconSelect from '../../forms/IconSelect.vue';
import Input from '~/components/forms/Input.vue';
import { onMounted } from 'vue';
import BaseDialog from '~/components/dialogs/BaseDialog.vue';

const { t } = useI18n();
const entityId = defineModel('entityId');
const api = useRepository('menu');
const visible = defineModel<boolean>({default: false});
const rules = {
    name: { required: true },
};



const props = defineProps({
    parent: { type: Object, default: null }
})

const { formData,formDialogRef,formRef, getLabel, formDialogProps, formProps, formSetInitValues } = useFormDialog(
    api,
    entityId,
    {
        visible,
        initData: {
            isEnabled: true,
        },
    });

onMounted(() => {
    formSetInitValues({
        parentId: props.parent?.id,
        parentName: props.parent?.name,
        order: (props.parent?.order || 0) + 1
    })
})



</script>