<template>
    <Hero>
        <el-card :title="t('Pages.404')">
            <template #content>
                <p>{{ t('Pages.404.Description') }}</p>
            </template>
        </el-card>
    </Hero>
</template>

<script setup lang="ts">
import { Hero } from '~/components';
import { useI18n } from '~/composables';

const { t } = useI18n();

</script>