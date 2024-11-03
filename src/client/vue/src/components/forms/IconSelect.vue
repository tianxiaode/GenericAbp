<template>
    <el-select v-bind="$attrs" filterable 
        :filter-method="filterIcons">
        <!-- 图标选项 -->
        <el-option v-for="icon in filteredIcons" :key="icon" :label="icon" :value="icon" class="icon-option">
            <i :class="['fa h-4 w-4', `fa-${icon}`]" ></i> 
            <span class="ml-2" v-html="highlightText(icon, search)"></span>
        </el-option>
    </el-select>
</template>

<script setup lang="ts">


import { ref, computed, onMounted } from 'vue';
import { highlightText } from '~/libs';
const iconFamilies =  import('@fortawesome/fontawesome-free/metadata/icon-families.json');
const icons = ref<any[]>([]);
const search = ref(''); // 搜索关键字

const filteredIcons = computed(() => {
    if (!search.value) return icons.value;
    return icons.value.filter((icon) =>
        icon.toLowerCase().includes(search.value.toLowerCase())
    );
});


const filterIcons = (query:any) => {
    search.value = query;
};

onMounted(() => {
    iconFamilies.then((data) => {
        const list = [];
        for (const key in data) {
            const icon = data[key] as any;
            if(icon.svgs?.classic?.solid){
                list.push(key);
            }
        }
        icons.value = list;
    })
})

</script>

