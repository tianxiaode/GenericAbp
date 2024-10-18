<template>
    <el-link v-if="!isAuthenticated && !isMobile" href="/login" >{{  t("Pages.Login.Login")  }}</el-link>
    <a href="/login" v-if="!isAuthenticated && isMobile">
        <i class="fa fa-circle-user text-white font-size-6"></i>
    </a>
    <el-dropdown v-if="isAuthenticated" :trigger="hasHover ? 'hover' : 'click'" role="navigation" size="large" >
        <el-avatar :src="avatar" :size="isMobile ? 24: 32" />
        <template #dropdown>
            <div class="text-center pt-3 pb-2 text-lg">{{ currentUser?.userName }}</div>
        <el-dropdown-menu style="width: 200px;">
            <el-dropdown-item :class="{'is-active': path === '/profile'}" @click="router.push('/profile')">
                <i class="fa fa-user"></i>
                {{ t("Pages.Profile.Profile") }}
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
import { account } from "~/libs";
import router from "~/router";
import { ref } from "vue";
const path = ref(router.currentRoute.value.path);
const { isAuthenticated,currentUser } = useAuthentication();
const { t } = useI18n();
console.log(currentUser)
const { isMobile, hasHover } = useBrowseEnv()

const logout = () => {
    account.logout();
};
</script>

