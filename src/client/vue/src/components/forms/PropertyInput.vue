<template>
    <el-descriptions v-bind="$attrs" :column="1" border class="h-full overflow-auto property-input" size="small">
        <el-descriptions-item class-name="w-1/2">
            <template #label>
                <div class="w-full">
                    <el-input v-model="name" size="small" ref="nameInput" @blur="handleNameChange" clearable
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
            <div>
                <el-input v-model="value" size="small" clearable :placeholder="t('Components.PropertyInput.Value')">
                    <template #append>
                        <i class="fa fa-plus success cursor-pointer" @click="addProperty"></i>
                    </template>
                </el-input>
            </div>
        </el-descriptions-item>

        <el-descriptions-item v-for="key in Object.keys(properties).sort((a:any, b:any) => a.localeCompare(b))" class-name="w-1/2" size="small">
            <template #label><span>{{ key }}</span></template>
            <div class="w-full flex justify-between items-center">
                <span class="flex-1">{{ properties[key] }}</span>
                <i class="fa fa-trash danger cursor-pointer" @click="removeProperty(key)"></i>
            </div>
        </el-descriptions-item>
        <el-descriptions-item v-if="noData" size="small" :label="t('Components.NoData')">
        </el-descriptions-item>
    </el-descriptions>
</template>

<script setup lang="ts">
import { ref, watch, defineProps, defineEmits } from 'vue';
import { useI18n } from '~/composables';
import { isEmpty } from '~/libs';

const props = defineProps({
    modelValue: {
        type: Object,
        default: () => ({})
    }
});

const emit = defineEmits(['update:modelValue']);
const { t } = useI18n();
const name = ref('');
const nameInput = ref<any>();
const value = ref('');
const errorMessage = ref('');
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

const handleNameChange = () => {
    if (isEmpty(name.value)) {
        errorMessage.value = t.value('Validation.Required');
        return;
    }
    if (properties.value.hasOwnProperty(name.value)) {
        errorMessage.value = t.value('Components.PropertyInput.AlreadyExists');
        return;
    }
    errorMessage.value = '';
}

function addProperty() {
    handleNameChange();
    if (!isEmpty(errorMessage.value)) {
        nameInput.value.focus();
        return;
    }
    properties.value = {...properties.value, [name.value]: value.value };
    name.value = '';
    value.value = '';
}

function removeProperty(key: any) {
    const value = properties.value;
    delete value[key];
    properties.value = {...value };
}
</script>

<style lang="scss">
.property-input {
    .el-input-group__append {
        padding: 0 10px;
    }
}
</style>