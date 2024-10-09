<template>
    <el-container style="height: 100vh;">
        <el-container>
            <el-aside :width="collapse ? '60px' : '200px'" class="aside">
                <el-menu router :collapse="collapse" :default-active="$route.path">
                    <el-menu-item index="/">
                        <el-icon>
                            <font-awesome-icon icon="home" />
                        </el-icon>
                        <span v-if="!collapse">首页</span>
                    </el-menu-item>
                    <el-menu-item index="/patient-infos" v-if="isGranted('PointToPoint', 'PatientInfos')">
                        <el-icon>
                            <font-awesome-icon icon="users" />
                        </el-icon>
                        <span v-if="!collapse">患者信息</span>
                    </el-menu-item>
                    <el-menu-item index="/medical-histories" v-if="isGranted('PointToPoint', 'MedicalHistories')">
                        <el-icon>
                            <font-awesome-icon icon="capsules" />
                        </el-icon>
                        <span v-if="!collapse">用药情况</span>
                    </el-menu-item>
                    <el-sub-menu index="4">
                        <template #title>
                            <el-icon><font-awesome-icon icon="cogs" /></el-icon>
                            <span v-if="!collapse">系统管理</span>
                        </template>
                        <el-menu-item index="/users" v-if="isGranted('AbpIdentity', 'Users')">
                            <el-icon><font-awesome-icon icon="user" /></el-icon>
                            用户管理
                        </el-menu-item>
                        <el-menu-item index="/roles" v-if="isGranted('AbpIdentity', 'Roles')">
                            <el-icon><font-awesome-icon icon="lock" /></el-icon>
                            角色管理
                        </el-menu-item>
                        <el-menu-item index="/sites" v-if="isGranted('PointToPoint', 'Sites')">
                            <el-icon><font-awesome-icon icon="location-pin" /></el-icon>
                            站点管理
                        </el-menu-item>
                    </el-sub-menu>
                </el-menu>
            </el-aside>
            <el-main>
                <router-view />
            </el-main>
        </el-container>
    </el-container>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { isGranted } from '../libs';
import UserButton from '../components/buttons/UserButton.vue';
const collapse = ref(false);

const toggleCollapse = () => {
    collapse.value = !collapse.value;
};




</script>

<style scoped>

.aside {
    transition: width 0.3s;
    /* 添加平滑过渡效果 */
}

.aside .el-menu {
    min-height: calc(100vh - 60px);
}

/* 设置文本的过渡效果 */
.logo-text {
    opacity: 1;
    height: auto;
    overflow: hidden;
    /* 隐藏溢出文本 */
    white-space: nowrap;
    /* 防止换行 */
    transition: opacity 0.3s ease, height 0.3s ease;
    /* 添加不透明度和高度的过渡效果 */
}

.logo-wrap.collapse .logo-text {
    opacity: 0;
    /* 折叠时隐藏文本 */
    height: 0;
    /* 设置高度为0 */
}
</style>
