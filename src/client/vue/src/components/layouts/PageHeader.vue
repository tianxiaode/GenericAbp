<template>
    <div class="w-full bg-primary h-15 page-header box-border">
        <div class="container mx-auto flex justify-between items-center h-full gap-4 px-2 box-border">
            <a href="/"><img :src="logo" alt="logo" class="h-12.5"></a>
            <div class="flex-1 h-full justify-center items-center gap-4 hidden sm:ml-6 sm:flex">
                <el-link href="/">{{ t('nav.home') }}</el-link>
                <el-link href="/app">{{ t('nav.app') }}</el-link>
                <el-link href="/docs">{{ t('nav.docs') }}</el-link>
                <el-link href="/about">{{ t('nav.about') }}</el-link>
            </div>
            <div class="flex-none flex h-full justify-end items-center gap-4">
                <UserButton />
                <LanguageButton />
                <a href="https://github.com/tianxiaode/GenericAbp" target="_blank">
                    <i class="fab fa-github text-white "
                       :class="hasHover ? 'font-size-6' : 'font-size-6'"
                    >
                    </i>
                </a>
                <MobileNavButton />
            </div>
        </div>
    </div>
</template>
<script setup lang="ts">
import { useBrowseEnv, useI18n } from '~/composables'
import logo from "~/assets/logo.png";
import LanguageButton from '../buttons/LanguageButton.vue';
import UserButton from '../buttons/UserButton.vue';
import MobileNavButton from '../buttons/MobileNavButton.vue';

const { t } = useI18n();
const { hasHover} = useBrowseEnv();



</script>


<style lang="scss">
.page-header {

    .el-link {
        color: var(--el-color-white);
        font-size: 18px;
        min-width: 70px;
    }

    .el-link:hover {
        text-decoration: underline;
        text-underline-offset: 4px;
    }

}
</style>
