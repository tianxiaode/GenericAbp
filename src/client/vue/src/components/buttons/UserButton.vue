<template>
    <el-button v-if="!isAuthenticated" link  router-link="/login" >{{ t('Login') }}</el-button>
    <el-popover v-if="isAuthenticated" placement="bottom-end" trigger="hover" width="300">
        <template #reference>
            <el-avatar :src="avatar" />
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
                    <el-button plain  @click="logout">注销</el-button>
                </div>
                
        </template>
    </el-popover>
</template>



<script setup lang="ts">
import { useAuthentication,useI18n } from "../../composables";
import avatar from "../../assets/avatar.png";
import { appConfig } from "../../libs";

const { isAuthenticated,currentUser } = useAuthentication();
const { t } = useI18n();

const logout = () => {
    // appConfig.logout();
};
</script>

