<template>
    <el-descriptions v-bind="$attrs" :column="1" border class="h-full overflow-auto value-list-input" size="small">
        <el-descriptions-item class-name="w-1/2">
            <template #label>
                <div class="w-full">
                    <el-input v-model="inputValue" size="small" ref="valueInput" @blur="handleValueChange" clearable
                        :placeholder="t('Components.PropertyInput.Name')" @keyup.enter.native="addProperty">
                        <template #append  v-if="errorMessage">
                            <el-tooltip placement="bottom" effect="light">
                                <template #content>
                                    <span class="danger">{{ t(errorMessage) }}</span>
                                </template>
                                <i class="fa fa-circle-exclamation danger"></i>
                            </el-tooltip>
                        </template>
                    </el-input>
                </div>
            </template>
            <i class="fa fa-plus success cursor-pointer" @click="addProperty"></i>
        </el-descriptions-item>

        <el-descriptions-item v-for="value in (model as any[]).map((m:any) => props.convertModel(m)).sort( (a:any, b:any) => a.localeCompare(b) )" size="small">
            <template #label><span class="cursor-pointer" @click="handleEdit(value)">{{ value }}</span></template>
                <i class="fa fa-trash danger cursor-pointer" @click="removeProperty(value)"></i>
        </el-descriptions-item>
        <el-descriptions-item v-if="model.length === 0" size="small" :label="t('Components.NoData')">
        </el-descriptions-item>
    </el-descriptions>

</template>

<script setup lang="ts">
import { ref,  defineProps  } from 'vue';
import {useI18n } from '~/composables';
import { capitalize, isEmpty, Validator } from '~/libs';

const model = defineModel<any>();

//这里需要传入两个转换函数，一个是将输入值转换为模型值，一个是将模型值转换为输入值
const props = defineProps({
    rules: {
        type: Object,
        default: () => ({})
    },
    convertValue: {
        type: Function,
        default: (value: any) => value
    },
    convertModel: {
        type: Function,
        default: (value: any) => value
    }
});

const { t } = useI18n();
const inputValue = ref('');
const valueInput = ref<any>();
const errorMessage = ref('');



const handleEdit = (value: any) => {
    inputValue.value =value;
    valueInput.value.focus();
}

const handleValueChange = () => {
    if (isEmpty(inputValue.value)) {
        errorMessage.value ='Validation.Required';
        return;
    }
    if (model.value.includes(props.convertValue(inputValue.value))) {
        errorMessage.value = 'Components.ValueAlreadyExist';
        return;
    }
    if(props.rules){
        for (const key in props.rules) {  
            console.log('handleValueChange', key, props.rules[key], inputValue.value)          
            const isValid = Validator.validate(key, inputValue.value, props.rules[key], null);
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

    model.value.push(props.convertValue(inputValue.value));
    inputValue.value = '';
}

function removeProperty(value: any) {
    model.value = model.value.filter((m: any) => m !== props.convertValue(value));
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
