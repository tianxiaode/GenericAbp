<template>
    <el-container>
        <el-header>
            <h3 class="m-0 p-0 font-size-8 leading-10">{{ t('AbpSettingManagement.Settings') }}</h3>
        </el-header>
        <el-container>
            <el-aside width="200px" v-if="settingManagement.canManageSettings">
                <el-menu :default-active="activeMenu" style="min-height:calc(100vh - 180px);" @select="handleSelect">
                    <el-menu-item index="identity" v-if="settingManagement.canManageIdentitySettings">
                        {{ t('AbpIdentity.Menu:IdentityManagement') }}
                    </el-menu-item>
                    <el-menu-item index="email" v-if="settingManagement.canManageEmailingSettings">
                        {{ t('AbpSettingManagement.Menu:Emailing') }}
                    </el-menu-item>
                    <el-menu-item index="timeZone" v-if="settingManagement.canManageTimeZone">
                        {{ t('AbpSettingManagement.Menu:TimeZone') }}
                    </el-menu-item>
                    <el-menu-item index="externalAuthentication" v-if="settingManagement.canManageExternalAuthentication">
                        {{ t('ExternalAuthentication.Menu:ExternalAuthenticationManagement') }}
                    </el-menu-item>
                </el-menu>
            </el-aside>
            <main class="px-4 w-full">
                <IdentitySettings v-if="activeMenu === 'identity'"></IdentitySettings>
                <EmailingSettings v-if="activeMenu === 'email'"></EmailingSettings>
                <TimeZoneSettings v-if="activeMenu === 'timeZone'"></TimeZoneSettings>
                <ExternalAuthenticationSettings v-if="activeMenu === 'externalAuthentication'"></ExternalAuthenticationSettings>
                <h3 class="text-center text-gray-500" v-if="!settingManagement.canManageSettings">{{
                    t('AbpSettingManagement.NoSettings') }}</h3>
            </main>
        </el-container>
    </el-container>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useI18n } from '~/composables';
import { settingManagement } from '~/libs';
import IdentitySettings from './IdentitySettings.vue';
import EmailingSettings from './EmailingSettings.vue';
import TimeZoneSettings from './TimeZoneSettings.vue';
import ExternalAuthenticationSettings from './ExternalAuthenticationSettings.vue';


const { t } = useI18n();
const activeMenu = ref('');
if (settingManagement.canManageIdentitySettings) {
    activeMenu.value = 'identity';
}
if (settingManagement.canManageEmailingSettings && !activeMenu.value) {
    activeMenu.value = 'email';
}
if (settingManagement.canManageTimeZone && !activeMenu.value) {
    activeMenu.value = 'timeZone';
}

if (settingManagement.canManageExternalAuthentication && !activeMenu.value) {
    activeMenu.value = 'externalAuthentication';
}

//activeMenu.value = 'externalAuthentication'

const handleSelect = (index: string) => {
    activeMenu.value = index;
};

</script>