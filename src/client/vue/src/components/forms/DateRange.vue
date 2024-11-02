<template>
    <el-date-picker v-bind="$attrs" v-model="dateValue" size="large" clearable class="col-span-2" 
        :type="format === 'date' ? 'daterange' : 'datetimerange'"
    />
</template>

<script setup lang="ts">
import { ref, watch } from 'vue';
import { formatDate, isDate } from '~/libs';

const dateValue = ref<any>();
const startDate= defineModel('startDate');
const endDate= defineModel('endDate');
const props =defineProps({
    format:{
        type: String,
        default: 'date'
    },    
});

watch(dateValue, (newVal) => {
    if(!newVal){
        startDate.value = null;
        endDate.value = null;
        return;
    }
    const formatString = props.format === 'date' ? 'yyyy-MM-dd' : 'yyyy-MM-dd hh:mm:ss'
    startDate.value = isDate(newVal[0]) ? formatDate(newVal[0], formatString) : null;
    endDate.value = isDate(newVal[1]) ? formatDate(newVal[1], formatString) : null;    
})
</script>