<template>
    <div v-bind="$attrs" class="h-full flex flex-col gap-2 box-border">
        <div class="flex-1 overflow-auto">
            <div class="flex flex-row w-full gap-2 py-1" v-for="key in Object.keys(properties)">
                <div class="flex-1">{{ key }}</div>
                <div class="flex-1">{{ properties[key] }}</div>
                <div class="w-10 text-center">
                    <IconButton @click="removeProperty(key)" icon="fa fa-trash danger" size="small" circle>
                    </IconButton>
                </div>
            </div>
            <div class="text-center text-lg font-bold" v-if="noData">{{ t('Components.NoData') }}</div>
        </div>
        <el-form ref="formRef" :model="formData" :rules="rules" :validate-on-rule-change="false">
            <div class="flex flex-row w-full gap-2" style="border-top: 1px solid #ccc; padding-top: 0.5rem;">
                <el-form-item prop="name" class="flex-1">
                    <el-input v-model="formData.name" size="small" :placeholder="t('Components.PropertyInput.Name')"
                        auto-complete="off" />
                </el-form-item>
                <el-form-item prop="value" class="flex-1">
                    <el-input v-model="formData.value" size="small"
                        :placeholder="t('Components.PropertyInput.Value')" />
                </el-form-item>
                <el-form-item>
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
        type: Object,
        default: () => ({})
    },
    label: {
        type: String,
        default: 'Components.PropertyInput.Label'
    }
});

const emit = defineEmits(['update:modelValue']);
const { t } = useI18n();
const properties = ref<Record<string, any>>(props.modelValue);
const noData = ref(Object.keys(properties.value).length === 0);
// 监听 properties 的变化，及时更新 modelValue
watch(properties, (newValue) => {
    noData.value = Object.keys(newValue).length === 0;
    emit('update:modelValue', newValue);
});

watch(() => props.modelValue, (newValue) => {
    properties.value = newValue;
});

const formRules = {
    name: {
        required: true,
        // eslint-disable-next-line no-unused-vars
        custom: (value: any) => {
            const exists = properties.value.hasOwnProperty(value);
            if (exists) {
                return (t.value('Components.PropertyInput.AlreadyExists'));
            }
            return true;
        }
    }
}


const { formData, formRef, handleSubmit } = useForm(addProperty)
const { rules } = useFormRules(formRules, formRef)
function addProperty() {
    properties.value[formData.value.name] = formData.value.value;
    formData.value = { name: '', value: '' };
    formRef.value.clearValidate();
}

function removeProperty(key: any) {
    delete properties.value[key];
}
</script>
