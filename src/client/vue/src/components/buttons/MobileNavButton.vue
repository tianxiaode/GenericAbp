<template>
    <i class="fa fa-bars text-lg text-white font-size-6" @click="showDrawer = true">
    </i>
    <el-drawer v-if="showDrawer" v-model="showDrawer" size="50%">
        <div class="flex flex-col items-start gap-4">
            <el-link :underline="false" href="/demos">{{ t('nav.demos') }}</el-link>
            <el-link :underline="false" href="/docs">{{ t('nav.docs') }}</el-link>
            <el-link :underline="false" href="/about">{{ t('nav.about') }}</el-link>
            <div class="leading-4 border-1 border-solid border-gray-300 w-full"></div>
            <LanguageButton />
        </div>
    </el-drawer>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useI18n } from '~/composables';
import LanguageButton from './LanguageButton.vue';
const showDrawer  = ref(false);
const { t } = useI18n();



</script>