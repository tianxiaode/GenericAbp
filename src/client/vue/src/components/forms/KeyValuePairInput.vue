<template>
    <el-descriptions v-bind="$attrs" :column="1" border class="h-full overflow-auto key-value-pair-input" size="small">
        <el-descriptions-item class-name="w-1/2">
            <template #label>
                <div class="w-full">
                    <el-input v-model="name" size="small" ref="nameInput" @blur="handleNameChange" clearable
                        :placeholder="t('Components.PropertyInput.Name')">
                        <template #append  v-if="errorMessage">
                            <el-tooltip placement="bottom" effect="light">
                                <template #content>
                                    <span class="danger" >{{ t(errorMessage) }}</span>
                                </template>
                                <i class="fa fa-circle-exclamation text-danger"></i>
                            </el-tooltip>
                        </template>
                    </el-input>
                </div>
            </template>
            <div>
                <el-input v-model="value" size="small" clearable :placeholder="t('Components.PropertyInput.Value')" @keyup.enter.native="addProperty">
                    <template #append>
                        <i class="fa fa-plus text-success cursor-pointer" @click="addProperty"></i>
                    </template>
                </el-input>
            </div>
        </el-descriptions-item>

        <el-descriptions-item v-for="key in Object.keys(model).sort((a:any, b:any) => a.localeCompare(b))" class-name="w-1/2" size="small">
            <template #label><span class="cursor-pointer" @click="handleEdit(key)" >{{ key }}</span></template>
            <div class="w-full flex justify-between items-center">
                <span class="flex-1 cursor-pointer" @click="handleEdit(key)" >{{ model[key] }}</span>
                <i class="fa fa-trash text-danger cursor-pointer" @click="removeProperty(key)"></i>
            </div>
        </el-descriptions-item>
        <el-descriptions-item v-if="Object.keys(model).length === 0" size="small" :label="t('Components.NoData')">
        </el-descriptions-item>
    </el-descriptions>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useI18n } from '~/composables';
import { isEmpty } from '~/libs';

const model = defineModel<any>();


const { t } = useI18n();
const name = ref('');
const nameInput = ref<any>();
const value = ref('');
const errorMessage = ref('');


const handleNameChange = () => {
    if (isEmpty(name.value)) {
        errorMessage.value = 'Validation.Required';
        return;
    }
    if (model.value.hasOwnProperty(name.value)) {
        errorMessage.value = 'Components.PropertyInput.AlreadyExists';
        return;
    }
    errorMessage.value = '';
}

const handleEdit = (key: any) => {
    name.value = key;
    value.value = model.value[key];
    nameInput.value.focus();
}

function addProperty() {
    handleNameChange();
    if (!isEmpty(errorMessage.value)) {
        nameInput.value.focus();
        return;
    }
    model.value[name.value] = value.value;
    name.value = '';
    value.value = '';
}

function removeProperty(key: any) {
    delete model.value[key];
}
</script>

<style lang="scss">
.key-value-pair-input {
    .el-input-group__append {
        padding: 0 10px;
    }
}
</style>