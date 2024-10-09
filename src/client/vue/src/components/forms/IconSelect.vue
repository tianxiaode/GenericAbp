<template>
    <el-select v-model="selectedIcon" placeholder="Select an icon" filterable class="icon-selector"
        :filter-method="filterIcons" :popper-class="'custom-select-dropdown'">
        <!-- 图标选项 -->
        <el-option v-for="icon in filteredIcons" :key="icon" :label="icon" :value="icon" class="icon-option">
            <i :class="['fa', `fa-${icon}`]" class="icon-preview"></i> {{ icon }}
        </el-option>
    </el-select>
</template>

<script setup lang="ts">


import { ref, computed } from 'vue';
const iconFamilies =  import('@fortawesome/fontawesome-free/metadata/icon-families.json');
const selectedIcon = ref(''); // 选中的图标
const icons = Object.keys(iconFamilies);
const search = ref(''); // 搜索关键字

const filteredIcons = computed(() => {
    if (!search.value) return icons;
    return icons.filter((icon) =>
        icon.toLowerCase().includes(search.value.toLowerCase())
    );
});

const filterIcons = (query:any) => {
    search.value = query;
};
</script>

<style>
.icon-selector {
    width: 300px;
}

.icon-option {
    display: inline-block;
    width: 10%;
    text-align: center;
}

.icon-preview {
    font-size: 20px;
    margin-right: 10px;
}

.custom-select-dropdown .el-select-dropdown__item {
    display: inline-block;
    width: 10%;
    /* 每行显示 10 个图标 */
    text-align: center;
}
</style>