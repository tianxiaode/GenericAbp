<template>
        <el-dropdown :trigger="hasHover ? 'hover' : 'click'" role="navigation">
            <i v-if="!hasHover" class="el-dropdown-link text-white font-size-6 fa fa-globe" ></i>
            <span v-if="hasHover" class="el-dropdown-link text-white font-size-4.5">{{  currentLanguage?.displayName }}</span>
            <template #dropdown>
                <el-dropdown-menu>
                    <el-dropdown-item v-for="item in languages" :key="item.cultureName"
                        :class="{ 'is-active': item.cultureName === currentLanguage?.cultureName }"
                        @click="changeLanguage(item.cultureName)">{{ item.displayName }}
                    </el-dropdown-item>
                </el-dropdown-menu>
            </template>
        </el-dropdown>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useLocalizationStore } from '~/store';
import { useBrowseEnv, useConfig } from '~/composables';
import { appConfig, LanguageType } from '~/libs';

const {  hasHover} = useBrowseEnv();

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