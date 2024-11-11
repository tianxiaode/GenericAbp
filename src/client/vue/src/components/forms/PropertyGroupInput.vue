<template>
    <el-collapse class="w-full" v-model="activeNames">
        <el-collapse-item v-for="group in groups" :name="group.name">
            <template #title>
                <el-text size="large" class="font-bold">{{ t(group.displayName || group.name) }}</el-text>
            </template>
            <PropertyInput :list="group.items" v-model="model[group.name]"></PropertyInput>
        </el-collapse-item>
    </el-collapse>
</template>

<script setup lang="ts">
import { onMounted, PropType, ref} from 'vue';
import { useI18n } from '~/composables';
import PropertyInput, { PropertyInputType } from './PropertyInput.vue';

const model = defineModel<any>({ default: {} });

export interface PropertyGroupType{
    name: string,
    displayName?: string,
    items: PropertyInputType[],
}

const props = defineProps({
    groups:{
        type: Array as PropType<PropertyGroupType[]>,
        default: () => []
    }
});

const { t } = useI18n();
const activeNames = ref([] as any[]);

onMounted(() => {
    activeNames.value = props.groups.map(group => group.name);
});

</script>

<style lang="scss" scoped>
</style>
