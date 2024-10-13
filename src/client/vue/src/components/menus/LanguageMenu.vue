<template>
    <el-sub-menu v-bind="$attrs"  v-if="languages.length > 0">
        <template #title>{{ currentLanguage?.displayName }}</template>
        <el-menu-item v-for="item in languages" :key="item.cultureName" @click="changeLanguage(item.cultureName)">{{
            item.displayName }}
        </el-menu-item>
    </el-sub-menu>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useLocalizationStore } from '~/store';
import { useConfig } from '~/composables';
import { appConfig, LanguageType } from '~/libs';

const languages = ref<LanguageType[]>([]);
const currentLanguage = ref<LanguageType>();

const refresh = () => {
    console.log('refresh languages');
    languages.value = appConfig.getLanguages();
    currentLanguage.value = appConfig.currentCulture;
}

const { } = useConfig(refresh);

const localizationStore = useLocalizationStore();

const changeLanguage = (cultureName: string) => {
    localizationStore.setLocale(cultureName);
}



</script>

<style lang="scss" scoped>
</style>