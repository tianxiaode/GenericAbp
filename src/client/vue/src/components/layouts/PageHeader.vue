<template>
    <div class="w-full bg-primary h-15 page-header" v-if="!isPhone">
        <div class="container mx-auto flex justify-between items-center h-full gap-4">
            <a href="/"><img :src="logo" alt="logo" class="h-12.5"></a>
            <div class="flex-1 h-full justify-center items-center gap-4 hidden sm:ml-6 sm:flex">
                    <el-link href="/">{{ t('nav.home') }}</el-link>
                    <el-link href="/app">{{ t('nav.app') }}</el-link>
                    <el-link href="/docs">{{ t('nav.docs') }}</el-link>
                    <el-link href="https://github.com/tianxiaode/GenericAbp" target="_blank">{{ t('nav.github') }}</el-link>
                    <el-link href="/about">{{ t('nav.about') }}</el-link>
            </div>
            <div class="flex-none flex h-full justify-end items-center gap-4">
                <LanguageButton />
                    <UserButton />
            </div>
        </div>
    </div>
    <el-menu v-if="isPhone" mode="horizontal" class=" bg-primary" :ellipsis="false" @select="phoneMenuSelect">
        <el-menu-item index="/">
            <PageLogo />
        </el-menu-item>
        <el-menu-item index="2">
            <i class="fa fa-bars" style="color: white;"></i>
        </el-menu-item>
    </el-menu>
    <el-drawer v-if="isPhone" size="70%" v-model="showDrawer" :with-header="false" :append-to-body="true">
        <el-menu mode="vertical" :collapse="false" @select="phoneMenuSelect" style="padding-right: 20%;">
            <el-menu-item index="/">
                Home
            </el-menu-item>
            <el-menu-item index="3">
                APP
            </el-menu-item>
            <el-menu-item index="/docs">
                Docs
            </el-menu-item>
            <el-menu-item index="https://github.com/tianxiaode/GenericAbp">
                GitHub
            </el-menu-item>
            <el-menu-item index="/about">
                About
            </el-menu-item>
            <LanguageMenu index="7" />
            <UserMenu index="8" />
        </el-menu>
    </el-drawer>
</template>
<script setup lang="ts">
import { ref, onMounted, onUnmounted } from 'vue';
import LanguageMenu from '../menus/LanguageMenu.vue';
import PageLogo from './PageLogo.vue';
import UserMenu from '../menus/UserMenu.vue';
import { useI18n } from '~/composables'
import logo from "~/assets/logo.png";
import LanguageButton from '../buttons/LanguageButton.vue';
import UserButton from '../buttons/UserButton.vue';

const { t } = useI18n();
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


<style lang="scss">
.page-header {

    .el-link {
        color: var(--el-color-white);
        font-size: 18px;
        min-width: 80px;
    }

    .el-link:hover {
        text-decoration: underline;
        text-underline-offset: 4px;
    }

}
</style>

