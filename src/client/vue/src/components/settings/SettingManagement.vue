<template>

    <el-container>
        <el-header>
            <h3 class="m-0 p-0 font-size-8 leading-10">{{ t('AbpSettingManagement.Settings') }}</h3>
        </el-header>
        <el-container>
            <el-aside width="200px" v-if="settingManagement.canManageSettings">
                <el-menu :default-active="activeMenu" style="min-height:calc(100vh - 180px);">
                    <el-menu-item index="identity" v-if="settingManagement.canManagerIdentitySettings">
                        {{ t('AbpIdentity.Menu:IdentityManagement') }}
                    </el-menu-item>
                    <el-menu-item index="email" v-if="settingManagement.canManageEmailingSettings">
                        {{ t('AbpSettingManagement.Menu:Emailing') }}
                    </el-menu-item>
                    <el-menu-item index="timeZone" v-if="settingManagement.canManageTimeZone">
                        {{ t('AbpSettingManagement.Menu:TimeZone') }}
                    </el-menu-item>
                </el-menu>
            </el-aside>
            <main class="p-2 w-full">
                <IdentitySetting v-if="activeMenu === 'identity'"></IdentitySetting>
                <h3 class="text-center text-gray-500" v-if="!settingManagement.canManageSettings">{{ t('SettingManagement.NoSettings') }}</h3>
            </main>
        </el-container>
    </el-container>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useI18n } from '~/composables';
import { settingManagement } from '~/libs';
import IdentitySetting from './IdentitySetting.vue';


const { t } = useI18n();
const activeMenu = ref('');
if (settingManagement.canManagerIdentitySettings) {
    activeMenu.value = 'identity';
}
if (settingManagement.canManageEmailingSettings && !activeMenu.value) {
    activeMenu.value = 'email';
}
if (settingManagement.canManageTimeZone && !activeMenu.value) {
    activeMenu.value = 'timeZone';
}

</script>