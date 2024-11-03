<template>
    <el-select v-bind="$attrs" filterable remote reserve-keyword :remote-method="loadData" clearable :loading="loading"
        :value-key="valueField"
        :placeholder="t('Components.SelectPlaceholder',{ name: t(placeholder) })"        
    >
        <el-option v-for="item in options"  :value="valueField ? item[valueField] : item" :key="valueField ? item[valueField] : item"
            :label="displayField ? item[displayField] : item"
        >
            <span v-html="highlightText(displayField ? item[displayField] : item, filterText)"></span>
        </el-option>    
    </el-select>
</template>

<script setup lang="ts">
import { onMounted, ref, h } from 'vue';
import { useI18n } from '~/composables';
import { highlightText } from '~/libs';

const relationalModel = ref<any>(null);

const props = defineProps({
    method: {
        type: Function,
        default: () => ([])
    },
    displayField: {
        type: String
    },
    valueField: {
        type: String
    },
    placeholder: {
        type: String,
        default: ''
    },
    relationalField:{
        type: String
    }
})

const loading = ref(false);
const options = ref<any[]>([]);
const filterText = ref('');
const {t} = useI18n();

const loadData = (filter: any) => {
    filterText.value =  filter;
    loading.value = true;
    props.method(filter).then((res: any) => {
        options.value = res.items || res || [];
        loading.value = false;
    }).catch(() => {
        options.value = [];
        loading.value = false;
    })
}

onMounted(() => {
    loadData('');
})

</script>
