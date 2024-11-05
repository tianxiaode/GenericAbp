<template>
    <el-drawer v-bind="$attrs" direction="rtl" class="detail-drawer" size="50%"
        :title="`${t('Components.Detail')} - ${title}`">
        <PropertyList :data="data" :row-items="rowItems" />
    </el-drawer>

</template>

<script setup lang="ts">
import { PropType } from 'vue';
import PropertyList, { RowItemType } from './lists/PropertyList.vue';
import { useI18n } from '~/composables';


defineProps({
    title: {
        type: String,
        default: ''
    },
    data: {
        type: Object,
        default: {}
    },
    rowItems: {
        type: Array as PropType<RowItemType[]>,
        default: () => []
    }
})
const { t } = useI18n(); 
</script>

<style lang="scss">
.detail-drawer {
    .el-descriptions__label {
        width: var(--el-descriptions-label-width, 200px);
    }
}
</style>