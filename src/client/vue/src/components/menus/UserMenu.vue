<template>
    <el-sub-menu v-bind="$attrs" v-if="isAuthenticated">
        <template #title>   
            <el-avatar :src="avatar" />
        </template>
        <el-menu-item >{{ currentUser?.userName  }}</el-menu-item>
        <el-menu-item >{{ currentUser?.email  }}</el-menu-item>
    </el-sub-menu>
    <el-menu-item v-if="!isAuthenticated" index="/login">{{ t('Login') }}</el-menu-item>
</template>

<script setup lang="ts">
import { useAuthentication,useI18n } from "~/composables";
import avatar from "~/assets/avatar.png";

const { isAuthenticated,currentUser } = useAuthentication();
const { t } = useI18n();

</script>

<style scoped>
</style>