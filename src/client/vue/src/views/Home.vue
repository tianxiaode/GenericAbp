<template>
    <div class="hero">
        {{ t('Home') }}
        <Tree :api="api" display-field="name"></Tree>
    </div>
</template>

<script setup lang="ts">
import Tree from '~/components/tree/Tree.vue';
import { useI18n, useRepository } from '../composables'
const { t } = useI18n();

const api = useRepository('menu');



</script>