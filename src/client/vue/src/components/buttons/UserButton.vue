<template>
    <el-link v-if="!isAuthenticated && !isMobile" href="/login" >{{  t("Pages.Login.Login")  }}</el-link>
    <a href="/login" v-if="!isAuthenticated && isMobile">
        <i class="fa fa-circle-user text-white font-size-6"></i>
    </a>
    <el-popover v-if="isAuthenticated" placement="bottom-end" :trigger="hasHover ? 'hover' : 'click'" width="300">
        <template #reference>
            <el-avatar :src="avatar" :size="isMobile ? 24: 32" />
        </template>
        <template #default>
                <el-descriptions :column="1" size="large" border>
                    <el-descriptions-item label="用户名">{{ currentUser?.userName }}</el-descriptions-item>
                    <el-descriptions-item label="姓名">{{ currentUser?.surname }}</el-descriptions-item>
                    <el-descriptions-item label="电子邮箱">{{ currentUser?.email }}</el-descriptions-item>
                    <el-descriptions-item label="手机号">{{ currentUser?.phone }}</el-descriptions-item>
                    <el-descriptions-item label="角色">{{ currentUser?.roles }}</el-descriptions-item>
                </el-descriptions>
                <div class="mt-2 text-center">
                    <el-button plain  @click="router.push('/profile')">{{ t("Pages.Profile.Profile") }}</el-button>
                    <el-button plain  @click="logout">{{ t("Pages.Login.Logout") }}</el-button>
                </div>
                
        </template>
    </el-popover>
</template>



<script setup lang="ts">
import { useAuthentication,useI18n,useBrowseEnv } from "../../composables";
import avatar from "../../assets/avatar.png";
import { account } from "~/libs";
import router from "~/router";

const { isAuthenticated,currentUser } = useAuthentication();
const { t } = useI18n();

const { isMobile, hasHover } = useBrowseEnv()

const logout = () => {
    account.logout();
};
</script>

