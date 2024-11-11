<template>
    <el-sub-menu v-if="menu.children && menu.children.length > 0" :index="getRouter(menu)" :collapse="true">
        <template #title>
            <el-icon><i :class="menu.icon"></i></el-icon>
            <span>{{ getDisplayName(menu) }}</span>
        </template>
        <MenuItem v-for="child in menu.children" :key="child.name" :menu="child" v-bind:menu-ref="menuRef"></MenuItem>
    </el-sub-menu>
    <el-menu-item v-else :index="getRouter(menu)">
        <el-icon><i :class="menu.icon"></i></el-icon>
        <span>{{ getDisplayName(menu) }}</span>
    </el-menu-item>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';
import { getId, isEmpty } from '~/libs';
import { MenuType } from '~/repositories';

const menuRef= defineModel<any>("menuRef")
defineProps({
    menu: {
        type: Object as () => MenuType,
        required: true
    }
})

const { locale } = useI18n();
const getDisplayName = (item: MenuType) => {
    return item.multilingual[locale.value] || item.name;
}

const getRouter = (item: MenuType) => {
    if (isEmpty(item.router)) {
        const id = getId();
        setTimeout(() => {
            menuRef.value?.open(id);
        }, 500);
        return id;
    }
    return item.router.startsWith('http') ? item.router : '/' + item.router;
}

</script>