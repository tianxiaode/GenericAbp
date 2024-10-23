<template>
    <div class="property-input">
        <div class="property-row header">
            <div class="cell">属性名称</div>
            <div class="cell">属性值</div>
            <div class="cell text-center">操作</div>
        </div>

        <el-form ref="formRef" :model="formData" :rules="rules">
            <div class="property-row">
                <div class="cell">
                    <el-form-item prop="name">
                        <el-input v-model="formData.name" />
                    </el-form-item>
                </div>
                <div class="cell">
                    <el-form-item prop="value">
                        <el-input v-model="formData.value" />
                    </el-form-item>
                </div>
                <div class="cell text-center">
                    <IconButton @click="addProperty" icon="fa fa-check primary" circle></IconButton>
                </div>
            </div>
        </el-form>

        <div class="property-row" v-for="(item, index) in properties" :key="index">
            <div class="cell">{{ item.name }}</div>
            <div class="cell">{{ item.value }}</div>
            <div class="cell text-center">
                <IconButton @click="removeProperty(item.name)" icon="fa fa-times danger" circle></IconButton>
            </div>
        </div>

    </div>
</template>

<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from 'vue';
import IconButton from '../buttons/IconButton.vue';
import { useForm, useFormRules } from '~/composables';

const props = defineProps({
    modelValue: {
        type: Object,
        default: () => ({})
    }
});

const emit = defineEmits(['update:modelValue']);

const properties = ref<{ name: string; value: string }[]>(Object.entries(props.modelValue).map(([key, value]) => ({ name: key, value })));

// 监听 properties 的变化，及时更新 modelValue
watch(properties, (newValue) => {
    const updatedObject = Object.fromEntries(newValue.map(item => [item.name, item.value]));
    emit('update:modelValue', updatedObject);
});

const formRules = {
    name: {
        required: true, 
        custom: (field:string, rule:any, value: any, callback: (error?: string) => void) => {
            console.log(field, rule, value, callback);
            const exists = properties.value.some(item => item.name === value);
            if (exists) {
                callback('此属性已存在，请使用不同的名称');
                return false
            }
            return true;
        }
    }
}

const { formData, formRef } = useForm(addProperty)
const { rules } = useFormRules(formRules, formRef)
function addProperty() {
    properties.value.push({ name: formData.value.name, value: formData.value.value });
    formData.value = { name: '', value: '' };
    formRef.value.clearValidate();
}

function removeProperty(key: string) {
    properties.value = properties.value.filter(item => item.name !== key);
}
</script>

<style scoped>
.property-input {
    display: flex;
    flex-direction: column;
    width: 100%;
}

.property-row {
    display: flex;
    align-items: center;
    margin-bottom: 10px;
}

.cell {
    flex: 1;
    padding: 5px;
}

.header {
    font-weight: bold;
    border-bottom: 1px solid #ccc;
}

input {
    width: calc(100% - 10px);
    box-sizing: border-box;
}

.error-message {
    color: red;
    font-size: 12px;
}
</style>
