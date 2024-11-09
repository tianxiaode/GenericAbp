<template>
    <component :is="formItemProp ? 'el-form-item' : 'div'"
        v-bind="formItemProp ? { label: t(label), prop: formItemProp } : null">
        <el-select v-bind="$attrs" filterable :filter-method="filterIcons"
            :placeholder="t('Components.SelectPlaceholder', { name: t(label) })" popper-class="icon-select-popper">
            <template #label="{ label, value }">
                <i :class="['h-4 w-4' ,`${value}`]"></i>
                {{ value }}
            </template>            
            <!-- 图标选项 -->
            <el-option v-for="icon in filteredIcons" :key="icon" :label="icon" :value="icon" class="icon-option">
                <i :class="['fa h-4 w-4', `fa-${icon}`]"></i>
                <span class="ml-2" v-html="highlightText(icon.replace('fa fa-', ''), search)"></span>
            </el-option>
        </el-select>
    </component>
</template>

<script setup lang="ts">


import { ref, computed, onMounted } from 'vue';
import { useI18n } from '~/composables';
import { highlightText } from '~/libs';
const iconFamilies = import('@fortawesome/fontawesome-free/metadata/icon-families.json');
const icons = ref<any[]>([]);
const search = ref(''); // 搜索关键字
const { t } = useI18n();
const filteredIcons = computed(() => {
    if (!search.value) return icons.value;
    return icons.value.filter((icon) =>
        icon.toLowerCase().includes(search.value.toLowerCase())
    );
});

defineProps({
    label: {
        type: String,
        default: null
    },
    formItemProp: {
        type: String,
        default: null
    }
});

const filterIcons = (query: any) => {
    search.value = query;
};

onMounted(() => {
    iconFamilies.then((data) => {
        const list = [];
        for (const key in data) {
            const icon = data[key] as any;
            if (icon.svgs?.classic?.solid) {
                list.push(`fa fa-${key}`);
            }
        }
        icons.value = list;
    })
})

</script>

<style lang="scss">
.icon-select-popper {

    .el-select-dropdown__wrap {
        position: relative;
        min-height: 300px;
        overflow: auto;

        ul {
            position: absolute;
            top: 0;
            left: 0;
            display: flex;
            flex-wrap: wrap;
            width: 100%;
            padding: 0;
            margin: 0;

            .icon-option {
                min-width: 0;
                flex: 0 0 25%;
                max-width: 25%;

            }
        }

    }
}
</style>