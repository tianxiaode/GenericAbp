<template>
    <el-descriptions v-bind="$attrs" :column="1" border class="h-full overflow-auto value-list-input" size="small">
        <el-descriptions-item class-name="w-1/2">
            <template #label>
                <div class="w-full">
                    <el-input v-model="value" size="small" ref="valueInput" @blur="handleValueChange" clearable
                        :placeholder="t('Components.PropertyInput.Name')">
                        <template #append  v-if="errorMessage">
                            <el-tooltip placement="bottom" effect="light">
                                <template #content>
                                    <span class="danger">{{ errorMessage }}</span>
                                </template>
                                <i class="fa fa-circle-exclamation danger"></i>
                            </el-tooltip>
                        </template>
                    </el-input>
                </div>
            </template>
            <i class="fa fa-plus success cursor-pointer" @click="addProperty"></i>
        </el-descriptions-item>

        <el-descriptions-item v-for="value in properties.sort( (a:any, b:any) => a.localeCompare(b) )" size="small" :label="value">
                <i class="fa fa-trash danger cursor-pointer" @click="removeProperty(value)"></i>
        </el-descriptions-item>
        <el-descriptions-item v-if="noData" size="small" :label="t('Components.NoData')">
        </el-descriptions-item>
    </el-descriptions>

</template>

<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from 'vue';
import {useI18n } from '~/composables';
import { capitalize, isEmpty, Validator } from '~/libs';

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
const value = ref('');
const valueInput = ref<any>();
const noData = ref(properties.value.length === 0);
const errorMessage = ref('');
watch(properties, (value) => {
    noData.value = properties.value.length === 0;
    emit('update:modelValue', value);
});

watch(() => props.modelValue, (newValue) => {
    properties.value = newValue;
});

const handleValueChange = () => {
    if (isEmpty(value.value)) {
        errorMessage.value = t.value('Validation.Required');
        return;
    }
    if (properties.value.includes(value.value)) {
        errorMessage.value = t.value('Components.ValueAlreadyExist');
        return;
    }
    if(props.rules){
        for (const key in props.rules) {  
            console.log('handleValueChange', key, props.rules[key], value.value)          
            const isValid = Validator.validate(key, value.value, props.rules[key], null);
            if(!isValid){
                errorMessage.value = t.value('Validation.' + capitalize(key));
                return;
            }
        }
    }
    errorMessage.value = '';
}


function addProperty() {
    handleValueChange();
    if (!isEmpty(errorMessage.value)) {
        valueInput.value.focus();
        return;
    }

    properties.value = [...properties.value, value.value];
    value.value = '';
}

function removeProperty(value: any) {
    properties.value = [...properties.value.filter(item => item !== value)];
}
</script>

<style lang="scss">
.value-list-input {
    .el-descriptions__content{
        width: 20px;
    }
    .el-input-group__append {
        padding: 0 10px;
    }
}
</style>
