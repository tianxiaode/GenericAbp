<template>
    <el-descriptions :column="1" border v-if="list.length > 0" class="property-input w-full">
        <el-descriptions-item v-for="(property, index) in list" :key="index" :class-name="className"
            :label-class-name="labelClassName">
            <template #label>
                <div>{{ t(property.label) }}</div>
            </template>
            <component :is="comMap[property.type || 'input']" v-bind="property.props" v-model="model[property.field]"
                clearable>
            </component>
        </el-descriptions-item>
    </el-descriptions>
</template>

<script setup lang="ts">
import { PropType, reactive, markRaw } from 'vue';
import { useI18n } from '~/composables';
import { ElInput, ElSelect } from 'element-plus';

const model = defineModel<any>({ default: {} });
export interface PropertyInputType {
    label: string,
    field: string,
    group?: string,
    type?: string,
    props?: Object,
    value?: any,    
};


defineProps({
    className: {
        type: String,
    },
    labelClassName: {
        type: String,
    },
    list: {
        type: Array as PropType<PropertyInputType[]>,
        required: true,
    }
});

const { t } = useI18n();
const comMap = reactive<any>({
    input: markRaw(ElInput),
    select: markRaw(ElSelect)
});

</script>
