<template>
    <el-aside :class="asideStore.isExpanded ? 'collapsed' : ''" class="aside">
        <el-menu ref="menuRef" router :collapse="asideStore.isExpanded" :default-active="$route.path" size="large"
            style="--el-menu-text-color:var(--el-color-info);"
            class="aside-menu"
        >
            <MenuItem v-for="item in menus" :menu="item" :key="item.name"  v-model:menu-ref="menuRef" />
        </el-menu>
    </el-aside>

</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import {  useRepository } from '~/composables';
import { MenuType } from '~/repositories';
import { useAsideStore } from '~/store';
import MenuItem from '../menus/MenuItem.vue';
const asideStore = useAsideStore();
const api = useRepository('menu');
const menus = ref<MenuType[]>([]);
const menuRef = ref<any>();

onMounted(()=>{
    api.getShowList('default').then((res:any) => {
        menus.value = res.items;
    })
})
</script>

<style lang="scss" scoped>
.aside {
    width: 250px;
    transition: width 0.3s;
    /* 添加平滑过渡效果 */
}

.aside.collapsed {
    width: 60px;
}

.aside .aside-menu {
    min-height: calc(100vh - 60px);
}
</style>