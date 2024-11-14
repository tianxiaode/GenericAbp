<template>
    <el-input v-bind="$attrs" :placeholder="t('Components.Search')" prefix-icon="el-icon-search" 
        v-model="filter"
        @input="filterInput"
        clearable>
        <template #prefix>                    
            <el-icon><i class="fa fa-search"></i></el-icon>
        </template>
        <template #suffix>
            <el-icon v-if="showEllipsisIcon" @click="ellipsisIconClick" class="cursor-pointer">
                <i class="fa fa-ellipsis-v"></i>
            </el-icon>
        </template>
    </el-input>

</template>

<script lang="ts" setup>
import { ref } from 'vue';
import { useI18n } from '~/composables';

const filter = ref('');
const props = defineProps({
    showEllipsisIcon: {
        type: Boolean,
        default: false  
    },
    ellipsisIconClick:{
        type: Function,
        default: () => {}
    },
    filter: {
        type: Function,
        default: () => {}
    }
});

const filterInput = (value: string) => {
    filter.value = value;
    props.filter(value);
}
const {t} = useI18n();
</script>