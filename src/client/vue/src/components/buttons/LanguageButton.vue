<template>
        <el-dropdown trigger="hover" size="large" role="navigation">
            <el-link class="el-dropdown-link">{{ currentLanguage?.displayName }}</el-link>
            <template #dropdown>
                <el-dropdown-menu>
                    <el-dropdown-item v-for="item in languages" :key="item.cultureName"
                        @click="changeLanguage(item.cultureName)">{{ item.displayName }}</el-dropdown-item>
                </el-dropdown-menu>
            </template>
        </el-dropdown>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useLocalizationStore } from '~/store';
import { useConfig } from '~/composables';
import { appConfig, LanguageType } from '~/libs';

const languages = ref<LanguageType[]>([]);
const currentLanguage = ref<LanguageType>();

const refresh = () => {
    languages.value = appConfig.getLanguages();
    currentLanguage.value = appConfig.currentCulture;
}

const {} = useConfig(refresh);

const localizationStore = useLocalizationStore();

const changeLanguage = (cultureName: string) => {
    localizationStore.setLocale(cultureName);
}



</script>