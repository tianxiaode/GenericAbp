<template>
    <div v-bind="$attrs" class="h-full flex flex-col gap-2 box-border">
        <div class="flex-1 overflow-auto">
            <div class="flex flex-row w-full gap-2 py-1" v-for="(item, index) in properties" :key="index">
                <div class="flex-1">{{ item }}</div>
                <div class="w-10 text-center">
                    <IconButton @click="removeProperty(item)" icon="fa fa-trash danger" size="small" circle>
                    </IconButton>
                </div>
            </div>
            <div v-if="properties.length === 0" class="text-center text-lg font-bold">{{ t('Components.NoData') }}</div>
        </div>
        <el-form ref="formRef" :model="formData" :rules="rules" :validate-on-rule-change="false"
            style="border-top: 1px;"
        >
            <div class="flex flex-row w-full gap-2" style="border-top: 1px solid #ccc; padding-top: 0.5rem;">
                <el-form-item prop="value" class="flex-1">
                    <el-input v-model="formData.value" size="small" 
                        :placeholder="t('Components.PropertyInput.Value')" />
                </el-form-item>
                <el-form-item >
                    <div class="w-10 text-center">
                        <IconButton @click="handleSubmit" icon="fa fa-check primary" circle size="small"></IconButton>
                    </div>
                </el-form-item>
            </div>
        </el-form>
    </div>
</template>

<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from 'vue';
import IconButton from '../buttons/IconButton.vue';
import { useForm, useFormRules, useI18n } from '~/composables';

const props = defineProps({
    modelValue: {
        type: Array,
        default: () => ([])
    },
    rules: {
        type: Object,
        default: () => ({})
    },
});

const emit = defineEmits(['update:modelValue']);
const { t } = useI18n();
const properties = ref(props.modelValue);

watch(properties, (value) => {
    emit('update:modelValue', value);
});

watch(() => props.modelValue, (newValue) => {
    properties.value = newValue;
});

const formRules = {
    value: {
        required: true,
        custom: (value: any) => {
            const exists = properties.value.includes(value);
            if (exists) {
                return (t.value('Components.ValueAlreadyExist'));
            }
            return true;
        },
        ...props.rules
    }
}

console.log(formRules);

const { formData, formRef, handleSubmit } = useForm(addProperty)
const { rules } = useFormRules(formRules, formRef)
function addProperty() {
    properties.value.push(formData.value.value);
    formData.value.value = '';
    formRef.value.clearValidate();
}

function removeProperty(value: any) {
    properties.value = properties.value.filter(item => item !== value);
}
</script>

