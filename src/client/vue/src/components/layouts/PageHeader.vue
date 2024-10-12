<template>
    <el-menu v-if="!isPhone" mode="horizontal" class="px-10 bg-primary" :ellipsis="false" style="
        --el-menu-bg-color:transparent;
        --el-menu-text-color:var(--el-color-white);
        --el-menu-active-color:var(--el-color-primary)">
        <el-menu-item index="1">
            <PageLogo />
        </el-menu-item>
        <el-menu-item index="2">
            Home
        </el-menu-item>
        <el-menu-item index="3">
            APP
        </el-menu-item>
        <el-menu-item index="4">
            Docs
        </el-menu-item>
        <el-menu-item index="5">
            GitHub
        </el-menu-item>
        <el-menu-item index="6">
            About
        </el-menu-item>
        <LanguageMenu index="2" style="
                --el-menu-bg-color:var(--el-color-primary-light-9);
            --el-menu-text-color:var(--el-color-white);
            --el-menu-active-color:var(--el-color-white);" />
        <el-menu-item index="8">
            <UserButton />
        </el-menu-item>
    </el-menu>
    <el-menu v-if="isPhone" mode="horizontal" class=" bg-primary" :ellipsis="false" @select="phoneMenuSelect">
        <el-menu-item index="1">
            <PageLogo />
        </el-menu-item>
        <el-menu-item index="2">
            <i class="fa fa-bars" style="color: white;"></i>
        </el-menu-item>
    </el-menu>
    <el-drawer v-if="isPhone" size="70%" v-model="showDrawer" :with-header="false" :append-to-body="true">
        <el-menu mode="vertical" :collapse="false" @select="phoneMenuSelect" style="padding-right: 20%;">
            <el-menu-item index="2">
                Home
            </el-menu-item>
            <el-menu-item index="3">
                APP
            </el-menu-item>
            <el-menu-item index="4">
                Docs
            </el-menu-item>
            <el-menu-item index="5">
                GitHub
            </el-menu-item>
            <el-menu-item index="6">
                About
            </el-menu-item>
            <LanguageMenu index="7" />
            <el-menu-item index="8">
                <UserButton />
            </el-menu-item>
        </el-menu>
    </el-drawer>
</template>
<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import UserButton from '../buttons/UserButton.vue';
import LanguageMenu from '../menus/LanguageMenu.vue';
import PageLogo from './PageLogo.vue';

const isPhone = ref(false);
const showDrawer = ref(false);
const handleResize = () => {
    isPhone.value = window.screen.width < 768;
}

const phoneMenuSelect = (index: number) => {
    if (index == 2) {
        showDrawer.value = !showDrawer.value;
        console.log(showDrawer.value);
    }
}

onMounted(() => {
    handleResize();
    window.addEventListener('resize', handleResize);
});

onUnmounted(() => {
    window.removeEventListener('resize', handleResize);
});

</script>
<style lang="scss" scoped>
.el-menu--horizontal>.el-menu-item:nth-child(1) {
    margin-right: auto;
}

.el-menu--horizontal>.el-menu-item:nth-child(1):hover {
    background-color: transparent;
}

.el-menu--horizontal.el-menu {
    border-bottom: none;
}
</style>