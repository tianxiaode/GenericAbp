<template>
    <el-dropdown :trigger="hasHover ? 'hover' : 'click'" role="navigation" size="large" v-if="!isMobile">
        <i class="fa fa-globe text-white font-size-6"></i>
        <template #dropdown>
            <el-dropdown-menu>
                <el-dropdown-item v-for="item in languages" :key="item.cultureName"
                    :class="{ 'is-active': item.cultureName === currentLanguage?.cultureName }"
                    @click="changeLanguage(item.cultureName)">{{ item.displayName }}
                </el-dropdown-item>
            </el-dropdown-menu>
        </template>
    </el-dropdown>
    <div v-if="isMobile" class="flex flex-col items-start gap-4">
        <el-text tag="b" class="leading-6 border-b-1 border-gray-300">{{  t('Components.Languages') }}</el-text>
        <div v-for="item in languages" @click="changeLanguage(item.cultureName)"
            :class="{ 'is-active': item.cultureName === currentLanguage?.cultureName }"
        >
            {{ item.displayName }}
        </div>
    </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useLocalizationStore } from '~/store';
import { useBrowseEnv, useConfig, useI18n } from '~/composables';
import { appConfig, LanguageType } from '~/libs';

const { hasHover, isMobile } = useBrowseEnv();
const { t } = useI18n();
const languages = ref<LanguageType[]>([]);
const currentLanguage = ref<LanguageType>();
const refresh = () => {
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
.is-active {
    color: var(--el-color-primary);
}
</style>