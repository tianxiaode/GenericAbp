<template>
    <div>
        <el-dropdown trigger="hover" size="large" role="navigation">
            <span class="el-dropdown-link text-white">{{ currentLanguage?.displayName }}</span>
            <template #dropdown>
                <el-dropdown-menu>
                    <el-dropdown-item v-for="item in languages" :key="item.cultureName"
                        @click="changeLanguage(item.cultureName)">{{ item.displayName }}</el-dropdown-item>
                </el-dropdown-menu>
            </template>
        </el-dropdown>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useLocalizationStore } from '../../store';
import { useConfigStoreWatcher } from '../..//composables/useConfigStoreWatcher';
import { globalConfig, LanguageType } from '../../libs';

const languages = ref<LanguageType[]>([]);
const currentLanguage = ref<LanguageType>();

const refresh = () => {
    languages.value = globalConfig.getLanguages();
    currentLanguage.value = globalConfig.currentCulture;
    console.log('refresh', languages.value, currentLanguage.value);
}

const {} = useConfigStoreWatcher(refresh);

const localizationStore = useLocalizationStore();

const changeLanguage = (cultureName: string) => {
    localizationStore.setLocale(cultureName);
}



</script>