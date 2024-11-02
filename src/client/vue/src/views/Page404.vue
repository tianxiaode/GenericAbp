<template>
    <div class="hero">
        <el-card>
            <template #header>
                <div class="text-center text-2xl font-bold">{{ t('Pages.404.Title') }}</div>
            </template>
            <p>{{ t('Pages.404.Description') }}</p>
        </el-card>
    </div>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';

const { t } = useI18n();

</script>