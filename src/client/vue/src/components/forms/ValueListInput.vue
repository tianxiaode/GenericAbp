<template>
    <div v-bind="$attrs" class="h-full flex flex-col gap-2 box-border">
        <div class="flex flex-row justify-between items-center" style="border-bottom:1px solid #ccc; padding-bottom: 0.5rem;">
            <div class="flex-1 font-bold text-lg">{{ t(label) }}</div>
            <IconButton @click="formVisible = true" icon="fa fa-plus primary" size="small" circle></IconButton>
        </div>

        <el-form ref="formRef" :model="formData" :rules="rules" :validate-on-rule-change="false" v-if="formVisible">
            <div class="flex flex-row w-full gap-2">
                <el-form-item prop="value" class="flex-1">
                    <el-input v-model="formData.value" size="small" 
                        :placeholder="t('Components.PropertyInput.Value')" />
                </el-form-item>
                <el-form-item >
                    <div class="w-20 text-center">
                        <IconButton @click="handleSubmit" icon="fa fa-check primary" circle size="small"></IconButton>
                        <IconButton @click="formVisible = false" icon="fa fa-times danger" circle size="small"></IconButton>
                    </div>
                </el-form-item>
            </div>
        </el-form>
        <div class="flex-1 overflow-auto">
            <div class="flex flex-row w-full gap-2 py-1" v-for="(item, index) in properties" :key="index">
                <div class="flex-1">{{ item }}</div>
                <div class="w-10 text-center">
                    <IconButton @click="removeProperty(item)" icon="fa fa-trash danger" size="small" circle>
                    </IconButton>
                </div>
            </div>
        </div>
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
    label: {
        type: String,
        default: 'Components.PropertyInput.Label'
    }
});

const emit = defineEmits(['update:modelValue']);
const { t } = useI18n();
const formVisible = ref(false);
const properties = ref(props.modelValue);

watch(properties, (value) => {
    console.log(value);
    emit('update:modelValue', value);
});

watch(() => props.modelValue, (newValue) => {
    properties.value = newValue;
});

const formRules = {
    value: {
        required: true,
        custom: (field: string, rule: any, value: any) => {
            console.log(field, value);
            const exists = properties.value.includes(value);
            if (exists) {
                return (t.value('Components.ValueAlreadyExist'));
            }
            return true;
        }
    }
}

const { formData, formRef, handleSubmit } = useForm(addProperty)
const { rules } = useFormRules(formRules, formRef)
function addProperty() {
    properties.value.push(formData.value.value);
    formData.value.value = '';
    formRef.value.clearValidate();
    formVisible.value = false;
}

function removeProperty(value: any) {
    properties.value = properties.value.filter(item => item !== value);
}
</script>

