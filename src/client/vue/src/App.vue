<template>
    <el-config-provider :locale="elementPlusLocale">
        <RouterView />
    </el-config-provider>
</template>

<script setup lang="ts">
import { useI18n } from './composables';

const { elementPlusLocale } = useI18n();
</script>

<style scoped></style>
