<template>
    <el-container>
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
import { useI18n } from '~/composables';
import { isGranted } from '~/libs';

const {t } = useI18n();
</script>