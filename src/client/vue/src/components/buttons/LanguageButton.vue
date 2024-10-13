<template>
        <el-dropdown :trigger="hasHover ? 'hover' : 'click'" role="navigation">
            <i class="fa fa-globe text-white el-dropdown-link"
                :class="isMobile ? 'font-size-6' : 'font-size-8'"
            ></i>
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

const { isMobile, hasHover} = useBrowseEnv();

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