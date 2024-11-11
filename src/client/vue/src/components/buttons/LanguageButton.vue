<template>
    <el-dropdown :trigger="hasHover ? 'hover' : 'click'" role="navigation" size="large" v-if="!isMobile">
        <span class="text-white font-size-4.5">{{ currentLanguage?.displayName }}</span>
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
import { useBrowseEnv, useI18n, useMultilingual } from '~/composables';

const { hasHover, isMobile } = useBrowseEnv();
const { t } = useI18n();

const { languages, currentLanguage, changeLanguage } = useMultilingual();




</script>

<style lang="scss" scoped>
.is-active {
    color: var(--el-color-primary);
}
</style>