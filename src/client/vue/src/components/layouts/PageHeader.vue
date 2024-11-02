<template>
    <div class="w-full bg-primary h-15 page-header box-border" v-if="!isMobile">
        <div class="container mx-auto flex justify-between items-center h-full gap-4 px-2 box-border">
            <a href="/"><img :src="logo" alt="logo" class="h-12.5"></a>
            <div class="flex-1"></div>
            <el-link href="/">{{ t('nav.home') }}</el-link>
            <el-link href="/demo">{{ t('nav.demos') }}</el-link>
            <el-link href="/docs">{{ t('nav.docs') }}</el-link>
            <el-link href="/about">{{ t('nav.about') }}</el-link>
            <div class="flex-1"></div>
            <UserButton />
            <a href="https://github.com/tianxiaode/GenericAbp" target="_blank">
                <i class="fab fa-github text-white font-size-7">
                </i>
            </a>
            <LanguageButton />
            <ThemeSwitch />
        </div>
    </div>
    <div v-else class="w-full bg-primary h-15 box-border px-2 gap-4 flex justify-between items-center">
        <a href="/"><img :src="logo" alt="logo" class="h-12.5"></a>
        <div class=flex-1></div>
        <UserButton />
        <ThemeSwitch />
        <MobileNavButton />
    </div>
</template>
<script setup lang="ts">
import { useBrowseEnv, useI18n } from '~/composables'
import logo from "~/assets/logo.png";
import LanguageButton from '../buttons/LanguageButton.vue';
import UserButton from '../buttons/UserButton.vue';
import MobileNavButton from '../buttons/MobileNavButton.vue';
import ThemeSwitch from '../buttons/ThemeSwitch.vue';

const { t } = useI18n();
const { isMobile } = useBrowseEnv();
</script>


<style lang="scss">
.page-header {

    .el-link {
        color: var(--el-color-white);
        font-size: 18px;
    }

    .el-link:hover {
        text-decoration: underline;
        text-underline-offset: 4px;
    }

}
</style>
