<template>
    <LoadingProgress />
    <el-config-provider :locale="elementPlusLocale" size="large">
        <RouterView />
    </el-config-provider>
</template>

<script setup lang="ts">
import LoadingProgress from './components/LoadingProgress.vue';
import { useI18n } from './composables';

const { elementPlusLocale } = useI18n();
</script>

