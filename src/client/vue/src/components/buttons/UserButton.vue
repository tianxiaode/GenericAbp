<template>
    <el-popover placement="bottom-end" trigger="hover" width="300">
        <template #reference>
            <el-avatar :src="avatar" />
        </template>
        <template #default>
                <el-descriptions :column="1" size="large" border>
                    <el-descriptions-item label="用户名">{{ user?.userName }}</el-descriptions-item>
                    <el-descriptions-item label="姓名">{{ user?.surname }}</el-descriptions-item>
                    <el-descriptions-item label="电子邮箱">{{ user?.email }}</el-descriptions-item>
                    <el-descriptions-item label="手机号">{{ user?.phone }}</el-descriptions-item>
                    <el-descriptions-item label="角色">{{ user?.roles }}</el-descriptions-item>
                </el-descriptions>
                <div class="mt-2 text-center">
                    <el-button plain  @click="logout">注销</el-button>
                </div>
                
        </template>
    </el-popover>
</template>



<script setup lang="ts">
import avatar from "../../assets/avatar.png";
import { globalConfig } from "../../libs";
import { ref } from "vue";

const user = ref(globalConfig.currentUser);

const logout = () => {
    globalConfig.logout();
};
</script>

