<template>

    <el-container>
        <el-header>
            <h3 class="m-0 p-0 font-size-8 leading-10">{{ t('AbpSettingManagement.Settings') }}</h3>
        </el-header>
        <el-container>
            <el-aside width="200px" v-if="hasPermission">
                <el-menu :default-active="activeMenu" style="min-height:calc(100vh - 180px);">
                    <el-menu-item index="identity" v-if="identityPermission">
                        {{ t('AbpIdentity.Menu:IdentityManagement') }}
                    </el-menu-item>
                    <el-menu-item index="email">
                        {{ t('AbpSettingManagement.Menu:Emailing') }}
                    </el-menu-item>
                    <el-menu-item index="timeZone">
                        {{ t('AbpSettingManagement.Menu:TimeZone') }}
                    </el-menu-item>
                </el-menu>
            </el-aside>
            <main class="p-2 w-full">
                <IdentitySetting v-if="activeMenu === 'identity'"></IdentitySetting>
                <h3 class="text-center text-gray-500" v-if="!hasPermission">{{ t('SettingManagement.NoSettings') }}</h3>
            </main>
        </el-container>
    </el-container>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useI18n } from '~/composables';
import { isGranted } from '~/libs';
import IdentitySetting from './IdentitySetting.vue';

const { t } = useI18n();
const activeMenu = ref('');
const identityPermission = isGranted('SettingManagement', 'PasswordPolicy') || isGranted('SettingManagement', 'LookupPolicy');
const emailPermission = isGranted('SettingManagement', 'Emailing') || isGranted('SettingManagement', 'Emailing.Test');
const timeZonePermission = isGranted('SettingManagement', 'TimeZone');
const hasPermission = identityPermission || emailPermission || timeZonePermission;
if (identityPermission) {
    activeMenu.value = 'identity';
}
if (emailPermission && !activeMenu.value) {
    activeMenu.value = 'email';
}
if (timeZonePermission && !activeMenu.value) {
    activeMenu.value = 'timeZone';
}

</script>