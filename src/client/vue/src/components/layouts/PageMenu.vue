<template>
    <el-menu :mode="menuMode" class="bg-primary border-0" :ellipsis="false" style="
            --el-menu-bg-color:transparent;
            --el-menu-text-color:var(--el-color-white);
            --el-menu-active-color:var(--el-color-primary)">
        <el-menu-item index="3">
            Home
        </el-menu-item>
        <el-menu-item index="3">
            APP
        </el-menu-item>
        <el-menu-item index="3">
            Docs
        </el-menu-item>
        <el-menu-item index="3">
            GitHub
        </el-menu-item>
        <el-menu-item index="3">
            About
        </el-menu-item>
        <LanguageMenu index="2" style="
                --el-menu-bg-color:var(--el-color-primary-light-9);
            --el-menu-text-color:var(--el-color-white);
            --el-menu-active-color:var(--el-color-white);" />
        <el-menu-item index="1">
            <UserButton />
        </el-menu-item>
    </el-menu>

</template>

<script setup lang="ts">
import { onMounted, onUnmounted, ref } from 'vue';
import UserButton from '../buttons/UserButton.vue';
import LanguageMenu from '../menus/LanguageMenu.vue';

const menuMode = ref('horizontal');
const handleResize = () => {
    menuMode.value = window.screen.width < 768 ? 'vertical' : 'horizontal';
    console.log(window.screen.width, menuMode.value);
}

onMounted(() => {
    console.log('mounted');
    handleResize();
    window.addEventListener('resize', handleResize);
});

onUnmounted(() => {
    window.removeEventListener('resize', handleResize);
});

</script>
<style lang="scss" scoped>
.el-menu--horizontal.el-menu {
    border-bottom: none;
}
</style>