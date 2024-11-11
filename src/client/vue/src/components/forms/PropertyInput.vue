<template>
    <el-collapse class="w-full" v-model="activeNames">
        <el-collapse-item v-for="(list, group) in groups" :name="group">
            <template #title>
                <el-text size="large" class="font-bold">{{ t(getGroupName(group)) }}</el-text>
            </template>
            <div class="w-full">
                <el-descriptions :column="1" border v-if="list.length > 0" class="w-full">
                    <el-descriptions-item v-for="(property, index) in list" :key="index" class-name="w-1/2">
                        <template #label>
                            <div>{{ t(property.label) }}</div>
                        </template>
                        <component v-if="group === 'default-'" :is="comMap[property.type || 'input']" v-bind="property.props"
                            v-model="model[property.name]" clearable>
                        </component>
                        <component v-else :is="comMap[property.type || 'input']" v-model="model[getGroupName(group)][property.name]" clearable v-bind="property.props">
                        </component>
                    </el-descriptions-item>
                </el-descriptions>
            </div>
        </el-collapse-item>
    </el-collapse>
</template>

<script setup lang="ts">
import { onMounted, PropType, reactive, ref, markRaw } from 'vue';
import { useI18n } from '~/composables';
import { groupBy } from '~/libs';
import { ElInput, ElSelect } from 'element-plus';

const model = defineModel<any>({ default: {} });
export interface PropertyInputType {
    label: string,
    name: string,
    group?: string,
    type?: string,
    props?: Object,
    value?: any,
};

interface GroupType {
    [name: string]: PropertyInputType[]
}

const props = defineProps({
    list: {
        type: Array as PropType<PropertyInputType[]>,
        required: true,
    },
});

const { t } = useI18n();
const groups = ref<GroupType>({} as any);
const activeNames = ref([] as any[]);
const comMap = reactive<any>({
    input: markRaw(ElInput),
    select: markRaw(ElSelect)
});

const getGroupName = (name: string | number ) => {
    return name.toString().replace('default-', '') || 'basic';
}

onMounted(() => {
    //将list按组重组为{ name: value }的对象
    groups.value = groupBy(props.list, 'group') as any;
    activeNames.value = Object.keys(groups.value);
});
</script>

<style lang="scss" scoped></style>
