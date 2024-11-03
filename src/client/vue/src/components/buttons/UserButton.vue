<template>
    <i v-if="!isAuthenticated" @click="router.push('/login')" class="fa fa-user cursor-pointer text-white font-size-6" :title="t('Pages.Login.Login')"></i>
    <el-dropdown v-if="isAuthenticated" :trigger="hasHover ? 'hover' : 'click'" role="navigation" size="large" >
        <el-avatar :src="avatar" :size="isMobile ? 24: 28" />
        <template #dropdown>
            <div class="flex flex-row justify-between items-center gap-2 p-4">
                    <el-avatar :src="avatar" :size="48" class="p-2" />
                <div class="flex flex-col gap-1 flex-1">
                    <el-text class="w-full" size="small">{{ currentUser?.userName }}</el-text>
                    <el-text class="w-full" v-if="currentTenant" size="small" >{{ currentTenant?.name }}</el-text>
                    <el-text class="w-full" size="small">{{ currentUser?.email }}</el-text>
                </div>
            </div>
        <el-dropdown-menu style="width: 200px;">            
            <el-dropdown-item :class="{'is-active': path === '/profile'}" @click="router.push('/profile')">
                <i class="fa fa-user"></i>
                {{ t("Pages.Profile.Profile") }}
            </el-dropdown-item>
            <el-dropdown-item :class="{'is-active': path === '/dashboard'}" @click="router.push('/dashboard')">
                <i class="fa fa-computer"></i>
                {{ t("Pages.Profile.ManagementCenter") }}
            </el-dropdown-item>
            <el-dropdown-item @click="logout" >
                <i class="fa fa-sign-out-alt"></i>
                {{ t("Pages.Login.Logout") }}
            </el-dropdown-item>
        </el-dropdown-menu>
    </template>
    </el-dropdown>
</template>



<script setup lang="ts">
import { useAuthentication,useI18n,useBrowseEnv } from "../../composables";
import avatar from "../../assets/avatar.png";
import { account, appConfig } from "~/libs";
import router from "~/router";
import { ref } from "vue";
const path = ref(router.currentRoute.value.path);
const { isAuthenticated,currentUser } = useAuthentication();
const { t } = useI18n();
const { isMobile, hasHover } = useBrowseEnv()
const currentTenant = appConfig.currentTenant;
const logout = () => {
    account.logout();
};
</script>

