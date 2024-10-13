<template>
    <el-dropdown v-if="isMobile" trigger="click" role="navigation">
        <i class="fa fa-bars text-lg text-white "
            :class="isMobile ? 'font-size-6' : 'font-size-8'"
        ></i>
        <template #dropdown>
            <el-dropdown-menu>
                <el-dropdown-item :class="{'is-active': $route.path === '/app'}">
                    <el-link href="/app">{{ t('nav.app') }}</el-link>
                </el-dropdown-item>
                <el-dropdown-item :class="{'is-active': $route.path === '/docs'}">
                    <el-link href="/docs">{{ t('nav.docs') }}</el-link>
                </el-dropdown-item>
                <el-dropdown-item :class="{'is-active': $route.path === '/about'}">
                    <el-link href="/about">{{ t('nav.about') }}</el-link>
                </el-dropdown-item>
            </el-dropdown-menu>
        </template>
    </el-dropdown>
</template>

<script setup lang="ts">
import { useBrowseEnv, useI18n } from '~/composables';

const { isMobile} = useBrowseEnv();
const { t } = useI18n();



</script>