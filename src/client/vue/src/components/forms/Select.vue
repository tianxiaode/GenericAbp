<template>
    <el-select v-bind="$attrs" filterable remote reserve-keyword :remote-method="loadData" clearable :loading="loading"
        :placeholder="t('Components.SelectPlaceholder',{ name: t(placeholder) })"
    >
        <el-option v-for="item in options"  :value="getNestedValue(item)" >
            <span v-html="getDisplayValue(item)"></span>
        </el-option>    
    </el-select>
</template>

<script setup lang="ts">
import { onMounted, ref, h } from 'vue';
import { useI18n } from '~/composables';
import { highlightText } from '~/libs';

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
})

const loading = ref(false);
const options = ref([]);
const filterText = ref('');
const {t} = useI18n();
const getDisplayValue = (item: any) => {
    const display = props.displayField? item[props.displayField] : item;
    return highlightText(display, filterText.value);
}

const getNestedValue = (item: any) => {
    return props.valueField ? item[props.valueField] : item;
}

const loadData = async (filter: any) => {
    filterText.value =  filter;
    await props.method(filter).then((res: any) => {
        options.value = res.items;
    }).catch(() => {
        options.value = [];
    })
}

onMounted(() => {
    loadData('');
})

</script>
