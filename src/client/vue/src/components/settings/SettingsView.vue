<template>
    <el-container>
        <el-header>
            <h3 class="m-0 p-0 font-size-8 leading-10">{{ t('AbpSettingManagement.Settings') }}</h3>
        </el-header>
        <el-aside width="200px">
            <el-menu default-active="/settings/identity" router>
                <el-menu-item index="/settings/identity" v-if="isGranted('AbpIdentity','Users','Create') || isGranted('AbpIdentity','Users','Update')">
                    {{ t('AbpIdentity.Menu:IdentityManagement') }}
                </el-menu-item>
                <el-menu-item index="2">

                </el-menu-item>
            </el-menu>
        </el-aside>
        <main><RouterView></RouterView></main>
    </el-container>
</template>

<script setup lang="ts">
import { onMounted, ref } from 'vue';
import { useI18n } from '~/composables';
import { isGranted } from '~/libs';
import router from '~/router';

const permissions = ['Emailing', 'Emailing.Test', 'TimeZone', 'PasswordPolicy', 'LookupPolicy']
const hasPermission = ref(permissions.some(p => isGranted('SettingManagement', p)));

const {t } = useI18n();

onMounted(() => {
    if (!hasPermission.value) {
        router.push('/dashboard');
    }
});
</script>