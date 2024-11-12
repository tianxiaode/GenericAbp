<template>
    <BaseDialog v-bind="$attrs" ref="dialogRef">
        <PropertyInput v-if="!rowItems" :list="getRowItems" v-model="model" :class-name="className" :label-class-name="labelClassName"></PropertyInput>
        <PropertyGroupInput v-else :groups="getRowItems"  v-model="model"></PropertyGroupInput>
    </BaseDialog>
</template>

<script setup lang="ts">
import {  useMultilingual } from '~/composables';
import { PropType, computed } from 'vue';
import PropertyInput from '../forms/PropertyInput.vue';
import BaseDialog from './BaseDialog.vue';
import PropertyGroupInput from '../forms/PropertyGroupInput.vue';

const props = defineProps({
    className: {
        type: String,
        default: 'w-9/12'
    },
    labelClassName: {
        type: String,
        default: ''
    },
    rowItems: {
        type: Array as PropType<any>
    }
})


const { defaultRowItems } = useMultilingual();
const model = defineModel<any>();
const dialogRef = defineModel<any>('dialogRef');
const getRowItems = computed(() => {
    if (props.rowItems) {
        const rowItems = [] as any;
        defaultRowItems.value.forEach((item: any) => {
            const groupItem = { 
                name: item.field, 
                label: item.label, 
                className: props.className, 
                labelClassName: props.labelClassName, 
                items: [] as any };
            props.rowItems.forEach((rowItem: any) => {
                groupItem.items.push({ ...rowItem});
            })
            rowItems.push(groupItem);
        });
        return rowItems;
    }
    return defaultRowItems.value.map((item: any) => ({ ...item, type: 'input' }));
})

</script>
