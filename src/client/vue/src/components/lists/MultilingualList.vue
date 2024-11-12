<template>
    <PropertyList v-if="!props.rowItems" :data="data" :row-items="defaultRowItems"
        :class-name="className" :label-class-name="labelClassName"
    >
</PropertyList>
    <div v-else>
        <div v-for="item in languages" :key="item.cultureName">
            <h2>{{item.displayName}}</h2>
            <PropertyList :data="data[item.cultureName]" :row-items="rowItems"
            :class-name="className" :label-class-name="labelClassName"
            ></PropertyList>            
        </div>
    </div>
</template>

<script setup lang="ts">
import { useMultilingual, usePropertyListProps } from '~/composables';
import PropertyList, {RowItemType} from './PropertyList.vue';
import { PropType} from 'vue';


const {languages, defaultRowItems} = useMultilingual();
const props = defineProps({
    data: {
        type: Object,
        default: {}
    },
    rowItems:{
        type: Array as PropType<RowItemType[]>
    },
    ...usePropertyListProps()
})



</script>