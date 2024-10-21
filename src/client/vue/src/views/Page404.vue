<template>
    <div class="hero">
        <el-card :title="t('Pages.404')">
            <template #content>
                <p>{{ t('Pages.404.Description') }}</p>
            </template>
        </el-card>
    </div>
</template>

<script setup lang="ts">
import { useI18n } from '~/composables';

const { t } = useI18n();

</script>